﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Utilities.Screens
{
    public class EndScreen : IPrintGraphics
    {
        public static void Print()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            string endScreen = @"
            ██████████████████████████████████████████████████████████████████████████████████████████████████████████
            █▌██╗    ██╗███████╗██╗      ██████╗ ██████╗ ███╗   ███╗███████╗    ██████╗  █████╗  ██████╗██╗  ██╗    ▐█
            █▌██║    ██║██╔════╝██║     ██╔════╝██╔═══██╗████╗ ████║██╔════╝    ██╔══██╗██╔══██╗██╔════╝██║ ██╔╝    ▐█
            █▌██║ █╗ ██║█████╗  ██║     ██║     ██║   ██║██╔████╔██║█████╗      ██████╔╝███████║██║     █████╔╝     ▐█
            █▌██║███╗██║██╔══╝  ██║     ██║     ██║   ██║██║╚██╔╝██║██╔══╝      ██╔══██╗██╔══██║██║     ██╔═██╗     ▐█
            █▌╚███╔███╔╝███████╗███████╗╚██████╗╚██████╔╝██║ ╚═╝ ██║███████╗    ██████╔╝██║  ██║╚██████╗██║  ██╗    ▐█
            █▌ ╚══╝╚══╝ ╚══════╝╚══════╝ ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚══════╝    ╚═════╝ ╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝    ▐█
            █▌                                                                                                      ▐█
            █▌                                      ████████╗ ██████╗                                               ▐█
            █▌                                      ╚══██╔══╝██╔═══██╗                                              ▐█
            █▌                                         ██║   ██║   ██║                                              ▐█
            █▌                                         ██║   ██║   ██║                                              ▐█
            █▌                                         ██║   ╚██████╔╝                                              ▐█
            █▌                                         ╚═╝    ╚═════╝                                               ▐█
            █▌                                                                                                      ▐█
            █▌████████╗██╗  ██╗███████╗██████╗ ███████╗███████╗██╗  ██╗ ██████╗ ████████╗███████╗██╗     ██╗     ██╗▐█
            █▌╚══██╔══╝██║  ██║██╔════╝██╔══██╗██╔════╝██╔════╝██║  ██║██╔═══██╗╚══██╔══╝██╔════╝██║     ██║     ██║▐█
            █▌   ██║   ███████║█████╗  ██████╔╝█████╗  ███████╗███████║██║   ██║   ██║   █████╗  ██║     ██║     ██║▐█
            █▌   ██║   ██╔══██║██╔══╝  ██╔══██╗██╔══╝  ╚════██║██╔══██║██║   ██║   ██║   ██╔══╝  ██║     ██║     ╚═╝▐█
            █▌   ██║   ██║  ██║███████╗██║  ██║███████╗███████║██║  ██║╚██████╔╝   ██║   ███████╗███████╗███████╗██╗▐█
            █▌   ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝╚══════╝╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚══════╝╚══════╝╚══════╝╚═╝▐█
            ██████████████████████████████████████████████████████████████████████████████████████████████████████████
                                                                                            ©Theres Östgård 2024   
            ";

            Console.WriteLine(endScreen);
            Console.ReadKey();
            Console.ResetColor();
            



        }
    }
}
