﻿using System.Collections.Generic;

namespace LidLaunchWebsite.Models
{
    public class Design
    {
        public int Id { get; set; }
        public string ArtSource { get; set; }
        public string PreviewImage { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal EmbroideredWidth { get; set; }
        public decimal EmbroideredHeight { get; set; }
        public decimal EmbroideredX { get; set; }
        public decimal EmbroideredY { get; set; }
        public string DigitizedFile { get; set; }
        public string DigitizedPreview { get; set; }
        public string DigitizedProductionSheet { get; set; }
        public string EMBFile { get; set; }
        public List<Note> lstNotes { get; set; }
        public List<Note> lstRevisionNotes { get; set; }
        public List<Note> lstCombinedNotes { get; set; }
        public bool CustomerApproved { get; set; }
        public bool InternallyApproved { get; set; }
        public bool Revision { get; set; }
        public string RevisionStatus { get; set; } //4:InternalChangesPending //5:OutsourcedChangesPending //2:RevisionChangesDone //3:AwaitingCustomerApproval //1:Pending
        public string Name { get; set; }
        public string StitchoutImage { get; set; }
        public bool StitchoutApproved { get; set; }
        public bool PredigitizingApproved { get; set; }
        public int StichCount { get; set; }
        public bool Deleted { get; set; }
    }
}