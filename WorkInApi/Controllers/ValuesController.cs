using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorkInApi.MachineLearning;
using WorkInApi.Models;

namespace WorkInApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public static string Hash(string data)
        {
            byte[] salt= new byte[16];
            var h = new Rfc2898DeriveBytes("123456", salt, 10000);
            var hashingBytes = h.GetBytes(20);

            return Convert.ToBase64String (hashingBytes);
        } 
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<DemandeurIdentite>> Get()
        {
            return new DemandeurIdentite[] { new DemandeurIdentite{
                Id=Guid.NewGuid().ToString(),
                Email="email1"
                },
                new DemandeurIdentite{
                Id=Guid.NewGuid().ToString(),
                Email="email2"
                },
                new DemandeurIdentite{
                Id=Guid.NewGuid().ToString(),
                Email="email3"
                }
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //return null;
            var comment = new PredictedModels.PredictedCommentaire()
            {
                Value = "Je déteste ce projet"
            };
            var data= comment.AttribScoreAndTypeOfComment();
            return JsonConvert.SerializeObject(data);
        }

        // POST api/values
        [HttpPost]
        public void Post()
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
