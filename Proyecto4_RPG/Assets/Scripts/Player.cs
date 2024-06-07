using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float inputH;
    private float inputV;

    // Para que el movimiento sea de cuadrícula en cuadrícula
    private Vector3 puntoDestino;
    private bool moviendo;

    // Para detectar lo que hay en la cuadrícula siguiente
    private Vector3 puntoInteraccion;
    private Vector3 ultimoInput;
    [SerializeField] private float radioInteraccion = 0.45f;

    // Recoger lo que hay en la próxima cuadrícula
    [SerializeField] private LayerMask queEsColisionable;
    private Collider2D colliderDelante;

    [SerializeField] private float velocidadMovimiento = 1f;

    private void Start()
    {
        moviendo = false;
    }
    private void Update()
    {
        //Para recoger el input pero evitando las diagonales
        if(inputV==0)
            inputH = Input.GetAxisRaw("Horizontal");
        if(inputH==0)
            inputV = Input.GetAxisRaw("Vertical");

        if (!moviendo && (inputH != 0 || inputV != 0))
        {
            // Actualizo cual fue mi último input, cual va a ser mi puntoDestino y cual es mi puntoInteracción
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;

            colliderDelante = LanzarCheck();
            if(colliderDelante == false)
                StartCoroutine(Mover());
        }

    }

    // Moverse hasta la cuadrícula indicada
    IEnumerator Mover()
    {
        moviendo = true;
        
        while (transform.position != puntoDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }
        // Ante un nuevo destino, necesito refrescar
        puntoInteraccion = transform.position + ultimoInput;
        moviendo = false;
    }

    private Collider2D LanzarCheck()
    {
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion, queEsColisionable);
    }

    /*
    private void OnDrawGizmos()
    {
        //Debug.Log("Dibujando");
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }*/
}
