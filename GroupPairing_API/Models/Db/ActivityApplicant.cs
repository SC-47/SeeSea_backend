using System;
using System.Collections.Generic;

#nullable disable

namespace GroupPairing_API.Models.Db
{
    public partial class ActivityApplicant
    {
        public int ActivityId { get; set; }
        public int ApplicantId { get; set; }
        public DateTime ApplicatingDateTime { get; set; }
        public string ApplicatingDescription { get; set; }
    }
}
