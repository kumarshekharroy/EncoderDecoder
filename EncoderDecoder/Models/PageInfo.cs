using EncoderDecoder.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncoderDecoder.Models
{
    public class PageInfo
    {
        public string Title { get; set; }
        public string Logo { get; set; }
        public string Header { get; set; } 
        public string Route { get; set; }
        public string Description { get; set; }
        public PageGroup Group { get; set; }
    }
}
