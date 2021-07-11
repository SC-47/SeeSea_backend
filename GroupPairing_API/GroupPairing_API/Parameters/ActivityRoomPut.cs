//-----------------------------------------------------------------------
// <copyright file="ActivityRoomPut.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Parameters
{
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The input parameters of the PUT ActivityRoom API.
    /// </summary>
    public class ActivityRoomPut
    {
        /// <summary>
        /// Gets or sets GroupPairing_API of the name of the activity room.
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the picture of the activity room.
        /// </summary>
        public IFormFile ActivityPicture { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the corresponded code of the required diving level which the participant needed.
        /// </summary>
        public byte? DivingLevelCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the exact address of the activity taking place.
        /// </summary>
        public short? ActivityPlaceCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the corresponded code of the way that participant go to the activity's place.
        /// </summary>
        public byte? TransportationCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the description of the activity room.
        /// </summary>
        public string ActivityDescription { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the assembling place if needed.
        /// </summary>
        public string? MeetingPlace { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the corresponded code of the estimate cost range of this activity.
        /// </summary>
        public byte? EstimateCostCode { get; set; }
    }
}