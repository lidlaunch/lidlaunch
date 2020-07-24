﻿using System.Collections.Generic;

namespace LidLaunchWebsite.Models
{
    public class BulkOrderItem
    {
        public int Id { get; set; }
        public int BulkOrderId { get; set; }
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public decimal ItemCost { get; set; }
        public List<Note> lstNotes { get; set; }
    }
}