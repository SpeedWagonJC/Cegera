using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 6f;
    public float fuerzaSalto = 12f;

    [Header("Wall Jump")]
    public float fuerzaSaltoParedX = 8f;
    public float fuerzaSaltoParedY = 12f;
    public float tiempoBloqueoMovimiento = 0.2f;

    [Header("Wall Slide")]
    public float velocidadDeslizPared = 1.5f;

    [Header("Detección")]
    public Transform puntoSuelo;
    public Transform puntoPared;
    public float radioChequeo = 0.2f;
    public LayerMask capaSuelo;
    public LayerMask capaPared;

    [Header("Vida")]
    public int vidaMaxima = 100;
    public int vidaActual;

    [Header("UI Vida")]
    public TextMeshProUGUI textoVida;

    [Header("Escena al morir")]
    public string escenaMuerte = "GameOver";

    [Header("Sonidos")]
    public AudioClip sonidoCaminar;
    public AudioClip sonidoSalto;
    public AudioClip sonidoWallJump;
    public AudioClip sonidoGolpe;

    Rigidbody2D rb;
    Animator anim;

    AudioSource audioPasos;
    AudioSource audioFX;

    float movimiento;
    bool enSuelo;
    bool enPared;
    bool deslizPared;
    bool puedeMoverse = true;
    bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        AudioSource[] audios = GetComponents<AudioSource>();
        audioPasos = audios[0];
        audioFX = audios[1];

        vidaActual = vidaMaxima;
        ActualizarVidaUI();
    }

    void Update()
    {
        ChequearColisiones();
        Movimiento();
        WallSlide();
        Salto();
        Animaciones();
    }


    void Movimiento()
    {
        if (!puedeMoverse) return;

        movimiento = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);

        if (movimiento > 0 && !mirandoDerecha) Girar();
        if (movimiento < 0 && mirandoDerecha) Girar();

        if (enSuelo && movimiento != 0)
        {
            if (!audioPasos.isPlaying)
            {
                audioPasos.clip = sonidoCaminar;
                audioPasos.loop = true;
                audioPasos.Play();
            }
        }
        else
        {
            audioPasos.Stop();
        }
    }

    
    void WallSlide()
    {
        if (enPared && !enSuelo && rb.linearVelocity.y < 0)
        {
            deslizPared = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -velocidadDeslizPared);
        }
        else
        {
            deslizPared = false;
        }
    }

   
    void Salto()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (enSuelo)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
                audioFX.PlayOneShot(sonidoSalto);
            }
            else if (enPared)
            {
                StartCoroutine(WallJump());
            }
        }
    }

    System.Collections.IEnumerator WallJump()
    {
        puedeMoverse = false;

        int direccion = mirandoDerecha ? -1 : 1;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(direccion * fuerzaSaltoParedX, fuerzaSaltoParedY), ForceMode2D.Impulse);

        Girar();
        anim.SetBool("isWallJumping", true);
        audioFX.PlayOneShot(sonidoWallJump);

        yield return new WaitForSeconds(tiempoBloqueoMovimiento);

        anim.SetBool("isWallJumping", false);
        puedeMoverse = true;
    }


    void ChequearColisiones()
    {
        enSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioChequeo, capaSuelo);
        enPared = Physics2D.OverlapCircle(puntoPared.position, radioChequeo, capaPared);
    }

   
    void Animaciones()
    {
        anim.SetBool("isWalking", movimiento != 0 && enSuelo);
        anim.SetBool("isJumping", !enSuelo && !enPared);
        anim.SetBool("isWallSliding", deslizPared);
    }

    // ---------------- VIDA ----------------
    public void RecibirDaño(int daño)
    {
        vidaActual -= daño;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        audioFX.PlayOneShot(sonidoGolpe);
        ActualizarVidaUI();

        if (vidaActual <= 0)
            SceneManager.LoadScene(escenaMuerte);
    }

    void ActualizarVidaUI()
    {
        textoVida.text = "" + vidaActual;
    }

    
    void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}
