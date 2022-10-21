//-----------------------------------------------------------------------
// <copyright file="EmailValidationController.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Controllers
{
    using GroupPairing_API.DataCenter;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The WebAPI controller related to Email Validation.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class EmailValidationController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailValidationController"/> class.
        /// </summary>
        /// <param name="dataCenter">The Algorithmic logic of the data about UserInfo.</param>
        public EmailValidationController(UserDataCenter dataCenter)
        {
            DataCenter = dataCenter;
        }

        /// <summary>
        /// The Algorithmic logic of the data about UserInfo.
        /// </summary>
        private readonly UserDataCenter DataCenter;

        // GET: api/<UserInfoController>

        /// <summary>
        /// This method is activating user's account.
        /// </summary>
        /// <param name="userEmailId">The user's EmailID.</param>
        /// <returns>If the emailId exist, activate the user's account.</returns>
        [HttpPut("{userEmailId}")]
        public IActionResult ActivateAccount(string userEmailId)
        {
            //嘗試取啟動帳號，若啟動成功回傳"已成功驗證帳號"(200)
            DataCenter.SetAccountActive(userEmailId);

            return Ok("以成功驗證帳號");
        }
    }
}