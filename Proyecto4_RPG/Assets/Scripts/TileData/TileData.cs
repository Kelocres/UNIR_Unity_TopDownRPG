using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    // Los tiles que ser�n afectados
    public TileBase[] tiles;

    public float walkingSpeed;

    public bool slippery = false;
    public bool fall = false;
}
