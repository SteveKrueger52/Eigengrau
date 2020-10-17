using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtScene : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject image;
    public GameObject preview;

    private int objType = 0;

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(true);
        image.SetActive(false);
    }

    public void loadDrawing(int objType)
    {
        //1 = bed, 2 = easel, 3 = lamp
        menuPanel.SetActive(false);
        image.SetActive(true);
        preview.gameObject.transform.GetChild(objType).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
