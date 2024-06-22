using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    
    private float inputH;
    private float inputV;

    // Para que el movimiento sea de cuadr�cula en cuadr�cula
    private Vector3 puntoDestino;
    private bool moviendo;

    // Para detectar lo que hay en la cuadr�cula siguiente
    private Vector3 puntoInteraccion;
    private Vector3 ultimoInput;
    [SerializeField] private float radioInteraccion = 0.45f;

    // Recoger lo que hay en la pr�xima cuadr�cula
    [SerializeField] private LayerMask queEsColisionable;
    private Collider2D colliderDelante;

    [SerializeField] private float velocidadMovimiento = 1f;

    private Animator anim;

    // Para evitar que se mueva mientras est� interaccionando
    private bool interactuando = false;

    public bool Interactuando { get => interactuando; set => interactuando = value; }

    private void Start()
    {
        moviendo = false;
        anim = GetComponent<Animator>();

        //Cargar el jugador en la escena seg�n lo que est� guardado en GameManager
        transform.position = gM.InitPlayerPosition;
        anim.SetFloat("inputH", gM.InitPlayerRotation.x);
        anim.SetFloat("inputV", gM.InitPlayerRotation.y); //22:09
    }
    private void Update()
    {
        
        LecturaInputs();


        MovimientoYAnimaciones();

    }

    private void LecturaInputs()
    {
        //Para recoger el input pero evitando las diagonales
        if (inputV == 0)
            inputH = Input.GetAxisRaw("Horizontal");
        if (inputH == 0)
            inputV = Input.GetAxisRaw("Vertical");

        // Para interaccionar
        if (Input.GetKeyDown(KeyCode.E))
            LanzarInteraccion();
    }

    private void MovimientoYAnimaciones()
    {
        if (!interactuando && !moviendo && (inputH != 0 || inputV != 0))
        {
            // Animaci�n de caminar
            anim.SetBool("isMoving", true);
            anim.SetFloat("inputH", inputH);
            anim.SetFloat("inputV", inputV);

            // Actualizo cual fue mi �ltimo input, cual va a ser mi puntoDestino y cual es mi puntoInteracci�n
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;

            colliderDelante = LanzarCheck();
            if (colliderDelante == false)
                StartCoroutine(Mover());
        }

        // Animaci�n idle
        else if (inputH == 0 && inputV == 0)
            anim.SetBool("isMoving", false);
    }

    private void LanzarInteraccion()
    {
        //Comprobar qu� tenemos delante
        colliderDelante = LanzarCheck();
        if (colliderDelante == null) return;

        if(colliderDelante.gameObject.TryGetComponent(out Interactuable inter))
        {
           inter.Interactuar();
        }
    }

    // Moverse hasta la cuadr�cula indicada
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

        // Animaci�n idle
        //if (inputH == 0 && inputV == 0)
        //    anim.SetBool("isMoving", false);

    }

    private Collider2D LanzarCheck()
    {
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion, queEsColisionable);
        //return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion);
    }

    /*
    private void OnDrawGizmos()
    {
        //Debug.Log("Dibujando");
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }*/

    private void OnDestroy()
    {
        
    }
}
