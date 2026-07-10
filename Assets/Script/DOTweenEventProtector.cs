using UnityEngine;

public class DOTweenEventProtector : MonoBehaviour
{
    [Header("제어할 타겟 UI 오브젝트")]
    [SerializeField] private GameObject targetObject;

    /// <summary>
    /// 오브젝트가 실제로 켜져(활성화) 있을 때만 안전하게 SetActive를 실행합니다.
    /// </summary>
    public void SafeSetActive(bool isActive)
    {
        if (targetObject == null) return;

        // 🌟 핵심 안전장치: 이 컴포넌트나 오브젝트가 인스펙터에서 꺼져 있다면 
        // 두트윈이 리와인드를 하더라도 아래 코드를 실행하지 않고 씹습니다.
        if (!gameObject.activeInHierarchy) return;

        // 이미 원하는 상태와 같다면 무한 루프 에러 방지를 위해 패스
        if (targetObject.activeSelf == isActive) return;

        targetObject.SetActive(isActive);
    }
}