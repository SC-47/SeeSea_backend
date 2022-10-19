//-----------------------------------------------------------------------
// <copyright file="EmailValidationController.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Controllers
{
    using GroupPairing_API.DataCenter;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The WebAPI controller related to Email Validation.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class EmailValidationController : ControllerBase
    {
        /// <summary>
        /// The Algorithmic logic of the data about GroupPairing_API.
        /// </summary>
        private readonly UserDataCenter DataCenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailValidationController" /> class.
        /// </summary>
        /// <param name="dataCenter">The Algorithmic logic of the data about UserInfo.</param>
        public EmailValidationController(UserDataCenter dataCenter)
        {
            DataCenter = dataCenter;
        }

        // GET: api/<UserInfoController>

        /// <summary>
        /// This method is activating user's account.
        /// </summary>
        /// <param name="userEmailId">The user's EmailID.</param>
        /// <returns>If the emailId exist, activate the user's account. Exception for validation, return the string descripting the detail of error.</returns>
        [HttpPut("{userEmailId}")]
        public ActionResult ActivateAccount(string userEmailId)
        {
            //嘗試取啟動帳號，若啟動成功回傳"已成功驗證帳號"(200)；若啟動失敗則回傳"發生錯誤，無法驗證帳號"(400)
            try
            {
                if (DataCenter.SetAccountActive(userEmailId))
                {
                    return Ok("已成功驗證帳號");
                }

                return BadRequest("無法驗證帳號");
            }
            catch (DbUpdateException)
            {
                return BadRequest("發生錯誤，無法驗證帳號");
            }

        }
    }
}
