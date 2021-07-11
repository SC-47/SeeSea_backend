using System;
using System.Collections.Generic;

#nullable disable

namespace GroupPairing_API.Models.Db
{
    public partial class ActivityRoom
    {
        public int ActivityId { get; set; }
        public int HostId { get; set; }
        public string ActivityName { get; set; }
        public byte ActivityStatusCode { get; set; }
        public string ActivityPicture { get; set; }
        public byte CurrentParticipantNumber { get; set; }
        public byte ParticipantNumber { get; set; }
        public byte DivingTypeCode { get; set; }
        public byte DivingLevelCode { get; set; }
        public byte ActivityPropertyCode { get; set; }
        public DateTime ActivityDateTime { get; set; }
        public byte ActivityAreaCode { get; set; }
        public short ActivityPlaceCode { get; set; }
        public byte TransportationCode { get; set; }
        public string ActivityDescription { get; set; }
        public string MeetingPlace { get; set; }
        public byte EstimateCostCode { get; set; }
    }
}
