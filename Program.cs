using System;
using System.Collections.Generic;

namespace TicTacToePart2
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Play the game at least once!
            string userStatus = "";
            do
            {
                Console.Clear();

                // Create the board and pieces
                int BOARDSIZE = GetBoardSize();
                char[,] gameBoard = CreateBoard(BOARDSIZE);
                char player1Piece = 'X', player2Piece = 'O', currentPiece = ' ';

                // Create a turn variable and obtain the number of elements in my board
                int playerTurn = 0, avaliableSpaces = BOARDSIZE * BOARDSIZE;

                PlayGame(BOARDSIZE, gameBoard, player1Piece, player2Piece, currentPiece, avaliableSpaces, playerTurn);

                // Check on the user after the game!
                Console.Write("Do you want to play again?(yes/no): ");
                userStatus = Console.ReadLine();

            } while (userStatus != "no" && userStatus != "No");

            ThankUser();    // Message after the game!
        }

        // Prompt user for size
        public static int GetBoardSize(bool isNum = false, int size = 0)
        {
            // Prepare my response variable
            string usersSize = "";
            while (!isNum || size < 3) // Keep asking until the user enters a valid number!
            {
                Console.Write("Enter a size(3 or greater): ");
                usersSize = Console.ReadLine();

                // Is it a number?
                isNum = int.TryParse(usersSize, out size);
                if (!isNum)
                    Console.WriteLine($"{usersSize} is not a valid boardsize. Please Try Again!");
                else if(size < 3)
                    Console.WriteLine($"{size} is not 3 or greater. Please Try Again");
                
                Console.ReadLine();
                Console.Clear();
            }

            return size;  

        }

        // Print, obtain board status
        public static void PrintBoard(char[,] board)
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (col < board.GetLength(1) - 1)
                        Console.Write(" " + board[row, col] + " |");
                    else
                        Console.Write(" " + board[row, col]);
                }
                if (row < board.GetLength(0) - 1)
                    Console.WriteLine("\n" + CreateHorizontalDivider(board.GetLength(0)));
            }

        }

        // Initialize an empty 2D game board with a specified size
        public static char[,] CreateBoard(int size)
        {
            char[,] board = new char[size, size];
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                    board[row, col] = ' ';
            }
            return board;
        }

        // Make the divider adjustable based on the dimensions of the gameboard
        public static string CreateHorizontalDivider(int size)
        {
            // Figure out the length of my divider!
            int dividerLength = (4 * size) - 1;

            // One by one, we'll add the dash until we meet the dimensions
            string divider = "";
            for (int i = 0; i < dividerLength; i++)
                divider += "-";

            return divider;
        }

        // Check and validate the input! No text and in bounds!
        public static bool InputIsValidated(string[] info, int size)
        {
            if (info.Length != 2)
                return false;

            // Exactly two inputs
            foreach (string piece in info)
            {
                bool isNum = int.TryParse(piece, out int data);
                if (!isNum)
                    return false;

                // Check if an input is out of bounds
                if (data < 0 || data > size)
                    return false;

            }
            return true;
        }

        // If invalid input, display the error
        public static void DisplayFormatError(string[] input)
        {
            Console.WriteLine("Invalid Prompt!");
            Console.Write("Your Input:");
            foreach (string prompt in input)
                Console.Write(" " + prompt);

            Console.WriteLine();    // Board and message to be in separate lines!

        }
    
        public static bool ValidatedLocation(char[,] board, int row, int column, char currentPiece)
        {
            // Reference my board at the specified position: Open Space or occupied?
            switch (board[row - 1, column - 1])
            {
                case ' ':
                    board[row - 1, column - 1] = currentPiece;
                    return true;
                default:
                    return false;
            }
        }

        public static bool HorizontalWin(char[,] board, char piece)
        {
            int consecutivePieces = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {   
                    if (board[i, j] == piece)
                        consecutivePieces++;
                }
                if (consecutivePieces == 3)
                    return true;

                // Clear, reset for the next row
                consecutivePieces.Clear();
            }

            return false;
        }

        public static bool VerticalWin(char[,] board, char piece)
        {
            int consecutivePieces = 0;
            for (int j = 0; j < board.GetLength(1); j++)
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    if (board[i, j] == piece)
                        consecutivePieces++;
                }
                if (consecutivePieces == 3)
                    return true;
                
                consecutivePieces.Clear();
            }
            return false;
        }

        public static bool negativelySlopedWin(char[,] board, char piece, int size)
        {
            int consecutivePieces = 0;
            for (int i = 0; i < size; i++)
            {
                // Exit early for any opposing piece
                if (board[i, i] != piece)
                    return false;
                
                consecutivePieces++
                if(consecutivePieces == 3)
                    break;
            }
            return true;
        }

        public static bool positivelySlopedWin(char[,] board, char piece, int size)
        {
            int consecutivePieces = 0;
            // Increment column, decrement row
            for (int i = 0; i < size; i++)
            {
                // Check for early exit
                if (board[(size - i) - 1, i] != piece)
                    return false;
                
                consecutivePieces++
                if(consecutivePieces == 3)
                    break;   
            }
            return true;

        }

        public static bool DiagonalWin(char[,] board, char piece, int size)
        {
            return negativelySlopedWin(board, piece, size) || positivelySlopedWin(board, piece, size);
        }
        
        public static bool WinningMove(char[,] board, char piece, int size)
        {
            return HorizontalWin(board, piece) || VerticalWin(board, piece) || DiagonalWin(board, piece, size);
        }

        // Entire Game Logic
        public static void PlayGame(int size, char[,] board, char player1, char player2, char workingPiece, int spaces, int turn)
        {
            // Game Loop
            do
            {
                PrintBoard(board);
                workingPiece = turn % 2 == 0 ? player1 : player2;

                string message = workingPiece == player1? "Player 1's turn!" : "Player 2's turn!";
                Console.WriteLine("\n\n" + message);
                Console.Write($"Enter a row and column({1} - {size}): ");
                string[] userInput = Console.ReadLine().Split(' ');

                Console.Clear();

                // Reset if I have invalid input
                if (!InputIsValidated(userInput, size))
                {
                    if(userInput.Length == 2)
                        Console.WriteLine($"Invalid Location: Row {userInput[0]}, Column {userInput[1]}");
                    else
                        DisplayFormatError(userInput);

                    continue;
                }
            
                // Store and verify the location
                int row = int.Parse(userInput[0]), column = int.Parse(userInput[1]);

                if (!ValidatedLocation(board, row, column, workingPiece))
                {
                    Console.WriteLine($"Row {row}, Column {column} is taken!");
                    continue;
                }

                // Indicate one less piece, check for all possible win combinations!
                spaces--;
                turn++;


            } while (spaces > 0 && !WinningMove(board, workingPiece, size));

            PrintBoard(board);
            if (spaces >= 0 && WinningMove(board, workingPiece, size))
            {
                string winningMessage = workingPiece == player1 ? "Player 1 wins!" : "Player 2 wins!";
                Console.WriteLine("\n\n" + winningMessage);
            }
            else
                Console.WriteLine("\n\nDraw!");

            Console.ReadLine();
            Console.Clear();

        }

        public static void ThankUser()
        {
            Console.Write("Thank you for playing!");
        }


    }
 
}
