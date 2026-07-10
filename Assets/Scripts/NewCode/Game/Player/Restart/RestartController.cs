using UnityEngine.SceneManagement;

namespace Garden
{
    public class RestartController
    {
        public void Restart()
        {
            SignalBusCleaner.ClearAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}