//-----------------------------------------------------------------------
// <copyright file="UserDataCenter.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.DataCenter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using GroupPairing_API.Dtos;
    using GroupPairing_API.Models.Db;
    using GroupPairing_API.Parameters;

    /// <summary>
    /// The Algorithmic logic of the user's data about UserInfo_API.
    /// </summary>
    public class UserDataCenter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataCenter" /> class.
        /// </summary>
        /// <param name="seeSeaTestContext">All the information about the datasheets from the database named SeeSeaTest.</param>
        public UserDataCenter(SeeSeaTestContext seeSeaTestContext)
        {
            SeeSeaTestContext = seeSeaTestContext;
        }

        /// <summary>
        /// Gets the SeeSeaContext class.
        /// </summary>
        public SeeSeaTestContext SeeSeaTestContext { get; }

        /// <summary>
        /// Check the inputting account whether existing in the database named SeeSeaTest.
        /// </summary>
        /// <param name="account">The account needs to be checked.</param>
        /// <returns>If the inputting account exists in the database, return true. Otherwise, return false.</returns>
        public bool IsAccountExist(string account)
        {
            //判斷輸入帳號是否已存在於Db之中
            return SeeSeaTestContext.UserInfoes.Where(user => user.UserAccount.Equals(account)).Any();
        }

        /// <summary>
        /// Check the inputting account whether existing in the database named SeeSeaTest.
        /// </summary>
        /// <param name="mailAddress">The Email needs to be checked.</param>
        /// <returns>If the inputting Email exists in the database, return true. Otherwise, return false.</returns>
        public bool IsEmailExist(string mailAddress)
        {
            //判斷輸入Email是否已存在於Db之中
            return SeeSeaTestContext.UserInfoes.Where(user => user.UserEmail.Equals(mailAddress)).Any();
        }

        /// <summary>
        /// Check the inputting phone number whether existing in the database named SeeSeaTest.
        /// </summary>
        /// <param name="phoneNumber">The phone number needs to be checked.</param>
        /// <returns>If the inputting phone number exists in the database, return true. Otherwise, return false.</returns>
        public bool IsPhoneExist(int phoneNumber)
        {
            //判斷輸入電話號碼是否已存在於Db之中
            return SeeSeaTestContext.UserInfoes.Where(user => user.UserPhone == phoneNumber).Any();
        }

        /// <summary>
        /// Post a new UserInfo into the database named SeeSeaTest.
        /// </summary>
        /// <param name="userInfoPost">The information needed to post into the database.</param>
        public void PostUserInfo(UserInfoPost userInfoPost)
        {
            //在資料庫中新增一筆UserIndo資料
            SeeSeaTestContext.UserInfoes.Add(new UserInfo
            {
                UserAccount = userInfoPost.UserAccount,
                UserPassword = userInfoPost.UserPassword,
                UserName = userInfoPost.UserName,
                UserNickName = userInfoPost.UserNickName,
                UserAge = userInfoPost.UserAge,
                UserPhone = userInfoPost.UserPhone,
                UserEmail = userInfoPost.UserEmail,
                UserEmailId = Global.GetMd5Method(userInfoPost.UserEmail),
                Approved = false,
                UserExperienceCode = userInfoPost.UserExperienceCode,
                UserDescription = userInfoPost.UserDescription,
                DivingTypeTag = userInfoPost.DivingTypeTag,
                AreaTag = userInfoPost.AreaTag
            });
        }

        /// <summary>
        /// Activate user's account.
        /// </summary>
        /// <param name="emailId">The querying Id of the email.</param>
        public void SetAccountActive(string emailId)
        {
            //根據EmailID搜尋指定User
            UserInfo userInfo = this.SeeSeaTestContext.UserInfoes.Where(user => user.UserEmailId.Equals(emailId)).SingleOrDefault();

            //若該User存在，則啟動帳號權限
            if (userInfo != null)
            {
                userInfo.Approved = true;
            }
        }

        /// <summary>
        /// Send validation email.
        /// </summary>
        /// <param name="userId">The specific user that need to activate his account.</param>
        public void SendValidationEmail(int userId)
        {
            //根據UserID查找特定使用者
            UserInfo user = this.GetUserInfo(userId);

            if (user != null)
            {
                //初始化MailMessage
                MailMessage msg = new MailMessage();

                //增加寄件人
                msg.To.Add($"{user.UserEmail}");

                //上面3個引數分別是發件人地址（可以隨便寫），發件人姓名，編碼
                msg.From = new MailAddress("SeeSee@net.com", "SeeSea特派員", System.Text.Encoding.UTF8);

                //郵件標題
                msg.Subject = "SeeSea帳號驗證信";

                //郵件標題編碼 
                msg.SubjectEncoding = System.Text.Encoding.UTF8;

                //郵件內容 
                msg.Body = $@"
                                <h2 style= 'color: black'>親愛的{user.UserNickName}您好：</h2>
                                <p style= 'color: black'>感謝您的註冊 🙌，我們想確認您所輸入的註冊信箱是正確的。<br> 
                                點擊下方連結完成信箱驗證，即可馬上啟用 SeeSea 帳號喔！<br> 
                                <a href= http://35.221.136.55/EmailValidation/{user.UserEmailId} >帳號驗證</a> </p> 
                                <img width='140' height='180' src='http://35.221.136.55/image/SeeSeaLogo.jpg' alt=''>";

                //郵件內容編碼 
                msg.BodyEncoding = System.Text.Encoding.UTF8;

                //是否是HTML郵件 
                msg.IsBodyHtml = true;

                //郵件優先順序 
                msg.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();

                //寫你的GMail郵箱和密碼 
                client.Credentials = new NetworkCredential("", "");

                //Gmail使用的埠 
                client.Port = 587;
                client.Host = "smtp.gmail.com";

                //經過ssl加密 
                client.EnableSsl = true;
                object userState = msg;

                //寄送Email
                client.Send(msg);
            }
        }

        /// <summary>
        /// Reset the password of the specific account.
        /// </summary>
        /// <param name="userInfo">The inputting UserInfo.</param>
        /// <param name="newPassword">The new setting password.</param>
        public void ResetPassword(UserInfo userInfo, string newPassword)
        {
            //透過MD5加密將密碼轉換成暗碼，同時覆寫該使用者原先密碼
            userInfo.UserPassword = Global.GetMd5Method(newPassword);
        }

        /// <summary>
        /// Send validation email.
        /// </summary>
        /// <param name="userId">The specific user that need to activate his account.</param>
        /// <param name="newPassword">The new setting password.</param>
        public void SendPasswordEmail(int userId, string newPassword)
        {
            //根據UserID查找特定使用者
            UserInfo user = this.GetUserInfo(userId);

            if (user != null)
            {
                //初始化MailMessage
                MailMessage msg = new MailMessage();

                //增加寄件人
                msg.To.Add($"{user.UserEmail}");

                //上面3個引數分別是發件人地址（可以隨便寫），發件人姓名，編碼
                msg.From = new MailAddress("SeeSee@net.com", "SeeSea特派員", System.Text.Encoding.UTF8);

                //郵件標題
                msg.Subject = "SeeSea密碼重置提醒";

                //郵件標題編碼 
                msg.SubjectEncoding = System.Text.Encoding.UTF8;

                //郵件內容 
                msg.Body = $@"
                                <h2 style= 'color: black'>親愛的{user.UserNickName}您好：</h2>
                                <p style= 'color: black'>您的新密碼為： {newPassword} <br> 
                                使用新密碼登入後，請盡速至會員中心更改您的密碼唷！<br> 
                                <img width='140' height='180' src='http://35.221.136.55/image/SeeSeaLogo.jpg' alt=''>";

                //郵件內容編碼 
                msg.BodyEncoding = System.Text.Encoding.UTF8;

                //是否是HTML郵件 
                msg.IsBodyHtml = true;

                //郵件優先順序 
                msg.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();

                //寫你的GMail郵箱和密碼 
                client.Credentials = new NetworkCredential("", "");

                //Gmail使用的埠 
                client.Port = 587;
                client.Host = "smtp.gmail.com";

                //經過ssl加密 
                client.EnableSsl = true;
                object userState = msg;

                //寄送Email
                client.Send(msg);
            }
        }

        /// <summary>
        /// Check the inputting account and password whether they are correct or not.
        /// </summary>
        /// <param name="userAccount">The inputting account.</param>
        /// <param name="userPassword">The inputting password.</param>
        /// <returns>If the inputting account and password is correct, return true. Otherwise, return false.</returns>
        public bool IsValidLogin(string userAccount, string userPassword)
        {
            // 驗證帳號密碼是否正確，若正確則回傳true
            return SeeSeaTestContext.UserInfoes.Any(user => user.UserAccount.Equals(userAccount) && user.UserPassword.Equals(userPassword));
        }

        /// <summary>
        /// Check the inputting UserID whether it exists or not.
        /// </summary>
        /// <param name="userId">The inputting userID.</param>
        /// <returns>If the inputting userId exists, return true. Otherwise, return false.</returns>
        public bool IsUserIDExist(int userId)
        {
            // 驗證此UserID是否存在於UserInfo資料表中
            return this.SeeSeaTestContext.UserInfoes.Where(user => user.UserId == userId).Any();
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
        /// Gets the UserInfo by specific emailID.
        /// </summary>
        /// <param name="emailID">The querying emailID.</param>
        /// <returns>The querying UserInfo.</returns>
        public UserInfo GetUserInfo(string emailID)
        {
            //return與輸入userID相同的UserInfo
            UserInfo userInfo = SeeSeaTestContext.UserInfoes
                .Where(user => user.UserEmailId == emailID)
                .SingleOrDefault();
            return userInfo;
        }

        /// <summary>
        /// Get UserInfoDTO of the specific userId by inputting userAccount and userPassword.
        /// </summary>
        /// <param name="userAccount">The user's account.</param>
        /// <param name="userPassword">The user's password corresponding to user's account.</param>
        /// <returns>The DTO of UserInfo</returns>
        public UserInfoDto GetUserInfoDto(string userAccount, string userPassword)
        {
            // 驗證帳號密碼是否正確，若正確則return UserInfoDTO
            UserInfo userInfo = SeeSeaTestContext.UserInfoes
                .Where(user => user.UserAccount.Equals(userAccount) && user.UserPassword.Equals(userPassword))
                .SingleOrDefault();

            if (userInfo == null)
            {
                return null;
            }

            return this.ConvertUnserInfoToUserInfoDto(userInfo);
        }

        /// <summary>
        /// Get UserInfoDTO of the specific userId.
        /// </summary>
        /// <param name="userId">The userId needs to be checked.</param>
        /// <returns>UserInfoDTO needs to send to frontend.</returns>
        public UserInfoDto GetUserInfoDto(int userId)
        {
            // 回傳指定UserId的UserInfoDto資料
            return this.ConvertUnserInfoToUserInfoDto(this.GetUserInfo(userId));
        }

        /// <summary>
        /// Gets the UserInfo by specific userAccount.
        /// </summary>
        /// <param name="userAccount">The querying userID.</param>
        /// <returns>The querying UserInfo.</returns>
        public UserInfo GetUserInfoByAccount(string userAccount)
        {
            //return與輸入userID相同的UserInfo
            UserInfo userInfo = SeeSeaTestContext.UserInfoes
                .Where(user => user.UserAccount == userAccount)
                .SingleOrDefault();
            return userInfo;
        }

        /// <summary>
        /// Check the inputting userID and password whether they are correct or not.
        /// </summary>
        /// <param name="parameter">The inputting parameter.</param>
        /// <returns>If the inputting userId and password is correct, return true. Otherwise, return false.</returns>
        public bool IsPasswordCorrect(CheckPasswordParameter parameter)
        {
            // 驗證帳號密碼是否正確，若正確則回傳true
            return SeeSeaTestContext.UserInfoes.Any(user => user.UserId == parameter.UserId && user.UserPassword.Equals(parameter.Password));
        }

        /// <summary>
        /// Update UserInfo with the specific UserId.
        /// </summary>
        /// <param name="userInfo">The UserInfo needed to be modified.</param>
        /// <param name="userInfoInput">The information for updating.</param>
        public void SetUserInfo(UserInfo userInfo, UserInfoPut userInfoInput, string dateString)
        {
            //更新使用者內容，若該欄位使用者未輸入，則沿用原先資料
            userInfo.UserPassword = userInfoInput.UserPassword ?? userInfo.UserPassword;
            userInfo.UserNickName = userInfoInput.UserNickName ?? userInfo.UserNickName;
            userInfo.UserPhone = userInfoInput.UserPhone ?? userInfo.UserPhone;

            //帳號未驗證才允許修改Email，並驗證修改過後的Email是否已存在於db中，重新寄發驗證信，否則不允許修改Email
            if (!userInfo.Approved && !string.IsNullOrEmpty(userInfoInput.UserEmail) && !this.IsEmailExist(userInfoInput.UserEmail))
            {
                userInfo.UserEmail = userInfoInput.UserEmail;
                userInfo.UserEmailId = Global.GetMd5Method(userInfoInput.UserEmail);
            }

            //若使用者經驗代號沒有落在設定區間中，則不更新使用者經驗代號
            if (userInfoInput.UserExperienceCode != null && userInfoInput.UserExperienceCode >= (int)Global.UserExperience.NO_EXPERIENCE && userInfoInput.UserExperienceCode <= (int)Global.UserExperience.VETERAN)
            {
                userInfo.UserExperienceCode = userInfoInput.UserExperienceCode ?? userInfo.UserExperienceCode;
            }

            //更新使用者內容，若該欄位使用者未輸入，則沿用原先資料
            if(userInfoInput.UserImage != null)
            {
                userInfo.UserImage = !userInfoInput.UserImage.FileName.Equals("profilePic.jpg")  ? $@"http://35.221.136.55/image/{dateString}{userInfoInput.UserImage.FileName}" : userInfo.UserImage;
            }
            userInfo.UserDescription = userInfoInput.UserDescription ?? userInfo.UserDescription;
            userInfo.DivingTypeTag = userInfoInput.DivingTypeTag ?? userInfo.DivingTypeTag;
            userInfo.AreaTag = userInfoInput.AreaTag ?? userInfo.AreaTag;
        }

        /// <summary>
        /// Post a new UserFavoriteActivity.
        /// </summary>
        /// <param name="userId">The inputting userId.</param>
        /// <param name="activityId">The inputting activityId.</param>
        public void PostUserFavoriteActivity(int userId, int activityId)
        {
            //在UserFavoriteActivity資料表中新增一筆UserFavoriteActivity
            SeeSeaTestContext.UserFavoriteActivities.Add(new UserFavoriteActivity
            {
                UserId = userId,
                FavoriteActivityId = activityId
            });
        }

        /// <summary>
        /// Get the specific UserFavoriteActivity.
        /// </summary>
        /// <param name="userId">The inputting userId.</param>
        /// <param name="activityId">The inputting activityId.</param>
        /// <returns>The specific UserFavoriteActivity.</returns>
        public UserFavoriteActivity GetUserFavoriteActivity(int userId, int activityId)
        {
            //以輸入條件查找特定的UserFavoriteActivity
            return SeeSeaTestContext.UserFavoriteActivities
                .Where(user => user.UserId == userId && user.FavoriteActivityId == activityId)
                .SingleOrDefault();
        }

        /// <summary>
        /// Get specific user's favorite room list from the database named SeeSeaTest.
        /// </summary>
        /// <param name="userId">The searching UserID</param>
        /// <returns>The ActivityID list of user's favorite room list.</returns>
        public List<int> GetUserFavoriteRoomList(int userId)
        {
            //從UserFavoriteActivity資料表中找出UserID與輸入的UserID相同的房間列表，判斷該UserID中的房間那些是正在進行中(揪團中或是人員已滿未宣告)
            List<int> favoriteRoomList = SeeSeaTestContext.UserFavoriteActivities
                .Join(
                SeeSeaTestContext.ActivityRooms,
                    favoriteRoom => favoriteRoom.FavoriteActivityId,
                    activityRoom => activityRoom.ActivityId,
                    (favoriteRoom, activityRoom) => new
                    {
                        ActivityId = favoriteRoom.FavoriteActivityId,
                        UserId = favoriteRoom.UserId,
                        ActivityStatusCode = activityRoom.ActivityStatusCode
                    })
            .Where(roomList => roomList.UserId == userId && (roomList.ActivityStatusCode == (int)Global.ActivityStatus.PAIRING || roomList.ActivityStatusCode == (int)Global.ActivityStatus.FULL || roomList.ActivityStatusCode == (int)Global.ActivityStatus.CONFIRMED))
            .Select(roomList => roomList.ActivityId)
            .ToList();

            //回傳輸出結果
            return favoriteRoomList;
        }

        /// <summary>
        /// Get specific user's signing up room list from the database named SeeSeaTest.
        /// </summary>
        /// <param name="userId">The searching UserID</param>
        /// <returns>The ActivityID list of user's signing up room list.</returns>
        public List<int> GetUserSigningUpRoomList(int userId)
        {
            //從ActivityParticipant資料表中找出ApplicantID與輸入的UserID相同的房間列表，判斷該UserID中的房間那些是已出團的揪團活動
            var signingUpRoomList = SeeSeaTestContext.ActivityApplicants
                 .Join(
                        SeeSeaTestContext.ActivityRooms,
                        signingUpRoomList => signingUpRoomList.ActivityId,
                        activityRoom => activityRoom.ActivityId,
                        (signingUpRoomList, activityRoom) => new
                        {
                            ActivityId = activityRoom.ActivityId,
                            UserId = signingUpRoomList.ApplicantId,
                            ActivityStatusCode = activityRoom.ActivityStatusCode
                        })
                .Where(roomList => roomList.UserId == userId && (roomList.ActivityStatusCode == (int)Global.ActivityStatus.PAIRING || roomList.ActivityStatusCode == (int)Global.ActivityStatus.FULL || roomList.ActivityStatusCode == (int)Global.ActivityStatus.CONFIRMED))
                .Select(roomList => roomList.ActivityId)
                .ToList();

            //回傳輸出結果
            return signingUpRoomList;
        }

        /// <summary>
        /// Get specific user's participating room list from the database named SeeSeaTest.
        /// </summary>
        /// <param name="userId">The searching UserID</param>
        /// <returns>The ActivityID list of user's participating room list.</returns>
        public List<int> GetUserParticipatingRoomList(int userId)
        {
            //建立輸出結果
            List<int> result = new List<int>();

            //從ActiviryRoom資料表中找出該UserID主辦之房間列表，判斷該UserID中的房間那些是正在進行中(揪團中或是人員已滿未宣告)
            var hostingRoomList = SeeSeaTestContext.ActivityRooms
            .Where(roomList => roomList.HostId == userId && (roomList.ActivityStatusCode == (int)Global.ActivityStatus.PAIRING || roomList.ActivityStatusCode == (int)Global.ActivityStatus.FULL || roomList.ActivityStatusCode == (int)Global.ActivityStatus.CONFIRMED))
            .Select(roomList => roomList.ActivityId);

            //從ActivityParticipant資料表中找出該UserID參加的的房間列表，判斷該UserID中的房間那些是正在進行中(揪團中或是人員已滿未宣告)
            var participatingRoomList = SeeSeaTestContext.ActivityParticipants
                .Join(
                        SeeSeaTestContext.ActivityRooms,
                        participatingRoomList => participatingRoomList.ActivityId,
                        activityRoom => activityRoom.ActivityId,
                        (participatingRoomList, activityRoom) => new
                        {
                            ActivityId = activityRoom.ActivityId,
                            UserId = participatingRoomList.ParticipantId,
                            ActivityStatusCode = activityRoom.ActivityStatusCode
                        })
                .Where(roomList => roomList.UserId == userId && (roomList.ActivityStatusCode == (int)Global.ActivityStatus.PAIRING || roomList.ActivityStatusCode == (int)Global.ActivityStatus.FULL || roomList.ActivityStatusCode == (int)Global.ActivityStatus.CONFIRMED))
                .Select(roomList => roomList.ActivityId);

            //將主辦人清單與參加者清單串接起來
            result = hostingRoomList.Concat(participatingRoomList).ToList();

            //回傳輸出結果
            return result;
        }

        /// <summary>
        /// Get specific user's ending room list from the database named SeeSeaTest.
        /// </summary>
        /// <param name="userId">The searching UserID</param>
        /// <returns>The ActivityID list of user's ending room list.</returns>
        public List<int> GetUserEndingRoomList(int userId)
        {
            //建立輸出結果
            List<int> result = new List<int>();

            //從ActiviryRoom資料表中找出該UserID主辦之房間列表，判斷該UserID中的房間那些是正在進行中(揪團中或是人員已滿未宣告)
            var hostingRoomList = SeeSeaTestContext.ActivityRooms
                .Where(roomList => roomList.HostId == userId && (roomList.ActivityStatusCode == (int)Global.ActivityStatus.DONE && roomList.ActivityStatusCode == (int)Global.ActivityStatus.FAIL_PAIRING))
                .Select(roomList => roomList.ActivityId)
                .ToList();

            //從ActivityParticipant資料表中找出該UserID參加的的房間列表，判斷該UserID中的房間那些是正在進行中(揪團中或是人員已滿未宣告)
            var participatingRoomList = SeeSeaTestContext.ActivityParticipants
                .Join(
                        SeeSeaTestContext.ActivityRooms,
                        participatingRoomList => participatingRoomList.ActivityId,
                        activityRoom => activityRoom.ActivityId,
                        (participatingRoomList, activityRoom) => new
                        {
                            ActivityId = activityRoom.ActivityId,
                            UserId = participatingRoomList.ParticipantId,
                            ActivityStatusCode = activityRoom.ActivityStatusCode
                        })
                .Where(roomList => roomList.UserId == userId && (roomList.ActivityStatusCode == (int)Global.ActivityStatus.DONE && roomList.ActivityStatusCode == (int)Global.ActivityStatus.FAIL_PAIRING))
                .Select(roomList => roomList.ActivityId)
                .ToList();

            //將主辦人清單與參加者清單串接起來
            result = hostingRoomList.Concat(participatingRoomList).ToList();

            //回傳輸出結果
            return result;
        }

        /// <summary>
        /// Check the inputting ActivityId whether it exists or not.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <returns>If the inputting activityId exists, return true. Otherwise, return false.</returns>
        public bool IsActivityIDExist(int activityId)
        {
            // 驗證此ActivityID是否存在於ActivityRoom料表中
            return this.SeeSeaTestContext.ActivityRooms.Where(room => room.ActivityId == activityId).Any();
        }

        /// <summary>
        /// Convert UserInfo to DTO form.
        /// </summary>
        /// <param name="userInfo">The inputting UserInfo data.</param>
        /// <returns>The DTO form of UserInfo data.</returns>
        private UserInfoDto ConvertUnserInfoToUserInfoDto(UserInfo userInfo)
        {
            //將UserInfo資料轉換成DTO形式
            UserInfoDto userInfoDto = new UserInfoDto
            {
                UserId = userInfo.UserId,
                UserAccount = userInfo.UserAccount,
                UserName = userInfo.UserName,
                UserNickName = userInfo.UserNickName,
                UserAge = userInfo.UserAge,
                UserPhone = userInfo.UserPhone ?? 0,
                UserEmail = userInfo.UserEmail,
                IsAcccountActive = userInfo.Approved,
                UserExperience = Global.GetDescription((Global.UserExperience)Enum.ToObject(typeof(Global.UserExperience), userInfo.UserExperienceCode)),
                UserImage = userInfo.UserImage ?? string.Empty,
                UserDescription = userInfo.UserDescription ?? string.Empty,
                DivingTypeTag = new List<string>(),
                AreaTag = new List<string>(),
            };

            //將UserInfo中的偏好潛水類型從string轉成List<int>
            List<int> divingTypeCodes = Global.ConvertStringToIntList(userInfo.DivingTypeTag ?? string.Empty);

            //將偏好潛水類型轉成List<string>
            foreach (int divingTypeCode in divingTypeCodes)
            {
                if (divingTypeCode >= (byte)Global.DivingType.FREE && divingTypeCode <= (byte)Global.DivingType.SCUBA)
                {
                    userInfoDto.DivingTypeTag.Add(Global.GetDescription((Global.DivingType)Enum.ToObject(typeof(Global.DivingType), divingTypeCode)));
                }
            }

            //將UserInfo中的偏好潛水地區從string轉成List<int>
            List<int> areaCodes = Global.ConvertStringToIntList(userInfo.AreaTag ?? string.Empty);

            //將偏好潛水地區轉成List<string>
            foreach (int areaCode in areaCodes)
            {
                if (areaCode >= (byte)Global.ActivityArea.NORTH && areaCode <= (byte)Global.ActivityArea.LIUQIU)
                {
                    userInfoDto.AreaTag.Add(Global.GetDescription((Global.ActivityArea)Enum.ToObject(typeof(Global.ActivityArea), areaCode)));
                }
            }

            //將UserInofoDTO中的UserId存下
            int userID = userInfoDto.UserId;

            //建立使用者關注房間清單
            userInfoDto.UserFavoriteRoom = this.GetUserFavoriteRoomList(userID) ?? new List<int>();

            //建立使用者尚未出團的參加房間清單
            userInfoDto.UserParticipatingActivity = this.GetUserParticipatingRoomList(userID) ?? new List<int>();

            //建立使用者報名房間清單
            userInfoDto.UserSigningUpActivity = this.GetUserSigningUpRoomList(userID) ?? new List<int>();

            //建立使用者已出團的參加房間清單
            userInfoDto.UserFinishActivity = this.GetUserEndingRoomList(userID) ?? new List<int>();

            return userInfoDto;
        }
    }
}
