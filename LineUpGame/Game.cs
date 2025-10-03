using System;
using System.IO;

namespace LineUpGame
{
    // Main game class 
    public class Game
    {
        private Grid gameGrid;
        private char current_player;      // @ for player 1, # for player 2
        private bool isGameFinished;
        private bool computerMode; 
        private Random random;          
        
        // players gets special discs at start of game
        private int player1_boring;   // current_player 1 Boring discs B 
        private int player1_magnetic; // current_player 1 Magnetic discs M 
        private int player2_boring;   // current_player 2 Boring discs b
        private int player2_magnetic; // current_player 2 Magnetic discs m
        
        // Main Game
        
        // Constructor
        public Game()
        {
            random = new Random();       // for generating computer moves
            
            // First, ask what the user wants to do
            bool load = chooseMode();
            
            if (load)
            {
                load_game();
            }
            else if (!isGameFinished)
            {
                startNewGame();
            }
        }
        
        // Let user choose what they want to do - new game, load, test
        private bool chooseMode()
        {
            Console.WriteLine("Choose one of the following");
            Console.WriteLine("1 - Start a new game");
            Console.WriteLine("2 - Load a saved game");
            Console.WriteLine("3 - Testing");  // 
            Console.WriteLine();
            
            while (true)
            {
                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();
                
                if (input == "1")
                {
                    return false; // Start new game
                }
                else if (input == "2")
                {
                    return true; // Load saved game
                }
                else if (input == "3")
                {
                    RunTestingMode();
                    return false; // Test
                }
                else
                {
                    Console.WriteLine("Please enter 1, 2, or 3");
                }
            }
        }
        
        // Start a new game
        private void startNewGame()
        {
            // Get gameGrid size from user
            int[] gameGridSize = GetGridSize();
            gameGrid = new Grid(gameGridSize[0], gameGridSize[1]);
            
            current_player = '@';         // current_player 1 goes first
            isGameFinished = false;
            
            // Setting special disc counts
            player1_boring = 2;
            player1_magnetic = 2;
            player2_boring = 2;
            player2_magnetic = 2;
            
            // Show discs
            int totalCells = gameGrid.Rows * gameGrid.Columns;
            int discsPercurrent_player = totalCells / 2;
            int ordinaryDiscs = discsPercurrent_player - 4; 
            
            Console.WriteLine($"Grid: {gameGrid.Rows}x{gameGrid.Columns} = {totalCells} total cells");
            Console.WriteLine($"Every player gets {discsPercurrent_player} discs (4 special + {ordinaryDiscs} ordinary)");
            Console.WriteLine($"To win get {gameGrid.WinLength} in a row!");
            Console.WriteLine();
            
            // Ask user to choose game mode
            ChooseGameMode();
        }

        
        // Get custom grid from user
        private int[] GetCustomGridSize()
        {
            while (true) 
            {
                Console.Write("Enter rows (min 6): ");
                string rowInput = Console.ReadLine();
                Console.Write("Enter columns (min 7): ");
                string colInput = Console.ReadLine();
                
                int rows = ConvertStringToNumber(rowInput);
                int columns = ConvertStringToNumber(colInput);
                
                if (rows == -1 || columns == -1)
                {
                    Console.WriteLine("Please enter valid numbers");
                    continue;
                }
                
                if (rows < 6)
                {
                    Console.WriteLine("Rows must be at least 6");
                    continue;
                }
                
                if (columns < 7)
                {
                    Console.WriteLine("Columns must be at least 7");
                    continue;
                }
                
                if (rows > columns)
                {
                    Console.WriteLine("Rows cannot be more than columns");
                    continue;
                }
                
                Console.WriteLine($"Custom Grid: {rows} rows x {columns} columns");
                return new int[] { rows, columns };
            }
        }
        
