using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Profe : MonoBehaviour, Interactuable
{
    [SerializeField] private ItemSO_Profe misDatos;
    [SerializeField] private GM_Profe gameManager;

    //Para evitar que aparezca en la escena si ya ha sido cogido
    public int id;

    public ItemSO_Profe MisDatos { get => misDatos; set => misDatos = value; }

    void Start()
    {
        if (gameManager.Items.ContainsKey(id))
        {
            // Si está apuntado como false, destruir (ya ha sido cogido)
            if (gameManager.Items[id] == false) Destroy(gameObject);
        }
        else
        {
            // Insertar item en la lista
            gameManager.Items.Add(id, true);
        }
    }

    public void Interactuar()
    {
        //gameManager.Inventario.NuevoItem(misDatos);
        gameManager.NewItem(misDatos);
        Destroy(gameObject);
    }

    internal void Obtain()
    {
        throw new NotImplementedException();
    }
}
