using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Utilities
{
    public static class MenuRenderer
    {
        public static void ShowMenu(string[] menuItems, ref int selectedIndex, string headerTitle, Action<int> onSelect)
        {

            bool inMenu = true;
            while (inMenu)
            {
                Console.Clear();
                AnsiConsole.Write(
                    new Rule($"[bold blue]{headerTitle}[/]")
                        .Justify(Justify.Center)
                );



                AnsiConsole.Write(new Markup(
                    "[italic grey]Använd [yellow]piltangenterna[/] för att navigera och [green]Enter[/] för att välja:[/]\n"
                ));

   
                var menuPanel = new Panel(RenderMenu(menuItems, selectedIndex))
                {
                    Border = BoxBorder.Double,
                    BorderStyle = Style.Parse("green"),
                    Header = new PanelHeader("[bold blue]Menyval[/]")

                };



                AnsiConsole.Write(menuPanel);


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
  
                        onSelect(selectedIndex);

                        if (menuItems[selectedIndex] == "Tillbaka till huvudmenyn")
                        {
                            inMenu = false;
                        }
                        break;
                }
            }
        }

        private static string RenderMenu(string[] menuItems, int selectedIndex)
        {
            var menuBuilder = new StringBuilder();

            for (int i = 0; i < menuItems.Length; i++)
            {
                string marker = i == selectedIndex ? " => " : "    "; 

                string style = i == selectedIndex
                    ? menuItems[i] == "Tillbaka till huvudmenyn" ? "[bold red]" : "[bold green]"
                    : "[white]"; 

                menuBuilder.AppendLine($"{marker}{style}{menuItems[i]}[/]");  
            }

            return menuBuilder.ToString();
        }
    }
}
