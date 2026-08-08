using UnityEngine;

public class changeSpriteMask : MonoBehaviour {
    public SpriteRenderer Sprite; 
    private SpriteMask spriteMask;
    private Sprite lastSprite;

    void Start()
    {
        spriteMask = GetComponent<SpriteMask>();

        if (Sprite != null)
            lastSprite = Sprite.sprite;
    }

    void Update()
    {
        if( Sprite!= null &&Sprite.sprite != spriteMask.sprite){
            lastSprite = Sprite.sprite;
            spriteMask.sprite = Sprite.sprite;
            
        }
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (Sprite.flipX ? -1 : 1), transform.localScale.y,transform.localScale.z);  
    }
}