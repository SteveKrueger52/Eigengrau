using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetPlacement : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player.position = new Vector3(12f * (SceneChanger.Instance._returnScene == (int) SceneChanger.SceneID.CLASS ? -1 : 1), -1.75f, 0f);
    }
}
