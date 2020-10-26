using UnityEngine;

public class SelectionMenu : MonoBehaviour
{
    [SerializeField]
    private Transform panel;
    
    // Start is called before the first frame update
    void Start()
    {
        panel.gameObject.SetActive(false);
    }
    
    public void Reveal(int state)
    {
        switch (state)
        {
            case 1:
                panel.gameObject.SetActive(true);
                break;
            case 0:
                panel.gameObject.SetActive(false);
                break;
            default:
                panel.gameObject.SetActive(!panel.gameObject.activeSelf);
                break;
        }
    }

    public void SelectToDraw(int drawIndex)
    {
        SceneChanger.Instance.BeginDrawing(drawIndex);
    }
}
