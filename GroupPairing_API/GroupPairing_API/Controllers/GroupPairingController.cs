//-----------------------------------------------------------------------
// <copyright file="GroupPairingController.cs" company="Cmoney">
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
    /// The WebAPI controller related to GroupPairing.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GroupPairingController : ControllerBase
    {
        /// <summary>
        /// The Algorithmic logic of the data about GroupPairing_API.
        /// </summary>
        private readonly RoomDataCenter RoomDataCenter;

        /// <summary>
        /// The Algorithmic logic of the data about GroupPairing_API.
        /// </summary>
        private readonly UserDataCenter UserDataCenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupPairingController" /> class.
        /// </summary>
        /// <param name="roomDataCenter">The Algorithmic logic of the data about GroupPairing_API.</param>
        /// <param name="userDataCenter">The Algorithmic logic of the data about UserInfo.</param>
        public GroupPairingController(RoomDataCenter roomDataCenter, UserDataCenter userDataCenter)
        {
            this.RoomDataCenter = roomDataCenter;
            this.UserDataCenter = userDataCenter;
        }

        // POST api/<GroupPairingController>

        /// <summary>
        /// Post the new ActivityRoom.
        /// </summary>
        /// <param name="activityRoomInput">The input JSON body.</param>
        /// <returns>If the information is valid, post the data. Exception for validation, return the string descripting the detail of error.</returns>
        [HttpPost("Room")]
        public async Task<IActionResult> PostRoom([FromForm] ActivityRoomPost activityRoomInput)
        {
            UserInfo user = this.UserDataCenter.GetUserInfo(activityRoomInput.HostId);

            //判斷主辦人ID是否存在於UserID列表中，若沒有則回傳"該主辦人ID不存在，無法新增資料"(400)
            if (user == null)
            {
                return this.BadRequest("該主辦人ID不存在，無法新增資料");
            }

            //判斷該ID的帳號是否已經啟用過，若沒有則回傳"該帳號尚未驗證，無法新增資料"(400)
            if (!user.Approved)
            {
                return this.BadRequest("該帳號尚未驗證，無法新增資料");
            }

            DateTime dateTime;
            string dateString = string.Empty;

            //判斷日期格式輸入是否正確，若否則回傳"活動日期輸入格式錯誤，無法新增資料"(400)
            if (DateTime.TryParse(activityRoomInput.ActivityDateTime, out dateTime))
            {
                //利用Validation Attribute先驗證輸入資料是否符合規範，再判斷是否能順利寫入Db中，可以回傳"新增成功"(201)，無法寫入則回傳"輸入錯誤，無法新增資料"(400)
                try
                {
                    //若上傳的圖片不為null，才上傳圖片
                    if (activityRoomInput.ActivityPicture != null)
                    {
                        //紀錄當前時間
                        dateString = DateTime.Now.ToString("yyyyMMddHHmm_");
                        //上傳檔案的檔案路徑
                        string imagePath = $@"image/{dateString}{activityRoomInput.ActivityPicture.FileName}";

                        //將檔案新增至指定路徑中
                        using (var strearm = new FileStream(imagePath, FileMode.Create))
                        {
                            await activityRoomInput.ActivityPicture.CopyToAsync(strearm);
                        }
                    }

                    //新增房間資料至DB中
                    this.RoomDataCenter.PostRoom(activityRoomInput, dateTime, dateString);
                    this.RoomDataCenter.SeeSeaTestContext.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    return this.BadRequest("輸入錯誤，無法新增資料");
                }

                return this.StatusCode((int)HttpStatusCode.Created, "新增成功");
            }

            return this.BadRequest("活動日期輸入格式錯誤，無法新增資料");
        }

        // GET: api/<GroupPairingController>

        /// <summary>
        /// Get the information of all the rooms in the database named SeeSeaTest.
        /// </summary>
        /// <returns>If the information exist, return the data.</returns>
        [HttpGet("Room")]
        public ActionResult GetAllRoomDto()
        {
            //取得所有房間列表，回傳房間列表資訊(200)，若沒有則回傳空的陣列(以ActivityRoomDto類別回傳)
            return this.Ok(this.RoomDataCenter.GetRoomDto());
        }

        // GET: api/<GroupPairingController>

        /// <summary>
        /// Get all the active rooms' information in the database named SeeSeaTest.
        /// </summary>
        /// <returns>If the information exist, return the data.</returns>
        [HttpGet("ActiveRoom")]
        public ActionResult GetActiveRoomDto()
        {
            //取得所有"未確認"的房間列表，回傳房間列表資訊(200)，若沒有則回傳空的陣列(以ActivityRoomDto類別回傳)
            return this.Ok(this.RoomDataCenter.GetActiveRoomDto());
        }

        // GET: api/<GroupPairingController>

        /// <summary>
        /// Get the information of the specific room in the database named SeeSeaTest.
        /// </summary>
        /// <param name="roomId">The querying roomID.</param>
        /// <returns>If the information exist, return the data. Exception for existing, return the string descripting the detail of error.</returns>
        [HttpGet("Room/RoomID/{roomId}")]
        public ActionResult GetRoomDto(int roomId)
        {
            ActivityRoomDto activityRoomDto = this.RoomDataCenter.GetRoomDto(roomId);
            return activityRoomDto != null ? this.Ok(activityRoomDto) : this.NotFound("查無資料");
        }

        // GET: api/<GroupPairingController>

        /// <summary>
        /// Get the specific IDs of the room' information in the database named SeeSeaTest.
        /// </summary>
        /// <param name="roomIDs">The specific IDs the user wants to get.</param>
        /// <returns>If the information exist, return the data.</returns>
        [HttpGet("Room/{roomIDs}")]
        public ActionResult GetRoomDto(string roomIDs)
        {
            //取得指定ActivityID(可輸入複數ID，以","分隔)的房間列表，回傳房間列表資訊(200)，若沒有則回傳空的陣列(以ActivityRoomDto類別回傳)
            return this.Ok(this.RoomDataCenter.GetRoomDto(roomIDs));
        }

        // GET: api/<GroupPairingController>

        /// <summary>
        /// Get the selected ActivityRoom data.
        /// </summary>
        /// <param name="divingType">The querying divingType tag.</param>
        /// <param name="property">The querying property tag.</param>
        /// <param name="area">The querying area tag.</param>
        /// <param name="estimateCost">The estimateCost tag.</param>
        /// <returns>The selected ActivityRoomDTOs.</returns>
        [HttpGet("Room/Select")]
        public ActionResult GetRoomDtoBySelector(string divingType, string property, string area, string estimateCost)
        {
            //取得過濾後的"未確認"房間列表，回傳房間列表資訊(200)，若沒有則回傳空的陣列(以ActivityRoomDto類別回傳)
            return this.Ok(this.RoomDataCenter.GetActiveRoomDtoBySelector(divingType, property, area, estimateCost));
        }

        // GET: api/<GroupPairingController>

        /// <summary>
        /// Get the selected ActivityRoomDTOs by user's preference.
        /// </summary>
        /// <param name="userId">The querying userId.</param>
        /// <returns>The selected ActivityRoomDTOs that user's may like.</returns>
        [HttpGet("UserPreferRoom/{userId}")]
        public ActionResult GetUserPreferRoomDtoList(int userId)
        {
            UserInfo userInfo = this.UserDataCenter.GetUserInfo(userId);
            string property = string.Empty;
            if (userInfo.UserExperienceCode == (int)Global.UserExperience.NO_EXPERIENCE)
            {
                property = $"{(int)Global.ActivityProperty.CLASS},{(int)Global.ActivityProperty.LEAD_BY_HOST}";
            }

            //取得過濾後的"未確認"房間列表，若沒有對應的房間資訊則回傳空表，若沒有則回傳空的陣列(以ActivityRoomDto類別回傳)
            return this.Ok(this.RoomDataCenter.GetActiveRoomDtoBySelector(userInfo.DivingTypeTag, property, userInfo.AreaTag, string.Empty));
        }

        // GET: api/<GroupPairingController>

        /// <summary>
        /// Get the specific room' information in the database named SeeSeaTest.
        /// </summary>
        /// <param name="divingPointId">The specific place (DivingPointID) the user wants to get.</param>
        /// <returns>If the information exist, return the data.</returns>
        [HttpGet("Room/Place/{divingPointId}")]
        public ActionResult GetRoomDivingPointDto(int divingPointId)
        {
            //取得指定地點(DivingPointID)的房間列表，回傳房間列表資訊(200)，若沒有則回傳空的陣列(以ActivityRoomDto類別回傳)
            return this.Ok(this.RoomDataCenter.GetActiveRoomDivingPointDto(divingPointId));
        }

        // PUT api/<UserInfoController>

        /// <summary>
        /// Put the ActivityRoom.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <param name="input">The inputting parameter of putting ActivityRoom.</param>
        /// <returns>If successful, return Ok. On the contrary, return the corresponding error message.</returns>        
        [HttpPut("Room/{activityId}/")]
        public async Task<IActionResult> PutActivity(int activityId, [FromForm] ActivityRoomPut input)
        {
            //透過activityId查找指定活動，並判斷該活動是否為「未出團」狀態
            ActivityRoom activityRoom = this.RoomDataCenter.GetActiveRoom(activityId);

            //判斷該UserID是否存在於資料庫之中
            if (activityRoom == null)
            {
                return this.NotFound("該活動Id非「未出團」狀態");
            }

            string dateString = string.Empty;

            //將更新資料設定至資料庫中，若更新失敗則
            try
            {
                //若上傳的圖片不為null，才上傳圖片
                if (input.ActivityPicture != null)
                {
                    //紀錄當前時間
                    dateString = DateTime.Now.ToString("yyyyMMddHHmm_");
                    //上傳檔案的檔案路徑
                    string imagePath = $@"image/{dateString}{input.ActivityPicture.FileName}";

                    //將檔案新增至指定路徑中
                    using (var strearm = new FileStream(imagePath, FileMode.Create))
                    {
                        await input.ActivityPicture.CopyToAsync(strearm);
                    }
                }

                this.RoomDataCenter.PutAcitivty(activityRoom, input, dateString);
                this.RoomDataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法更新資料");
            }

            return this.Ok("活動更新成功");
        }

        // PUT api/<UserInfoController>

        /// <summary>
        /// Put ActivityRoom's ActivityStatus.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <returns>If successful, return Ok. On the contrary, return the corresponding error message.</returns>        
        [HttpPut("Room/{activityId}/ActivityStatus")]
        public ActionResult PutActivityStatus(int activityId)
        {
            //透過activityId查找指定活動，並判斷該活動是否為「未出團」狀態
            ActivityRoom activityRoom = this.RoomDataCenter.GetActiveRoom(activityId);

            //判斷該UserID是否存在於資料庫之中
            if (activityRoom == null)
            {
                return this.NotFound("該活動Id非「未出團」狀態");
            }

            //將更新資料設定至資料庫中，若更新失敗則
            try
            {
                activityRoom.ActivityStatusCode = (int)Global.ActivityStatus.CONFIRMED;
                this.RoomDataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法更新資料");
            }

            return this.Ok("活動狀態更新成功");
        }

        // DELETE : api/<GroupPairingController>

        /// <summary>
        /// Delete the information of the specific room in the database named SeeSeaTest.
        /// </summary>
        /// <param name="roomId">The querying roomID.</param>
        /// <returns>If the information exist, delete the data. Exception for existing, return the string descripting the detail of error.</returns>
        [HttpDelete("Room/RoomID/{roomId}")]
        public ActionResult DeleteRoom(int roomId)
        {
            //透過ID尋找特定留言
            ActivityRoom target = this.RoomDataCenter.GetRoom(roomId).SingleOrDefault();

            //若輸入的ID找不到留言，則回傳"查無此活動ID"(404)
            if (target == null)
            {
                return this.NotFound("查無此活動ID");
            }

            //判斷是否能順利從Db中刪除資料，可以回傳"刪除成功"(200)，無法順利刪除則回傳"輸入錯誤，無法刪除資料"(400)
            try
            {
                this.RoomDataCenter.DeleteRoom(target);
                this.RoomDataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法刪除資料");
            }

            return this.Ok("刪除成功");
        }

        // POST : api/<GroupPairingController>

        /// <summary>
        /// Post a new Message.
        /// </summary>
        /// <param name="messageInput">The input JSON body.</param>
        /// <returns>If the information is valid, post the data and return the id of the message. Exception for validation, return the string descripting the detail of error.</returns>
        [HttpPost("Room/Message")]
        public ActionResult PostMessage([FromBody] MessagePost messageInput)
        {
            //判斷活動ID是否存在於ActivityID列表中，若沒有則回傳"該活動ID不存在，無法新增資料"(400)
            if (this.RoomDataCenter.GetRoom(messageInput.ActivityId) == null)
            {
                return this.BadRequest("該活動ID不存在，無法新增資料");
            }

            UserInfo user = this.UserDataCenter.GetUserInfo(messageInput.UserId);

            //判斷主辦人ID是否存在於UserID列表中，若沒有則回傳"該主辦人ID不存在，無法新增資料"(400)
            if (user == null)
            {
                return this.BadRequest("使用者ID不存在，無法新增資料");
            }

            //判斷該ID的帳號是否已經啟用過，若沒有則回傳"該帳號尚未驗證，無法新增資料"(400)
            if (!user.Approved)
            {
                return this.BadRequest("該帳號尚未驗證，無法新增資料");
            }

            //判斷日期格式輸入是否正確，若否則回傳"活動日期輸入格式錯誤，無法新增資料"(400)
            if (!DateTime.TryParse(messageInput.MessageDateTime, out DateTime result))
            {
                return this.BadRequest("留言日期輸入格式錯誤，無法新增資料");
            }

            //若輸入留言資料已存在於資料庫中，則回傳"該留言已重複，無法新增資料"(400)
            if (this.RoomDataCenter.IsMessageExist(messageInput))
            {
                return this.BadRequest("該留言已存在，無法新增資料，請稍後再試");
            }

            //利用Validation Attribute先驗證輸入資料是否符合規範，再判斷是否能順利寫入Db中，可以回傳"新增成功"(201)，無法寫入則回傳"輸入錯誤，無法新增資料"(400)
            try
            {
                this.RoomDataCenter.PostMessage(messageInput);
                this.RoomDataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法新增資料");
            }

            return this.StatusCode((int)HttpStatusCode.Created, this.RoomDataCenter.GetMessageID(messageInput));
        }

        // DELETE: api/<GroupPairingController>

        /// <summary>
        /// Delete the specific message.
        /// </summary>
        /// <param name="messageId">The target ID of the specific message.</param>
        /// <returns>If the information is valid, post the data. Exception for validation, return the string descripting the detail of error.</returns>
        [HttpDelete("Room/Message")]
        public ActionResult DeleteMessage(int messageId)
        {
            //透過ID尋找特定留言
            MessageBoard target = this.RoomDataCenter.SeeSeaTestContext.MessageBoards
                        .Where(message => message.MessageId == messageId)
                        .SingleOrDefault();

            //若輸入的ID找不到留言，則回傳"查無此留言ID"(404)
            if (target == null)
            {
                return this.NotFound("查無此留言ID");
            }

            //判斷是否能順利從Db中刪除資料，可以回傳"刪除成功"(200)，無法順利刪除則回傳"輸入錯誤，無法刪除資料"(400)
            try
            {
                this.RoomDataCenter.DeleteMessage(target);
                this.RoomDataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法刪除資料");
            }

            return this.Ok("刪除成功");
        }

        // POST api/<GroupPairingController>

        /// <summary>
        /// Post a new ActivityApplicant.
        /// </summary>
        /// <param name="activityApplicantInput">The input parameter to construct ActivityApplicant</param>
        /// <returns>If the information is valid, post the data. Except for validation , return the string descripting the detail of error.</returns>
        [HttpPost("Room/Applicant")]
        public ActionResult PostActivityApplicant([FromBody] ActivityApplicantPost activityApplicantInput)
        {
            //存下前端輸出的活動ID
            int activityId = activityApplicantInput.ActivityId;

            //存下前端輸出的申請者ID(即使用者的UserID)
            int userId = activityApplicantInput.ApplicantId;

            //根據userID查找特定user
            UserInfo user = this.UserDataCenter.GetUserInfo(userId);

            //判斷主辦人ID是否存在於UserID列表中，若沒有則回傳"該主辦人ID不存在，無法新增資料"(400)
            if (user == null)
            {
                return this.BadRequest("查無使用者ID");
            }

            //判斷該ID的帳號是否已經啟用過，若沒有則回傳"該帳號尚未驗證，無法新增資料"(400)
            if (!user.Approved)
            {
                return this.BadRequest("該帳號尚未驗證，無法使用此功能");
            }

            //根據輸入的活動ID找出對應的活動，並判斷該活動是否"尚未出團"
            ActivityRoom targetRoom = this.RoomDataCenter.GetActiveRoom(activityId);

            //若沒有符合條件的活動(為null)，則返回NotFound("此活動ID非未出團活動")
            if (targetRoom == null)
            {
                return this.NotFound("此活動ID非未出團活動");
            }

            //檢查該使用者ID是否為該活動ID主辦人
            if (targetRoom.HostId == userId)
            {
                return this.BadRequest("此UserID為該活動房主，無法作為參加者報名此活動");
            }

            //檢查該使用者ID是否為該活動ID參加者
            if (this.RoomDataCenter.IsParticipant(activityId, userId))
            {
                return this.BadRequest("此UserID已參加此活動");
            }

            //檢查該使用者ID及活動ID配對是否出現在ActivityApplicant資料表中，若有則回傳BadRequest("此資料已存在")
            if (this.RoomDataCenter.IsApplicant(activityId, userId))
            {
                return this.BadRequest("此資料已存在");
            }

            //判斷日期格式輸入是否正確，若否則回傳"日期輸入格式錯誤，無法新增資料"(400)
            if (!DateTime.TryParse(activityApplicantInput.ApplicatingDateTime, out DateTime result))
            {
                return this.BadRequest("日期輸入格式錯誤，無法新增資料");
            }

            //判斷是否能順利寫入Db中，可以回傳 $"ActivityId:{activityId} 成功將 UserId:{userId} 加入至活動報名者清單中"(201)，無法寫入則BadRequest("輸入錯誤，無法新增資料")
            try
            {
                this.RoomDataCenter.PostActivityApplicant(activityApplicantInput);
                this.RoomDataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法新增資料");
            }

            return this.StatusCode((int)HttpStatusCode.Created, $"ActivityId:{activityId} 成功將 UserId:{userId} 加入至活動報名者清單中");
        }

        // GET: api/<GroupPairingController>

        /// <summary>
        /// Get the specific ActivityID's applicants' list.
        /// </summary>
        /// <param name="roomId">The inputted roomId.</param>
        /// <returns>If the information exist, return the data.</returns>
        [HttpGet("Room/{roomId}/ApplicantList")]
        public ActionResult GetApplicantList(int roomId)
        {
            //取得指定房間的報名者清單，內容包含報名者ID、報名者姓名及報名者動機，若沒有則回傳空的陣列(以ActivityApplicantDto類別回傳)
            return this.Ok(this.RoomDataCenter.GetApplicantList(roomId));
        }

        // DELETE api/<GroupPairingController>

        /// <summary>
        /// Delete the specific ActivityApplicant.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <param name="userId">The inputting userId.</param>
        /// <returns>If the information exist, delete the data. Except for existing , return the string descripting the detail of error.</returns>
        [HttpDelete("Room/{activityId}/Applicant/{userId}")]
        public ActionResult DeleteActivityApplicant(int activityId, int userId)
        {
            //根據輸入活動ID即使用者ID，從Db中搜尋是否有符合條件的資料
            ActivityApplicant deleteTarget = this.RoomDataCenter.GetActivityApplicant(activityId, userId);

            //若沒有資料符合輸入條件(null)，則返回NotFound("查無資料")
            if (deleteTarget == null)
            {
                return this.NotFound("查無資料");
            }

            //若有資料符合條件，進行移除並儲存，若儲存更新失敗，則回傳BadRequest("輸入錯誤，無法刪除資料")
            try
            {
                this.RoomDataCenter.SeeSeaTestContext.Remove(deleteTarget);
                this.RoomDataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法刪除資料");
            }

            return this.StatusCode((int)HttpStatusCode.Created, $"ActivityID:{activityId} 成功將 UserID:{userId} 從活動報名者清單中移除");
        }

        // POST api/<GroupPairingController>

        /// <summary>
        /// Post a new ActivityParticipant.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <param name="userId">The inputting userId.</param>
        /// <returns>If the information exist, post the data. Except for existing , return the string descripting the detail of error.</returns>
        [HttpPost("Room/Participant")]
        public ActionResult PostActivityParticipant(int activityId, int userId)
        {
            //從ActivityApplicant資料表中搜尋指定條件的報名者
            ActivityApplicant participant = this.RoomDataCenter.GetActivityApplicant(activityId, userId);

            //檢查輸入的使用者ID是否存在於報名者清單中，若無則返回NotFound
            if (participant == null)
            {
                return this.NotFound("查無此報名者資料");
            }

            //根據輸入的活動ID查找指定活動，並且判斷該活動是否為"未出團"狀態
            ActivityRoom targetRoom = this.RoomDataCenter.GetNonFullRoom(activityId);

            //若沒有符合輸入條件的資料，則返回NotFound("此活動ID非未滿團活動")
            if (targetRoom == null)
            {
                return this.NotFound("此活動ID非未滿團活動");
            }

            //檢查該使用者ID是否為該活動ID主辦人，若使用者為主辦人則回傳BadRequest("此UserID為該活動房主，無法作為參加者參加此活動")
            if (targetRoom.HostId == userId)
            {
                return this.BadRequest("此UserID為該活動房主，無法作為參加者參加此活動");
            }

            //檢查該使用者ID及活動ID配對是否已存在於ActivityParticipant資料表中，若有則回傳BadRequest("此資料已存在")
            if (this.RoomDataCenter.IsParticipant(activityId, userId))
            {
                return this.BadRequest("此資料已存在");
            }

            //判斷是否能順利寫入Db中，可以則回傳"新增成功"(201)；若無法無法寫入則回傳BadRequest("輸入錯誤，無法新增資料")
            try
            {
                this.RoomDataCenter.PostActivityParticipant(activityId, userId);
                ActivityRoom activityRoom = this.RoomDataCenter.GetRoom(activityId).SingleOrDefault();
                activityRoom.CurrentParticipantNumber = ++activityRoom.CurrentParticipantNumber;
                if (activityRoom.CurrentParticipantNumber >= activityRoom.ParticipantNumber)
                {
                    activityRoom.ActivityStatusCode = (int)Global.ActivityStatus.FULL;
                }

                this.RoomDataCenter.SeeSeaTestContext.ActivityApplicants.Remove(participant);
                this.RoomDataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法新增資料");
            }

            return this.StatusCode((int)HttpStatusCode.Created, $"ActivityId:{activityId} 成功將 UserId:{userId} 加入至活動參加者清單中");
        }

        // GET: api/<GroupPairingController>

        /// <summary>
        /// Get the specific ActivityID's participants' list.
        /// </summary>
        /// <param name="roomId">The inputted roomId.</param>
        /// <returns>If the information exist, return the data.</returns>
        [HttpGet("Room/{roomId}/ParticipantList")]
        public ActionResult GetParticipantList(int roomId)
        {
            //取得指定房間的參加者清單，內容包含參加者ID及參加者姓名，若沒有則回傳空的陣列(以ActivityParticipantDto類別回傳)
            return this.Ok(this.RoomDataCenter.GetParticipantList(roomId));
        }

        // DELETE api/<GroupPairingController>

        /// <summary>
        /// Delete the specific ActivityParticipant.
        /// </summary>
        /// <param name="activityId">The inputting activityId.</param>
        /// <param name="userId">The inputting userId.</param>
        /// <returns>If the information exist, delete the data. Except for existing , return the string descripting the detail of error.</returns>
        [HttpDelete("Room/{activityId}/Participant/{userId}")]
        public ActionResult DeleteActivityParticipant(int activityId, int userId)
        {
            //根據前端輸出的活動ID及使用者ID，從ActivityParticipant資料表中查找是否有相對應的資料
            ActivityParticipant deleteTarget = this.RoomDataCenter.GetActivityParticipant(activityId, userId);

            //若沒有符合條件的資料(null)，則返回NotFound("查無資料")
            if (deleteTarget == null)
            {
                return this.NotFound("查無資料");
            }

            //移除符合條件資料，並且將資料內的當前參加者人數-1；若原先狀態是"已宣告"且人數已滿的狀況下，先解除宣告狀態；若原先活動資料狀態為"滿員中"，則將狀態改為"揪團配對中"
            try
            {
                this.RoomDataCenter.RemoveParticipant(deleteTarget);
                this.RoomDataCenter.SeeSeaTestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return this.BadRequest("輸入錯誤，無法刪除資料");
            }

            //若更新成功，則回傳Ok($"ActivityID:{activityId} 成功將 UserID:{userId} 從活動參加者清單中移除")
            return this.Ok($"ActivityID:{activityId} 成功將 UserID:{userId} 從活動參加者清單中移除");
        }

        // GET: api/<GroupPairingController>

        /// <summary>
        /// Get the specific UserID's UserRoomList.
        /// </summary>
        /// <param name="userId">The inputted userId.</param>
        /// <returns>If the information exist, return the data.</returns>
        [HttpGet("UserRoomList/{userId}")]
        public ActionResult GetUserRoomList(int userId)
        {
            //透過使用者ID查找使用者
            UserInfo user = this.UserDataCenter.GetUserInfo(userId);

            //判段使用者ID是否存在於UserID列表中，若沒有則回傳"該主辦人ID不存在，無法新增資料"(400)
            if (user == null)
            {
                return this.NotFound("該使用者ID不存在");
            }

            //取得指定使用者的ActivityRoomID列表，回傳房間列表資訊(200)，若沒有則回傳空的陣列(以UserRoomListDto類別回傳)
            UserRoomListDto userRoomListDto = new UserRoomListDto
            {
                //使用者參加的活動房間清單，包含作為主辦人或是參加者，活動當前狀態為揪團配對中或是已滿員但尚未出團的狀態
                ParticipatingRoomList = this.RoomDataCenter.GetUserParticipatingRoomList(userId),

                //使用者關注房間清單
                FavoriteRoomList = this.RoomDataCenter.GetUserFavoriteRoomList(userId),

                //使用者報名中的房間清單
                SigningUpRoomList = this.RoomDataCenter.GetUserSigningUpRoomList(userId),

                //使用者過去參加的活動坊間清單，包含作為主辦人或參加者，活動當前狀態為已出團或是已流團
                EndingRoomList = this.RoomDataCenter.GetUserEndingRoomList(userId)
            };
            return this.Ok(userRoomListDto);
        }
    }
}