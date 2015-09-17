using System;
using System.Security.Principal;

namespace FormsAuthTest.Application
{
    public class AdminUser : SYS_User, IIdentity
    {
        public string Name
        {
            get { return this.Username; }
            private set { throw new NotImplementedException(); }
        }
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated { get; private set; }
    }
}
