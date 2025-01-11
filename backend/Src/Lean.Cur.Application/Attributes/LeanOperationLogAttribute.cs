namespace Lean.Cur.Application.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class LeanOperationLogAttribute : Attribute
{
  public string Module { get; }
  public string Operation { get; }

  public LeanOperationLogAttribute(string module, string operation)
  {
    Module = module;
    Operation = operation;
  }
}