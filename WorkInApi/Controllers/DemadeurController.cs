using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorkInApi.MachineLearning;
using WorkInApi.DAL;
using WorkInApi.Models;
using WorkInApi.Services;

namespace WorkInApi.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json", "application/json-patch+json", "multipart/form-data")]
    public class DemadeurController : ControllerBase
    {
        // GET: api/Demadeur
        [HttpGet]
        public IEnumerable<Demandeur> Get()
        {
            return new UserCollection().GetAllItem();
        }

        // GET: api/Demadeur/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Demandeur> Get(string id)
        {
            UserCollection userCollection = new UserCollection();
            return userCollection.GetItems((d) => d.Id == id).FirstOrDefault();
        }

        // POST: api/Demadeur
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value)
        {
            return StatusCode(200);
        }

        // PUT: api/Demadeur/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpPost("connexion")]
        public ActionResult<DemandeurIdentite> Connexion([FromBody]string jsondata)
        {
            UserCollection userCollection = new UserCollection();
            var user = JsonConvert.DeserializeObject<DemandeurIdentite>(jsondata); 
            var resut= userCollection.GetItems((d) => d.Identite.Email == user.Email && d.Identite.Password==user.Password).FirstOrDefault();
            if (resut == null)
                return StatusCode(500, "Internal Server Error: Verifier les information de connexion");
            else
                return resut.Identite;
        }
        [HttpGet("{id}/publications")]
        public ActionResult<IEnumerable<Publication>> GetUserPublication(string id)
        {
            UserCollection userCollection = new UserCollection();
            var user = userCollection.GetItems(u => u.Id == id).FirstOrDefault();
            if (user == null)
                return StatusCode(500, "Internal Server Error : Utilisateur inexistant");
            else
                if (user.Publications == null)
                    return StatusCode(200, new List<Publication>());
                else
                    return StatusCode(200, user.Publications);
            
        }
        //[HttpPost("{id}/publications/nouvel")]
        //public ActionResult NewPublication(string id,[FromBody]string jsondata)
        //{
        //    var publication = JsonConvert.DeserializeObject<Publication>(jsondata);
        //    var user = new UserCollection().GetItems(u => u.Id == id).FirstOrDefault();
        //    if (user == null)
        //        return StatusCode(500, "Internal Server Error: Utilisateur inexistant");
        //    else
        //    {
        //        if (user.Publications == null)
        //            user.Publications = new List<Publication>
        //            {
        //                publication
        //            };
        //        else
        //            user.Publications.ToList().Add(publication);
        //        new UserCollection().UpdateItem(id, user);
        //        return StatusCode(200, "Ajout effectué avec succès");
        //    }
        //}
        [HttpGet("{id}/ranking")]
        public ActionResult<int> GetRanking(string id)
        {
            var user = new UserCollection().GetItems(u => u.Id == id).FirstOrDefault();
            if (user == null)
                return StatusCode(500, "Internal Server Error: Utilisateur inexistant");
            else
            {
                try
                {
                    var publications = user.Publications;
                    double? ranking = 0.0;
                    foreach (var pub in publications)
                    {
                        if (pub.Commentaires == null)
                            throw new Exception();
                        foreach(var c in pub.Commentaires)
                        {
                            var recup = new PredictedModels.PredictedCommentaire(c);
                            ranking += recup.AttribScoreAndTypeOfComment().Score;
                        }
                    }
                    return StatusCode(200, ranking);
                }
                catch (Exception e)
                {
                    return StatusCode(500, 0);
                }
            }
        }

        [HttpPost("inscription")]
        public ActionResult NouveauDemandeur( [FromBody]string jsondata)
        {
            var demandeur = JsonConvert.DeserializeObject<DemandeurIdentite>(jsondata);
            UserCollection userCollection = new UserCollection();
            var resut = userCollection.GetItems((d) => d.Identite.Email == demandeur.Email).FirstOrDefault();
            if (resut == null)
            {   
                userCollection.NewItems(new Demandeur
                {
                    Id=demandeur.Id,
                    Identite = demandeur
                });
                return StatusCode(200, "Success: Compte Utilisateur crée avec succes");
            }
            else
                return StatusCode(500, "Internal Server Error: Compte Utilisateur déjà existant");
        }
        [HttpPost("upload/{fileType}")]
        public ActionResult<string> UploadFile(string fileType,IFormFile file)
        {
            StorageAzureManager storage = new StorageAzureManager(fileType);
            var path = storage.UpladFile(file.FileName, file.OpenReadStream(), file.ContentType).GetAwaiter().GetResult();
            return path;
        }

        [HttpPost("{id}/publications/new")]
        public ActionResult NewPublication(string id,[FromBody]string jsondata)
        {
            UserCollection userCollection = new UserCollection();
            var user = userCollection.GetItems((d) => d.Id == id).FirstOrDefault();
            if (user != null)
            {
                var publicationDetail = JsonConvert.DeserializeObject<PublicationDetails>(jsondata);
                var publication = new Publication
                {
                    Id = publicationDetail.Id,
                    PublicationDetails = publicationDetail,
                    Commentaires = null,
                    Likes = null
                };
                if (user.Publications == null)
                    user.Publications = new List<Publication>();
                user.Publications.ToList().Add(publication);
                userCollection.UpdateItem(user.Id, user);
                return StatusCode(200);
            }
            else
            {
                return StatusCode(500, "User Collection Error: User not found");
            }
        }

        [HttpPut("update")]
        public ActionResult UpdateDemandeur([FromBody]string jsondata)
        {
            var demandeurIdentite = JsonConvert.DeserializeObject<DemandeurIdentite>(jsondata);
            UserCollection userCollection = new UserCollection();
            var resut = userCollection.GetItems((d) => d.Id == demandeurIdentite.Id).FirstOrDefault();
            var test = userCollection.GetItems(d => d.Identite.Username == demandeurIdentite.Username).FirstOrDefault();
            if (resut != null)
                if (test != null)
                    if (test.Id != resut.Id)
                        return StatusCode(500, "Internal Server Error: Ce nom d'utilisateur existe déjà");
            if (resut == null) {
                resut.Identite = demandeurIdentite;
                userCollection.UpdateItem(demandeurIdentite.Id, resut);
                return StatusCode(200, "Mofification reussi avec success!!");
            }
            else
                return StatusCode(500, "Internal Server Error: Le compte utilisateur doit d'abord exister avant toute modification");
        }
        
    }
}
