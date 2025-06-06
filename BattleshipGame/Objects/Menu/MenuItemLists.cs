namespace BattleshipGame.Objects.GameMenu
{
    public class MenuItemLists
    {
        private static readonly string[] mainMenu =
        {
            "New Game",
            "Resume Game",
            "Exit Game"
        };

        private static readonly string[] shipMenu =
        {
            "Aircraft Carrier",
            "Battleship",
            "Cruiser",
            "Destroyer",
            "Submarine"
        };

        public static string[] MainMenuItems
        {
            get
            {
                return mainMenu;
            }
        }

        public static string[] ShipMenuItems
        {
            get 
            {
                return shipMenu;
            }
        }
    }
}
