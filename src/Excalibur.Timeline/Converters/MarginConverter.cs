using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Excalibur.Timeline.Converters
{
    /// <summary>
    /// double数值转换为边距
    /// </summary>
    public class MarginConverter : IValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">边距模式</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || value == null)
            {
                return value;
            }

            try
            {
                var v = double.Parse(value.ToString());
                double left = 0d, right = 0d, top = 0d, bottom = 0d;
 
                var parameters = parameter.ToString().Split('|');
                foreach (var p in parameters)
                {
                    var mf = p.Split(',');
                    if(mf.Length >= 1)
                    {
                        var mode = (MarginMode)Enum.Parse(typeof(MarginMode), mf[0].Trim());
                        var offset = 0d;
                        if (mf.Length >= 2 && double.TryParse(mf[1].Trim(), out offset))
                        {
                        }

                        if (mode ==  MarginMode.Left)
                        {
                            left = v + offset;
                        }
                        if (mode == MarginMode.Right)
                        {
                            right = v + offset;
                        }
                        if (mode == MarginMode.Top)
                        {
                            top = v + offset;
                        }
                        if (mode == MarginMode.Bottom)
                        {
                            bottom = v + offset;
                        }
                    }
                }

                return new Thickness(left, top, right, bottom);
            }
            catch { }
            return value;
        }

        /// <summary>
        /// ignore
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// 边距模式
    /// </summary>
    [Flags]
    public enum MarginMode
    {
        /// <summary>
        /// 左
        /// </summary>
        Left = 1,
        /// <summary>
        /// 右
        /// </summary>
        Right = 2,
        /// <summary>
        /// 上
        /// </summary>
        Top = 4,
        /// <summary>
        /// 下
        /// </summary>
        Bottom = 8
    }
}
