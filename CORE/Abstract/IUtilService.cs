using CORE.Enums;
using Microsoft.AspNetCore.Http;

namespace CORE.Abstract;

public interface IUtilService
{
    public string Encrypt(string value);
    public string Decrypt(string value);
    public string CreateGuid();
    public string GetFolderName(EFileType type);
    string GetEnvFolderPath(string folderName);
}