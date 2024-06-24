using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palanca : MonoBehaviour, Interactuable
{
    public delegate void DelPalanca(bool estado);
    public event DelPalanca delPalanca;

    [SerializeField] private bool estadoPalanca = false;
    [SerializeField] private Sprite palancaEnTrue;
    [SerializeField] private Sprite palancaEnFalse;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void SpriteSegunEstado()
    {
        if (estadoPalanca)
            spriteRenderer.sprite = palancaEnTrue;
        else
            spriteRenderer.sprite = palancaEnFalse;
    }

    public void Interactuar()
    {
        estadoPalanca = !estadoPalanca;
        SpriteSegunEstado();
        if (delPalanca != null) delPalanca(estadoPalanca);
    }
}
