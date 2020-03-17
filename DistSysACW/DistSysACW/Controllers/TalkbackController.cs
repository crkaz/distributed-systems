using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    public class TalkBackController : BaseController
    {
        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public TalkBackController(Models.UserContext context) : base(context) { }


        [ActionName("Hello")]
        public string Get()
        {
            #region TASK1
            // TODO: add api/talkback/hello response

            // Should return "Hello World" in the body of the result with a status code of OK (200)
            #endregion

            new OkResult(); // Necessary?
            return "Hello World";
        }


        //https://localhost:44307/api/talkback/sort?integers[0]=1&integers[1]=3&integers[2]=2
        [ActionName("Sort")]
        public IActionResult Get([FromQuery]int[] integers)
        {
            #region TASK1
            // TODO: 
            // sort the integers into ascending order
            // send the integers back as the api/talkback/sort response

            //Should return [2,5,8] in the body of the result with a status code of OK (200)
            //If there are no integers submitted, the result should be[] with a status code of OK(200)
            //If the submitted integers are invalid(e.g.a char is submitted) the server should return with a with a status code of BAD REQUEST(400)
            #endregion

            //return BadRequest("ERR 400: Bad Request\nAn array of integers was not provided.");
            // Automatically returns code400 if bad request: do we need to alter?
            Array.Sort(integers);
            return Ok(integers);
        }
    }
}
