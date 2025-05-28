using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
       
        private static string GetRoundResultsSummary(Player player1, Player player2)
        {
            StringBuilder resultsText = new StringBuilder();

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
        private static string MakeGameBoard(Player player)
        {
            StringBuilder flattenedGameBoard = new StringBuilder();
            int gameBoardSize = (int)BoardDimensions.Width;
            var gameBoard = player.OutputGameBoard();

            for (int i = 0; i < gameBoardSize; i++)
            {
                string boardLine = string.Empty;

                for (int j = 0; j < gameBoardSize; j++)
                {
                    var gameBoardPanel = AddMarkupToGameboardPanel(gameBoard[i, j]);
                    boardLine = string.Concat(boardLine, gameBoardPanel);
                }

                flattenedGameBoard.Append(boardLine + "\n\n");
            }

            return flattenedGameBoard.ToString();
        }

        private static string MakeFiringBoard(Player player)
        {
            StringBuilder flattenedFiringBoard = new StringBuilder();
            int gameBoardSize = (int)BoardDimensions.Width;
            var firingBoard = player.OutputFiringBoard();

            for (int i = 0; i < gameBoardSize; i++)
            {
                string boardLine = string.Empty;

                for (int j = 0; j < gameBoardSize; j++)
                {
                    var gameBoardPanel = AddMarkupToGameboardPanel(firingBoard[i, j]);
                    boardLine = string.Concat(boardLine, gameBoardPanel);
                }

                flattenedFiringBoard.Append(boardLine + "\n\n");
            }

            return flattenedFiringBoard.ToString();
        }

        private static string AddMarkupToGameboardPanel(string gameBoardPanel)
        {
            string panelWithMarkup = String.Empty;
            string shipMarkupTag = "[green]";
            string missMarkupTag = "[yellow][invert]";
            string hitMarkupTag = "[red][invert]";
            string defaultMarkupTag = "[blue]";
            string tagClose = "[/]";

            switch (gameBoardPanel.Trim())
            {
                case "B":
                case "C":
                case "D":
                case "S":
                case "A":
                    panelWithMarkup = string.Format(shipMarkupTag + gameBoardPanel.Trim() + tagClose + "  ");
                    break;
                case "M":
                    panelWithMarkup = string.Format(missMarkupTag + "*" + tagClose + tagClose + "  ");
                    break;
                case "X":
                    panelWithMarkup = string.Format(hitMarkupTag + gameBoardPanel.Trim() + tagClose + tagClose + "  ");
                    break;
                case "o":
                    panelWithMarkup = string.Format(defaultMarkupTag + gameBoardPanel.Trim() + tagClose + "  ");
                    break;
                default:
                    panelWithMarkup = gameBoardPanel;
                    break;
            }

            return panelWithMarkup;
        }
    }
}
