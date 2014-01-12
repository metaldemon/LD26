using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LD26
{
    class BasicMath
    {
        public static double GetLength(Vector2 startpnt, Vector2 endpnt)
        {
            return (endpnt - startpnt).Length;
        }

        public static double GetRotation(Vector2 startpnt, Vector2 endpnt)
        {
            Vector2 temp = endpnt - startpnt;
            float opposite = temp.Y;
            float adjacent = temp.X;
            double temp2 = MathHelper.RadiansToDegrees((float)Math.Atan2(opposite, adjacent));
            return temp2;
        }
    }
}
