using BattleshipGame.Objects.Boards;
using System.Collections.Generic;
using System.Linq;


namespace BattleshipGame.Extensions
{
    public static class PanelExtensions
    {
        public static GameBoardPanel At(this List<GameBoardPanel> panels, int row, int column)
        {
            return panels.Where(x => x.Coordinates.Row == row && x.Coordinates.Column == column).First();
        }

        public static List<GameBoardPanel> Range(this List<GameBoardPanel> panels, int startRow, int startColumn, int endRow, int endColumn)
        {
            return panels.Where(x => x.Coordinates.Row >= startRow
                                     && x.Coordinates.Column >= startColumn
                                     && x.Coordinates.Row <= endRow
                                     && x.Coordinates.Column <= endColumn).ToList();
        }
    }
}
