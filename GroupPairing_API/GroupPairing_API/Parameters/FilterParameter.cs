//-----------------------------------------------------------------------
// <copyright file="FilterParameter.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Parameters
{
    /// <summary>
    /// The parameter for filtering ActivityRoom data by specific condition.
    /// </summary>
    public class FilterParameter
    {
        /// <summary>
        /// Gets or sets the diving typing's code of the Activity's Diving Typing.
        /// </summary>
        public int[] DivingType { get; set; }

        /// <summary>
        /// Gets or sets the property's code of the Activity's Property.
        /// </summary>
        public int[] Property { get; set; }

        /// <summary>
        /// Gets or sets the area's code of the Activity's Area.
        /// </summary>
        public int[] Area { get; set; }

        /// <summary>
        /// Gets or sets the estimate cost's code of the Activity's Area.
        /// </summary>
        public int[] EstimateCodeCost { get; set; }
    }
}
