using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.VisualScripting;

public class EffectButton : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler{
    public TextMeshProUGUI textMesh;
    private Vector3 originalScale;
    public float scaleMultiplier = 1.3f;
    public float duration = 0.2f;
    private Coroutine scaleCoroutine;
    
   

    public void Start(){
    originalScale = textMesh.transform.localScale;
    }

    public void bigger(){
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleText(originalScale * scaleMultiplier));
    }
    
    
    public void OnPointerEnter(PointerEventData eventData){
        //Debug.Log("on mouse over");
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleText(originalScale * scaleMultiplier));

    }

    

    public void OnPointerExit(PointerEventData eventData){
        //Debug.Log("exit");
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleText(originalScale));
    }
    public IEnumerator ScaleText(Vector3 targetScale){
        //Debug.Log(targetScale + " org" + originalScale);
        float time = 0;
        Vector3 startScale = textMesh.transform.localScale;

        while (time < duration){
            textMesh.gameObject.transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        
        textMesh.gameObject.transform.localScale = targetScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        AudioManager.instance.PlaySound("clickButton");
    }
}
