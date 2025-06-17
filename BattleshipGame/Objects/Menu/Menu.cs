using Spectre.Console;
using System.Collections.Generic;
using System.Text;

namespace BattleshipGame.Objects.GameMenu
{
    internal class Menu
    {
        private int CurrentItemIndex { get; set; }
        public string SelectedItemName
        {
            get
            {
                return menuItemList[CurrentItemIndex].ItemName;
            }
        }

        private List<MenuItem> menuItemList;

        public Menu(string[] menuItems)
        {
            menuItemList = new List<MenuItem>();

            foreach (var item in menuItems)
            {
                MenuItem menuItem = new MenuItem(item, false);
                menuItemList.Add(menuItem);
            }

            SetFirstItemAsSelected();
        }

        public void AddMenuItem(MenuItem item)
        {
            if (menuItemList == null)
            {
                menuItemList = new List<MenuItem>();
                CurrentItemIndex = 0;
            }

            menuItemList.Add(item);
        }

        public void SelectNextItem()
        {
            int nextSelection = CurrentItemIndex + 1;
            int maxSelection = menuItemList.Count - 1;

            if (nextSelection <= maxSelection)
            {
                menuItemList[CurrentItemIndex].IsSelected = false;
                CurrentItemIndex++;
                menuItemList[CurrentItemIndex].IsSelected = true;
            }
        }

        public void SelectPreviousItem()
        {
            int nextSelection = CurrentItemIndex -1;
            int minSelection = 0;

            if (nextSelection >= minSelection)
            {
                menuItemList[CurrentItemIndex].IsSelected = false;
                CurrentItemIndex--;
                menuItemList[CurrentItemIndex].IsSelected = true;
            }
        }

        public Panel GetMenuAsPanel()
        {
            var menuText = new Markup(CreateMenuMarkup()).Centered(); 
            var menuPanel = new Panel(menuText).Expand().Header("").HeaderAlignment(Justify.Center);

            return menuPanel;
        }

        public MainMenuItems GetSelectedItem()
        {
            return (MainMenuItems)CurrentItemIndex;
        }

        public void Reset()
        {
            menuItemList[CurrentItemIndex].IsSelected = false;
            SetFirstItemAsSelected();
        }

        private void SetFirstItemAsSelected()
        {
            menuItemList[0].IsSelected = true;
            CurrentItemIndex = 0;
        }

        private string CreateMenuMarkup()
        {
            string verticalSpacing = "\n\n\n";

            StringBuilder menuString = new StringBuilder();

            foreach (var item in menuItemList)
            {
                menuString.Append(verticalSpacing + item.GetNameWithMarkup());
            }

            return menuString.ToString();
        }

    }
}
