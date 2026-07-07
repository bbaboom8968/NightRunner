using UnityEngine;
using UnityEngine.SceneManagement; // ⭐️ 씬(Scene) 관리를 위해 필수
using DG.Tweening;                 // DOTween 제어용

public class SceneResetManager : MonoBehaviour
{
    [Header("리셋 설정")]
    [Tooltip("체크하면 현재 씬을 다시 로드합니다. (하나의 씬에서 작업 중일 때 추천)")]
    public bool reloadCurrentScene = true;
    
    [Tooltip("다른 씬으로 가야 한다면 위 체크를 끄고 씬 이름을 적어주세요.")]
    public string targetSceneName = "MainScene";

    // ⭐️ 버튼 클릭 시 실행될 함수 (외부에서 연결할 수 있게 public 설정)
    public void ResetGame()
    {
        // 1. 씬을 다시 불러오기 전, 실행 중이던 모든 DOTween 연출을 강제 종료 (메모리 누수 및 에러 방지)
        DOTween.KillAll();

        // 2. 씬 로드
        if (reloadCurrentScene)
        {
            // 현재 화면에 켜져 있는 씬을 알아내서 처음부터 다시 불러옵니다.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            // 지정한 이름의 다른 씬으로 이동합니다.
            SceneManager.LoadScene(targetSceneName);
        }
    }
}