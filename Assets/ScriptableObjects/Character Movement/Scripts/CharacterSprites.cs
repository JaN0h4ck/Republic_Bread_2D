using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteData", menuName = "Sprites/CharacterSprites")]
public class CharacterSprites : ScriptableObject
{
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;
    public Sprite upLeft;
    public Sprite upRight;
    public Sprite downLeft;
    public Sprite downRight;
}
