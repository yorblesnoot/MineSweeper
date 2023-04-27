using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    internal class BoardMaker
    {
        public void RenderBoard(int[,] knownGrid, int highlightX, int highlightY)
        {
            //spacer character for use between grid elements
            string spacer = "  ";

            int boardSize = knownGrid.GetLength(0);

            //greetings and instructions
            Console.WriteLine("~~~~~~~~Minesweeper~~~~~~~~");
            Console.WriteLine("W A S D: Move the cursor. C: Flag a square. V: Reveal a square.");

            //generate text lines for printing on the screen

            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize-1; y++)
                {
                    //build out each grid line from the known grid values and spacers
                    Console.Write(spacer);
                    //if the currently rendering square is highlighted, change foreground and background for that square
                    if (x == highlightX && y == highlightY)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;                     
                    }
                    Console.Write(TranslateKnown(knownGrid[x, y]));
                    //then change back to default
                    Console.ResetColor();
                    
                }
                Console.Write(spacer);
                //to avoid misaligning the board lines, the last character of each row has to be a writeline
                //theres probably a better way to do this lol
                if (x == highlightX && boardSize - 1 == highlightY)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine(TranslateKnown(knownGrid[x, boardSize - 1]));
                Console.ResetColor();
            }
        }

        public bool[,] GenerateHidden(int size, int frequency)
        {
            bool[,] boardNew = new bool[size, size];
            //initialize the board with random mine placements, default visible tiles
            Random rng = new Random();

            //use rng to generate the minefield

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (rng.Next(frequency + 1) == frequency)
                    {
                        boardNew[x, y] = true;
                    }
                    else
                    {
                        boardNew[x, y] = false;
                    }
                }
            }
            return boardNew;
        }

        public int[,] GenerateKnown(int size)
        {
            int[,] boardNew = new int[size, size];

            //Board states: -3 mine (game over), -2 unknown, -1 flagged, 0+ revealed

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    boardNew[x, y] = -2;
                }
            }
            return boardNew;
        }

        public string TranslateKnown (int value)
        {
            //convert from int board values to string display values
            string output = "";
            if (value == -3)
            {
                //mine
                output = "X";
            }
            else if(value == -2)
            {
                //unknown
                output = "~";
            }
            else if (value == -1)
            {
                //flagged
                output = "!";

            }
            else if (value >= 0)
            {
                //revealed
                output = value.ToString();
            }

            return output;
        }

    }
}
