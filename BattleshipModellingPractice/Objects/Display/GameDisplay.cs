using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleshipGame.Objects.Display
{
    // TODO Add app config file to pull game settings rather than hard coding them
    public class GameDisplay
    {
        private static Layout gameLayout;
        private static Player gamePlayer1;
        private static Player gamePlayer2;

        public GameDisplay(Player player1, Player player2)
        {
            gamePlayer1 = player1;
            gamePlayer2 = player2;

            gameLayout = CreateLayout();
        }
        public void ShowDisplay()
        {
            AnsiConsole.Clear();

            // TODO Fix issues with window sizing which appear when game is launched using a maximized console
            Console.WindowWidth = (int)((decimal)WindowDimensions.Width * GameConstants.WindowScaleFactor);
            Console.WindowHeight = (int)((decimal)WindowDimensions.Height * GameConstants.WindowScaleFactor);
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
            bool continuePlaying = true;

            while (continuePlaying)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                    switch (keyPressed.Key)
                    {
                        case ConsoleKey.Backspace:
                            gameLayout["PlayerBoard"].Update(CreatePlayerBoardLayout("Player Board-(" + gamePlayer1.Name + ")"));
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
        private static Layout CreateLayout()
        {
            var titleLayout = CreateDefaultTitleLayout();
            var boardLayout = CreateGameBoardLayout();
            var resultsLayout = CreateResultsLayout();

            var gameLayout = new Layout().SplitRows(titleLayout, boardLayout, resultsLayout);

            return gameLayout;
        }
        private static Layout CreateDefaultTitleLayout()
        {
            return CreateTitleLayout("[purple]");
        }
        private static Layout CreateTitleLayout(string titleColor)
        {
            var titleLayout = new Layout("Title");

            var titleText = new Markup($"\n\n{titleColor}{GameConstants.GameTitle}[/]").Centered();
            var titlePanel = new Panel(titleText).Expand().Border(BoxBorder.Double);

            titleLayout.Size((int)LayoutSize.Title);
            titleLayout.Update(titlePanel);

            return titleLayout;
        }
        private static Layout CreateGameBoardLayout()
        {
            var playerBoardLayout = CreatePlayerBoardLayout(gamePlayer1.Name + "'s Board");
            var firingBoardLayout = CreateFiringBoardLayout(gamePlayer2.Name + "'s Board");
            var statusBoardLayout = CreateStatusBoardsLayout();

            var boardLayout = new Layout("GameBoard").SplitColumns(playerBoardLayout, firingBoardLayout, statusBoardLayout);
            boardLayout.Size((int)LayoutSize.Gameboard);

            return boardLayout;
        }
        private static Layout CreatePlayerBoardLayout(string playerBoardHeader)
        {

            var playerBoardLayout = new Layout("PlayerBoard").MinimumSize(60);

            var playerBoard1 = String.Format("{0}{1}{2}", "\n[blue]", MakeGameBoard(), "[/]");
            var playerBoardText = new Markup(playerBoard1).Centered();
            var playerBoardPanel = new Panel(playerBoardText).Expand().Header(playerBoardHeader).HeaderAlignment(Justify.Center);

            playerBoardLayout.Update(playerBoardPanel);

            return playerBoardLayout;
        }
        private static Layout CreateFiringBoardLayout(string playerBoardHeader)
        {
            var firingBoardLayout = new Layout("FiringBoard").MinimumSize(60);

            var playerBoard2 = String.Format("{0}{1}{2}", "\n[green][invert]", MakeGameBoard(), "[/][/]");
            var firingBoardText = new Markup(playerBoard2).Centered();
            var firingBoardPanel = new Panel(firingBoardText).Expand().Header(playerBoardHeader).HeaderAlignment(Justify.Center);

            firingBoardLayout.Update(firingBoardPanel);

            return firingBoardLayout;
        }
        private static Layout CreatePlayerBoardLayoutTest()
        {
            var playerBoardLayout = new Layout("PlayerBoard").MinimumSize(60);

            var playerBoard1 = String.Format("{0}{1}{2}", "\n[yellow]", MakeGameBoard(), "[/]");
            var playerBoardText = new Markup(playerBoard1).Centered();
            var playerBoardPanel = new Panel(playerBoardText).Expand().Header("Player Board Yellow").HeaderAlignment(Justify.Center);

            playerBoardLayout.Update(playerBoardPanel);

            return playerBoardLayout;
        }
        private static Layout CreateStatusBoardsLayout()
        {
            var playerStatusLayout = CreatePlayerStatusLayout("PlayerStatus", "Status (" + gamePlayer1.Name + ")", gamePlayer1);
            var opponentStatusLayout = CreatePlayerStatusLayout("OpponentStatus", "Status (" + gamePlayer2.Name + ")", gamePlayer2);

            var statusBoardLayout = new Layout("StatusBoard").SplitRows(playerStatusLayout, opponentStatusLayout);

            return statusBoardLayout;
        }
        private static Layout CreatePlayerStatusLayout(string layoutName, string statusHeader, Player player)
        {
            var playerStatusLayout = new Layout(layoutName);
            var playerStatusText = GetShipStatusLists(player.Ships);
            var playerStatusMarkup = new Markup(playerStatusText).LeftJustified();
            var playerStatusPanel = new Panel(playerStatusMarkup).Expand().Header(statusHeader).HeaderAlignment(Justify.Center);

            playerStatusLayout.Update(playerStatusPanel);

            return playerStatusLayout;
        }
        private static Layout CreateResultsLayout()
        {
            var resultsLayout = new Layout("Results");
            var resultsText = GetRoundResultsString(gamePlayer1, gamePlayer2);
            var resultsMarkup = new Markup(resultsText.ToString()).LeftJustified();
            var resultsPanel = new Panel(resultsMarkup).Expand().Header("Battle Updates").HeaderAlignment(Justify.Left);

            resultsLayout.Size((int)LayoutSize.Result);
            resultsLayout.Update(resultsPanel);

            return resultsLayout;
        }
        private static string GetRoundResultsString(Player player1, Player player2)
        {
            StringBuilder resultsText = new StringBuilder();

            resultsText.AppendLine(player1.FiredShot);
            resultsText.AppendLine(player2.ReceivedShot);
            resultsText.AppendLine(player2.FiredShot);
            resultsText.AppendLine(player1.ReceivedShot);

            return resultsText.ToString();
        }
        private static string GetShipStatusLists(List<Ship> shipsList)
        {
            StringBuilder shipStatus = new StringBuilder();

            shipStatus.Append(String.Format("\n{0,8}          {1,10}\n", "Ship", "Hits Left"));

            foreach (var ship in shipsList)
            {
                int hitsLeft = ship.Width - ship.Hits;

                string shipName = hitsLeft == 0 ? "[invert][red]" + ship.Name.Trim().PadRight(18) + "[/][/]" : ship.Name.Trim().PadRight(18);

                shipStatus.Append(String.Format($"\n{shipName,-16}   {hitsLeft,4}"));
            }

            return shipStatus.ToString();

        }
        private static string MakeGameBoard()
        {
            StringBuilder gameBoard = new StringBuilder();
            const int gameBoardSize = 10;

            for (int i = 0; i < gameBoardSize; i++)
            {
                var line = Enumerable.Repeat("*  ", gameBoardSize);
                var boardLine = string.Concat(line);
                gameBoard.Append(boardLine + "\n\n");
            }
            return gameBoard.ToString();
        }
    }
}
