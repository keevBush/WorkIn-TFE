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
using Microsoft.AspNetCore.Cors;

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
        [EnableCors("CorsPolicy")]
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Demandeur> Get(string id)
        {
            UserCollection userCollection = new UserCollection();
            var user = userCollection.GetItems((d) => d.Id == id).FirstOrDefault();
            if (user != null)
                return StatusCode(200, user);
            else
                return StatusCode(500, "Internal Server Error: Demandeur not found");
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

        [EnableCors("CorsPolicy")]
        [HttpGet("{id}/confirm")]
        public ActionResult<string> ConfirmerLeCompte(string id)
        {
            try
            {
                var demandeurCollection = new UserCollection();
                var entreprise = demandeurCollection.GetItems(e => e.Id == id).FirstOrDefault();
                if (entreprise == null)
                    throw new Exception("Erreur: Utilisateur non trouvé");
                else
                {
                    entreprise.Identite.IsVerified = true;
                    demandeurCollection.UpdateItem(id, entreprise);
                    return StatusCode(200, "Compte confirmé");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erreur: {e.Message}");
            }
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("{id}/publications")]
        public ActionResult<IEnumerable<Publication>> GetPublications(string id)
        {
            try
            {
                var publicationCollection = new PublicationCollection();
                var publication = publicationCollection.GetItems(p => p.Demandeur.Id == id).ToList();
                return publication;
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }

        }

        [EnableCors("CorsPolicy")]
        [HttpGet("{id}/ranking")]
        public ActionResult<double> GetRanking(string id)
        {
            try
            {
                var demandeur = new UserCollection().GetItems(u => u.Id == id).FirstOrDefault();
                if (demandeur == null)
                    throw new Exception("User not found");

                var userRanking = MachineLearning.Prediction.AttribScoreForOneCandidat(demandeur);
                return userRanking.Item1;
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
            
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
        [EnableCors("CorsPolicy")]
        [HttpGet("publications/page/{page}")]
        public ActionResult<IEnumerable<Publication>> GetPublication(int page)
        {
            try
            {
                PublicationCollection publicationCollection = new PublicationCollection();
                var publications = publicationCollection.GetAllItem().OrderByDescending(p => p.PublicationDetails.Date).ToList();
                if (publications.Count <= page)
                    return StatusCode(200, new List<Publication>());
                var returnPubs = new List<Publication>();
                for (int i = 0; i < publications.Count; i++)
                    if (i <= page * 5 && i >= (page * 5) - 5)
                        returnPubs.Add(publications[i]);
                return StatusCode(200, returnPubs);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
           
        }

        public ActionResult NewNotification(string ids,[FromBody]Notification notification)
        {
            return Ok("cest bon");
        }
        [HttpGet("{id}/notifications")]
        public ActionResult<IEnumerable<Notification>> GetNotifications(string id)
        {
            try
            {
                var userCollection = new UserCollection();
                var demandeur = userCollection.GetItems(u => u.Id == id).FirstOrDefault();
                if (demandeur == null)
                    throw new Exception("User not found");
                else
                    return StatusCode(200, demandeur.Notifications);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
           
        }


        [EnableCors("CorsPolicy")]
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
        [EnableCors("CorsPolicy")]
        [HttpGet("publications/{idPub}/like/{idEntreprise}")]
        public ActionResult<bool> Like(string idEntreprise,string idPub)
        {
            try
            {
                PublicationCollection publicationCollection = new PublicationCollection();
                var publication = publicationCollection.GetItems(p => p.Id == idPub).FirstOrDefault();
                if (publication == null)
                    throw new Exception($"Publication not found");
                var like = publication.Likes.Where(l => l.Id == idEntreprise).FirstOrDefault();
                if(like == null)
                {
                    
                    ((List<Like>)(publication.Likes)).Add(new Models.Like
                    {
                        Id = idEntreprise,
                        Etat = true
                    });
                    publicationCollection.UpdateItem(idPub, publication);
                    return StatusCode(200, true);
                }
                else
                {
                    if(like.Etat == false)
                    {
                        publication.Likes.Where(p => p.Id == idEntreprise).FirstOrDefault().Etat = true;
                        var p2 = publication;
                        publicationCollection.UpdateItem(idPub, publication);
                        return StatusCode(200, true);
                    }
                    else
                    {
                        publication.Likes.Where(p => p.Id == idEntreprise).FirstOrDefault().Etat = false;
                        var p2 = publication;
                        publicationCollection.UpdateItem(idPub, publication);
                        return StatusCode(200, false);
                    }
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
           
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

        [HttpPost("inscription")]
        public ActionResult NouveauDemandeur([FromBody]string jsondata)
        {
            try
            {
                var demandeur = JsonConvert.DeserializeObject<DemandeurIdentite>(jsondata);
                UserCollection userCollection = new UserCollection();
                var resut = userCollection.GetItems((d) => d.Identite.Email == demandeur.Email).FirstOrDefault();
                if (resut == null)
                {
                    userCollection.NewItems(new Demandeur
                    {
                        Id = demandeur.Id,
                        Identite = demandeur
                    });
                    Helpers.EmailHelper.SendEmails(subject: "Confirmation de compte",
                        $"Merci d'avoir créer votre compte chez nous, Veuillez confirmer en suivant ce liens http://localhost:5002/api/entreprise/{demandeur.Id}/confirm"
                        , emails: demandeur.Email);
                    return StatusCode(200, "Success: Compte Utilisateur crée avec succes");
                }
                else
                    return StatusCode(500, "Internal Server Error: Compte Utilisateur déjà existant");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
            
        }
        [HttpPost("upload/{fileType}")]
        public ActionResult<string> UploadFile(string fileType,IFormFile file)
        {
            StorageAzureManager storage = new StorageAzureManager(fileType);
            var path = storage.UpladFile(file.FileName, file.OpenReadStream(), file.ContentType).GetAwaiter().GetResult();
            return path;
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("publications/{id}")]
        public ActionResult<object> GetPublication(string id)
        {
            try
            {
                PublicationCollection publicationCollection = new PublicationCollection();
                var publication = publicationCollection.GetItems(p => p.Id == id).FirstOrDefault();
                if (publication == null)
                    throw new Exception("Publication not found");
                return StatusCode(200, publication);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
           
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
                    Commentaires = new List<CommentaireSend>(),
                    Likes = new List<Like>()
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
            if (resut != null) {
                resut.Identite = demandeurIdentite;
                userCollection.UpdateItem(demandeurIdentite.Id, resut);
                return StatusCode(200, "Mofification reussi avec success!!");
            }
            else
                return StatusCode(500, "Internal Server Error: Le compte utilisateur doit d'abord exister avant toute modification");
        }
        
    }
}
