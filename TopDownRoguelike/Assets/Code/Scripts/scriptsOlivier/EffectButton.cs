using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class EffectButton : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler{
    public TextMeshProUGUI textMesh;
    private Vector3 originalScale;
    public float scaleMultiplier = 1.3f;
    public float duration = 0.2f;
    private Coroutine scaleCoroutine;
    
   

    public void Start(){
    originalScale = textMesh.transform.localScale;
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
    private IEnumerator ScaleText(Vector3 targetScale){
        float time = 0;
        Vector3 startScale = textMesh.transform.localScale;

        while (time < duration){
            textMesh.transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        textMesh.transform.localScale = targetScale;
    }

    

}
