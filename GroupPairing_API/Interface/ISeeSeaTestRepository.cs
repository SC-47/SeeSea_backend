//-----------------------------------------------------------------------
// <copyright file="ISeeSeaTestRepository.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Interface
{
    using GroupPairing_API.Models.Db;
    using GroupPairing_API.Parameters;
    using System.Collections.Generic;

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
        public void SetAccountActive(string userEmailId);

        /// <summary>
        /// Gets the UserInfo by specific userID.
        /// </summary>
        /// <param name="userID">The querying userID.</param>
        /// <returns>The querying UserInfo.</returns>
        public UserInfo GetUserInfo(int userID);

        /// <summary>
        /// Post a new Activity room to the database named SeeSeaTest.
        /// </summary>
        /// <param name="activityRoom">Activity Room.</param>
        public void PostRoom(ActivityRoom activityRoom);

        /// <summary>
        /// Get specific Activity rooms from the database named SeeSeaTest by ActivityID.
        /// </summary>
        /// <param name="activityId">The specific activityId.</param>
        /// <returns>The specific ActivityRoom.</returns>
        public ActivityRoom GetRoom(int activityId);

        /// <summary>
        /// Get all active Activity rooms from the database named SeeSeaTest.
        /// </summary>
        /// <returns>All the ActivityRoom.</returns>
        public IEnumerable<ActivityRoom> GetAllActiveRoom();

        /// <summary>
        /// Get the information of all the ActivityRoomPreDto in the database named SeeSeaTest.
        /// </summary>
        /// <returns>All the ActivityRoomPreDto in the database named SeeSeaTest</returns>
        public IEnumerable<ActivityRoomPreDto> GetAllRoomPreDto();

        /// <summary>
        /// Get all active ActivityRoomPreDtos from the database named SeeSeaTest.
        /// </summary>
        /// <returns>All the ActivityRoomPreDtos.</returns>
        public IEnumerable<ActivityRoomPreDto> GetAllActiveRoomPreDtos();

        /// <summary>
        /// Get specific ActivityRoomPreDto from the database named SeeSeaTest by ActivityID.
        /// </summary>
        /// <param name="activityId">The specific activityId.</param>
        /// <returns>The specific ActivityRoomPreDto.</returns>
        public IEnumerable<ActivityRoomPreDto> GetRoomPreDto(int activityId);
    }
} 