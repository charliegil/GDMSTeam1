using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public float regenerationPerSecond= 0;
    [HideInInspector] public float totalHP =100;

    public float currentHP = 100;

    public bool regenerate = false;

    public Slider sliderChange;

    public Slider HealthBar;

    public float animationDuration = 0.1f;

    private float time= 0;
    public TMP_Text Val;

    private Coroutine currentLerpCoroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if(Time.time - time >=1f && regenerate){
            time = Time.time;
            setHP(currentHP+regenerationPerSecond , true);
            //sliderChange.value = currentHP;
        }

        /*if (currentHP <= 0) {
            SceneManager.LoadScene("EndDeat");
        }*/

    }


    public void setHP(float hp, bool animation)
    {
        if (hp > totalHP) hp = totalHP;
        currentHP = hp;
        if (animation)
        {
            // Stop any existing coroutine before starting a new one
            if (currentLerpCoroutine != null){
                StopCoroutine(currentLerpCoroutine);
            }

            
            currentLerpCoroutine = StartCoroutine(LerpHealthBar(HealthBar.value, hp / totalHP));
        }
        else
        {
            HealthBar.value =  hp / totalHP;
        }
        Val.text = (int)hp + "/" + (int)totalHP;
    }
    public void onValueChange(){
        float value = sliderChange.value;
        setHP(value*totalHP , true);
        
    }

    private IEnumerator LerpHealthBar(float start, float end){
        float timeElapsed = 0f;

        while (timeElapsed < animationDuration)
        {
            HealthBar.value = Mathf.Lerp(start, end, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        HealthBar.value = end;
    }

    public void addHP(float add, bool animation){
        setHP(currentHP + add, animation);
    }
    
}
