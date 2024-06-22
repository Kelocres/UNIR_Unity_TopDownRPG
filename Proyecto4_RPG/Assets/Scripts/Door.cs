using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private GameManagerSO gM;

    //[SerializeField] private int nextSceneIndex;
    [SerializeField] private string nextScene;
    [SerializeField] private Vector3 nextScenePosition;
    [SerializeField] private Vector2 nextSceneOrientation;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            gM.LoadNewScene(nextScenePosition, nextSceneOrientation, nextScene);
        }
    }
}
