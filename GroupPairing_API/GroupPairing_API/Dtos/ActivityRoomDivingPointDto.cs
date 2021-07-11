//-----------------------------------------------------------------------
// <copyright file="ActivityRoomDivingPointDto.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Dtos
{
    /// <summary>
    /// The DTO of the ActivityRoom.
    /// </summary>
    public class ActivityRoomDivingPointDto
    {
        /// <summary>
        /// Gets or sets GroupPairing_API of the serial number of the activity room.
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the name of the activity room.
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the diving type of the activity room.
        /// </summary>
        public string DivingType { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the exact address of the activity taking place.
        /// </summary>
        public string ActivityProperty { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the corresponded code of the activity room status.
        /// </summary>
        public byte ActivityStatusCode { get; set; }
    }
}
