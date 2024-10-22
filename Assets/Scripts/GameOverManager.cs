using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RestartGame() {
        // รีเซ็ตค่าเลือดตัวละคร
        PlayerHealth.Instance.ResetHealth();

        // โหลดฉากใหม่
        SceneManager.LoadScene("Scene 1");
    }
}