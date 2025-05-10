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
        private DisplayMode displayMode;

        public GameDisplay(Player player1, Player player2)
        {
            gamePlayer1 = player1;
            gamePlayer2 = player2;

            // At start up show Main menu
            displayMode = DisplayMode.MainMenu;

            // for working on demo use:
            displayMode = DisplayMode.Demo;

            gameLayout = CreateLayout();
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
                displayMode = DisplayMode.MainMenu;
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
                    }

                    if (displayMode == DisplayMode.MainMenu)
                    {
                        ProcessMainMenuInputs(keyPressed);
                    }
                    else
                    {
                        ProcessOtherInputs(keyPressed);
                    }
                    ctx.Refresh();
                }
            }
        }

        private void ProcessMainMenuInputs(ConsoleKey keyPressed)
        {
            switch (keyPressed)
            {
                case ConsoleKey.LeftArrow:
                    gameLayout["Title"].Update(CreateTitleLayout("[red]"));
                    break;
                case ConsoleKey.UpArrow:
                    gameLayout["Title"].Update(CreateTitleLayout("[green]"));
                    break;
                case ConsoleKey.RightArrow:
                    gameLayout["Title"].Update(CreateTitleLayout("[blue]"));
                    break;
                case ConsoleKey.DownArrow:
                    gameLayout["Title"].Update(CreateTitleLayout("[purple]"));
                    break;
                case ConsoleKey.Backspace:
                    displayMode = DisplayMode.GamePlay;
                    DeactivateMainMenuMode();
                    ActivateGamePlayMode();
                    break;
                default:
                    break;
            }
        }

        private void ProcessOtherInputs(ConsoleKey keyPressed)
        {
            switch (keyPressed)
            {
                case ConsoleKey.D:
                    displayMode = DisplayMode.Demo;
                    ActivateDemoMode();
                    break;
                case ConsoleKey.Backspace:
                    displayMode = DisplayMode.GamePlay;
                    ActivateGamePlayMode();
                    break;
                case ConsoleKey.Tab:
                    displayMode = DisplayMode.ShipPlacement;
                    ActivateShipPlacementMode();
                    break;
                case ConsoleKey.Spacebar:
                    displayMode = DisplayMode.MainMenu;
                    ActivateMainMenuMode();
                    break;
                default:
                    break;
            }
        }
    }
}
