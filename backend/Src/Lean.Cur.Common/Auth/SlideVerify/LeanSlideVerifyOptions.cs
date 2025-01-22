using System.Drawing;

namespace Lean.Cur.Common.Auth.SlideVerify;

/// <summary>
/// 滑块验证码配置选项
/// </summary>
public class LeanSlideVerifyOptions
{
  /// <summary>
  /// 背景图片目录
  /// </summary>
  public string BackgroundDirectory { get; set; } = "wwwroot/images/slide-verify/background";

  /// <summary>
  /// 滑块图片目录
  /// </summary>
  public string SliderDirectory { get; set; } = "wwwroot/images/slide-verify/slider";

  /// <summary>
  /// 背景图片宽度
  /// </summary>
  public int BackgroundWidth { get; set; } = 300;

  /// <summary>
  /// 背景图片高度
  /// </summary>
  public int BackgroundHeight { get; set; } = 150;

  /// <summary>
  /// 滑块图片宽度
  /// </summary>
  public int SliderWidth { get; set; } = 50;

  /// <summary>
  /// 滑块图片高度
  /// </summary>
  public int SliderHeight { get; set; } = 50;

  /// <summary>
  /// 验证码有效期（分钟）
  /// </summary>
  public int ExpiryMinutes { get; set; } = 5;

  /// <summary>
  /// 允许的误差范围（像素）
  /// </summary>
  public int AllowedDeviation { get; set; } = 5;
}