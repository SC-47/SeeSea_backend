using GroupPairing_API.Interface;
using GroupPairing_API.Models.Db;
using System.Collections.Generic;
using System.Linq;

namespace GroupPairing_API.Repository
{
    /// <summary>
    /// All the information about the datasheets from the database named SeeSeaTest.
    /// </summary>
    public class SeeSeaTestRepository : ISeeSeaTestRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeeSeaTestRepository" /> class.
        /// </summary>
        /// <param name="seeSeaTestContext">All the information about the datasheets from the database named SeeSeaTest.</param>
        public SeeSeaTestRepository(SeeSeaTestContext seeSeaTestContext)
        {
            SeeSeaTestContext = seeSeaTestContext;
        }

        /// <summary>
        /// Gets the SeeSeaContext class.
        /// </summary>
        private readonly SeeSeaTestContext SeeSeaTestContext;

        /// <summary>
        /// Get the information of all the diving points in the database named SeeSeaTest.
        /// </summary>
        /// <returns>The information of all the diving points</returns>
        public IEnumerable<DivingPoint> GetDivingPoints()
        {
            //取得所有潛點資訊，抓至記憶體中
            return SeeSeaTestContext.DivingPoints;
        }

        /// <summary>
        /// Get the number of activity of the certain diving points.
        /// </summary>
        /// <param name="divingPointId">The ID of diving point.</param>
        /// <returns>The number of activity of the certain diving point.</returns>
        public int GetDivingPointActivityNumber(int divingPointId)
        {
            return SeeSeaTestContext.ActivityRooms.Where(room => room.ActivityPlaceCode == divingPointId && (room.ActivityStatusCode == (int)Global.ActivityStatus.PAIRING || room.ActivityStatusCode == (int)Global.ActivityStatus.FULL)).Count();
        }

        /// <summary>
        /// Activate user's account.
        /// </summary>
        /// <param name="userEmailId">The Id of the user's email.</param>
        public bool SetAccountActive(string userEmailId)
        {
            //根據Email ID搜尋指定User
            UserInfo userInfo = SeeSeaTestContext.UserInfoes.Where(user => user.UserEmailId.Equals(userEmailId)).SingleOrDefault();
            if (userInfo is null)
            {
                return false;
            }

            userInfo.Approved = true;
            SeeSeaTestContext.SaveChanges();
            return true;
        }
    }
}