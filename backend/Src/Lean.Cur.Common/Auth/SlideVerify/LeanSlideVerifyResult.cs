namespace Lean.Cur.Common.Auth.SlideVerify;

/// <summary>
/// 滑块验证码结果
/// </summary>
public class LeanSlideVerifyResult
{
  /// <summary>
  /// 验证码ID
  /// </summary>
  public string Id { get; set; } = string.Empty;

  /// <summary>
  /// 背景图片（Base64）
  /// </summary>
  public string BackgroundImage { get; set; } = string.Empty;

  /// <summary>
  /// 滑块图片（Base64）
  /// </summary>
  public string SliderImage { get; set; } = string.Empty;

  /// <summary>
  /// 背景图片宽度
  /// </summary>
  public int BackgroundWidth { get; set; }

  /// <summary>
  /// 背景图片高度
  /// </summary>
  public int BackgroundHeight { get; set; }

  /// <summary>
  /// 滑块图片宽度
  /// </summary>
  public int SliderWidth { get; set; }

  /// <summary>
  /// 滑块图片高度
  /// </summary>
  public int SliderHeight { get; set; }

  /// <summary>
  /// 滑块初始位置Y坐标
  /// </summary>
  public int SliderY { get; set; }
}