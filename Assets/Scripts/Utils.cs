using System;

namespace Scripts
{
    public static class Utils
    {
        public static Random Random { get; private set; }

        static Utils()
        {
            Random = new Random();
        }
    }
}