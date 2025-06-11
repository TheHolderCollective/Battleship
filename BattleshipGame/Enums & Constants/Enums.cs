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
        Miss,

        [Description("+")]
        Crosshair
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
        GamePlay,
        MainMenu,
        ShipPlacement,
    }

    public enum MainMenuItems
    {
        [Description("New Game")]
        NewGame,

        [Description("Resume Game")]
        ResumeGame,

        [Description("Exit Game")]
        ExitGame
    }
    public enum ShipType
    {
        [Description("Carrier")]
        Carrier,

        [Description("Battleship")]
        Battleship,

        [Description("Cruiser")]
        Cruiser,

        [Description("Destroyer")]
        Destroyer,

        [Description("Submarine")]
        Submarine,

        [Description("Unknown")]
        Unknown
    }

    public enum ShipOrientation
    {
        Horizontal,
        Vertical
    }

    public enum ShipPlacementMode
    {
        SelectShip,
        PositionShip
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum BoardDimensions
    {
        Width = 10,
        Height = 10
    }

    public enum GameStatus
    {
        GameNotStarted,
        GameInProgress,
        GameOver,
    }
}
