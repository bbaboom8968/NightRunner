using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필수 추가
using DG.Tweening;

public class TVTransition : MonoBehaviour
{
    [Header("트랜지션 대상 UI")]
    [Tooltip("작아지면서 사라질 메인 컨텐츠(또는 화면 캡처 이미지)를 넣으세요.")]
    [SerializeField] private RectTransform targetScreen;
    
    [Header("씬 전환 설정")]
    [Tooltip("이동할 다음 씬의 이름을 정확히 입력하세요.")]
    [SerializeField] private string nextSceneName = "NextScene";

    [Header("연출 설정")]
    [SerializeField] private float totalDuration = 0.5f;

    public void PlayTVOffEffectAndLoadScene()
    {
        if (targetScreen == null) return;

        // 시작 전 스케일 초기화
        targetScreen.localScale = Vector3.one;

        // DOTween 시퀀스 생성
        Sequence tvSequence = DOTween.Sequence();
        
        // 🌟 타임스케일 무시: 게임이 멈춰있어도 트랜지션은 무조건 재생되도록 설정
        tvSequence.SetUpdate(true); 

        // 1단계: Y축(세로)으로 빠르게 납작해지기
        tvSequence.Append(targetScreen.DOScaleY(0.01f, totalDuration * 0.7f).SetEase(Ease.InCubic));

        // 2단계: X축(가로)이 점으로 사라지기
        tvSequence.Append(targetScreen.DOScaleX(0f, totalDuration * 0.3f).SetEase(Ease.OutQuint));

        // 🌟 연출 완료 후 씬 전환 실행!
        tvSequence.OnComplete(() =>
        {
            Debug.Log($"TV 연출 완료! [{nextSceneName}] 씬으로 넘어갑니다.");
            SceneManager.LoadScene(nextSceneName);
        });
    }
}