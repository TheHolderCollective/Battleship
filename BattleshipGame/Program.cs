using BattleshipGame.Objects;
using BattleshipGame.Objects.Display;
using System;

namespace BattleshipGame
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Code to test GameDisplay class fucntionality

            //Player Player1 = new Player("Amy");
            //Player Player2 = new Player("Vince");

            //Player1.PlaceShips();
            //Player2.PlaceShips();

            //int hitCount = 0;
            //int roundNumber1, roundNumber2;


            //while (hitCount < 20)
            //{
            //    var coordinates = Player1.FireShot();
            //    var result = Player2.ProcessShot(coordinates);
            //    Player1.ProcessShotResult(coordinates, result);

            //    if (result == ShotResult.Hit)
            //    {
            //        hitCount++;
            //    }

            //    if (!Player2.HasLost) //If player 2 already lost, we can't let them take another turn.
            //    {
            //        coordinates = Player2.FireShot();
            //        result = Player1.ProcessShot(coordinates);
            //        Player2.ProcessShotResult(coordinates, result);
            //    }

            //    if (result == ShotResult.Hit)
            //    {
            //        hitCount++;
            //    }

            //}

            //roundNumber1 = Player1.RoundNumber;
            //roundNumber2 = Player2.RoundNumber;


            try
            {
                Player Player1 = new Player("Challenger");
                Player Player2 = new Player("Computer");

                GameDisplay gameDisplay = new GameDisplay(Player1, Player2);
                gameDisplay.ShowDisplay();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType()}: {ex.Message}");
                throw;
            }

            #endregion

            #region Original code for main
            //int player1Wins = 0, player2Wins = 0;

            //Console.WriteLine("How many games do you want to play?");
            //var numGames = int.Parse(Console.ReadLine());

            //for (int i = 0; i < numGames; i++)
            //{
            //    Game game1 = new Game();
            //    game1.PlayToEnd();
            //    if(game1.Player1.HasLost)
            //    {
            //        player2Wins++;
            //    }
            //    else
            //    {
            //        player1Wins++;
            //    }
            //}

            //Console.WriteLine("Player 1 Wins: " + player1Wins.ToString());
            //Console.WriteLine("Player 2 Wins: " + player2Wins.ToString());
            //Console.ReadLine();

            #endregion
        }
    }
}
