using UnityEngine;

public class cambiiarmundo : MonoBehaviour
{
    public GameObject[] objetoscambiar;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            alternarobjetos();
        }
    }

    void alternarobjetos()
    {
        foreach (GameObject obj in objetoscambiar)
        {
            if(obj != null)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
    }
}
