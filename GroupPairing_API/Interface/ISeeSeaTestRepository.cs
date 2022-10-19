using GroupPairing_API.Models.Db;
using System.Collections.Generic;

namespace GroupPairing_API.Interface
{
    /// <summary>
    /// All the information about the datasheets from the database named SeeSeaTest.
    /// </summary>
    public interface ISeeSeaTestRepository
    {
        /// <summary>
        /// Get the information of all the diving points in the database named SeeSeaTest.
        /// </summary>
        /// <returns>The information of all the diving points</returns>
        public IEnumerable<DivingPoint> GetDivingPoints();

        /// <summary>
        /// Get the number of activity by the certain diving points.
        /// </summary>
        /// <param name="divingPointId">The ID of diving point.</param>
        /// <returns>The number of activity.</returns>
        public int GetDivingPointActivityNumber(int divingPointId);

        /// <summary>
        /// Activate user's account.
        /// </summary>
        /// <param name="userEmailId">The Id of the user's email.</param>
        public bool SetAccountActive(string userEmailId);
    }
}