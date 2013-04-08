using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TowerOffense.Entities
{
    public class TOCircle
    {
        private Vector2 pPosition;
        private int pRadius;

        public TOCircle(Vector2 aPosition, int aRadius)
        {
            pPosition = aPosition;
            pRadius = aRadius;
        }

        public Boolean CollidesWith(TOCircle aCircle)
        {
            return false;
        }

        public Boolean CollidesWith(Rectangle aRectangle)
        {
            return false;
        }
    }
}
