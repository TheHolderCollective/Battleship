using BattleshipGame.Objects.Players;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private void PlayRound() 
        {
            if (gamePlayer1.IsShotAvailable() && gameStatus != GameStatus.GameOver)
            {
                FireShotAtOpponent(gamePlayer1, gamePlayer2);

                if (gamePlayer2.HasLost)
                {
                    SetGameStatus(GameStatus.GameOver);
                    victoriousPlayer = gamePlayer1;
                    return;
                }

                FireShotAtOpponent(gamePlayer2, gamePlayer1);

                if (gamePlayer1.HasLost)
                {
                    SetGameStatus(GameStatus.GameOver);
                    victoriousPlayer = gamePlayer2;
                    return;
                }

            }
        }
        private void FireShotAtOpponent(Player firingPlayer, Player opponent)
        {
            var coordinates = firingPlayer.FireShot();
            var result = opponent.ProcessShot(coordinates);
            firingPlayer.ProcessShotResult(coordinates, result);
        }
    }
}
