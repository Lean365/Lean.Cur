namespace Lean.Cur.Common.Configs
{
  public class LeanCorsSettings
  {
    public bool Enabled { get; set; }
    public List<string> Origins { get; set; } = new();
    public List<string> Methods { get; set; } = new();
    public List<string> Headers { get; set; } = new();
    public bool AllowCredentials { get; set; }
    public List<string> ExposedHeaders { get; set; } = new();
  }
}