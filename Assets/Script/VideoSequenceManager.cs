using UnityEngine;
using UnityEngine.Video;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class VideoSequenceManager : MonoBehaviour
{
    [Header("비디오 플레이어")]
    public VideoPlayer firstVideo;  // 첫 번째 영상 (인트로)
    public VideoPlayer secondVideo; // 두 번째 영상 (루프)

    [Header("StreamingAssets 영상 파일명")]
    [Tooltip("확장자(.mp4)까지 정확히 적어주세요.")]
    public string firstVideoFileName = "Video1_Intro.mp4";
    public string secondVideoFileName = "Video2_Loop.mp4";

    [Header("캔버스 그룹 (투명도 조절용)")]
    public CanvasGroup firstVideoCanvas;  
    public CanvasGroup secondVideoCanvas; 
    
    [Header("로딩 패널 및 UI")]
    public CanvasGroup loadingPanel;
    public TextMeshProUGUI progressText;

    [Header("오브젝트 제어")]
    [Tooltip("첫 번째 영상이 끝날 때 끌(비활성화할) 오브젝트들을 개수에 맞게 넣으세요.")]
    public GameObject[] objectsToDisable; 

    [Header("이벤트 연출")]
    [Tooltip("두 번째 영상이 시작될 때 실행할 DOTween 연출을 연결하세요.")]
    public UnityEvent onSecondVideoStart; 

    private Tween progressTween;
    private bool isTransitioned = false; // ⭐️ 중복 실행 방지용 플래그

    void Start()
    {
        if (firstVideoCanvas != null) firstVideoCanvas.alpha = 1f;
        if (secondVideoCanvas != null) secondVideoCanvas.alpha = 0f;

        string firstUrl = System.IO.Path.Combine(Application.streamingAssetsPath, firstVideoFileName).Replace("\\", "/");
        string secondUrl = System.IO.Path.Combine(Application.streamingAssetsPath, secondVideoFileName).Replace("\\", "/");
        
        firstVideo.url = firstUrl;
        secondVideo.url = secondUrl;

        if (progressText != null) progressText.text = "0%";
        progressTween = DOVirtual.Float(0f, 90f, 2f, (value) => 
        {
            if (progressText != null)
                progressText.text = Mathf.RoundToInt(value).ToString() + "%";
        }).SetEase(Ease.OutQuad);

        secondVideo.Prepare();
        
        firstVideo.prepareCompleted += OnFirstVideoReady;
        firstVideo.loopPointReached += OnFirstVideoFinished;
        
        firstVideo.Prepare();
    }

    void OnFirstVideoReady(VideoPlayer vp)
    {
        if (progressTween != null) progressTween.Kill();
        if (progressText != null) progressText.text = "100%";

        firstVideo.Play();
        
        DOVirtual.DelayedCall(0.3f, () => 
        {
            if (loadingPanel != null)
            {
                loadingPanel.DOFade(0f, 1f).OnComplete(() => {
                    loadingPanel.gameObject.SetActive(false);
                });
            }
        });
    }

    // 영상이 끝까지 재생되었을 때 자동 호출
    void OnFirstVideoFinished(VideoPlayer vp)
    {
        ExecuteVideoTransition();
    }

    // ⭐️ 버튼 클릭 시 호출할 수 있는 스킵 함수 (외부 접근을 위해 public으로 선언)
    public void SkipIntroVideo()
    {
        // 아직 전환되지 않았다면
        if (!isTransitioned)
        {
            // 첫 번째 영상이 재생 중이라면 즉시 강제 정지
            if (firstVideo.isPlaying)
            {
                firstVideo.Stop();
            }
            
            // 즉시 전환 로직 실행
            ExecuteVideoTransition();
        }
    }

    // ⭐️ 흩어져 있던 전환 로직을 하나의 함수로 통합
    private void ExecuteVideoTransition()
    {
        // 이미 넘어갔다면(버튼 연타 등) 무시
        if (isTransitioned) return;
        isTransitioned = true; // 전환 완료 상태로 체크

        secondVideo.Play();

        if (secondVideoCanvas != null) secondVideoCanvas.alpha = 1f; 
        if (firstVideoCanvas != null) firstVideoCanvas.alpha = 0f;  

        if (objectsToDisable != null)
        {
            foreach (GameObject obj in objectsToDisable)
            {
                if (obj != null) obj.SetActive(false);
            }
        }

        onSecondVideoStart?.Invoke();
    }
}