using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Limestone
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerSave
    {
        [JsonProperty]
        public string map = "none";

        public PlayerSave(string location = "none")
        {
            map = location;
        }
    }
}
