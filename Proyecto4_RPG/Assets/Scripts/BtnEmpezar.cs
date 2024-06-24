using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnEmpezar : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;

    //[SerializeField] private int nextSceneIndex;
    [SerializeField] private string nextScene;
    [SerializeField] private Vector3 nextScenePosition;
    [SerializeField] private Vector2 nextSceneOrientation;

    // Update is called once per frame
    public void EmpezarPartida()
    {
        gM.LoadNewScene(nextScenePosition, nextSceneOrientation, nextScene);
    }
}
