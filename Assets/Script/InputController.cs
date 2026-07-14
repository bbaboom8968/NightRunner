using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용을 위해 필수

public class InputController : MonoBehaviour
{
    [Header("UI 컴포넌트 연결")]
    [SerializeField] private TMP_InputField myInputField; // 유저가 글자를 칠 입력창
    [SerializeField] private Button confirmButton;        // 활성화/비활성화 될 확인 버튼

    [Header("버튼 텍스트 설정")]
    [SerializeField] private TextMeshProUGUI buttonText;  // 버튼 안에 있는 텍스트
    [SerializeField] private Color activeTextColor = Color.white;  // 활성화 시 텍스트 색상
    [SerializeField] private Color inactiveTextColor = Color.gray; // 비활성화 시 텍스트 색상

    void Start()
    {
        // 1. 시작할 때 버튼과 텍스트 컬러를 비활성화 상태로 초기화합니다.
        if (confirmButton != null)
        {
            confirmButton.interactable = false;
        }
        
        if (buttonText != null)
        {
            buttonText.color = inactiveTextColor;
        }

        // 2. 입력창의 글자가 바뀔 때마다 OnTextChanged 함수를 실행하도록 연결합니다.
        if (myInputField != null)
        {
            myInputField.onValueChanged.AddListener(OnTextChanged);
        }
    }

    /// <summary>
    /// InputField에 타자가 쳐질 때마다 실시간으로 발동하는 함수
    /// </summary>
    private void OnTextChanged(string inputText)
    {
        // 공백(스페이스바)이 아닌 유효한 글자가 있는지 체크
        bool hasValidText = !string.IsNullOrWhiteSpace(inputText);

        // 3. 버튼 자체의 활성화/비활성화 상태 변경
        if (confirmButton != null)
        {
            confirmButton.interactable = hasValidText;
        }

        // 🌟 4. 버튼 안의 텍스트 색상 동기화
        if (buttonText != null)
        {
            buttonText.color = hasValidText ? activeTextColor : inactiveTextColor;
        }
    }
}