using Spectre.Console;
using System.Linq;
using System;
using System.Text;
using System.Collections.Generic;
using System.Numerics;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private void UpdateShipPlacementGameboard()
        {
            var playerBoard1 = String.Format(Environment.NewLine + MakeGameBoard(gamePlayer1));
            var playerBoardText = new Markup(playerBoard1).Centered();
            var playerBoardPanel = new Panel(playerBoardText).Expand().Header(gamePlayer1.Name + "'s Board").HeaderAlignment(Justify.Center);

            gameLayout["ShipPlacementGameBoard"].Update(playerBoardPanel);
        }
        private void UpdateShipPlacementInfo()
        {
            StringBuilder updatesText = new StringBuilder();
            List<string> shipUpdatesLog = gamePlayer1.ShipPlacementLogs;

            //var lastFiveShipUpdates = shipUpdatesLog.Skip(Math.Max(0,shipUpdatesLog.Count - 6)); // remove magic number at some point

            foreach (var update in shipUpdatesLog)
            {
                updatesText.AppendLine(update);
            }

            var shipUpdatesMarkup = new Markup(updatesText.ToString()).LeftJustified();
            var shipUpdatesPanel = new Panel(shipUpdatesMarkup).Expand().Header("Ship Placement Updates").HeaderAlignment(Justify.Center);

            gameLayout["ShipPlacementInfo"].Update(shipUpdatesPanel);

        }
        private void UpdateTargetInfo()
        {
            var targetText = $"Targeting: ({gamePlayer1.CrosshairsX} , {gamePlayer1.CrosshairsY})";
            var targetMarkup = new Markup(targetText.ToString()).LeftJustified();
            var targetPanel = new Panel(targetMarkup).Expand().Header("Targeting Dashboard").HeaderAlignment(Justify.Center);

            gameLayout["TargetInfo"].Update(targetPanel);
        }
        private void UpdateFiringBoard()
        {
            var playerBoard2 = String.Format(Environment.NewLine + MakeFiringBoard(gamePlayer1));
            var firingBoardText = new Markup(playerBoard2).Centered();
            var firingBoardPanel = new Panel(firingBoardText).Expand().Header("Firing Board").HeaderAlignment(Justify.Center);

            gameLayout["OpponentFiringBoard"].Update(firingBoardPanel);
        }
        private void UpdateGameboard()
        {
            var playerBoard1 = String.Format(Environment.NewLine + MakeGameBoard(gamePlayer1));
            var playerBoardText = new Markup(playerBoard1).Centered();
            var playerBoardPanel = new Panel(playerBoardText).Expand().Header("Player Board").HeaderAlignment(Justify.Center);
          
            gameLayout["PlayerGameBoard"].Update(playerBoardPanel);
        }
        private void UpdateBattleResults()
        {
            var resultsText = GetRoundResultsSummary(gamePlayer1, gamePlayer2);
            var resultsMarkup = new Markup(resultsText.ToString()).LeftJustified();
            var resultsPanel = new Panel(resultsMarkup).Expand().Header("Battle Updates").HeaderAlignment(Justify.Center);

            gameLayout["Results"].Update(resultsPanel);

        }

        private ShipType GetShipType(string shipName)
        {
            ShipType shipType;

            switch (shipName) // change this to use enums
            {
                case "Aircraft Carrier":
                    shipType = ShipType.Carrier;
                    break;
                case "Battleship":
                    shipType = ShipType.Battleship;
                    break;
                case "Cruiser":
                    shipType = ShipType.Cruiser;
                    break;
                case "Destroyer":
                    shipType = ShipType.Destroyer;
                    break;
                case "Submarine":
                    shipType = ShipType.Submarine;
                    break;
                default:
                    shipType = ShipType.Unknown;
                    break;
            }
            return shipType;
        }
    }
}
