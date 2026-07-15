using UnityEngine;
using UnityEngine.Video;
using System.IO; // 경로 합성을 위해 필수

public class StreamingVideoLoader : MonoBehaviour
{
    [Header("비디오 플레이어 연결")]
    [SerializeField] private VideoPlayer myVideoPlayer;

    [Header("영상 파일 이름 (확장자 포함)")]
    [Tooltip("StreamingAssets 폴더 안에 있는 영상 이름을 정확히 적어주세요. 예: IntroVideo.mp4")]
    [SerializeField] private string videoFileName = "Video1_Intro.mp4"; 

    void Start()
    {
        if (myVideoPlayer == null) return;

        // 1. 현재 기기/플랫폼에 맞는 StreamingAssets 폴더의 절대 경로를 가져옵니다.
        // 2. 그 경로 뒤에 파일 이름을 합쳐서 완벽한 주소를 만듭니다.
        string videoPath = Path.Combine(Application.streamingAssetsPath, videoFileName);

        // 3. 비디오 플레이어의 URL에 주소를 넣고 재생시킵니다.
        myVideoPlayer.url = videoPath;
        myVideoPlayer.Play();
    }
}