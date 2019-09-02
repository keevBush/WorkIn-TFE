﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
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
    [Consumes("application/json", "text/plain", "application/json-patch+json", "multipart/form-data")]
    public class EntrepriseController : ControllerBase
    {
        [EnableCors("CorsPolicy")]
        [HttpPost("inscription")]
        public ActionResult NouvelEntreprise([FromBody]EmployeurIdentite employeurIdentite)
        {
            var id = Guid.NewGuid().ToString();
            employeurIdentite.Id = id;
            var collection = new EntrepriseCollection();
            var exist = collection.GetItems(e => e.EmployeurIdentite.Email == employeurIdentite.Email).FirstOrDefault();
            if (exist == null)
            {
                new EntrepriseCollection().NewItems(
                new Entreprise
                {
                    Id = id,
                    EmployeurIdentite = employeurIdentite
                });
                return StatusCode(200);
            }
            else
            {
                return StatusCode(500, "Internal Server Error : L'utilisateur existe deja ");
            }
        }
        [HttpPut]
        public ActionResult ModifierEntreprise([FromBody]Entreprise entreprise)
        {
            new EntrepriseCollection().UpdateItem(entreprise.Id, entreprise);
            return StatusCode(200);
        }
        [HttpDelete]
        public void DeleteEntreprise([FromBody]Entreprise entreprise)
        {

        }
        [EnableCors("CorsPolicy")]
        [HttpPost("connexion")]
        public ActionResult<Entreprise> Connexion([FromBody]EmployeurIdentite identite)
        {
            var entreprise = new EntrepriseCollection().GetItems(
                (e) => e.EmployeurIdentite.Email == identite.Email && e.EmployeurIdentite.MotDePasse == identite.MotDePasse).FirstOrDefault();
            if (entreprise == null)
                return StatusCode(500, "Internal Server Error, Entreprise Not Found");
            return entreprise;
        }
        [HttpPost("{id}/offres/{idOffre}/postuler")]
        public ActionResult Postuler(string id, string idOffre,[FromBody]string jsondata)
        {
            return StatusCode(200);
        }

        [HttpPost,DisableRequestSizeLimit]
        public void UploadFile()
        {
        }
    }
}
