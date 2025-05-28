using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleshipGame.Objects.Boards;
using BattleshipGame.Objects.Ships;
using BattleshipGame.Extensions;

namespace BattleshipGame.Objects
{
    public class Player
    {
        public string Name { get; set; }
        public GameBoard GameBoard { get; set; }
        public FiringBoard FiringBoard { get; set; }
        public List<Ship> Ships { get; set; }

        // new additions
        public List<ShipPlacements> ShipLocations { get; set; }
        public string FiredShot { get; set; }
        public string ReceivedShot { get; set; }
        public string ShipStatus { get; set; }
        public int RoundNumber { get; set; }
        //
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
            { // RH: order significant do not change
                new Carrier(),
                new Battleship(),
                new Cruiser(),
                new Destroyer(),
                new Submarine()
            };

            GameBoard = new GameBoard();
            FiringBoard = new FiringBoard();
            ShipLocations = new List<ShipPlacements>();

            RoundNumber = 0; // new
        }

        public string[,] OutputGameBoard()
        {
            int boardWidth = (int) BoardDimensions.Width;
            int boardHeight = (int) BoardDimensions.Height;

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
            int boardWidth = (int) BoardDimensions.Width;
            int boardHeight = (int) BoardDimensions.Height;

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

        /// <summary>
        /// Places ship at position given by startRow and startCol. Returns true if placement is successful
        /// </summary>
        public bool PlaceShip(ShipType shipType, ShipOrientation shipOrientation, int startRow, int startCol)
        {
            Ship selectedShip = Ships[(int)shipType];

            if (selectedShip.isPlaced) 
            {
                return true;
            }

            (int Row, int Col) shipStart = (startRow, startCol);
            (int Row, int Col) shipEnd = CalculateShipEndPoint(selectedShip, shipOrientation, shipStart.Row, shipStart.Col);

            var affectedPanels = GameBoard.Panels.Range(shipStart.Row, shipStart.Col, shipEnd.Row, shipEnd.Col);

            if (!IsShipWithinBounds(shipEnd.Row, shipEnd.Col) || !IsBoardRangeUnoccupied(affectedPanels))
            {
                return false;
            }

            // update board
            Coordinates[] shipCoords = new Coordinates[affectedPanels.Count];
            for (int i = 0; i < affectedPanels.Count; i++)
            {
                affectedPanels[i].OccupationType = selectedShip.OccupationType;
                shipCoords[i] = new Coordinates(affectedPanels[i].Coordinates.Row, affectedPanels[i].Coordinates.Column);
            }

            ShipPlacements shipLocation = new ShipPlacements();
            shipLocation.Type = shipType;
            shipLocation.UpdateCoordinates(shipCoords);
            ShipLocations.Add(shipLocation);

            selectedShip.isPlaced = true; // check this if there are problems
            return true; 
        }

        public (int, int) CalculateShipEndPoint(Ship ship, ShipOrientation shipOrientation ,int startRow, int startCol)
        {
            (int Row, int Col) shipEnd = (startRow, startCol);

            if (shipOrientation == ShipOrientation.Horizontal)
            {
                for (int i = 1; i < ship.Width; i++)
                {
                    shipEnd.Col++;
                }
            }
            else
            {
                for (int i = 1; i < ship.Width; i++)
                {
                    shipEnd.Row++;
                }
            }

            return shipEnd;
        }

        public bool IsShipWithinBounds(int endRow, int endCol)
        {
            if (endRow > (int) BoardDimensions.Height || endCol > (int) BoardDimensions.Width)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsBoardRangeUnoccupied(List<GameBoardPanel> selectedPanels)
        {
            if (selectedPanels.Any(x => x.IsOccupied))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public void PlaceShips()
        {
            //Random class creation stolen from http://stackoverflow.com/a/18267477/106356
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            foreach (var ship in Ships)
            {
                //Select a random row/column combination, then select a random orientation.
                //If none of the proposed panels are occupied, place the ship
                //Do this for all ships

                bool isOpen = true;
                while (isOpen)
                {
                    var startcolumn = rand.Next(1, 11);
                    var startrow = rand.Next(1, 11);
                    int endrow = startrow, endcolumn = startcolumn;
                    var orientation = rand.Next(1, 101) % 2; //0 for Horizontal

                    List<int> panelNumbers = new List<int>();
                    if (orientation == (int) ShipOrientation.Horizontal)
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

                    //We cannot place ships beyond the boundaries of the board
                    if (endrow > 10 || endcolumn > 10)
                    {
                        isOpen = true;
                        continue;
                    }

                    //Check if specified panels are occupied
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
                }
            }
        }

        public Coordinates FireShot()
        {
            // new
            RoundNumber++;
            //

            //If there are hits on the board with neighbors which don't have shots, we should fire at those first.
            var hitNeighbors = FiringBoard.GetHitNeighbors();
            Coordinates coords;
            if (hitNeighbors.Any())
            {
                coords = SearchingShot();
            }
            else
            {
                coords = RandomShot();
            }
            // Console.WriteLine(Name + " says: \"Firing shot at " + coords.Row.ToString() + ", " + coords.Column.ToString() + "\"");
            //FiredShot = String.Format("Round " + RoundNumber + " - " + Name + " says: \"Firing shot at " + coords.Row.ToString() + ", " + coords.Column.ToString() + "\"");
            FiredShot = String.Format(Name + " says: \"Firing shot at " + coords.Row.ToString() + ", " + coords.Column.ToString() + "\"");

            return coords;
        }

        private Coordinates RandomShot()
        {
            var availablePanels = FiringBoard.GetOpenRandomPanels();
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            var panelID = rand.Next(availablePanels.Count);
            return availablePanels[panelID];
        }

        private Coordinates SearchingShot()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            var hitNeighbors = FiringBoard.GetHitNeighbors();
            var neighborID = rand.Next(hitNeighbors.Count);
            return hitNeighbors[neighborID];
        }

        public ShotResult ProcessShot(Coordinates coords)
        {
            var panel = GameBoard.Panels.At(coords.Row, coords.Column);
            ShipStatus = String.Empty;
            
            if (!panel.IsOccupied)
            {
                ReceivedShot = String.Format(Name + " says: \"Miss!\""); //new
                GameBoard.Panels.At(coords.Row, coords.Column).OccupationType = OccupationType.Miss; // new -- update playerboard

                return ShotResult.Miss;
            }
            var ship = Ships.First(x => x.OccupationType == panel.OccupationType);
            ship.Hits++;

            ReceivedShot = String.Format(Name + " says: \"Hit!\"");
            //new -- update playerboard
            GameBoard.Panels.At(coords.Row, coords.Column).OccupationType = OccupationType.Hit;

            if (ship.IsSunk)
            {
                ShipStatus = String.Format(Name + " says: \"You sunk my " + ship.Name + "!\""); //new
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
                    break;

                default:
                    panel.OccupationType = OccupationType.Miss;
                    break;
            }
        }

        #region Defunct -- to be removed
        public void OutputBoards()
        {
            Console.WriteLine(Name);
            Console.WriteLine("Own Board:                          Firing Board:");
            for (int row = 1; row <= 10; row++)
            {
                for (int ownColumn = 1; ownColumn <= 10; ownColumn++)
                {
                    Console.Write(GameBoard.Panels.At(row, ownColumn).Status + " ");
                }
                Console.Write("                ");
                for (int firingColumn = 1; firingColumn <= 10; firingColumn++)
                {
                    Console.Write(FiringBoard.Panels.At(row, firingColumn).Status + " ");
                }
                Console.WriteLine(Environment.NewLine);
            }
            Console.WriteLine(Environment.NewLine);
        }
        #endregion
    }
}
