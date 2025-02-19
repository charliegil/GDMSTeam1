using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class newHealth : MonoBehaviour
{
    public float regenerationPerSecond= 0;
    public float totalHP =100;

    public float currentHP = 100;

    public bool regenerate = false;

    public Slider sliderChange;

    public Slider HealthBar;

    public float animationDuration = 0.1f;

    private float time= 0;
    public TMP_Text Val;

    public GameObject panel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(panel != null) panel.active  = false;
        setHP(sliderChange.value*totalHP, false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - time >=1f && regenerate){
            time = Time.time;
            setHP(currentHP+regenerationPerSecond , true);
            //sliderChange.value = currentHP;
        }
    }

    void setHP(float hp, bool animation){
        
        if(hp> totalHP) hp = totalHP;
        currentHP = hp;
        if(animation){
            StartCoroutine(LerpHealthBar(HealthBar.value, (float)hp/totalHP));
        }
        else{
            HealthBar.value = hp;
        }
        Val.text = (int)hp+"/"+totalHP;
        if(hp == 0) {
            Time.timeScale = 0;
            if(panel != null) panel.active  = true;
            if(sliderChange != null) sliderChange.interactable   = false;
            }
        
    }
    public void onValueChange(){
        float value = sliderChange.value;
        setHP(value*totalHP , true);
        
    }
   
    IEnumerator LerpHealthBar(float start, float end)
    {
        float timeElapsed = 0f;

        while (timeElapsed < animationDuration)
        {
            HealthBar.value = Mathf.Lerp(start, end, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        HealthBar.value = end; 
    }
    public void addHP(float add , bool animation){
        setHP(currentHP+add, animation);
    }
    
}
