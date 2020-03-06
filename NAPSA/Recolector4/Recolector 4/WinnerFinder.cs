using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoRecolector
{
    class WinnerFinder
    {
        
        // Winner number variables
        int winnerDA = -1;
        int winnerXY = -1;

        public WinnerFinder()
        {
            ReadNumbersTable();
        }

        ~WinnerFinder()
        {

        }

        /**
        * Determines the angle of a point against the coordinates center
        */
        public int GetAngleOfPointToZero(System.Drawing.Point p)
        {
            double angle = Math.Round(Math.Atan2(p.Y, p.X) * radian) % 360.0;
            if (angle < 0.0)
                angle = angle + 360.0;

            return (int)angle;
        }


        // Finds the integer square root of a positive number  
        private int Isqrt(int num)
        {
            if (0 == num) { return 0; }  // Avoid zero divide  
            int n = (num / 2) + 1;       // Initial estimate, never low  
            int n1 = (n + (num / n)) / 2;
            while (n1 < n)
            {
                n = n1;
                n1 = (n + (num / n)) / 2;
            } // end while  
            return n;
        } // end Isqrt()  

        // Find distance between zero and ball
        public int FindDistance(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            return Isqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }



        //Roulette wheel number sequence
        //The pockets of the roulette wheel are numbered from 0 to 36.
        //In number ranges from 1 to 10 and 19 to 28, odd numbers are red and even are black.
        //In ranges from 11 to 18 and 29 to 36, odd numbers are black and even are red.
        //There is a green pocket numbered 0 (zero). In American roulette, there is a second green pocket marked 00. 
        //Pocket number order on the roulette wheel adheres to the following clockwise sequence in most casinos

        //Single-zero wheel 
        //0-32-15-19-4-21-2-25-17-34-6-27-13-36-11-30-8-23-10-5-24-16-33-1-20-14-31-9-22-18-29-7-28-12-35-3-26
        public int[,] RouletteNumbers =
        {
           { 0, 2},
           {32, 1}, {15, 0}, {19, 1}, { 4, 0}, {21, 1}, { 2, 0}, {25, 1}, {17, 0}, {34, 1}, { 6, 0},
           {27, 1}, {13, 0}, {36, 1}, {11, 0}, {30, 1}, { 8, 0}, {23, 1}, {10, 0}, { 5, 1}, {24, 0},
           {16, 1}, {33, 0}, { 1, 1}, {20, 0}, {14, 1}, {31, 0}, { 9, 1}, {22, 0}, {18, 1}, {29, 0},
           { 7, 1}, {28, 0}, {12, 1}, {35, 0}, { 3, 1}, {26, 0},
        };

        //Double-zero wheel 
        //0-28-9-26-30-11-7-20-32-17-5-22-34-15-3-24-36-13-1-00-27-10-25-29-12-8-19-31-18-6-21-33-16-4-23-35-14-2
        //Triple-zero wheel 
        //0-000-00-32-15-19-4-21-2-25-17-34-6-27-13-36-11-30-8-23-10-5-24-16-33-1-20-14-31-9-22-18-29-7-28-12-35-3-26

        // Numbers by Angle
        private int[,] NumbersByDistAngle;

        // Numbers by Coordinates XY
        private int[,] NumbersByXY;

        private readonly double radian = 180.0 / Math.PI;

        const int DIFF_DIST = 2;
        const int DIFF_ANGLE = 1;

        public int FindNumberByAngle(int distance, int angle)
        {

            int winner = -1;

            // Angle range at 0 and at 359 are awkard
            if (angle != 0 && angle != 359)
            {
                int min_angle = (angle - DIFF_ANGLE);
                int max_angle = (angle + DIFF_ANGLE);

                // For some reason, Atan2() is 0 when it shouldn't probably
                for (int i = 0; i < 37; i++)
                {

                    if ((NumbersByDistAngle[i, 0] >= (distance - DIFF_DIST)) &&
                        (NumbersByDistAngle[i, 0] <= (distance + DIFF_DIST)))
                    {
                        if ((NumbersByDistAngle[i, 1] >= Math.Min(min_angle, max_angle)) &&
                            (NumbersByDistAngle[i, 1] <= Math.Max(min_angle, max_angle)))
                        {
                            winner = i;
                            break;
                        }
                    }
                }
            }

            return winner;
        }

        // Delta x, y accepted range
        const int DIFF_XY = 3;

        public int FindNumberByXY(int x, int y)
        {
            int winner = -1;
            int min_X = (x - DIFF_XY);
            int max_X = (x + DIFF_XY);
            int min_Y = (y - DIFF_XY);
            int max_Y = (y + DIFF_XY);

            for (int i = 0; i < 37; i++)
            {
                if ((NumbersByXY[i, 0] >= min_X && NumbersByXY[i, 0] <= max_X) &&
                    (NumbersByXY[i, 1] >= min_Y && NumbersByXY[i, 1] <= max_Y))
                {
                    winner = i;
                    break;
                }
            }

            return winner;
        }
 
        public int FindWinnerNumber(Point ZeroPosToCenter, int ZeroAngleToCenter, Point BallPosToCenter, int BallAngleToCenter, JuegoRuleta juego)
        {
            int winner = -1;

            winnerXY = FindNumberByXY(BallPosToCenter.X, BallPosToCenter.Y);
            if (winnerXY != -1)
            {
                juego.SetNewWinnerNumber(winnerXY);
            }

            int DistanceZeroBall = FindDistance(ZeroPosToCenter, BallPosToCenter);
            int CorrectedAngleToCenter = BallAngleToCenter + (ZeroAngleToCenter - 90);

            if (CorrectedAngleToCenter < 0)
                CorrectedAngleToCenter = 359;

            if (CorrectedAngleToCenter > 359)
                CorrectedAngleToCenter = 0;

            winnerDA = FindNumberByAngle(DistanceZeroBall, CorrectedAngleToCenter);
            if (winnerDA != -1)
            {
                juego.SetNewWinnerNumber(winnerDA);
            }

            if ((winnerXY != -1) && (winnerDA != -1) && (winnerXY == winnerDA))
            {
                winner = winnerDA;
                juego.SetNewWinnerNumber(winner);
            }

            return winner;
        }

        public void FindWinnerNumber(Point ZeroPosToCenter, int ZeroAngleToCenter, Point BallPosToCenter, int BallAngleToCenter, ref int winner)
        {
            winnerXY = FindNumberByXY(BallPosToCenter.X, BallPosToCenter.Y);

            int DistanceZeroBall = FindDistance(ZeroPosToCenter, BallPosToCenter);
            int CorrectedAngleToCenter = BallAngleToCenter + (ZeroAngleToCenter - 90);

            if (CorrectedAngleToCenter < 0)
                CorrectedAngleToCenter = 359;

            if (CorrectedAngleToCenter > 359)
                CorrectedAngleToCenter = 0;

            winnerDA = FindNumberByAngle(DistanceZeroBall, CorrectedAngleToCenter);

            if ((winnerXY != -1) && (winnerDA != -1) && (winnerXY == winnerDA))
            {
                winner = winnerDA;
            }
        }

        public void SetNumberCoordinates(int num, int x, int y, int distance, int angle)
        {
            NumbersByXY[num, 0] = x;
            NumbersByXY[num, 1] = y;

            NumbersByDistAngle[num, 0] = distance;
            NumbersByDistAngle[num, 1] = angle;
        }

        public int GetNumberX(int num)
        {
            return NumbersByXY[num, 0];
        }

        public int GetNumberY(int num)
        {
            return NumbersByXY[num, 1];
        }

        public int GetDistance(int num)
        {
            return NumbersByDistAngle[num, 0];
        }

        public int GetAngle(int num)
        {
            return NumbersByDistAngle[num, 1];
        }

        private void ReadNumbersTable()
        {
            // Read numbers data for distance and angle detection
            using (var stream = new StreamReader(@"./dataDA.json"))
            {
                this.NumbersByDistAngle = JsonConvert.DeserializeObject<int[,]>(stream.ReadToEnd());
            }
            // Read numbers' data for ball's X & Y detection
            using (var stream = new StreamReader(@"./dataXY.json"))
            {
                this.NumbersByXY = JsonConvert.DeserializeObject<int[,]>(stream.ReadToEnd());
            }
        }

        public void WriteNumbersTable()
        {
            // write the data (overwrites) distance and angle detection data
            using (var stream = new StreamWriter(@"./dataDA.json", append: false))
            {
                stream.Write(JsonConvert.SerializeObject(this.NumbersByDistAngle));
                stream.Flush();
            }
            // write the data (overwrites) X & Y detection data
            using (var stream = new StreamWriter(@"./dataXY.json", append: false))
            {
                stream.Write(JsonConvert.SerializeObject(this.NumbersByXY));
                stream.Flush();
            }

        }

        public void SaveCSV()
        {
            using (var w = new StreamWriter(@"./dataDA.csv", append: false))
            {
                for (int i = 0; i < 37; i++)
                {
                    var first = NumbersByDistAngle[i, 0];
                    var second = NumbersByDistAngle[i, 1];
                    var third = NumbersByXY[i, 0];
                    var fourth = NumbersByXY[i, 1];
                    string line = string.Format("{0},{1},{2},{3},{4}", first, second, third, fourth, i);
                    w.WriteLine(line);
                }
                w.Flush();
                MessageBox.Show("Archivo CSV generado");
            }

        }

    }
}
