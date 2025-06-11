using Spectre.Console;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private void ActivateMainMenuMode()
        {
            DeactivateAllLayouts();
            gameLayout["StartScreen"].Visible();
            SetDisplayMode(DisplayMode.MainMenu);
        }
        private void ActivateShipPlacementMode()
        {
            DeactivateAllLayouts();
            gameLayout["ShipPlacementBoard"].Visible();
            SetDisplayMode(DisplayMode.ShipPlacement);
            SetShipPlacementMode(ShipPlacementMode.SelectShip);
            SetGameStatus(GameStatus.ShipPlacementInProgress);
        }
        private void ActivateGamePlayMode()
        {
            DeactivateAllLayouts();
            gameLayout["GameBoard"].Visible();
            SetDisplayMode(DisplayMode.GamePlay);
            SetGameStatus(GameStatus.BattleInProgress);
        }
    }
}
