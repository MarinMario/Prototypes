using System;
using System.Numerics;

namespace Src
{
    static class Util
    {
        public static Vector2 AngleToVec(float angle, float length)
        {
            return new Vector2(MathF.Sin(angle / 57.295f), MathF.Cos(angle / 57.295f)) * length;
        }

        public static float VecToAngle(Vector2 vec)
        {
            return MathF.Atan2(vec.Y, vec.X) * 57.295f;
        }
    }
}