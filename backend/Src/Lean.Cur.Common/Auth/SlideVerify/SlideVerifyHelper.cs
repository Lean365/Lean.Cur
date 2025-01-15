using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace Lean.Cur.Common.Auth.SlideVerify;

/// <summary>
/// 滑块验证码工具类
/// </summary>
public class SlideVerifyHelper
{
  private readonly SlideVerifyOptions _options;
  private readonly IMemoryCache _cache;
  private readonly Random _random;

  public SlideVerifyHelper(IOptions<SlideVerifyOptions> options, IMemoryCache cache)
  {
    _options = options.Value;
    _cache = cache;
    _random = new Random();
  }

  /// <summary>
  /// 生成滑块验证码
  /// </summary>
  public SlideVerifyResult Generate()
  {
    // 获取随机背景图和滑块图
    var backgroundPath = GetRandomImage(_options.BackgroundDirectory);
    var sliderPath = GetRandomImage(_options.SliderDirectory);

    // 生成验证码ID
    var id = Guid.NewGuid().ToString("N");

    // 随机生成滑块的Y坐标
    var sliderY = _random.Next(_options.SliderHeight, _options.BackgroundHeight - _options.SliderHeight);

    // 使用SkiaSharp处理图片
    using var backgroundStream = File.OpenRead(backgroundPath);
    using var sliderStream = File.OpenRead(sliderPath);
    using var backgroundImage = SKBitmap.Decode(backgroundStream);
    using var sliderImage = SKBitmap.Decode(sliderStream);

    // 调整图片大小
    using var resizedBackground = ResizeImage(backgroundImage, _options.BackgroundWidth, _options.BackgroundHeight);
    using var resizedSlider = ResizeImage(sliderImage, _options.SliderWidth, _options.SliderHeight);

    // 随机生成滑块的X坐标
    var sliderX = _random.Next(_options.SliderWidth, _options.BackgroundWidth - _options.SliderWidth);

    // 将滑块位置缓存起来，用于验证
    _cache.Set(id, sliderX, TimeSpan.FromMinutes(_options.ExpiryMinutes));

    return new SlideVerifyResult
    {
      Id = id,
      BackgroundImage = ImageToBase64(resizedBackground),
      SliderImage = ImageToBase64(resizedSlider),
      BackgroundWidth = _options.BackgroundWidth,
      BackgroundHeight = _options.BackgroundHeight,
      SliderWidth = _options.SliderWidth,
      SliderHeight = _options.SliderHeight,
      SliderY = sliderY
    };
  }

  /// <summary>
  /// 验证滑块位置
  /// </summary>
  public bool Verify(string id, int x)
  {
    if (!_cache.TryGetValue<int>(id, out var correctX))
    {
      return false;
    }

    _cache.Remove(id);
    return Math.Abs(x - correctX) <= _options.AllowedDeviation;
  }

  private string GetRandomImage(string directory)
  {
    if (!Directory.Exists(directory))
    {
      throw new DirectoryNotFoundException($"Directory not found: {directory}");
    }

    var files = Directory.GetFiles(directory, "*.*")
        .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                   f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
        .ToArray();

    if (files.Length == 0)
    {
      throw new InvalidOperationException($"No image files found in directory: {directory}");
    }

    return files[_random.Next(files.Length)];
  }

  private SKBitmap ResizeImage(SKBitmap original, int width, int height)
  {
    var resized = new SKBitmap(width, height);
    using var canvas = new SKCanvas(resized);

    // 使用高质量的缩放
    canvas.Scale((float)width / original.Width, (float)height / original.Height);
    canvas.DrawBitmap(original, 0, 0);

    return resized;
  }

  private string ImageToBase64(SKBitmap image)
  {
    using var data = image.Encode(SKEncodedImageFormat.Png, 100);
    return Convert.ToBase64String(data.ToArray());
  }
}