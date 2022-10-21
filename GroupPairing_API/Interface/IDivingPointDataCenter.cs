//-----------------------------------------------------------------------
// <copyright file="IDivingPointDataCenter.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Interface
{
    using GroupPairing_API.Dtos;
    using System.Collections.Generic;

    /// <summary>
    /// The Algorithmic logic of the data about the local diving points.
    /// </summary>
    public interface IDivingPointDataCenter
    {
        /// <summary>
        /// Get the DTO of all the information of diving points.
        /// </summary>
        /// <returns>The DTO of all the information of diving points.</returns>
        public IEnumerable<DivingPointDto> GetDivingPointDtoList();
    }
}