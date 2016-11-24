using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Limestone
{
    public class Options
    {
        public static bool DEBUGDRAWCAMERACLAMP = false;
        public static bool DEBUGDRAWENEMYHITBOXES = false;
        public static bool DEBUGDRAWPROJECTILEHITBOXES = false;
        public static bool DEBUGDRAWCOLLECTABLEHITBOXES = false;
        public static bool DEBUGDRAWNPCHITBOXES = false;
        public static bool DEBUGDRAWNPCINTERACTIONRADIUS = false;

        public static Keys KEYMOVEUP = Keys.W;
        public static Keys KEYMOVERIGHT = Keys.D;
        public static Keys KEYMOVEDOWN = Keys.S;
        public static Keys KEYMOVELEFT = Keys.A;
    }
}
