using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Limestone
{
    public class TextureInfo
    {
        public string name;
        public int texX, texY, texSize;
        public TextureInfo(string name, int texX, int texY, int texSize)
        {
            this.name = name;
            this.texX = texX;
            this.texY = texY;
            this.texSize = texSize;
        }
    }
}
