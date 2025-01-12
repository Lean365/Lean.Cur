using System.Security.Cryptography;
using System.Text;

namespace Lean.Cur.Common.Extensions;

/// <summary>
/// 字符串扩展方法
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 判断字符串是否为空
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>是否为空</returns>
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 判断字符串是否为空或空白
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>是否为空或空白</returns>
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 转换为小写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>小写字符串</returns>
    public static string ToLower(this string str)
    {
        return str?.ToLower() ?? string.Empty;
    }

    /// <summary>
    /// 转换为大写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>大写字符串</returns>
    public static string ToUpper(this string str)
    {
        return str?.ToUpper() ?? string.Empty;
    }

    /// <summary>
    /// 转换为首字母小写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>首字母小写字符串</returns>
    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        return char.ToLower(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// 转换为首字母大写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>首字母大写字符串</returns>
    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        return char.ToUpper(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// 将字符串转换为 MD5 加密字符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>MD5 加密字符串</returns>
    public static string ToMD5(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        
        using var md5 = MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(str);
        var hash = md5.ComputeHash(bytes);
        var builder = new StringBuilder();
        
        foreach (var b in hash)
        {
            builder.Append(b.ToString("x2"));
        }
        
        return builder.ToString();
    }
} 