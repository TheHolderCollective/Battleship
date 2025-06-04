using Spectre.Console;
using System;
using BattleshipGame.Objects.GameMenu;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
        private static Layout CreateLayouts()
        {
            var titleLayout = CreateDefaultTitleLayout();
            var startScreenLayout = CreateStartScreenLayout();
            var boardLayout = CreateGameBoardLayout();         
            var shipPlacementLayout = CreateShipPlacementLayout();
            var demoBoardlayout = CreateDemoLayout();

            var gameLayout = new Layout().SplitRows(titleLayout, boardLayout, startScreenLayout, shipPlacementLayout, demoBoardlayout);
            return gameLayout;
        }
        private void DeactivateAllLayouts()
        {
            gameLayout["StartScreen"].Invisible();
            gameLayout["GameBoard"].Invisible();
            gameLayout["ShipPlacementBoard"].Invisible();
            gameLayout["DemoBoard"].Invisible();
        }

        #region Game Title
        private static Layout CreateDefaultTitleLayout()
        {
            return CreateTitleLayout("[purple]");
        }
        private static Layout CreateTitleLayout(string titleColor)
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
        private static Layout CreateStartScreenLayout()
        {

            var mainMenuLayout = CreateMainMenuLayout();
            var menuLeftBorderLayout = CreateMenuBorderLayout("MenuLeftBorder");
            var menuRightBorderLayout = CreateMenuBorderLayout("MenuRightBorder");
            var footerLayout = CreateFooterLayout();

            var startScreenLayout = new Layout("StartScreen").SplitRows(new Layout("Menu").SplitColumns(menuLeftBorderLayout, mainMenuLayout, menuRightBorderLayout), footerLayout);
            return startScreenLayout;
        }
        private static Layout CreateGameBoardLayout()
        {
            var playerBoardLayout = CreatePlayerBoardLayout(gamePlayer1,"PlayerGameBoard");
            var firingBoardLayout = CreateFiringBoardLayout(gamePlayer1, gamePlayer2.Name);
            var statusBoardLayout = CreateStatusBoardsLayout();
            var gameInfoLayout = CreateGameInfoLayout();

            var playerBoardsLayout = new Layout("PlayerBoards").SplitColumns(playerBoardLayout, firingBoardLayout, statusBoardLayout);

            playerBoardsLayout.Size((int)LayoutSize.Gameboard);

            var boardLayout = new Layout("GameBoard").SplitRows(playerBoardsLayout, gameInfoLayout);
            return boardLayout;
        }
        private static Layout CreateShipPlacementLayout()
        {
            var playerBoardLayout = CreatePlayerBoardLayout(gamePlayer1,"ShipPlacementGameBoard");
            var shipSelectionMenuLayout = CreateShipSelectionMenuLayout();
            var shipPlacementInfoLayout = CreateShipPlacementInfoLayout();

            var shipPlacementLayout = new Layout("ShipPlacementBoard").SplitRows(new Layout("ShipPlacementSubBoard").SplitColumns(playerBoardLayout, shipSelectionMenuLayout), shipPlacementInfoLayout);
            
            return shipPlacementLayout;
        }
        private static Layout CreateDemoLayout()
        {
            var playerBoardLayout = CreatePlayerBoardLayout(gamePlayer1,"DemoGameboard");
            var firingBoardLayout = CreateFiringBoardLayout(gamePlayer1, gamePlayer2.Name);
            var statusBoardLayout = CreateStatusBoardsLayout();

            var demoBoardLayout = new Layout("DemoBoard").SplitColumns(playerBoardLayout, firingBoardLayout, statusBoardLayout);

            demoBoardLayout.Size((int)LayoutSize.Gameboard);
            return demoBoardLayout;
           
        }
        #endregion

        #region Gameplay 
        private static Layout CreatePlayerBoardLayout(Player player,string layoutName)
        {

            var playerBoardLayout = new Layout(layoutName).MinimumSize(60);

            var playerBoard1 = String.Format(Environment.NewLine + MakeGameBoard(player));
            var playerBoardText = new Markup(playerBoard1).Centered();
            var playerBoardPanel = new Panel(playerBoardText).Expand().Header(player.Name + "'s Board").HeaderAlignment(Justify.Center);

            playerBoardLayout.Update(playerBoardPanel);

            return playerBoardLayout;
        }
        private static Layout CreateFiringBoardLayout(Player player, string opponentsName)
        {
            var firingBoardLayout = new Layout("FiringBoard").MinimumSize(60);

            var playerBoard2 = String.Format(Environment.NewLine + MakeFiringBoard(player));
            var firingBoardText = new Markup(playerBoard2).Centered();
            var firingBoardPanel = new Panel(firingBoardText).Expand().Header(opponentsName + "'s Board").HeaderAlignment(Justify.Center);

            firingBoardLayout.Update(firingBoardPanel);

            return firingBoardLayout;
        }
        private static Layout CreateStatusBoardsLayout()
        {
            var playerStatusLayout = CreatePlayerStatusLayout("PlayerStatus", "Status (" + gamePlayer1.Name + ")", gamePlayer1);
            var opponentStatusLayout = CreatePlayerStatusLayout("OpponentStatus", "Status (" + gamePlayer2.Name + ")", gamePlayer2);

            var statusBoardLayout = new Layout("StatusBoard").SplitRows(playerStatusLayout, opponentStatusLayout);

            return statusBoardLayout;
        }
        private static Layout CreatePlayerStatusLayout(string layoutName, string statusHeader, Player player)
        {
            var playerStatusLayout = new Layout(layoutName);
            var playerStatusText = GetShipStatusLists(player.Ships);
            var playerStatusMarkup = new Markup(playerStatusText).LeftJustified();
            var playerStatusPanel = new Panel(playerStatusMarkup).Expand().Header(statusHeader).HeaderAlignment(Justify.Center);

            playerStatusLayout.Update(playerStatusPanel);

            return playerStatusLayout;
        }
        private static Layout CreateGameInfoLayout()
        {
            var resultsLayout = CreateResultsLayout();
            var targetInfoLayout = CreateTargetInfoLayout();
            var tipsLayout = CreateTipsLayout();


            var gameInfoLayout = new Layout("GameInfo").SplitColumns(resultsLayout, targetInfoLayout, tipsLayout);
            gameInfoLayout.Size((int)LayoutSize.GameInfo);

            return gameInfoLayout;

        }
        private static Layout CreateTargetInfoLayout()
        {
            // TODO Determine how target info will be fed into panel
            var targetInfoLayout = new Layout("TargetInfo");
            var targetText = "X: " + "Y: " + "\nTargeting: ( , )";
            var targetMarkup = new Markup(targetText.ToString()).LeftJustified();
            var targetPanel = new Panel(targetMarkup).Expand().Header("Targeting Dashboard").HeaderAlignment(Justify.Center);

            targetInfoLayout.MinimumSize(60);
            targetInfoLayout.Update(targetPanel);

            return targetInfoLayout;

        }
        private static Layout CreateTipsLayout()
        {
            // TODO Find a way to pull in keyboard tips
            var tipsLayout = new Layout("Tips");
            var tipsText = "Keystroke tips go here.";
            var tipsMarkup = new Markup(tipsText.ToString()).LeftJustified();
            var tipsPanel = new Panel(tipsMarkup).Expand().Header("Keyboard Tips").HeaderAlignment(Justify.Center);

            tipsLayout.Update(tipsPanel);

            return tipsLayout;
        }
        private static Layout CreateResultsLayout()
        {
            var resultsLayout = new Layout("Results");
            var resultsText = GetRoundResultsSummary(gamePlayer1, gamePlayer2);
            var resultsMarkup = new Markup(resultsText.ToString()).LeftJustified();
            var resultsPanel = new Panel(resultsMarkup).Expand().Header("Battle Updates").HeaderAlignment(Justify.Center);

            resultsLayout.MinimumSize(60);
            resultsLayout.Update(resultsPanel);

            return resultsLayout;
        }
        #endregion

        // TODO update texts used for ships
        #region Start Screen Sub Layouts
        private static Layout CreateMainMenuLayout()
        {
            var menuLayout = new Layout("MainMenu").MinimumSize(80);
            menuLayout.Update(mainMenu.GetMenuAsPanel());

            return menuLayout;
        }
        private static Layout CreateMenuBorderLayout(string borderName)
        {
            var menuBorderLayout = new Layout(borderName).MinimumSize(20);

            var menuBorderText = "\n" + GameConstants.MainMenuBorder;
            var menuBorderMarkup = new Markup("[purple]" + menuBorderText + "[/]").Centered();
            var menuBorderPanel = new Panel(menuBorderMarkup).Expand();

            menuBorderLayout.Update(menuBorderPanel);
            return menuBorderLayout;
        }
        private static Layout CreateFooterLayout()
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

        #region Ship Selection/Placement
        private static Layout CreateShipSelectionMenuLayout()
        {
            var shipPlacementLayout = new Layout("ShipPlacementMenu").MinimumSize(60);
            shipPlacementLayout.Update(shipMenu.GetMenuAsPanel());

            return shipPlacementLayout;
        }
        private static Layout CreateShipPlacementInfoLayout()
        {
            var infoLayout = CreateShipPlacementUpdatesLayout();
            var tipsLayout = CreateTipsLayout();

            var placementInfoLayout = new Layout("GameInfo").SplitColumns(infoLayout,tipsLayout);
            placementInfoLayout.Size((int)LayoutSize.GameInfo);

            return placementInfoLayout;

        }
        private static Layout CreateShipPlacementUpdatesLayout()
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
