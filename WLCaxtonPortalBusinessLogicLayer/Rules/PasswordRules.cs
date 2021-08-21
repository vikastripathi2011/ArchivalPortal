//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : PasswordRules.cs
// Program Description  : This class provide the passowrd rules for the user.
// Programmed By        : Naushad Ali,Nadeem Ishrat.
// Programmed On        : 03-Jan-2013
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Text.RegularExpressions;

namespace WLCaxtonPortalBusinessLogicLayer.Rules
{
    /// <summary>
    /// This class provide the passowrd rules for the applications.
    /// </summary>
    internal class PasswordRules
    {
        #region Private variables
        private string password = String.Empty;
        private const string allowedCharsCaps = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
        private const string allowedCharsLower = "abcdefghijkmnopqrstuvwxyz";
        private const string allowedCharsNumeric = "0123456789";
        private const string allowedCharsSpecial = "!@#$%^&()_-;~";
        private const string notAllowedChars = ":/\\?[]|<>=,?*{}.`";
        #endregion


        #region Constructor
        internal PasswordRules(string password)
        {
            this.password = password;
        } 
        #endregion

        /// <summary>
        /// To validate password
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        #region Internal methods
        internal bool ValidatePassword(out string messageId, out string message)
        {
            bool isValid = true;
            message = String.Empty;
            
            isValid = CheckPassowrdLength(out messageId, out message);
            if (isValid == false)
                goto Exit;

            isValid = CheckSpecialCharacters(out messageId, out message);
            if (isValid == false)
                goto Exit;

            isValid = CheckFormates(out messageId, out message);
            
            Exit:
            return isValid;
        }

        /// <summary>
        /// To match password patterns
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        internal bool MatchPasswordPattern(string oldPassword, string newPassword)
        {
            bool isMatchedPattern = false;
            int patternLength = 2;

            isMatchedPattern = ValidatePattern(oldPassword, newPassword.Substring(0, patternLength));

            for (Int32 i = patternLength; i < newPassword.Length; i++)
            {
                isMatchedPattern = ValidatePattern(oldPassword, newPassword.Substring(0, i));
                if (isMatchedPattern)
                    return true;
            }

            for (Int32 i = newPassword.Length - 1; i > patternLength; i--)
            {
                isMatchedPattern = ValidatePattern(oldPassword, newPassword.Substring(i - patternLength + 1, newPassword.Length - i + 1));
                if (isMatchedPattern)
                    return true;

            }
                       
            return isMatchedPattern;
        }

        /// <summary>
        /// To validate password patterns
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private static bool ValidatePattern(string value, string pattern)
        {
           bool isMatch = false;
           
            if (value.Contains(pattern))
                return true;
            
            return isMatch;

        }

        /// <summary>
        /// To genrate temp password for the user for first time login
        /// </summary>
        /// <param name="length"></param>
        /// <param name="nonAlphaNumericChars"></param>
        /// <param name="hashPassword"></param>
        /// <returns></returns>
        internal static string GeneratePassword(int length, int nonAlphaNumericChars, out string hashPassword)
        {
            char[] arrPwd = new char[8];
            int randomNumber = -1;
            Random random = new Random();

            randomNumber = random.Next(0, 24);
            arrPwd[0] = Convert.ToChar(allowedCharsCaps.Substring(randomNumber, 1));

            randomNumber = random.Next(0, 24);
            arrPwd[1] = Convert.ToChar(allowedCharsLower.Substring(randomNumber, 1));

            randomNumber = random.Next(0, 7);
            arrPwd[2] = Convert.ToChar(allowedCharsNumeric.Substring(randomNumber, 1));

            randomNumber = random.Next(0, 11);
            arrPwd[3] = Convert.ToChar(allowedCharsSpecial.Substring(randomNumber, 1));

            randomNumber = random.Next(0, 24);
            arrPwd[4] = Convert.ToChar(allowedCharsCaps.Substring(randomNumber, 1));

            randomNumber = random.Next(0, 24);
            arrPwd[5] = Convert.ToChar(allowedCharsLower.Substring(randomNumber, 1));

            randomNumber = random.Next(0, 7);
            arrPwd[6] = Convert.ToChar(allowedCharsNumeric.Substring(randomNumber, 1));

            randomNumber = random.Next(0, 11);
            arrPwd[7] = Convert.ToChar(allowedCharsSpecial.Substring(randomNumber, 1));


            string password = new string(arrPwd);
            
            hashPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            return password;
        } 
        #endregion

        #region Private methods
        /// <summary>
        /// To check password length
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool CheckPassowrdLength(out string messageId, out string message)
        {
            messageId = String.Empty;
            message = String.Empty;
            if (this.password.Length < 8)
            {
                messageId = "M00015";
                message = "The password must be at least 8 characters long";
                return false;
            }
            return true;
        }

        /// <summary>
        /// To check special characters
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool CheckSpecialCharacters(out string messageId, out string message)
        {
            messageId = String.Empty;
            message = String.Empty;

            if (password.IndexOfAny(notAllowedChars.ToCharArray()) > -1)
            {
                messageId = "revNewPassword";
                message = @"Your new password cannot contain any of the following characters - : / \ ? [ ] | < > = , ? * { } . ` ";
                return false;
            }
            return true;
        }

        /// <summary>
        /// To check Formates
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool CheckFormates(out string messageId, out string message)
        {
            int checkCount = 0;

            if (password.IndexOfAny(allowedCharsCaps.ToCharArray()) > -1)
            {
                checkCount++;
            }

            if (password.IndexOfAny(allowedCharsLower.ToCharArray()) > -1)
            {
                checkCount++;
            }

            if (password.IndexOfAny(allowedCharsNumeric.ToCharArray()) > -1)
            {
                checkCount++;
            }

            if (password.IndexOfAny(allowedCharsSpecial.ToCharArray()) > -1)
            {
                checkCount++;

            }

            if (checkCount >= 3)
            {
                messageId = "M00076";
                message = "Success";
                return true;


            }
            else
            {
                messageId = "M00016";
                message = @"The password must contain at least three of the following;<br>
                            •	Upper case character<br>
                            •	Lower case character<br>
                            •	Numeric character<br>
                            •	Punctuation Character including these:  ! @ # $ % ^ & ( ) _ - ; ~ ";
                return false;
            }
        } 
        #endregion        
    }
}
