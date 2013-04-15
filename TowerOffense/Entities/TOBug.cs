using System;
using System.Linq;
using System.Text;
using TowerOffense.Level_Editor;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TowerOffense.Entities
{
    public class TOBug : TOActor
    {
        private List<TOTile> pPath = new List<TOTile>();
        protected double Reward = 1;
        protected int Health = 100;

        public TOBug(Vector2 aPosition, Texture2D aTexture)
            : base(aPosition, aTexture)
        {
            pPath = WorldInfo.Path;
            pVelocity = 2;
        }

        public virtual void Hit(TOProjectile aProjectile)
        {
            Health -= aProjectile.Damage;

            if (Health < 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            WorldInfo.Money += Reward;
            WorldInfo.AllActors.Remove(this);
        }

        public List<TOTile> Path
        {
            get { return pPath; }
            set { pPath = value; }
        }

        public override void Update(GameTime aGameTime)
        {
            if (pTarget != null && pPosition == pTarget.Position)
            {
                if (pPath.Count > 0)
                {
                    pTarget = pPath[0];
                }
                else
                {
                    //WE HAVE HIT AN END ZONE!
                }
            }
            else
            {
                base.Update(aGameTime);
            }

        }

        public override void Draw(GameTime aGameTime)
        {
            //Draw Health Bar
            DrawRectangle(Color.Red, TOTile.MAX_WIDTH, 16);
            DrawRectangle(Color.Green, (int)((TOTile.MAX_WIDTH / 100.0) * Health), 16);

            base.Draw(aGameTime);
        }

        public void DrawRectangle(Color aColor, int aWidth, int aHeight)
        {
            //Rectangle Object
            Texture2D aRectangle = new Texture2D(WorldInfo.GetGame().GraphicsDevice, aWidth, aHeight, false, SurfaceFormat.Color);
            
            //Set The Color
            Color[] color = new Color[aWidth * aHeight];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = aColor;
            }
            aRectangle.SetData(color);

            //Draw It
            WorldInfo.GetGame().GetSpriteBatch().Draw(aRectangle, new Vector2(Position.X, pPosition.Y - 18), aColor);
        }
    }
}
