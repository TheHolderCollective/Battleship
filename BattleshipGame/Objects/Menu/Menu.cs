using Spectre.Console;
using System.Collections.Generic;
using System.Text;

namespace BattleshipGame.Objects.GameMenu
{
    internal class Menu
    {
        public int CurrentSelection { get; set; }

        public string SelectedItemName
        {
            get
            {
                return menuItemList[CurrentSelection].ItemName;
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

            // select first item by default
            menuItemList[0].IsSelected = true;
            CurrentSelection = 0;
        }

        public void AddMenuItem(MenuItem item)
        {
            if (menuItemList == null)
            {
                menuItemList = new List<MenuItem>();
                CurrentSelection = 0;
            }

            menuItemList.Add(item);
        }

        public void SelectNextItem()
        {
            int nextSelection = CurrentSelection + 1;
            int maxSelection = menuItemList.Count - 1;

            if (nextSelection <= maxSelection)
            {
                menuItemList[CurrentSelection].IsSelected = false;
                CurrentSelection++;
                menuItemList[CurrentSelection].IsSelected = true;
            }
        }

        public void SelectPreviousItem()
        {
            int nextSelection = CurrentSelection -1;
            int minSelection = 0;

            if (nextSelection >= minSelection)
            {
                menuItemList[CurrentSelection].IsSelected = false;
                CurrentSelection--;
                menuItemList[CurrentSelection].IsSelected = true;
            }
        }

        public Panel GetMenuAsPanel()
        {
            var menuText = new Markup(CreateMenuMarkup()).Centered(); 
            var menuPanel = new Panel(menuText).Expand().Header("").HeaderAlignment(Justify.Center);

            return menuPanel;
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
