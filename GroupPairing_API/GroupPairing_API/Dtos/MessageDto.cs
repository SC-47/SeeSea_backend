//----------------------------------------------
// <copyright file="MessageDto.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Dtos
{
    /// <summary>
    /// The DTO of Message.
    /// </summary>
    public class MessageDto
    {
        /// <summary>
        /// Gets or sets GroupPairing_API of the Id of the message.
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the user's Id of the message.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the user's name of the message.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the route of the user's picture.
        /// </summary>
        public string UserImage { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the context of the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the date time of the message.
        /// </summary>
        public string MessageDateTime { get; set; }
    }
}
