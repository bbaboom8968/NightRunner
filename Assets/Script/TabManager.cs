using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용을 위해 필수

public class TabManager : MonoBehaviour
{
    [Header("탭 1 (왼쪽) 설정")]
    [SerializeField] private Button tab1Button;
    [SerializeField] private GameObject tab1ActiveImage; // 켜졌을 때 보일 배경/밑줄 이미지
    [SerializeField] private TextMeshProUGUI tab1Text;

    [Header("탭 2 (오른쪽) 설정")]
    [SerializeField] private Button tab2Button;
    [SerializeField] private GameObject tab2ActiveImage;
    [SerializeField] private TextMeshProUGUI tab2Text;

    [Header("텍스트 컬러 설정")]
    [SerializeField] private Color activeTextColor = Color.white;   // 선택된 탭의 글자색
    [SerializeField] private Color inactiveTextColor = Color.gray;  // 선택 안 된 탭의 글자색

    void Start()
    {
        // 버튼 클릭 이벤트 연결
        if (tab1Button != null) tab1Button.onClick.AddListener(SelectTab1);
        if (tab2Button != null) tab2Button.onClick.AddListener(SelectTab2);

        // 시작할 때 기본으로 탭 1을 선택한 상태로 초기화
        SelectTab1();
    }

    // 탭 1을 눌렀을 때
    public void SelectTab1()
    {
        UpdateTabVisuals(true);
    }

    // 탭 2를 눌렀을 때
    public void SelectTab2()
    {
        UpdateTabVisuals(false);
    }

    /// <summary>
    /// 실제 이미지와 텍스트 컬러를 업데이트하는 핵심 함수
    /// </summary>
    private void UpdateTabVisuals(bool isTab1Active)
    {
        // 1. 활성화 이미지 On/Off
        tab1ActiveImage.SetActive(isTab1Active);
        tab2ActiveImage.SetActive(!isTab1Active);

        // 2. 텍스트 컬러 변경
        tab1Text.color = isTab1Active ? activeTextColor : inactiveTextColor;
        tab2Text.color = !isTab1Active ? activeTextColor : inactiveTextColor;

        // 3. (선택 사항) 이미 선택된 탭은 다시 못 누르게 버튼 비활성화 처리
        tab1Button.interactable = !isTab1Active;
        tab2Button.interactable = isTab1Active;
    }
}