using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string currentSceneName;
    [SerializeField] private string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadSceneManager.instance.LoadNextLevel(currentSceneName, nextSceneName);
        }
    }
}
