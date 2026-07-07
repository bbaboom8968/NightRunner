using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Graphic 클래스를 제어하기 위해 필요

public class GlitchEffectController : MonoBehaviour
{
    [Header("글리치 적용 대상")]
    [Tooltip("영상(Raw Image)과 일반 이미지(Image)를 자유롭게 섞어서 등록하세요.")]
    public Graphic[] targetGraphics;   // ⭐️ RawImage와 Image를 모두 품을 수 있는 마법의 클래스로 변경!
    public Material glitchMaterial;    

    [Header("발동 타이밍 (초)")]
    public float minWaitTime = 2f;     
    public float maxWaitTime = 6f;     
    
    [Header("글리치 유지 시간 (초)")]
    public float minGlitchDuration = 0.05f; 
    public float maxGlitchDuration = 0.2f;  

    private Material[] originalMaterials; // 각 오브젝트의 원본 매터리얼 기억용

    void Start()
    {
        // 1. 등록된 UI 요소들의 개수만큼 기억할 공간 생성
        originalMaterials = new Material[targetGraphics.Length];
        
        for (int i = 0; i < targetGraphics.Length; i++)
        {
            if (targetGraphics[i] != null)
            {
                // 2. 각각의 원본 매터리얼을 순서대로 저장
                originalMaterials[i] = targetGraphics[i].material;
            }
        }

        // 3. 타이머 시작
        StartCoroutine(GlitchRoutine());
    }

    IEnumerator GlitchRoutine()
    {
        while (true) 
        {
            // 글리치 발동 전 대기
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // ⭐️ 등록된 모든 UI(영상+일반 이미지)에 글리치 덮어씌우기
            if (glitchMaterial != null)
            {
                for (int i = 0; i < targetGraphics.Length; i++)
                {
                    if (targetGraphics[i] != null)
                        targetGraphics[i].material = glitchMaterial;
                }
            }

            // 찰나의 시간 유지
            float glitchDuration = Random.Range(minGlitchDuration, maxGlitchDuration);
            yield return new WaitForSeconds(glitchDuration);

            // ⭐️ 다시 원래 상태로 복구
            for (int i = 0; i < targetGraphics.Length; i++)
            {
                if (targetGraphics[i] != null)
                    targetGraphics[i].material = originalMaterials[i];
            }
        }
    }
}