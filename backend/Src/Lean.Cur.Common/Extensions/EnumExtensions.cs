namespace Lean.Cur.Common.Extensions;

/// <summary>
/// 枚举扩展方法
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>描述</returns>
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();
        
        var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute?.Description ?? value.ToString();
    }

    /// <summary>
    /// 获取枚举值
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>值</returns>
    public static int GetValue(this Enum value)
    {
        return Convert.ToInt32(value);
    }
} 