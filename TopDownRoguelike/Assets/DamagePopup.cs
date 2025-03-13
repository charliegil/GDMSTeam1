using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    private float disappearTimer = 1f;  
    private Color textColor;

    public void Setup(float damageAmount)
    {
        damageText.text = damageAmount.ToString();
        textColor = damageText.color; 
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
