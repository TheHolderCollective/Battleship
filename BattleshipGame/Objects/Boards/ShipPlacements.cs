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
        public Coordinates[] shipCoordinates { get; set; } 

        public void UpdateCoordinates(Coordinates[] coordinates)
        {
            shipCoordinates = new Coordinates[coordinates.Length];

            for (int i = 0; i < coordinates.Length; i++)
            {
                shipCoordinates[i] = new Coordinates(coordinates[i].Row, coordinates[i].Column);
            }
        }
    }
}
