//-----------------------------------------------------------------------
// <copyright file="Global.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using GroupPairing_API.Models.Db;

namespace GroupPairing_API
{

    /// <summary>
    /// Store all the needed static variables and enum in this class.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// The first ID of the diving point.
        /// </summary>
        public const int DIVING_POINT_MIN = 1;

        /// <summary>
        /// The last ID of the diving point.
        /// </summary>
        public const int DIVING_POINT_MAX = 139;

        /// <summary>
        /// The first element of the list.
        /// </summary>
        public const int FIRST = 0;

        /// <summary>
        /// The second element of the list.
        /// </summary>
        public const int SECOND = 1;

        /// <summary>
        /// The third element of the list.
        /// </summary>
        public const int THIRD = 2;

        /// <summary>
        /// The fourth element of the list.
        /// </summary>
        public const int FOURTH = 3;

        /// <summary>
        /// The fifth element of the list.
        /// </summary>
        public const int FIFTH = 4;

        /// <summary>
        /// The sixth element of the list.
        /// </summary>
        public const int SIXTH = 5;

        /// <summary>
        /// just zero.
        /// </summary>
        public const int ZERO = 0;

        /// <summary>
        /// just one.
        /// </summary>
        public const int ONE = 1;

        /// <summary>
        /// The minimum of the number of the participants.
        /// </summary>
        public const int PARTICIPANT_NUM_MIN = 2;

        /// <summary>
        /// The maximum of the number of the participants.
        /// </summary>
        public const int NAME_LENGTH_MAX = 25;

        /// <summary>
        /// The minimum of the number of the participants.
        /// </summary>
        public const int PARTICIPANT_NUM_MAX = 64;

        /// <summary>
        /// The maximum of the number of the age.
        /// </summary>
        public const int AGE_MAX = 120;

        /// <summary>
        /// The minimum of the HostID.
        /// </summary>
        public const int ID_MIN = 1;

        /// <summary>
        /// The corresponded type of the ActivityStatusCode.
        /// </summary>
        public enum ActivityStatus : byte
        {
            /// <summary>
            /// The ActivityRoom is pairing now.
            /// </summary>
            PAIRING = 1,

            /// <summary>
            /// The ActivityRoom is full of participants now. 
            /// </summary>
            FULL = 2,

            /// <summary>
            /// The ActivityRoom is confirmed by the host. 
            /// </summary>
            CONFIRMED = 3,

            /// <summary>
            /// The Activity is over. 
            /// </summary>
            DONE = 4,

            /// <summary>
            /// The Activity is lack of participants. 
            /// </summary>
            FAIL_PAIRING = 5
        }

        /// <summary>
        /// The corresponded string of the DivingTypeCode.
        /// </summary>
        public enum DivingType : byte
        {
            /// <summary>
            /// The Activity is scuba diving.
            /// </summary>
            [Description("自由潛水")]
            FREE = 1,

            /// <summary>
            /// The Activity is free diving.
            /// </summary>
            [Description("水肺潛水")]
            SCUBA = 2
        }

        /// <summary>
        /// The corresponded string of the DivingLevelCode.
        /// </summary>
        public enum DivingLevel : byte
        {
            /// <summary>
            /// The participants of this activity don't need any diving certification.
            /// </summary>
            [Description("經驗不拘")]
            NON_SETTING = 1,

            /// <summary>
            /// The participants of this activity need scuba diving certification which is OW.
            /// </summary>
            OW = 2,

            /// <summary>
            /// The participants of this activity need scuba diving certification which is AOW.
            /// </summary>
            AOW = 3,

            /// <summary>
            /// The participants of this activity need free diving certification which is AIDA1.
            /// </summary>
            AIDA1 = 4,

            /// <summary>
            /// The participants of this activity need free diving certification which is AIDA2.
            /// </summary>
            AIDA2 = 5,

            /// <summary>
            /// The participants of this activity need free diving certification which is AIDA3.
            /// </summary>
            AIDA3 = 6
        }

