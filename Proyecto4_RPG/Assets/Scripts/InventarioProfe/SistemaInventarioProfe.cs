using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaInventarioProfe : MonoBehaviour
{
    [SerializeField] private GM_Profe gM;
    
    [SerializeField] private GameObject marco;

    private List<ItemSO_Profe> items = new List<ItemSO_Profe>();
    
    [SerializeField] private ItemSlot_Profe[] slots;
    [SerializeField] private ItemSlot_Profe[] usableSlots;
    private int itemsCollected = 0;

    private void OnEnable()
    {
        gM.OnNewItem += NuevoItem;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            marco.SetActive(!marco.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) // 
        {
            //Utilizar item
            usableSlots[0].GetComponentInChildren<ItemInfo_Profe>().UseItem();
        }
    }

    internal void NuevoItem(ItemSO_Profe item)
    {
        if (items.Contains(item))
        {
            // Encontrar el índice en el que existe el item repetido
            int index = items.FindIndex(x => x.Equals(item));
            slots[index].UpdateStackItem();
        }
        else
        {
            items.Add(item);
            slots[itemsCollected].gameObject.SetActive(true);
            slots[itemsCollected].FeedData(item);
            itemsCollected++;
        }
    }
}
