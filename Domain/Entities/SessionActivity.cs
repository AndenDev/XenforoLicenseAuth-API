using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SessionActivity
    {
        public int UserId { get; set; }
        public byte[] UniqueKey { get; set; }
        public byte[] Ip { get; set; }
        public string ControllerName { get; set; }
        public string ControllerAction { get; set; }
        public string ViewState { get; set; }
        public byte[] Params { get; set; }
        public int ViewDate { get; set; }
        public byte[] RobotKey { get; set; }
    }

}
