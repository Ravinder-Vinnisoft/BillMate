using BillMate.Models;
using System.Collections.Generic;

namespace BillMate.Services.Interface
{
    public interface IURLShortnerService
    {
        public List<ShortenedURL> GetAllShortenedUrls();
        public string GetURLByToken(string token);
        public ShortenedURL GetShortenedUrl(int clientId, int patientId, string url);

        public bool SaveShortnedUrl(ShortenedURL shortenedURLModel);

        public bool DoesShortenedUrlExists(string url);
    }
}
