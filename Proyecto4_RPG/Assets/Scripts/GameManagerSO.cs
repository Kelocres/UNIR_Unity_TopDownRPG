using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Scriptable Objects/GameManager")]
public class GameManagerSO : ScriptableObject
{
    private Player player;
    private SistemaInventario inventario;
    public SistemaInventario Inventario { get => inventario; }


    //Para conservar la informaci�n del jugador de una escena a otra
    [SerializeField] private Vector3 initPlayerPosition = new Vector3(-1.5f, -7.5f, 0);
    [SerializeField] private Vector2 initPlayerRotation = new Vector2(0, -1);
    public Vector3 InitPlayerPosition { get => initPlayerPosition; set => initPlayerPosition = value; }
    public Vector2 InitPlayerRotation { get => initPlayerRotation; set => initPlayerRotation = value; }


    //Para controlar qu� items no deben aparecer en la escena:
    [NonSerialized] private Dictionary<int, bool> items = new Dictionary<int, bool>();
    public Dictionary<int, bool> Items { get => items; set => items = value; }

    private void OnEnable() //Llamadas por evento
    {
        //Para vincular elementos cuando se termine de cargar la escena
        SceneManager.sceneLoaded += NuevaEscenaCargada;
    }

    private void NuevaEscenaCargada(Scene arg0, LoadSceneMode arg1)
    {
        player = FindObjectOfType<Player>();
        inventario = FindObjectOfType<SistemaInventario>();
    }

    public void CambiarEstadoPlayer(bool estado)
    {
        player.Interactuando = estado;
    }

    public void LoadNewScene(Vector3 nextScenePosition, Vector2 nextSceneOrientation, string nextScene)
    {
        initPlayerPosition = nextScenePosition;
        initPlayerRotation = nextSceneOrientation;

        SceneManager.LoadScene(nextScene);
    }
}