        /// <summary>
        /// The corresponded string of the ActivityPropertyCode.
        /// </summary>
        public enum ActivityProperty : byte
        {
            /// <summary>
            /// This activity is for fun.
            /// </summary>
            [Description("一般揪團")]
            FUN = 1,

            /// <summary>
            /// This activity is held by a professional diver.
            /// </summary>
            [Description("潛店潛導帶潛")]
            LEAD_BY_HOST = 2,

            /// <summary>
            /// This activity is a diving class.
            /// </summary>
            [Description("教練開課")]
            CLASS = 3,

            /// <summary>
            /// This activity is belonged to other type.
            /// </summary>
            [Description("其他")]
            OTHER = 4,
        }

        /// <summary>
        /// The corresponded string of the ActivityAreaCode.
        /// </summary>
        public enum ActivityArea : byte
        {
            /// <summary>
            /// The activity area is north area.
            /// </summary>
            [Description("北海岸")]
            NORTH = 1,

            /// <summary>
            /// The activity area is KENTING.
            /// </summary>
            [Description("墾丁")]
            KENTING = 2,

            /// <summary>
            /// The activity area is for indoor..
            /// </summary>
            [Description("室內/東部")]
            INDOOR = 3,

            /// <summary>
            /// The outlying island is Green Island.
            /// </summary>
            [Description("綠島")]
            GREEN_ISLAND = 4,

            /// <summary>
            /// The outlying island is LANYU.
            /// </summary>
            [Description("蘭嶼")]
            LANYU = 5,

            /// <summary>
            /// The outlying island is PENGHU.
            /// </summary>
            [Description("澎湖")]
            PENGHU = 6,

            /// <summary>
            /// The outlying island is LIUQIU.
            /// </summary>
            [Description("小琉球")]
            LIUQIU = 7
        }

        /// <summary>
        /// The corresponded string of the TransportationCode.
        /// </summary>
        public enum Transportation : byte
        {
            /// <summary>
            /// The participants need to go to the activity place by self.
            /// </summary>
            [Description("參加者自行前往")]
            SELF = 1,

            /// <summary>
            /// The participants can be taken driven by the host.
            /// </summary>
            [Description("主辦人提供車位")]
            TAKE_DRIVE = 2,

            /// <summary>
            /// The participants can assemble and go to the activity place together.
            /// </summary>
            [Description("一起搭大眾交通工具")]
            PUBLIC_TRANPORTATION = 3,
        }

        /// <summary>
        /// The corresponded string of the EstimateCostCode.
        /// </summary>
        public enum EstimateCost : byte
        {
            /// <summary>
            /// The host doesn't set estimate cost.
            /// </summary>
            [Description("")]
            NON_SETTING = 0,

            /// <summary>
            /// The host estimate the cost  of this activity is below 2000.
            /// </summary>
            [Description("2000 元以下")]
            LOWEST = 1,

            /// <summary>
            /// The host estimate the cost  of this activity is between 2000 and 5000.
            /// </summary>
            [Description("2000 ~ 5000 元")]
            MEDIUM_LOW = 2,

            /// <summary>
            /// The host estimate the cost  of this activity is between 5000 and 10000.
            /// </summary>
            [Description("5000 ~ 10000 元")]
            MEDIUM_HIGH = 3,

            /// <summary>
            /// The host estimate the cost  of this activity is higher than 10000.
            /// </summary>
            [Description("無限制")]
            HIGHEST = 4,
        }

        /// <summary>
        /// The corresponded string of the EstimateCostTag.
        /// </summary>
        public enum EstimateCostTag : byte
        {
            /// <summary>
            /// The user's acceptable estimate the cost  of this activity is below 2000.
            /// </summary>
            [Description("2000 元以下")]
            LOWEST = 1,

            /// <summary>
            /// The user's acceptable estimate the cost  of this activity is below 5000.
            /// </summary>
            [Description("5000 元以下")]
            MEDIUM_LOW = 2,

            /// <summary>
            /// The user's acceptable estimate the cost  of this activity is below 10000.
            /// </summary>
            [Description("10000 元以下")]
            MEDIUM_HIGH = 3,

