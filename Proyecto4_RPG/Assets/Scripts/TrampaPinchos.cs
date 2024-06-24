using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EstadoTrampa
{
    A_Guardado,
    B_Saliendo,
    C_Fuera

}
public class TrampaPinchos : MonoBehaviour
{


    [SerializeField] private Sprite imagenGuardado;
    [SerializeField] private float tiempoGuardado;
    [SerializeField] private Sprite imagenSaliendo;
    [SerializeField] private float tiempoSaliendo;
    [SerializeField] private Sprite imagenFuera;
    [SerializeField] private float tiempoFuera;

    private SpriteRenderer spriteRenderer;
    private EstadoTrampa estado;
    [SerializeField] private EstadoTrampa estadoInicio;
    private Collider2D collidertrampa;
    private IEnumerator corutina;

    [SerializeField] private Palanca palanca;
    [SerializeField] private bool activarEnTrue;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collidertrampa = GetComponent<Collider2D>();
        corutina = Cambiando();

        if (palanca != null) palanca.delPalanca += SenyalPalanca;

        
        //EmpezarCorutina();
    }

    public void SenyalPalanca(bool estadoPalanca)
    {
        if((estadoPalanca && activarEnTrue) || (!estadoPalanca && !activarEnTrue))
        {
            EmpezarCorutina();
        }
        else
        {
            PararCorutina();
        }
    }

    public void EmpezarCorutina()
    {
        CambiarEstado(estadoInicio);
        StartCoroutine(corutina);
    }

    public void PararCorutina()
    {
        StopCoroutine(corutina);
        CambiarAGuardado();
    }

    // Update is called once per frame
    private IEnumerator Cambiando()
    {
        while(true)
        {
            yield return new WaitForSeconds(TiempoEstadoActual());
            SiguienteEstado();
        }
    }

    private void SiguienteEstado()
    {

        //Debug.Log("SiguienteEstado()");
        if (estado == EstadoTrampa.A_Guardado && tiempoSaliendo > 0)
            CambiarASaliendo();

        else if (estado == EstadoTrampa.B_Saliendo && tiempoFuera > 0)
            CambiarAFuera(); 

        else if (estado == EstadoTrampa.C_Fuera && tiempoGuardado > 0)
            CambiarAGuardado();
    }

    public void CambiarEstado(EstadoTrampa estado)
    {
        if (estado == EstadoTrampa.A_Guardado)
            CambiarAGuardado();

        else if (estado == EstadoTrampa.B_Saliendo)
            CambiarASaliendo();

        else if (estado == EstadoTrampa.C_Fuera)
            CambiarAFuera();
    }

    public float TiempoEstadoActual()
    {
        //Debug.Log("TiempoEstadoActual");
        if (estado == EstadoTrampa.A_Guardado)
            return tiempoGuardado;

        if (estado == EstadoTrampa.B_Saliendo)
            return tiempoSaliendo;

        if (estado == EstadoTrampa.C_Fuera)
            return tiempoFuera;

        return 0f;
    }

    private float CambiarAGuardado()
    {
        //Debug.Log("Guardado");
        estado = EstadoTrampa.A_Guardado;
        spriteRenderer.sprite = imagenGuardado;
        collidertrampa.enabled = false;
        return tiempoGuardado;
    }

    private float CambiarASaliendo()
    {
        //Debug.Log("Saliendo");
        estado = EstadoTrampa.B_Saliendo;
        spriteRenderer.sprite = imagenSaliendo;
        collidertrampa.enabled = false;
        return tiempoSaliendo;
    }

    private float CambiarAFuera()
    {
        //Debug.Log("Ataque");
        estado = EstadoTrampa.C_Fuera;
        spriteRenderer.sprite = imagenFuera;
        
        Collider2D collision = Physics2D.OverlapCircle(transform.position, 0.45f);
        if (collision!=null && collision.CompareTag("Player"))
              collision.GetComponent<Player>().MuerteJugador();

        collidertrampa.enabled = true;
        collidertrampa.isTrigger = true;
        return tiempoFuera;
    }



}
