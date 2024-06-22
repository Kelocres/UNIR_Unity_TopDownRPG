using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot_Profe : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemCountText;

    [SerializeField] private Transform backGround;

    private ItemInfo_Profe currentInfo;
    private int count;

    private void Awake()
    {
        currentInfo = GetComponentInChildren<ItemInfo_Profe>();
    }

    public void FeedData(ItemSO_Profe item)
    {
        image.sprite = item.icon;
        itemNameText.text = item.itemName;
        UpdateStackItem();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Obtengo el itemInfo de aquello que me droppean
        ItemInfo_Profe newItemInfo = eventData.pointerDrag.GetComponent<ItemInfo_Profe>();
        Transform itemTransform = eventData.pointerDrag.transform;

        if(backGround.childCount > 0)  //Significa que ya tengo información acoplada
        {
            //El que tenia acoplado la paso al padre del nuevo que me traen
            currentInfo.transform.SetParent(newItemInfo.InitParent);
            currentInfo.transform.localPosition = Vector3.zero;

            newItemInfo.InitParent.GetComponentInParent<ItemSlot_Profe>().currentInfo = currentInfo;
        }
        
        itemTransform.SetParent(backGround);
        itemTransform.localPosition = Vector3.zero;

        currentInfo = newItemInfo;
    }

    public void UpdateStackItem()
    {
        count++;
        itemCountText.text = "x" + count;
    }
}
