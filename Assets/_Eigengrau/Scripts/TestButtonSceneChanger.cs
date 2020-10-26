using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonSceneChanger : MonoBehaviour
{
    public int BuildIndex;

    public void ChangeScene()
    {
        Debug.Log("Changing Scene to : " + BuildIndex);
        SceneChanger.Instance.GoToScene(BuildIndex);
    }
}
