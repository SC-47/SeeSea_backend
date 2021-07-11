//-----------------------------------------------------------------------
// <copyright file="MessagePost.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Parameters
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The posting parameter of Message.
    /// </summary>
    public class MessagePost
    {
        /// <summary>
        /// Gets or sets GroupPairing_API of the Id of the activity of the message.
        /// </summary>
        [Required]
        public int ActivityId { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the user's Id of the message.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the context of the message.
        /// </summary>
        [Required]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the date time of the message.
        /// </summary>
        [Required]
        public string MessageDateTime { get; set; }
    }
}
