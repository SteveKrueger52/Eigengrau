using UnityEngine;

public class HotSwap : MonoBehaviour
{
    public string ItemName;
    public SpriteRenderer placeholder;
    public SpriteRenderer replacement;
    
    private void Awake()
    {
        Hotswap();
    }

    public void Hotswap()
    {
        string path = Application.persistentDataPath + "/SaveImages/" + ItemName + ".png";
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            
            placeholder.gameObject.SetActive(false);
            replacement.gameObject.SetActive(true);
            replacement.sprite = sprite;
        }
        else
        {
            placeholder.gameObject.SetActive(true);
            replacement.gameObject.SetActive(false);
        }
            
    }
}
