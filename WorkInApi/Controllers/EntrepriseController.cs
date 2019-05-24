using System;
using System.Collections.Generic;
using System.IO;
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
    public class EntrepriseController : ControllerBase
    {
        // GET: api/Entreprise
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Entreprise/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Entreprise
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Entreprise/5
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
        public void NouvelEntreprise([FromBody]EmployeurIdentite employeurIdentite)
        {
            var id = Guid.NewGuid().ToString();
            employeurIdentite.Id = id;
            new EntrepriseCollection().NewItems(
                new Entreprise {
                    Id=id,
                    EmployeurIdentite = employeurIdentite
            }); 
        }
        [HttpPut]
        public void ModifierEntreprise([FromBody]Entreprise entreprise)
        {
            new EntrepriseCollection().UpdateItem(entreprise.Id, entreprise);
        }
        [HttpDelete]
        public void DeleteEntreprise([FromBody]Entreprise entreprise)
        {

        }
        [HttpGet]
        public ActionResult<Entreprise> Connection([FromBody]string email,[FromBody]string password)
        {
            var entreprise = new EntrepriseCollection().GetItems(
                (e) => e.EmployeurIdentite.Email == email && e.EmployeurIdentite.MotDePasse == password).FirstOrDefault();
            if (entreprise == null)
                return StatusCode(500, "Internal Server Error, Entreprise Not Found");
            return entreprise;
        }
        [HttpPost,DisableRequestSizeLimit]
        public void UploadFile()
        {
        }
    }
}
