using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WorkInApi.DAL;
using WorkInApi.MachineLearning;
using WorkInApi.Models;
using WorkInApi.Services;

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
            try
            {
                var id = Guid.NewGuid().ToString();
                employeurIdentite.Id = id;
                var collection = new EntrepriseCollection();
                var exist = collection.GetItems(e => e.EmployeurIdentite.Email == employeurIdentite.Email).FirstOrDefault();
                if (exist == null)
                {
                    Helpers.EmailHelper.SendEmails(subject: "Confirmation de compte",
                        $"Merci d'avoir créer votre compte chez nous, Veuillez confirmer en suivant ce liens ${Services.HostConfig.Host}/api/entreprise/{id}/confirm"
                        , emails: employeurIdentite.Email);
                    new EntrepriseCollection().NewItems(
                    new Entreprise
                    {
                        Id = id,
                        EmployeurIdentite = employeurIdentite,
                        Propositions = new List<Offre>(),
                        Publicites = new List<Publicite>()
                    });

                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error : L'utilisateur existe deja ");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error : {e.Message} ");
            }
           
        }
        [EnableCors("CorsPolicy")]
        [HttpPost("{id}/update/{type}")]
        public ActionResult<string> UpdateEntreprise(string id, int type ,[FromBody]EmployeurIdentite employeur)
        {
            try
            {
                string dataResult = "";
                var entrepriseCollection = new EntrepriseCollection();
                PublicationCollection publicationCollection = new PublicationCollection();
                var employeurIdentite = entrepriseCollection.GetItems(e => e.Id == id).FirstOrDefault();
                if (type == 1)
                {
                    employeurIdentite.EmployeurIdentite.Nom = employeur.Nom;
                    employeurIdentite.EmployeurIdentite.Adresse = employeur.Adresse;
                    employeurIdentite.EmployeurIdentite.IdNational = employeur.IdNational;
                    entrepriseCollection.UpdateItem(id, employeurIdentite);
                    dataResult = "Identité modifié avec succès";
                }
                else if (type == 2)
                {
                    employeurIdentite.EmployeurIdentite.MotDePasse = employeur.MotDePasse;
                    entrepriseCollection.UpdateItem(id, employeurIdentite);
                    dataResult = "Mot de passe modifié avec succès";
                }
                else if (type == 3)
                {
                    employeurIdentite.EmployeurIdentite.AboutEntreprise = employeur.AboutEntreprise;
                    employeurIdentite.EmployeurIdentite.Domaines = employeur.Domaines;
                    entrepriseCollection.UpdateItem(id, employeurIdentite);
                    dataResult = "Les details modifié avec succès";
                }
                else
                    throw new Exception("Modifications non pris en charge");
                return StatusCode(200, dataResult);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("{id}/confirm")]
        public ActionResult<string> ConfirmerLeCompte(string id)
        {
            try
            {
                var entrepriseCollection = new EntrepriseCollection();
                var entreprise = entrepriseCollection.GetItems(e => e.Id == id).FirstOrDefault();
                if (entreprise == null)
                    throw new Exception("Erreur: Utilisateur non trouvé");
                else
                {
                    entreprise.EmployeurIdentite.IsActive = true;
                    entrepriseCollection.UpdateItem(id, entreprise);
                    return StatusCode(200, "Compte confirmé");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erreur: {e.Message}");
            }
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("propositions/page/{page}")]
        public ActionResult<IEnumerable<Proposition>> Getpropositions(int page)
        {
            try
            {
                var PropositionCollection = new PropositionCollection();
                var Propositions = PropositionCollection.GetItems(p => p.Offre.DeadLine >= DateTime.Now || p.Offre.MaxParticipant > p.DemandeurIdentites.Count());
                if (Propositions.Count() < page)
                    return StatusCode(200, new List<Proposition>());
                var returnProp = new List<Proposition>();
                for (int i = 0; i < Propositions.Count(); i++)
                    if (i <= page * 5 && i >= (page * 5) - 5)
                        returnProp.Add(Propositions.ToList()[i]);
                return StatusCode(200, returnProp);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
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
                                                    signingCredentials: tokencredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new
            {
                token = tokenString,
                entreprise = entreprise.EmployeurIdentite

            });
        }
        [EnableCors("CorsPolicy")]
        [HttpPost("{id}/offres/{idOffre}/postuler")]
        public ActionResult Postuler(string id, string idOffre, [FromBody]string jsondata)
        {
            return StatusCode(200);
        }

        ///////////////////////// revenir tester l'existance de l'offre
        [EnableCors("CorsPolicy")]
        [HttpGet("{id}/propositions/{idProposition}")]
        public ActionResult<Proposition> GetProposition(string id, string idProposition)
        {
            try
            {
                var entrepriseCollection = new EntrepriseCollection();
                var propositionCollection = new PropositionCollection();
                var user = entrepriseCollection.GetItems((u) => u.Id == id ).FirstOrDefault();
              
                if (user == null)
                    throw new Exception("Internal Server Error: Entreprise not found");
                if (user.Propositions == null)
                    throw new Exception("Internal Server Error: Aucune proposition");
                var propositionExist = user.Propositions.ToList().Select(p => p.Id).Contains(idProposition);
                if (propositionExist == false)
                    throw new Exception("Internal Server Error: Proposition not found");
                else
                {
                    var proposition = propositionCollection.GetItems(p => p.Id == idProposition).FirstOrDefault();
                    if (proposition == null)
                        throw new Exception("Internal Server Error: Proposition not found");
                    if (proposition.DemandeurIdentites == null)
                        proposition.DemandeurIdentites = new List<DemandeurIdentite>();
                    return Ok(proposition);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("{id}/dataprofil")]
        public ActionResult<(IEnumerable<Proposition>,IEnumerable<Publicite>)> GetPubliciteAndPropositions(string id)
        {
            try
            {
                var entrepriseCollection = new EntrepriseCollection();
                var entreprise = entrepriseCollection.GetItems(e => e.Id == id).FirstOrDefault();
                if (entreprise == null)
                    throw new Exception("Entreprise not found");
                var publicites = entreprise.Publicites.ToList();
                var idPropositions = entreprise.Propositions.Select(p => p.Id).ToList();
                var propositionCollection = new PropositionCollection();
                var propositions = propositionCollection.GetAllItem().Where(p => idPropositions.Contains(p.Id)).ToList();
                return Ok(new
                {
                    propositions= propositions,
                    publicites= publicites
                });
            }
            catch (Exception e)
            {
                return StatusCode(200, e.Message);
            }
            
        }


        [EnableCors("CorsPolicy")]
        [HttpPost("commenter/{publicationId}")]
        public ActionResult<double> Commenter(string publicationId, [FromBody]CommentaireSend commentaire)
        {
            try
            {
                var id = Guid.NewGuid().ToString();
                commentaire.Id = id;
                var com = commentaire.Commentaire;
                com.Id = id;
                var CommentaireML = new PredictedModels.PredictedCommentaire(com);
                CommentaireML = CommentaireML.AttribScoreAndTypeOfComment();
                var commentf = CommentaireML;
                var publicationCollection = new PublicationCollection();
                var publication = publicationCollection.GetItems(p1 => p1.Id == publicationId).FirstOrDefault();
                if (publication == null)
                    throw new Exception("Publication not found");
                ((List<CommentaireSend>)(publication.Commentaires)).Add(commentaire);
                var p = publication;
                publicationCollection.UpdateItem(publication.Id, publication);
                return StatusCode(200,commentaire);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
            
        }



        [EnableCors("CorsPolicy")]
        [HttpPost("prediction")]
        public ActionResult<IEnumerable<(double,DemandeurIdentite)>> Prediction([FromBody]string[] competences)
        {
            try
            {
                var demandeurCollection = new UserCollection();
                var demandeurs = demandeurCollection.GetAllItem().ToList();
                demandeurs = demandeurs.Where(d => d.Competances.Intersect(competences).Count() != 0).ToList();
                List<(double, DemandeurIdentite)> dataFinal = new List<(double, DemandeurIdentite)>();
                foreach (var d in demandeurs)
                {
                    dataFinal.Add(MachineLearning.Prediction.AttribScoreForOneCandidat(d));
                }
                return dataFinal.OrderByDescending(d => d.Item1).Take(4).ToList();
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
            
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

        [EnableCors("CorsPolicy")]
        [HttpPost("{id}/upload/type/{type}"), DisableRequestSizeLimit]
        public ActionResult<string> UploadFile(string id, string type)
        {
            try
            {
                if (type != "profil")
                    throw new Exception("File type not supported for this URL");
                var file = Request.Form.Files[0];
                var ext = file.FileName.Split('.').Last();
                var entrepriseCollection = new EntrepriseCollection();
                var entreprise = entrepriseCollection.GetItems(e => e.Id == id).FirstOrDefault();
                if (entreprise == null)
                    throw new Exception("Entreprise not found");
                StorageAzureManager storage = new StorageAzureManager("profilsentreprises");
                var path = storage.UpladFile($"entreprises/{id}.{ext}", file.OpenReadStream(), file.ContentType).GetAwaiter().GetResult();
                entreprise.EmployeurIdentite.ImageProfil = path;
                entrepriseCollection.UpdateItem(id, entreprise);
                //var stream = new Stream(file)
                return Ok(path);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            
        }
    }
}
