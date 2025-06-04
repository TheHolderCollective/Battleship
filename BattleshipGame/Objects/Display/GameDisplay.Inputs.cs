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
                    break;
                case DisplayMode.ShipPlacement:
                    switch (shipPlacementMode)
                    {
                        case ShipPlacementMode.SelectShip:
                            break;
                        case ShipPlacementMode.PositionShip:
                            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
                            gamePlayer1.MoveShip(selectedShip, ShipDirection.Left);
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
                    break;
                case DisplayMode.ShipPlacement:
                    switch (shipPlacementMode)
                    {
                        case ShipPlacementMode.SelectShip:
                            break;
                        case ShipPlacementMode.PositionShip:
                            ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
                            gamePlayer1.MoveShip(selectedShip, ShipDirection.Right);
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

        private void ProcessInputSpacebar(DisplayMode displayMode)
        {
            switch (displayMode)
            {
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
