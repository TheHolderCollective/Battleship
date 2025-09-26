using BattleshipGame.Extensions;
using BattleshipGame.Objects.Boards;
using BattleshipGame.Objects.Ships;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BattleshipGame.Objects.Players
{
    public abstract class Player
    {
        public string Name { get; set; }
        public GameBoard GameBoard { get; set; }
        public FiringBoard FiringBoard { get; set; }
        public List<Ship> Ships { get; set; }
        public List<ShipPlacements> ShipLocations { get; set; }
        public List<string> ShipPlacementLogs { get; }
        public string FiredShot { get; set; }
        public string ReceivedShot { get; set; }
        public string ShipStatus { get; set; }
        public int RoundNumber { get; set; }
        protected OccupationType PriorOccupationType { get; set; }
        private bool ShipsAlreadyPlaced
        {
            get
            {
                return ShipLocations.Count > 0;
            }
        }
        public bool HasLost
        {
            get
            {
                return Ships.All(x => x.IsSunk);
            }
        }
        
        public Player(string name)
        {
            Name = name;
            Ships = new List<Ship>()
            {
                new Carrier(),
                new Battleship(),
                new Cruiser(),
                new Destroyer(),
                new Submarine()
            };

            GameBoard = new GameBoard();
            FiringBoard = new FiringBoard();
            ShipLocations = new List<ShipPlacements>();
            ShipPlacementLogs = new List<string>();
           
            PriorOccupationType = FiringBoard.Panels.At(5, 5).OccupationType;

            RoundNumber = 0;
        }
        public string[,] OutputGameBoard()
        {
            int boardWidth = (int)BoardDimensions.Width;
            int boardHeight = (int)BoardDimensions.Height;

            string[,] gameBoard = new string[boardWidth, boardHeight];

            for (int row = 1; row <= boardWidth; row++)
            {
                for (int height = 1; height <= boardHeight; height++)
                {
                    gameBoard[row - 1, height - 1] = GameBoard.Panels.At(row, height).Status + "  ";
                }
            }
            return gameBoard;
        }

        public string[,] OutputFiringBoard()
        {
            int boardWidth = (int)BoardDimensions.Width;
            int boardHeight = (int)BoardDimensions.Height;

            string[,] firingBoard = new string[boardWidth, boardHeight];

            for (int row = 1; row <= boardWidth; row++)
            {
                for (int height = 1; height <= boardHeight; height++)
                {
                    firingBoard[row - 1, height - 1] = FiringBoard.Panels.At(row, height).Status + "  ";
                }
            }
            return firingBoard;

        }
        public abstract Coordinates FireShot();

        public ShotResult ProcessShot(Coordinates coords)
        {
            var panel = GameBoard.Panels.At(coords.Row, coords.Column);
            ShipStatus = String.Empty;

            if (!panel.IsOccupied)
            {
                ReceivedShot = String.Format(Name + " says: \"Miss!\"");
                GameBoard.Panels.At(coords.Row, coords.Column).OccupationType = OccupationType.Miss;
                return ShotResult.Miss;
            }
            var ship = Ships.First(x => x.OccupationType == panel.OccupationType);
            ship.Hits++;

            ReceivedShot = String.Format(Name + " says: \"Hit!\"");
            GameBoard.Panels.At(coords.Row, coords.Column).OccupationType = OccupationType.Hit;

            if (ship.IsSunk)
            {
                ShipStatus = String.Format(Name + " says: \"You sunk my " + ship.Name + "!\"");
            }

            return ShotResult.Hit;
        }

        public void ProcessShotResult(Coordinates coords, ShotResult result)
        {
            var panel = FiringBoard.Panels.At(coords.Row, coords.Column);
            switch (result)
            {
                case ShotResult.Hit:
                    panel.OccupationType = OccupationType.Hit;
                    PriorOccupationType = OccupationType.Hit;
                    break;

                default:
                    panel.OccupationType = OccupationType.Miss;
                    PriorOccupationType = OccupationType.Miss;
                    break;
            }
        }

        public bool IsShotAvailable()
        {
            bool targetPanelAvailable = (PriorOccupationType == OccupationType.Empty) ? true : false;

            return targetPanelAvailable;
        }

        public void PlaceShipsRandomly()
        {
            if (ShipsAlreadyPlaced)
            {
                return;
            }

            //Random class creation stolen from http://stackoverflow.com/a/18267477/106356
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            foreach (var ship in Ships)
            {
                //Select a random row/column combination, then select a random orientation.
                //If none of the proposed panels are occupied, place the ship
                //Do this for all ships

                Ship selectedShip = ship;

                bool isOpen = true;
                while (isOpen)
                {
                    var startcolumn = rand.Next(1, 11);
                    var startrow = rand.Next(1, 11);
                    int endrow = startrow, endcolumn = startcolumn;
                    ShipOrientation orientation = (rand.Next(1, 101) % 2) == 0 ? ShipOrientation.Horizontal : ShipOrientation.Vertical;

                    List<int> panelNumbers = new List<int>();
                    if (orientation == ShipOrientation.Horizontal)
                    {
                        for (int i = 1; i < ship.Width; i++)
                        {
                            endcolumn++;
                        }
                    }
                    else
                    {
                        for (int i = 1; i < ship.Width; i++)
                        {
                            endrow++;
                        }
                    }

                    // We cannot place ships beyond the boundaries of the board
                    if (endrow > 10 || endcolumn > 10)
                    {
                        isOpen = true;
                        continue;
                    }

                    // Check if specified panels are occupied
                    var affectedPanels = GameBoard.Panels.Range(startrow, startcolumn, endrow, endcolumn);
                    if (affectedPanels.Any(x => x.IsOccupied))
                    {
                        isOpen = true;
                        continue;
                    }

                    foreach (var panel in affectedPanels)
                    {
                        panel.OccupationType = ship.OccupationType;
                    }
                    isOpen = false;

                    // new -- update ShipLocation
                    Coordinates[] shipCoords = new Coordinates[affectedPanels.Count];
                    for (int i = 0; i < affectedPanels.Count; i++)
                    {
                        shipCoords[i] = new Coordinates(affectedPanels[i].Coordinates.Row, affectedPanels[i].Coordinates.Column);
                    }

                    ShipPlacements shipLocation = new ShipPlacements(selectedShip.ShipType, orientation, shipCoords);
                    ShipLocations.Add(shipLocation);
                    selectedShip.IsPlaced = true;
                    UpdateShipPlacementLog();
                }
            }
        }
        
        private void UpdateShipPlacementLog()
        {
            ShipPlacementLogs.Clear();
            foreach (var location in ShipLocations)
            {
                ShipPlacementLogs.Add(location.PlacementLog);
            }
        }

    }

}
