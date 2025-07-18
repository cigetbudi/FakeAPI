namespace FakeAPI.Application.Common.Interfaces;

public interface IJWTService
{
    string GenerateToken(string username);
    bool ValidateUser(string username, string password);
}
