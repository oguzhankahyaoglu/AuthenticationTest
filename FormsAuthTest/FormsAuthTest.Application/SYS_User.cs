using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FormsAuthTest.Application
{
    public partial class SYS_User: IIdentity
    {
        public SYS_User()
        {
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string ImageFile { get; set; }
        public string Description { get; set; }
        public bool IsVisibleOnRightMenu { get; set; }

        public string Name {
            get { return Username; }
            private set { throw new NotImplementedException();}
        }
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated { get; private set; }
    }
}
