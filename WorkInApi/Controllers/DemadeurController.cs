using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorkInApi.DAL;
using WorkInApi.Models;

namespace WorkInApi.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
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
