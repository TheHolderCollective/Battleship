using BattleshipGame.Objects.GameMenu;
using Spectre.Console;
using System;

namespace BattleshipGame.Objects.Display
{
    // TODO Add app config file to pull game settings rather than hard coding them
    public partial class GameDisplay
    {
        private static Layout gameLayout;
        private static Player gamePlayer1;
        private static Player gamePlayer2;

        private static DisplayMode displayMode;
        private static ShipPlacementMode shipPlacementMode;
        private static Menu mainMenu;
        private static Menu shipMenu;

        private static int shipX;
        private static int shipY;
        private static ShipOrientation shipOrientation;

        public GameDisplay(Player player1, Player player2)
        {
            gamePlayer1 = player1;
            gamePlayer2 = player2;
            gamePlayer2.PlaceShipsRandomly();

            //shipX = GameConstants.DefaultShipX;
            //shipY = GameConstants.DefaultShipY;

            mainMenu = new Menu(MenuItemLists.MainMenuItems);
            shipMenu = new Menu(MenuItemLists.ShipMenuItems);

            // At start up show Main menu
            displayMode = DisplayMode.MainMenu;
            
            // for working on demo use:
            //displayMode = DisplayMode.Demo;
            gameLayout = CreateLayouts();
        }
        public void ShowDisplay()
        {
            AnsiConsole.Clear();

            // TODO Fix issues with window sizing which appear when game is launched using a maximized console
            Console.WindowWidth = (int)((decimal)WindowDimensions.Width * GameConstants.WindowWidthScaleFactor);
            Console.WindowHeight = (int)((decimal)WindowDimensions.Height * GameConstants.WindowHeightScaleFactor);
            Console.CursorVisible = false;
            //

            LiveDisplay gameDisplay = AnsiConsole.Live(gameLayout);

            gameDisplay.AutoClear(false);
            gameDisplay.Overflow(VerticalOverflow.Ellipsis);
            gameDisplay.Cropping(VerticalOverflowCropping.Top);

            gameDisplay.Start(ctx =>
            {
                ActivateMainMenuMode();
                ctx.Refresh();
                ProcessUpdates(ctx);
            });
        }
        private void ProcessUpdates(LiveDisplayContext ctx)
        {
            // test code to be replaced
            bool continuePlaying = true;

            while (continuePlaying)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey keyPressed = Console.ReadKey(true).Key;

                    if (keyPressed == ConsoleKey.Escape)
                    {
                       continuePlaying = false;
                       continue;
                    }

                    ProcessPlayerInputs(keyPressed);
                    ctx.Refresh();
                }
            }
        }
        private void ProcessPlayerInputs(ConsoleKey keyPressed)
        {
            switch (keyPressed)
            {
                case ConsoleKey.F1:
                    ActivateMainMenuMode();
                    //ActivateDemoMode();
                    break;
                case ConsoleKey.F2:
                    ProcessInputF2Key(displayMode);
                    break;
                case ConsoleKey.F3:
                    ActivateShipPlacementMode();
                    break;
                case ConsoleKey.F4:
                    ProcessInputF4Key(displayMode);
                    break;
                case ConsoleKey.UpArrow:
                    ProcessInputUpArrowKey(displayMode);
                    break;
                case ConsoleKey.DownArrow:
                    ProcessInputDownArrowKey(displayMode);
                    break;
                case ConsoleKey.RightArrow:
                    ProcessInputRightArrowKey(displayMode);
                    break;
                case ConsoleKey.LeftArrow:
                    ProcessInputLeftArrowKey(displayMode);
                    break;
                case ConsoleKey.Enter:
                    ProcessInputEnterKey(displayMode);
                    break;
                case ConsoleKey.Spacebar:
                    ProcessInputSpacebar(displayMode);
                    break;
                default:
                    break;
            }
        }
    }
}
