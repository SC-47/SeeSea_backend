//-----------------------------------------------------------------------
// <copyright file="SeeSeaTestRepository.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Repository
{
    using GroupPairing_API.Interface;
    using GroupPairing_API.Models.Db;
    using GroupPairing_API.Parameters;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Linq;

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
        public void SetAccountActive(string userEmailId)
        {
            //根據Email ID搜尋指定User
            UserInfo userInfo = SeeSeaTestContext.UserInfoes.Where(user => user.UserEmailId.Equals(userEmailId)).SingleOrDefault();
            if (userInfo is null)
            {
                throw new SqlNullValueException("此帳號不存在");
            }

            userInfo.Approved = true;
            SeeSeaTestContext.SaveChanges();
        }

        /// <summary>
        /// Gets the UserInfo by specific userID.
        /// </summary>
        /// <param name="userID">The querying userID.</param>
        /// <returns>The querying UserInfo.</returns>
        public UserInfo GetUserInfo(int userID)
        {
            //return與輸入userID相同的UserInfo
            UserInfo userInfo = SeeSeaTestContext.UserInfoes
                .Where(user => user.UserId == userID)
                .SingleOrDefault();

            return userInfo;
        }

        /// <summary>
        /// Post a new Activity room to the database named SeeSeaTest.
        /// </summary>
        /// <param name="activityRoom">Activity Room.</param>
        public void PostRoom(ActivityRoom activityRoom)
        {
            SeeSeaTestContext.ActivityRooms.Add(activityRoom);
            SeeSeaTestContext.SaveChanges();
        }

        /// <summary>
        /// Get specific Activity rooms from the database named SeeSeaTest by ActivityID.
        /// </summary>
        /// <param name="activityId">The specific activityId.</param>
        /// <returns>The specific ActivityRoom.</returns>
        public ActivityRoom GetRoom(int activityId)
        {
            return SeeSeaTestContext.ActivityRooms.Where(room => room.ActivityId == activityId).SingleOrDefault();
        }

        /// <summary>
        /// Get all active Activity rooms from the database named SeeSeaTest.
        /// </summary>
        /// <returns>All the ActivityRoom.</returns>
        public IEnumerable<ActivityRoom> GetAllActiveRoom()
        {
            return SeeSeaTestContext.ActivityRooms.Where(room => room.ActivityStatusCode == (byte)Global.ActivityStatus.PAIRING || room.ActivityStatusCode == (byte)Global.ActivityStatus.FULL);
        }

        /// <summary>
        /// Get the information of all the rooms in the database named SeeSeaTest.
        /// </summary>
        /// <returns>All the rooms in the database named SeeSeaTest</returns>
        public IEnumerable<ActivityRoomPreDto> GetAllRoomPreDto()
        {
            return ChangeActivityRoomToData(SeeSeaTestContext.ActivityRooms);
        }

        /// <summary>
        /// Get all active ActivityRoomPreDtos from the database named SeeSeaTest.
        /// </summary>
        /// <returns>All the ActivityRoomPreDtos.</returns>
        public IEnumerable<ActivityRoomPreDto> GetAllActiveRoomPreDtos()
        {
            return ChangeActivityRoomToData(SeeSeaTestContext.ActivityRooms.Where(room => room.ActivityStatusCode == (byte)Global.ActivityStatus.PAIRING || room.ActivityStatusCode == (byte)Global.ActivityStatus.FULL));
        }

        /// <summary>
        /// Get specific ActivityRoomPreDto from the database named SeeSeaTest by ActivityID.
        /// </summary>
        /// <param name="activityId">The specific activityId.</param>
        /// <returns>The specific ActivityRoomPreDto.</returns>
        public IEnumerable<ActivityRoomPreDto> GetRoomPreDto(int activityId)
        {
            return ChangeActivityRoomToData(SeeSeaTestContext.ActivityRooms.Where(room => room.ActivityId == activityId));
        }

        /// <summary>
        /// Convert ActivityRoom to Data form.
        /// </summary>
        /// <param name="input">The inputting ActivityRoom data.</param>
        /// <returns>The data form of ActivityRoom data.</returns>
        private IEnumerable<ActivityRoomPreDto> ChangeActivityRoomToData(IQueryable<ActivityRoom> input)
        {
            //先將ActivityRoom及UserBasicInfo兩者透過ActivityId Join起來，得到HostName及HostImage
            return input
                .Join(
                        SeeSeaTestContext.UserInfoes,
                        activityRoom => activityRoom.HostId,
                        hostUserInfo => hostUserInfo.UserId,
                        (activityRoom, hostUserInfo) => new ActivityRoomPreDto
                        {
                            ActivityId = activityRoom.ActivityId,
                            HostId = activityRoom.HostId,
                            HostName = hostUserInfo.UserNickName ?? hostUserInfo.UserName,
                            HostImage = hostUserInfo.UserImage,
                            ActivityName = activityRoom.ActivityName,
                            ActivityStatusCode = activityRoom.ActivityStatusCode,
                            ActivityPicture = activityRoom.ActivityPicture ?? string.Empty,
                            CurrentParticipantNumber = activityRoom.CurrentParticipantNumber,
                            ParticipantNumber = activityRoom.ParticipantNumber,
                            DivingTypeCode = activityRoom.DivingTypeCode,
                            DivingLevelCode = activityRoom.DivingLevelCode,
                            ActivityPropertyCode = activityRoom.ActivityPropertyCode,
                            ActivityDate = activityRoom.ActivityDateTime.Date.ToString("yyyy-MM-dd, ddd"),
                            ActivityTime = activityRoom.ActivityDateTime.TimeOfDay.ToString(),
                            ActivityAreaCode = activityRoom.ActivityAreaCode,
                            ActivityPlace = SeeSeaTestContext.DivingPoints.Where(point => point.DivingPointId == activityRoom.ActivityPlaceCode).Select(point => point.DivingPointName).SingleOrDefault(),
                            TransportationCode = activityRoom.TransportationCode,
                            ActivityDescription = activityRoom.ActivityDescription ?? string.Empty,
                            MeetingPlace = activityRoom.MeetingPlace ?? string.Empty,
                            EstimateCostCode = activityRoom.EstimateCostCode,
                            Message = SeeSeaTestContext.MessageBoards.Where(message => message.ActivityId == activityRoom.ActivityId),
                            ApplicantImage = SeeSeaTestContext.ActivityApplicants
                                                .Where(applicant => applicant.ActivityId == activityRoom.ActivityId)
                                                .Join(
                                                        SeeSeaTestContext.UserInfoes,
                                                        applicant => applicant.ApplicantId,
                                                        userInfo => userInfo.UserId,
                                                        (applicant, userInfo) => userInfo.UserImage),
                            FavoriteNumber = SeeSeaTestContext.UserFavoriteActivities.Where(activity => activity.FavoriteActivityId == activityRoom.ActivityId).Count()
                        })
                .OrderBy(activityRoom => activityRoom.ActivityId);
        }
    }
}