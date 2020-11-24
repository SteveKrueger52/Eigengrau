using System.Collections;
using System.Collections.Generic;
using FreeDraw;
using UnityEngine;
using UnityEngine.UI;

public class ArtScene : MonoBehaviour
{
    public GameObject image;
    public GameObject preview;
    public Drawable canvas;

    private int objType = 0;
    //  0 - Easel       1 - Painting    2 - Desk        3 - Bed
    //  4 - Wallpaper   5 - Door        6 - Window      7 - Lamp
    //  8 - Tree        9 - Podium     10 - S. Desk    11 - Blackboard
    // 12 - Skyline    13 - Decor      14 - Streetlamp 15 - Sign

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
        canvas.setObjType(objType);
        image.SetActive(true);
        /*
        preview.gameObject.transform.GetChild(objType).gameObject.SetActive(true);
        var tempColor = preview.gameObject.transform.GetChild(objType).gameObject.GetComponent<Image>().color;
        tempColor.a = 0.5f;
        preview.gameObject.transform.GetChild(objType).gameObject.GetComponent<Image>().color = tempColor;
        */
        togglePreview();
    }

    public void saveDrawing()
    {
        canvas.SaveDrawing();
        GameDataLogger.ExitDrawingScene(true);
        SceneChanger.Instance.EndDrawing();
    }

    public void quitWithoutSaving()
    {
        GameDataLogger.ExitDrawingScene(false);
        SceneChanger.Instance.EndDrawing();
    }

    public void togglePreview()
    {
        if(preview.gameObject.transform.GetChild(objType).gameObject.activeSelf == true)
        {
            preview.gameObject.transform.GetChild(objType).gameObject.SetActive(false);
        }

        else
        {
            preview.gameObject.transform.GetChild(objType).gameObject.SetActive(true);
            var tempColor = preview.gameObject.transform.GetChild(objType).gameObject.GetComponent<Image>().color;
            tempColor.a = 0.5f;
            preview.gameObject.transform.GetChild(objType).gameObject.GetComponent<Image>().color = tempColor;
        }
        
    }
}
