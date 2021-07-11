//-----------------------------------------------------------------------
// <copyright file="Login.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Parameters
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The account and password for login.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Gets or sets the account of the user for logging in.
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the password of the user for logging in.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
