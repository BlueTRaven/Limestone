using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;

namespace Limestone
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        internal bool DEBUGDRAWCAMERACLAMP = false;
        internal bool DEBUGDRAWENEMYHITBOXES = false;
        internal bool DEBUGDRAWPROJECTILEHITBOXES = false;
        internal bool DEBUGDRAWPROJECTILEVELVECTOR = false;
        internal bool DEBUGDRAWCOLLECTABLEHITBOXES = false;
        internal bool DEBUGDRAWNPCHITBOXES = false;
        internal bool DEBUGDRAWNPCINTERACTIONRADIUS = false;

        [JsonProperty]
        public Keys KEYMOVEUP = Keys.W;
        public readonly Keys KEYMOVEUPDEFAULT = Keys.W;

        [JsonProperty]
        public Keys KEYMOVERIGHT = Keys.D;
        public readonly Keys KEYMOVERIGHTDEFAULT = Keys.D;

        [JsonProperty]
        public Keys KEYMOVEDOWN = Keys.S;
        public readonly Keys KEYMOVEDOWNDEFAULT = Keys.S;

        [JsonProperty]
        public Keys KEYMOVELEFT = Keys.A;
        public readonly Keys KEYMOVELEFTDEFAULT = Keys.A;

        public Options()
        {   //dummy ctor for json.net

        }

        public void ResetToDefaults()
        {
            KEYMOVEUP = KEYMOVEUPDEFAULT;
            KEYMOVELEFT = KEYMOVELEFTDEFAULT;
            KEYMOVERIGHT = KEYMOVERIGHTDEFAULT;
            KEYMOVEDOWN = KEYMOVEDOWNDEFAULT;
        }
    }
}
