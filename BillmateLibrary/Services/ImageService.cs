using System;
using System.IO;
using System.Text.RegularExpressions;
using BillMate.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;



public class ImageService : IIMageService
{
    private readonly IWebHostEnvironment _env;
    private readonly string _serverDomain;

    public ImageService(IWebHostEnvironment env, IConfiguration config)
    {
        _env = env;
        _serverDomain = config["ServerDomain"];
    }

    public string SaveBase64AsImage(string base64String, string companyId)
    {
        string fileName = Guid.NewGuid().ToString() + ".jpg"; // ensure unique filename
        string relativePath = Path.Combine("logos", companyId, fileName); // save in wwwroot/logos/companyId directory
        string absolutePath = Path.Combine(_env.WebRootPath, relativePath);

        // Remove data:image/jpeg;base64, from the string
        string base64Data = Regex.Replace(base64String, "^data:image\\/[a-zA-Z]+;base64,", string.Empty);

        // Convert base64 string to byte[]
        byte[] imageBytes = Convert.FromBase64String(base64Data);

        // Ensure the directory exists
        string directoryPath = Path.GetDirectoryName(absolutePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Write bytes to file
        File.WriteAllBytes(absolutePath, imageBytes);

        // return URL for the image
        string imageUrl = $"{_serverDomain}/{relativePath.Replace("\\", "/")}";
        return imageUrl;
    }

}
