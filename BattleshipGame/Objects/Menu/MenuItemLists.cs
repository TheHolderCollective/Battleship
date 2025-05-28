namespace BattleshipGame.Objects.GameMenu
{
    public class MenuItemLists
    {
        private static readonly string[] mainMenu =
        {
            "Play Game",
            "Run Demo",
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
