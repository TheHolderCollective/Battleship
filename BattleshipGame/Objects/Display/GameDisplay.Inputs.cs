using Spectre.Console;
using System;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
		#region Enter Key Processing
		private void ProcessInputEnterKey(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.GamePlay:
                    break;
                case DisplayMode.MainMenu:
                    ProcessCurrentMainMenuSelection();
					break;
                case DisplayMode.ShipPlacement:
                    ProcessShipPlacements(shipPlacementMode);
					break;
                default:
                    break;
            }
        }

        private void ProcessCurrentMainMenuSelection()
        {
			if (mainMenu.SelectedItemName == "Play Game") // change this to use enums
			{
				ActivateShipPlacementMode();
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
                    shipPlacementMode = ShipPlacementMode.SelectShip;
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

            while (shipNotPlaced)
            {
                shipX = rand.Next(1, 11);
                shipY = rand.Next(1, 11);
                shipOrientation = (ShipOrientation)rand.Next(0, 2);

                shipNotPlaced = !gamePlayer1.PlaceShip(selectedShip, shipOrientation, shipX, shipY);
            }

            UpdateShipPlacementGameboard();
            UpdateShipPlacementInfo();
            shipPlacementMode = ShipPlacementMode.PositionShip;

        }

		#endregion

		#region Spacebar Processing
        private void ProcessInputSpacebar(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.GamePlay:
                    PlayRound();
                    UpdateGameboard();
                    UpdateFiringBoard();
                    UpdateBattleResults();
                    break;
                case DisplayMode.ShipPlacement:
                    switch (shipPlacementMode)
                    {
                        case ShipPlacementMode.SelectShip:
                            break;
                        case ShipPlacementMode.PositionShip:
                            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
                            gamePlayer1.RotateShip(selectedShip);
                            UpdateShipPlacementGameboard();
                            UpdateShipPlacementInfo();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Function Key Processing
        private void ProcessInputF2Key(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.ShipPlacement:
                    if (gamePlayer1.ShipLocations.Count == GameConstants.ShipTotal)
                    {
                        ActivateGamePlayMode();
                        UpdateGameboard();
                    }
                    break;
                default:
                    break;
            }
        }
        private void ProcessInputF4Key(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.ShipPlacement:
                    gamePlayer1.PlaceShipsRandomly();
                    UpdateShipPlacementGameboard();
                    UpdateShipPlacementInfo();
                    break;
                default:
                    break;
            }

        }
        #endregion

        #region Arrow Keys Processing
        private void ProcessInputUpArrowKey(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.GamePlay:
                    gamePlayer1.MoveCrosshairs(Direction.Up);
                    UpdateFiringBoard();
                    UpdateTargetInfo();
                    break;
                case DisplayMode.MainMenu:
                    MainMenuSelectPrevious();
                    break;
                case DisplayMode.ShipPlacement:
                    switch (shipPlacementMode)
                    {
                        case ShipPlacementMode.SelectShip:
                            ShipMenuSelectPrevious();
                            break;
                        case ShipPlacementMode.PositionShip:
                            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
                            gamePlayer1.MoveShip(selectedShip, Direction.Up);
                            UpdateShipPlacementGameboard();
                            UpdateShipPlacementInfo();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        private void ProcessInputDownArrowKey(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.GamePlay:
                    gamePlayer1.MoveCrosshairs(Direction.Down);
                    UpdateFiringBoard();
                    UpdateTargetInfo();
                    break;
                case DisplayMode.MainMenu:
                    MainMenuSelectNext();
                    break;
                case DisplayMode.ShipPlacement:
                    switch (shipPlacementMode)
                    {
                        case ShipPlacementMode.SelectShip:
                            ShipMenuSelectNext();
                            break;
                        case ShipPlacementMode.PositionShip:
                            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
                            gamePlayer1.MoveShip(selectedShip, Direction.Down);
                            UpdateShipPlacementGameboard();
                            UpdateShipPlacementInfo();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        private void ProcessInputLeftArrowKey(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.GamePlay:
                    gamePlayer1.MoveCrosshairs(Direction.Left);
                    UpdateFiringBoard();
                    UpdateTargetInfo();
                    break;
                case DisplayMode.ShipPlacement:
                    switch (shipPlacementMode)
                    {
                        case ShipPlacementMode.SelectShip:
                            break;
                        case ShipPlacementMode.PositionShip:
                            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
                            gamePlayer1.MoveShip(selectedShip, Direction.Left);
                            UpdateShipPlacementGameboard();
                            UpdateShipPlacementInfo();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        private void ProcessInputRightArrowKey(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.GamePlay:
                    gamePlayer1.MoveCrosshairs(Direction.Right);
                    UpdateFiringBoard();
                    UpdateTargetInfo();
                    break;
                case DisplayMode.ShipPlacement:
                    switch (shipPlacementMode)
                    {
                        case ShipPlacementMode.SelectShip:
                            break;
                        case ShipPlacementMode.PositionShip:
                            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
                            gamePlayer1.MoveShip(selectedShip, Direction.Right);
                            UpdateShipPlacementGameboard();
                            UpdateShipPlacementInfo();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void ProcessInputBackspaceKey(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.ShipPlacement:
                    switch (shipPlacementMode)
                    {
                        case ShipPlacementMode.SelectShip:
                            break;
                        case ShipPlacementMode.PositionShip:
                            shipPlacementMode = ShipPlacementMode.SelectShip;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
