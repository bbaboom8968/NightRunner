using UnityEngine;
using UnityEngine.UI;

public class AudioMuteController : MonoBehaviour
{
    [Header("버튼 이미지 (선택 사항)")]
    [Tooltip("버튼의 이미지를 바꿀 거라면 UI의 Image 컴포넌트를 연결하세요.")]
    public Image buttonImage;
    public Sprite soundOnIcon;  // 소리 켜짐 이미지 (예: 🔊)
    public Sprite soundOffIcon; // 소리 꺼짐 이미지 (예: 🔇)

    private bool isMuted = false;

    // ⭐️ 버튼을 누를 때마다 실행될 함수
    public void ToggleMute()
    {
        isMuted = !isMuted; // 상태 뒤집기 (false -> true -> false...)

        if (isMuted)
        {
            // 유니티 전체 마스터 볼륨 0으로 끄기
            AudioListener.volume = 0f; 
            
            if (buttonImage != null && soundOffIcon != null)
                buttonImage.sprite = soundOffIcon;
        }
        else
        {
            // 유니티 전체 마스터 볼륨 1(최대)로 켜기
            AudioListener.volume = 1f; 
            
            if (buttonImage != null && soundOnIcon != null)
                buttonImage.sprite = soundOnIcon;
        }
    }
}