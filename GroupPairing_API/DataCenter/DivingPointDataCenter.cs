using GroupPairing_API.Dtos;
using GroupPairing_API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GroupPairing_API.DataCenter
{
    /// <summary>
    /// The Algorithmic logic of the data about the local diving points.
    /// </summary>
    public class DivingPointDataCenter : IDivingPointDataCenter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DivingPointDataCenter" /> class.
        /// </summary>
        /// <param name="repository">All the information about the datasheets from the database named SeeSeaTest.</param>
        public DivingPointDataCenter(ISeeSeaTestRepository repository)
        {
            Repository = repository;
        }

        /// <summary>
        /// All the information about the datasheets from the database named SeeSeaTest.
        /// </summary>
        private ISeeSeaTestRepository Repository { get; }

        /// <summary>
        /// Get the DTO of all the information of diving points.
        /// </summary>
        /// <returns>The DTO of all the information of diving points.</returns>
        public IEnumerable<DivingPointDto> GetDivingPointDtoList()
        {
            // 取得所有潛點資訊，抓至記憶體中，並將潛點資訊轉換成DTO型式給前端
            var divingPointDtos = Repository
                .GetDivingPoints()
                .AsEnumerable()
                .Select(
                    divingPoint => new DivingPointDto
                    {
                        DivingPointId = divingPoint.DivingPointId,
                        DivingPointName = divingPoint.DivingPointName,
                        Latitude = divingPoint.Latitude,
                        Longitude = divingPoint.Longitude,
                        DivingPlaceDescription = divingPoint.DivingPlaceDescription ?? string.Empty,
                        AreaTag = Global.GetDescription((Global.ActivityArea)Enum.ToObject(typeof(Global.ActivityArea), divingPoint.AreaTag)),
                        DivingTypeTag = ConvertDivingTypeTagToDivingTypeList(divingPoint.DivingTypeTag),
                        DivingDifficulty = Global.GetDescription((Global.DivingDifficulty)Enum.ToObject(typeof(Global.DivingDifficulty), divingPoint.DivingDifficultyCode)),
                        DivingPointPicture = divingPoint.DivingPointPicture,
                        DivingPointActivityNumber = Repository.GetDivingPointActivityNumber(divingPoint.DivingPointId),
                    });

            return divingPointDtos;
        }


        /// <summary>
        /// Convert the string code of the diving type to the exactly diving type to the frontend.
        /// </summary>
        /// <param name="divingTypeTag">The inputting string code of diving type tag.</param>
        /// <returns>The showing string to the frontend.</returns>
        private static List<string> ConvertDivingTypeTagToDivingTypeList(string divingTypeTag)
        {
            //將輸入字串轉換成INT串列
            List<int> divingCodeList = Global.ConvertStringToIntList(divingTypeTag);

            //初始化輸出字串串列
            List<string> output = new();

            //取INT串列的數值，轉換成對應diving type 字串
            foreach (int divingCode in divingCodeList)
            {
                output.Add(Global.GetDescription((Global.DivingType)Enum.ToObject(typeof(Global.DivingType), divingCode)));
            }

            return output;
        }
    }
}