using System.Text;

namespace Main.Scripts.Utilities
{
    public static class StringHelper
    {
        private static readonly StringBuilder _builder = new();
        public static StringBuilder CreateNewBuild()
        {
            _builder.Clear();
            return _builder;
        }
    }
}