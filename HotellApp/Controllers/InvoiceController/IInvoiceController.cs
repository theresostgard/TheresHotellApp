namespace HotellApp.Controllers.InvoiceController
{
    public interface IInvoiceController
    {
        void CreateInvoice();
        void ReadInvoice();

        void UpdateInvoice();
        void DeleteInvoice();
    }
}