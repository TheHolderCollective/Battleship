using Spectre.Console;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private void ActivateMainMenuMode()
        {
            DeactivateAllLayoutsExceptTitleLayout();
            gameLayout["StartScreen"].Visible();
            SetDisplayMode(DisplayMode.MainMenu);
        }
        private void ActivateShipPlacementMode()
        {
            DeactivateAllLayoutsExceptTitleLayout();
            gameLayout["ShipPlacementBoard"].Visible();
            SetDisplayMode(DisplayMode.ShipPlacement);
            SetShipPlacementMode(ShipPlacementMode.SelectShip);
            SetGameStatus(GameStatus.ShipPlacementInProgress);
        }
        private void ActivateGamePlayMode()
        {
            DeactivateAllLayoutsExceptTitleLayout();
            gameLayout["GameBoard"].Visible();
            SetDisplayMode(DisplayMode.GamePlay);
            SetGameStatus(GameStatus.BattleInProgress);
        }
    }
}
