using BillMate.Services;
using System.Configuration;
using System.Linq;
using System;
using BillMate.Services.Interface;
using Microsoft.Extensions.Configuration;

namespace BillMate.Models.UrlShortener
{
    public class Shortner
    {
        public Shortner(string url, int clientId, int patientId, IURLShortnerService urlShortnerService, IConfiguration configuration)
        {
            var urls = urlShortnerService.GetAllShortenedUrls();

            if (urls.Exists(u => u.Url == url))
            {
                shortenedURLModel = urlShortnerService.GetShortenedUrl(clientId, patientId, url);
                Token = shortenedURLModel.Token;
            }
            else
            {
                var token = GenerateToken();

                //while the token exists in our database, generate a new one
                while (urls.Exists(u => u.Token == token.Token))
                {
                    token = GenerateToken();
                }

                Token = token.Token;
                shortenedURLModel = new ShortenedURL
                {
                    Token = Token,
                    Url = url,
                    ClientId = clientId,
                    PatientId = patientId,
                    StoredTime = DateTime.UtcNow,
                    DateExpires = null,
                    ShortUrl = configuration["ServerDomain"] + Token
                };

                //save the url to the database
                urlShortnerService.SaveShortnedUrl(shortenedURLModel);
            }
        }

        // Letters and numbers that are not easily mixed with others when reading
        private const string ValidChars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
        private readonly ShortenedURL shortenedURLModel;
        private readonly Random random = new Random();

        public string Token { get; set; }


        public Shortner GenerateToken()
        {
            int randomLength = random.Next(2, 6);
            Token = new string(Enumerable.Repeat(ValidChars, randomLength)
                                         .Select(s => s[random.Next(s.Length)])
                                         .ToArray());
            return this;
        }
    }
}
