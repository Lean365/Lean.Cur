using System.Text.Json.Serialization;

namespace Lean.Cur.Common.Models;

/// <summary>
/// 通用选项模型
/// </summary>
public class LeanOptionModel
{
  /// <summary>
  /// 选项标签
  /// </summary>
  [JsonPropertyName("label")]
  public string Label { get; set; } = string.Empty;

  /// <summary>
  /// 选项值
  /// </summary>
  [JsonPropertyName("value")]
  public string Value { get; set; } = string.Empty;
}