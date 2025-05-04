using BattleshipGame.Objects;
using BattleshipGame;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame.Objects.Display
{
    public partial class GameDisplay
    {
		private static Layout CreateLayout()
		{
			var titleLayout = CreateDefaultTitleLayout();
			var boardLayout = CreateGameBoardLayout();
			var gameInfoLayout = CreateGameInfoLayout();

			var gameLayout = new Layout().SplitRows(titleLayout, boardLayout, gameInfoLayout);

			return gameLayout;
		}
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
		private static Layout CreateGameBoardLayout()
		{
			var playerBoardLayout = CreatePlayerBoardLayout(gamePlayer1);
			var firingBoardLayout = CreateFiringBoardLayout(gamePlayer1, gamePlayer2.Name);
			var statusBoardLayout = CreateStatusBoardsLayout();

			var boardLayout = new Layout("GameBoard").SplitColumns(playerBoardLayout, firingBoardLayout, statusBoardLayout);
			boardLayout.Size((int)LayoutSize.Gameboard);

			return boardLayout;
		}
		private static Layout CreatePlayerBoardLayout(Player player)
		{

			var playerBoardLayout = new Layout("PlayerBoard").MinimumSize(60);

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
		private static Layout CreatePlayerBoardLayoutTest()
		{
			var playerBoardLayout = new Layout("PlayerBoard").MinimumSize(60);

			var playerBoard1 = String.Format("{0}{1}{2}", "\n[yellow]", MakeGameBoard(gamePlayer1), "[/]");
			var playerBoardText = new Markup(playerBoard1).Centered();
			var playerBoardPanel = new Panel(playerBoardText).Expand().Header("Player Board Yellow").HeaderAlignment(Justify.Center);

			playerBoardLayout.Update(playerBoardPanel);

			return playerBoardLayout;
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

	}
}
