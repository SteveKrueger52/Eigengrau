using System.Collections;
using System.Collections.Generic;
using FreeDraw;
using UnityEngine;

public class ArtScene : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject image;
    public GameObject preview;
    public Drawable canvas;

    private int objType = 0;

    // Start is called before the first frame update
    void Start()
    {
        // menuPanel.SetActive(true);
        image.SetActive(false);
        loadDrawing(SceneChanger.Instance.GetActiveDrawing());
    }

    public void loadDrawing(int objType)
    {
        this.objType = objType < 0 ? 0 : objType;
        //0 = bed, 1 = easel, 2 = lamp
        canvas.setObjType(objType);
        menuPanel.SetActive(false);
        image.SetActive(true);
        preview.gameObject.transform.GetChild(objType).gameObject.SetActive(true);
    }

    public void saveDrawing()
    {
        canvas.SaveDrawing();
        SceneChanger.Instance.EndDrawing();
    }
}
