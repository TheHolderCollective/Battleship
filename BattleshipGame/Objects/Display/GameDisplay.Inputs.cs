﻿using System;
using System.ComponentModel;
using BattleshipGame.Extensions;

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
                    if (gameStatus == GameStatus.GameOver)
                    {
                        SetGameStatus(GameStatus.Restart);
                        ActivateMainMenuMode();
                    }
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
        #endregion

        #region Spacebar Processing
        private void ProcessInputSpacebar(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.GamePlay:
                    UpdateGamePlayBoards();
                    break;
                case DisplayMode.ShipPlacement:
                    if (shipPlacementMode == ShipPlacementMode.PositionShip)
                    {
                        ShipType selectedShip = GetShipType(shipMenu.SelectedItemName);
                        gamePlayer1.RotateShip(selectedShip);
                        UpdateShipPlacementGameboard();
                        UpdateShipPlacementInfo();
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Function Key Processing
        private void ProcessInputF1Key()
        {
            switch (gameStatus)
            {
                case GameStatus.ShipPlacementInProgress:
                    SetGameStatus(GameStatus.SuspendedShipPlacement);
                    break;
                case GameStatus.BattleInProgress:
                    SetGameStatus(GameStatus.SuspendedBattle);
                    break;
                case GameStatus.GameOver:
                    SetGameStatus(GameStatus.Restart);
                    break;
                default:
                    break;
            }
            ActivateMainMenuMode();
        }
        private void ProcessInputF2Key(DisplayMode displayMode)
        {
            int shipTotal = Int32.Parse(Properties.Resources.ShipsTotal);

            if (displayMode == DisplayMode.ShipPlacement && (gamePlayer1.ShipLocations.Count == shipTotal))
            {
                ActivateGamePlayMode();
                UpdateGameboard();
            }
        }
        private void ProcessInputF4Key(DisplayMode displayMode)
        {
            if (displayMode == DisplayMode.ShipPlacement)
            {
                gamePlayer1.PlaceShipsRandomly();
                UpdateShipPlacementGameboard();
                UpdateShipPlacementInfo();
            }
        }
        #endregion

        #region Arrow Keys Processing
        private void ProcessInputUpArrowKey(DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case DisplayMode.GamePlay:
                    if (gameStatus != GameStatus.GameOver)
                    {
                        UpdateCrosshairsPosition(Direction.Up);
                    }
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
                            UpdateShipPosition(Direction.Up);
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
                    if (gameStatus != GameStatus.GameOver)
                    {
                        UpdateCrosshairsPosition(Direction.Down);
                    }
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
                            UpdateShipPosition(Direction.Down);
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
                    if (gameStatus != GameStatus.GameOver)
                    {
                        UpdateCrosshairsPosition(Direction.Left);
                    }
                    break;
                case DisplayMode.ShipPlacement:
                    if (shipPlacementMode == ShipPlacementMode.PositionShip)
                    {
                        UpdateShipPosition(Direction.Left);
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
                    if (gameStatus != GameStatus.GameOver)
                    {
                        UpdateCrosshairsPosition(Direction.Right);
                    }
                    break;
                case DisplayMode.ShipPlacement:
                    if (shipPlacementMode == ShipPlacementMode.PositionShip)
                    {
                        UpdateShipPosition(Direction.Right);
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

    }
}
