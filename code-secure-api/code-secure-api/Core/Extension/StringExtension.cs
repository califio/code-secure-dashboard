using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace CodeSecure.Core.Extension;

public static class StringExtension
{
    public static bool IsHttpUrl(this string? value)
    {
        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
        }

        return false;
    }
    public static string NormalizeUpper(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }
        return value.Trim().ToUpper();
    }
    
    public static string UrlEncode(this string value)
    {
        return HttpUtility.UrlEncode(value);
    }
    
    public static string UrlDecode(this string value)
    {
        return HttpUtility.UrlDecode(value);
    }
    public static bool IsEmail(this string email)
    {
        return MailAddress.TryCreate(email, out _);
    }

    public static string Slug(this string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        text = text.ToLower();
        text = Regex.Replace(text, @"(à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ)", "a");
        text = Regex.Replace(text, @"(è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ)", "e");
        text = Regex.Replace(text, @"(ì|í|ị|ỉ|ĩ)", "i");
        text = Regex.Replace(text, @"(ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ)", "o");
        text = Regex.Replace(text, @"(ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ)", "u");
        text = Regex.Replace(text, @"(ỳ|ý|ỵ|ỷ|ỹ)", "y");
        text = Regex.Replace(text, @"(đ)", "d");
        // Remove special characters
        text = Regex.Replace(text, @"[^0-9a-z\s-]", "-");
        // Replace whitespace with '-'
        text = Regex.Replace(text, @"\s+", "-");
        // Remove consecutive '-'
        text = Regex.Replace(text, @"-+", "-");
        // Trim leading and trailing '-'
        text = text.Trim('-');
        return text;
    }
}