using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame.Objects.Boards
{
    public class ShipPlacements
    {
        public ShipType Type { get; set; }
        public ShipOrientation Orientation { get; set; }
        public Coordinates[] ShipCoordinates { get; set; } 

        public ShipPlacements()
        {

        }
        public ShipPlacements(ShipType shipType, ShipOrientation shipOrientation, Coordinates[] shipCoordinates)
        {
            Type = shipType;
            Orientation = shipOrientation;
            UpdateCoordinates(shipCoordinates);
        }
        public void UpdateCoordinates(Coordinates[] coordinates)
        {
            ShipCoordinates = new Coordinates[coordinates.Length];

            for (int i = 0; i < coordinates.Length; i++)
            {
                ShipCoordinates[i] = new Coordinates(coordinates[i].Row, coordinates[i].Column);
            }
        }
    }
}
