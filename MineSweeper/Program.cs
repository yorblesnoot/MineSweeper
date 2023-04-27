
using MineSweeper;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using static System.Reflection.Metadata.BlobBuilder;

namespace MineSweeper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //board generation parameters
            int mineFrequency = 6;
            int boardSize = 10;
            

            //generate boards
            BoardMaker boardMaker = new BoardMaker();
            bool[,] hiddenBoard = boardMaker.GenerateHidden(boardSize, mineFrequency);
            int[,] knownBoard = boardMaker.GenerateKnown(boardSize);

            int xHighlight = 0;
            int yHighlight = 0;

            bool gameOver = false;
            bool youLose = true;

            boardMaker.RenderBoard(knownBoard, xHighlight, yHighlight);
            while (gameOver == false)
            {
                ConsoleKey keyPress = Console.ReadKey().Key;
                switch (keyPress)
                { 
                    //mine    -3
                    //unknown -2
                    //flagged -1
                    //revealed 0+

                    case (ConsoleKey.W):
                        if (xHighlight > 0)
                        {
                            //go up
                            xHighlight -= 1;
                        }
                        break;

                    case (ConsoleKey.A):
                        if (yHighlight > 0)
                        {
                            //go left
                            yHighlight -= 1;
                        }
                        break;

                    case (ConsoleKey.D):
                        if (yHighlight < boardSize - 1)
                        {
                            //go right
                            yHighlight += 1;
                        }
                        break;

                    case (ConsoleKey.S):
                        if (xHighlight < boardSize - 1)
                        {
                            //go down
                            xHighlight += 1;
                        }
                        break;

                    case (ConsoleKey.C):
                        //flag square
                        if (knownBoard[xHighlight, yHighlight] == -2)
                        {
                            knownBoard[xHighlight, yHighlight] = -1;
                        }
                        else if(knownBoard[xHighlight, yHighlight] == -1)
                        {
                            knownBoard[xHighlight, yHighlight] = -2;
                        }
                        break;

                    case (ConsoleKey.V):
                        //pop square
                        knownBoard = PopSquare(xHighlight, yHighlight, hiddenBoard, knownBoard);
                        //if the square we popped was a mine, game over
                        if (hiddenBoard[xHighlight,yHighlight] == true)
                        {
                            gameOver = true;
                            youLose = true;
                        }
                        break;
                }
                //update the known board
                Console.Clear();
                boardMaker.RenderBoard(knownBoard, xHighlight, yHighlight);
                //end the game if we revealed everything
                if(CheckWin(knownBoard, hiddenBoard) == true)
                {
                    gameOver = true;
                    youLose = false;
                }
            }
            if (youLose == true)
            {
                Console.WriteLine("You hit a mine! GAME OVER.");
            }
            else if(youLose == false)
            {
                Console.WriteLine("You revealed all the tiles without hitting any mines. YOU WIN!");
            }
        }

        public static int[,] PopSquare(int x, int y, bool[,] hiddenBoard, int[,] knownBoard)
        {
            int[,] gridUpdate = knownBoard;
            int size = gridUpdate.GetLength(0);
            //check that we didnt hit a mine
           
            if (hiddenBoard[x, y] == false)
            {
                //check the surrounding squares for mines and count them 
                int bombCount = 0;
                for (int xCheck = -1; xCheck < 2; xCheck++)
                {
                    for (int yCheck = -1; yCheck < 2; yCheck++)
                    {
                        int xtoCheck = x + xCheck;
                        int ytoCheck = y + yCheck;
                        if (xtoCheck >= 0 && xtoCheck < size && ytoCheck >= 0 && ytoCheck < size)
                        {
                            if (hiddenBoard[xtoCheck, ytoCheck] == true)
                            {
                                bombCount++;
                            }
                        }
                    }
                }
                //reveal the number of mines around the chosen square
                knownBoard[x,y] = bombCount;
                //if we didnt find any mines, chain reveal adjacent squares
                if (bombCount == 0)
                {
                    for (int xCheck = -1; xCheck < 2; xCheck++)
                    {
                        for (int yCheck = -1; yCheck < 2; yCheck++)
                        {
                            int xtoCheck = x + xCheck;
                            int ytoCheck = y + yCheck;
                            if (xtoCheck >= 0 && xtoCheck < size && ytoCheck >= 0 && ytoCheck < size)
                            {
                                //chain reveal on every adjacent square
                                if (hiddenBoard[xtoCheck, ytoCheck] == false )
                                {
                                    //...that hasn't already been revealed
                                    if (knownBoard[xtoCheck, ytoCheck] < 0)
                                    {
                                        //recur the function
                                        gridUpdate = PopSquare(xtoCheck, ytoCheck, hiddenBoard, gridUpdate);
                                    }
                                }
                            }
                        }
                    }
                } 
            }
            else 
            {
                //we hit a mine
                gridUpdate[x, y] = -3;
            }
            return gridUpdate;
        }

        public static bool CheckWin(int[,] knownBoard, bool[,] hiddenBoard)
        {
            //check if the whole board is revealed
            int revealCount = 0;
            int mineCount = 0;
            int totalCount = 0;
            for (int xCheck = 0; xCheck < knownBoard.GetLength(0); xCheck++)
            {
                for (int yCheck = 0; yCheck < knownBoard.GetLength(1); yCheck++)
                {
                    if(hiddenBoard[xCheck,yCheck] == true)
                    {
                        mineCount++;
                    }
                    if (knownBoard[xCheck, yCheck] >= 0)
                    {
                        revealCount++;
                    }
                    totalCount++;
                }
            }
            if(totalCount == mineCount + revealCount)
            {
                return true;
            }
            else
            { 
                return false;
            }
        }
    }
}
