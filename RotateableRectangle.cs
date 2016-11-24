using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;

namespace Limestone
{
    public class RotateableRectangle
    {
        private Vector2 topLeft;
        private Vector2 bottomLeft;
        private Vector2 topRight;
        private Vector2 bottomRight;
        public Vector2 source;

        public Vector2 position { get { return topLeft; } set { } }
        public Vector2 sizeWH { get { return new Vector2(width, height); }set { } }
        private Vector2 size;

        public float width { get { return  topRight.X - topLeft.X; } }
        public float height { get { return bottomLeft.Y - topLeft.Y; } }

        public Vector2 center { get { return VectorHelper.GetMidPoint(topLeft, bottomRight); } set { } }

        public float angle;

        //public Vector2 position { get { return topLeft; } set { } }
        public RotateableRectangle(Rectangle rectangle, Vector2 source, float angle)
        {
            topLeft = rectangle.Location.ToVector2();
            bottomLeft = new Vector2(rectangle.Location.X, rectangle.Location.Y + rectangle.Height);
            topRight = new Vector2(rectangle.Location.X + rectangle.Width, rectangle.Location.Y);
            bottomRight = (rectangle.Location + rectangle.Size).ToVector2();
            this.source = source;

            this.size = rectangle.Size.ToVector2();

            this.angle = angle;

            RotateTo(source, angle);
        }

        public RotateableRectangle(Rectangle rectangle)
        {
            topLeft = rectangle.Location.ToVector2();
            bottomLeft = new Vector2(rectangle.Location.X, rectangle.Location.Y + rectangle.Height);
            topRight = new Vector2(rectangle.Location.X + rectangle.Width, rectangle.Location.Y);
            bottomRight = (rectangle.Location + rectangle.Size).ToVector2();

            this.size = rectangle.Size.ToVector2();
        }

        public bool Intersects(RotateableRectangle rect)
        {
            Vector2 normal;    //The normal of the current side.
            //extremely ugly way to do this
            bool topOverlap = false, rightOverlap = false, bottomOverlap = false, leftOverlap = false;
            normal = VectorHelper.GetNormal(topLeft, topRight);    //First: top side
            if (CheckAllProjectedForOverlap(normal, rect))
                topOverlap = true;

            normal = VectorHelper.GetNormal(topRight, bottomRight); //Second: right side
            if (CheckAllProjectedForOverlap(normal, rect))
                rightOverlap = true;

            normal = VectorHelper.GetNormal(bottomRight, bottomLeft);  //bottom
            if (CheckAllProjectedForOverlap(normal, rect))
                bottomOverlap = true;

            normal = VectorHelper.GetNormal(bottomLeft, topLeft);  //left
            if (CheckAllProjectedForOverlap(normal, rect))
                leftOverlap = true;

            if (topOverlap && rightOverlap && bottomOverlap && leftOverlap)
            {
                return true;
            }
            else return false;
        }

        public bool Intersects(Rectangle rectunrotated)
        {
            RotateableRectangle rect = new RotateableRectangle(rectunrotated);
            Vector2 normal;    //The normal of the current side.
            //extremely ugly way to do this
            bool topOverlap = false, rightOverlap = false, bottomOverlap = false, leftOverlap = false;
            normal = VectorHelper.GetNormal(topLeft, topRight);    //First: top side
            if (CheckAllProjectedForOverlap(normal, rect))
                topOverlap = true;

            normal = VectorHelper.GetNormal(topRight, bottomRight); //Second: right side
            if (CheckAllProjectedForOverlap(normal, rect))
                rightOverlap = true;

            normal = VectorHelper.GetNormal(bottomRight, bottomLeft);  //bottom
            if (CheckAllProjectedForOverlap(normal, rect))
                bottomOverlap = true;

            normal = VectorHelper.GetNormal(bottomLeft, topLeft);  //left
            if (CheckAllProjectedForOverlap(normal, rect))
                leftOverlap = true;

            if (topOverlap && rightOverlap && bottomOverlap && leftOverlap)
            {
                return true;
            }
            else return false;
        }

        private bool CheckAllProjectedForOverlap(Vector2 normal, RotateableRectangle rect)
        {
            float dotTopLeft = Vector2.Dot(topLeft, normal);
            float dotTopRight = Vector2.Dot(topRight, normal);
            float dotBottomLeft = Vector2.Dot(bottomLeft, normal);
            float dotBottomRight = Vector2.Dot(bottomRight, normal);

            float[] allcorners = { dotTopLeft, dotTopRight, dotBottomLeft, dotBottomRight};

            float largest = allcorners.Max();
            float smallest = allcorners.Min(); 

            float dotTopLeft2 = Vector2.Dot(rect.topLeft, normal);
            float dotTopRight2 = Vector2.Dot(rect.topRight, normal);
            float dotBottomLeft2 = Vector2.Dot(rect.bottomLeft, normal);
            float dotBottomRight2 = Vector2.Dot(rect.bottomRight, normal);

            float[] allcorners2 = { dotTopLeft2, dotTopRight2, dotBottomLeft2, dotBottomRight2 };
            float largest2 = allcorners2.Max();
            float smallest2 = allcorners2.Min();

            if (largest2 > smallest || largest < smallest2)
            {
                return true;
            }
            else return false;
        }

