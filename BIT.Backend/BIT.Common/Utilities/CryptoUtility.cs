namespace BIT.Common.Utilities;

public static class CryptoUtility
{
    private const int SaltSize = 16;

    public static string CreateHash(string input, string salt)
    {
        return BCrypt.Net.BCrypt.HashPassword(input, salt);
    }

    public static bool VerifyHash(string input, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(input, hash);
    }

    public static string GenerateSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(SaltSize);
    }
}