            /// <summary>
            /// The user's acceptable estimate the cost  of this activity is no limited.
            /// </summary>
            [Description("無限制")]
            HIGHEST = 4,
        }

        /// <summary>
        /// The corresponded string of the UserExperience.
        /// </summary>
        public enum UserExperience : byte
        {
            /// <summary>
            /// The user has no or limited experience for diving.
            /// </summary>
            [Description("無經驗")]
            NO_EXPERIENCE = 1,

            /// <summary>
            /// The user has the basic certification of diving.
            /// </summary>
            [Description("一般")]
            NORMAL = 2,

            /// <summary>
            /// The user has lots of experience of diving.
            /// </summary>
            [Description("資深")]
            VETERAN = 3,
        }

        /// <summary>
        /// The corresponded string of the DivingDifficulty.
        /// </summary>
        public enum DivingDifficulty : byte
        {
            /// <summary>
            /// The diving point is suited for everybody.
            /// </summary>
            [Description("入門")]
            BEGINNER = 1,

            /// <summary>
            /// The diving point is suited for regular diver.
            /// </summary>
            [Description("一般")]
            NORMAL = 2,

            /// <summary>
            /// The diving point is suited for veteran diver.
            /// </summary>
            [Description("進階")]
            VETERAN = 3,
        }

        /// <summary>
        /// Gets the SeeSeaContext class.
        /// </summary>
        public static SeeSeaTestContext SeeSeaTestContext { get; }

        /// <summary>
        /// Gets the stopwatch
        /// </summary>
        public static Stopwatch STOPWATCH { get; } = new Stopwatch();

        /// <summary>
        /// Gets Attribute Description from Enum.
        /// </summary>
        /// <param name="value">Enum class.</param>
        /// <returns>The description of the corresponding enum.</returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > ZERO ? attributes[ZERO].Description : value.ToString();
        }

        /// <summary>
        /// Convert string to List of INT.
        /// </summary>
        /// <param name="input">Input string data.</param>
        /// <returns>Output List of INT data.</returns>
        public static List<int> ConvertStringToIntList(string input)
        {
            //將輸入字串以','作切割
            string[] numberList = input.Split(',');

            //初始化輸出List<int>
            List<int> result = new();

            //將陣列內容轉換成List<int>
            foreach (string numberString in numberList)
            {
                //判斷切割後的字串內容是否能從string轉換成int
                if (int.TryParse(numberString, out int numberInt))
                {
                    result.Add(numberInt);
                }
            }

            //將結果回傳出去
            return result;
        }

        /// <summary>
        /// Get a random password.
        /// </summary>
        /// <returns>A new random password.</returns>
        public static string GetRandomPassword()
        {
            //密碼字元組合來源
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";

            //密碼長度
            int passwordLength = 8;

            //新設定的密碼(為字元陣列型態)
            char[] chars = new char[passwordLength];
            Random random = new();

            //allowedChars -> 這個String ，隨機取得一個字，丟給chars[i]
            for (int i = ZERO; i < passwordLength; i++)
            {
                chars[i] = allowedChars[random.Next(ZERO, allowedChars.Length)];
            }

            //將字元陣列轉為字串並輸出出去
            return new string(chars);
        }

        /// <summary>
        /// ComputeHash by MD5.
        /// </summary>
        /// <param name="input">The string needs to be encrypted.</param>
        /// <returns>Encrypted string </returns>
        public static string GetMd5Method(string input)
        {
            //新增一個MD5加密Provider
            MD5CryptoServiceProvider md5Hasher = new();

            //利用Md5Hasher將輸入字串進行轉換
            byte[] myData = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            //新增一個StringBuilder實體
            StringBuilder stringBuilder = new();

            //將輸入內容轉換成加密字串
            for (int i = 0; i < myData.Length; i++)
            {
                stringBuilder.Append(myData[i].ToString("x"));
            }

            return string.Format(stringBuilder.ToString());
        }
    }
}