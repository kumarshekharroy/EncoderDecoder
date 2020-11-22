using EncoderDecoder.Models;
using EncoderDecoder.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncoderDecoder.Utilities
{
    public class Pages
    {
        private readonly Dictionary<string, List<PageInfo>> _all_pages;
        public Pages()
        {
            _all_pages = AllPages.GroupBy(x => x.Group).ToDictionary(x => x.Key.ToString(), y => y.ToList());
        }

        private readonly List<PageInfo> AllPages = new List<PageInfo> {
            new PageInfo {
                Group=PageGroup.Timestamp_Tools,
                Header="EpochConverter",
                Description="Epoch & Unix Timestamp Conversion Tools",
                Logo="images/clock.png",
                Title="Epoch Converter",
                Route="epoch-converter"
            },

            new PageInfo {
                Group=PageGroup.Internet_Tools,
                Header="WebSocket Playground",
                Description="WebSocket Playground : Test and Debug web socket endpoints",
                Logo="images/websocket.png",
                Title="WebSocket Playground",
                Route="websocket-playground"
            }, 
            new PageInfo {
                Group=PageGroup.String_Tools,
                Header="String Length Calculator",
                Description="Character Counter : Calculate String Length",
                Logo="images/scale.png",
                Title="String Length Calculator",
                Route="string-length-calculator"
            },

            new PageInfo {
                Group=PageGroup.String_Tools,
                Header="Json Tool",
                Description="Json formatter : Format, Serialize, Deserealize and more",
                Logo="images/json.png",
                Title="Json Tool",
                Route="json-validator-formatter"
            }
        };
        public Dictionary<string, List<PageInfo>> GetAllPages
        {
            get
            {

                return _all_pages;

            }
        }
    }
}
