using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;



public class GameOverUI : MonoBehaviour {

    public void Quit()
    {
        Debug.Log("APPLICATION QUIT");
        Application.Quit();
            
    }

    public void Retry()
    {
        Debug.Log("APPLICATION RESTART");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
