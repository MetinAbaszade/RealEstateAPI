using CORE.Abstract;
using CORE.Config;
using CORE.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Implementations;

public class UtilService(ConfigSettings config, IHttpContextAccessor context, IWebHostEnvironment environment) : IUtilService
{

    public string Encrypt(string value)
    {
        var key = config.CryptographySettings.KeyBase64;
        var privatekey = config.CryptographySettings.VBase64;
        var privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
        var keybyte = Encoding.UTF8.GetBytes(key);
        SymmetricAlgorithm algorithm = Aes.Create();
        var transform = algorithm.CreateEncryptor(keybyte, privatekeyByte);
        var inputbuffer = Encoding.Unicode.GetBytes(value);
        var outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Convert.ToBase64String(outputBuffer);
    }

    public string Decrypt(string value)
    {
        var key = config.CryptographySettings.KeyBase64;
        var privatekey = config.CryptographySettings.VBase64;
        var privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
        var keybyte = Encoding.UTF8.GetBytes(key);
        SymmetricAlgorithm algorithm = Aes.Create();
        var transform = algorithm.CreateDecryptor(keybyte, privatekeyByte);
        var inputbuffer = Convert.FromBase64String(value);
        var outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Encoding.Unicode.GetString(outputBuffer);
    }

    public string CreateGuid()
    {
        return Guid.NewGuid().ToString();
    }

    public string GetFolderName(EFileType type)
    {
        return type switch
        {
            EFileType.UserImages => @"files\images\user_profile",
            _ => "files/error"
        };
    }

    public string GetEnvFolderPath(string folderName)
    {
        return Path.Combine(environment.WebRootPath, folderName);
    }
}