using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public float disappearTimer = 1f;  
    private Color textColor;

    [SerializeField] PlayerController playerController;

    public void Setup(float damageAmount)
    {
        string damage = damageAmount.ToString("F1");
        if(damage.EndsWith(".0")) damage = damage.Split(".")[0];

        damageText.text = damage;
        
    }
    public void setColor(Color color){
        damageText.color = color;
    }

    private void Update()
    {
        float moveYSpeed = 1f;
        transform.position += new Vector3(0, moveYSpeed * Time.deltaTime, 0);

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float fadeSpeed = 3f;
            textColor.a -= fadeSpeed * Time.deltaTime;
            damageText.color = textColor;

            if (textColor.a <= 0f)
            {
                Destroy(gameObject); 
            }
        }
    }
}