        // Ask the user how big they want the gameGrid to be
        private int[] GetGridSize()
        {
            Console.WriteLine("Set Grid size:");
            Console.WriteLine("Default: 6 rows x 7 columns"); 
            Console.WriteLine();
            
            while (true)
            {
                Console.Write("Use default Grid size? (Y/N): ");
                string input = Console.ReadLine();
                input = input.ToUpper();
                
                if (input == "Y")
                {
                    return new int[] { 6, 7 };
                }
                else if (input == "N")
                {
                    return GetCustomGridSize();
                }
                else
                {
                    Console.WriteLine("Enter Y or N");
                }
            }
        }
        
        
        // Let user choose game mode
        private void ChooseGameMode()
        {
            Console.WriteLine("Choose a game mode:");
            Console.WriteLine("1 - Player vs Player");
            Console.WriteLine("2 - Player vs Computer");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Enter your choice (1 or 2): ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    computerMode = false;
                    Console.WriteLine("Mode: Player vs Player");
                    break;
                }
                else if (input == "2")
                {
                    computerMode = true;
                    Console.WriteLine("Mode: Player vs Computer");
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter 1 or 2");
                }
            }
            Console.WriteLine();
        }

      
        private int ConvertStringToNumber(string input)
        {
            // check each possibility manually
            if (input == "1") return 1;
            if (input == "2") return 2;
            if (input == "3") return 3;
            if (input == "4") return 4;
            if (input == "5") return 5;
            if (input == "6") return 6;
            if (input == "7") return 7;
            if (input == "8") return 8;
            if (input == "9") return 9;
            if (input == "10") return 10;
            if (input == "11") return 11;
            if (input == "12") return 12;
            if (input == "13") return 13;
            if (input == "14") return 14;
            if (input == "15") return 15;
            if (input == "16") return 16;
            if (input == "17") return 17;
            if (input == "18") return 18;
            if (input == "19") return 19;
            if (input == "20") return 20;
            
            return -1;  // means invalid input
        }


        // Show what special discs the current player has left

        private void Showcurrent_playerInventory()
        {
            if (current_player == '@')
            {
                Console.WriteLine($"Your inventory: Boring discs: {player1_boring}, Magnetic discs: {player1_magnetic}");
            }
            else if (!computerMode)  // show for player 2
            {
                Console.WriteLine($"Your inventory: Boring discs: {player2_boring}, Magnetic discs: {player2_magnetic}");
            }
        }

        // Get disc symbol for current player
        private char GetDiscSymbol(string discType)
        {
            if (discType == "boring")
            {
                if (current_player == '@')
                {
                    return 'B';
                }
                else
                {
                    return 'b';
                }
            }
            else if (discType == "magnetic")
            {
                if (current_player == '@')
                {
                    return 'M';
                }
                else
                {
                    return 'm';
                }
            }
            else // normal
            {
                return current_player;
            }
        }

        // Get the boring disc symbol for current player
        private char BoringDiscs()
        {
            return GetDiscSymbol("boring");
        }

        // Check if current player has discs of specified type
        private bool HasDiscs(string discType)
        {
            if (discType == "magnetic")
            {
                if (current_player == '@')
                {
                    return player1_magnetic > 0;
                }
                else
                {
                    return player2_magnetic > 0;
                }
            }
            else // boring
            {
                if (current_player == '@')
                {
                    return player1_boring > 0;
                }
                else
                {
                    return player2_boring > 0;
                }
            }
        }

        // Check if current player has magnetic discs
        private bool HasMagneticDiscs()
        {
            return HasDiscs("magnetic");
        }

        // Use a disc of specified type
        private void UseDisc(string discType)
        {
            if (discType == "boring")
            {
                if (current_player == '@')
                {
                    player1_boring = player1_boring - 1;
                }
                else
                {
                    player2_boring = player2_boring - 1;
                }
            }
            else // magnetic
            {
                if (current_player == '@')
                {
                    player1_magnetic = player1_magnetic - 1;
                }
                else
                {
                    player2_magnetic = player2_magnetic - 1;
                }
            }
        }

        // Use a boring disc
        private void UseBoringDisc()
        {
            UseDisc("boring");
        }

        // Place magnetic disc
        private bool PlaceMagneticDisc(int column, char discType)
        {
            // place the magnetic disc
            bool placed = gameGrid.PlaceDisc(column, discType);

            if (placed)
            {
                // lift nearest same-player ordinary disc
                char playerSymbol;
                if (discType == 'M')
                {
                    playerSymbol = '@';
                }
                else
                {
                    playerSymbol = '#';
                }
                gameGrid.ApplyMagneticEffect(column, playerSymbol);
            }

            return placed;
        }

        // Get the ordinary disc symbol for current player
        private char NormalDisc()
        {
            return GetDiscSymbol("normal");
        }

        // Use a magnetic disc
        private void UseMagneticDisc()
        {
            UseDisc("magnetic");
        }

        // Place boring disc 
        private bool PlaceBoringDisc(int column, char discType)
        {
            // Remove all existing discs from the column
            gameGrid.ClearColumn(column);

            // Place the boring disc at the bottom
            return gameGrid.PlaceDisc(column, discType);
        }

        // Check if current player has boring discs
        private bool HasBoringDiscs()
        {
            return HasDiscs("boring");
        }

        // Get the magnetic disc symbol for current player
        private char GetMagneticDiscSymbol()
        {
            return GetDiscSymbol("magnetic");
        }

        // Place disc with special effects
        private bool PlaceDiscEffects(int column, char discType)
        {
            // Check if column is valid and available
            if (!gameGrid.ColumnAvailable(column))
            {
                Console.WriteLine($"Column {column} is full! Try another column.");
                return false;
            }

            // Apply special effects based on disc type
            if (discType == 'B' || discType == 'b')
            {
                // Remove all discs from column, place boring disc at bottom
                return PlaceBoringDisc(column, discType);
            }
            else if (discType == 'M' || discType == 'm')
            {
                // Place disc, then apply magnetic effect
                return PlaceMagneticDisc(column, discType);
            }
            else
            {
                // Normal placement
                return gameGrid.PlaceDisc(column, discType);
            }
        }

        // Main game loop
        public void Begin_Game()
        {
            // if done, exit
            if (isGameFinished)
            {
                return;
            }
            
            Console.WriteLine("Player 1: @ (Ordinary), B (Boring), M (Magnetic)");
            if (computerMode)
            {
                Console.WriteLine("Computer: # (Ordinary), b (Boring), m (Magnetic)");
            }
            else
            {
                Console.WriteLine("Player 2: # (Ordinary), b (Boring), m (Magnetic)");
            }
            Console.WriteLine($"Goal: Get {gameGrid.WinLength} in a row!");
            Console.WriteLine();
            
            // Show the empty gameGrid to start
            gameGrid.DisplayGrid();
            
            // Keep playing until game is over
            while (isGameFinished == false)
            {
                // tell the user whose turn it is
                if (Current_playerComputer())
                {
                    Console.WriteLine("Computer's turn (#)");
                }
                else
                {
                    if (current_player == '@')
                    {
                        Console.WriteLine("Player 1's turn (@)");
                    }
                    else
                    {
                        Console.WriteLine("Player 2's turn (#)");
                    }
                }
                
                // get what move they want to make
                char discType;
                int column;
                
                if (Current_playerComputer())  // computer's turn
                {
                    discType = ComputerDisc();
                    column = GetComputerMove();
                }
                else  // player turn
                {
                    // first check if they want to save or need help
                    string playerAction = Getcurrent_playerAction();
                    
                    if (playerAction == "Save")
                    {
                        save_game();
                        continue;  // don't switch players, same person goes again
                    }
                    else if (playerAction == "Help")
                    {
                        HelpMenu();
                        continue; 
                    }
                    else
                    {
                        discType = GetPlayerDiscChoice();
                        column = GetPlayerMove();
                    }
                }
                
                // place the disc
                bool moveSuccessful = PlaceDiscEffects(column, discType);
                
                // if it didn't work, let them try again
                if (!moveSuccessful)
                {
                    Console.WriteLine("Move failed! Try again.");
                    continue;  // back to top of loop
                }
                
                // show the gameGrid after the move
                gameGrid.DisplayGrid();
                
                // check if someone won
                if (gameGrid.CheckWin(current_player))
                {
                    if (Current_playerComputer())
                    {
                        Console.WriteLine("Computer wins! Better luck next time!");
                    }
                    else
                    {
                        if (current_player == '@')
                        {
                            Console.WriteLine("Player 1 (@) WINS!");
                        }
                        else
                        {
                            Console.WriteLine("Player 2 (#) WINS!");
                        }
                    }
                    isGameFinished = true;
                }
                else if (IsGridFull())  // no more spaces left
                {
                    Console.WriteLine("Grid is full! It's a tie!");
                    isGameFinished = true;
                }
                else
                {
                    // nobody won yet, so switch to other player
                    Switchcurrent_player();
                }
            }
            
            Console.WriteLine("Game Over! Thanks for playing!");
        }

       
        
        // Check if current player is the computer
        private bool Current_playerComputer()
        {
           
            return current_player == '#' && computerMode;
        }
        
        // Switch from current player to the other player
        private void Switchcurrent_player()
        {
            if (current_player == '@')
            {
                current_player = '#';
            }
            else
            {
                current_player = '@';
            }
        }
        

        // Check if the gameGrid is full 
        private bool IsGridFull()
        {
            // Check if any column has space
            for (int col = 1; col <= gameGrid.Columns; col++)
            {
                if (gameGrid.ColumnAvailable(col))
                {
                    return false; // empty column
                }
            }
            return true; // All columns are full
        }

        

        // asking the player for turn
        private string Getcurrent_playerAction()
        {
            Console.WriteLine();
            Console.WriteLine("Choose an action:");
            Console.WriteLine("P - Play a disc");
            Console.WriteLine("S - Save game");
            Console.WriteLine("H - Help menu");
            Console.WriteLine();
            
            while (true)
            {
                Console.Write("Enter your choice (P/S/H): ");
                string input = Console.ReadLine();
                input = input.ToUpper();
                
                if (input == "P")
                {
                    return "Play";
                }
                else if (input == "S")
                {
                    return "Save";
                }
                else if (input == "H")
                {
                    return "Help";
                }
                else
                {
                    Console.WriteLine("Please enter P, S, or H");
                }
            }
        }

        // Ask user what disc they want to use
        private char GetPlayerDiscChoice()
        {
            Showcurrent_playerInventory();  // show them what they have left
            
            while (true)
            {
                Console.Write("Choose disc type (O=Ordinary, B=Boring, M=Magnetic): ");
                string input = Console.ReadLine();
                input = input.ToUpper();
                
                if (input == "O")
                {
                    return NormalDisc();
                }
                else if (input == "B")
                {
                    if (HasBoringDiscs())
                    {
                        UseBoringDisc();
                        return BoringDiscs();
                    }
                    else
                    {
                        Console.WriteLine("No Boring discs left!");
                    }
                }
                else if (input == "M")
                {
                    if (HasMagneticDiscs())
                    {
                        UseMagneticDisc();
                        return GetMagneticDiscSymbol();
                    }
                    else
                    {
                        Console.WriteLine("No Magnetic discs left!");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter O, B, or M");
                }
            }
        }

        // Get a move from the current player
        private int GetPlayerMove()
        {
            while (true) 
            {
                Console.Write($"Choose a column (1-{gameGrid.Columns}): ");
                string input = Console.ReadLine();
                
                int column = ConvertStringToNumber(input);
                
                if (column == -1 || column < 1 || column > gameGrid.Columns)
                {
                    Console.WriteLine($"Please enter a number between 1 and {gameGrid.Columns}");
                    continue;
                }
                
                if (gameGrid.ColumnAvailable(column))
                {
                    return column;
                }
                else
                {
                    Console.WriteLine($"Column {column} is full! Try another column.");
                }
            }
        }

        
        
        // computer decision for choosing disc
        private char ComputerDisc()
        {
            
            int choice = random.Next(1, 11); // selecting a random disc
            
            if (choice <= 8)  // 80% of the time
            {
                return NormalDisc();
            }
            else if (choice <= 9 && HasBoringDiscs())  // use boring 
            {
                UseBoringDisc();
                return BoringDiscs();
            }
            else if (HasMagneticDiscs())  // use  magnetic 
            {
                UseMagneticDisc();
                return GetMagneticDiscSymbol();
            }
            else  // no special discs left, use ordinary
            {
                return NormalDisc();
            }
        }

        // Computer logic for choosing column
        private int GetComputerMove()
        {
           
             // check if player is about to win and block them
            int blockingMove = CheckforWinMove('@');
            if (blockingMove != -1)
            {
                return blockingMove;  // block their win
            }

             // check if computer can win
            int winningMove = CheckforWinMove('#');
            if (winningMove != -1)
            {
                return winningMove;  // take the win!
            }
            
            // pick a random column otherwise
            return GetRandomMove();
        }
        
        // Pick a random column that isn't full
        private int GetRandomMove()
        {
            // just keep trying until we find one that works
            while (true)
            {
                int randomColumn = random.Next(1, gameGrid.Columns + 1);
                
                if (gameGrid.ColumnAvailable(randomColumn)) 
                {
                    return randomColumn;
                }
                // if that column is full, try another one
            }
        }
        
        // Find if a player can win by playing in any column
        private int CheckforWinMove(char playerSymbol)
        {
            // Try each column to see win
            for (int col = 1; col <= gameGrid.Columns; col++)
            {
                if (gameGrid.ColumnAvailable(col))
                {
                
                    if (WillMoveWin(col, playerSymbol))
                    {
                        return col; 
                    }
                }
            }
            return -1; 
        }
        
        // Check if placing a disc in this column would give a win
        private bool WillMoveWin(int column, char playerSymbol)
        {
            // Find where the disc would land
            int landingRow = CheckforLandingRow(column);
            
            if (landingRow == -1)
            {
                return false; // Column is full
            }
            
            // Temporarily place the disc
            gameGrid.PlaceDiscAt(landingRow, column, playerSymbol);
            
            // Check if this creates a win
            bool wouldWin = gameGrid.CheckWin(playerSymbol);
            
            // Remove the disc
            gameGrid.RemoveDiscFrom(landingRow, column);
            
            return wouldWin;
        }
        
        // Find which row a disc will land
        private int CheckforLandingRow(int column)
        {
            // Convert to 0 index
            int colIndex = column - 1;
            
            // Find the lowest empty row
            for (int row = 0; row < gameGrid.Rows; row++)
            {
                if (gameGrid.IsEmpty(row, colIndex))
                {
                    return row; // where its going to land
                }
            }
            
            return -1; // Column is full
        }


        
        
        // Save everything to a text file so we can load it later
        private void save_game()
        {
            try  // in case something goes wrong with file writing
            {
                Console.Write("Enter filename to save (for example newgame'): ");
                string filename = Console.ReadLine();
                
                if (string.IsNullOrEmpty(filename))
                {
                    filename = "savegame";
                }
                
                string filepath = filename + ".txt";
                
                using (StreamWriter writer = new StreamWriter(filepath))
                {
                    // write all the important game info to the file
                    writer.WriteLine(gameGrid.Rows);  // gameGrid size
                    writer.WriteLine(gameGrid.Columns);
                    
                    writer.WriteLine(current_player);  // whose turn it is
                    
                    writer.WriteLine(computerMode);  
                    
                    writer.WriteLine(isGameFinished);  // is game finished
                    
                    // how many special discs each player has left
                    writer.WriteLine(player1_boring);
                    writer.WriteLine(player1_magnetic);
                    writer.WriteLine(player2_boring);
                    writer.WriteLine(player2_magnetic);
                    
                    // now save what's in each cell of the gameGrid
                    for (int row = 0; row < gameGrid.Rows; row++)
                    {
                        for (int col = 0; col < gameGrid.Columns; col++)
                        {
                            char disc = gameGrid.GetDisc(row, col);
                            writer.Write(disc);
                        }
                        writer.WriteLine();  // next row
                    }
                }
                
                Console.WriteLine($"Game saved '{filepath}'!");
                Console.WriteLine();
            }
            catch
            {
                Console.WriteLine("Error: Could not save the game. Please try again.");
                Console.WriteLine();
            }
        }
        
        // Load a saved game from file
        private void load_game()
        {
            try
            {
                Console.Write("Enter filename to load (without .txt): ");
                string filename = Console.ReadLine();
                
                if (string.IsNullOrEmpty(filename))
                {
                    filename = "savegame";
                }
                
                string filepath = filename + ".txt";
                
                if (!File.Exists(filepath))
                {
                    Console.WriteLine($"Error: File '{filepath}' not found. Starting a new game instead.");
                    Console.WriteLine();
                    startNewGame();
                    return;
                }
                
                using (StreamReader reader = new StreamReader(filepath))
                {
                    // Load gameGrid dimensions
                    int rows = ConvertStringToNumber(reader.ReadLine());
                    int columns = ConvertStringToNumber(reader.ReadLine());
                    
                    if (rows == -1 || columns == -1)
                    {
                        Console.WriteLine("Error: Invalid save file format. Starting new game.");
                        Console.WriteLine();
                        startNewGame();
                        return;
                    }
                    
                    // Create gameGrid with loaded dimensions
                    gameGrid = new Grid(rows, columns);
                    
                    // Load current player
                    string playerLine = reader.ReadLine();
                    current_player = playerLine[0];
                    
                    // Load game mode
                    string gameModeLine = reader.ReadLine();
                    computerMode = gameModeLine == "True";
                    
                    // Load game over status
                    string isGameFinishedLine = reader.ReadLine();
                    isGameFinished = isGameFinishedLine == "True";
                    
                    // Load special disc inventories
                    player1_boring = ConvertStringToNumber(reader.ReadLine());
                    player1_magnetic = ConvertStringToNumber(reader.ReadLine());
                    player2_boring = ConvertStringToNumber(reader.ReadLine());
                    player2_magnetic = ConvertStringToNumber(reader.ReadLine());
                    
                    // Load gameGrid state
                    for (int row = 0; row < gameGrid.Rows; row++)
                    {
                        string rowData = reader.ReadLine();
                        if (rowData != null)
                        {
                            for (int col = 0; col < gameGrid.Columns && col < rowData.Length; col++)
                            {
                                char disc = rowData[col];
                                if (disc != ' ')
                                {
                                    gameGrid.PlaceDiscAt(row, col + 1, disc);
                                }
                            }
                        }
                    }
                }
                
                Console.WriteLine($"Game loaded from '{filepath}'!");
                
                // Show current game info
                string modeText;
                if (computerMode)
                {
                    modeText = "Player vs Computer";
                }
                else
                {
                    modeText = "Player vs Player";
                }
                Console.WriteLine($"Mode: {modeText}");
                if (current_player == '@')
                {
                    Console.WriteLine("Current player: Player 1 (@)");
                }
                else if (computerMode)
                {
                    Console.WriteLine("Current player: Computer (#)");
                }
                else
                {
                    Console.WriteLine("Current player: Player 2 (#)");
                }
                Console.WriteLine($"Win condition: Get {gameGrid.WinLength} in a row!");
                Console.WriteLine();
            }
            catch
            {
                Console.WriteLine("Error: Could not load the game. Starting new game instead.");
                Console.WriteLine();
                startNewGame();
            }
        }

        
        
        // Show help menu
        private void HelpMenu()
        {
            Console.WriteLine();
            Console.WriteLine("--- Help ----");
            Console.WriteLine();
            Console.WriteLine("Goal: Get " + gameGrid.WinLength + " discs in a row");
            Console.WriteLine();
            Console.WriteLine("Discs:");
            Console.WriteLine("O - Ordinary disc (normal)");
            Console.WriteLine("B - Boring disc (clears column)");  
            Console.WriteLine("M - Magnetic disc (lifts disc up)");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("P - Play a disc");
            Console.WriteLine("S - Save game");
            Console.WriteLine("H - Help");
            Console.WriteLine();
            Console.WriteLine("Each player has 2 Boring + 2 Magnetic discs");
            Console.WriteLine("---------------");
            Console.WriteLine();
        }

        
        
        // Testing logic
        private void RunTestingMode()
        {
            Console.WriteLine();
            Console.WriteLine("--- Testing ---");
            Console.WriteLine("Enter sequence: DiscType+Column, separated by commas");
            Console.WriteLine("Disc Types: O, B, M");  
            Console.WriteLine("Example: O4,O5,O3,O6,O2,O5,O4,O5,M5,B4");
            Console.WriteLine();
            
            Console.Write("Enter test sequence: ");
            string input = Console.ReadLine();
            
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("No input provided.");
                isGameFinished = true;
                return;
            }
            
            // testing always uses standard 6x7 gameGrid
            gameGrid = new Grid(6, 7);
            current_player = '@';  // player 1 starts
            isGameFinished = false;
            computerMode = false;  // both players for testing
            
            // give each player their special discs
            player1_boring = 2;
            player1_magnetic = 2;
            player2_boring = 2;
            player2_magnetic = 2;
            
            Console.WriteLine("Starting testing with 6x7 Grid");
            Console.WriteLine("P1: @ | P2: #");
            Console.WriteLine();
            
            // Parse and execute the moves
            ExecuteTestMoves(input);
            
            isGameFinished = true;
        }
        
        // Go through each move in the test sequence
        private void ExecuteTestMoves(string input)
        {
            // break up the input string
            string[] moves = input.Split(',');
            int moveNumber = 1;  // for display purposes
            
            gameGrid.DisplayGrid();
            
            foreach (string move in moves)
            {
                if (isGameFinished) break; // Stop if game is already over
                
                if (move.Length < 2)
                {
                    Console.WriteLine($"Move {moveNumber}: Invalid format - skipping");
                    moveNumber = moveNumber + 1;
                    continue;
                }
                
                // Extract disc type and column
                char discTypeChar = move[0];
                string columnStr = move.Substring(1);
                int column = ConvertStringToNumber(columnStr);
                
                if (column == -1 || column < 1 || column > gameGrid.Columns)
                {
                    Console.WriteLine($"Move {moveNumber}: Invalid column - skipping");
                    moveNumber = moveNumber + 1;
                    continue;
                }
                
                // Convert disc type and check availability
                char discType = GetTestDiscType(discTypeChar);
                if (discType == '?')
                {
                    Console.WriteLine($"Move {moveNumber}: Invalid disc type - skipping");
                    moveNumber = moveNumber + 1;
                    continue;
                }
                
                // Check if player has the special disc
                if (!CanUseDiscType(discType))
                {
                    Console.WriteLine($"Move {moveNumber}: No {discTypeChar} discs left - using O");
                    discType = NormalDisc();
                }
                else if (discType == 'B' || discType == 'b')
                {
                    UseBoringDisc();
                }
                else if (discType == 'M' || discType == 'm')
                {
                    UseMagneticDisc();
                }
                
                // Execute the move
                int playerNumber;
                if (current_player == '@')
                {
                    playerNumber = 1;
                }
                else
                {
                    playerNumber = 2;
                }
                Console.WriteLine($"Move {moveNumber}: P{playerNumber} plays {discTypeChar} in Column {column}");
                
                bool success = PlaceDiscEffects(column, discType);
                
                if (!success)
                {
                    Console.WriteLine($"Move {moveNumber}: Column full");
                    moveNumber = moveNumber + 1;
                    continue;
                }
                
                gameGrid.DisplayGrid();
                
                // Check for win
                if (gameGrid.CheckWin(current_player))
                {
                    int winnerNumber;
                    if (current_player == '@')
                    {
                        winnerNumber = 1;
                    }
                    else
                    {
                        winnerNumber = 2;
                    }
                    Console.WriteLine($"P{winnerNumber} Wins!");
                    isGameFinished = true;
                    break;
                }
                else if (IsGridFull())
                {
                    Console.WriteLine("Tie!");
                    isGameFinished = true;
                    break;
                }
                
                // Switch to next player
                Switchcurrent_player();
                moveNumber++;
            }
            
            Console.WriteLine($"Testing finished. {moveNumber - 1} moves processed.");
        }
        
        // Get disc type for testing mode 
        private char GetTestDiscType(char discTypeChar)
        {
            if (discTypeChar == 'O')
            {
                return NormalDisc();
            }
            else if (discTypeChar == 'B')
            {
                return BoringDiscs();
            }
            else if (discTypeChar == 'M')
            {
                return GetMagneticDiscSymbol();
            }
           
            return '?'; // Invalid disc type
        }
        
        // Check if current player can use this disc type
        private bool CanUseDiscType(char discType)
        {
            if (discType == NormalDisc())
            {
                return true; // Can always use ordinary discs
            }
            else if (discType == BoringDiscs())
            {
                return HasBoringDiscs();
            }
            else if (discType == GetMagneticDiscSymbol())
            {
                return HasMagneticDiscs();
            }
            
            return false;
        }
    }
}