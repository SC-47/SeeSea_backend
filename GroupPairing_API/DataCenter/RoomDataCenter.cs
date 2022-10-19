//-----------------------------------------------------------------------
// <copyright file="RoomDataCenter.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GroupPairing_API.DataCenter;
    using GroupPairing_API.Dtos;
    using GroupPairing_API.Models.Db;
    using GroupPairing_API.Parameters;

    /// <summary>
    /// The Algorithmic logic of the data about GroupPairing_API.
    /// </summary>
    public class RoomDataCenter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoomDataCenter" /> class.
        /// </summary>
        /// <param name="seeSeaTestContext">All the information about the datasheets from the database named SeeSeaTest.</param>
        /// <param name="userDataCenter">The Algorithmic logic of the data about UserInfo_API. </param>
        public RoomDataCenter(SeeSeaTestContext seeSeaTestContext, UserDataCenter userDataCenter)
        {
            SeeSeaTestContext = seeSeaTestContext;
            UserDataCenter = userDataCenter;
        }

        /// <summary>
        /// Gets the SeeSeaContext class.
        /// </summary>
        public SeeSeaTestContext SeeSeaTestContext { get; }

        /// <summary>
        /// Gets the algorithmic logic of the user's data about GroupPairing_API. 
        /// </summary>
        public UserDataCenter UserDataCenter { get; }

        /// <summary>
        /// Post a new Activity room to the database named SeeSeaTest.
        /// </summary>
        /// <param name="activityRoomInput">The input parameter of ActivityRoom.</param>
        /// <param name="dateTime">The inputting DateTime.</param>
        public void PostRoom(ActivityRoomPost activityRoomInput, DateTime dateTime, string dateString)
        {
            //新增一筆ActivityRoom資料
            ActivityRoom activityRoom = new()
            {
                HostId = activityRoomInput.HostId,
                ActivityName = activityRoomInput.ActivityName,
                ActivityStatusCode = 1,
                CurrentParticipantNumber = 1,
                ParticipantNumber = activityRoomInput.ParticipantNumber,
                DivingTypeCode = activityRoomInput.DivingTypeCode,
                DivingLevelCode = activityRoomInput.DivingLevelCode,
                ActivityPropertyCode = activityRoomInput.ActivityPropertyCode,
                ActivityDateTime = dateTime,
                ActivityAreaCode = activityRoomInput.ActivityAreaCode,
                ActivityPlaceCode = activityRoomInput.ActivityPlaceCode,
                TransportationCode = activityRoomInput.TransportationCode,
                ActivityDescription = activityRoomInput.ActivityDescription,
                MeetingPlace = activityRoomInput.MeetingPlace,
                EstimateCostCode = activityRoomInput.EstimateCostCode
            };

            if (activityRoomInput.ActivityPicture != null)
            {
                activityRoom.ActivityPicture = $@"http://35.221.136.55/image/{dateString}{activityRoomInput.ActivityPicture.FileName}";            
            }

            //若潛水型態是水肺，則需要對前端給的潛水難度數值做對應調整
            if (activityRoom.DivingTypeCode == (byte)Global.DivingType.FREE)
            {
                switch (activityRoom.DivingLevelCode)
                {
                    case 2:
                        activityRoom.DivingLevelCode = (byte)Global.DivingLevel.AIDA1;
                        break;
                    case 3:
                        activityRoom.DivingLevelCode = (byte)Global.DivingLevel.AIDA2;
                        break;
                    case 4:
                        activityRoom.DivingLevelCode = (byte)Global.DivingLevel.AIDA3;
                        break;
                }
            }

            //在Db資料庫中新添加一筆ActivityRoom資料
            SeeSeaTestContext.ActivityRooms.Add(activityRoom);
        }

        /// <summary>
        /// Get specific Activity rooms from the database named SeeSeaTest by ActivityID.
        /// </summary>
        /// <param name="activityId">The specific activityId.</param>
        /// <returns>The specific ActivityRoom.</returns>
        public IEnumerable<ActivityRoom> GetRoom(int activityId)
        {
            //回傳ActivityID的ActivityRoom
            return SeeSeaTestContext.ActivityRooms.Where(room => room.ActivityId == activityId);
        }

        /// <summary>
        /// Get specific non-full Activity rooms from the database named SeeSeaTest by ActivityID.
        /// </summary>
        /// <param name="activityId">The querying activityId.</param>
        /// <returns>The specific non-full ActivityRoom.</returns>
        public ActivityRoom GetNonFullRoom(int activityId)
        {
            //回傳ActivityID的ActivityRoom
            return SeeSeaTestContext.ActivityRooms.Where(room => room.ActivityId == activityId && room.ActivityStatusCode == (int)Global.ActivityStatus.PAIRING).SingleOrDefault();
        }

        /// <summary>
        /// Get the specific active Activity rooms from the database named SeeSeaTest.
        /// </summary>
        /// <param name="activityId">The querying activityID.</param>
        /// <returns>Return the specific active ActivityRoom</returns>
        public ActivityRoom GetActiveRoom(int activityId)
        {
            //回傳指定AcitivtyID的「未完成」的活動房間(未完成包含兩種狀態：揪團中以及滿員但尚未出團)
            return GetActiveRoom().Where(room => room.ActivityId == activityId).SingleOrDefault();
        }

        /// <summary>
        /// Get specific Activity room (according to input activityID) from the database named SeeSeaTest.
        /// </summary>
        /// <param name="activityId">The searching ActivityID.</param>
        /// <returns>The ActivityRoomDTO which send to the frontend side.</returns>
        public ActivityRoomDto GetRoomDto(int activityId)
        {
            //將指定房間ID的活動房間以DTO型式回傳
            return ChangeActivityRoomToDto(GetRoom(activityId)).SingleOrDefault();
        }

        /// <summary>
        /// Get all Activity rooms from the database named SeeSeaTest.
        /// </summary>
        /// <returns>The ActivityRoomDTO which send to the frontend side.</returns>
        public IEnumerable<ActivityRoomDto> GetRoomDto()
        {
            //將所有活動房間列表以DTO型式回傳
            return ChangeActivityRoomToDto(SeeSeaTestContext.ActivityRooms);
        }

        /// <summary>
        /// Get the querying Activity rooms from the database named SeeSeaTest.
        /// </summary>
        /// <param name="activityRooms">The inputting ActivityRooms.</param>
        /// <returns>The ActivityRoomDTO which send to the frontend side.</returns>
        public IEnumerable<ActivityRoomDto> GetRoomDto(IEnumerable<ActivityRoom> activityRooms)
        {
            //將結果回傳
            return ChangeActivityRoomToDto(activityRooms);
        }

        /// <summary>
        /// Get specific Activity rooms (according to input ActivityIDs) from the database named SeeSeaTest.
        /// </summary>
        /// <param name="activityIDs">The searching ActivityIDs.</param>
        /// <returns>The ActivityRoomDTO which send to the frontend side.</returns>
        public IEnumerable<ActivityRoomDto> GetRoomDto(string activityIDs)
        {
            //將輸入字串轉換成int列表
            List<int> activityIDList = Global.ConvertStringToIntList(activityIDs);

            //初始化結果列表(輸出類別為"ActivityRoomDto")
            List<ActivityRoomDto> result = new();

            //將參數陣列取出與ActivityRoom資料表中ActivityID相同的房間
            foreach (int activityID in activityIDList)
            {
                //找出ActivityRoom中是否有與輸入的ActivityID相同的房間資訊
                var activityRoomDto = GetRoomDto(activityID);

                //若有與輸入的ActivityID相同的房間資訊，則將結果加入至輸出串列中
                if (activityRoomDto != null)
                {
                    result.Add(activityRoomDto);
                }
            }

            //回傳輸出串列結果
            return result;
        }

        /// <summary>
        /// Get all active Activity rooms from the database named SeeSeaTest.
        /// </summary>
        /// <returns>The ActivityRoomDTO which send to the frontend side.</returns>
        public IEnumerable<ActivityRoomDto> GetActiveRoomDto()
        {
            //回傳所有「未完成」的活動房間列表(未完成包含兩種狀態：揪團中以及滿員但尚未出團)，以DTO型式回傳
            return GetRoomDto(GetActiveRoom());
        }

        /// <summary>
        /// The selected ActivityRoomDTO data by specific condition.
        /// </summary>
        /// <param name="divingType">The querying string of divingType tag.</param>
        /// <param name="property">The querying string of property tag.</param>
        /// <param name="area">The querying string of area tag.</param>
        /// <param name="estimateCost">The querying string of estimateCost tag.</param>
        /// <returns>The returning selected ActivityRoomDTOs.</returns>
        public IEnumerable<ActivityRoomDto> GetActiveRoomDtoBySelector(string divingType, string property, string area, string estimateCost)
        {
            //取得所有「未完成」的活動房間列表(未完成包含兩種狀態：揪團中以及滿員但尚未出團)
            IEnumerable<ActivityRoom> result = GetActiveRoom();

            //若有對潛水類型進行篩選，則進入潛水類型篩選判斷
            if (!string.IsNullOrWhiteSpace(divingType))
            {
                List<int> divingCodeList = Global.ConvertStringToIntList(divingType);
                if (divingCodeList.Count == Global.ONE)
                {
                    result = result.Where(room => room.DivingTypeCode == divingCodeList.First());
                }
            }

            //若有對揪團性質進行篩選，則進入揪團性質篩選判斷
            if (!string.IsNullOrEmpty(property))
            {
                List<int> propertyCodeList = Global.ConvertStringToIntList(property);
                switch (propertyCodeList.Count)
                {
                    case 1:
                        result = result.Where(room => room.ActivityPropertyCode == propertyCodeList.First());
                        break;
                    case 2:
                        result = result.Where(room => room.ActivityPropertyCode == propertyCodeList[Global.FIRST] || room.ActivityPropertyCode == propertyCodeList[Global.SECOND]);
                        break;
                    case 3:
                        result = result.Where(room => room.ActivityPropertyCode == propertyCodeList[Global.FIRST] || room.ActivityPropertyCode == propertyCodeList[Global.SECOND] || room.ActivityPropertyCode == propertyCodeList[Global.THIRD]);
                        break;
                }
            }

            //若有對活動地區進行篩選，則進入對活動地區篩選判斷
            if (!string.IsNullOrEmpty(area))
            {
                List<int> areaCodeList = Global.ConvertStringToIntList(area);
                switch (areaCodeList.Count)
                {
                    case 1:
                        result = result.Where(room => room.ActivityAreaCode == areaCodeList.First());
                        break;
                    case 2:
                        result = result.Where(room => room.ActivityAreaCode == areaCodeList[Global.FIRST] || room.ActivityAreaCode == areaCodeList[Global.SECOND]);
                        break;
                    case 3:
                        result = result.Where(room => room.ActivityAreaCode == areaCodeList[Global.FIRST] || room.ActivityAreaCode == areaCodeList[Global.SECOND] || room.ActivityAreaCode == areaCodeList[Global.THIRD]);
                        break;
                    case 4:
                        result = result.Where(room => room.ActivityAreaCode == areaCodeList[Global.FIRST] || room.ActivityAreaCode == areaCodeList[Global.SECOND] || room.ActivityAreaCode == areaCodeList[Global.THIRD] || room.ActivityAreaCode == areaCodeList[Global.FOURTH]);
                        break;
                    case 5:
                        result = result.Where(room => room.ActivityAreaCode == areaCodeList[Global.FIRST] || room.ActivityAreaCode == areaCodeList[Global.SECOND] || room.ActivityAreaCode == areaCodeList[Global.THIRD] || room.ActivityAreaCode == areaCodeList[Global.FOURTH] || room.ActivityAreaCode == areaCodeList[Global.FIFTH]);
                        break;
                    case 6:
                        result = result.Where(room => room.ActivityAreaCode == areaCodeList[Global.FIRST] || room.ActivityAreaCode == areaCodeList[Global.SECOND] || room.ActivityAreaCode == areaCodeList[Global.THIRD] || room.ActivityAreaCode == areaCodeList[Global.FOURTH] || room.ActivityAreaCode == areaCodeList[Global.FIFTH] || room.ActivityAreaCode == areaCodeList[Global.SIXTH]);
                        break;
                }
            }

            //若有對預估金額進行篩選，則進入預估金額篩選判斷，
            if (!string.IsNullOrEmpty(estimateCost))
            {
                int estimateCostCode = (int)Global.EstimateCostTag.HIGHEST;
                if (int.TryParse(estimateCost, out estimateCostCode))
                {
                    result = result.Where(room => room.EstimateCostCode <= estimateCostCode);
                }
            }

            //將篩選完結果回傳出去
            return ChangeActivityRoomToDto(result);
        }

        /// <summary>
        /// Get all Activity rooms from the database named SeeSeaTest.
        /// </summary>
        /// <returns>The ActivityRoomDTO which send to the frontend side.</returns>
        public IEnumerable<ActivityRoomUserRoomDto> GetRoomUserRoomDto()
        {
            //將結果回傳
            return ChangeActivityRoomToUserRoomDto(SeeSeaTestContext.ActivityRooms);
        }

        /// <summary>
        /// Get specific Activity rooms (according to input ActivityIDs) from the database named SeeSeaTest.
        /// </summary>
        /// <param name="activityIDList">The searching ActivityIDs.</param>
        /// <returns>The ActivityRoomDTO which send to the frontend side.</returns>
        public IEnumerable<ActivityRoomUserRoomDto> GetRoomUserRoomDto(List<int> activityIDList)
        {
            //取得所有房間列表，並轉換成Dto
            IEnumerable<ActivityRoomUserRoomDto> activityRooms = GetRoomUserRoomDto();

            //初始化結果列表(輸出類別為"ActivityRoomDto")
            List<ActivityRoomUserRoomDto> result = new();

            //將參數陣列取出與ActivityRoom資料表中ActivityID相同的房間
            foreach (int activityID in activityIDList)
            {
                //找出ActivityRoom中是否有與輸入的ActivityID相同的房間資訊
                var activityRoomDto = activityRooms.SingleOrDefault(room => room.ActivityId == activityID);

                //若有與輸入的ActivityID相同的房間資訊，則將結果加入至輸出串列中
                if (activityRoomDto != null)
                {
                    result.Add(activityRoomDto);
                }
            }

            //回傳輸出串列結果
            return result;
        }

        /// <summary>
        /// Get specific Activity rooms (according to input divingPointId) from the database named SeeSeaTest.
        /// </summary>
        /// <param name="divingPointId">The searching ActivityID.</param>
        /// <returns>The ActivityRoomDTO which send to the frontend side.</returns>
        public IEnumerable<ActivityRoomDivingPointDto> GetActiveRoomDivingPointDto(int divingPointId)
        {
            //找出ActivityRoom中是否有與輸入的divingPointId相同的房間資訊，回傳輸出串列結果
            return ChangeActivityRoomToDivingPointDto(GetActiveRoom().Where(room => room.ActivityPlaceCode == divingPointId));
        }

        /// <summary>
        /// Put specific Activity room from the database named SeeSeaTest.
        /// </summary>
        /// <param name="activityRoom">The searching ActivityID.</param>
        /// <param name="activityRoomInput">The inputting of the ActivityRoom for updating the ActivityRoom.</param>
        public void PutAcitivty(ActivityRoom activityRoom, ActivityRoomPut activityRoomInput, string dateString)
        {
            //更新活動內容，若該欄位使用者未輸入，則沿用原先資料
            activityRoom.ActivityName = activityRoomInput.ActivityName ?? activityRoom.ActivityName;
            if (activityRoomInput.ActivityPicture != null)
            {
                activityRoom.ActivityPicture = !activityRoomInput.ActivityPicture.FileName.Equals("profilePic.jpg") ? $@"http://35.221.136.55/image/{dateString}{activityRoomInput.ActivityPicture.FileName}" : activityRoom.ActivityPicture;
            }

            //並判斷是否該欄位是否位在正確的輸入範圍
            byte divingLevelCode = activityRoomInput.DivingLevelCode ?? Global.ZERO;
            if (divingLevelCode != Global.ZERO && divingLevelCode >= (byte)Global.DivingLevel.NON_SETTING && divingLevelCode <= (byte)Global.DivingLevel.AIDA3)
            {
                activityRoom.DivingLevelCode = divingLevelCode;
            }

            short activityPlaceCode = activityRoomInput.ActivityPlaceCode ?? Global.ZERO;
            if (activityPlaceCode != Global.ZERO && activityPlaceCode >= Global.DIVING_POINT_MIN && activityPlaceCode <= Global.DIVING_POINT_MAX)
            {
                activityRoom.ActivityPlaceCode = activityPlaceCode;
            }

            byte transportationCode = activityRoomInput.TransportationCode ?? Global.ZERO;
            if (transportationCode != Global.ZERO && transportationCode >= (byte)Global.Transportation.SELF && transportationCode <= (byte)Global.Transportation.PUBLIC_TRANPORTATION)
            {
                activityRoom.TransportationCode = transportationCode;
            }

            activityRoom.ActivityDescription = activityRoomInput.ActivityDescription ?? activityRoom.ActivityDescription;
            activityRoom.MeetingPlace = activityRoomInput.MeetingPlace ?? activityRoom.MeetingPlace;

            byte estimateCostCode = activityRoomInput.EstimateCostCode ?? Global.ZERO;

            if (estimateCostCode != Global.ZERO && estimateCostCode >= (byte)Global.EstimateCost.LOWEST && estimateCostCode <= (byte)Global.EstimateCost.HIGHEST)
            {
                activityRoom.EstimateCostCode = estimateCostCode;
            }
        }

        /// <summary>
        /// Delete the specific ActivityRoom.
        /// </summary>
        /// <param name="targetRoom">The specific of ActivityRoom.</param>
        public void DeleteRoom(ActivityRoom targetRoom)
        {
            //將指定活動ID存下
            int activityID = targetRoom.ActivityId;

            //刪除該活動ID的留言資料
            var messages = SeeSeaTestContext.MessageBoards.Where(message => message.ActivityId == activityID);
            foreach (var message in messages)
            {
                SeeSeaTestContext.MessageBoards.Remove(message);
            }

            //從資料庫中刪除該活動ID的報名者清單
            var applicants = SeeSeaTestContext.ActivityApplicants.Where(applicant => applicant.ActivityId == activityID);
            foreach (var applicant in applicants)
            {
                SeeSeaTestContext.ActivityApplicants.Remove(applicant);
            }

            //從資料庫中刪除該活動的參加者清單
            var participants = SeeSeaTestContext.ActivityParticipants.Where(participant => participant.ActivityId == activityID);
            foreach (var participant in participants)
            {
                SeeSeaTestContext.ActivityParticipants.Remove(participant);
            }

            //從使用者的關注清單中移除該活動
            var favoriteRooms = SeeSeaTestContext.UserFavoriteActivities.Where(user => user.FavoriteActivityId == activityID);
            foreach (var favoriteRoom in favoriteRooms)
            {
                SeeSeaTestContext.UserFavoriteActivities.Remove(favoriteRoom);
            }

            //從資料庫中刪除指定活動
            SeeSeaTestContext.ActivityRooms.Remove(targetRoom);
        }

        /// <summary>
        /// Post a new ActivityApplicant.
        /// </summary>
        /// <param name="input">The inputting parameter of ActivityApplicant.</param>
        public void PostActivityApplicant(ActivityApplicantPost input)
        {
            //新增一個ActivityApplicant
            SeeSeaTestContext.ActivityApplicants.Add(new ActivityApplicant
            {
                ActivityId = input.ActivityId,
                ApplicantId = input.ApplicantId,
                ApplicatingDateTime = DateTime.ParseExact(input.ApplicatingDateTime, "yyyy/M/dd HH:mm", null),
                ApplicatingDescription = input.ApplicatingDescription
            });
        }

        /// <summary>
        /// Get the specific ActivityApplicant.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <param name="userId">The inputting userId.</param>
        /// <returns>The specific ActivityApplicant.</returns>
        public ActivityApplicant GetActivityApplicant(int activityId, int userId)
        {
            //根據指定條件查找指定ActivityApplicant
            return SeeSeaTestContext.ActivityApplicants.Where(activity => activity.ActivityId == activityId && activity.ApplicantId == userId).SingleOrDefault();
        }

        /// <summary>
        /// Get the applicants' list by the roomId.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <returns>The room's applicants' list.</returns>
        public IEnumerable<ActivityApplicantDto> GetApplicantList(int activityId)
        {
            //從ActivityApplicant資料表中，找到與輸入ActivityId相同的參加者，並轉換成DTO形式給前端
            IEnumerable<ActivityApplicantDto> result = SeeSeaTestContext.ActivityApplicants
                .Where(applicant => applicant.ActivityId == activityId)
                .Join(
                        SeeSeaTestContext.UserInfoes,
                        applicant => applicant.ApplicantId,
                        userInfo => userInfo.UserId,
                        (applicant, userInfo) => new
                        {
                            applicant,
                            ApplicantName = userInfo.UserNickName ?? userInfo.UserName,
                            ApplicantImage = userInfo.UserImage
                        })
                .OrderBy(activiy => activiy.applicant.ApplicatingDateTime)
                .Select(activiy => new ActivityApplicantDto
                {
                    ApplicantId = activiy.applicant.ApplicantId,
                    ApplicantName = activiy.ApplicantName,
                    ApplicantImage = activiy.ApplicantImage ?? string.Empty,
                    ApplicatingDateTime = activiy.applicant.ApplicatingDateTime.ToString("yyyy/MM/dd HH:mm"),
                    ApplicatingDescription = activiy.applicant.ApplicatingDescription ?? string.Empty
                });

            //將結果輸出
            return result;
        }

        /// <summary>
        /// Check whether the inputting applicant exists or not.
        /// </summary>
        /// <param name="activityId">The querying activityID.</param>
        /// <param name="userId">The querying userID.</param>
        /// <returns>If the data exists, return true. Otherwise, return false.</returns>
        public bool IsApplicant(int activityId, int userId)
        {
            return SeeSeaTestContext.ActivityApplicants
                    .Where(user => user.ActivityId == activityId && user.ApplicantId == userId)
                    .Any();
        }

        /// <summary>
        /// Post a new ActivityParticipant.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <param name="userId">The inputting userId.</param>
        public void PostActivityParticipant(int activityId, int userId)
        {
            //在ActivityParticipant資料表中添加一筆新的ActivityParticipant
            SeeSeaTestContext.ActivityParticipants.Add(new ActivityParticipant
            {
                ActivityId = activityId,
                ParticipantId = userId
            });
        }

        /// <summary>
        /// Get the specific ActivityParticipant.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <param name="userId">The inputting userId.</param>
        /// <returns>The inputting parameter of ActivityParticipant.</returns>
        public ActivityParticipant GetActivityParticipant(int activityId, int userId)
        {
            //根據指定條件查找指定ActivityParticipant
            return SeeSeaTestContext.ActivityParticipants.Where(activity => activity.ActivityId == activityId && activity.ParticipantId == userId).SingleOrDefault();
        }

        /// <summary>
        /// Get the participants' list by the roomId.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <returns>The room's participants' list.</returns>
        public IEnumerable<ActivityParticipantDto> GetParticipantList(int activityId)
        {
            //從ActivityParticipant資料表中，找到與輸入ActivityId相同的參加者，並轉換成DTO形式給前端
            IEnumerable<ActivityParticipantDto> result = SeeSeaTestContext.ActivityParticipants
                .Where(participant => participant.ActivityId == activityId)
                .Join(
                        SeeSeaTestContext.UserInfoes,
                        participant => participant.ParticipantId,
                        userInfo => userInfo.UserId,
                        (participant, userInfo) => new
                        {
                            participant,
                            ParticipantName = userInfo.UserNickName ?? userInfo.UserName,
                            ParticipantImage = userInfo.UserImage,
                        })
                .Select(
                        activity => new ActivityParticipantDto
                        {
                            ParticipantId = activity.participant.ParticipantId,
                            ParticipantName = activity.ParticipantName,
                            ParticipantImage = activity.ParticipantImage,
                        });

            //將結果輸出
            return result;
        }

        /// <summary>
        /// Check whether the inputting participant exists or not.
        /// </summary>
        /// <param name="activityId">The querying activityID.</param>
        /// <param name="userId">The querying userID.</param>
        /// <returns>If the data exists, return true. Otherwise, return false.</returns>
        public bool IsParticipant(int activityId, int userId)
        {
            return SeeSeaTestContext.ActivityParticipants
                    .Where(user => user.ActivityId == activityId && user.ParticipantId == userId)
                    .Any();
        }

        /// <summary>
        /// Remove the specific participant.
        /// </summary>
        /// <param name="target">The target participant.</param>
        public void RemoveParticipant(ActivityParticipant target)
        {
            SeeSeaTestContext.Remove(target);
            ActivityRoom activityRoom = GetRoom(target.ActivityId).SingleOrDefault();

            if (activityRoom.CurrentParticipantNumber == activityRoom.ParticipantNumber || activityRoom.ActivityStatusCode == (int)Global.ActivityStatus.CONFIRMED)
            {
                activityRoom.ActivityStatusCode = (int)Global.ActivityStatus.PAIRING;
            }

            activityRoom.CurrentParticipantNumber = --activityRoom.CurrentParticipantNumber;
            if (activityRoom.ActivityStatusCode == (int)Global.ActivityStatus.FULL)
            {
                activityRoom.ActivityStatusCode = (int)Global.ActivityStatus.PAIRING;
            }
        }

        /// <summary>
        /// Post a new Message.
        /// </summary>
        /// <param name="input">The inputting parameter of message.</param>
        public void PostMessage(MessagePost input)
        {
            //在MessageBoard資料表中添加一筆新的留言
            SeeSeaTestContext.MessageBoards.Add(new MessageBoard
            {
                ActivityId = input.ActivityId,
                UserId = input.UserId,
                Message = input.Message,
                MessageDateTime = DateTime.ParseExact(input.MessageDateTime, "yyyy-MM-dd HH:mm", null),
            });
        }

        /// <summary>
        /// Check whether the inputting message exists or not.
        /// </summary>
        /// <param name="messageInput">The inputting context of the message.</param>
        /// <returns>If message exists in the database , return true. Otherwise, return false.</returns>
        public bool IsMessageExist(MessagePost messageInput)
        {
            return SeeSeaTestContext.MessageBoards
                .Any(message => message.ActivityId == messageInput.ActivityId && message.UserId == messageInput.UserId && message.Message.Equals(messageInput.Message) && message.MessageDateTime.Equals(DateTime.ParseExact(messageInput.MessageDateTime, "yyyy-MM-dd HH:mm", null)));
        }

        /// <summary>
        /// Get the inputting id of the specific message.
        /// </summary>
        /// <param name="messageInput">The inputting context of the message.</param>
        /// <returns>If message exists in the database , return the id of the message. Otherwise, return false.</returns>
        public int GetMessageID(MessagePost messageInput)
        {
            return SeeSeaTestContext.MessageBoards
                .Where(message => message.ActivityId == messageInput.ActivityId && message.UserId == messageInput.UserId && message.Message.Equals(messageInput.Message) && message.MessageDateTime.Equals(DateTime.ParseExact(messageInput.MessageDateTime, "yyyy-MM-dd HH:mm", null)))
                .Select(message => message.MessageId)
                .SingleOrDefault();
        }

        /// <summary>
        /// Delete the specific Message.
        /// </summary>
        /// <param name="targetMessage">The specific of message.</param>
        public void DeleteMessage(MessageBoard targetMessage)
        {
            //從資料庫中刪除指定留言
            SeeSeaTestContext.MessageBoards.Remove(targetMessage);
        }

        /// <summary>
        /// Get specific user's favorite room list from the database named SeeSeaTest.
        /// </summary>
        /// <param name="userId">The searching UserID</param>
        /// <returns>The ActivityID list of user's favorite room list.</returns>
        public List<ActivityRoomUserRoomDto> GetUserFavoriteRoomList(int userId)
        {
            //從UserFavoriteActivity資料表中找出UserID與輸入的UserID相同的房間列表，判斷該UserID中的房間那些是正在進行中(揪團中或是人員已滿未宣告)，回傳輸出結果
            return GetRoomUserRoomDto(UserDataCenter.GetUserFavoriteRoomList(userId)).ToList();
        }

        /// <summary>
        /// Get specific user's participating room list from the database named SeeSeaTest.
        /// </summary>
        /// <param name="userId">The searching UserID</param>
        /// <returns>The ActivityID list of user's participating room list.</returns>
        public List<ActivityRoomUserRoomDto> GetUserParticipatingRoomList(int userId)
        {
            //從ActivityParticipant資料表中找出ApplicantID與輸入的UserID相同的房間列表，判斷該UserID中的房間那些是尚未出團的揪團活動，回傳輸出結果
            return GetRoomUserRoomDto(UserDataCenter.GetUserParticipatingRoomList(userId)).ToList();
        }

        /// <summary>
        /// Get specific user's signing up room list from the database named SeeSeaTest.
        /// </summary>
        /// <param name="userId">The searching UserID</param>
        /// <returns>The ActivityID list of user's signing up room list.</returns>
        public List<ActivityRoomUserRoomDto> GetUserSigningUpRoomList(int userId)
        {
            //從ActivityApplicant資料表中找出ApplicantID與輸入的UserID相同的房間列表，判斷該UserID中的房間那些是已出團的揪團活動，回傳輸出結果
            return GetRoomUserRoomDto(UserDataCenter.GetUserSigningUpRoomList(userId)).ToList();
        }

        /// <summary>
        /// Get specific user's ending room list from the database named SeeSeaTest.
        /// </summary>
        /// <param name="userId">The searching UserID</param>
        /// <returns>The ActivityID list of user's ending room list.</returns>
        public List<ActivityRoomUserRoomDto> GetUserEndingRoomList(int userId)
        {
            //從ActivityParticipant資料表中找出ApplicantID與輸入的UserID相同的房間列表，判斷該UserID中的房間那些是已出團的揪團活動，回傳輸出結果
            return GetRoomUserRoomDto(UserDataCenter.GetUserEndingRoomList(userId)).ToList();
        }

        /// <summary>
        /// Get all active Activity rooms from the database named SeeSeaTest.
        /// </summary>
        /// <returns>All the ActivityRoom.</returns>
        private IEnumerable<ActivityRoom> GetActiveRoom()
        {
            //回傳所有「未完成」的活動房間列表(未完成包含兩種狀態：揪團中以及滿員但尚未出團)
            return SeeSeaTestContext.ActivityRooms.Where(room => room.ActivityStatusCode == (byte)Global.ActivityStatus.PAIRING || room.ActivityStatusCode == (byte)Global.ActivityStatus.FULL);
        }

        /// <summary>
        /// Convert ActivityRoom to DTO form.
        /// </summary>
        /// <param name="input">The inputting ActivityRoom data.</param>
        /// <returns>The DTO form of ActivityRoom data.</returns>
        private IEnumerable<ActivityRoomDto> ChangeActivityRoomToDto(IEnumerable<ActivityRoom> input)
        {
            //先將ActivityRoom及UserBasicInfo兩者透過ActivityId Join起來，得到HostName及HostImage
            var activityRoom = input
                .Join(
                        SeeSeaTestContext.UserInfoes,
                        activityRoom => activityRoom.HostId,
                        hostUserInfo => hostUserInfo.UserId,
                        (activityRoom, hostUserInfo) => new
                        {
                            activityRoom.ActivityId,
                            activityRoom.HostId,
                            HostName = hostUserInfo.UserNickName ?? hostUserInfo.UserName,
                            HostImage = hostUserInfo.UserImage,
                            activityRoom.ActivityName,
                            activityRoom.ActivityStatusCode,
                            ActivityPicture = activityRoom.ActivityPicture ?? string.Empty,
                            activityRoom.CurrentParticipantNumber,
                            activityRoom.ParticipantNumber,
                            activityRoom.DivingTypeCode,
                            activityRoom.DivingLevelCode,
                            activityRoom.ActivityPropertyCode,
                            ActivityDate = activityRoom.ActivityDateTime.Date.ToString("yyyy-MM-dd, ddd"),
                            ActivityTime = activityRoom.ActivityDateTime.TimeOfDay.ToString()[..5],
                            activityRoom.ActivityAreaCode,
                            ActivityPlace = SeeSeaTestContext.DivingPoints.Where(point => point.DivingPointId == activityRoom.ActivityPlaceCode).Select(point => point.DivingPointName).SingleOrDefault(),
                            activityRoom.TransportationCode,
                            ActivityDescription = activityRoom.ActivityDescription ?? string.Empty,
                            MeetingPlace = activityRoom.MeetingPlace ?? string.Empty,
                            activityRoom.EstimateCostCode,
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
                .OrderBy(activityRoom => activityRoom.ActivityId)
                .AsEnumerable();

            //將資料轉換成DTO形式給前端
            var activityRoomDto = activityRoom
                .Select(
                            activityRoom => new ActivityRoomDto
                            {
                                ActivityId = activityRoom.ActivityId,
                                HostId = activityRoom.HostId,
                                HostName = activityRoom.HostName,
                                HostImage = activityRoom.HostImage,
                                ActivityName = activityRoom.ActivityName,
                                ActivityStatusCode = activityRoom.ActivityStatusCode,
                                ActivityPicture = activityRoom.ActivityPicture,
                                CurrentParticipantNumber = activityRoom.CurrentParticipantNumber,
                                ParticipantNumber = activityRoom.ParticipantNumber,
                                DivingType = Global.GetDescription((Global.DivingType)Enum.ToObject(typeof(Global.DivingType), activityRoom.DivingTypeCode)),
                                DivingLevel = Global.GetDescription((Global.DivingLevel)Enum.ToObject(typeof(Global.DivingLevel), activityRoom.DivingLevelCode)),
                                ActivityProperty = Global.GetDescription((Global.ActivityProperty)Enum.ToObject(typeof(Global.ActivityProperty), activityRoom.ActivityPropertyCode)),
                                ActivityDate = activityRoom.ActivityDate,
                                ActivityTime = activityRoom.ActivityTime,
                                ActivityArea = Global.GetDescription((Global.ActivityArea)Enum.ToObject(typeof(Global.ActivityArea), activityRoom.ActivityAreaCode)),
                                ActivityPlace = activityRoom.ActivityPlace,
                                Transportation = Global.GetDescription((Global.Transportation)Enum.ToObject(typeof(Global.Transportation), activityRoom.TransportationCode)),
                                ActivityDescription = activityRoom.ActivityDescription,
                                MeetingPlace = activityRoom.MeetingPlace,
                                EstimateCost = Global.GetDescription((Global.EstimateCost)Enum.ToObject(typeof(Global.EstimateCost), activityRoom.EstimateCostCode)),
                                MessageBoard = ChangeMessageToDto(activityRoom.Message).ToList(),
                                ApplicantImage = activityRoom.ApplicantImage.ToList() ?? new List<string>(),
                                FavoriteNumber = (byte)activityRoom.FavoriteNumber
                            });

            //將結果回傳
            return activityRoomDto;
        }

        /// <summary>
        /// Convert ActivityRoom to DTO form. (Simplify)
        /// </summary>
        /// <param name="input">The inputting ActivityRoom data.</param>
        /// <returns>The DTO form of ActivityRoom data.</returns>
        private IEnumerable<ActivityRoomUserRoomDto> ChangeActivityRoomToUserRoomDto(IEnumerable<ActivityRoom> input)
        {
            //先將ActivityRoom及UserBasicInfo兩者透過ActivityId Join起來，得到HostName及HostImage
            var activityRoom = input
                .Join(
                        SeeSeaTestContext.DivingPoints,
                        activityRoom => activityRoom.ActivityPlaceCode,
                        divingPoint => divingPoint.DivingPointId,
                        (activityRoom, divingPoint) => new
                        {
                            activityRoom.ActivityId,
                            activityRoom.HostId,
                            activityRoom.ActivityName,
                            activityRoom.ActivityStatusCode,
                            activityRoom.DivingTypeCode,
                            ActivityPlace = divingPoint.DivingPointName
                        })
                .OrderBy(activityRoom => activityRoom.ActivityId)
                .AsEnumerable();

            //將資料轉換成DTO形式(Simplify)給前端
            var activityRoomSimplifyDto = activityRoom
                .Select(
                            activityRoom => new ActivityRoomUserRoomDto
                            {
                                ActivityId = activityRoom.ActivityId,
                                HostId = activityRoom.HostId,
                                ActivityName = activityRoom.ActivityName,
                                ActivityStatusCode = activityRoom.ActivityStatusCode,
                                DivingType = Global.GetDescription((Global.DivingType)Enum.ToObject(typeof(Global.DivingType), activityRoom.DivingTypeCode)),
                                ActivityPlace = activityRoom.ActivityPlace
                            });

            //將結果回傳
            return activityRoomSimplifyDto;
        }

        /// <summary>
        /// Convert ActivityRoom to DTO form. (for DivingPoint List)
        /// </summary>
        /// <param name="input">The inputting ActivityRoom data.</param>
        /// <returns>The DTO form of ActivityRoom data. (for DivingPoint List)</returns>
        private static IEnumerable<ActivityRoomDivingPointDto> ChangeActivityRoomToDivingPointDto(IEnumerable<ActivityRoom> input)
        {
            //先將ActivityRoom及UserBasicInfo兩者透過ActivityId Join起來，得到HostName及HostImage
            var activityRoom = input
                .OrderBy(activityRoom => activityRoom.ActivityId)
                .AsEnumerable();

            //將資料轉換成DTO形式(DivingPoint)給前端
            var activityRoomDivingPointDto = activityRoom
                .Select(
                            activityRoom => new ActivityRoomDivingPointDto
                            {
                                ActivityId = activityRoom.ActivityId,
                                ActivityName = activityRoom.ActivityName,
                                ActivityStatusCode = activityRoom.ActivityStatusCode,
                                DivingType = Global.GetDescription((Global.DivingType)Enum.ToObject(typeof(Global.DivingType), activityRoom.DivingTypeCode)),
                                ActivityProperty = Global.GetDescription((Global.ActivityProperty)Enum.ToObject(typeof(Global.ActivityProperty), activityRoom.ActivityPropertyCode))
                            });

            //將結果回傳
            return activityRoomDivingPointDto;
        }

        /// <summary>
        /// Convert Message to DTO form.
        /// </summary>
        /// <param name="input">The inputting Message data.</param>
        /// <returns>The DTO form of message data.</returns>
        private IEnumerable<MessageDto> ChangeMessageToDto(IEnumerable<MessageBoard> input)
        {
            //先將ActivityRoom及UserBasicInfo兩者透過ActivityId Join起來，得到HostName及HostImage，以留言時間排序
            var message = input
                .Join(
                        SeeSeaTestContext.UserInfoes,
                        message => message.UserId,
                        userInfo => userInfo.UserId,
                        (message, userInfo) => new MessageDto
                        {
                            MessageId = message.MessageId,
                            UserId = message.UserId,
                            UserName = userInfo.UserNickName ?? userInfo.UserName,
                            UserImage = userInfo.UserImage,
                            Message = message.Message,
                            MessageDateTime = message.MessageDateTime.ToString("yyyy-MM-dd HH:mm"),
                        })
                .OrderBy(message => message.MessageDateTime);

            //將結果回傳
            return message;
        }
    }
}