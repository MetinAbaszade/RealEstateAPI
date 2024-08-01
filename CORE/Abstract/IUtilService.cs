﻿using CORE.Enums;

namespace CORE.Abstract;

public interface IUtilService
{
    public string? GetTokenString();
    public bool IsValidToken();
    public Guid GetUserIdFromToken();
    public Guid? GetCompanyIdFromToken();
    public string GenerateRefreshToken();
    public string? GetRoleFromToken(string jwtToken);
    public string TrimToken(string? jwtToken);
    public string Encrypt(string value);
    public string Decrypt(string value);
    public Task SendMailAsync(string email, string message);
    public string CreateGuid();
    public string GetFolderName(EFileType type);
    string GetEnvFolderPath(string folderName);
}