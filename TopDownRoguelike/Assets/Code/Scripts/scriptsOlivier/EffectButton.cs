using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class EffectButton : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler{
    public TextMeshProUGUI textMesh;
    private float originalScale;
    public float scaleMultiplier = 1.3f;
    public float duration = 0.2f;
    private Coroutine scaleCoroutine;
    
   

    public void OnEnable(){
    originalScale = textMesh.fontSize;
    scaleCoroutine = null;
    }
    public void OnDisable(){
        if(scaleCoroutine != null)StopCoroutine(scaleCoroutine);
    }

    public void bigger(){
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleText(originalScale * scaleMultiplier));
    }
    
    
    public void OnPointerEnter(PointerEventData eventData){
        Debug.Log("on mouse over");
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleText(originalScale * scaleMultiplier));
    }

    public void OnPointerExit(PointerEventData eventData){
        Debug.Log("exit");
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleText(originalScale));
    }
    
    public IEnumerator ScaleText(float targetScale){
        Debug.Log(targetScale + " org" + originalScale);
        float time = 0;
        float startScale = textMesh.fontSize;

        while (time < duration){
            textMesh.fontSize = Mathf.Lerp(originalScale,10,time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        
        textMesh.fontSize = targetScale;
        scaleCoroutine = null;

    }

    

}
