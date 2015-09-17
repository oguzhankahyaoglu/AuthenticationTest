using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Kahia.Common.Extensions.StringExtensions;

namespace FormsAuthTest.Application
{
    public class MyPrincipal : IPrincipal
    {
        public MyPrincipal(SYS_User identity, string roles)
        {
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            return ((SYS_User) Identity).Roles.SplitAndEliminateEmptyStrings(',').Contains(role);
        }

        public IIdentity Identity { get; private set; }
    }
}
