using System.Collections;
using UnityEngine;

public class HitTarget : MonoBehaviour
{
    [SerializeField]
    private RangeDetector m_RangeDetector;
    [SerializeField]
    private GameObject m_temporaryTarget;
    [SerializeField]
    private AudioSource m_audioSource;
    [SerializeField]
    private AudioClip m_swordSound;
    [SerializeField]
    private float m_swordSoundDelay = 0.5f;
    [SerializeField, Range(0, 1)]
    private float m_volume = 0.8f;
    public void PerformHit()
{
    Debug.Log("Performing hit");
    GameObject target = null;

    // Use only the detected target, not a temporary fallback
    if (m_RangeDetector != null && m_RangeDetector.DetectedTarget != null)
    {
        target = m_RangeDetector.DetectedTarget.gameObject;
    }

    if (target == null)
    {
        Debug.LogWarning("No valid target to hit.");
        return; // Stop execution if no valid target is found
    }

    HitHandler handler = target.GetComponent<HitHandler>();
    if (handler != null)
    {
        handler.GetHit(gameObject);
    }

    StopAllCoroutines();
    StartCoroutine(PlaySwordSound());
}

    private IEnumerator PlaySwordSound()
    {
        yield return new WaitForSeconds(m_swordSoundDelay);
        if (m_audioSource != null && m_swordSound != null)
        {
            m_audioSource.volume = m_volume;
            m_audioSource.PlayOneShot(m_swordSound);
        }
    }
}
