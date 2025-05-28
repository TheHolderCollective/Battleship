namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        public void MainMenuSelectNext()
        {
            mainMenu.SelectNextItem();
            gameLayout["MainMenu"].Update(mainMenu.GetMenuAsPanel());
        }

        public void MainMenuSelectPrevious()
        {
            mainMenu.SelectPreviousItem();
            gameLayout["MainMenu"].Update(mainMenu.GetMenuAsPanel());
        }

        public void ShipMenuSelectNext()
        {
            shipMenu.SelectNextItem();
            gameLayout["ShipPlacementMenu"].Update(shipMenu.GetMenuAsPanel());
        }

        public void ShipMenuSelectPrevious()
        {
            shipMenu.SelectPreviousItem();
            gameLayout["ShipPlacementMenu"].Update(shipMenu.GetMenuAsPanel());
        }
    }
}
