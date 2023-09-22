using BillMate.Data;
using BillMate.Models;
using BillMate.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BillMate.Services
{
    public class URLShortnerService : IURLShortnerService
    {
        private BillMateDBContext _dbContext;

        public URLShortnerService(BillMateDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool DoesShortenedUrlExists(string url)
        {
            bool doesExist = false;

            if (_dbContext.ShortenedURL.Count(u => u.ShortUrl == url) > 0)
            {
                doesExist = true;
            }

            return doesExist;
        }

        public List<ShortenedURL> GetAllShortenedUrls()
        {
            List<ShortenedURL> shortenedURLModels = new List<ShortenedURL>();
            var urls = _dbContext.ShortenedURL.ToList();
            shortenedURLModels = (from s in urls
                                  select new ShortenedURL
                                  {
                                      PatientId = s.PatientId,
                                      ClientId = s.ClientId,
                                      StoredTime = s.StoredTime,
                                      DateExpires = s.DateExpires,
                                      ShortenUrlId = s.ShortenUrlId,
                                      ShortUrl = s.ShortUrl,
                                      Token = s.Token,
                                      Url = s.Url
                                  }).ToList();
            return shortenedURLModels;
        }

        public ShortenedURL GetShortenedUrl(int clientId, int patientId, string url)
        {
            ShortenedURL shortenedURL = new ShortenedURL();
            var urlFromDB = _dbContext.ShortenedURL.Where(s => s.ClientId == clientId && s.PatientId == patientId && s.Url == url).SingleOrDefault();
            shortenedURL = new ShortenedURL()
            {
                ClientId = urlFromDB.ClientId,
                PatientId = urlFromDB.PatientId,
                StoredTime = urlFromDB.StoredTime,
                DateExpires = urlFromDB.DateExpires,
                ShortUrl = urlFromDB.ShortUrl,
                Token = urlFromDB.Token,
                Url = urlFromDB.Url,
                ShortenUrlId = urlFromDB.ShortenUrlId
            };
            return shortenedURL;
        }

        public string GetURLByToken(string token)
        {
            string url = string.Empty;
            var shortenedURL = _dbContext.ShortenedURL.Where(s => s.Token == token).SingleOrDefault();
            if (shortenedURL != null)
            {
                url = shortenedURL.Url;
            }
            return url;
        }

        public bool SaveShortnedUrl(ShortenedURL shortenedURLModel)
        {
            ShortenedURL shortenedURL = new ShortenedURL
            {
                ClientId = shortenedURLModel.ClientId,
                PatientId = shortenedURLModel.PatientId,
                StoredTime = shortenedURLModel.StoredTime,
                DateExpires = shortenedURLModel.DateExpires,
                ShortUrl = shortenedURLModel.ShortUrl,
                Token = shortenedURLModel.Token,
                Url = shortenedURLModel.Url
            };
            _dbContext.ShortenedURL.Add(shortenedURL);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
