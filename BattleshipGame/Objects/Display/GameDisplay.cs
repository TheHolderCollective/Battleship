using BattleshipGame.Extensions;
using BattleshipGame.Objects.GameMenu;
using Spectre.Console;
using System;
using System.ComponentModel;

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

        public GameDisplay(Player player1, Player player2)
        {
            // Note - players and menus have to be setup before layouts can be created
            SetupPlayers(player1, player2);
            CreateMenus();
            CreateGameLayouts();
            SetDisplayMode(DisplayMode.MainMenu);
            SetGameStatus(GameStatus.GameInProgress);
        }
        public void ShowDisplay()
        {
            SetupConsole();
            SetupLiveDisplay(gameLayout);
            StartLiveDisplay();
        }
        private void ProcessUpdates(LiveDisplayContext ctx)
        {
            // test code to be replaced
            bool continuePlaying = true;

            while (continuePlaying)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey keyPressed = Console.ReadKey(true).Key;

                    if (keyPressed == ConsoleKey.Escape)
                    {
                       continuePlaying = false;
                       continue;
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
                    ActivateMainMenuMode();
                    break;
                case ConsoleKey.F2:
                    ProcessInputF2Key(displayMode);
                    break;
                case ConsoleKey.F3:
                    ActivateShipPlacementMode();
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
            
            // update this code to take into account all options
            switch (mainMenuItem)
            {
                case MainMenuItems.NewGame: // figure out how to reset game for new game option
                    ActivateShipPlacementMode();
                    break;
                case MainMenuItems.ResumeGame:
                    ActivateGamePlayMode();
                    break;
                case MainMenuItems.ExitGame:
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
