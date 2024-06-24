using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private bool empezarConGM = false;
    
    private float inputH;
    private float inputV;

    // Para que el movimiento sea de cuadr�cula en cuadr�cula
    private Vector3 puntoDestino;
    private bool moviendo;

    // Cuando el personaje est� en suelo resbaladizo
    private bool resbaladizo = false;

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

    // Para ser afectado por los tiles del suelo
    private MapManager mapManager;

    //Si el jugador muere
    [SerializeField] private string gameOverScene;
    private bool estaMuerto = false;

    private void Start()
    {
        moviendo = false;
        anim = GetComponent<Animator>();

        //Cargar el jugador en la escena seg�n lo que est� guardado en GameManager
        if (empezarConGM)
        {
            transform.position = gM.InitPlayerPosition;
            anim.SetFloat("inputH", gM.InitPlayerRotation.x);
            anim.SetFloat("inputV", gM.InitPlayerRotation.y); //22:09
        }

        mapManager = FindObjectOfType<MapManager>();
    }
    private void Update()
    {
        if (estaMuerto) return;

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
        
        while (transform.position != puntoDestino && !estaMuerto)
        {
            float tileSpeed = 1f;
            TileData currentTile = mapManager?.GetTileInfo(transform.position);
            bool fall = false;

            if (currentTile != null)
            {
                resbaladizo = currentTile.slippery;
                tileSpeed = currentTile.walkingSpeed;
                fall = currentTile.fall;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * tileSpeed * Time.deltaTime);

            // Si se ha llegado al punto de destino 
            // PERO!
            // el suelo es resbaladizo y no hemos chocado con nada 
            if (transform.position == puntoDestino)
            {
                puntoInteraccion = transform.position + ultimoInput;
                if (resbaladizo && LanzarCheck() == null )
                //if (resbaladizo)
                    puntoDestino = puntoInteraccion;
                
                if (fall)
                    MuerteJugador();
            }
            
            yield return null;
        }
        // Ante un nuevo destino, necesito refrescar
        //puntoInteraccion = transform.position + ultimoInput;
        moviendo = false;

    }



    public void MuerteJugador()
    {
        //Parar movimiento
        //StopAllCoroutines(); // Esta l�nea aqu� mata la funci�n
        //Animaci�n de muerte por trampa o ca�da

        //Muerte
        estaMuerto = true;
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(EsparaParaCargarPorMuerte());
    }

    private IEnumerator EsparaParaCargarPorMuerte()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(gameOverScene);
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
