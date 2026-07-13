using UnityEngine;
using TMPro; // TextMeshPro 필수
using DG.Tweening;

public class DynamicTextAnimator : MonoBehaviour
{
    [Header("타겟 텍스트 컴포넌트")]
    [SerializeField] private TextMeshProUGUI targetText;

    [Header("연출 설정")]
    [Tooltip("글자가 모두 바뀌는 데 걸리는 시간")]
    [SerializeField] private float duration = 0.4f;

    [Tooltip("글자가 바뀔 때 무작위 문자가 섞이는 스크램블 효과")]
    [SerializeField] private ScrambleMode scrambleMode = ScrambleMode.Uppercase;

    // 현재 재생 중인 트윈을 추적하여 충돌을 막기 위한 변수
    private Tween currentTextTween;

    /// <summary>
    /// 버튼의 OnClick 이벤트에서 직접 호출할 함수
    /// 인스펙터에서 버튼마다 원하는 문자열을 직접 타이핑해서 넣어줍니다.
    /// </summary>
    public void ChangeTextWithAnim(string newText)
    {
        if (targetText == null) return;

        // 🌟 1. 핵심 방어 로직: 기존에 글자가 바뀌고 있던 중이라면 즉시 취소하고 파괴
        if (currentTextTween != null && currentTextTween.IsActive())
        {
            currentTextTween.Kill();
        }

        // 🌟 2. 빈 텍스트로 초기화 후 새로운 글자로 DOText 실행
        // (글자가 완전히 지워졌다가 다시 써지는 연출을 원한다면 string.Empty 할당)
        // targetText.text = ""; 

        // 3. DOText 실행: newText를 향해 duration 동안 스크램블하며 변환
        currentTextTween = targetText.DOText(newText, duration, true, scrambleMode, null)
            .SetEase(Ease.Linear); // 텍스트 타이핑 연출은 Linear가 가장 자연스럽습니다.
    }
}