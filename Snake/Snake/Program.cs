using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    class Program
    {
        public struct Coordinate : IEquatable<Coordinate> 
        {
            public int x, y;
            
            public Coordinate(int xcoord, int ycoord)
            {
                x = xcoord;
                y = ycoord;
            }

            // i got a bit carried away implementing all this dumb shit but it taught me how to do it so i guess it was worthwhile

            public static Coordinate operator + (Coordinate a, Coordinate b)
            {
                return new Coordinate(a.x + b.x, a.y + b.y);
            }

            public static Coordinate operator * (Coordinate a, int b)
            // don't multiply a coordinate by a non-int or you'll fuck shit up
            {
                return new Coordinate(a.x * b, a.y * b);
            }

            public static Coordinate operator - (Coordinate a, Coordinate b)
            // TODO: add some exceptions so this can't go out of range
            {
                return new Coordinate(a.x - b.x, a.y - b.y);
            }

            public static bool operator == (Coordinate a, Coordinate b)
            {
                if ((a.x == b.x) && (a.y == b.y))
                    return true;
                else
                    return false;
            }

            public static bool operator != (Coordinate a, Coordinate b)
            {
                if (a == b)
                    return false;
                else
                    return true;
            }

            public bool Equals(Coordinate a)
            {
                if ((a.x == this.x) && (a.y == this.y))
                    return true;
                else
                    return false;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Coordinate)) return false;
                return base.Equals(obj);
            }
            // i don't really know what this does but oh well

            public override int GetHashCode()
            {
                return this.GetHashCode();
            }

        }

        internal static Coordinate food = new Coordinate(10,10);
        internal static List<Coordinate> snake = new List<Coordinate>();
        internal static List<Coordinate> walls = new List<Coordinate>();
        internal static Coordinate screen_size = new Coordinate(79, 40);

        static void Main(string[] args)
        {
            // List<Coordinate> snake = new List<Coordinate>();
            snake.Add(new Coordinate(1, 1));
            // initialize snake with length of 1 at 1,1 (closest poss point to origin)

            walls = InititialiseWalls(screen_size);
            
            food = new Coordinate(10,10); // make random food constructor soon. make it an object i guess

            DrawScreen(walls, snake, food);

            // start the snake moving
            Coordinate motion_direction = new Coordinate(1,0); // start snake moving in the +x direction, w/speed 1
            // nb before fucking with speed implement out of bounds checks
            int time = 0;
            // BEWARE OF INT32 WRAPAROUND BUGS HERE LOL

            while (time < 30)
            // move snake perpetually unless we break out of loop
            {
                DrawScreen(walls, snake, food);

                snake[0] = snake[0] + motion_direction;
                // if hit wall, break

                // if hit food, eat food, length++ (i.e. new coord at last point) snake.add(snake[snake.length-1]) after rest of snake moves

                // move rest of snake
                for (int i = snake.Count - 1; i > 1; i--)
                {
                    // loop backwards along snake moving each coord forward
                    snake[i] = snake[i - 1];
                }

                System.Threading.Thread.Sleep(500);
                time++;
                // each time unit is 0.5s

            }

        }

        static List<Coordinate> InititialiseWalls(Coordinate screen_size)
        {
            int xwidth = screen_size.x;
            int ywidth = screen_size.y;

            // set up the walls on lines x = 0; y = 0; x = 82; y = 82; (to create 80x80 playing space)
            List<Coordinate> walls = new List<Coordinate>();
            for (int i = 1; i < ywidth; i++)
            {
                walls.Add(new Coordinate(0, i));
                // x = 0;
                walls.Add(new Coordinate(xwidth, i));
                // x = width
            }

            for (int i = 1; i < xwidth; i++)
            {
                walls.Add(new Coordinate(i, 0));
                // y = 0;
                
                walls.Add(new Coordinate(i, ywidth));
                // y = width

            }
            walls.Add(new Coordinate(0, 0));
            walls.Add(new Coordinate(0, ywidth));
            walls.Add(new Coordinate(xwidth, 0));
            walls.Add(new Coordinate(xwidth, ywidth));
            return walls;
            
        }

        static void DrawScreen(List<Coordinate> walls, List<Coordinate> snake, Coordinate food)
        {
            Console.SetWindowSize(screen_size.x + 1, screen_size.y + 2);
            Console.Clear();
            // draw walls (u+2588)
            foreach (var point in walls)
            {
                Console.SetCursorPosition(point.x, point.y);
                Console.Write("\u2588");
            }
            // draw snake (u+25aa)
            foreach (var point in snake)
            {
                Console.SetCursorPosition(point.x, point.y);
                Console.Write("@");
            }
            // draw food (u+25cb)
            Console.SetCursorPosition(food.x, food.y);
            Console.Write("o");
            // get the cursor the fuck out
            Console.SetCursorPosition(0, screen_size.y + 1);
        }

    }
}
