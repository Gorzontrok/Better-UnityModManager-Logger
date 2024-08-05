using UnityEngine;

namespace BetterUnityModManagerLogger
{
    internal class CreateTexture
    {
        public static Texture WithColor(Color color, int size = 2)
        {
            var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
            Color[] colors = new Color[tex.width * tex.height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            tex.SetPixels(colors);
            return tex;
        }
    }
}
