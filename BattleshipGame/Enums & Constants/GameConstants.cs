namespace BattleshipGame
{
    public static class GameConstants
    {
        public static string GameTitle
        {
            get
            {
                return @"                 __  __ _____ _____ _   ___  __  _  _ _ ___                 " + "\n" +
                       @" //\  __  //\   |  \/  \_   _|_   _| | | __/' _/| || | | _,\   //\  __  //\ " + "\n" +
                       @"`\//'|__|`\//'  | -< /\ || |   | | | |_| _|`._`.| >< | | v_/  `\//'|__|`\//'" + "\n" +
                       @"                |__/_||_||_|   |_| |___|___|___/|_||_|_|_|                  ";

            }
        }

        public static string MainMenuBorder
        {
            get
            {
                return @"________   " + "\n" +
                       @"\_____  \  " + "\n" +
                       @" /   |   \ " + "\n" +
                       @"/    |    \" + "\n" +
                       @"\_______  /" + "\n" +
                       @"        \/ " + "\n" +
                       @"________   " + "\n" +
                       @"\_____  \  " + "\n" +
                       @" /   |   \ " + "\n" +
                       @"/    |    \" + "\n" +
                       @"\_______  /" + "\n" +
                       @"        \/ " + "\n" +
                       @"________   " + "\n" +
                       @"\_____  \  " + "\n" +
                       @" /   |   \ " + "\n" +
                       @"/    |    \" + "\n" +
                       @"\_______  /" + "\n" +
                       @"        \/ ";
            }
        }

        public static string GameFooter
        {
            get
            {
                return @"  ____   ____   ____   ____   ____   ____   ____   ____   ____   ____   ____   ____   ____   ____   ____   ____   ____   ____   ____   " + "\n" +
                       @" / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \ / /\ \  " + "\n" +
                       @"< <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > X <  > > " + "\n" +
                       @"`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/`\_\/_/  " + "\n";
            }
        }

        public static string ShipPlacementKeyboardTips
        {
            get
            {
                return @"Arrow keys - Select ship from menu or move ships on the board" + "\n" +
                       @"Enter      - Place selected ship on board" + "\n" +
                       @"             Fix ship position and reactivate menu selection" + "\n" +
                       @"Space      - Rotate ship" + "\n" +
                       @"F2         - Start battle (works only after placing all ships)" + "\n" +
                       @"F4         - Randomly place ships on board (only works on empty board)" + "\n";
            }
        }

        public static string GamePlayKeyboardTips
        {
            get
            {
                return @"Arrow keys - Move crosshairs to desired target cell" + "\n" +
                       @"Space      - Fire shot" + "\n" +
                       @"F1         - Return to main menu at any time" + "\n" +
                       @"Enter      - Return to main menu when game is over" + "\n" +
                       @"ESC        - Exit game" + "\n";
            }
        }

        public static decimal WindowWidthScaleFactor
        {
            get
            {
                return 1.27M;
            }
        }
        public static decimal WindowHeightScaleFactor
        {
            get
            {
                return 1.35M;
            }
        }

        public static int ShipTotal
        {
            get
            {
                return 5;
            }
        }

    }
}
