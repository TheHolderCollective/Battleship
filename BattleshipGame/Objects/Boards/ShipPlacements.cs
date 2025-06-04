using BattleshipGame.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame.Objects.Boards
{
    public class ShipPlacements
    {
        public ShipType TypeOfShip { get; set; }
        public ShipOrientation Orientation { get; set; }
        public Coordinates[] ShipCoordinates { get; set; } 
        public string PlacementLog { get; set; }
        public ShipPlacements()
        {

        }
        public ShipPlacements(ShipType shipType, ShipOrientation shipOrientation, Coordinates[] shipCoordinates)
        {
            TypeOfShip = shipType;
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

            UpdatePlacementLog();
        }
        private void UpdatePlacementLog()
        {
            int shipStartRow = ShipCoordinates[0].Row;
            int shipStartCol = ShipCoordinates[0].Column;
            int shipEndRow = ShipCoordinates[ShipCoordinates.Length - 1].Row;
            int shipEndCol = ShipCoordinates[ShipCoordinates.Length - 1].Column;

            string shipName = TypeOfShip.GetAttributeOfType<DescriptionAttribute>().Description;

            PlacementLog = $"> {shipName} placed at ({shipStartRow},{shipStartCol} -- ({shipEndRow},{shipEndCol})";
        }
    }
}
