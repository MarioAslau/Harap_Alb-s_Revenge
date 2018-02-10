
using UnityEngine;
//using System.Collections;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{

    [SerializeField]
    WaveSpawner spawner; //refference for wavespawner that keeps track of countdown,state,etc

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountdownText;

    [SerializeField]
    Text waveCountText;

    /*
     We need to trigger the animation. Inside the UpdateCountingUI, we will trigger the animation only once. We will check if we have switched to the state.
     we store the previous state and compare it to the actual state
      */

    private WaveSpawner.SpawnState previousState;

    // Use this for initialization
    void Start()
    {
        if (spawner == null)
        {
            Debug.LogError("No spawner referenced!");
            this.enabled = false;

        }

        if (waveAnimator == null)
        {
            Debug.LogError("No  waveAnimator referenced!");
            this.enabled = false;

        }

        if (waveCountdownText == null)
        {
            Debug.LogError("No waveCountdownText referenced!");
            this.enabled = false;

        }

        if (waveCountText == null)
        {
            Debug.LogError("No waveCountText referenced!");
            this.enabled = false;

        }
    }

    
    void Update()
    {
        switch (spawner.State)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
        }

        previousState = spawner.State;

    }

    void UpdateCountingUI()
    {
        if (previousState != WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
           // Debug.Log("COUNTING");
        }
        waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();//updating the text
    }

    void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
             waveAnimator.SetBool("WaveCountdown", false);
             waveAnimator.SetBool("WaveIncoming", true);

            waveCountText.text = spawner.NextWave.ToString();

            // Debug.Log("SPAWNING");
        }
     

    }
}
