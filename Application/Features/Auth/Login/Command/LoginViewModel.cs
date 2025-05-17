using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Login.Command
{
    public class LoginViewModel
    {
        public bool Success { get; set; }
        public string SessionId { get; set; }
        public LoginUserViewModel User { get; set; }

        public class LoginUserViewModel
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public int UserGroupId { get; set; }
            public string UserGroupName { get; set; }
        }
    }
}
