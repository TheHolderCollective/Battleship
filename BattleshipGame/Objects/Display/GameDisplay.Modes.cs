using Spectre.Console;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private void ActivateMainMenuMode()
        {
            DeactivateAllLayouts();
            gameLayout["StartScreen"].Visible(); 
        }
        private void ActivateShipPlacementMode()
        {
            DeactivateAllLayouts();
            gameLayout["ShipPlacementBoard"].Visible();
        }

        private void ActivateGamePlayMode()
        {
            DeactivateAllLayouts();
            gameLayout["GameBoard"].Visible();
        }
        private void ActivateDemoMode()
        {
            DeactivateAllLayouts();
            gameLayout["DemoBoard"].Visible();

        } 
    }
}
