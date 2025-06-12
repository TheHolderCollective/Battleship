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
        public List<ShipPlacements> ShipLocations { get; set; }
        public List<string> ShipPlacementLogs { get; }
        public string FiredShot { get; set; }
        public string ReceivedShot { get; set; }
        public string ShipStatus { get; set; }
        public int RoundNumber { get; set; }
        public int CrosshairsX 
        {
            get
            {
                return CrosshairsPosition.Row;
            }
        }
        public int CrosshairsY
        {
            get
            {
                return CrosshairsPosition.Column;
            }
        }
        private bool ShipsAlreadyPlaced 
        {
            get 
            {
                return ShipLocations.Count > 0;
            }
        }
        private Coordinates CrosshairsPosition { get; set;}
        private OccupationType PriorOccupationType { get; set; }
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

            // initial position of target crosshairs
            CrosshairsPosition = new Coordinates(5, 5); 
            PriorOccupationType = FiringBoard.Panels.At(5, 5).OccupationType;
            PlaceInitialCrossHairs();

            RoundNumber = 0; 
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

        private void PlaceInitialCrossHairs()
        {
            PlaceCrosshairs(CrosshairsPosition.Row, CrosshairsPosition.Column);
        }

        private void PlaceCrosshairs(int row, int column)
        {
            // check boundaries
            if (row < 1 || column < 1 || row > 10 || column > 10)
            {
                return; 
            }

            // restore panel
            var oldPanel = FiringBoard.Panels.Range(CrosshairsPosition.Row, CrosshairsPosition.Column, 
                                                    CrosshairsPosition.Row, CrosshairsPosition.Column);
            oldPanel[0].OccupationType = PriorOccupationType;

            // save occupation type for later
            var newPanel = FiringBoard.Panels.Range(row, column, row, column);
            PriorOccupationType = newPanel[0].OccupationType;

            // update new panel
            newPanel[0].OccupationType = OccupationType.Crosshair;
            CrosshairsPosition = new Coordinates(row, column);
        }

        public void MoveCrosshairs(Direction direction)
        {
            int row = CrosshairsPosition.Row;
            int col = CrosshairsPosition.Column;

            switch (direction)
            {
                case Direction.Up:
                    row--;
                    break;
                case Direction.Down:
                    row++;
                    break;
                case Direction.Left:
                    col--;
                    break;
                case Direction.Right:
                    col++;
                    break;
                default:
                    break;
            }

            PlaceCrosshairs(row, col);
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
                    ShipOrientation orientation = (rand.Next(1, 101) % 2) == 0 ? ShipOrientation.Horizontal: ShipOrientation.Vertical;

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

        public bool PlaceShip(ShipType shipType, ShipOrientation shipOrientation, int startRow, int startCol)
        {
            Ship selectedShip = GetShip(shipType);

            if (selectedShip.IsPlaced) 
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

            selectedShip.IsPlaced = true; 
            UpdateShipPlacementLog();
            return true; 
        }

        public bool MoveShip(ShipType shipType, Direction shipDirection)
        {

            if (ShipLocations.Count != 0)
            {
                Ship selectedShip = GetShip(shipType);
                ShipPlacements selectedShipLocation = GetShipLocation(shipType);
                ShipPlacements updatedShipLocation = new ShipPlacements();

                (int Row, int Col) offset = (0, 0);
                
                updatedShipLocation.Orientation = selectedShipLocation.Orientation;
                updatedShipLocation.UpdateCoordinates(selectedShipLocation.ShipCoordinates);

                Coordinates[] newShipLocation = new Coordinates[updatedShipLocation.ShipCoordinates.Length];

                // update offset based on direction
                switch (shipDirection)
                {
                    case Direction.Up:
                        offset.Row = -1;
                        break;
                    case Direction.Down:
                        offset.Row = 1;
                        break;
                    case Direction.Left:
                        offset.Col = -1;
                        break;
                    case Direction.Right:
                        offset.Col = 1;
                        break;
                    default:
                        break;
                }

                // update coordinates
                Coordinates oldShipStart = new Coordinates(0, 0);
                Coordinates oldShipEnd = new Coordinates(0, 0);
                Coordinates newShipStart = new Coordinates(0, 0);
                Coordinates newShipEnd = new Coordinates(0, 0);
              
                for (int i = 0; i < updatedShipLocation.ShipCoordinates.Length; i++)
                {
                    int oldRow = updatedShipLocation.ShipCoordinates[i].Row;
                    int oldCol = updatedShipLocation.ShipCoordinates[i].Column;
                    int newRow = oldRow + offset.Row;
                    int newCol = oldCol + offset.Col;

                    newShipLocation[i] = new Coordinates(newRow, newCol);

                    if (i == 0)
                    {
                        newShipStart = new Coordinates(newRow, newCol);
                        oldShipStart = new Coordinates(oldRow, oldCol);
                    }
                    else if (i == updatedShipLocation.ShipCoordinates.Length - 1)
                    {
                        newShipEnd = new Coordinates(newRow, newCol);
                        oldShipEnd = new Coordinates(oldRow, oldCol);
                    }
                }

                var newAffectedPanels = GameBoard.Panels.Range(newShipStart.Row, newShipStart.Column, newShipEnd.Row, newShipEnd.Column);
                var oldAffectedPanels = GameBoard.Panels.Range(oldShipStart.Row, oldShipStart.Column, oldShipEnd.Row, oldShipEnd.Column);
                var affectedPanels = newAffectedPanels;

                // modify affected panel range based on direction chosen and orientation
                if ((updatedShipLocation.Orientation == ShipOrientation.Horizontal) && (shipDirection == Direction.Left))
                {
                    affectedPanels = GameBoard.Panels.Range(newShipStart.Row, newShipStart.Column, newShipStart.Row, newShipStart.Column);
                }
                else if ((updatedShipLocation.Orientation == ShipOrientation.Horizontal) && (shipDirection == Direction.Right))
                {
                    affectedPanels = GameBoard.Panels.Range(newShipEnd.Row, newShipEnd.Column, newShipEnd.Row, newShipEnd.Column);
                }
                else if ((updatedShipLocation.Orientation == ShipOrientation.Vertical) && (shipDirection == Direction.Up))
                {
                    affectedPanels = GameBoard.Panels.Range(newShipStart.Row, newShipStart.Column, newShipStart.Row, newShipStart.Column);
                }
                else if ((updatedShipLocation.Orientation == ShipOrientation.Vertical) && (shipDirection == Direction.Down))
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
                        if (shipLocation.TypeOfShip == shipType)
                        {
                            shipLocation.UpdateCoordinates(newShipLocation);
                        }
                    }
                    UpdateShipPlacementLog();
                    return true;
                    
                }
            }
           
            return false;
        }

        public bool RotateShip(ShipType shipType)
        { 
            if (ShipLocations.Count != 0)
            {
                Ship selectedShip = GetShip(shipType);
                ShipPlacements selectedShipLocation = GetShipLocation(shipType);
                ShipPlacements updatedShipLocation = new ShipPlacements();

                int panelCountBeforePivot = 0;
                int panelCountAfterPivot = 0;
                int pivotPanelNumber = 0;

                SetPivotAndAdjacentPanelCounts(selectedShip.ShipType, ref pivotPanelNumber, ref panelCountBeforePivot, ref panelCountAfterPivot);

                int pivotRow = selectedShipLocation.ShipCoordinates[pivotPanelNumber].Row;
                int pivotColumn = selectedShipLocation.ShipCoordinates[pivotPanelNumber].Column;


                var updatedShipCoordinates = CalculateRotatedShipCoordinates(selectedShipLocation.Orientation, pivotPanelNumber, selectedShip.Width,
                                                                              pivotRow, pivotColumn, panelCountBeforePivot, panelCountAfterPivot);
                updatedShipLocation.UpdateCoordinates(updatedShipCoordinates);
                updatedShipLocation.Orientation = (selectedShipLocation.Orientation == ShipOrientation.Horizontal) ? ShipOrientation.Vertical : ShipOrientation.Horizontal; 

                // get ship start and end coordinates
                Coordinates oldShipStart = new Coordinates(0, 0);
                Coordinates oldShipEnd = new Coordinates(0, 0);
                Coordinates newShipStart = new Coordinates(0, 0);
                Coordinates newShipEnd = new Coordinates(0, 0);

                for (int i = 0; i < selectedShipLocation.ShipCoordinates.Length; i++)
                {
                    int oldRow = selectedShipLocation.ShipCoordinates[i].Row;
                    int oldCol = selectedShipLocation.ShipCoordinates[i].Column;
                    int newRow = updatedShipLocation.ShipCoordinates[i].Row;
                    int newCol = updatedShipLocation.ShipCoordinates[i].Column;

                    if (i == 0)
                    {
                        oldShipStart = new Coordinates(oldRow, oldCol);
                        newShipStart = new Coordinates(newRow, newCol);
                    }
                    else if (i == selectedShipLocation.ShipCoordinates.Length - 1)
                    {
                        oldShipEnd = new Coordinates(oldRow, oldCol);
                        newShipEnd = new Coordinates(newRow, newCol);
                    }
                }

                var oldAffectedPanels = GameBoard.Panels.Range(oldShipStart.Row, oldShipStart.Column, oldShipEnd.Row, oldShipEnd.Column);
                var newAffectedPanels = GameBoard.Panels.Range(newShipStart.Row, newShipStart.Column, newShipEnd.Row, newShipEnd.Column);

                // exclude pivot panel from test 
                var affectedPanels = new List<GameBoardPanel>();
                foreach (var panel in newAffectedPanels)
                {
                    if (panel.Coordinates.Row == pivotRow && panel.Coordinates.Column == pivotColumn)
                    {
                        continue;
                    }
                    affectedPanels.Add(panel);
                }
                
                if (IsShipWithinBounds(newShipStart.Row, newShipStart.Column, newShipEnd.Row, newShipEnd.Column) &&
                  IsBoardRangeUnoccupied(affectedPanels))
                {
                    // erase old panels
                    for (int i = 0; i < oldAffectedPanels.Count; i++)
                    {
                        oldAffectedPanels[i].OccupationType = OccupationType.Empty;
                    }

                    // write new ones
                    for (int i = 0; i < newAffectedPanels.Count; i++)
                    {
                        newAffectedPanels[i].OccupationType = selectedShip.OccupationType;
                    }

                    // update coordinates
                    selectedShipLocation.UpdateCoordinates(updatedShipCoordinates);
                    selectedShipLocation.Orientation = (selectedShipLocation.Orientation == ShipOrientation.Horizontal) ? ShipOrientation.Vertical : ShipOrientation.Horizontal;
                    UpdateShipPlacementLog();
                    return true;
                }
            }

            return false;
        }

        private void UpdateShipPlacementLog()
        {
            ShipPlacementLogs.Clear();
            foreach (var location in ShipLocations)
            {
                ShipPlacementLogs.Add(location.PlacementLog);
            }
        }

        private Coordinates[] CalculateRotatedShipCoordinates(ShipOrientation shipOrientation, int pivotPanel, int shipLength, int pivotRow, int pivotColumn, int panelCountBeforePivot, int panelCountAfterPivot)
        {
            // updated the coordinates for rotating the ship
            Coordinates[] updatedShipCoordinates = new Coordinates[shipLength];

            if (shipOrientation == ShipOrientation.Horizontal)
            {
                // update columns
                for (int i = 0; i < updatedShipCoordinates.Length; i++)
                {
                    updatedShipCoordinates[i] = new Coordinates(0, pivotColumn);
                }

                // complete pivot panel coordinates
                updatedShipCoordinates[pivotPanel].Row = pivotRow;

                // update panel coordinates before pivot
                for (int i = 0; i < panelCountBeforePivot; i++)
                {
                    updatedShipCoordinates[i].Row = pivotRow - panelCountBeforePivot + i;
                }

                // update panel coordinates after pivot
                for (int i = 0; i < panelCountAfterPivot; i++)
                {
                    updatedShipCoordinates[pivotPanel + i + 1].Row = pivotRow + i + 1;
                }
            }
            else if (shipOrientation == ShipOrientation.Vertical)
            {
                // update rows
                for (int i = 0; i < updatedShipCoordinates.Length; i++)
                {
                    updatedShipCoordinates[i] = new Coordinates(pivotRow, 0);
                }

                // complete pivot panel coordinates
                updatedShipCoordinates[pivotPanel].Column = pivotColumn;


                // update panel coordinates before pivot
                for (int i = 0; i < panelCountBeforePivot; i++)
                {
                    updatedShipCoordinates[i].Column = pivotColumn - panelCountBeforePivot + i;
                }

                // update panel coordinates after pivot
                for (int i = 0; i < panelCountAfterPivot; i++)
                {
                    updatedShipCoordinates[pivotPanel + i + 1].Column = pivotColumn + i + 1;
                }
            }
            return updatedShipCoordinates;
        }
        
        private void SetPivotAndAdjacentPanelCounts(ShipType shipType,ref int pivotPanel, ref int beforePivotPanelCount, ref int afterPivotPanelCount)
        {   
            // pivot panel numbers are one less the actual pivot panel number due to the zero based index 
            switch (shipType)
            {
                case ShipType.Carrier:
                    beforePivotPanelCount = 2;
                    afterPivotPanelCount = 2;
                    pivotPanel = 2; 
                    break;
                case ShipType.Battleship:
                    beforePivotPanelCount = 1;
                    afterPivotPanelCount = 2;
                    pivotPanel = 1;
                    break;
                case ShipType.Cruiser:
                    beforePivotPanelCount = 1;
                    afterPivotPanelCount = 1;
                    pivotPanel = 1;
                    break;
                case ShipType.Destroyer:
                    beforePivotPanelCount = 0;
                    afterPivotPanelCount = 1;
                    pivotPanel = 0;
                    break;
                case ShipType.Submarine:
                    beforePivotPanelCount = 1;
                    afterPivotPanelCount = 1;
                    pivotPanel = 1;
                    break;
                default:
                    break;
            }
        }
        
        private ShipPlacements GetShipLocation(ShipType shipType)
        {
            foreach (var location in ShipLocations)
            {
                if (location.TypeOfShip == shipType)
                {
                    return location;
                }

            }
            return null;
        }
        
        private Ship GetShip(ShipType shipType)
        {
            foreach (var ship in Ships)
            {
                if (ship.ShipType == shipType)
                {
                    return ship;
                }
            }
            return null; 
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

        public bool IsShotAvailable()
        {
            bool targetPanelAvailable = (PriorOccupationType == OccupationType.Empty) ? true : false;

            return targetPanelAvailable;
        }

        public Coordinates FireManualShot()
        {
            RoundNumber++;
            Coordinates coords = new Coordinates(CrosshairsPosition.Row, CrosshairsPosition.Column);
            FiredShot = String.Format(Name + " says: \"Firing shot at " + coords.Row.ToString() + ", " + coords.Column.ToString() + "\"");
            return coords;
        }
        
        public Coordinates FireAutoShot()
        {
            RoundNumber++;
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
    }
}
