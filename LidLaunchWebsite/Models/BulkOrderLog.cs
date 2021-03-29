
using System;

namespace LidLaunchWebsite.Models
{
    public class BulkOrderLog
    {
        public int Id { get; set; }
        public int BulkOrderId { get; set; }
        public int UserId { get; set; }
        public string LogEntry { get; set; }
        public DateTime Date { get; set; }        
    }
}