using System;
using BattleshipGame.Objects.Display;


namespace BattleshipGame.Objects.Games
{
    public class Game
    {
        private Player Player1;
        private Player Player2;
        private GameDisplay gameDisplay;

        public Game()
        {
            Player1 = new Player("Challenger");
            Player2 = new Player("General Supreme");
            gameDisplay = new GameDisplay(Player1, Player2); 
        }

        public void PlayGame()
        {
            gameDisplay.ShowDisplay();
        } 

    }
}
