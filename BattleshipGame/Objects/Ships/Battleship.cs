namespace BattleshipGame.Objects.Ships
{
    public class Battleship : Ship
    {
        public Battleship()
        {
            Name = "Battleship";
            Width = 4;
            OccupationType = OccupationType.Battleship;
            IsPlaced = false;
            ShipType = ShipType.Battleship;
        }
    }
}
