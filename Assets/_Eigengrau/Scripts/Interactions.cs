using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interactions : MonoBehaviour
{
    string text = "";
    public GameObject txtObj;
    int textState; //0 = no text displayed, 1 = text animating, 2 = text done, waiting for click to proceed, 3 = door switch
    public GameObject chara;
    bool doorSwitch;
    int changingScene; //0 = no choice selected, 1 = changing scene, 2 = not changing scene
    int targetScene;
    public GameObject yesBtn;
    public GameObject noBtn;

    private void Start()
    {        
        yesBtn.SetActive(false);
        noBtn.SetActive(false);
        txtObj.gameObject.SetActive(false);
        textState = 0;
        doorSwitch = false;
        changingScene = 0;
    }

    private void Update()
    {
        if(textState != 0) //disable character movement if they're interacting with something
        {            
            chara.GetComponent<Figure>().moveSpeed = 0;
        }
        else
        {
            if(txtObj.gameObject.activeSelf)
            {
                textBoxOff();
            }            
            chara.GetComponent<Figure>().moveSpeed = 5;
        }

        if (textState == 2)
        {            
            StartCoroutine(WaitForClick());
        }

        if (textState == 3)
        {
            yesBtn.SetActive(true);
            noBtn.SetActive(true);
            StartCoroutine(SceneChangeCheck());
        }
    }

    public void setTextBed()
    {
        if(textState == 0)
        {
            text = "It's not time to sleep yet...";
            displayFlavorText();
        }
    }

    public void setTextPainting()
    {
        if (textState == 0)
        {
            text = "A true classic! How did I get it out of the museum again...?";
            displayFlavorText();
        }            
    }

    public void setTextDesk()
    {
        if (textState == 0)
        {
            text = "My desk is kind of cluttered, oops.";
            displayFlavorText();
        }            
    }

    public void setTextWallpaper()
    {
        if (textState == 0)
        {
            text = "This wallpaper really livens up my room! I wonder if I can make it better...";
            displayFlavorText();
        }            
    }

    public void setTextWindow()
    {
        if (textState == 0)
        {
            text = "The view from my apartment is great! I can see everything I want!";
            displayFlavorText();
        }            
    }

    public void setTextLamp()
    {
        if (textState == 0)
        {
            text = "*Click* Light on! *Click* Light off. *Click* Light on! *Click* Light off.";
            displayFlavorText();
        }            
    }

    public void interactDoor()
    {
        if (textState == 0)
        {
            text = "Should I head out?";
            doorSwitch = true;
            targetScene = (int) SceneChanger.SceneID.STREET;
            displayFlavorText();
        }
    }
    
    public void interactGoHome()
    {
        if (textState == 0)
        {
            text = "Time to head home?";
            doorSwitch = true;
            targetScene = (int) SceneChanger.SceneID.HOME;
            displayFlavorText();
        }
    }
    
    public void interactGoToSchool()
    {
        if (textState == 0)
        {
            text = "Work is this way. Shall we?";
            doorSwitch = true;
            targetScene = (int) SceneChanger.SceneID.CLASS;
            displayFlavorText();
        }
    }

    void displayFlavorText()
    {        
        textState = 1;
        textBoxOn();
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        for (int i = 0; i < text.Length+1; i++)
        {
            txtObj.transform.GetChild(1).GetComponent<Text>().text = text.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }

        if(doorSwitch)
        {
            textState = 3;
        }

        else
        {
            textState = 2;
        }
        
        yield return null;
    }

    IEnumerator WaitForClick()
    {
        if(Input.GetMouseButton(0))
        {
            textState = 0;
            yield break;
        }
    }

    IEnumerator SceneChangeCheck()
    {
        while (changingScene == 0)
        {
            yield return null;
        }
    }

    public void confirmChangeScene()
    {
        changingScene = 1;
        //SceneManager.LoadScene(streetscene);
        //The rest of this function can be deleted when we get the new scene!
        SceneChanger.Instance.GoToScene(targetScene);
        
        textState = 0;
        yesBtn.SetActive(false);
        noBtn.SetActive(false);
        doorSwitch = false;
    }

    public void notChangingScene()
    {
        changingScene = 2;
        textState = 0;
        yesBtn.SetActive(false);
        noBtn.SetActive(false);
        doorSwitch = false;
    }

    void textBoxOn()
    {
        txtObj.gameObject.SetActive(true);
        for (int i = 0; i < txtObj.transform.childCount; i++)
        {
            txtObj.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    void textBoxOff()
    {
        txtObj.gameObject.SetActive(false);
        for (int i = 0; i < txtObj.transform.childCount; i++)
        {
            txtObj.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
