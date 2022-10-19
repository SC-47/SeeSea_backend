using GroupPairing_API.Dtos;
using System.Collections.Generic;

namespace GroupPairing_API.Interface
{
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