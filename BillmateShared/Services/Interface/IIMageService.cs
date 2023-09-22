namespace BillMate.Services.Interface
{
    public interface IIMageService
    {
        public string SaveBase64AsImage(string base64String, string companyId);
    }
}
