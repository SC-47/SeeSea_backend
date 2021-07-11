//-----------------------------------------------------------------------
// <copyright file="DivingPointDto.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Dtos
{
    using System.Collections.Generic;

    /// <summary>
    /// The DTO of DivingPoint.
    /// </summary>
    public partial class DivingPointDto
    {
        /// <summary>
        /// Gets or sets DivingPoint_API of the serial number of the diving point.
        /// </summary>
        public int DivingPointId { get; set; }

        /// <summary>
        /// Gets or sets DivingPoint_API of the name of the diving point.
        /// </summary>
        public string DivingPointName { get; set; }

        /// <summary>
        /// Gets or sets DivingPoint_API of the latitude of the diving point.
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Gets or sets DivingPoint_API of the longitude of the diving point.
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// Gets or sets DivingPoint_API of the description of the diving point.
        /// </summary>
        public string DivingPlaceDescription { get; set; }

        /// <summary>
        /// Gets or sets DivingPoint_API of the area tag of the diving point.
        /// </summary>
        public string AreaTag { get; set; }

        /// <summary>
        /// Gets or sets DivingPoint_API of the diving type tag of the diving point.
        /// </summary>
        public List<string> DivingTypeTag { get; set; }

        /// <summary>
        /// Gets or sets DivingPoint_API of the diving difficulty of the diving point.
        /// </summary>
        public string DivingDifficulty { get; set; }

        /// <summary>
        /// Gets or sets DivingPoint_API of the route of the picture of the diving point.
        /// </summary>
        public string DivingPointPicture { get; set; }

        /// <summary>
        /// Gets or sets DivingPoint_API of the current number of the activity.
        /// </summary>
        public int DivingPointActivityNumber { get; set; }
    }
}
