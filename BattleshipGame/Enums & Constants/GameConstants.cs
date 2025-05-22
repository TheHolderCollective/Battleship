
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

        public static readonly string[] MainMenu =
        {
            "Play Game",
            "Run Demo",
            "Add Player Info",
            "Exit Game"
        };

    }
}
