﻿using System;
using System.Collections.Generic;
using System.Text;
using BattleshipGame.Objects.GameMenu;
using Spectre.Console;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        #region Helpers for constructor
        private void SetupPlayers()
        {
            gamePlayer1 = new Player("Challenger");
            gamePlayer2 = new Player("General Supreme");
            gamePlayer2.PlaceShipsRandomly();
        }
        private void CreateMenus()
        {
            mainMenu = new Menu(MenuItemLists.MainMenuItems);
            shipMenu = new Menu(MenuItemLists.ShipMenuItems);
        }
        private void CreateGameLayouts()
        {
            gameLayout = CreateLayouts();
        }
        #endregion

        #region Helpers for updating game modes and statuses
        private void SetGameStatus(GameStatus status)
        {
            gameStatus = status;
        }
        private void SetDisplayMode(DisplayMode mode)
        {
            displayMode = mode; 
        }
        private void SetShipPlacementMode(ShipPlacementMode mode)
        {
            shipPlacementMode = mode;
        }
        #endregion

        #region Helpers for setting up the display
        private void SetupConsole()
        {
            // TODO Fix issues with window sizing which appear when game is launched using a maximized console
            decimal windowWidthScaleFactor = Decimal.Parse(Properties.Resources.WindowWidthScaleFactor);
            decimal windowHeightScaleFactor = Decimal.Parse(Properties.Resources.WindowHeightScaleFactor);

            AnsiConsole.Clear();
            Console.WindowWidth = (int)((decimal)WindowDimensions.Width * windowWidthScaleFactor);
            Console.WindowHeight = (int)((decimal)WindowDimensions.Height * windowHeightScaleFactor);
            Console.CursorVisible = false;
        }
        private void SetupLiveDisplay(Layout layout)
        {
            liveDisplay = AnsiConsole.Live(layout);
            liveDisplay.AutoClear(false);
            liveDisplay.Overflow(VerticalOverflow.Ellipsis);
            liveDisplay.Cropping(VerticalOverflowCropping.Top);
        }
        private void StartLiveDisplay()
        {
            liveDisplay.Start(ctx =>
            {
                ActivateMainMenuMode();
                ctx.Refresh();
                ProcessUpdates(ctx);
            });
        }
        private void ResetLayoutsForRestart()
        {
            SetupPlayers(); // players need to be setup again for restart
            UpdateShipPlacementGameboard();
            UpdateShipPlacementInfo();
            UpdateShipSelectionMenu();
            UpdateGameboard();
            UpdateFiringBoard();
            UpdateStatusboards();
            UpdateBattleResults();
            UpdateTargetInfo();
        }
        #endregion

        #region Helpers for Layouts
        private string GetRoundResultsSummary(Player player1, Player player2)
        {
            StringBuilder resultsText = new StringBuilder();
            if (player1.RoundNumber > 0 )
            {
                resultsText.AppendLine("Round " + player1.RoundNumber + " - " + player1.FiredShot);
                resultsText.AppendLine("Round " + player2.RoundNumber + " - " + player2.ReceivedShot);

                if (player2.ShipStatus != String.Empty)
                {
                    resultsText.AppendLine("Round " + player2.RoundNumber + " - " + player2.ShipStatus);
                }

                resultsText.AppendLine("Round " + player2.RoundNumber + " - " + player2.FiredShot);
                resultsText.AppendLine("Round " + player1.RoundNumber + " - " + player1.ReceivedShot);

                if (player1.ShipStatus != String.Empty)
                {
                    resultsText.AppendLine("Round " + player1.RoundNumber + " - " + player1.ShipStatus);
                }
            }
            return resultsText.ToString();
        }
        private string GetShipStatusLists(List<Ship> shipsList)
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
        private string MakeGameBoard(Player player)
        {
            StringBuilder flattenedGameBoard = new StringBuilder();
            int gameBoardSize = (int)BoardDimensions.Width;
            var gameBoard = player.OutputGameBoard();
            string padding = "  ";

            for (int i = 0; i < gameBoardSize; i++)
            {
                string boardLine = string.Empty;

                for (int j = 0; j < gameBoardSize; j++)
                {
                    var gameBoardPanel = AddMarkupToGameboardPanel(gameBoard[i, j], padding);
                    boardLine = string.Concat(boardLine, gameBoardPanel);
                }

                flattenedGameBoard.Append(boardLine + "\n\n");
            }

            return flattenedGameBoard.ToString();
        }
        private string MakeFiringBoard(Player player)
        {
            StringBuilder flattenedFiringBoard = new StringBuilder();
            int gameBoardSize = (int)BoardDimensions.Width;
            var firingBoard = player.OutputFiringBoard();
            string padding = "  ";

            for (int i = 0; i < gameBoardSize; i++)
            {
                string boardLine = string.Empty;

                for (int j = 0; j < gameBoardSize; j++)
                {
                    var gameBoardPanel = AddMarkupToGameboardPanel(firingBoard[i, j], padding);
                    boardLine = string.Concat(boardLine, gameBoardPanel);
                }

                flattenedFiringBoard.Append(boardLine + "\n\n");
            }

            return flattenedFiringBoard.ToString();
        }
        private string AddMarkupToGameboardPanel(string gameBoardPanel, string padding)
        {
            string panelWithMarkup = String.Empty;
            string shipMarkupTag = "[green]";
            string missMarkupTag = "[yellow][invert]";
            string hitMarkupTag = "[red][invert]";
            string defaultMarkupTag = "[blue]";
            string crosshairsMarkupTag = "[yellow]";
            string tagClose = "[/]";

            switch (gameBoardPanel.Trim())
            {
                case "B":
                case "C":
                case "D":
                case "S":
                case "A":
                    panelWithMarkup = string.Format(shipMarkupTag + gameBoardPanel.Trim() + tagClose + padding);
                    break;
                case "M":
                    panelWithMarkup = string.Format(missMarkupTag + "*" + tagClose + tagClose + padding);
                    break;
                case "X":
                    panelWithMarkup = string.Format(hitMarkupTag + gameBoardPanel.Trim() + tagClose + tagClose + padding);
                    break;
                case "o":
                    panelWithMarkup = string.Format(defaultMarkupTag + gameBoardPanel.Trim() + tagClose + padding);
                    break;
                case "+":
                    panelWithMarkup = string.Format(crosshairsMarkupTag + gameBoardPanel.Trim() + tagClose + padding); // check if display is messed up
                    break;
                default:
                    panelWithMarkup = gameBoardPanel;
                    break;
            }

            return panelWithMarkup;
        }
        #endregion

    }
}
