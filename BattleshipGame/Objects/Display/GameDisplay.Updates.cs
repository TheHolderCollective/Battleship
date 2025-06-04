using Spectre.Console;
using System.Linq;
using System;
using System.Text;
using System.Collections.Generic;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        public void UpdateShipPlacementGameboard()
        {
            var playerBoard1 = String.Format(Environment.NewLine + MakeGameBoard(gamePlayer1));
            var playerBoardText = new Markup(playerBoard1).Centered();
            var playerBoardPanel = new Panel(playerBoardText).Expand().Header(gamePlayer1.Name + "'s Board").HeaderAlignment(Justify.Center);

            gameLayout["ShipPlacementGameBoard"].Update(playerBoardPanel);
        }

        public void UpdateShipPlacementInfo()
        {
            StringBuilder updatesText = new StringBuilder();
            List<string> shipUpdatesLog = gamePlayer1.ShipPlacementLogs;

            var lastFiveShipUpdates = shipUpdatesLog.Skip(Math.Max(0,shipUpdatesLog.Count - 6)); // remove magic number at some point

            foreach (var update in lastFiveShipUpdates)
            {
                updatesText.AppendLine(update);
            }

            var shipUpdatesMarkup = new Markup(updatesText.ToString()).LeftJustified();
            var shipUpdatesPanel = new Panel(shipUpdatesMarkup).Expand().Header("Ship Placement Updates").HeaderAlignment(Justify.Center);

            gameLayout["ShipPlacementInfo"].Update(shipUpdatesPanel);

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
