using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioGlitchEffectController : MonoBehaviour
{
    [Header("오디오 설정")]
    public AudioSource audioSource;
    public int spectrumSize = 256; 
    
    [Tooltip("0(저음)부터 255(고음) 사이의 대역을 선택하세요. (드럼/베이스는 보통 0~5 부근)")]
    public int frequencyBand = 2; 
    
    [Tooltip("이 수치를 넘을 때만 글리치가 발동됩니다.")]
    public float threshold = 0.05f;

    [Header("글리치 적용 대상")]
    [Tooltip("영상(Raw Image)과 일반 이미지(Image)를 자유롭게 섞어서 등록하세요.")]
    public Graphic[] targetGraphics; 
    public Material glitchMaterial;    

    [Header("글리치 유지 시간 (초)")]
    public float minGlitchDuration = 0.05f; 
    public float maxGlitchDuration = 0.15f;  

    private Material[] originalMaterials;
    private float[] spectrumData;
    private bool isGlitching = false; // ⭐️ 중복 실행(글리치가 겹치는 현상) 방지용 플래그

    void Start()
    {
        if (audioSource != null)
        {
            spectrumData = new float[spectrumSize];
        }

        // 1. 등록된 UI 요소들의 원본 매터리얼을 순서대로 기억
        originalMaterials = new Material[targetGraphics.Length];
        for (int i = 0; i < targetGraphics.Length; i++)
        {
            if (targetGraphics[i] != null)
            {
                originalMaterials[i] = targetGraphics[i].material;
            }
        }
    }

    void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            // 오디오 주파수 대역별 크기를 가져옵니다.
            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
            float currentFrequencyVolume = spectrumData[frequencyBand];

            // ⭐️ 특정 음의 크기가 기준치를 넘었고, 현재 글리치 연출 중이 아닐 때만 발동!
            if (currentFrequencyVolume > threshold && !isGlitching)
            {
                StartCoroutine(TriggerGlitch());
            }
        }
    }

    // 실제 글리치를 씌우고 벗기는 연출 루틴
    IEnumerator TriggerGlitch()
    {
        isGlitching = true; // 연출 시작 잠금

        // 2. 등록된 모든 UI(영상+일반 이미지)에 글리치 덮어씌우기
        if (glitchMaterial != null)
        {
            for (int i = 0; i < targetGraphics.Length; i++)
            {
                if (targetGraphics[i] != null)
                    targetGraphics[i].material = glitchMaterial;
            }
        }

        // 3. 찰나의 시간 유지
        float glitchDuration = Random.Range(minGlitchDuration, maxGlitchDuration);
        yield return new WaitForSeconds(glitchDuration);

        // 4. 다시 원래 매터리얼(상태)로 복구
        for (int i = 0; i < targetGraphics.Length; i++)
        {
            if (targetGraphics[i] != null)
                targetGraphics[i].material = originalMaterials[i];
        }

        isGlitching = false; // 연출 종료, 다음 비트 대기
    }
}