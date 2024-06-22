using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Scriptable Objects/GM_Profe")]
public class GM_Profe : ScriptableObject
{
    private Player_Profe player;
    private SistemaInventarioProfe inventario;
    public SistemaInventarioProfe Inventario { get => inventario; }


    //Para conservar la información del jugador de una escena a otra
    // NonSerialized: Para que los datos almacenados en este GameManager se reseteen cuando quites el play
    [NonSerialized] private Vector3 initPlayerPosition = new Vector3(-1.5f, -7.5f, 0);
    [NonSerialized] private Vector2 initPlayerRotation = new Vector2(0, -1);
    public Vector3 InitPlayerPosition { get => initPlayerPosition; set => initPlayerPosition = value; }
    public Vector2 InitPlayerRotation { get => initPlayerRotation; set => initPlayerRotation = value; }


    //Para controlar qué items no deben aparecer en la escena:
    [NonSerialized] private Dictionary<int, bool> items = new Dictionary<int, bool>();
    public Dictionary<int, bool> Items { get => items; set => items = value; }
    

    [NonSerialized] private int monedas;
    public int Monedas { get => monedas; set => monedas = value; }

    // Evento cuando el jugador coge  un item
    public event Action<ItemSO_Profe> OnNewItem;

    private void OnEnable() //Llamadas por evento
    {
        //Para vincular elementos cuando se termine de cargar la escena
        SceneManager.sceneLoaded += NuevaEscenaCargada;
    }

    private void NuevaEscenaCargada(Scene arg0, LoadSceneMode arg1)
    {
        player = FindObjectOfType<Player_Profe>();
        inventario = FindObjectOfType<SistemaInventarioProfe>();
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

    internal void NewItem(ItemSO_Profe item)
    {
        // El gm avisa de que un item ha sido cogido
        OnNewItem?.Invoke(item);
    }
}
