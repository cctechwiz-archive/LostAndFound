using System;
using System.Collections.Generic;

namespace LostAndFound.Models
{
    public abstract class Item
    {
        public DateTime DateReported { get; set; }

        public List<DescriptionTag> DescriptionTags { get; set; }

        public List<LocationTag> LocationTags { get; set; }

        public List<StorageTag> StorageTags { get; set; } 

        public string Recorder { get; set; }
    }
}
