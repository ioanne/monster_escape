using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public static CharacterSpawner Instance { get; private set; }

    [Header("Character Settings")]
    public GameObject characterPrefab;
    public Vector3 spawnPosition;
    private GameObject currentCharacter;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of CharacterSpawner
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SpawnCharacter();
    }

    public void SpawnCharacter()
    {
        if (currentCharacter != null)
        {
            Debug.LogWarning("Character already exists. Not spawning a new one.");
            return;
        }

        if (characterPrefab == null)
        {
            Debug.LogError("Character prefab is not assigned.");
            return;
        }

        // Instantiate the character at the spawn position
        currentCharacter = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
        currentCharacter.SetActive(true);
        Debug.Log("Spawned character at position: " + spawnPosition);

        // Ensure the character has the Character component
        Character characterComponent = currentCharacter.GetComponent<Character>();
        if (characterComponent == null)
        {
            Debug.LogWarning("Character component not found on the spawned character.");
        }
    }
}
