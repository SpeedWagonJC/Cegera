using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }

    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }
}
