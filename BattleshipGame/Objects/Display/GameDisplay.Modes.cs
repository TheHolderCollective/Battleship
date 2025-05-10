using Spectre.Console;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private void ActivateShipPlacementMode()
        {
            //DeactivateGamePlayLayouts();
            gameLayout["ShipPlacementBoard"].Visible();
            gameLayout["PlayerBoard"].Visible();
            gameLayout["FiringBoard"].Invisible();
            gameLayout["StartScreen"].Invisible();
        }
        private void DeactivateShipPlacementMode()
        {
            ActivateGamePlayMode();
        }
        private void ActivateGamePlayMode()
        {
            ActivateGamePlayLayouts();
        }
        private void DeactivateGamePlayMode()
        {
            DeactivateGamePlayLayouts();
        }
        private void ActivateMainMenuMode()
        {
            DeactivateGamePlayLayouts();
            gameLayout["StartScreen"].Visible(); 

        }

        private void DeactivateMainMenuMode()
        {
            gameLayout["StartScreen"].Invisible();
        }
        private void ActivateDemoMode()
        {
            gameLayout["PlayerBoard"].Visible();
            gameLayout["FiringBoard"].Visible();
            gameLayout["StatusBoard"].Visible();
            gameLayout["ShipPlacementBoard"].Invisible();
            gameLayout["TargetInfo"].Invisible();

            gameLayout["MenuLeftBorder"].Invisible();
            gameLayout["MenuRightBorder"].Invisible();
        }
        private void DeactivateDemoMode()
        {
            ActivateGamePlayLayouts();
        }
        private void ActivateGamePlayLayouts()
        {
            gameLayout["GameBoard"].Visible();
            gameLayout["GameInfo"].Visible();
            gameLayout["ShipPlacementBoard"].Invisible();

        }
        private void DeactivateGamePlayLayouts()
        {
            gameLayout["GameBoard"].Invisible();
            gameLayout["GameInfo"].Invisible();
        }
    }
}
