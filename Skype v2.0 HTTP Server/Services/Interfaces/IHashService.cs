namespace HttpServer.Services.Interfaces
{
    public interface IHashService
    {
        string HashPassword(string rawPassword, byte[] salt);

        byte[] GenerateSalt(int bits = 128);
    }
}