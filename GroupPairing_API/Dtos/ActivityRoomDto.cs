//-----------------------------------------------------------------------
// <copyright file="ActivityRoomDto.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Dtos
{
    using System.Collections.Generic;

    /// <summary>
    /// The DTO of the ActivityRoom.
    /// </summary>
    public class ActivityRoomDto
    {
        /// <summary>
        /// Gets or sets GroupPairing_API of the serial number of the activity room.
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the host's Id of the activity room.
        /// </summary>
        public int HostId { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the host's UserName of the activity room.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the host's Image of the activity room.
        /// </summary>
        public string HostImage { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the name of the activity room.
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the corresponded code of the activity room status.
        /// </summary>
        public byte ActivityStatusCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the route of the picture of the activity room.
        /// </summary>
        public string ActivityPicture { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the number of the current participant of the activity room.
        /// </summary>
        public byte CurrentParticipantNumber { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the number of the participant of the activity room.
        /// </summary>
        public byte ParticipantNumber { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the diving type of the activity room.
        /// </summary>
        public string DivingType { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the required diving level which the participants needed.
        /// </summary>
        public string DivingLevel { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the activity's diving property.
        /// </summary>
        public string ActivityProperty { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the date of the activity taking place.
        /// </summary>
        public string ActivityDate { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the time of the activity taking place.
        /// </summary>
        public string ActivityTime { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the area of the activity taking place. 
        /// </summary>
        public string ActivityArea { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the exact address of the activity taking place.
        /// </summary>
        public string ActivityPlace { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the way that participant go to the activity's place.
        /// </summary>
        public string Transportation { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the description of the activity room.
        /// </summary>
        public string ActivityDescription { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the assembling place if needed.
        /// </summary>
        public string MeetingPlace { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the estimate cost range of this activity.
        /// </summary>
        public string EstimateCost { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the context of this message board.
        /// </summary>
        public List<MessageDto> MessageBoard { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the list of the applicants' image.
        /// </summary>
        public List<string> ApplicantImage { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the number of the user adding this activity in the favorite list.
        /// </summary>
        public int FavoriteNumber { get; set; }
    }
}
