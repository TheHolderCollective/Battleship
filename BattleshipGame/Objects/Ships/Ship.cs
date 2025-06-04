namespace BattleshipGame.Objects
{
    /// <summary>
    /// Represents a player's ship as placed on their Game Board.
    /// </summary>
    public abstract class Ship
    {
        public string Name { get; set; }
        public ShipType ShipType { get; set;} // new
        public int Width { get; set; }
        public int Hits { get; set; }
        public OccupationType OccupationType { get; set; }
        public bool IsSunk
        {
            get
            {
                return Hits >= Width;
            }
        }
        public bool IsPlaced { get; set; } // new
    }
}
