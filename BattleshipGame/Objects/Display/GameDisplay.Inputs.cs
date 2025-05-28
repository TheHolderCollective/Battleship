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
                    ProcessCurrentShipMenuSelection();
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

        private void ProcessCurrentShipMenuSelection()
        {
            shipPlacementMode = ShipPlacementMode.PositionShip;

            // the code below needs to be changed to initially position the selected ship on the board
            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);

            // temporary just for testing
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            shipX = rand.Next(1, 11);
            shipY = rand.Next(1, 11);
            shipOrientation =(ShipOrientation) rand.Next(0, 2);
            //

            if (gamePlayer1.PlaceShip(selectedShip, shipOrientation, shipX, shipY))
            {
                //ActivateShipPlacementMode();
                UpdatePlayerBoardForShipPlacement();
                shipPlacementMode = ShipPlacementMode.PositionShip;
            }

		}

		#endregion

		#region Space Key Processing

        private void ProcessInputSpaceKey(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.Demo:
                    break;
                case DisplayMode.GamePlay:
                    break;
                case DisplayMode.ShipPlacement:
                    RotateShip();
                    break;
                default:
                    break;
            }
        }


        private void RotateShip()
        {
            // check if ship can be placed in position
            bool shipCanBeRotated = true; // add code to really test if this is true

            if (shipCanBeRotated)
            {
                switch (shipOrientation)
                {
                    case ShipOrientation.Horizontal:
                        shipOrientation = ShipOrientation.Vertical;
                        break;
                    case ShipOrientation.Vertical:
						shipOrientation = ShipOrientation.Horizontal;
						break;
                    default:
                        break;
                }
            }
        }

        #endregion
        
        #region Arrow Keys Processing
        private void ProcessInputUpArrowKey(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.GamePlay:
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
                            gamePlayer1.MoveShip(selectedShip, ShipDirection.Up);
                            UpdatePlayerBoardForShipPlacement();
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
                            gamePlayer1.MoveShip(selectedShip, ShipDirection.Down);
                            UpdatePlayerBoardForShipPlacement();
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
                    break;
                case DisplayMode.ShipPlacement:
                    switch (shipPlacementMode)
                    {
                        case ShipPlacementMode.SelectShip:
                            break;
                        case ShipPlacementMode.PositionShip:
                            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
                            gamePlayer1.MoveShip(selectedShip, ShipDirection.Left);
                            UpdatePlayerBoardForShipPlacement();
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
                    break;
                case DisplayMode.ShipPlacement:
                    switch (shipPlacementMode)
                    {
                        case ShipPlacementMode.SelectShip:
                            break;
                        case ShipPlacementMode.PositionShip:
                            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
                            gamePlayer1.MoveShip(selectedShip, ShipDirection.Right);
                            UpdatePlayerBoardForShipPlacement();
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

        private void ProcessBackspaceKey(DisplayMode displayMode)
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
