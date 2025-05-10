using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Session
    {
        public byte[] SessionId { get; set; }
        public byte[] SessionData { get; set; }
        public int ExpiryDate { get; set; }
    }

}
