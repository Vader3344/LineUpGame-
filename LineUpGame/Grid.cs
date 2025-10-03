using System;

namespace LineUpGame
{
    // This class handles the game grid.
    public class Grid
    {
        // Main grid data
        private char[,] cells;      // 2D array to store all the discs
        private int rows;           // how many rows we have
        private int columns;        // how many columns
        private int winLength;      // how many in a row you need to win
        
        public int Rows 
        { 
            get { return rows; } 
        }
        public int Columns 
        { 
            get { return columns; } 
        }
        public int WinLength 
        { 
            get { return winLength; } 
        }


       
        public Grid(int rows = 6, int columns = 7)
        {
            this.rows = rows;
            this.columns = columns;

            // calculate how many you need in a row to win
            double calculation = rows * columns * 0.1;
            int ceilingVal = (int)Math.Ceiling(calculation);
            int maxVal = Math.Max(3, ceilingVal);
            this.winLength = Math.Min(4, maxVal);  // between 3 and 4

            // make the actual 2D array
            cells = new char[rows, columns];

            // fill it with spaces (empty cells)
            FillGrid();

            Console.WriteLine("Created " + rows + "x" + columns + " grid!");
        }

        // Fill the grid with empty spaces
        private void FillGrid()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    cells[row, col] = ' '; // Empty space character
                }
            }
        }

        

        

        // show current grid
        public void DisplayGrid()
        {
            Console.WriteLine("\n---- Grid ----");
            
            // start from top row and work down ( row goes backwards)
            for (int row = rows - 1; row >= 0; row--)
            {
                Console.Write("|");  // left side border
                
                for (int col = 0; col < columns; col++)
                {
                    Console.Write(" " + cells[row, col] + " |");
                }
                
                Console.WriteLine();  // new line for next row
            }
            
            // Display column numbers
            DisplayColumnNumbers();
            Console.WriteLine("--------------\n");
        }

        // show column numbers
        private void DisplayColumnNumbers()
        {
            Console.Write(" "); // Align with the grid
            for (int col = 1; col <= columns; col++)
            {
                if (col < 10)
                {
                    Console.Write(" " + col + "  "); // Single digit
                }
                else
                {
                    Console.Write(" " + col + " "); // Double digit
                }
            }
            Console.WriteLine();
        }

        // disc operations 

        // Put a disc in a column.
        public bool PlaceDisc(int column, char discSymbol)
        {
            // users type 1-7, but arrays start at 0
            int colIndex = column - 1;
            
            // making sure it's a valid column
            if (colIndex < 0 || colIndex >= columns)
            {
                Console.WriteLine("Invalid column number! please choose from 1-7" + columns);
                return false;
            }
            
            // find the lowest empty spot
            for (int row = 0; row < rows; row++)  // bottom row is 0
            {
                if (cells[row, colIndex] == ' ')  // found an empty spot
                {
                    cells[row, colIndex] = discSymbol;
                    Console.WriteLine("Disc placed.");
                    return true;
                }
            }
            
            // column must be full if we get here
            Console.WriteLine("Column " + column + " is full! Try another column.");
            return false;
        }

        // if we can place a disc in a column
        public bool ColumnAvailable(int column)
        {
            int colIndex = column - 1;
            
            if (colIndex < 0 || colIndex >= columns)
                return false;
            
            // Check if the top row of this column is empty
            return cells[rows - 1, colIndex] == ' ';
        }



        // computer and saving
        public bool IsEmpty(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= columns)
                return false;
            
            return cells[row, col] == ' ';
        }
        
        // Get disc at specific position (for saving game state)
        public char GetDisc(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= columns)
                return ' ';
            
            return cells[row, col];
        }
        
        // Direct placement for loading saved games
        public void PlaceDiscAt(int row, int column, char discSymbol)
        {
            int colIndex = column - 1;
            
            if (row >= 0 && row < rows && colIndex >= 0 && colIndex < columns)
            {
                cells[row, colIndex] = discSymbol;
            }
        }
        
        // Remove a disc
        public void RemoveDiscFrom(int row, int column)
        {
            int colIndex = column - 1;
            
            if (row >= 0 && row < rows && colIndex >= 0 && colIndex < columns)
            {
                cells[row, colIndex] = ' ';
            }
        }

    
        
        // Clear all discs from a column (for Boring disc)
        public void ClearColumn(int column)
        {
            int colIndex = column - 1;
            
            if (colIndex >= 0 && colIndex < columns)
            {
                for (int row = 0; row < rows; row++)
                {
                    cells[row, colIndex] = ' ';
                }
            }
        }
        
        // Apply magnetic effect
        public void ApplyMagneticEffect(int column, char playerSymbol)
        {
            int colIndex = column - 1;

            if (colIndex < 0 || colIndex >= columns) return;

            // Find the magnetic disc position
            int magneticdiscRow = -1;
            for (int row = 0; row < rows; row++)
            {
                char discCell = cells[row, colIndex];
                if (discCell == 'M' || discCell == 'm')
                {
                    magneticdiscRow = row;
                    break;
                }
            }

            if (magneticdiscRow == -1) return;

            // Find the nearest ordinary disc of the same player (search from magnetic disc downward)
            int samePlayerRow = -1;
            for (int row = magneticdiscRow - 1; row >= 0; row--)
            {
                if (cells[row, colIndex] == playerSymbol)
                {
                    samePlayerRow = row;
                    break;
                }
            }

            // Lift if found (pull up the player's disc)
            if (samePlayerRow != -1)
            {
                // Pull the player's disc up one position, pushing any disc above it down
                int targetRow = samePlayerRow + 1;
                if (targetRow < rows)
                {
                    // Save what's in the target position
                    char discInTarget = cells[targetRow, colIndex];

                    // Move the player's disc up
                    char samePlayerDisc = cells[samePlayerRow, colIndex];
                    cells[targetRow, colIndex] = samePlayerDisc;

                    // If there was a disc in the target position, push it down
                    if (discInTarget != ' ')
                    {
                        cells[samePlayerRow, colIndex] = discInTarget;
                    }
                    else
                    {
                        cells[samePlayerRow, colIndex] = ' ';
                    }
                }
            }

        }



        // Check if a player has won the game
        public bool CheckWin(char playerSymbol)
        {
            // need to check all 4 possible directions for a line
            if (CheckHorizontal(playerSymbol)) return true;  // left-right
            if (CheckVertical(playerSymbol)) return true;    // up-down
            if (CheckDiagonalDownRight(playerSymbol)) return true;  // diagonal 
            if (CheckDiagonalDownLeft(playerSymbol)) return true;   // diagonal 
            
            return false;  // didn't find a win
        }

        // Look for horizontal wins (left to right)
        private bool CheckHorizontal(char playerSymbol)
        {
            for (int row = 0; row < rows; row++)
            {
                // check each possible starting position in this row
                for (int col = 0; col <= columns - winLength; col++)
                {
                    bool hasWin = true;
                    // check if we have enough in a row starting from this position
                    for (int i = 0; i < winLength; i++)
                    {
                        if (cells[row, col + i] != playerSymbol)
                        {
                            hasWin = false;
                            break;  // not a win, try next position
                        }
                    }
                    if (hasWin) return true;  // found a win!
                }
            }
            return false;  // no horizontal wins found
        }

        // Check for vertical wins
        private bool CheckVertical(char playerSymbol)
        {
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row <= rows - winLength; row++)
                {
                    bool hasWin = true;
                    for (int i = 0; i < winLength; i++)
                    {
                        if (cells[row + i, col] != playerSymbol)
                        {
                            hasWin = false;
                            break;
                        }
                    }
                    if (hasWin) return true;
                }
            }
            return false;
        }

        // Check for diagonal wins going down-right
        private bool CheckDiagonalDownRight(char playerSymbol)
        {
            for (int row = 0; row <= rows - winLength; row++)
            {
                for (int col = 0; col <= columns - winLength; col++)
                {
                    bool hasWin = true;
                    for (int i = 0; i < winLength; i++)
                    {
                        if (cells[row + i, col + i] != playerSymbol)
                        {
                            hasWin = false;
                            break;
                        }
                    }
                    if (hasWin) return true;
                }
            }
            return false;
        }

        // Check for diagonal wins going down-left
        private bool CheckDiagonalDownLeft(char playerSymbol)
        {
            for (int row = 0; row <= rows - winLength; row++)
            {
                for (int col = winLength - 1; col < columns; col++)
                {
                    bool hasWin = true;
                    for (int i = 0; i < winLength; i++)
                    {
                        if (cells[row + i, col - i] != playerSymbol)
                        {
                            hasWin = false;
                            break;
                        }
                    }
                    if (hasWin) return true;
                }
            }
            return false;
        }
    }
}