using System;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// handler for displaying the boost that are obtained from the collectables.
/// Even if this class has a lot of methods, it is designed to be as seamless as possible for working with that in other 
/// classes
/// </summary>
public class BoostUIHandler : MonoBehaviour, IEventListener {

    // could use a dictionary. map the collectable types to the UIBoost
    [SerializeField] public UIBoost [] UIBoosts;

    private Vector3[] positions;
    private bool [] positionsOccupied;
    private int numBoostActive= 0;

    void Start(){
        
        positions = new Vector3[UIBoosts.Length];
        positionsOccupied = new bool[UIBoosts.Length];
        
        for(int i = 0;i<positions.Length;i++){
            UIBoosts[i].rectTransform = UIBoosts[i].UI.GetComponent<RectTransform>();
            positions[i] = UIBoosts[i].rectTransform.transform.position;
        }
        
        
        subscribe();
        foreach(UIBoost boost in UIBoosts){
            boost.textMesh = boost.UI.GetComponentInChildren<TextMeshProUGUI>();
            boost.UI.SetActive(false);
        }
    }

    public void showCounter(float duration , CollectableType type){
        
        int index = getIndexFromType(type);
        
        if(index == -1) return;
        
        if(UIBoosts[index].coroutine== null){
            UIBoosts[index].coroutine = StartCoroutine(makeCounterEnumerator(duration,index));
        }
        else{
            if(UIBoosts[index].timeLeft > duration){
                Debug.Log("the current coroutine has more time left than the current boost");
            }
            else{
                StopCoroutine(UIBoosts[index].coroutine);
                UIBoosts[index].coroutine = StartCoroutine(makeCounterEnumerator(duration,index));
            }
            
        }
    }
    public IEnumerator makeCounterEnumerator(float duration, int index){
        int position = getFirstOpenPosition();
        positionsOccupied[position] = true;
        UIBoosts[index].rectTransform.transform.position = positions[position];
        numBoostActive++;
        UIBoosts[index].UI.SetActive(true);
        float roundedValue;
        UIBoosts[index].timeLeft = duration;
        while(duration > 0){
            roundedValue = MathF.Round(duration, 0);
            UIBoosts[index].textMesh.text = roundedValue +"";
            UIBoosts[index].timeLeft = duration;
            duration-=Time.deltaTime;
            yield return null;
        }
        positionsOccupied[position] = false;
        UIBoosts[index].UI.SetActive(false);
        UIBoosts[index].coroutine = null;
        numBoostActive--;


    }
    public int getFirstOpenPosition(){
        for(int i=0; i<positionsOccupied.Length;i++){
            if(!positionsOccupied[i]) return i;
        }
        return -1;
    }

    public int getIndexFromType(CollectableType type){
        for(int i=0; i<UIBoosts.Length;i++){
            if(UIBoosts[i].boostType == type) return i;
        }
        return -1;
    }

    public void subscribe()
    {
        EventManager.OnDurationBoostStarted += showCounter;
    }

    public void unsubscribe()
    {
        EventManager.OnDurationBoostStarted -= showCounter;
    }
    void OnDisable()
    {
        unsubscribe();
    }
}
[System.Serializable]
public class UIBoost {
    public GameObject UI;
    public CollectableType boostType;
    [HideInInspector] public TextMeshProUGUI textMesh = null;
    [HideInInspector] public RectTransform rectTransform = null;
    [HideInInspector] public Coroutine coroutine = null;
    [HideInInspector] public float timeLeft = -1;

}
