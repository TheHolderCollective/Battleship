﻿using System;

namespace BattleshipGame.Objects.Games
{
    public class Game
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

       // private GameDisplay gameDisplay;

        public Game()
        {
            Player1 = new Player("Amy");
            Player2 = new Player("Vince");

            Player1.PlaceShips();
            Player2.PlaceShips();

            Player1.OutputBoards();
            Player2.OutputBoards();

            //gameDisplay = new GameDisplay(Player1, Player2);
        }

        public void PlayRound()
        {
            //Each exchange of shots is called a Round.
            //One round = Player 1 fires a shot, then Player 2 fires a shot.
            var coordinates = Player1.FireShot();
            var result = Player2.ProcessShot(coordinates);
            Player1.ProcessShotResult(coordinates, result);

            if (!Player2.HasLost) //If player 2 already lost, we can't let them take another turn.
            {
                coordinates = Player2.FireShot();
                result = Player1.ProcessShot(coordinates);
                Player2.ProcessShotResult(coordinates, result);
            }
        }

        public void PlayToEnd()
        {
            while (!Player1.HasLost && !Player2.HasLost)
            {
                PlayRound();
            }

            // THC: Display code has to be integrated here
            Player1.OutputBoards();
            Player2.OutputBoards();

            if (Player1.HasLost)
            {
                Console.WriteLine(Player2.Name + " has won the game!");
            }
            else if (Player2.HasLost)
            {
                Console.WriteLine(Player1.Name + " has won the game!");
            }
        }
    }
}
