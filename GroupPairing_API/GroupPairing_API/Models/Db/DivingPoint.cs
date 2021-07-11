using System;
using System.Collections.Generic;

#nullable disable

namespace GroupPairing_API.Models.Db
{
    public partial class DivingPoint
    {
        public int DivingPointId { get; set; }
        public string DivingPointName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string DivingPlaceDescription { get; set; }
        public byte? AreaTag { get; set; }
        public string DivingTypeTag { get; set; }
        public byte? DivingDifficultyCode { get; set; }
        public string DivingPointPicture { get; set; }
    }
}
