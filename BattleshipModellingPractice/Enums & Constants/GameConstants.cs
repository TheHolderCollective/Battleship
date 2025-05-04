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
    }
}
