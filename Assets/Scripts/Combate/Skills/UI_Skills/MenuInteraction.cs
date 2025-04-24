using UnityEngine;

public class MenuInteraction : Interaction
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject actionPanel;

    private bool isMenuOpen = false;

    public override void Run(Fighter emitter)
    {
        menuPanel.SetActive(true);
        actionPanel?.SetActive(false);
        isMenuOpen = true;

        var zona = menuPanel.GetComponent<ZonaMenu>();
        if (zona != null)
            zona.SetEmitter(emitter);

        StartCoroutine(WatchForCancel());
    }

    private System.Collections.IEnumerator WatchForCancel()
    {

        yield return null;

        while (isMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseSubMenu();
            }
            yield return null;
        }
    }

    public void CloseSubMenu()
    {
        menuPanel.SetActive(false);
        actionPanel?.SetActive(true);
        isMenuOpen = false;
    }
}