        public void Move(Vector2 movedir)
        {
            topLeft += movedir;
            topRight += movedir;
            bottomLeft += movedir;
            bottomRight += movedir;
        }

        public void MoveTo(Vector2 moveTo, Vector2 source)
        {   //note that this moves relative to a center position.
            float currentRot = GetCurrentRotation();
            RotateTo(source, 0);
            topLeft = moveTo - (size / 2);
            bottomRight = moveTo + (size / 2);
            topRight  = new Vector2(moveTo.X + (size.X / 2), moveTo.Y - (size.Y / 2));
            bottomLeft = new Vector2(moveTo.X - (size.X / 2), moveTo.Y + (size.Y / 2));

            Rotate(source, currentRot);
        }

        public void MoveTo(Vector2 moveTo)
        {   //note that this moves relative to a center position.
            topLeft = moveTo - (size / 2);
            bottomRight = moveTo + (size / 2);
            topRight = new Vector2(moveTo.X + (size.X / 2), moveTo.Y - (size.Y / 2));
            bottomLeft = new Vector2(moveTo.X - (size.X / 2), moveTo.Y + (size.Y / 2));
        }

        public void Rotate(Vector2 source, float angle)
        {
            Matrix rotateMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(angle));

            Vector2 topLeftR = Vector2.Transform(topLeft - source, rotateMatrix);   //Rotate relative to origin
            topLeft = topLeftR + source; //Move it back to original position
            Vector2 bottomLeftR = Vector2.Transform(bottomLeft - source, rotateMatrix);
            bottomLeft = bottomLeftR + source;
            Vector2 topRightR = Vector2.Transform(topRight - source, rotateMatrix);
            topRight = topRightR + source;
            Vector2 bottomRightR = Vector2.Transform(bottomRight - source, rotateMatrix);
            bottomRight = bottomRightR + source;
        }

        public void RotateTo(Vector2 source, float angle)
        {
            float angleDist = GetCurrentRotation() - angle;

            Matrix rotateMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(angleDist));

            Vector2 topLeftR = Vector2.Transform(topLeft - source, rotateMatrix);   //Rotate relative to origin
            topLeft = topLeftR + source; //Move it back to original position
            Vector2 bottomLeftR = Vector2.Transform(bottomLeft - source, rotateMatrix);
            bottomLeft = bottomLeftR + source;
            Vector2 topRightR = Vector2.Transform(topRight - source, rotateMatrix);
            topRight = topRightR + source;
            Vector2 bottomRightR = Vector2.Transform(bottomRight - source, rotateMatrix);
            bottomRight = bottomRightR + source;
        }

        public static RotateableRectangle Rotate(RotateableRectangle rect, Vector2 source, float angle)
        {
            Matrix rotateMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(angle));

            rect.topLeft = Vector2.Transform(source, rotateMatrix);
            rect.bottomLeft = Vector2.Transform(source, rotateMatrix);
            rect.topRight = Vector2.Transform(source, rotateMatrix);
            rect.bottomRight = Vector2.Transform(source, rotateMatrix);

            return rect;
        }

        public float GetCurrentRotation()
        {
            Vector2 midCheck = VectorHelper.GetMidPoint(topLeft, topRight); //Get the midpoint
            Vector2 normal = VectorHelper.GetNormal(topLeft, topRight) * 32;//Get the normal, multiply by 32 for safety's sake

            float a = VectorHelper.GetAngleBetweenPoints(midCheck, midCheck + normal);//Get the angle between the midpoint and the normal added to the midpoint (since normal is a scalar vector)

            return a;
        }

        public static float GetRectRotation(RotateableRectangle rect)
        {
            Vector2 midCheck = VectorHelper.GetMidPoint(rect.topLeft, rect.topRight); //Get the midpoint
            Vector2 normal = VectorHelper.GetNormal(rect.topLeft, rect.topRight) * 32;//Get the normal, multiply by 32 for safety's sake

            float a = VectorHelper.GetAngleBetweenPoints(midCheck, midCheck + normal);//Get the angle between the midpoint and the normal added to the midpoint (since normal is a scalar vector)
            return a;
        }

        public void DebugDraw(SpriteBatch batch)
        {
            DrawGeometry.DrawLine(batch, topLeft, topRight, Color.OrangeRed, 1);
            DrawGeometry.DrawLine(batch, topRight, bottomRight, Color.OrangeRed, 1);
            DrawGeometry.DrawLine(batch, bottomRight, bottomLeft, Color.OrangeRed, 1);
            DrawGeometry.DrawLine(batch, topLeft, bottomLeft, Color.OrangeRed, 1);

            DrawGeometry.DrawLine(batch, center, bottomRight, Color.Red);
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(topLeft.ToPoint(), new Point((int)width, (int)height));
        }
    }
}
