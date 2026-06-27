using UnityEngine;

public class cambiocamara : MonoBehaviour
{
    public GameObject camaraActivar;
    public GameObject camaraDesactivar;
    public string playerTag = "Player";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            SwitchCameras();
        }
    }

    private void SwitchCameras()
    {
        if (camaraActivar !=null && camaraDesactivar != null)
        {
            camaraActivar.SetActive(true);
            camaraDesactivar.SetActive(false);
        }
    }
}
