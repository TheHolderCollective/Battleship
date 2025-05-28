using Spectre.Console;
using System.Numerics;
using System;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        public void UpdatePlayerBoardForShipPlacement()
        {
            var playerBoard1 = String.Format(Environment.NewLine + MakeGameBoard(gamePlayer1));
            var playerBoardText = new Markup(playerBoard1).Centered();
            var playerBoardPanel = new Panel(playerBoardText).Expand().Header(gamePlayer1.Name + "'s Board").HeaderAlignment(Justify.Center);

            gameLayout["ShipPlacementGameBoard"].Update(playerBoardPanel);
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
