using System;
using System.Text;

/// <summary>
/// CaptchaHelper 的摘要描述
/// </summary>
public static class CaptchaHelper
{
    public static void CreateImageText(System.Web.SessionState.HttpSessionState session)
    {
        string randomString = GetRandomString(4);
        session["ImgText"] = randomString;
    }

    public static string GetRandomString(int length)
    {
        //char[] chars = @"23456789ABCDEFGHIJKLMNPQRSTUVWXYZ".ToCharArray();
        char[] chars = @"0123456789".ToCharArray();
        Random r = new Random((int)DateTime.Now.Ticks);
        StringBuilder sb = new StringBuilder(length);
        for (int i = 0; i < length; i++)
            sb.Append(chars[r.Next(chars.Length)]);
        return sb.ToString();
    }
}