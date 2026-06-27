using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject panelPausa;

    [Header("Audio")]
    public Slider sliderVolumen;

    private bool pausado = false;
    private const string VOLUMEN_KEY = "volumenGeneral";

    void Start()
    {
        panelPausa.SetActive(false);

        
        float volumenGuardado = PlayerPrefs.GetFloat(VOLUMEN_KEY, 1f);
        AudioListener.volume = volumenGuardado;

        if (sliderVolumen != null)
        {
            sliderVolumen.value = volumenGuardado;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (pausado)
                Reanudar();
            else
                Pausar();
        }
    }

    void Pausar()
    {
        pausado = true;
        Time.timeScale = 0f;
        panelPausa.SetActive(true);
    }

    public void Reanudar()
    {
        pausado = false;
        Time.timeScale = 1f;
        panelPausa.SetActive(false);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }

    
    public void CambiarVolumen(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat(VOLUMEN_KEY, valor);
        PlayerPrefs.Save();
    }

    
    public void SalirDelJuego()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
