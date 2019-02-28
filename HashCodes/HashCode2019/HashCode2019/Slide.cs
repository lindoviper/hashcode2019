using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    public class Slide
    {
        public int NumberOfPhotos { get; set; }
        public List<Photo> Photos { get; set; }

        public List<string> GetTags()
        {
            return Photos.SelectMany(ph => ph.Tags).ToList();
        }
    }
}
