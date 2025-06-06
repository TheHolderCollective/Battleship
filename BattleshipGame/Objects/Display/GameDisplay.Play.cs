using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private void PlayRound() // TODO update this so that game ends properly
        {
            var coordinates = gamePlayer1.FireManualShot();
            var result = gamePlayer2.ProcessShot(coordinates);
            gamePlayer1.ProcessShotResult(coordinates, result);

            if (!gamePlayer2.HasLost) //If player 2 already lost, we can't let them take another turn.
            {
                coordinates = gamePlayer2.FireAutoShot();
                result = gamePlayer1.ProcessShot(coordinates);
                gamePlayer2.ProcessShotResult(coordinates, result);
            }
        }
    }
}
