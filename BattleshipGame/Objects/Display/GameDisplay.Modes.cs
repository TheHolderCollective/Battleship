using Spectre.Console;

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
