using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Excalibur.Timeline.Helper
{
    /// <summary>
    /// 值帮助类
    /// </summary>
    public static class ValueHelper
    {
        /// <summary>
        /// 获取double类型数据的小数点后位数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int GetDecimalPlaces(this double value)
        {
            CultureInfo info = CultureInfo.CurrentCulture;

            char[] sep = info.NumberFormat.NumberDecimalSeparator.ToCharArray();

            string[] segments = value.ToString().Split(sep);

            switch (segments.Length)
            {
                case 1:
                    return 0;
                case 2:
                    return segments[1].Length;
                default:
                    throw new Exception("GetDecimalPlaces Failed!");
            }
        }
    }
}
