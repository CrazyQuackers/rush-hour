using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushHour
{
    internal class Car
    {
        public int length;
        public int row;
        public int column;
        public bool isHorizontal;
        public int color;

        public Car(int length, int row, int column, bool isHorizontal, int color)
        {
            this.length = length;
            this.row = row;
            this.column = column;
            this.isHorizontal = isHorizontal;
            this.color = color;
        }

        public Car(Car other)
        {
            this.length = other.length;
            this.row = other.row;
            this.column = other.column;
            this.isHorizontal = other.isHorizontal;
            this.color = other.color;
        }
    }
}