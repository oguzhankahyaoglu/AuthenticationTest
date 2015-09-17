using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using FormsAuthTest.Application.Helpers;
using Kahia.Common.Extensions.StringExtensions;

namespace FormsAuthTest.Application
{
    public class AdminMembershipManager
    {
        //private bool BypassAuthOnLocalRequests = ConfigurationManager.AppSettings["Kahia.Admin.BypassAuthOnLocalRequests"].ThrowIfNull("'Kahia.Admin.BypassAuthOnLocalRequests' appseting'i bulunamadı. Admin bypass mekanizması için gerekli.").ToBoolean();

        //internal SYS_User CurrentUser
        //{
        //    get { return HttpContext.Current.Session["AdminAuthHelper.CurrentUser"] as SYS_User; }
        //    private set { HttpContext.Current.Session["AdminAuthHelper.CurrentUser"] = value; }
        //}

        internal IIdentity CurrentUser
        {
            get { return HttpContext.Current.User.Identity; }
        }

        #region User Fields

        public Boolean IsLoggedIn
        {
            get
            {
                //return CurrentUser != null;
                return HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        //public String[] CurrentUserRoles
        //{
        //    get
        //    {
        //        return IsLoggedIn ? CurrentUser.Roles.SplitAndEliminateEmptyStrings(',') : new string[0];
        //    }
        //}

        public String CurrentUserName { get { return IsLoggedIn ? CurrentUser.Name : null; } }

        public int? CurrentUserID
        {
            get
            {
                //return IsLoggedIn ? CurrentUser.UserID : (int?) null;
                return null;
            }
        }

        public Boolean IsSuperAdmin { get { return HasRole("SuperAdmin"); } }

        #endregion

        #region Role Check

        public Boolean HasRole(String role)
        {
            var user = HttpContext.Current.User;
            return user.IsInRole(role);
        }

        /// <summary>
        /// Roller , ile ayrılmış olmalıdır.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Boolean HasRoles(String roles)
        {
            var roleArr = roles.ToStringByDefaultValue().SplitAndEliminateEmptyStrings(',');
            return roleArr.All(HasRole);
        }

        #endregion

        public AdminAuthManager.LoginStatuses TryToLogin(String username, String password)
        {
            if (Membership.ValidateUser(username, password))
            {
                var user = Membership.GetUser(username);
                //HttpContext.Current.User = new RolePrincipal(user);
                return AdminAuthManager.LoginStatuses.SUCCESS;
            }
            return AdminAuthManager.LoginStatuses.FAIL;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        //public Boolean HasRole(String role)
        //{
        //    return role.IsNullOrEmptyString() || CurrentUserRoles.Contains(role);
        //}

        ///// <summary>
        ///// Roller , ile ayrılmış olmalıdır.
        ///// </summary>
        ///// <param name="roles"></param>
        ///// <returns></returns>
        //public Boolean HasRoles(String roles)
        //{
        //    var roleArr = roles.ToStringByDefaultValue().SplitAndEliminateEmptyStrings(',');
        //    return roleArr.All(HasRole);
        //}

        //public void Logout()
        //{
        //    CurrentUser = null;
        //}

        //internal void SetCurrentUser(SYS_User user)
        //{
        //    CurrentUser = user;
        //    var cookie = FormsAuthentication.GetAuthCookie(user.Username, false);
        //    HttpContext.Current.Response.Cookies.Add(cookie);
        //}

        /// <summary>
        /// Local Request'lerde admin'deki ayara bakılarak logini bypass etmek için.
        /// </summary>
        //public void ByPassLoginWithSuperAdminOnLocalRequests()
        //{
        //    if (BypassAuthOnLocalRequests && !IsLoggedIn && HttpContext.Current.Request.IsLocal)
        //    {
        //        using (var context = new KahiaDataEntities())
        //        {
        //            //var user = context.FetchWithoutEnumerating("SYS_User").Where("Roles.Contains(@0)", "SuperAdmin").Cast<SYS_User>().FirstOrDefault();
        //            var user = FindFirstSuperAdmin(context);
        //            SetCurrentUser(user);
        //        }
        //    }
        //}

        //private SYS_User FindFirstSuperAdmin(KahiaDataEntities context)
        //{
        //    var user = context.SYS_User.FirstOrDefault(u => u.Roles.Contains("SuperAdmin"));
        //    if (user != null)
        //    {
        //        user.LastLoginDate = DateTime.Now;
        //        context.SaveChanges();
        //    }
        //    return user;
        //}

        public String DecryptPassword(Object password)
        {
            return EncryptionHelper.Decrypt(password.ToStringByDefaultValue());
        }

        public String EncryptPassword(Object password)
        {
            return EncryptionHelper.Encrypt(password.ToStringByDefaultValue());
        }

        #region Login Mechanism

        public int IPBanTime = 15;
        public int MaxTrialCount = 5;

        //private Dictionary<String, DateTime?> BannedIPs = new Dictionary<String, DateTime?>();
        //private Dictionary<String, int> TriedIPs = new Dictionary<String, int>();

        //public AdminAuthManager.LoginStatuses TryToLogin(String username, String password)
        //{
        //    SYS_User resultUser = null;
        //    var hashedPass = EncryptPassword(password);
        //    var result = CheckIfBannedAndTryToLogin(username, hashedPass, ref resultUser);
        //    if (result == AdminAuthManager.LoginStatuses.SUCCESS)
        //    {
        //        SetCurrentUser(resultUser);
        //    }
        //    return result;
        //}

        //private Boolean IsIPBanned(String iPAddress)
        //{
        //    if (BannedIPs.ContainsKey(iPAddress) == false)
        //        return false;

        //    var banDate = BannedIPs[iPAddress];
        //    if (banDate == null)
        //        return false;

        //    if (DateTime.Now.Subtract(banDate.Value).Minutes < IPBanTime)
        //    {
        //        return true;
        //    }

        //    BannedIPs.Remove(iPAddress);
        //    return false;
        //}

        //private AdminAuthManager.LoginStatuses CheckIfBannedAndTryToLogin(String username, String password, ref SYS_User resultUser)
        //{
        //    resultUser = null;

        //    //Initially, check if it is banned
        //    var currentIP = HttpContext.Current.FindIpAddressOfClient();
        //    if (IsIPBanned(currentIP))
        //        return AdminAuthManager.LoginStatuses.IP_ALREADY_BANNED;

        //    //If not, add his ip address to tried IPs
        //    if (!TriedIPs.ContainsKey(currentIP))
        //    {
        //        TriedIPs.Add(currentIP, 1);
        //    }

        //    //Check login
        //    using (var context = new KahiaDataEntities())
        //    {
        //        resultUser = context.SYS_User.FirstOrDefault(u => u.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase) && u.Password.Equals(password, StringComparison.CurrentCultureIgnoreCase));
        //        if (resultUser != null)
        //        {
        //            resultUser.LastLoginDate = DateTime.Now;
        //            context.SaveChanges();
        //        }
        //        //resultUser = context.FetchWithoutEnumerating("SYS_User").Where("Username.Equals(@0,@1)", username, StringComparison.InvariantCultureIgnoreCase).FirstOrDefault() as SYS_User;
        //    }

        //    if (resultUser != null)
        //    {
        //        TriedIPs.Remove(currentIP);
        //        BannedIPs.Remove(currentIP);
        //        return AdminAuthManager.LoginStatuses.SUCCESS;
        //    }

        //    //If login failed, increment his trial count and ban if exceeded the MaxTrialCount
        //    var trialCount = ++TriedIPs[currentIP];
        //    if (trialCount > MaxTrialCount)
        //    {
        //        BannedIPs.Add(currentIP, DateTime.Now);
        //        TriedIPs.Remove(currentIP);
        //        return AdminAuthManager.LoginStatuses.FAILBANNED;
        //    }

        //    return AdminAuthManager.LoginStatuses.FAIL;
        //}

        #endregion
    }
}
