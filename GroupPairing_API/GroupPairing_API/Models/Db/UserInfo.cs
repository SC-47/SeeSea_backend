using System;
using System.Collections.Generic;

#nullable disable

namespace GroupPairing_API.Models.Db
{
    public partial class UserInfo
    {
        public int UserId { get; set; }
        public string UserAccount { get; set; }
        public string UserPassword { get; set; }
        public string UserName { get; set; }
        public string UserNickName { get; set; }
        public byte UserAge { get; set; }
        public int? UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserEmailId { get; set; }
        public bool Approved { get; set; }
        public string UserImage { get; set; }
        public string UserDescription { get; set; }
        public byte UserExperienceCode { get; set; }
        public string DivingTypeTag { get; set; }
        public string AreaTag { get; set; }
    }
}
