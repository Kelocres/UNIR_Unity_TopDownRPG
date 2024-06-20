using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Mobile : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento = 1f;
    [SerializeField] private float tiempoEntreEsperas = 1f;
    [SerializeField] private float distanciaMaxima = 2f;

    private Vector3 posicionObjetivo;
    [SerializeField] private LayerMask queEsObstaculo;

    private Vector3 posicionInicial;
    private float radioDeteccion = 0.45f;

    private Animator anim;

    private void Awake()
    {
        posicionInicial = transform.position;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(IrHaciaDestinoYEsperar());
    }

    private IEnumerator IrHaciaDestinoYEsperar()
    {
        while (true)
        {
            //Calcular punto destino (si no se ha encontrado, esperamos un poco más)
            if (CalcularNuevoDestino())
            {
                anim.SetBool("isMoving", true);
                while (transform.position != posicionObjetivo)
                {
                    transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidadMovimiento * Time.deltaTime);
                    yield return null;
                }
                anim.SetBool("isMoving", false);
            }
            // Ha llegado a destino
            yield return new WaitForSeconds(tiempoEntreEsperas);
        }
    }

    private bool CalcularNuevoDestino()
    {
        bool tileLibre = false;
        int intentos = 0;
        while (!tileLibre && intentos < 9)
        {
            int prob = Random.Range(0, 4);
            if (prob == 0)
            {
                posicionObjetivo = transform.position + Vector3.left;
                anim.SetFloat("inputV", 0);
                anim.SetFloat("inputH", -1);
            }
            else if (prob == 1)
            {
                posicionObjetivo = transform.position + Vector3.right;
                anim.SetFloat("inputV", 0);
                anim.SetFloat("inputH", 1);
            }
            else if (prob == 2)
            {
                posicionObjetivo = transform.position + Vector3.up;
                anim.SetFloat("inputV", 1);
                anim.SetFloat("inputH", 0);
            }
            else
            {
                posicionObjetivo = transform.position + Vector3.down;
                anim.SetFloat("inputV", -1);
                anim.SetFloat("inputH", 0);
            }

            tileLibre = TileLibreYDentroDeDistancia();
            intentos++;
        }

        return tileLibre;
        
    }

    private bool TileLibreYDentroDeDistancia()
    {
        //Comprobar si se sale de la distancia máxima
        if (Vector3.Distance(posicionInicial, posicionObjetivo) > distanciaMaxima)
            return false;

        // Combrobar si el tile de destino está libre de obstáculos
        return !Physics2D.OverlapCircle(posicionObjetivo, radioDeteccion, queEsObstaculo);

    }
}
