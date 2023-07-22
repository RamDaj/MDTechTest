using System.Collections;
using UnityEngine;

public class TalkingGirl : MonoBehaviour, IInteractable
{
    //Hidden phone Amy uses to call
    [SerializeField] GameObject phone;

    [SerializeField] TextToSpeech tts;

    //All of Amy's voice lines
    [SerializeField] string[] Sentences;

    private Animator _animator;
    bool _hasAnimator;
    // Start is called before the first frame update
    void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);

        //Tells the player "Come here"
        tts.createAudioClip(Sentences[0]);
        GameManager.Instance.IncrementGameState();
    }

    //Times the talking animation with Amy's speech.
    public IEnumerator waitForSound()
    {
        int gameState = GameManager.Instance.GetGameState();
        //Amy will remain in the calling animation
        if (_hasAnimator && gameState == 3)
        {
            _animator.SetTrigger("Call");
            GameManager.Instance.IncrementGameState();
        }
        else if (_hasAnimator && gameState < 3) _animator.SetTrigger("startTalking");
        //Wait Until Sound has finished playing
        while (tts.audioSource.isPlaying)
        {
            yield return null;
        }

        //Auidio has finished playing, return to idle
        if (_hasAnimator && gameState < 3) _animator.SetTrigger("stopTalking");
    }

    public bool Interact(Interactor player)
    {
        if (!CanInteract()) return false;

        GameManager manager = GameManager.Instance;
        int gameState = manager.GetGameState();

        //Select voice line, Amy's behaviour depends on the game state
        switch (gameState)
        {
            //Waiting for her phone
            case 2: 
                //Player has Amy's phone
                if (manager.phoneAquired) 
                {
                    manager.IncrementGameState();
                    StartCoroutine(activatePhone());
                }
                //Amy either scolds you or thanks you if you have her phone or not
                tts.createAudioClip(Sentences[manager.GetGameState()]);
                break;
            //Calls friend
            case 3:
                manager.IncrementGameState();
                tts.createAudioClip(Sentences[manager.GetGameState()]);
                break;
            case 11:
                tts.createAudioClip(Sentences[gameState]);
                break;
            //Default case, just say Sentence #gameState and move on to the next state
            default:
                tts.createAudioClip(Sentences[gameState]);
                manager.IncrementGameState();
                break;
        }
        return true;
    }

    //Giving Amy her phone back
    IEnumerator activatePhone()
    {
        //Waits for amy to extend her palm
        yield return new WaitForSeconds(0.5f);
        phone.SetActive(true);
    }

    //Amy can't be interrupted mid-speech
    public bool CanInteract()
    {
        return !tts.audioSource.isPlaying;
    }
}
