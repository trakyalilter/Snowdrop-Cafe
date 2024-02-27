using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject[] tabs; // Assign your tab content panels here
    public Button[] tabButtons; // Assign your tab buttons here

    public void OpenTab(int tabIndex)
    {
        // Deactivate all tabs
        foreach (var tab in tabs)
        {
            tab.SetActive(false);
        }

        // Activate the selected tab
        tabs[tabIndex].SetActive(true);
    }

    private void Start()
    {
        // Initialize the tabs (e.g., open the first tab by default)
        OpenTab(0);

        // Optionally, add listeners to your tab buttons
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int closureIndex = i; // Prevents the infamous closure issue in loops
            tabButtons[i].onClick.AddListener(() => OpenTab(closureIndex));
        }
    }
}
