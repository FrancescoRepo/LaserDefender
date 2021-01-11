using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    private int _score;
    private float _health;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        Debug.Log("Setup singleton");
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log(gameObject);
        }
    }

    public int GetScore()
    {
        return _score;
    }

    public void AddScore(int scoreValue)
    {
        _score += scoreValue;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
    
}
