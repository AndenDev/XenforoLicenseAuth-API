using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Log
    {
        public int LogId { get; set; }
        public string? Message { get; set; }
        public string? MessageTemplate { get; set; }
        public LogLevel Level { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? Exception { get; set; }
        public string? LogEvent { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public string? TraceId { get; set; }
        public string? CorrelationId { get; set; }
    }
}
