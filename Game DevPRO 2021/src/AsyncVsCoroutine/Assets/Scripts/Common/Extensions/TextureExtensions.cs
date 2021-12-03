using UnityEngine;

namespace Common.Extensions
{
    public static class TextureExtensions
    {
        public static void Release(this Texture texture)
        {
            if (texture != null)
            {
                Object.Destroy(texture);
            }
        }
    }
}