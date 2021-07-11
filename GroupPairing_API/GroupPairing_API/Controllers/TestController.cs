//-----------------------------------------------------------------------
// <copyright file="TestController.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The WebAPI controller for testing.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// The Algorithmic logic of the data about GroupPairing_API.
        /// </summary>
        private readonly RoomDataCenter DataCenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestController" /> class.
        /// </summary>
        /// <param name="dataCenter">The Algorithmic logic of the data about GroupPairing_API.</param>
        public TestController(RoomDataCenter dataCenter)
        {
            this.DataCenter = dataCenter;
        }

        // POST api/<TestController>

        /// <summary>
        /// Convert Email to EmailID.
        /// </summary>
        /// <param name="email">Inputting Email address.</param>
        /// <returns>Return encoded Email address.</returns>
        [HttpGet("test1")]
        public ActionResult ConvertEmailID(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return this.BadRequest("未輸入正確內容");
            }

            return this.Ok($"{DateTime.Now.ToString("yyyyMMddHHmm")}");
        }
    }
}
