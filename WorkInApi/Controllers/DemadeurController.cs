using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkInApi.DAL;
using WorkInApi.Models;

namespace WorkInApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IEnumerable<Demandeur> Get(string id)
        {
            UserCollection userCollection = new UserCollection();
            return userCollection.GetItems((d) => d.Id == id);
        }

        // POST: api/Demadeur
        [HttpPost]
        public void Post([FromBody] DemandeurIdentite value)
        {
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
        [HttpPost]
        public IEnumerable<Demandeur> Connexion([FromBody]string email,string password)
        {
            UserCollection userCollection = new UserCollection();
            return userCollection.GetItems((d) => d.Identite.Email == email && d.Identite.Password==password);
        }
        [HttpPost]
        public void NouveauDemandeur([FromBody]DemandeurIdentite demandeur)
        {
            UserCollection userCollection = new UserCollection();
            userCollection.NewItems(new Demandeur
            {
                Identite = demandeur
            });
        }
        [HttpPut]
        public void UpdateDemandeur([FromBody]Demandeur demandeur)
        {
            UserCollection userCollection = new UserCollection();
            userCollection.UpdateItem(demandeur.Identite.Id,demandeur);
        }
    }
}
