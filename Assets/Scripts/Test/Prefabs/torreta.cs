using System.Collections;
using UnityEngine;

public class torreta : MonoBehaviour
{
    public GameObject bala;
    public Transform balaPos;
    
    public float velocidadRotacion = 5f; 
    public float offsetAngulo = -90f; 

    [Header("Configuración de Animación (Sprites)")]
    public SpriteRenderer spriteRendererHijo; 
    public Sprite[] spritesDisparo; 
    public float tiempoEntreSprites = 0.03f; 
    private Sprite spriteOriginal; 

    private float timer;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (spriteRendererHijo != null)
        {
            spriteOriginal = spriteRendererHijo.sprite;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < 20)
        {
            ApuntarAlJugador();

            timer += Time.deltaTime;
            if (timer > 3)
            {
                timer = 0;
                Shoot();
            }
        }
    }

    void ApuntarAlJugador()
    {
        Vector3 direccion = player.transform.position - transform.position;
        
        float anguloZ = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        Quaternion rotacionDeseada = Quaternion.Euler(0, 0, anguloZ + offsetAngulo);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * velocidadRotacion);
    }

    void Shoot()
    {
        Instantiate(bala, balaPos.position, balaPos.rotation);

        if (spriteRendererHijo != null && spritesDisparo.Length > 0)
        {
            StartCoroutine(AnimarDisparo());
        }
    }

    IEnumerator AnimarDisparo()
    {
        for (int i = 0; i < spritesDisparo.Length; i++)
        {
            spriteRendererHijo.sprite = spritesDisparo[i];
            yield return new WaitForSeconds(tiempoEntreSprites);
        }
        spriteRendererHijo.sprite = spriteOriginal;
    }
}