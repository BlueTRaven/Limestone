using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

namespace Limestone.Component
{
    class SystemSparse : System
    {
        Dictionary<int, Component> dict = new Dictionary<int, Component>();
    }
}
