using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CursorTextures", menuName = "Sprites/CursorTextures")]
public class CursorTextureObject : ScriptableObject
{
    [SerializeField]
    private Texture2D defaultCursor;

    [SerializeField]
    private Texture2D hoverCursor;

    [SerializeField]
    private Vector2 hotspot;

    public Vector2 Hotspot { get => hotspot; }

    public Texture2D DefaultCursor { get => defaultCursor; }

    public Texture2D HoverCursor { get => hoverCursor; }
}
