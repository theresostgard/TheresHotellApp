using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Controllers.MenuControllers
{
    public interface IMenuService
    {
        void ShowMenu();  // Visa menyn
        void HandleMenuSelection(int selectedIndex);  // Hantera val
    }
}
