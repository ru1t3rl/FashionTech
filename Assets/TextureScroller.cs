using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    // Scroll the main texture based on time

    public Vector2 scrollSpeed = new Vector2(0.4f, 0.4f);
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offsetX = Time.time * scrollSpeed.x;
        float offsetY = Time.time * scrollSpeed.y;
        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
