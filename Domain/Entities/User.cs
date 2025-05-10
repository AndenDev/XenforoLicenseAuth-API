using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int UserGroupId { get; set; }
        public string Email { get; set; }

        // Navigation
        public UserGroup UserGroup { get; set; }
        public UserAuthenticate Authenticate { get; set; }
    }

}
