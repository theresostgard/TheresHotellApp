using HotellApp.Controllers.InvoiceController;
using HotellApp.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.MenuServices.InvoiceMenues
{
    public class InvoiceMenu(IInvoiceController invoiceController) : IInvoiceMenu
    {
        private readonly IInvoiceController _invoiceController;


        private readonly string[] _menuItems =
            {
                 "Lägg till ny faktura",
                "Visa faktura",
                "Uppdatera faktura",
                "Radera faktura",
                "Tillbaka till huvudmenyn"
            };

   

        public void ShowInvoiceOptions()
        {
            int selectedIndex = 0;
            MenuRenderer.ShowMenu(_menuItems, ref selectedIndex, "Fakturameny", HandleMenuSelection);
        }

        private void HandleMenuSelection(int selectedIndex)
        {
            string selectedItem = _menuItems[selectedIndex];
            switch (selectedItem)
            {
                case "Lägg till ny faktura":
                    Console.Clear();
                    _invoiceController.CreateInvoice();
                    Console.ReadKey();
                    break;
                case "Visa faktura":
                    Console.Clear();
                    _invoiceController.ReadInvoice();
                    Console.ReadKey();
                    break;
                case "Uppdatera faktura":
                    Console.Clear();
                    _invoiceController.UpdateInvoice();
                    Console.ReadKey();
                    break;
                case "Radera faktura":
                    Console.Clear();
                    _invoiceController.DeleteInvoice();
                    Console.ReadKey();
                    break;
                case "Tillbaka till huvudmenyn":
                    Console.WriteLine("Tillbaka till huvudmenyn...");
                    return;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }


        }
    }
}
