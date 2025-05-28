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

            Coordinates shipStart = new Coordinates(startRow, startCol);
            Coordinates shipEnd = CalculateShipEndPoint(selectedShip, shipOrientation,shipStart);

            var affectedPanels = GameBoard.Panels.Range(shipStart.Row, shipStart.Column, shipEnd.Row, shipEnd.Column);

            if (!IsShipWithinBounds(shipStart.Row, shipStart.Column, shipEnd.Row, shipEnd.Column) || !IsBoardRangeUnoccupied(affectedPanels))
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

            ShipPlacements shipLocation = new ShipPlacements(shipType, shipOrientation, shipCoords);
            ShipLocations.Add(shipLocation);

            selectedShip.isPlaced = true; // check this if there are problems
            return true; 
        }

        public bool MoveShip(ShipType shipType, ShipDirection shipDirection)
        {
            Ship selectedShip = Ships[(int)shipType];
            ShipPlacements selectedShipPosition = new ShipPlacements();

            (int Row, int Col) offset = (0, 0);

            if (ShipLocations.Count != 0)
            {
                foreach (var shipLocation in ShipLocations)
                {
                    if (shipLocation.Type == shipType)
                    {
                        selectedShipPosition.Orientation = shipLocation.Orientation;
                    }
                }
                
                foreach (var shipLocation in ShipLocations)
                {
                    if (shipType == shipLocation.Type)
                    {
                        selectedShipPosition.UpdateCoordinates(shipLocation.ShipCoordinates);
                    }
                }

                Coordinates[] newShipLocation = new Coordinates[selectedShipPosition.ShipCoordinates.Length];

                // update offset based on direction
                switch (shipDirection)
                {
                    case ShipDirection.Up:
                        offset.Row = -1;
                        break;
                    case ShipDirection.Down:
                        offset.Row = 1;
                        break;
                    case ShipDirection.Left:
                        offset.Col = -1;
                        break;
                    case ShipDirection.Right:
                        offset.Col = 1;
                        break;
                    default:
                        break;
                }

                // update coordinates
                Coordinates newShipStart = new Coordinates(0, 0);
                Coordinates newShipEnd = new Coordinates(0, 0);
                Coordinates oldShipStart = new Coordinates(0, 0);
                Coordinates oldShipEnd = new Coordinates(0, 0);

                for (int i = 0; i < selectedShipPosition.ShipCoordinates.Length; i++)
                {
                    int oldRow = selectedShipPosition.ShipCoordinates[i].Row;
                    int oldCol = selectedShipPosition.ShipCoordinates[i].Column;
                    int newRow = oldRow + offset.Row;
                    int newCol = oldCol + offset.Col;

                    newShipLocation[i] = new Coordinates(newRow, newCol);

                    if (i == 0)
                    {
                        newShipStart = new Coordinates(newRow, newCol);
                        oldShipStart = new Coordinates(oldRow, oldCol);
                    }
                    else if (i == selectedShipPosition.ShipCoordinates.Length - 1)
                    {
                        newShipEnd = new Coordinates(newRow, newCol);
                        oldShipEnd = new Coordinates(oldRow, oldCol);
                    }

                }

               
                var newAffectedPanels = GameBoard.Panels.Range(newShipStart.Row, newShipStart.Column, newShipEnd.Row, newShipEnd.Column);
                var oldAffectedPanels = GameBoard.Panels.Range(oldShipStart.Row, oldShipStart.Column, oldShipEnd.Row, oldShipEnd.Column);

                var affectedPanels = newAffectedPanels;

                // modify affected panel range based on direction chosen and orientation
                if ((selectedShipPosition.Orientation == ShipOrientation.Horizontal) && (shipDirection == ShipDirection.Left))
                {
                    affectedPanels = GameBoard.Panels.Range(newShipStart.Row, newShipStart.Column, newShipStart.Row, newShipStart.Column);
                }
                else if ((selectedShipPosition.Orientation == ShipOrientation.Horizontal) && (shipDirection == ShipDirection.Right))
                {
                    affectedPanels = GameBoard.Panels.Range(newShipEnd.Row, newShipEnd.Column, newShipEnd.Row, newShipEnd.Column);
                }
                else if ((selectedShipPosition.Orientation == ShipOrientation.Vertical) && (shipDirection == ShipDirection.Up))
                {
                    affectedPanels = GameBoard.Panels.Range(newShipStart.Row, newShipStart.Column, newShipStart.Row, newShipStart.Column);
                }
                else if ((selectedShipPosition.Orientation == ShipOrientation.Vertical) && (shipDirection == ShipDirection.Down))
                {
                    affectedPanels = GameBoard.Panels.Range(newShipEnd.Row, newShipEnd.Column, newShipEnd.Row, newShipEnd.Column);
                }

            
                // check if new range is occupied and update board if it isn't
                if (IsShipWithinBounds(newShipStart.Row, newShipStart.Column, newShipEnd.Row, newShipEnd.Column) && 
                    IsBoardRangeUnoccupied(affectedPanels))
                {

                    for (int i = 0; i < oldAffectedPanels.Count; i++)
                    {
                        oldAffectedPanels[i].OccupationType = OccupationType.Empty;
                    }
                    // done separately because combining the updates causes the newAffectedPanels to be overwritten
                    for (int i = 0; i < newAffectedPanels.Count; i++)
                    { 
                        newAffectedPanels[i].OccupationType = selectedShip.OccupationType;
                    }

                    foreach (var shipLocation in ShipLocations)
                    {
                        if (shipLocation.Type == shipType)
                        {
                            shipLocation.UpdateCoordinates(newShipLocation);
                        }
                    }
                    return true;
                    
                }
               
            }
           
            return false;
            
        }


        private Coordinates CalculateShipEndPoint(Ship ship, ShipOrientation shipOrientation ,Coordinates shipStart)
        {
            Coordinates shipEnd = new Coordinates(shipStart.Row, shipStart.Column);

            if (shipOrientation == ShipOrientation.Horizontal)
            {
                for (int i = 1; i < ship.Width; i++)
                {
                    shipEnd.Column++;
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

        private bool IsShipWithinBounds(int startRow, int startCol, int endRow, int endCol)
        {
            if (startRow < 1 || startCol < 1 || endRow > (int) BoardDimensions.Height || endCol > (int) BoardDimensions.Width)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsBoardRangeUnoccupied(List<GameBoardPanel> selectedPanels)
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
