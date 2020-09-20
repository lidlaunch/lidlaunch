
using System;

namespace LidLaunchWebsite.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool CustomerAdded { get; set; }
        public string Attachment { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
    }
}