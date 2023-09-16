using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushHour
{
    internal class Move
    {
        public int color;
        public char direction;
        public int distance;
        public int carLength;

        public Move(int color, char direction, int distance, int carLength)
        {
            this.color = color;
            this.direction = direction;
            this.distance = distance;
            this.carLength = carLength;
        }

        public Move(Move other)
        {
            this.color = other.color;
            this.direction = other.direction;
            this.distance = other.distance;
            this.carLength = other.carLength;
        }

        public bool isSame(Move other)
        {
            return this.color == other.color && this.direction.ToString().Equals(other.direction.ToString());
        }

        public bool isOppositeDirections(Move other)
        {
            if(this.color == other.color)
                if ((this.direction.ToString().Equals("R") && other.direction.ToString().Equals("L")) || (this.direction.ToString().Equals("L") && other.direction.ToString().Equals("R")) || (this.direction.ToString().Equals("U") && other.direction.ToString().Equals("D")) || (this.direction.ToString().Equals("D") && other.direction.ToString().Equals("U")))
                    return true;
            return false;
        }
    }
}