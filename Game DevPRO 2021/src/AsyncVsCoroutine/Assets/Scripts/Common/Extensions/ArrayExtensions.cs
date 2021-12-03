using System;

namespace Common.Extensions
{
    public static class ArrayExtensions
    {
        public static void Clear<T>(this T[] array)
        {
            Array.Clear(array, 0, array.Length);
        }
    }
}