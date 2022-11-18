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
        public static int GetBoardSize()
        {
            // Prepare my response variable
            string usersSize = "";
            bool isNum = false;
            while (!isNum) // Keep asking until the user enters a valid number!
            {
                Console.Write("Enter a size: ");
                usersSize = Console.ReadLine();

                // Is it a number?
                isNum = int.TryParse(usersSize, out int size);
                if (!isNum)
                {
                    // Keep the error message until the return key is pressed
                    Console.WriteLine($"{usersSize} is not a valid boardsize. Please Try Again!");
                    Console.ReadLine();
                }

                Console.Clear();
            }

            return int.Parse(usersSize);    // Returns a number because we break out of the loop!

        }

        // Print, obtain board status
        public static void PrintBoard(char[,] board)
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    // For every row: Leave a space, place the empty piece, space, then the divider(only if I'm not on the last column)
                    if (col < board.GetLength(1) - 1)
                        Console.Write(" " + board[row, col] + " |");
                    else
                        Console.Write(" " + board[row, col]);
                }
                // Place the horizontal divider where appropriate, need to obtain the number of rows
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
            // Early exit
            if (info.Length != 2)
                return false;

            // Exactly two inputs
            foreach (string piece in info)
            {
                bool isNum = int.TryParse(piece, out int data);
                // Exit early if the current piece is not a number!
                if (!isNum)
                    return false;

                // Check if an input is out of bounds
                if (data < 0 || data > size)
                    return false;

            }
            // Do I have a row number and a column number?
            return true;
        }

        // If invalid input, display the error
        public static void DisplayFormatError(string[] input)
        {
            // Display error message, user's invalid input
            Console.WriteLine("Invalid Prompt!");
            Console.Write("Your Input:");
            foreach (string prompt in input)
                Console.Write(" " + prompt);

            Console.WriteLine();    // Board and message to be in separate lines!

        }
    

        // Check for valid location
        public static bool ValidatedLocation(char[,] board, int row, int column, char currentPiece)
        {
            // Reference my board at the specified position: Open Space or occupied?
            switch (board[row - 1, column - 1])
            {
                // Empty space
                case ' ':
                    board[row - 1, column - 1] = currentPiece;
                    return true;
                // X or an O
                default:
                    return false;
            }
        }

        // Check horizontal wins!
        public static bool HorizontalWin(char[,] board, char piece, int size)
        {
            // Create a tracking list to count for consecutive pieces
            List<char> consecutivePieces = new List<char>();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                // Go through every spot in my row
                for (int j = 0; j < board.GetLength(1); j++)
                {   
                    if (board[i, j] == piece)
                    consecutivePieces.Add(piece);
                }
                // Do I have 3 of a kind?
                if (consecutivePieces.Count == size)
                    return true;

                // Clear, reset for the next row
                consecutivePieces.Clear();
            }

            return false;
        }

        // Check Vertical wins! Down through each column
        public static bool VerticalWin(char[,] board, char piece, int size)
        {
            // Create a tracking list to count for consecutive pieces
            List<char> consecutivePieces = new List<char>();
            for (int j = 0; j < board.GetLength(1); j++)
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    if (board[i, j] == piece)
                        consecutivePieces.Add(piece);
                }
                if (consecutivePieces.Count == size)
                    return true;
                // Clear, prepare for the next column
                consecutivePieces.Clear();
            }

            return false;
        }

        public static bool negativelySlopedWin(char[,] board, char piece, int size)
        {
            for (int i = 0; i < size; i++)
            {
                // Exit early for any opposing piece
                if (board[i, i] != piece)
                    return false;
            }
            // By default
            return true;
        }

        public static bool positivelySlopedWin(char[,] board, char piece, int size)
        {
            // Increment column, decrement row
            for (int i = 0; i < size; i++)
            {
                // Check for early exit
                if (board[(size - i) - 1, i] != piece)
                    return false;
            }
            // Default!
            return true;

        }
        // Account for positive and negative sloped wins above!
        public static bool DiagonalWin(char[,] board, char piece, int size)
        {
            return negativelySlopedWin(board, piece, size) || positivelySlopedWin(board, piece, size);
        }
        // Check All Possible Wins!
        public static bool WinningMove(char[,] board, char piece, int size)
        {
            return HorizontalWin(board, piece, size) || VerticalWin(board, piece, size) || DiagonalWin(board, piece, size);
        }

        // Entire Game Logic
        public static void PlayGame(int size, char[,] board, char player1, char player2, char workingPiece, int spaces, int turn)
        {
            // Game Loop
            do
            {
                // Print board state, prepare the piece
                PrintBoard(board);
                workingPiece = turn % 2 == 0 ? player1 : player2;

                // Turn message, prompt user
                string message = workingPiece == player1? "Player 1's turn!" : "Player 2's turn!";
                Console.WriteLine("\n\n" + message);
                Console.Write($"Enter a row and column({1} - {size}): ");
                string[] userInput = Console.ReadLine().Split(' ');

                Console.Clear();

                // Reset if I have invalid input
                if (!InputIsValidated(userInput, size))
                {
                    // Either 2 inputs or not
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

            // Endgame: Print out final board state, check for either a win or a draw
            PrintBoard(board);
            if (spaces >= 0 && WinningMove(board, workingPiece, size))
            {
                string winningMessage = workingPiece == player1 ? "Player 1 wins!" : "Player 2 wins!";
                Console.WriteLine("\n\n" + winningMessage);
            }
            else
                Console.WriteLine("\n\nDraw!");

            // Keep the state until the return key is pressed
            Console.ReadLine();
            Console.Clear();

        }

        public static void ThankUser()
        {
            Console.Write("Thank you for playing!");
        }


    }
 
}
