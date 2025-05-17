using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.ValidateSession.Command
{
    public class ValidateSessionViewModel
    {
        public bool Success { get; set; }
        public string SessionId { get; set; }
        public SessionUserViewModel User { get; set; }

        public class SessionUserViewModel
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public int UserGroupId { get; set; }
            public string UserGroupName { get; set; }
        }
    }
}
