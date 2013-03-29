using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TowerOffense.Entities
{
    class TOActor
    {
        protected Vector2 pPosition;
        protected TOWorldInfo WorldInfo = TOWorldInfo.Instance;

        public TOActor(Vector2 aPosition)
        {
            pPosition = aPosition;

            WorldInfo.RegisterEntity(this);
        }

        public virtual void Update(GameTime aGameTime)
        {

        }

        public virtual void Draw(GameTime aGameTime)
        {

        }

        public Vector2 Position
        {
            get { return pPosition; }
            set { pPosition = value; }
        }
    }
}
