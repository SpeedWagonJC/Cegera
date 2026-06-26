using UnityEngine;

public class enemigos : MonoBehaviour
{
    public Transform player;
    public float deteccion = 5.0f;
    public float speed = 2.0f;

    private Rigidbody rb;
    private Vector3 movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < deteccion)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = new Vector3(direction.x, 0);
        }
        else
        {
            movement = Vector3.zero;
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime); 
    }
}
