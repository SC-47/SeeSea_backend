//-----------------------------------------------------------------------
// <copyright file="UserRoomListDto.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Dtos
{
    using System.Collections.Generic;

    /// <summary>
    /// The DTO of the user's room list.
    /// </summary>
    public class UserRoomListDto
    {
        /// <summary>
        /// Gets or sets the participating room list of the specific userId.
        /// </summary>
        public List<ActivityRoomUserRoomDto> ParticipatingRoomList { get; set; }

        /// <summary>
        /// Gets or sets the favorite room list of the specific userId.
        /// </summary>
        public List<ActivityRoomUserRoomDto> FavoriteRoomList { get; set; }

        /// <summary>
        /// Gets or sets the signing up room list of the specific userId.
        /// </summary>
        public List<ActivityRoomUserRoomDto> SigningUpRoomList { get; set; }

        /// <summary>
        /// Gets or sets the room list which activity has done of the specific userId.
        /// </summary>
        public List<ActivityRoomUserRoomDto> EndingRoomList { get; set; }
    }
}
