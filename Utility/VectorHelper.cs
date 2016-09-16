using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Limestone.Utility
{
    public static class VectorHelper
    {
        public static byte[] ConvertVectorToBytes(Vector2 vector)
        {
            float[] vecFloats = new float[] { vector.X, vector.Y };
            byte[] fByte = new byte[8];

            Buffer.BlockCopy(vecFloats, 0, fByte, 0, fByte.Length);

            return fByte;
        }

        public static float FindAngleBetweenTwoPoints(Vector2 A, Vector2 B)
        {
            return MathHelper.ToDegrees((float)Math.Atan2(A.Y - B.Y, A.X - B.X));
        }

        public static Vector2 GetNormal(Vector2 A, Vector2 B)
        {
            return Vector2.Normalize(GetPerp(B - A));
        }

        public static Vector2 GetPerp(Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }

        public static Vector2 ProjectToAxis(Vector2 toProject, string axis)
        {
            Vector2 projected = Vector2.Zero;

            if (axis == "horizontal")
                projected = new Vector2(toProject.X, 0);
            else if (axis == "vertical")
                projected = new Vector2(0, toProject.Y);
            else
                Logger.Log("Could not get axis projection, the given axis string was incorrect. '" + axis + "'", false);

            return projected;
        }

        public static Vector2 ProjectToVector(Vector2 A, Vector2 B)
        {
            Vector2 final;
            float dot = Vector2.Dot(A, B);
            final.X = (dot / (B.X * B.X + B.Y * B.Y)) * B.X;
            final.Y = (dot / (B.X * B.X + B.Y * B.Y)) * B.Y;

            return final;
            //return projection + Vector2.Dot(toProject - projection, Vector2.Normalize(projection)) * Vector2.Normalize(projection);
        }

        public static Vector2 FindLargest(Vector2[] vectors)
        {
            Vector2 longest = Vector2.Zero;
            foreach (Vector2 v in vectors)
            {
                if (v.Length() > longest.Length())
                    longest = v;
            }

            return longest;
        }

        public static Vector2 FindSmallest(Vector2[] vectors)
        {
            Vector2 smallest = vectors[0];
            foreach (Vector2 v in vectors)
            {
                if (v.Length() < smallest.Length())
                    smallest = v;
            }

            return smallest;
        }

        public static Vector2 GetMidPoint(Vector2 A, Vector2 B)
        {
            return (A + B) / 2f;
        }

        public static Vector2 ConvertWorldToScreenCoords(Vector2 worldCoords)
        {
            return worldCoords - Main.camera.Position;
        }

        public static Vector2 ConvertScreenToWorldCoords(Vector2 screenCoords)
        {
            return screenCoords + Main.camera.Position;
        }
    }
}
