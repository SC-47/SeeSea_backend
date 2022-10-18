//-----------------------------------------------------------------------
// <copyright file="UserInfoPut.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Parameters
{
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The putting parameter of UserInfo with uploading picture (form-data version).
    /// </summary>
    public class UserInfoPut
    {
        /// <summary>
        /// Gets or sets UserInfo_API of the password of the user.
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the nickname of the user.
        /// </summary>
        public string UserNickName { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the cell phone number of the user.
        /// </summary>
        public int? UserPhone { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the user's E-mail's address
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the code of user's identity.
        /// </summary>
        public byte? UserExperienceCode { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the picture of the user's image.
        /// </summary>
        public IFormFile UserImage { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the description of the user.
        /// </summary>
        public string UserDescription { get; set; }

        /// <summary>
        /// Gets or sets the prefer diving type's code string of the user.
        /// </summary>
        public string DivingTypeTag { get; set; }

        /// <summary>
        /// Gets or sets the prefer activity area's code string of the user.
        /// </summary>
        public string AreaTag { get; set; }
    }
}
