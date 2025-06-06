using BattleshipGame.Objects.Games;
using System;

namespace BattleshipGame
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Game battleshipGame = new Game();
                battleshipGame.PlayGame();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType()}: {ex.Message}");
            }
        }
    }
}
