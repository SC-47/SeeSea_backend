//-----------------------------------------------------------------------
// <copyright file="UserInfoController.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using GroupPairing_API.DataCenter;
    using GroupPairing_API.Dtos;
    using GroupPairing_API.Models.Db;
    using GroupPairing_API.Parameters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The WebAPI controller related to UserInfo.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        /// <summary>
        /// The Algorithmic logic of the user's data about UserInfo_API.
        /// </summary>
        private readonly UserDataCenter DataCenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoController" /> class.
        /// </summary>
        /// <param name="dataCenter">The Algorithmic logic of the data about UserInfo_API.</param>
        public UserInfoController(UserDataCenter dataCenter)
        {
            this.DataCenter = dataCenter;
        }

        // POST api/<UserInfoController>

        /// <summary>
        /// Post a new UserInfo into the database named SeeSeaTest.
        /// </summary>
        /// <param name="userInfoPost">The inputting information for posting.</param>
        /// <returns>If successful, return "Created". On the contrary, return the corresponding error message.</returns>
        [HttpPost]
        public ActionResult PostUserInfo([FromBody] UserInfoPost userInfoPost)
        {
            //確認輸入帳號是否已存在於資料庫中
            if (this.DataCenter.IsAccountExist(userInfoPost.UserAccount))
            {
                return this.BadRequest("此帳號已被使用");
            }

            //確認輸入Email是否已存在於資料庫中
            if (this.DataCenter.IsEmailExist(userInfoPost.UserEmail))
            {
                return this.BadRequest("此信箱已被使用");
            }

            ////確認輸入電話號碼是否已存在於資料庫中
            //if (this.DataCenter.IsPhoneExist(userInfoPost.UserPhone))
            //{
            //    return this.BadRequest("此電話號碼已被使用");
            //}

            //將密碼透過Md5進行加密動作
            userInfoPost.UserPassword = Global.GetMd5Method($"{userInfoPost.UserPassword}");

            //利用Validation Attribute先驗證輸入資料是否符合規範，在判斷是否能順利寫入Db中，可以回傳"新增成功"(201)，無法寫入則回傳"輸入錯誤，無法新增資料"(500)
            try
            {
                this.DataCenter.PostUserInfo(userInfoPost);
                this.DataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法新增資料");
            }

            //寄送帳號驗證信給新辦帳號使用者
            UserInfo user = this.DataCenter.GetUserInfo(Global.GetMd5Method(userInfoPost.UserEmail));

            if (user != null)
            {
                this.DataCenter.SendValidationEmail(user.UserId);
            }

            return this.StatusCode((int)HttpStatusCode.Created, "新增成功");
        }

        // GET: api/<UserInfoController>

        /// <summary>
        /// Check whether the inputting account is valid or not.
        /// </summary>
        /// <param name="account">The account needs to be confirmed.</param>
        /// <returns>If existing, return false. On the contrary, return true.</returns>
        [HttpGet("CheckUserAccount")]
        public bool CheckAccountValidation(string account)
        {
            //判斷輸入帳號是否已存在在資料庫中，若已存在則不允許使用者使用該帳號，回傳false
            return !this.DataCenter.IsAccountExist(account);
        }

        // GET: api/<UserInfoController>

        /// <summary>
        /// Check whether the inputting Email is valid or not.
        /// </summary>
        /// <param name="mailAddress">The Email needs to be confirmed.</param>
        /// <returns>If existing, return false. On the contrary, return true.</returns>
        [HttpGet("CheckUserEmail")]
        public bool CheckEmailValidation(string mailAddress)
        {
            //判斷輸入Email是否已存在在資料庫中，若已存在則不允許使用者使用該Email，回傳false
            return !this.DataCenter.IsEmailExist(mailAddress);
        }

        // GET: api/<UserInfoController>

        /// <summary>
        /// Check whether the inputting phone number is valid or not.
        /// </summary>
        /// <param name="phoneNumber">The phone number needs to be confirmed.</param>
        /// <returns>If existing, return false. On the contrary, return true.</returns>
        [HttpGet("CheckUserPhone")]
        public bool CheckPhoneValidation(int phoneNumber)
        {
            //判斷輸入電話號碼是否已存在在資料庫中，若已存在則不允許使用者使用該電話號碼l，回傳false
            return !this.DataCenter.IsPhoneExist(phoneNumber);
        }

        // Post: api/<UserInfoController>

        /// <summary>
        /// Check whether the inputting information for login is correct.
        /// </summary>
        /// <param name="logIn">Login's information.</param>
        /// <returns>If the information is correct, return true. Otherwise, return false.</returns>
        [HttpPost("Login/Validation")]
        public bool IsAccountAndPassWordCorrect([FromBody] Login logIn)
        {
            //將輸入的密碼透過Md5轉換後，再與資料庫暗碼比對，若帳號密碼正確，回傳true，反之回傳false
            return this.DataCenter.IsValidLogin(logIn.Account, Global.GetMd5Method($"{logIn.Password}"));
        }

        // Post: api/<UserInfoController>

        /// <summary>
        /// Get the specific user's UserInfo by inputting account and inputting password.
        /// </summary>
        /// <param name="logIn">Login's information.</param>
        /// <returns>If the information exist, return the data. Except for existing , return the string descripting the detail of error.</returns>
        [HttpPost("Login")]
        public ActionResult CheckAccountAndPassWord([FromBody] Login logIn)
        {
            //將輸入的密碼透過Md5轉換後，再與資料庫暗碼比對
            UserInfoDto result = this.DataCenter.GetUserInfoDto(logIn.Account, Global.GetMd5Method($"{logIn.Password}"));

            //若帳號密碼正確，回傳使用者資料，否則顯示"帳號密碼輸入錯誤"
            return result != null ? this.Ok(result) : this.BadRequest("帳號密碼輸入錯誤");
        }

        // GET: api/<UserInfoController>

        /// <summary>
        /// Get the UserInfoDTO by the specific UserID.
        /// </summary>
        /// <param name="userId">The inputting userId</param>
        /// <returns>If the information exist, return the data. Except for existing , return the string descripting the detail of error.</returns>
        [HttpGet("{userId}")]
        public ActionResult Get(int userId)
        {
            //搜尋特定UserId的UserInfoDto
            UserInfoDto result = this.DataCenter.GetUserInfoDto(userId);

            //若帳號密碼正確，回傳使用者資料，否則顯示"帳號密碼輸入錯誤"
            return result != null ? this.Ok(result) : this.NotFound("查無此UserID");
        }

        // PUT api/<UserInfoController>

        /// <summary>
        /// Reset the password of the specific account.
        /// </summary>
        /// <param name="userAccount">The specific UserAccount needing to be modified.</param>
        /// <param name="userEmail">The parameters for putting UserInfo.</param>
        /// <returns>If successful, return Ok. On the contrary, return the corresponding error message.</returns>
        [HttpPut("UserAccount/{userAccount}/PasswordForgotten")]
        public ActionResult ResetAccountPassword(string userAccount, string userEmail)
        {
            //取得指定UserID的UserInfo
            var userInfo = this.DataCenter.GetUserInfoByAccount(userAccount);

            //判斷該UserID是否存在於資料庫之中
            if (userInfo == null)
            {
                return this.NotFound("查無使用者代號");
            }

            if (!userInfo.UserEmail.Equals(userEmail))
            {
                return this.BadRequest("資料輸入錯誤，拒絕該請求");
            }

            //判斷是否能順利寫入Db中，可以回傳"新增成功"(201)，無法寫入則回傳"輸入錯誤，無法新增資料"(500)
            try
            {
                string newPassword = Global.GetRandomPassword();
                this.DataCenter.ResetPassword(userInfo, newPassword);
                this.DataCenter.SeeSeaTestContext.SaveChanges();
                this.DataCenter.SendPasswordEmail(userInfo.UserId, newPassword);
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("發生錯誤");
            }

            return this.Ok("已寄送密碼修改信");
        }

        // Post: api/<UserInfoController>

        /// <summary>
        /// Check whether the inputting information for login is correct.
        /// </summary>
        /// <param name="parameter">The inputting information.</param>
        /// <returns>If the information is correct, return true. Otherwise, return false.</returns>
        [HttpPost("Login/PasswordCorrection")]
        public bool IsOldPasswordCorrect([FromBody] CheckPasswordParameter parameter)
        {
            //將輸入的密碼透過Md5轉換後
            parameter.Password = Global.GetMd5Method(parameter.Password);

            //若使用者ID與密碼正確，回傳true，反之回傳false
            return this.DataCenter.IsPasswordCorrect(parameter);
        }

        // PUT api/<UserInfoController>

        /// <summary>
        /// Put the specific UserInfo (with picture) by UserId.
        /// </summary>
        /// <param name="userId">The specific UserId needing to be modified.</param>
        /// <param name="para">The parameters for putting UserInfo.</param>
        /// <returns>If successful, return Ok. On the contrary, return the corresponding error message.</returns>
        [HttpPut("UserId/{userId}")]
        public async Task<IActionResult> Put(int userId, [FromForm] UserInfoPut para)
        {
            //取得指定UserID的UserInfo
            var userInfo = this.DataCenter.GetUserInfo(userId);

            //判斷該UserID是否存在於資料庫之中
            if (userInfo == null)
            {
                return this.NotFound("查無使用者代號");
            }

            //若使用者想變更密碼，則透過MD5加密將密碼轉換成暗碼
            if (para.UserPassword != null)
            {
                para.UserPassword = Global.GetMd5Method($"{para.UserPassword}");
            }

            string dateString = string.Empty;
            //將更新資料設定至資料庫中，若更新失敗則
            try
            {
                //若上傳的圖片不為null，才上傳圖片
                if (para.UserImage != null)
                {
                    //紀錄當前時間
                    dateString = DateTime.Now.ToString("yyyyMMddHHmm_");
                    //上傳檔案的檔案路徑
                    string imagePath = $@"image/{dateString}{para.UserImage.FileName}";

                    //將檔案新增至指定路徑中
                    using (var strearm = new FileStream(imagePath, FileMode.Create))
                    {
                        await para.UserImage.CopyToAsync(strearm);
                    }
                }

                //更新使用者資料
                this.DataCenter.SetUserInfo(userInfo, para, dateString);

                //將結果儲存至DB
                this.DataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法更新資料");
            }

            //若尚未驗證帳號過，重寄驗證信
            if (!userInfo.Approved)
            {
                this.DataCenter.SendValidationEmail(userInfo.UserId);
            }

            return this.Ok(this.DataCenter.GetUserInfoDto(userId));
        }

        // POST api/<UserInfoController>

        /// <summary>
        /// Post new UserFavoriteActivity.
        /// </summary>
        /// <param name="userId">Inputting userId.</param>
        /// <param name="activityId">Inputting activityId.</param>
        /// <returns>If successful, return Ok. On the contrary, return the corresponding error message.</returns>
        [HttpPost("{userId}/FavoriteRoomList")]
        public ActionResult PostUserFavoriteRoom(int userId, int activityId)
        {
            //檢查輸入使用者ID是否存在於DB之中，若無則返回NotFound
            if (!this.DataCenter.IsUserIDExist(userId))
            {
                return this.NotFound("查無使用者ID");
            }

            //檢查輸入活動ID是否存在於DB之中，若無則返回NotFound
            if (!this.DataCenter.IsActivityIDExist(activityId))
            {
                return this.NotFound("查無活動ID");
            }

            //檢查該使用者ID及活動ID配對是否出現在UserFavoriteActivity資料表中，若有則告訴前端資料已經存在
            if (this.DataCenter.GetUserFavoriteActivity(userId, activityId) != null)
            {
                return this.BadRequest("此資料已存在");
            }

            //判斷是否能順利寫入Db中，可以回傳"新增成功"(201)，無法寫入則回傳"輸入錯誤，無法新增資料"(500)
            try
            {
                this.DataCenter.PostUserFavoriteActivity(userId, activityId);
                this.DataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法新增資料");
            }

            return this.StatusCode((int)HttpStatusCode.Created, $"UserID:{userId} 成功將 ActivityID:{activityId} 加入關注清單中");
        }

        // DELETE api/<UserInfoController>

        /// <summary>
        /// Delete the specific UserFavoriteActivity.
        /// </summary>
        /// <param name="userId">Inputting userId.</param>
        /// <param name="activityId">Inputting activityId.</param>
        /// <returns>If successful, return Ok. On the contrary, return the corresponding error message.</returns>
        [HttpDelete("{userId}/FavoriteRoomList")]
        public ActionResult DeleteUserFavoriteRoom(int userId, int activityId)
        {
            //檢查該使用者ID及活動ID配對是否出現在UserFavoriteActivity資料表中，若沒有則返回NotFound("查無資料")
            UserFavoriteActivity deleteTarget = this.DataCenter.GetUserFavoriteActivity(userId, activityId);

            if (deleteTarget == null)
            {
                return this.NotFound("查無資料");
            }

            //判斷是否能順利寫入Db中，可以回傳"新增成功"(201)，無法寫入則回傳"輸入錯誤，無法新增資料"(500)
            try
            {
                this.DataCenter.SeeSeaTestContext.Remove(deleteTarget);
                this.DataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法刪除資料");
            }

            return this.StatusCode((int)HttpStatusCode.Created, $"UserID:{userId} 成功將 ActivityID:{activityId} 從關注清單中移除");
        }
    }
}
