using Spectre.Console;
using System.Collections.Generic;

namespace BattleshipGame.Objects.Boards
{
    /// <summary>
    /// Represents a collection of Panels to provide a Player with their Game Board (e.g. where their ships are placed).
    /// </summary>
    public class GameBoard
    {
        public List<GameBoardPanel> Panels { get; set; }

        public GameBoard()
        {
            Panels = new List<GameBoardPanel>();
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    Panels.Add(new GameBoardPanel(i, j));
                }
            }
        }
    }
}
