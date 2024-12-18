namespace HotellApp.Services.InvoiceServices
{
    public interface IInvoiceService
    {
        void CreateInvoice();
        void ReadInvoice();

        void UpdateInvoice();
        void DeleteInvoice();
    }
}