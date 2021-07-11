//-----------------------------------------------------------------------
// <copyright file="ActivityApplicantDto.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Dtos
{
    /// <summary>
    /// The DTO of the member of the applicants' list.
    /// </summary>
    public class ActivityApplicantDto
    {
        /// <summary>
        /// Gets or sets GroupPairing_API of the serial number of the applicant.
        /// </summary>
        public int ApplicantId { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the name of the applicant.
        /// </summary>
        public string ApplicantName { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the route of the applicant's picture.
        /// </summary>
        public string ApplicantImage { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the time for application.
        /// </summary>
        public string ApplicatingDateTime { get; set; }

        /// <summary>
        /// Gets or sets GroupPairing_API of the description of the applicant.
        /// </summary>
        public string ApplicatingDescription { get; set; }
    }
}
