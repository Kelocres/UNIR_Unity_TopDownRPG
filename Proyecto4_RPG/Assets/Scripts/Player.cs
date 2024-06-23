using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private bool empezarConGM = false;
    
    private float inputH;
    private float inputV;

    // Para que el movimiento sea de cuadrícula en cuadrícula
    private Vector3 puntoDestino;
    private bool moviendo;

    // Cuando el personaje esté en suelo resbaladizo
    private bool resbaladizo = false;

    // Para detectar lo que hay en la cuadrícula siguiente
    private Vector3 puntoInteraccion;
    private Vector3 ultimoInput;
    [SerializeField] private float radioInteraccion = 0.45f;

    // Recoger lo que hay en la próxima cuadrícula
    [SerializeField] private LayerMask queEsColisionable;
    private Collider2D colliderDelante;

    [SerializeField] private float velocidadMovimiento = 1f;

    private Animator anim;

    // Para evitar que se mueva mientras esté interaccionando
    private bool interactuando = false;

    public bool Interactuando { get => interactuando; set => interactuando = value; }

    // Para ser afectado por los tiles del suelo
    private MapManager mapManager;

    private void Start()
    {
        moviendo = false;
        anim = GetComponent<Animator>();

        //Cargar el jugador en la escena según lo que esté guardado en GameManager
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
            // Animación de caminar
            anim.SetBool("isMoving", true);
            anim.SetFloat("inputH", inputH);
            anim.SetFloat("inputV", inputV);

            // Actualizo cual fue mi último input, cual va a ser mi puntoDestino y cual es mi puntoInteracción
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;

            colliderDelante = LanzarCheck();
            if (colliderDelante == false)
                StartCoroutine(Mover());
        }

        // Animación idle
        else if (inputH == 0 && inputV == 0)
            anim.SetBool("isMoving", false);
    }

    private void LanzarInteraccion()
    {
        //Comprobar qué tenemos delante
        colliderDelante = LanzarCheck();
        if (colliderDelante == null) return;

        if(colliderDelante.gameObject.TryGetComponent(out Interactuable inter))
        {
           inter.Interactuar();
        }
    }

    // Moverse hasta la cuadrícula indicada
    IEnumerator Mover()
    {
        moviendo = true;
        
        while (transform.position != puntoDestino)
        {
            float tileSpeed = 1f;
            TileData currentTile = mapManager.GetTileInfo(transform.position);

            if (currentTile != null)
            {
                if (currentTile.fall) Caida();

                resbaladizo = currentTile.slippery;
                tileSpeed = currentTile.walkingSpeed;
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
            }
            
            yield return null;
        }
        // Ante un nuevo destino, necesito refrescar
        //puntoInteraccion = transform.position + ultimoInput;
        moviendo = false;

    }

    private void Caida()
    {
        //Parar movimiento
        StopAllCoroutines();
        //Animación de caída
        //Muerte
        Destroy(gameObject);

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
