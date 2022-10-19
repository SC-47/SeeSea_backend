//-----------------------------------------------------------------------
// <copyright file="UserInfoPost.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Parameters
{
    using GroupPairing_API;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The posting parameter of UserInfo.
    /// </summary>
    public partial class UserInfoPost
    {
        /// <summary>
        /// Gets or sets UserInfo_API of the account of the user.
        /// </summary>
        [Required]
        public string UserAccount { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the password of the user.
        /// </summary>
        [Required]
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the name of the user.
        /// </summary>
        [Required]
        [MaxLength(Global.NAME_LENGTH_MAX)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the nickname of the user.
        /// </summary>
        [MaxLength(Global.NAME_LENGTH_MAX)]
        public string UserNickName { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the age of the user.
        /// </summary>
        [Range(Global.ZERO, Global.AGE_MAX)]
        public byte UserAge { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the cell phone number of the user.
        /// </summary>
        public int? UserPhone { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the user's E-mail's address
        /// </summary>
        [Required]
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the code of user's identity.
        /// </summary>
        [Range((int)Global.UserExperience.NO_EXPERIENCE, (int)Global.UserExperience.VETERAN)]
        public byte UserExperienceCode { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the description of the user.
        /// </summary>
        public string UserDescription { get; set; }

        /// <summary>
        /// Gets or sets the prefer diving type of the user.
        /// </summary>
        public string DivingTypeTag { get; set; }

        /// <summary>
        /// Gets or sets the prefer activity area of the user.
        /// </summary>
        public string AreaTag { get; set; }
    }
}
