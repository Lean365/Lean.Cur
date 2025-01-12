using System.Security.Cryptography;
using System.Text;

namespace Lean.Cur.Common.Security;

/// <summary>
/// 密码处理工具类
/// </summary>
public static class LeanPassword
{
  /// <summary>
  /// 生成密码盐值
  /// </summary>
  /// <param name="size">盐值长度，默认32字节</param>
  /// <returns>盐值的十六进制字符串</returns>
  public static string GenerateSalt(int size = 32)
  {
    var bytes = new byte[size];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(bytes);
    return Convert.ToHexString(bytes);
  }

  /// <summary>
  /// 使用PBKDF2算法加密密码
  /// </summary>
  /// <param name="password">原始密码</param>
  /// <param name="salt">盐值</param>
  /// <param name="iterations">迭代次数，默认10000次</param>
  /// <returns>加密后的密码</returns>
  public static string HashPassword(string password, string salt, int iterations = 10000)
  {
    using var pbkdf2 = new Rfc2898DeriveBytes(
        Encoding.UTF8.GetBytes(password),
        Convert.FromHexString(salt),
        iterations,
        HashAlgorithmName.SHA256);

    var hash = pbkdf2.GetBytes(32); // 256位的哈希
    return Convert.ToHexString(hash);
  }

  /// <summary>
  /// 验证密码
  /// </summary>
  /// <param name="password">待验证的密码</param>
  /// <param name="salt">盐值</param>
  /// <param name="hash">已存储的密码哈希</param>
  /// <param name="iterations">迭代次数，默认10000次</param>
  /// <returns>密码是否正确</returns>
  public static bool VerifyPassword(string password, string salt, string hash, int iterations = 10000)
  {
    var newHash = HashPassword(password, salt, iterations);
    return newHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
  }

  /// <summary>
  /// 生成随机密码
  /// </summary>
  /// <param name="length">密码长度，默认12位</param>
  /// <returns>随机密码</returns>
  public static string GenerateRandomPassword(int length = 12)
  {
    const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
    const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const string numberChars = "0123456789";
    const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    var allChars = lowerChars + upperChars + numberChars + specialChars;
    var result = new char[length];
    var rng = RandomNumberGenerator.Create();

    // 确保至少包含一个小写字母、一个大写字母、一个数字和一个特殊字符
    result[0] = GetRandomChar(lowerChars, rng);
    result[1] = GetRandomChar(upperChars, rng);
    result[2] = GetRandomChar(numberChars, rng);
    result[3] = GetRandomChar(specialChars, rng);

    // 填充剩余字符
    for (int i = 4; i < length; i++)
    {
      result[i] = GetRandomChar(allChars, rng);
    }

    // 打乱顺序
    for (int i = length - 1; i > 0; i--)
    {
      var j = GetRandomInt(0, i + 1, rng);
      (result[i], result[j]) = (result[j], result[i]);
    }

    return new string(result);
  }

  private static char GetRandomChar(string chars, RandomNumberGenerator rng)
  {
    return chars[GetRandomInt(0, chars.Length, rng)];
  }

  private static int GetRandomInt(int minValue, int maxValue, RandomNumberGenerator rng)
  {
    var bytes = new byte[4];
    rng.GetBytes(bytes);
    var value = BitConverter.ToInt32(bytes);
    return Math.Abs(value % (maxValue - minValue)) + minValue;
  }
}