using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public ActionResult<EmployeurIdentite> Connexion([FromBody]EmployeurIdentite identite)
        {
            var entreprise = new EntrepriseCollection().GetItems(
                (e) => e.EmployeurIdentite.Email == identite.Email && e.EmployeurIdentite.MotDePasse == identite.MotDePasse).FirstOrDefault();
            if (entreprise == null)
                return StatusCode(500, "Internal Server Error, Entreprise Not Found");
            var token = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("userkeyBush@243789"));
            var tokencredentials = new SigningCredentials(token, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(issuer: "http://localhost:5002/api", 
                                                    audience: "http://localhost:5002/api", 
                                                    claims: new List<Claim>(), expires: DateTime.Now.AddDays(1), 
                                                    signingCredentials:tokencredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new
            {
                token=tokenString,
                entreprise = entreprise.EmployeurIdentite

            });
        }
        [HttpPost("{id}/offres/{idOffre}/postuler")]
        public ActionResult Postuler(string id, string idOffre,[FromBody]string jsondata)
        {
            return StatusCode(200);
        }
        [EnableCors("CorsPolicy")]
        [HttpPost("{id}/propositions/new")]
        public ActionResult NewProposition(string id,[FromBody]Offre offre)
        {
            try
            {
                var proposition = new Proposition
                {
                    Id = offre.Id,
                    Offre = offre
                };
                var entrepriseCollection = new EntrepriseCollection();
                var entreprise = entrepriseCollection.GetItems(e => e.Id == id).FirstOrDefault();
                if (entreprise == null)
                    return StatusCode(500, "Internal Server Error: Entreprise not found");
                if (entreprise.Propositions == null)
                {
                    entreprise.Propositions = new List<Offre>();
                }
                ((List<Offre>)(entreprise.Propositions)).Add(offre);
                entrepriseCollection.UpdateItem(entreprise.Id, entreprise);
                var propositionCollection = new PropositionCollection();
                propositionCollection.NewItems(proposition);
                return Ok("Propositions posté avec success");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            
        }

        [HttpPost,DisableRequestSizeLimit]
        public void UploadFile()
        {
        }
    }
}
