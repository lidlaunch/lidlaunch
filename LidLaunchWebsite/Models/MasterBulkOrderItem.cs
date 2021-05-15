using System.Collections.Generic;
using System;

namespace LidLaunchWebsite.Models
{
    public class MasterBulkOrderItem
    {
        public int Id { get; set; }
        public string ItemStyle { get; set; }
        public string ItemName { get; set; }
        public bool OSFA { get; set; }
        public bool OSFAStock { get; set; }
        public bool LXL { get; set; }
        public bool LXLStock { get; set; }
        public bool SM { get; set; }
        public bool SMStock { get; set; }
        public bool XLXXL { get; set; }
        public bool XLXXLStock { get; set; }
        public string ItemColor { get; set; }
        public string Manufacturer { get; set; }
        public bool Available { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Cost { get; set; }
        public string SKU { get; set; }
        public string ThumbnailpreviewImagePath { get; set; }
        public string PreviewImagePath { get; set; }
        public string DistributorLink { get; set; }
        public int DisplayOrder { get; set; }
        public string FrontEndName { get; set; }
    }

    public class BulkOrderHatSelectModel
    {
        public List<MasterBulkOrderItem> FlexFit6277Items { get; set; }
        public List<MasterBulkOrderItem> FlexFit6511Items { get; set; }
        public List<MasterBulkOrderItem> FlexFit110Items { get; set; }
        public List<MasterBulkOrderItem> FlexFit6297Items { get; set; }
        public List<MasterBulkOrderItem> Yupoong6089Items { get; set; }
        public List<MasterBulkOrderItem> YupoongDadCapItems { get; set; }
        public List<MasterBulkOrderItem> Yupoong6606Items { get; set; }
        public List<MasterBulkOrderItem> Yupoong6006Items { get; set; }
        public List<MasterBulkOrderItem> YupoongShortBeanieItems { get; set; }
        public List<MasterBulkOrderItem> YupoongCuffedBeanieItems { get; set; }
        public List<MasterBulkOrderItem> Richardson112Items { get; set; }
        public List<MasterBulkOrderItem> ClassicCapsItems { get; set; }
    }
}