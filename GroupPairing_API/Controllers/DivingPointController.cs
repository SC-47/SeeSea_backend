//-----------------------------------------------------------------------
// <copyright file="DivingPointController.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Controllers
{
    using GroupPairing_API.Interface;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The WebAPI controller related to DivingPoint.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DivingPointController : ControllerBase
    {
        /// <summary>
        /// Gets the SeeSeaContext class.
        /// </summary>
        public readonly IDivingPointDataCenter DivingPointDataCenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DivingPointController" /> class.
        /// </summary>
        /// <param name="divingPointDataCenter">The Algorithmic logic of the data about the local diving points.</param>
        public DivingPointController(IDivingPointDataCenter divingPointDataCenter)
        {
            DivingPointDataCenter = divingPointDataCenter;
        }

        // GET: api/<DivingPointController>

        /// <summary>
        /// Get the information of all the diving points in the database named SeeSeaTest.
        /// </summary>
        /// <returns>If the information exist, return the data.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DivingPointDataCenter.GetDivingPointDtoList());
        }
    }
}