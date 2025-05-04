using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                    switch (keyPressed.Key)
                    {
                        case ConsoleKey.Backspace:
                            gameLayout["PlayerBoard"].Update(CreatePlayerBoardLayout(gamePlayer1));
                            break;
                        case ConsoleKey.Tab:
                            gameLayout["PlayerBoard"].Invisible();
                            gameLayout["StatusBoard"].Invisible();
                            break;
                        case ConsoleKey.Clear:
                            break;
                        case ConsoleKey.Enter:
                            gameLayout["PlayerBoard"].Visible();
                            gameLayout["StatusBoard"].Visible();
                            break;
                        case ConsoleKey.Pause:
                            break;
                        case ConsoleKey.Escape:
                            continuePlaying = false;
                            break;
                        case ConsoleKey.Spacebar:
                            gameLayout["PlayerBoard"].Update(CreatePlayerBoardLayoutTest());
                            break;
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
                        default:
                            break;
                    }
                    ctx.Refresh();
                }
            }
        }
       
    }
}
