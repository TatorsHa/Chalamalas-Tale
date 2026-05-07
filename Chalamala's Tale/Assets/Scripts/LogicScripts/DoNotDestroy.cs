using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DoNotDestroy : MonoBehaviour
{
    private static HashSet<string> existing = new HashSet<string>();

    // to avoid having 2 minimaps at the same time, when we exit the tutorial, its map is destroyed
    [Header("Destroy Settings")]
    public string destroyInScene; // Scene name
    public GameObject uiToDestroy; // UI object to destroy

    void Awake()
    {
        string key = GetKey();

        if (!existing.Contains(key))
        {
            existing.Add(key);
            DontDestroyOnLoad(gameObject);

            // Listen for scene changes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // checks the current scene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the target scene is loaded
        if (scene.name == destroyInScene)
        {
            if (uiToDestroy != null)
            {
                Destroy(uiToDestroy);
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    string GetKey()
    {
        // clé basée sur le type du GameObject
        return gameObject.name;
    }
}