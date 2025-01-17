using System.Security.Cryptography;

namespace CodeSecure.Authentication;

public static class PasswordGenerator
{
    private static readonly char[] UpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private static readonly char[] LowerCaseLetters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
    private static readonly char[] Digits = "0123456789".ToCharArray();
    private static readonly char[] SpecialCharacters = "~!@#$%^*-_".ToCharArray();

    private static readonly char[] AllCharacters =
        $"{new string(UpperCaseLetters)}{new string(LowerCaseLetters)}{new string(Digits)}{new string(SpecialCharacters)}"
            .ToCharArray();

    public static string GeneratePassword(int length = 8)
    {
        if (length < 6) length = 6;
        // Create a buffer to store the generated password
        var password = new char[length];
        // Add at least one character from each category
        password[0] = UpperCaseLetters[RandomNumberGenerator.GetInt32(UpperCaseLetters.Length)];
        password[1] = LowerCaseLetters[RandomNumberGenerator.GetInt32(LowerCaseLetters.Length)];
        password[2] = Digits[RandomNumberGenerator.GetInt32(Digits.Length)];
        password[3] = SpecialCharacters[RandomNumberGenerator.GetInt32(SpecialCharacters.Length)];
        for (var i = 4; i < length; i++)
            password[i] = AllCharacters[RandomNumberGenerator.GetInt32(AllCharacters.Length)];
        var rd = new Random();
        rd.Shuffle(password);
        return new string(password);
    }
}