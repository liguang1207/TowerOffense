using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TowerOffense.Entities
{
    public class TOActor
    {
        protected Vector2 pPosition = Vector2.Zero;
        protected float pVelocity = 0.0f;

        protected Texture2D pTexture = null;
        protected Color pBackgroundColor = Color.White;

        protected TOWorldInfo WorldInfo = TOWorldInfo.Instance;

        protected float pRotation;
        protected TOActor pTarget;

        public static int MAX_WIDTH = 64;

        public TOActor(Vector2 aPosition, Texture2D aTexture)
        {
            pPosition = aPosition;
            pTexture = aTexture;

            WorldInfo.RegisterEntity(this);
        }

        public virtual void Update(GameTime aGameTime)
        {
            double TurnToTarget = GetTurnDifference();
            if (TurnToTarget == 0)
            {
                if (pVelocity > 0)
                {
                    //Update Velocity
                    pPosition.X = (float)Math.Cos(pRotation) * pVelocity;
                    pPosition.Y = (float)Math.Sin(pRotation) * pVelocity;
                }
            }
            else
            {
                pRotation += (TurnToTarget > 0 ? -1 : 1);
            }
        }

        private double GetTurnDifference()
        {
            //pTarget
            return 1.0;
        }

        

        public virtual void Draw(GameTime aGameTime)
        {
            if (pTexture != null)
            {
                SpriteBatch aSpriteBatch = WorldInfo.GetSpriteBatch();
                aSpriteBatch.Draw(pTexture, BoundingBox, new Rectangle?(pTexture.Bounds), pBackgroundColor, pRotation, new Vector2(0, 0), SpriteEffects.None, 0.0f);
            }
        }

        public Vector2 Position
        {
            get { return pPosition; }
            set { pPosition = value; }
        }

        public Vector2 CenterPoint
        {
            get { return new Vector2(pPosition.X + pTexture.Width / 2, pPosition.Y + pTexture.Height / 2); }
        }

        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)pPosition.X, (int)pPosition.Y, pTexture.Width, pTexture.Height); }
        }

        public TOCircle Circle
        {
            get { return new TOCircle(CenterPoint, pTexture.Width / 2); }
        }

        protected double DotProduct(Vector2 A, Vector2 B)
        {
            return Math.Acos(Math.Round((A.X * B.X + A.Y * B.Y) / (Math.Sqrt(A.X * A.X + A.Y * A.Y) * (Math.Sqrt(B.X * B.X + B.Y * B.Y))), 6));
        }

        protected double DistanceTo(Vector2 OtherPosition)
        {
            float Y_diff, X_diff;

            Y_diff = Math.Abs(pPosition.Y) - Math.Abs(OtherPosition.Y);
            X_diff = Math.Abs(pPosition.X) - Math.Abs(OtherPosition.X);

            return Math.Abs(Math.Sqrt(Math.Pow(X_diff, 2) + Math.Pow(Y_diff, 2)));
        }
    }
}
