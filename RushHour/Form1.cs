using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace RushHour
{
    public partial class Form1 : Form
    {

        int[,] board = new int[8, 8]; //1 for a wall, 0 for empty spaces, 2-17 for cars
        Color[] colors = new Color[18] { Color.DarkGray, Color.Gray, Color.White, Color.Red, Color.MediumSpringGreen, Color.ForestGreen, Color.SteelBlue, Color.Aqua, Color.DarkViolet, Color.DeepPink, Color.Magenta, Color.Maroon, Color.SaddleBrown, Color.Black, Color.DarkOrange, Color.Yellow, Color.Lime, Color.Blue };
        //dark orange, yellow, lime and blue and trucks. the rest are cars
        Car[] cars = new Car[1] { new Car(0, 0, 0, true, 0) };
        String[] history = new String[1] {""};
        Move[] moves = new Move[1] { new Move(0, 'A', 0, 0) };
        int carLength;
        int carColor = 0;
        bool carHorizontal;
        bool solved = false;
        int currentMove;
        int[,] solvedBoard;

        public Form1()
        {
            InitializeComponent();
            for(int i=0; i<8; i++)
                for(int j=0; j<8; j++)
                {
                    PictureBox space = new PictureBox();
                    space.Name = "space" + i + j;
                    space.Size = new Size(75, 75);
                    int x = 150 + (j * 75);
                    int y = 200 + (i * 75);
                    space.Location = new Point(x, y);
                    if (i == 0 || j == 0 || i == 7 || (j == 7 && i != 3))
                        space.BackColor = Color.Gray;
                    else
                    {
                        space.BackColor = Color.DarkGray;
                        if(j!=7)
                        {
                            space.Cursor = Cursors.Hand;
                            space.Click += new EventHandler(space_Click);
                            space.MouseEnter += new EventHandler(space_MouseEnter);
                            space.MouseLeave += new EventHandler(space_MouseLeave);
                        }
                    }
                    space.BorderStyle = BorderStyle.FixedSingle;
                    Controls.Add(space);
                }
            for(int i=0; i<3; i++)
                for(int j=0; j<12; j++)
                {
                    PictureBox piece = new PictureBox();
                    piece.Name = "piece" + i + j;
                    piece.Size = new Size(50, 50);
                    int x = 900;
                    if(i!=2)
                        x += (50 * j) + ((j/2) * 50);
                    else
                        x += (((j%3)+1) * 50) + (j/3 * 200);
                    int y = 50 + (100 * i);
                    piece.Location = new Point(x, y);
                    if (i != 2)
                        piece.BackColor = colors[(i == 0 ? 2 : 8) + (j/2)];
                    else
                        piece.BackColor = colors[14 + (j/3)];
                    piece.Cursor = Cursors.Hand;
                    piece.Click += new EventHandler(piece_Click);
                    piece.BorderStyle = BorderStyle.FixedSingle;
                    Controls.Add(piece);
                }
            resetBoard();
        }

        private void spawnMoves()
        {
            for(int z=0; z<moves.Length; z++)
            {
                int i = z % 17;
                int j = z / 17;
                Label move = new Label();
                move.Name = "move" + z;
                move.AutoSize = true;
                int x = 1000 + (j * 200);
                int y = 30 + (60 * i);
                move.Location = new Point(x, y);
                move.BackColor = Color.CornflowerBlue;
                move.Font = new Font("Cascadia Mono", 18f, FontStyle.Regular);
                move.Text = ((z + 1).ToString() + ".  " + moves[z].direction + moves[z].distance);
                move.ForeColor = colors[moves[z].color];
                move.Click += new EventHandler(move_Click);
                if (z == 0)
                    move.Cursor = Cursors.Hand;
                Controls.Add(move);
            }
        }

        private void move_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            int number;
            if (label.Name.Length == 5)
                number = int.Parse(label.Name[4].ToString());
            else
                number = int.Parse(label.Name.Substring(4));
            if (currentMove != number)
                return;
            PlayMove();
            Move move = moves[number];
            int x = 0, y = 0;
            for(int i=0; i<8; i++)
                for(int j=0; j<8; j++)
                    if (board[i,j] == move.color)
                    {
                        x = i;
                        y = j;
                        i = 8;
                        j = 8;
                    }
            switch (move.direction)
            {
                case 'R':
                    for(int j=y+move.carLength; j<y+move.carLength+move.distance; j++)
                        board[x, j] = move.color;
                    for (int j = y; j < y + move.distance; j++)
                        board[x, j] = 0;
                    break;
                case 'L':
                    for (int j = y - 1; j > y - 1 - move.distance; j--)
                        board[x, j] = move.color;
                    for (int j = y - 1 + move.carLength; j > y - 1 + move.carLength - move.distance; j--)
                        board[x, j] = 0;
                    break;
                case 'U':
                    for (int i = x - 1; i > x - 1 - move.distance; i--)
                        board[i, y] = move.color;
                    for (int i = x - 1 + move.carLength; i > x - 1 + move.carLength - move.distance; i--)
                        board[i, y] = 0;
                    break;
                case 'D':
                    for (int i = x + move.carLength; i < x + move.carLength + move.distance; i++)
                        board[i, y] = move.color;
                    for (int i = x; i < x + move.distance; i++)
                        board[i, y] = 0;
                    break;
            }
            colorBoard();
            currentMove++;
            label.Cursor = Cursors.Default;
            String str = label.Text;
            if (currentMove + 1 > 9)
                label.Text = "✓" + str.Substring(3);
            else
                label.Text = "✓" + str.Substring(2);
            if (currentMove != moves.Length)
                findMove(currentMove).Cursor = Cursors.Hand;
            else
                perform();
        }

        private void resetBoard()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (i == 0 || j == 0 || i == 7 || (j == 7 && i != 3))
                        board[i, j] = 1;
                    else
                        board[i, j] = 0;
        }

        private void space_Click(object sender, EventArgs e)
        {
            if (solved)
                return;
            PictureBox space = (PictureBox)sender;
            int i = int.Parse(space.Name[5].ToString());
            int j = int.Parse(space.Name[6].ToString());
            if(space.BackColor != colors[0] && space.BackColor != colors[1])
                if (board[i,j] == 0)
                {
                    PlaySpace();
                    cars = increaseCars(cars, new Car(carLength, i, j, carHorizontal, carColor));
                    if (carHorizontal)
                        for (int x = j; x < j + carLength; x++)
                            board[i, x] = carColor;
                    else
                        for (int x = i; x < i + carLength; x++)
                            board[x, j] = carColor;
                    carColor = 0;
                }
                else
                {
                    PlayPiece();
                    if(carColor!=0)
                        returnPiece(carColor);
                    int carRow = 0, carColumn = 0;
                    for(int x=0; x<cars.Length; x++)
                        if (cars[x].color == board[i,j])
                        {
                            carColor = cars[x].color;
                            carLength = cars[x].length;
                            carHorizontal = cars[x].isHorizontal;
                            carRow = cars[x].row;
                            carColumn = cars[x].column;
                            break;
                        }
                    if (carHorizontal)
                        for (int x = carColumn; x < carColumn + carLength; x++)
                            board[i, x] = 0;
                    else
                        for (int x = carRow; x < carRow + carLength; x++)
                            board[x, j] = 0;
                    cars = removeCar(cars, carColor);
                }
        }

        private void colorBoard()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    findSpace(i, j).BackColor = colors[board[i, j]];
        }

        private void space_MouseLeave(object sender, EventArgs e)
        {
            colorBoard();
        }

        private void space_MouseEnter(object sender, EventArgs e)
        {
            colorBoard();
            PictureBox space = (PictureBox)sender;
            int i = int.Parse(space.Name[5].ToString());
            int j = int.Parse(space.Name[6].ToString());
            if (carColor!=0 && i>=1 && i<=7 && j>=1 && j<=7)
                if(carHorizontal && ((carColor==2 && i==3)||(carColor!=2&&i!=3)))
                {
                    int c = 0;
                    for (int x = j; x < j + carLength; x++)
                    {
                        if (x < 7 && board[i, x] == 0)
                            c++;
                    }
                    if(c==carLength)
                        for(int x=j; x<j + carLength; x++)
                            findSpace(i, x).BackColor = colors[carColor];
                }
                else if(!carHorizontal)
                {
                    int c = 0;
                    for (int x = i; x < i + carLength; x++)
                    {
                        if (x < 7 && board[x, j] == 0)
                            c++;
                    }
                    if (c == carLength)
                        for (int x = i; x < i + carLength; x++)
                            findSpace(x, j).BackColor = colors[carColor];
                }
        }

        private void piece_Click(object sender, EventArgs e)
        {
            if (solved)
                return;
            PlayPiece();
            PictureBox piece = (PictureBox)sender;
            int i = int.Parse(piece.Name[5].ToString());
            int j = piece.Name.Length == 7 ? int.Parse(piece.Name[6].ToString()) : int.Parse(piece.Name.Substring(6));
            int color;
            if (i != 2)
                color = (i == 0 ? 2 : 8) + (j / 2);
            else
                color = 14 + (j / 3);
            if (carColor != 0)
                returnPiece(carColor);
            carColor = color;
            carLength = (i == 2) ? 3 : 2;
            carHorizontal = true;
            int p = (i / 2) + 2;
            for (int x = 0; x < 12; x++)
                if (x / p == j / p)
                    findPiece(i, x).Visible = false;
        }

        private void returnPiece(int color)
        {
            color -= 2;
            int i = 2, j;
            if (color < 12)
                if (color < 6)
                    i = 0;
                else
                    i = 1;
            if (i != 2)
            {
                color = color % 6;
                j = color * 2;
            }
            else
            {
                color = color % 12;
                j = color * 3;
                findPiece(i, j + 2).Visible = true;
            }
            findPiece(i, j).Visible = true;
            findPiece(i, j + 1).Visible = true;
        }

        private void solveButton_Click(object sender, EventArgs e)
        {
            PlayClick();
            if (findPiece(0, 0).Visible || carColor != 0)
            {
                MessageBox.Show("Can't Solve yet.\nMake sure the white car is on the board\nand make sure you're not holding any car.");
                PlayClick();
                return;
            }
            cars = removeCar(cars, 0);
            if (solveRushHourUtil())
            {
                solveButton.Visible = false;
                solved = true;
                board[3, 5] = 0;
                board[3, 7] = 2;
                solvedBoard = new int[8, 8];
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        solvedBoard[i, j] = board[i, j];
                for (int x = 0; x < cars.Length; x++)
                    if (cars[x].color == 2)
                    {
                        cars[x].column = 6;
                        break;
                    }
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        board[i,j] = backColorToBoard(i, j);
                solveMessage.Text = "Solved!";
                moves = removeFirstMove(moves);
                moves[moves.Length - 1].distance++;
                cleanMovesUtil(0);
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 12; j++)
                        findPiece(i, j).Visible = false;
                instructionsLabel.Text = "Click on the moves to perform them";
                currentMove = 0;
                spawnMoves();
                performButton.Visible = true;
            }
            else
            {
                solveMessage.Text = "Couldn't Solve!";
            }
        }

        private int backColorToBoard(int i, int j)
        {
            PictureBox space = findSpace(i, j);
            for (int x = 0; x < colors.Length; x++)
                if (space.BackColor == colors[x])
                    return x;
            return 0;
        }

        private void cleanMovesUtil(int c)
        {
            removeMoves(c);
            for (int x=0; x<moves.Length-1; x++)
            {
                if (moves[x].isSame(moves[x+1]))
                {
                    moves[x].distance += moves[x+1].distance;
                    moves[x+1] = new Move(0, 'A', 0, 0);
                    cleanMovesUtil(1);
                    return;
                }
                if (x+1<moves.Length && moves[x].isOppositeDirections(moves[x+1]))
                {
                    if (x + 1 < moves.Length && moves[x].distance == moves[x+1].distance)
                    {
                        moves[x] = new Move(0, 'A', 0, 0);
                        moves[x + 1] = new Move(0, 'A', 0, 0);
                        cleanMovesUtil(2);
                        return;
                    }
                    if (x + 1 < moves.Length && moves[x].distance > moves[x + 1].distance)
                    {
                        moves[x].distance -= moves[x + 1].distance;
                        moves[x + 1] = new Move(0, 'A', 0, 0);
                        cleanMovesUtil(1);
                        return;
                    }
                    moves[x + 1].distance -= moves[x].distance;
                    moves[x] = new Move(0, 'A', 0, 0);
                    cleanMovesUtil(1);
                    return;
                }
                if (x+2<moves.Length && moves[x].isOppositeDirections(moves[x+2]))
                {
                    if (x + 2 < moves.Length && moves[x].distance == moves[x + 2].distance)
                    {
                        moves[x] = new Move(0, 'A', 0, 0);
                        moves[x + 2] = new Move(0, 'A', 0, 0);
                        cleanMovesUtil(2);
                        return;
                    }
                    if (x + 2 < moves.Length && moves[x].distance > moves[x + 2].distance)
                    {
                        moves[x].distance -= moves[x + 2].distance;
                        moves[x + 2] = new Move(0, 'A', 0, 0);
                        cleanMovesUtil(1);
                        return;
                    }
                    moves[x + 2].distance -= moves[x].distance;
                    moves[x] = new Move(0, 'A', 0, 0);
                    cleanMovesUtil(1);
                    return;
                }
            }
        }

        private void removeMoves(int c)
        {
            Move[] newMoves = new Move[moves.Length - c];
            c = 0;
            for (int x = 0; x < moves.Length; x++)
                if (moves[x].color != 0)
                {
                    newMoves[c] = new Move(moves[x]);
                    c++;
                }
            moves = new Move[newMoves.Length];
            for (int x = 0; x < moves.Length; x++)
                moves[x] = new Move(newMoves[x]);
        }

        private PictureBox findSpace(int i, int j)
        {
            foreach(Control c in this.Controls)
                if(c.GetType().Name.Equals("PictureBox"))
                {
                    PictureBox space = (PictureBox)c;
                    if(space.Name.Equals("space" + i + j))
                        return space;
                }
            return null;
        }

        private PictureBox findPiece(int i, int j)
        {
            foreach(Control c in this.Controls)
                if(c.GetType().Name.Equals("PictureBox"))
                {
                    PictureBox piece = (PictureBox)c;
                    if (piece.Name.Equals("piece" + i + j))
                        return piece;
                }
            return null;
        }

        private Label findMove(int x)
        {
            foreach (Control c in this.Controls)
                if (c.GetType().Name.Equals("Label"))
                {
                    Label move = (Label)c;
                    if (move.Name.Equals("move" + x))
                        return move;
                }
            return null;
        }

        private String stampBoard()
        {
            String stamp = "";
            for (int i = 1; i < 7; i++)
                for (int j = 1; j < 7; j++)
                    stamp = stamp + board[i, j].ToString();
            return stamp;
        }

        private Car[] sendCarToFront(Car[]car, int color)
        {
            for(int x=0; x<car.Length; x++)
                if (car[x].color == color)
                {
                    if (x == 0)
                        return car;
                    Car carPop = new Car(car[x]);
                    for(int z=x-1; z>=0; z--)
                        car[z + 1] = new Car(car[z]);
                    car[0] = new Car(carPop);
                    return car;
                }
            return null;
        }

        private Car[] sendCarToBack(Car[]car, int color)
        {
            for(int x=0; x<car.Length; x++)
                if (car[x].color == color)
                {
                    if (x == car.Length - 1)
                        return car;
                    Car carPop = new Car(car[x]);
                    for (int z = x + 1; z < car.Length; z++)
                        car[z - 1] = new Car(car[z]);
                    car[car.Length - 1] = new Car(carPop);
                    return car;
                }
            return null;
        }

        private Car[] applyHeuristics()
        {
            Car[] newCars = new Car[cars.Length];
            for(int x=0; x<cars.Length; x++)
                newCars[x] = new Car(cars[x]);
            for (int x=0; x<cars.Length; x++)
            {
                if (cars[x].isHorizontal && board[cars[x].row, cars[x].column + cars[x].length] == 0)
                    newCars = sendCarToFront(newCars, cars[x].color);
                else
                    newCars = sendCarToBack(newCars, cars[x].color);
            }
            for (int x = 0; x < cars.Length; x++)
                if (!cars[x].isHorizontal && board[cars[x].row - 1, cars[x].column] == 0)
                    newCars = sendCarToFront(newCars, cars[x].color);
            for (int x = 0; x < cars.Length; x++)
                if (cars[x].isHorizontal && cars[x].color != 2 && board[cars[x].row, cars[x].column - 1] == 0)
                    newCars = sendCarToFront(newCars, cars[x].color);
            for (int x = 0; x < cars.Length; x++)
                if (!cars[x].isHorizontal && cars[x].length == 3 && board[cars[x].row + 3, cars[x].column] == 0)
                    newCars = sendCarToFront(newCars, cars[x].color);
            if ((board[3, 2] == 2 && board[3, 3] == 0) || (board[3, 3] == 2 && board[3, 4] == 0) || (board[3, 4] == 2 && board[3, 5] == 0) || (board[3, 5] == 2 && board[3, 6] == 0))
                newCars = sendCarToFront(newCars, 2);
            else
                newCars = sendCarToBack(newCars, 2);
            return newCars;
        }

        private Car[] increaseCars(Car[]arr, Car car)
        {
            Car[] newArr = new Car[arr.Length + 1];
            for (int x = 0; x < arr.Length; x++)
                newArr[x] = new Car(arr[x]);
            newArr[arr.Length] = new Car(car);
            return newArr;
        }

        private String[] increaseArray(String[]arr, String str)
        {
            String[] newArr = new String[arr.Length + 1];
            for (int x = 0; x < arr.Length; x++)
                newArr[x] = arr[x];
            newArr[arr.Length] = str;
            return newArr;
        }

        private Move[] increaseMoves(Move[]arr, Move move)
        {
            Move[] newArr = new Move[arr.Length + 1];
            for (int x = 0; x < arr.Length; x++)
                newArr[x] = new Move(arr[x]);
            newArr[arr.Length] = new Move(move);
            return newArr;
        }

        private Move[] decreaseMoves(Move[]arr)
        {
            Move[] newArr = new Move[arr.Length - 1];
            for (int x = 0; x < newArr.Length; x++)
                newArr[x] = new Move(arr[x]);
            return newArr;
        }

        private Car[] removeCar(Car[]arr, int color)
        {
            Car[] newArr = new Car[arr.Length - 1];
            int c = 0;
            for(int x=0; x<arr.Length; x++)
                if (arr[x].color != color)
                {
                    newArr[c] = arr[x];
                    c++;
                }
            return newArr;
        }

        private Move[] removeFirstMove(Move[]arr)
        {
            Move[] newArr = new Move[arr.Length - 1];
            for (int x = 1; x < arr.Length; x++)
                newArr[x - 1] = new Move(arr[x]);
            return newArr;
        }

        private int findCar(int color)
        {
            for (int x = 0; x < cars.Length; x++)
                if (cars[x].color == color)
                    return x;
            return 0;
        }

        private bool solveRushHourUtil()
        {
            if (board[3, 6] == 2)
                return true;
            String stamp = stampBoard();
            if (history.Contains(stamp))
                return false;
            history = increaseArray(history, stamp);
            Car[] newCars = applyHeuristics();
            for(int x=0; x<newCars.Length; x++)
            {
                int row = newCars[x].row;
                int column = newCars[x].column;
                int color = newCars[x].color;
                int length = newCars[x].length;
                bool isHorizontal = newCars[x].isHorizontal;
                if (isHorizontal)
                {
                    if (color == 2)
                    {
                        if (column + length < 8 && board[row, column + length] == 0)
                        {
                            board[row, column + length] = color;
                            board[row, column] = 0;
                            cars[findCar(color)].column = column + 1;
                            moves = increaseMoves(moves, new Move(color, 'R', 1, length));
                            if (solveRushHourUtil())
                                return true;
                            moves = decreaseMoves(moves);
                            cars[findCar(color)].column = column;
                            board[row, column + length] = 0;
                            board[row, column] = color;
                        }
                        if (column - 1 >= 0 && board[row,column-1] == 0)
                        {
                            board[row, column - 1 ] = color;
                            board[row, column + length - 1] = 0;
                            cars[findCar(color)].column = column - 1;
                            moves = increaseMoves(moves, new Move(color, 'L', 1, length));
                            if (solveRushHourUtil())
                                return true;
                            moves = decreaseMoves(moves);
                            cars[findCar(color)].column = column;
                            board[row, column - 1] = 0;
                            board[row, column + length - 1] = color;
                        }
                    }
                    else
                    {
                        if (column - 1 >= 0 && board[row, column - 1] == 0)
                        {
                            board[row, column - 1] = color;
                            board[row, column + length - 1] = 0;
                            cars[findCar(color)].column = column - 1;
                            moves = increaseMoves(moves, new Move(color, 'L', 1, length));
                            if (solveRushHourUtil())
                                return true;
                            moves = decreaseMoves(moves);
                            cars[findCar(color)].column = column;
                            board[row, column - 1] = 0;
                            board[row, column + length - 1] = color;
                        }
                        if (column + length < 8 && board[row, column + length] == 0)
                        {
                            board[row, column + length] = color;
                            board[row, column] = 0;
                            cars[findCar(color)].column = column + 1;
                            moves = increaseMoves(moves, new Move(color, 'R', 1, length));
                            if (solveRushHourUtil())
                                return true;
                            moves = decreaseMoves(moves);
                            cars[findCar(color)].column = column;
                            board[row, column + length] = 0;
                            board[row, column] = color;
                        }
                    }
                }
                else
                {
                    if (length == 3)
                    {
                        if (row + length < 8 && board[row + length, column] == 0)
                        {
                            board[row + length, column] = color;
                            board[row, column] = 0;
                            cars[findCar(color)].row = row + 1;
                            moves = increaseMoves(moves, new Move(color, 'D', 1, length));
                            if (solveRushHourUtil())
                                return true;
                            moves = decreaseMoves(moves);
                            cars[findCar(color)].row = row;
                            board[row + length, column] = 0;
                            board[row, column] = color;
                        }
                        if (row - 1 >= 0 && board[row - 1, column] == 0)
                        {
                            board[row - 1, column] = color;
                            board[row + length - 1, column] = 0;
                            cars[findCar(color)].row = row - 1;
                            moves = increaseMoves(moves, new Move(color, 'U', 1, length));
                            if (solveRushHourUtil())
                                return true;
                            moves = decreaseMoves(moves);
                            cars[findCar(color)].row = row;
                            board[row - 1, column] = 0;
                            board[row + length - 1, column] = color;
                        }
                    }
                    else
                    {
                        if (row - 1 >= 0 && board[row - 1, column] == 0)
                        {
                            board[row - 1, column] = color;
                            board[row + length - 1, column] = 0;
                            cars[findCar(color)].row = row - 1;
                            moves = increaseMoves(moves, new Move(color, 'U', 1, length));
                            if (solveRushHourUtil())
                                return true;
                            moves = decreaseMoves(moves);
                            cars[findCar(color)].row = row;
                            board[row - 1, column] = 0;
                            board[row + length - 1, column] = color;
                        }
                        if (row + length < 8 && board[row + length, column] == 0)
                        {
                            board[row + length, column] = color;
                            board[row, column] = 0;
                            cars[findCar(color)].row = row + 1;
                            moves = increaseMoves(moves, new Move(color, 'D', 1, length));
                            if (solveRushHourUtil())
                                return true;
                            moves = decreaseMoves(moves);
                            cars[findCar(color)].row = row;
                            board[row + length, column] = 0;
                            board[row, column] = color;
                        }
                    }
                }
            }
            return false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R && carColor != 0 && carColor != 2)
            {
                PlayRotate();
                carHorizontal = !carHorizontal;
            }
            if (e.KeyCode == Keys.F && carColor != 0)
            {
                PlayRelease();
                returnPiece(carColor);
                carColor = 0;
                colorBoard();
            }
        }

        private void performButton_Click(object sender, EventArgs e)
        {
            PlayClick();
            perform();
        }

        private void perform()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    board[i, j] = solvedBoard[i, j];
            colorBoard();
            performButton.Visible = false;
            instructionsLabel.Text = "";
            for(int x=0; x<moves.Length; x++)
            {
                Label move = findMove(x);
                Controls.Remove(move);
                move.Dispose();
            }
            restartButton.Visible = true;
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            PlayClick();
            instructionsLabel.Text = "Click on the cars to add them to the board          Rotate the car by pressing 'R'         Release the car by pressing 'F'";
            solveMessage.Text = "";
            solveButton.Visible = true;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 12; j++)
                    findPiece(i, j).Visible = true;
            resetBoard();
            colorBoard();
            cars = new Car[1] { new Car(0, 0, 0, true, 0) };
            history = new String[1] { "" };
            moves = new Move[1] { new Move(0, 'A', 0, 0) };
            carColor = 0;
            solved = false;
            restartButton.Visible = false;
        }

        private void warning_Click(object sender, EventArgs e)
        {
            PlayClick();
            MessageBox.Show("Some puzzles are too complicated for the computer\nto solve and will result in an error.");
            PlayClick();
        }

        private void PlayClick()
        {
            System.IO.Stream str = Properties.Resources.click;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }

        private void PlayPiece()
        {
            System.IO.Stream str = Properties.Resources.piece;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }

        private void PlaySpace()
        {
            System.IO.Stream str = Properties.Resources.space;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }

        private void PlayRotate()
        {
            System.IO.Stream str = Properties.Resources.rotate;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }

        private void PlayRelease()
        {
            System.IO.Stream str = Properties.Resources.release;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }

        private void PlayMove()
        {
            System.IO.Stream str = Properties.Resources.move;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }
    }
}