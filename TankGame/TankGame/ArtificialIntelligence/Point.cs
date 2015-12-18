using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankGame.ArtificialIntelligence
{
    class Point : IEquatable<Point>
    {
        int x, y, value;
        DirectionConstants lastDirection, firstDirection;

        public DirectionConstants FirstDirection
        {
            get
            {
                return firstDirection;
            }
            set
            {
                firstDirection = value;
            }
        }

        public DirectionConstants LastDirection
        {
            get
            {
                return lastDirection;
            }
            set
            {
                lastDirection = value;
            }
        }

        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                if (value >= 0)
                    y = value;
            }
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                if (value >= 0)
                    x = value;
            }
        }
        public bool Equals(Point other)
        {
            if (this.x == other.x && this.y == other.y)
            {
                return true;
            }
            return false;
        }

    }
}
