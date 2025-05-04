using System.ComponentModel;

namespace BattleshipGame
{
    public enum OccupationType
    {
        [Description("o")]
        Empty,

        [Description("B")]
        Battleship,

        [Description("C")]
        Cruiser,

        [Description("D")]
        Destroyer,

        [Description("S")]
        Submarine,

        [Description("A")]
        Carrier,

        [Description("X")]
        Hit,

        [Description("M")]
        Miss
    }

    public enum ShotResult
    {
        Miss,
        Hit
    }

    public enum LayoutSize
    {
        Title = 10,
        Gameboard = 22,
        GameInfo = 8
    }

    public enum WindowDimensions
    {
        Width = 120,
        Height = 30
    }

    public enum DisplayMode
    {
        MainMenu,
        ShipPlacement,
        GamePlay,
        DataEntry
    }

    public enum BoardDimensions
    {
        Width = 10,
        Height = 10
    }


}
