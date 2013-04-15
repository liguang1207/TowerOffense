using System;
using System.Linq;
using System.Text;
using TowerOffense.Level_Editor;
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

        public Boolean bDeleteMe = false;
        

        public static int MAX_WIDTH = 64;

        public TOActor(Vector2 aPosition, Texture2D aTexture)
        {
            pPosition = aPosition;
            pTexture = aTexture;

            WorldInfo.RegisterEntity(this);
        }

        public virtual void Update(GameTime aGameTime)
        {
            if (OffWorld())
            {
                bDeleteMe = true;
                WorldInfo.DestroyActor(this);
            }

            if (!bDeleteMe)
            {
                if (pTarget != null)
                {
                    pRotation = GetTurnDifference();
                }

                if (pVelocity > 0)
                {
                    //Update Velocity
                    pPosition.X += (float)Math.Cos(pRotation) * pVelocity;
                    pPosition.Y += (float)Math.Sin(pRotation) * pVelocity;
                }
            }
        }

        public virtual Boolean OffWorld()
        {
            if (CenterPoint.X < 0 || CenterPoint.Y < 0)
                return true;
            else if (CenterPoint.X > TOTile.MAX_WIDTH * 10 || CenterPoint.Y > TOTile.MAX_WIDTH * 10)
                return true;
            else
                return false;
        }


        public float Rotation
        {
            get { return pRotation; }
            set { pRotation = value; }
        }

        protected float GetTurnDifference()
        {
            
            //Calculate the distance from the square to the mouse's X and Y position
            float XDistance = pPosition.X - pTarget.Position.X;
            float YDistance = pPosition.Y - pTarget.Position.Y;

            //Calculate the required rotation by doing a two-variable arc-tan
            return (float)(Math.Atan2(YDistance, XDistance) - Math.PI / 2);

        }

        

        public virtual void Draw(GameTime aGameTime)
        {
            if (!bDeleteMe && pTexture != null)
            {
                SpriteBatch aSpriteBatch = WorldInfo.GetSpriteBatch();
                aSpriteBatch.Draw(pTexture, CenterPoint, null, pBackgroundColor, pRotation, new Vector2(pTexture.Width / 2, pTexture.Height / 2), 1f, SpriteEffects.None, 0);
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
