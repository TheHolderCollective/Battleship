using BattleshipGame.Objects.GameMenu;
using Spectre.Console;
using System;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private Layout gameLayout;
        private LiveDisplay liveDisplay;
        private Player gamePlayer1;
        private Player gamePlayer2;
        private Player victoriousPlayer;

        private GameStatus gameStatus;
        private DisplayMode displayMode;
        private ShipPlacementMode shipPlacementMode;
        private ShipOrientation shipOrientation;
        private Menu mainMenu;
        private Menu shipMenu;

        public GameDisplay()
        {
            SetupPlayers();
            CreateMenus();
            SetGameStatus(GameStatus.NotStarted);
            SetDisplayMode(DisplayMode.MainMenu);
            CreateGameLayouts();
        }
        public void PlayGame()
        {
            ShowDisplay();
        }
        private void ShowDisplay()
        {
            SetupConsole();
            SetupLiveDisplay(gameLayout);
            StartLiveDisplay();
        }
        private void ProcessUpdates(LiveDisplayContext ctx)
        {
            bool continueDisplay = true;

            while (continueDisplay)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey keyPressed = Console.ReadKey(true).Key;
                    if (keyPressed == ConsoleKey.Escape)
                    {
                        continueDisplay = false;
                    }

                    ProcessPlayerInputs(keyPressed);
                    ctx.Refresh();
                }
            }
        }
        private void ProcessPlayerInputs(ConsoleKey keyPressed)
        {
            switch (keyPressed)
            {
                case ConsoleKey.F1:
                    ProcessInputF1Key();
                    break;
                case ConsoleKey.F2:
                    ProcessInputF2Key(displayMode);
                    break;
                case ConsoleKey.F4:
                    ProcessInputF4Key(displayMode);
                    break;
                case ConsoleKey.UpArrow:
                    ProcessInputUpArrowKey(displayMode);
                    break;
                case ConsoleKey.DownArrow:
                    ProcessInputDownArrowKey(displayMode);
                    break;
                case ConsoleKey.RightArrow:
                    ProcessInputRightArrowKey(displayMode);
                    break;
                case ConsoleKey.LeftArrow:
                    ProcessInputLeftArrowKey(displayMode);
                    break;
                case ConsoleKey.Enter:
                    ProcessInputEnterKey(displayMode);
                    break;
                case ConsoleKey.Spacebar:
                    ProcessInputSpacebar(displayMode);
                    break;
                default:
                    break;
            }
        }
        private void ProcessCurrentMainMenuSelection()
        {
            MainMenuItems mainMenuItem = mainMenu.GetSelectedItem();

            switch (mainMenuItem)
            {
                case MainMenuItems.NewGame: // figure out how to reset game for new game option
                    switch (gameStatus)
                    {
                        case GameStatus.NotStarted:
                            SetGameStatus(GameStatus.ShipPlacementInProgress);
                            ActivateShipPlacementMode();
                            break;
                        case GameStatus.Restart:
                            ResetLayoutsForRestart();
                            ActivateShipPlacementMode();
                            break;
                        default:
                            break;
                    }
                    break;
                case MainMenuItems.ResumeGame:
                    switch (gameStatus)
                    {
                        case GameStatus.SuspendedShipPlacement:
                            SetGameStatus(GameStatus.ShipPlacementInProgress);
                            ActivateShipPlacementMode();
                            break;
                        case GameStatus.SuspendedBattle:
                            SetGameStatus(GameStatus.BattleInProgress);
                            ActivateGamePlayMode();
                            break;
                        default:
                            break;
                    }
                    break;
                case MainMenuItems.ExitGame:
                    System.Environment.Exit(0);
                    break;
                default:
                    break;
            }

        }
        private void ProcessShipPlacements(ShipPlacementMode spMode)
        {
            switch (spMode)
            {
                case ShipPlacementMode.SelectShip:
                    ProcessCurrentShipMenuSelection();
                    break;
                case ShipPlacementMode.PositionShip:
                    SetShipPlacementMode(ShipPlacementMode.SelectShip);
                    break;
                default:
                    break;
            }
        }
        private void ProcessCurrentShipMenuSelection()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
            bool shipNotPlaced = true;
            int shipX, shipY;

            while (shipNotPlaced)
            {
                shipX = rand.Next(1, 11);
                shipY = rand.Next(1, 11);
                shipOrientation = (ShipOrientation)rand.Next(0, 2);

                shipNotPlaced = !gamePlayer1.PlaceShip(selectedShip, shipOrientation, shipX, shipY);
            }

            UpdateShipPlacementGameboard();
            UpdateShipPlacementInfo();
            SetShipPlacementMode(ShipPlacementMode.PositionShip);
        }
    }
}
