using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class ChanngeScene : MonoBehaviour
{
    public string newScene;
    public SpriteRenderer sprt;
    public string tag;

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
        if(other.CompareTag(tag))
        {
            Debug.Log("chatch");
            sprt.enabled = true;
            //await Task.Delay(300);
            SceneManager.LoadScene(newScene);
        }
        else
        {
            Debug.Log("NoColl");
        }
    }
}
