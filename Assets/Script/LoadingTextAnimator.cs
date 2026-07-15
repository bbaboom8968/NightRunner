using UnityEngine;
using TMPro;
using DG.Tweening;

public class LoadingTextAnimator : MonoBehaviour
{
    [Header("텍스트 연결")]
    [SerializeField] private TextMeshProUGUI loadingText;

    [Header("연출 설정")]
    [Tooltip("기본 텍스트 (예: Loading)")]
    [SerializeField] private string baseText = "Loading";
    
    [Tooltip("점이 하나씩 추가되는 시간 간격")]
    [SerializeField] private float dotInterval = 0.3f;
    
    [Tooltip("최대 점 개수")]
    [SerializeField] private int maxDots = 3;

    private Tween dotTween;
    private int currentDotCount = 0;

    void OnEnable()
    {
        if (loadingText == null) return;

        // 시작할 때 점 개수 초기화
        currentDotCount = 0;
        loadingText.text = baseText;

        // DOTween을 이용한 무한 반복 타이머 생성
        dotTween = DOVirtual.Float(0, 1, dotInterval, _ => { })
            .SetLoops(-1, LoopType.Restart) // -1은 무한 반복을 의미합니다.
            .OnStepComplete(() => 
            {
                // 간격(dotInterval)마다 점 개수를 증가시키고, 최대치를 넘으면 다시 0으로 만듦
                currentDotCount = (currentDotCount + 1) % (maxDots + 1);
                
                // baseText 뒤에 점(.)을 currentDotCount 개수만큼 붙여서 출력
                loadingText.text = baseText + new string('.', currentDotCount);
            })
            .SetLink(gameObject); // 오브젝트가 파괴되면 트윈도 안전하게 종료
    }

    void OnDisable()
    {
        // UI가 꺼지면 불필요한 연산이 돌아가지 않도록 애니메이션을 확실히 죽여줍니다.
        if (dotTween != null)
        {
            dotTween.Kill();
        }
    }
}