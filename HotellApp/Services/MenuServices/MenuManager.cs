using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.MenuServices
{
    public class MenuManager : IMenuService
    {

        private readonly string[] _menuItems;
        private readonly Action<int> _handleMenuSelection;


        public MenuManager(string[] menuItems, Action<int> handleMenuSelection)
        {
            _menuItems = menuItems;
            _handleMenuSelection = handleMenuSelection;
        }


        public void ShowMenu()
        {
            int selectedIndex = 0;
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("Använd upp- och ner-pilarna för att navigera och Enter för att välja:");

                // Visa menyn
                for (int i = 0; i < _menuItems.Length; i++)
                {
                    string spacing = i == selectedIndex ? " => " : "    ";
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{spacing} {_menuItems[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"{spacing} {_menuItems[i]}");
                    }
                }

                // Hantera knapptryckningar
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex == 0 ? _menuItems.Length - 1 : selectedIndex - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex == _menuItems.Length - 1 ? 0 : selectedIndex + 1;
                        break;

                    case ConsoleKey.Enter:
                        _handleMenuSelection(selectedIndex);  // Anropar den valda metoden

                        // Om användaren valde "Tillbaka till huvudmenyn", återgå till huvudmenyn
                        if (selectedIndex == _menuItems.Length - 1)  // Tillbaka till huvudmenyn
                        {
                            isRunning = false;
                        }
                        break;
                }
            }
        }


        public void HandleMenuSelection(int selectedIndex)
        {
            _handleMenuSelection(selectedIndex);
        }

        public string GetMenuItem(int index)
        {
            if (index >= 0 && index < _menuItems.Length)
            {
                return _menuItems[index];
            }
            throw new ArgumentOutOfRangeException(nameof(index), "Index utanför giltigt intervall.");
        }
    }

}
