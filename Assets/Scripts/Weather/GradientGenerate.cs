using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace VRolijk
{
    [ExecuteInEditMode]
    public class GradientGenerate : MonoBehaviour
    {
        public Material material;
        public string texturePorperty;
        public bool realtimeGeneration;
        public UnityEngine.Gradient lutGradient;
        public Vector2Int lutTextureSize;
        public Texture2D lutTexture;

        private void Update()
        {
            if (realtimeGeneration)
            {
                GenerateLutTexture();
            }
        }

        public void GenerateLutTexture()
        {
            lutTexture = new Texture2D(lutTextureSize.x, lutTextureSize.y)
            { wrapMode = TextureWrapMode.Clamp };
            for (int x = 0; x < lutTextureSize.x; x++)
            {
                var color = lutGradient.Evaluate(x / (float)lutTextureSize.x);
                for (int y = 0; y < lutTextureSize.y; y++)
                {
                    lutTexture.SetPixel(x, y, color);
                }
            }

            lutTexture.Apply();
            material.SetTexture(texturePorperty, lutTexture);
        }
    }
}
