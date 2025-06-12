using BattleshipGame.Objects.Display;
using System;

namespace BattleshipGame
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GameDisplay battleshipDisplay = new GameDisplay();
                battleshipDisplay.PlayGame();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType()}: {ex.Message}");
            }
        }
    }
}
