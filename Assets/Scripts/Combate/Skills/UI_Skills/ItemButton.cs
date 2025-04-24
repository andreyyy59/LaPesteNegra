using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public PlayerFighter player;
    [SerializeField] private GameObject itemMenu;
    private CanvasGroup group;

    public void UseThisItem()
    {
        var skill = GetComponent<SkillInteraction>();
        if (skill != null)
        {
            skill.Run(player);

            group = itemMenu.GetComponent<CanvasGroup>();

            if (itemMenu != null)
            {
                
                if (group != null)
                {
                    group.alpha = 0f;
                    group.interactable = false;
                    group.blocksRaycasts = false;
                }
            }

            StartCoroutine(FinishAfterDelay(2.5f));
        }
    }

    private System.Collections.IEnumerator FinishAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (group != null)
        {
            group.alpha = 1f;
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        if (itemMenu != null)
        itemMenu.SetActive(false);

        player.isTurnFinished = true;
    }
}