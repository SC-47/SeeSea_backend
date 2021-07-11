//-----------------------------------------------------------------------
// <copyright file="UserInfoDto.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Dtos
{
    using System.Collections.Generic;

    /// <summary>
    /// The DTO of UserInfo.
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// Gets or sets UserInfo_API of the serial number of the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the account of the user.
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the name of the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the nickname of the user.
        /// </summary>
        public string UserNickName { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the age of the user.
        /// </summary>
        public byte UserAge { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the cell phone number of the user.
        /// </summary>
        public int UserPhone { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the user's E-mail's address
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this account is active or not.
        /// </summary>
        public bool IsAcccountActive { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the route of the user's image.
        /// </summary>
        public string UserImage { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the description of the user.
        /// </summary>
        public string UserDescription { get; set; }

        /// <summary>
        /// Gets or sets UserInfo_API of the user's experience for diving.
        /// </summary>
        public string UserExperience { get; set; }

        /// <summary>
        /// Gets or sets the prefer diving type's string list of the user.
        /// </summary>
        public List<string> DivingTypeTag { get; set; }

        /// <summary>
        /// Gets or sets the prefer activity area's string list of the user.
        /// </summary>
        public List<string> AreaTag { get; set; }

        /// <summary>
        /// Gets or sets the favorite room list of the specific userId.
        /// </summary>
        public List<int> UserFavoriteRoom { get; set; }

        /// <summary>
        /// Gets or sets the participating room list of the specific userId.
        /// </summary>
        public List<int> UserParticipatingActivity { get; set; }

        /// <summary>
        /// Gets or sets the signing up room list of the specific userId.
        /// </summary>
        public List<int> UserSigningUpActivity { get; set; }

        /// <summary>
        /// Gets or sets the finish room list of the specific userId.
        /// </summary>
        public List<int> UserFinishActivity { get; set; }
    }
}
