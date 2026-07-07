using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AudioToAnimator : MonoBehaviour
{
    public AudioSource audioSource;
    public Animator animator;
    public int sampleSize = 1024;
    public float sensitivity = 100f;

    private float[] audioData;

    void Start()
    {
        if(audioSource != null)
        {
            audioData = new float[sampleSize];
        }
    }

    void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.GetOutputData(audioData, 0);

            float rmsSum = 0;
            for (int i = 0; i < sampleSize; i++)
            {
                rmsSum += audioData[i] * audioData[i];
            }
            float rms = Mathf.Sqrt(rmsSum / sampleSize);

            float normalizedAmplitude = Mathf.Clamp01(rms * sensitivity);

            animator.SetFloat("Blend", normalizedAmplitude);
        }
    }
}