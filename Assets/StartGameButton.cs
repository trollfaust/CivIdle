using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void OnButtonPressed(int i)
    {
        SceneManager.LoadScene(i);
    }
}
