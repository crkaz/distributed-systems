using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lab5.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TranslateController : ControllerBase
    {
        // GET: api/Translate
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Translate/GetInt/5
        [HttpGet("{id:int}", Name = "GetInt")]
        public string GetInt(int id)
        {
            string value = id.ToString();
            return value;
        }

        // GET: api/Translate/GetString/Zak
        [HttpGet("{input}")]
        public string GetString(string input)
        {
            return input + "eth";
        }

        // GET: api/Translate/GetName/Zak
        [HttpGet("{name}")]
        [ActionName("GetName")]
        public string GetName(string name)
        {
            return name;
        }


        // GET: api/Translate/GetLocation?location=Hull
        [HttpGet]
        [ActionName("GetLocation")]
        public string Get([FromQuery]string location)
        {
            return "You are at " + location;
        }

        // GET: api/Translate/GetString/Zak
        [HttpGet("{input}")]
        public ActionResult<string> GetActionResult(string input)
        {
            if (input == "bad")
            {
                return BadRequest();
            }
            else if (input == null)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }

        // POST: api/Translate
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Translate/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
