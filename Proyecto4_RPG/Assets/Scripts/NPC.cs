using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour, Interactuable
{
    [SerializeField] private GameManagerSO gameManager;

    // De 1 a 5 líneas
    [SerializeField, TextArea(1, 5)] private string[] lineas;
    [SerializeField] private float tiempoEntreLetras;
    [SerializeField] private GameObject cuadroDialogo;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    
    private bool hablando = false;
    private int indiceActual = -1;



    void Start()
    {
        
    }

    public void Interactuar()
    {
        gameManager.CambiarEstadoPlayer(true);
        cuadroDialogo.SetActive(true);

        // Si no se ha empezado a escribir, empezará la corutina
        if (!hablando)
            SiguienteFrase();

        // Si ya está escribiendo, se escribirá toda la línea al momento
        else
            CompletarLinea();
    }

    private void SiguienteFrase()
    {
        indiceActual++;
        if (indiceActual >= lineas.Length)
        {
            TerminarDialogo();
        }
        else
            StartCoroutine(EscribirFrase());
    }

    IEnumerator EscribirFrase()
    {
        hablando = true;
        textoDialogo.text = "";
        char[] caracteresFrase = lineas[indiceActual].ToCharArray();
        foreach (char caracter in caracteresFrase)
        {
            textoDialogo.text += caracter;
            yield return new WaitForSeconds(tiempoEntreLetras);
        }
        hablando = false;
    }

    private void CompletarLinea()
    {
        StopAllCoroutines();
        textoDialogo.text = lineas[indiceActual];
        hablando = false;
    }

    private void TerminarDialogo()
    {
        hablando = false;
        textoDialogo.text = "";
        indiceActual = -1;
        cuadroDialogo.SetActive(false);

        gameManager.CambiarEstadoPlayer(false);
    }
}
