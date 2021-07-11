//-----------------------------------------------------------------------
// <copyright file="ActivityApplicantPost.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Parameters
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The input parameters of the POST ActivityApplicant API.
    /// </summary>
    public class ActivityApplicantPost
    {
        /// <summary>
        /// Gets or sets the serial number of the ActivityRoom.
        /// </summary>
        [Range(Global.ID_MIN, int.MaxValue)]
        public int ActivityId { get; set; }

        /// <summary>
        /// Gets or sets the serial number of the applicant (applicant's userId).
        /// </summary>
        [Range(Global.ID_MIN, int.MaxValue)]
        public int ApplicantId { get; set; }

        /// <summary>
        /// Gets or sets the time for application of the applicant.
        /// </summary>
        [Required]
        public string ApplicatingDateTime { get; set; }

        /// <summary>
        /// Gets or sets the description of the applicant to tell why he wants to join this activity.
        /// </summary>
        public string ApplicatingDescription { get; set; }
    }
}
