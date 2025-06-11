using System;
using Spectre.Console;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private Layout CreateLayouts()
        {
            var titleLayout = CreateDefaultTitleLayout();
            var startScreenLayout = CreateStartScreenLayout();
            var boardLayout = CreateGameBoardLayout();
            var shipPlacementLayout = CreateShipPlacementLayout();

            var gameLayout = new Layout().SplitRows(titleLayout, boardLayout, startScreenLayout, shipPlacementLayout);
            return gameLayout;
        }
        private void DeactivateAllLayouts()
        {
            gameLayout["StartScreen"].Invisible();
            gameLayout["GameBoard"].Invisible();
            gameLayout["ShipPlacementBoard"].Invisible();
        }

        #region Game Title
        private Layout CreateDefaultTitleLayout()
        {
            return CreateTitleLayout("[purple]");
        }
        private Layout CreateTitleLayout(string titleColor)
        {
            var titleLayout = new Layout("Title");

            var titleText = new Markup($"\n\n{titleColor}{GameConstants.GameTitle}[/]").Centered();
            var titlePanel = new Panel(titleText).Expand().Border(BoxBorder.Double);

            titleLayout.Size((int)LayoutSize.Title);
            titleLayout.Update(titlePanel);

            return titleLayout;
        }

        #endregion

        #region Main Layouts
        private Layout CreateStartScreenLayout()
        {

            var mainMenuLayout = CreateMainMenuLayout();
            var menuLeftBorderLayout = CreateMenuBorderLayout("MenuLeftBorder");
            var menuRightBorderLayout = CreateMenuBorderLayout("MenuRightBorder");
            var footerLayout = CreateFooterLayout();

            var startScreenLayout = new Layout("StartScreen").SplitRows(new Layout("Menu").SplitColumns(menuLeftBorderLayout, mainMenuLayout, menuRightBorderLayout), footerLayout);
            return startScreenLayout;
        }
        private Layout CreateGameBoardLayout()
        {
            var playerBoardLayout = CreatePlayerBoardLayout(gamePlayer1, "PlayerGameBoard");
            var firingBoardLayout = CreateFiringBoardLayout(gamePlayer1, "OpponentFiringBoard");
            var statusBoardLayout = CreateStatusBoardsLayout();
            var gameInfoLayout = CreateGameInfoLayout();

            var playerBoardsLayout = new Layout("PlayerBoards").SplitColumns(playerBoardLayout, firingBoardLayout, statusBoardLayout);

            playerBoardsLayout.Size((int)LayoutSize.Gameboard);

            var boardLayout = new Layout("GameBoard").SplitRows(playerBoardsLayout, gameInfoLayout);
            return boardLayout;
        }
        private Layout CreateShipPlacementLayout()
        {
            var playerBoardLayout = CreatePlayerBoardLayout(gamePlayer1, "ShipPlacementGameBoard");
            var shipSelectionMenuLayout = CreateShipSelectionMenuLayout();
            var shipPlacementInfoLayout = CreateShipPlacementInfoLayout();

            var shipPlacementLayout = new Layout("ShipPlacementBoard").SplitRows(new Layout("ShipPlacementSubBoard").SplitColumns(playerBoardLayout, shipSelectionMenuLayout), shipPlacementInfoLayout);

            return shipPlacementLayout;
        }

        #endregion

        #region Gameplay 
        private Layout CreatePlayerBoardLayout(Player player, string layoutName)
        {

            var playerBoardLayout = new Layout(layoutName).MinimumSize(60);

            var playerBoard1 = String.Format(Environment.NewLine + MakeGameBoard(player));
            var playerBoardText = new Markup(playerBoard1).Centered();
            var playerBoardPanel = new Panel(playerBoardText).Expand().Header("Player Board").HeaderAlignment(Justify.Center);
            //var playerBoardPanel = new Panel(playerBoardText).Expand().Header(player.Name + "'s Board").HeaderAlignment(Justify.Center);

            playerBoardLayout.Update(playerBoardPanel);

            return playerBoardLayout;
        }
        private Layout CreateFiringBoardLayout(Player player, string layoutName)
        {
            var firingBoardLayout = new Layout(layoutName).MinimumSize(60);

            var playerBoard2 = String.Format(Environment.NewLine + MakeFiringBoard(player));
            var firingBoardText = new Markup(playerBoard2).Centered();
            var firingBoardPanel = new Panel(firingBoardText).Expand().Header("Firing Board").HeaderAlignment(Justify.Center);
            //var firingBoardPanel = new Panel(firingBoardText).Expand().Header(opponentsName + "'s Board").HeaderAlignment(Justify.Center);

            firingBoardLayout.Update(firingBoardPanel);

            return firingBoardLayout;
        }
        private Layout CreateStatusBoardsLayout()
        {
            var playerStatusLayout = CreatePlayerStatusLayout("PlayerStatus", "Status (" + gamePlayer1.Name + ")", gamePlayer1);
            var opponentStatusLayout = CreatePlayerStatusLayout("OpponentStatus", "Status (" + gamePlayer2.Name + ")", gamePlayer2);
            var statusBoardLayout = new Layout("StatusBoard").SplitRows(playerStatusLayout, opponentStatusLayout);

            return statusBoardLayout;
        }
        private Layout CreatePlayerStatusLayout(string layoutName, string statusHeader, Player player)
        {
            var playerStatusLayout = new Layout(layoutName);
            var playerStatusPanel = CreatePlayerStatusPanel(statusHeader, player);
            playerStatusLayout.Update(playerStatusPanel);

            return playerStatusLayout;
        }
        private Panel CreatePlayerStatusPanel(string statusHeader, Player player)
        {
            var playerStatusText = GetShipStatusLists(player.Ships);
            var playerStatusMarkup = new Markup(playerStatusText).LeftJustified();
            var playerStatusPanel = new Panel(playerStatusMarkup).Expand().Header(statusHeader).HeaderAlignment(Justify.Center);

            return playerStatusPanel;
        }
        private Layout CreateGameInfoLayout()
        {
            var resultsLayout = CreateResultsLayout();
            var targetInfoLayout = CreateTargetInfoLayout();
            var tipsLayout = CreateTipsLayout();


            var gameInfoLayout = new Layout("GameInfo").SplitColumns(resultsLayout, targetInfoLayout, tipsLayout);
            gameInfoLayout.Size((int)LayoutSize.GameInfo);

            return gameInfoLayout;

        }
        private Layout CreateTargetInfoLayout()
        {
            // TODO Determine how target info will be fed into panel
            var targetInfoLayout = new Layout("TargetInfo");
            var targetText = $"Targeting: ({gamePlayer1.CrosshairsX} , {gamePlayer1.CrosshairsY})";
            var targetMarkup = new Markup(targetText.ToString()).LeftJustified();
            var targetPanel = new Panel(targetMarkup).Expand().Header("Targeting Dashboard").HeaderAlignment(Justify.Center);

            targetInfoLayout.MinimumSize(60);
            targetInfoLayout.Update(targetPanel);

            return targetInfoLayout;

        }
        private Layout CreateTipsLayout()
        {
            // TODO Find a way to pull in keyboard tips
            var tipsLayout = new Layout("Tips");
            var tipsText = "Keystroke tips go here.";
            var tipsMarkup = new Markup(tipsText.ToString()).LeftJustified();
            var tipsPanel = new Panel(tipsMarkup).Expand().Header("Keyboard Tips").HeaderAlignment(Justify.Center);

            tipsLayout.Update(tipsPanel);

            return tipsLayout;
        }
        private Layout CreateResultsLayout()
        
        {
            var resultsLayout = new Layout("Results");
            var resultsPanel = CreateResultsPanelForBattleUpdates(gamePlayer1, gamePlayer2);

            resultsLayout.MinimumSize(60);
            resultsLayout.Update(resultsPanel);

            return resultsLayout;
        }
        private Panel CreateResultsPanelForBattleUpdates(Player player1, Player player2)
        {
            var resultsText = GetRoundResultsSummary(player1, player2);
            var resultsMarkup = new Markup(resultsText.ToString()).LeftJustified();
            var resultsPanel = new Panel(resultsMarkup).Expand().Header("Battle Updates").HeaderAlignment(Justify.Center);

            return resultsPanel;
        }
        private Panel CreateResultsPanelForGameOver(Player player)
        {
            var resultsText = $"> [purple]Game over![/] \n> [purple]{player.Name} wins![/]";
            var resultsMarkup = new Markup(resultsText.ToString()).LeftJustified();
            var resultsPanel = new Panel(resultsMarkup).Expand().Header("Battle Updates").HeaderAlignment(Justify.Center);

            return resultsPanel;
        }
        #endregion

        #region Start Screen Sub Layouts
        private Layout CreateMainMenuLayout()
        {
            var menuLayout = new Layout("MainMenu").MinimumSize(80);
            menuLayout.Update(mainMenu.GetMenuAsPanel());

            return menuLayout;
        }
        private Layout CreateMenuBorderLayout(string borderName)
        {
            var menuBorderLayout = new Layout(borderName).MinimumSize(20);

            var menuBorderText = "\n" + GameConstants.MainMenuBorder;
            var menuBorderMarkup = new Markup("[purple]" + menuBorderText + "[/]").Centered();
            var menuBorderPanel = new Panel(menuBorderMarkup).Expand();

            menuBorderLayout.Update(menuBorderPanel);
            return menuBorderLayout;
        }
        private Layout CreateFooterLayout()
        {
            var footerLayout = new Layout("Footer");

            var footerText = GameConstants.GameFooter;
            var footerMarkup = new Markup("[purple]" + footerText + "[/]").Centered();
            var footerPanel = new Panel(footerMarkup).Expand();

            footerLayout.Size(7);
            footerLayout.Update(footerPanel);
            return footerLayout;
        }
        #endregion

        // TODO update texts used for ships
        #region Ship Selection/Placement
        private Layout CreateShipSelectionMenuLayout()
        {
            var shipPlacementLayout = new Layout("ShipPlacementMenu").MinimumSize(60);
            shipPlacementLayout.Update(shipMenu.GetMenuAsPanel());

            return shipPlacementLayout;
        }
        private Layout CreateShipPlacementInfoLayout()
        {
            var infoLayout = CreateShipPlacementUpdatesLayout();
            var tipsLayout = CreateTipsLayout();

            var placementInfoLayout = new Layout("GameInfo").SplitColumns(infoLayout, tipsLayout);
            placementInfoLayout.Size((int)LayoutSize.GameInfo);

            return placementInfoLayout;

        }
        private Layout CreateShipPlacementUpdatesLayout()
        {
            var updatesLayout = new Layout("ShipPlacementInfo");
            var updatesText = ""; // update this text
            var updatesMarkup = new Markup(updatesText.ToString()).LeftJustified();
            var resultsPanel = new Panel(updatesMarkup).Expand().Header("Ship Placement Updates").HeaderAlignment(Justify.Center);

            updatesLayout.MinimumSize(60);
            updatesLayout.Update(resultsPanel);

            return updatesLayout;
        }

        #endregion
    }
}
