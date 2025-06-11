namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private void PlayRound() // figure out how to end game right after computer wins
        {
            if (gamePlayer1.IsShotAvailable() && gameStatus != GameStatus.GameOver)
            {
                if (gamePlayer1.HasLost)
                {
                    SetGameStatus(GameStatus.GameOver);
                    victoriousPlayer = gamePlayer2;
                    return;
                }
                FireShotHumanPlayer();

                if (gamePlayer2.HasLost)
                {
                    SetGameStatus(GameStatus.GameOver);
                    victoriousPlayer = gamePlayer1;
                    return;
                }
                FireShotComputerPlayer();
            }
        }

        private void FireShotHumanPlayer()
        {
            var coordinates = gamePlayer1.FireManualShot();
            var result = gamePlayer2.ProcessShot(coordinates);
            gamePlayer1.ProcessShotResult(coordinates, result);
        }

        private void FireShotComputerPlayer()
        {
           var coordinates = gamePlayer2.FireAutoShot();
           var result = gamePlayer1.ProcessShot(coordinates);
           gamePlayer2.ProcessShotResult(coordinates, result);
        }
    }
}
