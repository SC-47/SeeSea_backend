//-----------------------------------------------------------------------
// <copyright file="ActivityParticipantDto.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Dtos
{
    /// <summary>
    /// The DTO of the member of the participantList.
    /// </summary>
    public class ActivityParticipantDto
    {
        /// <summary>
        /// Gets or sets GroupPairing_API of the serial number of the participant.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the name of the participant.
        /// </summary>
        public string ParticipantName { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the route of the participant's picture.
        /// </summary>
        public string ParticipantImage { get; set; }
    }
}
