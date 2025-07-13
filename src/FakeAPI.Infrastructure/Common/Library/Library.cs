using System.Security.Cryptography;
using System.Text;
using FakeAPI.Application.Common.Interfaces;

namespace FakeAPI.Infrastructure.Common.Library;

public class Library : ILibrary
{
    public string GenerateUUID()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        int nanoPart = RandomNumberGenerator.GetInt32(1000000); // Simulate nano accuracy
        string timeComponent = $"{timestamp}{nanoPart:D6}";

        byte[] randomBytes = new byte[16];
        RandomNumberGenerator.Fill(randomBytes);
        string randomComponent = BitConverter.ToString(randomBytes).Replace("-", "");

        string combined = timeComponent + randomComponent;
        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));

        byte[] uuidBytes = new byte[16];
        Array.Copy(hash, uuidBytes, 16);

        Guid uuid = new Guid(uuidBytes);
        return uuid.ToString();
        
    }
}
