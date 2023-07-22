using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Does the player have Amy's phone?
    public bool phoneAquired = false;

    //Created a singleton to acess GameManager from anywhere
    public static GameManager Instance { get; private set; }

    int _gameState = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    //Amy's current voice line
    public int GetGameState()
    {
        return _gameState;
    }

    //Amy's voice lines depend on the current state, when the previous state is satisfied move onto the next
    public int IncrementGameState()
    {
        return _gameState++;
    }
}
