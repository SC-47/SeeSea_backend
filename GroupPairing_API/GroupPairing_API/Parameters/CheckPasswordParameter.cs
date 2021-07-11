//-----------------------------------------------------------------------
// <copyright file="CheckPasswordParameter.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Parameters
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The parameter for resetting password.
    /// </summary>
    public class CheckPasswordParameter
    {
        /// <summary>
        /// Gets or sets the ID of the user for resetting password.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the password of the user for logging in.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
