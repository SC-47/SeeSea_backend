//-----------------------------------------------------------------------
// <copyright file="DivingPointController.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GroupPairing_API.Dtos;
    using GroupPairing_API.Models.Db;
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
        public readonly SeeSeaTestContext SeeSeaTestContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DivingPointController" /> class.
        /// </summary>
        /// <param name="seeSeaTestContext">The Algorithmic logic of the data about GroupPairing_API.</param>
        public DivingPointController(SeeSeaTestContext seeSeaTestContext)
        {
            this.SeeSeaTestContext = seeSeaTestContext;
        }

        // GET: api/<DivingPointController>

        /// <summary>
        /// Get the information of all the diving points in the database named SeeSeaTest.
        /// </summary>
        /// <returns>If the information exist, return the data.</returns>
        [HttpGet]
        public ActionResult Get()
        {
            //取得所有潛點資訊，抓至記憶體中
            var divingPoints = this.SeeSeaTestContext.DivingPoints.AsEnumerable();

            //將潛點資訊轉換成DTO型式給前端
            var divingPointDtos = divingPoints
                .Select(
                    divingPoint => new DivingPointDto
                    {
                        DivingPointId = divingPoint.DivingPointId,
                        DivingPointName = divingPoint.DivingPointName,
                        Latitude = divingPoint.Latitude,
                        Longitude = divingPoint.Longitude,
                        DivingPlaceDescription = divingPoint.DivingPlaceDescription ?? string.Empty,
                        AreaTag = Global.GetDescription((Global.ActivityArea)Enum.ToObject(typeof(Global.ActivityArea), divingPoint.AreaTag)),
                        DivingTypeTag = this.ConvertDivingTypeTagToDivingTypeList(divingPoint.DivingTypeTag),
                        DivingDifficulty = Global.GetDescription((Global.DivingDifficulty)Enum.ToObject(typeof(Global.DivingDifficulty), divingPoint.DivingDifficultyCode)),
                        DivingPointPicture = divingPoint.DivingPointPicture,
                        DivingPointActivityNumber = this.SeeSeaTestContext.ActivityRooms.Where(room => room.ActivityPlaceCode == divingPoint.DivingPointId && (room.ActivityStatusCode == (int)Global.ActivityStatus.PAIRING || room.ActivityStatusCode == (int)Global.ActivityStatus.FULL)).Count()
                    });

            return this.Ok(divingPointDtos);
        }

        /// <summary>
        /// Convert the string code of the diving type to the exactly diving type to the frontend.
        /// </summary>
        /// <param name="divingTypeTag">The inputting string code of diving type tag.</param>
        /// <returns>The showing string to the frontend.</returns>
        private List<string> ConvertDivingTypeTagToDivingTypeList(string divingTypeTag)
        {
            //將輸入字串轉換成INT串列
            List<int> divingCodeList = Global.ConvertStringToIntList(divingTypeTag);

            //初始化輸出字串串列
            List<string> output = new List<string>();

            //取INT串列的數值，轉換成對應diving type 字串
            foreach (int divingCode in divingCodeList)
            {
                output.Add(Global.GetDescription((Global.DivingType)Enum.ToObject(typeof(Global.DivingType), divingCode)));
            }

            return output;
        }
    }
}
