using System;

namespace Lean.Cur.Common.Excel
{
    /// <summary>
    /// Excel列特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LeanExcelColumnAttribute : Attribute
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 列宽
        /// </summary>
        public double Width { get; set; } = 20;

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// 是否允许导出
        /// </summary>
        public bool AllowExport { get; set; } = true;

        /// <summary>
        /// 是否允许导入
        /// </summary>
        public bool AllowImport { get; set; } = true;

        /// <summary>
        /// 错误提示
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 表头样式
        /// </summary>
        public LeanExcelHeaderStyle? HeaderStyle { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        public LeanExcelColumnAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="order">排序</param>
        public LeanExcelColumnAttribute(string name, int order)
        {
            Name = name;
            Order = order;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="order">排序</param>
        /// <param name="required">是否必填</param>
        public LeanExcelColumnAttribute(string name, int order, bool required)
        {
            Name = name;
            Order = order;
            Required = required;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="order">排序</param>
        /// <param name="required">是否必填</param>
        /// <param name="errorMessage">错误提示</param>
        public LeanExcelColumnAttribute(string name, int order, bool required, string errorMessage)
        {
            Name = name;
            Order = order;
            Required = required;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Excel表头样式
    /// </summary>
    public class LeanExcelHeaderStyle
    {
        /// <summary>
        /// 字体大小
        /// </summary>
        public float FontSize { get; set; } = 11;

        /// <summary>
        /// 是否加粗
        /// </summary>
        public bool IsBold { get; set; } = true;

        /// <summary>
        /// 背景颜色（HTML颜色代码）
        /// </summary>
        public string BackgroundColor { get; set; } = "#CCCCCC";

        /// <summary>
        /// 字体颜色（HTML颜色代码）
        /// </summary>
        public string FontColor { get; set; } = "#000000";
    }
} 