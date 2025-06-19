using UnityEngine;

public class PortalSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip portalOpenSound;  // Portal açıldığında çalacak ses
    public AudioSource portalAudioSource;  // Ses kaynağı

    // Bu fonksiyonu portal açılacak yerden tetikleyebilirsiniz
    public void PlayPortalSound()
    {
        if (portalAudioSource != null && portalOpenSound != null)
        {
            portalAudioSource.PlayOneShot(portalOpenSound);
        }
    }
}
