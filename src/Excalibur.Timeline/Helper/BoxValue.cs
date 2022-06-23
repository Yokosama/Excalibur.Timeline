using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 值类型装箱
    /// </summary>
    public static class BoxValue
    {
        /// <summary>
        /// true
        /// </summary>
        public static readonly object True = true;
        /// <summary>
        /// false
        /// </summary>
        public static readonly object False = false;
        /// <summary>
        /// 0d
        /// </summary>
        public static readonly object Double0 = 0d;
        /// <summary>
        /// 1d
        /// </summary>
        public static readonly object Double1 = 1d;
        /// <summary>
        /// 5d
        /// </summary>
        public static readonly object Double5 = 5d;
        /// <summary>
        /// 10d
        /// </summary>
        public static readonly object Double10 = 10d;
        /// <summary>
        /// 12d
        /// </summary>
        public static readonly object Double12 = 12d;
        /// <summary>
        /// 15d
        /// </summary>
        public static readonly object Double15 = 15d;
        /// <summary>
        /// 16d
        /// </summary>
        public static readonly object Double16 = 16d;
        /// <summary>
        /// 20d
        /// </summary>
        public static readonly object Double20 = 20d;
        /// <summary>
        /// 25d
        /// </summary>
        public static readonly object Double25 = 25d;
        /// <summary>
        /// 20d
        /// </summary>
        public static readonly object Double40 = 40d;
        /// <summary>
        /// 1
        /// </summary>
        public static readonly object Int1 = 1;
        /// <summary>
        /// 10
        /// </summary>
        public static readonly object Int10 = 10;
        /// <summary>
        /// 100
        /// </summary>
        public static readonly object Int100 = 100;
        /// <summary>
        /// 1f
        /// </summary>
        public static readonly object Float1 = 1f;
        /// <summary>
        /// 10f
        /// </summary>
        public static readonly object Float10 = 10f;
        /// <summary>
        /// Default Point
        /// </summary>
        public static readonly object Point = default(Point);
        /// <summary>
        /// Default Rect
        /// </summary>
        public static readonly object Rect = default(Rect);
    }
}
