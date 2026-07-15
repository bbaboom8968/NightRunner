using UnityEngine;
using UnityEngine.Rendering; // Volume 제어를 위해 필수
using DG.Tweening;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("트랜지션 대상 연결")]
    [Tooltip("연출이 끝나면 비활성화할 캔버스 오브젝트를 연결하세요.")]
    [SerializeField] private GameObject transitionCanvas; 
    [Tooltip("투명도를 조절할 Canvas Group 컴포넌트")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;   
    [Tooltip("적용된 Global Volume")]
    [SerializeField] private Volume blurVolume; 

    [Header("연출 설정")]
    [SerializeField] private float transitionDuration = 1.5f; // 연출 진행 시간
    
    [Header("시작 및 지연 설정")]
    [Tooltip("체크하면 Start 시점에 자동으로 페이드인 연출을 시작합니다.")]
    [SerializeField] private bool autoStart = true;
    [Tooltip("자동 시작 시 페이드 연출 전 대기할 시간 (씬 로딩 렉이나 이미지 로딩 대기)")]
    [SerializeField] private float startDelay = 0.5f;

    void Start()
    {
        // 🌟 1. 씬이 열리자마자 '무조건' 화면을 까맣고 뿌옇게 덮습니다. (로딩 중 안 보이게)
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 1f;
        if (blurVolume != null) blurVolume.weight = 1f;
        
        if (transitionCanvas != null) transitionCanvas.SetActive(true);
        if (fadeCanvasGroup != null) fadeCanvasGroup.gameObject.SetActive(true);

        // 2. 자동 시작이 켜져있다면, 설정한 대기 시간(Delay) 이후에 애니메이션 실행
        if (autoStart)
        {
            DOVirtual.DelayedCall(startDelay, () => PlayFadeIn());
        }
    }

    // 🌟 이 함수는 이제 외부 스크립트에서도 원할 때 직접 호출할 수 있습니다.
    public void PlayFadeIn()
    {
        // Canvas Group 알파값 투명해지기 + 완료 시 캔버스 끄기
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.DOFade(0f, transitionDuration)
                .SetEase(Ease.InOutSine)
                .SetLink(fadeCanvasGroup.gameObject) 
                .OnComplete(() => 
                {
                    if (transitionCanvas != null) 
                    {
                        transitionCanvas.SetActive(false);
                    }
                    Debug.Log("씬 시작 트랜지션 완료! 캔버스를 비활성화했습니다.");
                });
        }

        // 볼륨의 Weight 값을 1에서 0으로 서서히 깎아 블러 걷어내기
        if (blurVolume != null)
        {
            DOTween.To(() => blurVolume.weight, x => blurVolume.weight = x, 0f, transitionDuration)
                .SetEase(Ease.InOutSine)
                .SetLink(blurVolume.gameObject); 
        }
    }
}