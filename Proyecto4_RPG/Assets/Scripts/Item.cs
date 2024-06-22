using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Interactuable
{
    [SerializeField] private ItemSO misDatos;
    [SerializeField] private GameManagerSO gameManager;

    //Para evitar que aparezca en la escena si ya ha sido cogido
    public int id;
    
    void Start()
    {
        if(gameManager.Items.ContainsKey(id))
        {
            // Si est� apuntado como false, destruir (ya ha sido cogido)
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
        gameManager.Inventario.NuevoItem(misDatos);
        Destroy(gameObject);
    }

    
}
