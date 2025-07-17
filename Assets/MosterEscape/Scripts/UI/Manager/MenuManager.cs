using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Paneles")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject configPanel;

    private bool isPaused = false;
    private bool isInventoryOpen = false;
    private bool isConfigOpen = false;

    public bool IsPaused => isPaused;
    public bool IsInventoryOpen => isInventoryOpen;
    public bool IsConfigOpen => isConfigOpen;
    public bool IsAnyMenuOpen => isPaused || isInventoryOpen || isConfigOpen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        HideAllMenus();
        LockCursor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleConfig();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        ShowCursor();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        HideCursorIfNeeded();
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
        if (isInventoryOpen) ShowCursor(); else HideCursorIfNeeded();
    }

    private void ToggleConfig()
    {
        isConfigOpen = !isConfigOpen;
        configPanel.SetActive(isConfigOpen);
        if (isConfigOpen) ShowCursor(); else HideCursorIfNeeded();
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HideCursorIfNeeded()
    {
        if (!isPaused && !isInventoryOpen && !isConfigOpen)
        {
            LockCursor();
        }
    }

    private void HideAllMenus()
    {
        pauseMenu?.SetActive(false);
        inventoryPanel?.SetActive(false);
        configPanel?.SetActive(false);
    }
}
