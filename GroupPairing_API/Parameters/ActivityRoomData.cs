using GroupPairing_API.Dtos;
using GroupPairing_API.Models.Db;
using System.Collections.Generic;
using System.Linq;

namespace GroupPairing_API.Parameters
{
    /// <summary>
    /// The previous dto of activityRoom.
    /// </summary>
    public class ActivityRoomPreDto
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
        /// Gets or sets the code of the diving type of the activity room.
        /// </summary>
        public byte DivingTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the code of the required diving level which the participants needed.
        /// </summary>
        public byte DivingLevelCode { get; set; }

        /// <summary>
        /// Gets or sets the code of the activity's diving property.
        /// </summary>
        public byte ActivityPropertyCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the date of the activity taking place.
        /// </summary>
        public string ActivityDate { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the time of the activity taking place.
        /// </summary>
        public string ActivityTime { get; set; }

        /// <summary>
        /// Gets or sets the code of the area of the activity taking place. 
        /// </summary>
        public byte ActivityAreaCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the exact address of the activity taking place.
        /// </summary>
        public string ActivityPlace { get; set; }

        /// <summary>
        /// Gets or sets the code of the way that participant go to the activity's place.
        /// </summary>
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
        /// Gets or sets the code of the estimate cost range of this activity.
        /// </summary>
        public byte EstimateCostCode { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the context of this message board.
        /// </summary>
        public IQueryable<MessageBoard> Message { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the list of the applicants' image.
        /// </summary>
        public IQueryable<string> ApplicantImage { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the number of the user adding this activity in the favorite list.
        /// </summary>
        public int FavoriteNumber { get; set; }
    }
}
