using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;

    // Public accessor for the singleton instance
    public static GameManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // Destroy duplicate instance
            Destroy(gameObject);
            return;
        }

        // Set the instance
        instance = this;

        // Keep the GameManager object throughout scenes
        DontDestroyOnLoad(gameObject);

    }

    private void OnDestroy()
    {
        // Clear the instance if it's being destroyed
        if (instance == this)
        {
            instance = null;
        }
    }
}
