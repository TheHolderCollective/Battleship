using Spectre.Console;
using System.Numerics;
using System;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private void ActivateMainMenuMode()
        {
            DeactivateAllLayouts();
            gameLayout["StartScreen"].Visible();
            displayMode = DisplayMode.MainMenu;
        }
        private void ActivateShipPlacementMode()
        {
            DeactivateAllLayouts();
            gameLayout["ShipPlacementBoard"].Visible();
            displayMode = DisplayMode.ShipPlacement;
            shipPlacementMode = ShipPlacementMode.SelectShip;
        }
        private void ActivateGamePlayMode()
        {
            DeactivateAllLayouts();

            //new
            var playerBoard1 = String.Format(Environment.NewLine + MakeGameBoard(gamePlayer1));
            var playerBoardText = new Markup(playerBoard1).Centered();
            var playerBoardPanel = new Panel(playerBoardText).Expand().Header(gamePlayer1.Name + "'s Board").HeaderAlignment(Justify.Center);
            gameLayout["PlayerGameBoard"].Update(playerBoardPanel);
            //

            gameLayout["GameBoard"].Visible();
            displayMode = DisplayMode.GamePlay;
        }
        private void ActivateDemoMode()
        {
            DeactivateAllLayouts();
            gameLayout["DemoBoard"].Visible();
            displayMode = DisplayMode.Demo;
        } 
    }
}
