using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletproofGlitch : MonoBehaviour
{
    [Header("글리치 설정 ⚡")]
    public Graphic targetGraphic;
    public Material glitchMaterial;
    public float glitchDuration = 0.1f;

    private Material originalMaterial;
    private bool isGlitching = false; // 광클 방어막

    // ⭐️ Start가 아닌 Awake를 사용해 가장 먼저 원본을 기억합니다.
    void Awake() 
    {
        if (targetGraphic != null)
        {
            originalMaterial = targetGraphic.material;
        }
    }

    public void PlayGlitch()
    {
        // 이미 연출 중이거나, 컴포넌트가 없으면 무시합니다.
        if (isGlitching || targetGraphic == null || glitchMaterial == null) return;

        isGlitching = true;
        targetGraphic.material = glitchMaterial;
        
        // ⭐️ UGUI에게 강제로 화면을 다시 그리라고 명령합니다! (버그 방지)
        targetGraphic.SetMaterialDirty(); 

        DOVirtual.DelayedCall(glitchDuration, () =>
        {
            RestoreMaterial();
        }).SetUpdate(true).SetLink(gameObject);
    }

    // 오브젝트가 꺼지거나 파괴될 때 강제 복구
    void OnDisable()
    {
        RestoreMaterial();
    }

    // 매터리얼 복구 전용 함수 (중복 코드 최소화)
    private void RestoreMaterial()
    {
        if (targetGraphic != null && originalMaterial != null)
        {
            targetGraphic.material = originalMaterial;
            targetGraphic.SetMaterialDirty(); // 복구할 때도 강제로 다시 그리게 명령!
        }
        isGlitching = false;
    }
}