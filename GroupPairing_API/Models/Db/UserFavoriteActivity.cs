using System;
using System.Collections.Generic;

#nullable disable

namespace GroupPairing_API.Models.Db
{
    public partial class UserFavoriteActivity
    {
        public int UserId { get; set; }
        public int FavoriteActivityId { get; set; }
    }
}
