using UnityEngine;

public class disparotorreta : MonoBehaviour
{
    public GameObject bala;
    public Transform balaPos;

    private float timer;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < 4)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                timer = 0;
                shoot();
            }
        }

       
    }

    void shoot()
    {
        Instantiate(bala, balaPos.position, Quaternion.identity);
    }
}
