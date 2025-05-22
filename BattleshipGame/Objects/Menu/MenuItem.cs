namespace BattleshipGame.Objects.GameMenu
{
    internal class MenuItem
    {
        public string ItemName {get; set;}
        public bool IsSelected { get; set;} 

        public MenuItem(string itemName, bool selectedStatus)
        {
            ItemName = itemName;
            IsSelected = selectedStatus;
        }

        /// <summary>
        /// Returns item name with markup applied
        /// </summary>
        /// <returns></returns>
        public string GetNameWithMarkup()
        {
            string nameWithMarkup = ItemName;

            if (IsSelected)
            {
                nameWithMarkup = "[invert]" + ItemName + "[/]";
            }

            return nameWithMarkup;
        }
    }
}


