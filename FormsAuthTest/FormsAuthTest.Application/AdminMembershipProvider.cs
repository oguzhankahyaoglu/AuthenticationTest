using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web.Security;
using FormsAuthTest.Application.Helpers;
using Kahia.Common.Extensions.StringExtensions;

namespace FormsAuthTest.Application
{
    public class AdminMembershipProvider : MembershipProvider
    {
        private static List<SYS_User> UserList = new List<SYS_User>
                                                 {
                                                     new SYS_User{Username = "oguzhan", Password = "77551144", Roles = "SuperAdmin"}
                                                 };

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
        }

        #region Not Implemented Properties

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException("MinRequiredPasswordLength değeriyle ilgisi olmamalı hiçbir şekilde provider'ın, password hatırlatma gibi bir mekanizma olmadığından."); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException("MinRequiredNonAlphanumericCharacters değeriyle ilgisi olmamalı hiçbir şekilde provider'ın, password hatırlatma gibi bir mekanizma olmadığından."); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException("PasswordStrengthRegularExpression değeriyle ilgisi olmamalı hiçbir şekilde provider'ın, password hatırlatma gibi bir mekanizma olmadığından."); }
        }

        #endregion

        #region Not Implemented Methods

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            Debug.WriteLine("[AdminMembershipProvider] CreateUser: un:'{0}', pw:'{1}'".FormatString(username, password));
            throw new NotImplementedException("CreateUser metodunu kimse çağırmamalı, admin'den ekleniyor userlar zaten?");
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException("ChangePasswordQuestionAndAnswer metodunu kimse çağırmamalı, admin'den ekleniyor userlar zaten?");
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException("GetPassword metodunu kimse çağırmamalı, admin'den ekleniyor userlar zaten?");
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException("ChangePassword metodunu kimse çağırmamalı, admin'den ekleniyor userlar zaten?");
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException("ResetPassword metodunu kimse çağırmamalı, admin'den ekleniyor userlar zaten?");
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException("UpdateUser metodunu kimse çağırmamalı, admin'den ekleniyor userlar zaten?");
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException("UnlockUser metodunu kimse çağırmamalı, admin'den ekleniyor userlar zaten?");
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException("GetUserNameByEmail metodunu kimse çağırmamalı, admin'den ekleniyor userlar zaten?");
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException("DeleteUser metodunu kimse çağırmamalı, admin'den ekleniyor userlar zaten?");
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override bool ValidateUser(string username, string password)
        {
            SYS_User resultUser;
            //var encrypted = EncryptionHelper.Encrypt(password.ToStringByDefaultValue());
            //using (var context = new KahiaDataEntities())
            //{
            //    resultUser = context.SYS_User.FirstOrDefault(u => u.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase) && u.Password.Equals(encrypted, StringComparison.CurrentCultureIgnoreCase));
            //    if (resultUser != null)
            //    {
            //        resultUser.LastLoginDate = DateTime.Now;
            //        context.SaveChanges();
            //    }
            //    //resultUser = context.FetchWithoutEnumerating("SYS_User").Where("Username.Equals(@0,@1)", username, StringComparison.InvariantCultureIgnoreCase).FirstOrDefault() as SYS_User;
            //}
            resultUser = UserList.Find(u => u.Username == username && u.Password == password);
            return resultUser != null;
        }

        public override bool EnablePasswordRetrieval { get { return false; } }

        public override bool EnablePasswordReset { get { return false; } }

        public override bool RequiresQuestionAndAnswer { get { return false; } }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return 2;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                Debug.WriteLine("[AdminMemberShipProvider] birisi üst üste yanlış şifreler girdi ve 60 dakika bloklandı.");
                return 60;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Encrypted; }
        }

    }
}
