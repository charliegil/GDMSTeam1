using UnityEngine;
using UnityEngine.UI;


public class healthBar : MonoBehaviour
{
    public int health;
    public int numOfHearths;

    public Image[] hearths;
    public Sprite fullHeart;
    public Sprite emptyHearth;
    public Sprite halfHearth;

    public Slider slider;


    private void Start(){
        setMaxHealth(numOfHearths);
        getMaxhealth();
    }
    private void setHealth(int num){
        health = num;
        for(int i = 0; i< health ;i++){
            hearths[i].sprite = fullHeart;
        }
        for(int  i=health; i<numOfHearths;i++){
            hearths[i].sprite = emptyHearth;
        }
    }
    private void addHealth(int num){
        for(int i = 0; i< num ;i++){
            hearths[i+health].sprite = fullHeart;
        }
        health+=num;
    }
    private void increaseMaxHealth(int num){
    for(int i =0 ; i<num;i++){
        hearths[i+numOfHearths].enabled = true;
        hearths[i+numOfHearths].sprite = emptyHearth;
    }
    numOfHearths+= num;
   }
    private void setMaxHealth(int num){
    increaseMaxHealth(num-numOfHearths);
    
   }
    private void getMaxhealth(){
    setHealth(numOfHearths);
   }

   public void ValueChange(){
    setHealth((int)slider.value);
   }
}
