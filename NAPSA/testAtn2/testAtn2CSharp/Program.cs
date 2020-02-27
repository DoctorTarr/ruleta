using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace testAtn2CSharp
{
    class Program
    {
        static float atan2_approximation1(float y, float x)
        {
            //http://pubs.opengroup.org/onlinepubs/009695399/functions/atan2.html
            //Volkan SALMA

            const float ONEQTR_PI = (float)Math.PI / 4.0f;
            const float THRQTR_PI = 3.0f * (float)Math.PI / 4.0f;
            float r, angle;
            float abs_y = Math.Abs(y) + 1e-10f;      // kludge to prevent 0/0 condition
            if (x < 0.0f)
            {
                r = (x + abs_y) / (abs_y - x);
                angle = THRQTR_PI;
            }
            else
            {
                r = (x - abs_y) / (x + abs_y);
                angle = ONEQTR_PI;
            }
            angle += (0.1963f * r * r - 0.9817f) * r;
            if (y < 0.0f)
                return (-angle);     // negate if in quad III or IV
            else
                return (angle);
        }


        const float PI_FLOAT = 3.14159265f;
        const float PIBY2_FLOAT = 1.5707963f;
        // |error| < 0.005
        static float atan2_approximation2(float y, float x)
        {
            if (x == 0.0f)
            {
                if (y > 0.0f) return PIBY2_FLOAT;
                if (y == 0.0f) return 0.0f;
                return -PIBY2_FLOAT;
            }
            float atan;
            float z = y / x;
            if (Math.Abs(z) < 1.0f)
            {
                atan = z / (1.0f + 0.28f * z * z);
                if (x < 0.0f)
                {
                    if (y < 0.0f) return atan - PI_FLOAT;
                    return atan + PI_FLOAT;
                }
            }
            else
            {
                atan = PIBY2_FLOAT - z / (z * z + 0.28f);
                if (y < 0.0f) return atan - PI_FLOAT;
            }
            return atan;
        }

        private static double radian = 180.0 / Math.PI;

        /**
        * Determines the angle of a point against the coordinates center
        */
        private static int GetAngleOfPointToZero(int x, int y)
        {
            return (int)Math.Round(Math.Atan2(y, x) * radian);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Testing atan2 atan2_aprox1 and atan2_aprox2 in C#\n");
            Stopwatch lStopwatch = new Stopwatch();


            //float x = 1;
            //float y = 0;
            //float angle = 0.0f;
            //Console.WriteLine("Start Math.Atn2() test\n");
            lStopwatch.Start();
            //for (y = 0; y < 2 * Math.PI; y += 0.1f)
            //{
            //    for (x = 0; x < 2 * Math.PI; x += 0.1f)
            //    {
            //        angle = (float)Math.Atan2(y, x);
            //    }
            //}
            //lStopwatch.Stop();
            ////Console.WriteLine("Stop Math.Atn2() test\n");
            //long elapsedTime = lStopwatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
            //Console.WriteLine("RunTime Math.Atn2() " + elapsedTime);

            ////Console.WriteLine("\n\nStart atan2_approximation1() test\n");
            //lStopwatch.Restart();

            //for (y = 0; y < 2 * Math.PI; y += 0.1f)
            //{
            //    for (x = 0; x < 2 * Math.PI; x += 0.1f)
            //    {
            //        angle = atan2_approximation1(y, x);
            //    }
            //}
            //lStopwatch.Stop();
            ////Console.WriteLine("Stop atan2_approximation1() test\n");
            //elapsedTime = lStopwatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));

            //Console.WriteLine("RunTime atn2_aprox1() " + elapsedTime);

            ////Console.WriteLine("\n\nStart atan2_approximation1() test\n");
            //lStopwatch.Restart();

            //for (y = 0; y < 2 * Math.PI; y += 0.1f)
            //{
            //    for (x = 0; x < 2 * Math.PI; x += 0.1f)
            //    {
            //        angle = atan2_approximation2(y, x);
            //    }
            //}

            int n = 89;
            int x, y;
            int iAngle;
 
            for (y = -n - 1; y <= n; y++)
            {
                for (x = -n - 1; x <= n; x++)
                {
                    iAngle = GetAngleOfPointToZero(y, x);
                    Console.WriteLine($"x: {x} y: {y} = {iAngle}");
                }
            }
            lStopwatch.Stop();

            long elapsedTime = lStopwatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));

            Console.WriteLine("RunTime GetAngleOfPointToZero() " + elapsedTime);

            Console.ReadKey();
        }
    }
}
