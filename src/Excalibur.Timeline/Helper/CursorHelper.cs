using System.IO;
using System.Reflection;
using System.Windows.Input;
using System.Resources;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 鼠标辅助类
    /// </summary>
    public static class CursorHelper
    {
        private static ResourceManager resourceManager;

        /// <summary>
        /// 获取鼠标
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static Cursor GetCursor(string path)
        {
            var a = Assembly.GetExecutingAssembly();
            if(resourceManager == null)
                resourceManager = new ResourceManager(a.GetName().Name + ".g", a);
            using (Stream s = resourceManager.GetStream(path.ToLowerInvariant()))
            {
                return new Cursor(s);
            }
        }
    }
}
