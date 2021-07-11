//-----------------------------------------------------------------------
// <copyright file="ActivityRoomPost.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Parameters
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The input parameters of the POST ActivityRoom API.
    /// </summary>
    public class ActivityRoomPost
    {
        /// <summary>
        /// Gets or sets GroupPairing_API of the host's UserID of the activity room.
        /// </summary>
        [Range(Global.ID_MIN, int.MaxValue)]
        public int HostId { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the name of the activity room.
        /// </summary>
        [Required]
        [MaxLength(12)]
        public string ActivityName { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the picture of the activity room.
        /// </summary>
        public IFormFile ActivityPicture { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the number of the participant of the activity room.
        /// </summary>
        [Range(Global.PARTICIPANT_NUM_MIN, Global.PARTICIPANT_NUM_MAX)]
        public byte ParticipantNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the corresponded code of the diving type activity room status.
        /// </summary>
        [Range((int)Global.DivingType.FREE, (int)Global.DivingType.SCUBA)]
        public byte DivingTypeCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the corresponded code of the required diving level which the participant needed.
        /// </summary>
        [Range((int)Global.DivingLevel.NON_SETTING, (int)Global.DivingLevel.AIDA3)]
        public byte DivingLevelCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the corresponded code of the activity' property.
        /// </summary>
        [Range((int)Global.ActivityProperty.FUN, (int)Global.ActivityProperty.OTHER)]
        public byte ActivityPropertyCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the date and time of the activity taking place.
        /// </summary>
        [Required]
        public string ActivityDateTime { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the corresponded code of the area of the activity taking place. 
        /// </summary>
        [Range((int)Global.ActivityArea.NORTH, (int)Global.ActivityArea.LIUQIU)]
        public byte ActivityAreaCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the exact address of the activity taking place.
        /// </summary>
        [Required]
        [Range(Global.DIVING_POINT_MIN, Global.DIVING_POINT_MAX)]
        public short ActivityPlaceCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the corresponded code of the way that participant go to the activity's place.
        /// </summary>
        [Range((int)Global.Transportation.SELF, (int)Global.Transportation.PUBLIC_TRANPORTATION)]
        public byte TransportationCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the description of the activity room.
        /// </summary>
        public string ActivityDescription { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the assembling place if needed.
        /// </summary>
        public string MeetingPlace { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the corresponded code of the estimate cost range of this activity.
        /// </summary>
        [Range((int)Global.EstimateCost.NON_SETTING, (int)Global.EstimateCost.HIGHEST)]
        [Required]
        public byte EstimateCostCode { get; set; }
    }
}