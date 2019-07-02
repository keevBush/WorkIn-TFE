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
        public ActionResult<Entreprise> Connection([FromBody]string email,string password)
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
