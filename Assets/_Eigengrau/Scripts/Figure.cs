using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed;

    private float xInput;
    private Animator playerAction;

    private void Start()
    {
        this.playerAction = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (xInput >= 1)
        {
            playerAction.SetBool("isWalkingRight", true);
            playerAction.SetBool("isWalkingLeft", false);
        }
        else if (xInput <= -1)
        {
            playerAction.SetBool("isWalkingRight", false);
            playerAction.SetBool("isWalkingLeft", true);
        }
        else
        {
            playerAction.SetBool("isWalkingRight", false);
            playerAction.SetBool("isWalkingLeft", false);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        this.xInput = Input.GetAxisRaw("Horizontal");
        transform.Translate(new Vector2(xInput, 0) * moveSpeed * Time.deltaTime);
    }
}
