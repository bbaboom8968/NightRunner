using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioGlitchBaker : MonoBehaviour
{
    [Header("모드 설정 🛠️")]
    [Tooltip("체크하면 에디터에서 음악을 분석해 타이밍을 기록합니다. 빌드할 때는 반드시 체크를 해제하세요!")]
    public bool isBakingMode = true;

    [Header("오디오 설정 🎶")]
    public AudioSource audioSource;
    public int spectrumSize = 256;
    public int frequencyBand = 2;
    public float threshold = 0.05f;

    [Header("추출된 타이밍 데이터 ⏱️")]
    [Tooltip("베이킹 모드로 실행하면 여기에 글리치가 터질 시간(초)들이 자동으로 저장됩니다.")]
    public List<float> bakedTimestamps = new List<float>();

    [Header("글리치 설정 ⚡")]
    public Graphic[] targetGraphics;
    public Material glitchMaterial;
    public float minGlitchDuration = 0.05f;
    public float maxGlitchDuration = 0.15f;

    private Material[] originalMaterials;
    private float[] spectrumData;
    private bool isGlitching = false;
    private int currentTimingIndex = 0; // 웹에서 현재 몇 번째 글리치를 터뜨릴 차례인지 기억하는 숫자

    void Start()
    {
        if (audioSource != null)
        {
            spectrumData = new float[spectrumSize];
        }

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
        if (audioSource == null || !audioSource.isPlaying) return;

        if (isBakingMode)
        {
            // [모드 1: 베이킹] 에디터에서 주파수를 분석하고 시간을 기록합니다.
            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
            float currentFrequencyVolume = spectrumData[frequencyBand];

            if (currentFrequencyVolume > threshold && !isGlitching)
            {
                // 글리치가 터진 정확한 시간(Time)을 리스트에 추가
                bakedTimestamps.Add(audioSource.time);
                StartCoroutine(TriggerGlitch(true)); // 베이킹 전용 딜레이로 실행
            }
        }
        else
        {
            // [모드 2: 플레이] 웹 브라우저에서는 기록된 '시간표'만 보고 재생합니다.
            if (currentTimingIndex < bakedTimestamps.Count)
            {
                // 현재 오디오 재생 시간이, 기록해둔 시간표에 도달했다면?
                if (audioSource.time >= bakedTimestamps[currentTimingIndex])
                {
                    currentTimingIndex++; // 다음 순서표로 이동
                    if (!isGlitching)
                    {
                        StartCoroutine(TriggerGlitch(false));
                    }
                }
            }
        }
    }

    IEnumerator TriggerGlitch(bool isBaking)
    {
        isGlitching = true;

        if (glitchMaterial != null)
        {
            for (int i = 0; i < targetGraphics.Length; i++)
            {
                if (targetGraphics[i] != null) targetGraphics[i].material = glitchMaterial;
            }
        }

        float glitchDuration = Random.Range(minGlitchDuration, maxGlitchDuration);
        
        // 에디터에서 기록 중일 때는 중복 기록을 막기 위해 딜레이를 살짝 길게(0.2초) 줍니다.
        yield return new WaitForSeconds(isBaking ? 0.2f : glitchDuration);

        for (int i = 0; i < targetGraphics.Length; i++)
        {
            if (targetGraphics[i] != null) targetGraphics[i].material = originalMaterials[i];
        }

        isGlitching = false;
    }
}