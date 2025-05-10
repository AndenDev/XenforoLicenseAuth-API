using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserAuthenticate
    {
        public int UserId { get; set; }
        public string SchemeClass { get; set; }
        public byte[] Data { get; set; }

        // Navigation
        public User User { get; set; }
    }

}
