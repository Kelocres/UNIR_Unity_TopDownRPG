using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfo_Profe : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemCountText;

    private RectTransform myTransform;
    private Canvas myCanvas;
    private CanvasGroup canvasGroup;
    private Transform initParent;
    private Vector3 initPosition;

    public Transform InitParent { get => initParent; set => initParent = value; }

    private void Awake()
    {
        myCanvas = transform.root.GetComponent<Canvas>();
        myTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    internal void UseItem()
    {
        throw new NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initParent = myTransform.parent;
        initPosition = myTransform.localPosition;

        // Hay que desvincular el gameobject del padre y vincularlo al canvas 
        // para que no se muestre por detrás de los demás elementos de la UI
        myTransform.SetParent(myCanvas.transform);
        
        // Modificar el color
        canvasGroup.alpha = 0.5f;

        // Para que se detecte lo que hay debajo del gameobject arrastrado
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // eventData == puntero del ratón
        // el valor de eventData.delta (el arrastre del ratón) debe adaptarse
        // a la escala del canvas
        myTransform.anchoredPosition += eventData.delta / myCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Dropeo fallido
        if(myTransform.parent == myCanvas.transform)
        {
            myTransform.SetParent(initParent);
            myTransform.localPosition = initPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
