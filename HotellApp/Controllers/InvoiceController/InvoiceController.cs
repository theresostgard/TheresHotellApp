using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Controllers.InvoiceController
{
    public class InvoiceController : IInvoiceController
    {
        public void CreateInvoice() => Console.WriteLine("Invoice Created");
        public void ReadInvoice() => Console.WriteLine("Invoice Read");

        public void UpdateInvoice() => Console.WriteLine("Invoice Updated");
        public void DeleteInvoice() => Console.WriteLine("Invoice Deleted");
    }
}
