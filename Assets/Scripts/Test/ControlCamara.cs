using UnityEngine;

public class ControlCamara : MonoBehaviour
{
    public GameObject CamaraActivar;
    public GameObject CamaraDesactivar;
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
        if(other.CompareTag(playerTag))
        {

        }
    }

    private void CambiarCamara()
    {
        if(CamaraActivar != null && CamaraDesactivar != null)
        {
            CamaraActivar.SetActive(true);
            CamaraDesactivar.SetActive(false);
        }
    }
}
