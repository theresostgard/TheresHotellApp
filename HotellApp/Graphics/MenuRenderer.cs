using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Graphics
{
    public static class MenuRenderer
    {
        public static void ShowMenu(string[] menuItems, ref int selectedIndex, string headerTitle, Action<int> onSelect)
        {

            bool inMenu = true;
            while (inMenu)
            {
                Console.Clear();
                //Skapa en regel för menytiteln
                Spectre.Console.AnsiConsole.Write(
                    new Spectre.Console.Rule($"[bold blue]{headerTitle}[/]")
                        .Justify(Spectre.Console.Justify.Center)
                );



                // Instruktioner för att navigera
                Spectre.Console.AnsiConsole.Write(new Spectre.Console.Markup(
                    "[italic grey]Använd [yellow]piltangenterna[/] för att navigera och [green]Enter[/] för att välja:[/]\n"
                ));

                // Skapa en panel runt menyn
                var menuPanel = new Spectre.Console.Panel(RenderMenu(menuItems, selectedIndex))
                {
                    Border = Spectre.Console.BoxBorder.Double,
                    BorderStyle = Spectre.Console.Style.Parse("green"),
                    Header = new Spectre.Console.PanelHeader("[bold blue]Menyval[/]")

                };



                Spectre.Console.AnsiConsole.Write(menuPanel);

                // Vänta på användarens val
                var key = Console.ReadKey(intercept: true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex == 0 ? menuItems.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex == menuItems.Length - 1 ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        // Utför det valda alternativet
                        onSelect(selectedIndex);

                        // Om det valda alternativet är "Tillbaka till huvudmenyn", sätt inMenu till false
                        if (menuItems[selectedIndex] == "Tillbaka till huvudmenyn")
                        {
                            inMenu = false;
                        }
                        break;
                }
            }
        }


        // Metod för att formatera och skapa strängen för menyn
        private static string RenderMenu(string[] menuItems, int selectedIndex)
        {
            var menuBuilder = new System.Text.StringBuilder();

            for (int i = 0; i < menuItems.Length; i++)
            {
                string marker = i == selectedIndex ? " => " : "    "; // Marker for the selected item

                // Conditional styling: Blue for selected, Red for "Tillbaka till huvudmenyn"
                string style = i == selectedIndex
                    ? (menuItems[i] == "Tillbaka till huvudmenyn" ? "[bold red]" : "[bold green]")
                    : "[white]"; // Default white for non-selected items

                // Add the menu item with the proper style
                menuBuilder.AppendLine($"{marker}{style}{menuItems[i]}[/]");  // Using Spectre markup to apply styles
            }

            return menuBuilder.ToString();
        }
    }
}
