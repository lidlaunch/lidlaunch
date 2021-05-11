
using System;

namespace LidLaunchWebsite.Models
{
    public class BulkOrderAttachment
    {
        public int Id { get; set; }
        public int BulkOrderId { get; set; }
        public string AttachmentType { get; set; }
        public string AttachmentName{ get; set; }
        public string AttachmentPath { get; set; }
        public DateTime UploadDate { get; set; }        
        public bool Deleted { get; set; }
    }
}