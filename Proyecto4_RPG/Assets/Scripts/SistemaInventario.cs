using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SistemaInventario : MonoBehaviour
{
    [SerializeField] private GameObject marcoInventario;
    [SerializeField] private Button[] botones;

    private int itemsDisponibles;

    private void Start()
    {
        itemsDisponibles = 0;
        for(int i=0; i< botones.Length; i++)
        {
            //Para prevenir errores de copia o referencia
            int indiceBoton = i;

            botones[i].onClick.AddListener(() => BotonClickado(indiceBoton));
        }
    }

    private void BotonClickado(int indiceBoton)
    {
        //
        Debug.Log("Boton clickado " + indiceBoton);
    }

    public void NuevoItem(ItemSO datosItem)
    {
        botones[itemsDisponibles].gameObject.SetActive(true);
        botones[itemsDisponibles].GetComponent<Image>().sprite = datosItem.icono;
        itemsDisponibles++;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            /*if(marcoInventario.activeSelf)
            {
                marcoInventario.SetActive(false);
            }
            else
            {
                marcoInventario.SetActive(true);
            }   */

            marcoInventario.SetActive(!marcoInventario.activeSelf);
        }
    }
}
