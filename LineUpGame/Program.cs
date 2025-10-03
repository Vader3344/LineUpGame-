using System;

namespace LineUpGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to LineUp Game");
            Console.WriteLine();

            // This will create and start a new game
            Game game = new Game();
            game.Begin_Game();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}