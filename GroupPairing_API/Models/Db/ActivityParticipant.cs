using System;
using System.Collections.Generic;

#nullable disable

namespace GroupPairing_API.Models.Db
{
    public partial class ActivityParticipant
    {
        public int ActivityId { get; set; }
        public int ParticipantId { get; set; }
    }
}
