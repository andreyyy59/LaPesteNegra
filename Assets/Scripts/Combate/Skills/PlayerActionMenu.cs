using UnityEngine;
using System.Collections;

public class PlayerActionMenu : MonoBehaviour
{
    public GameObject actionPanel;
    public PlayerFighter player;

    public MenuInteraction attackMenu;
    public MenuInteraction itemMenu;

    public void StartPlayerTurn()
    {
        actionPanel.SetActive(true);
    }

    public void OnBasicAttackPressed()
    {
        var skill = player.GetSkillByIndex(0);
        skill.SetTarget(GetEnemy());
        skill.Run(player);

        Hide();
        StartCoroutine(FinishAfterDelay(2.5f));
    }
    public void OnDefendPressed()
    {
        var skill = player.GetSkillByIndex(1);
        skill.Run(player);

        Hide();
        StartCoroutine(FinishAfterDelay(2.5f));
    }
    public void OnAttackPressed()
    {
        Hide();
        attackMenu.Run(player);
    }
    public void OnItemPressed()
    {
        Hide();
        itemMenu.Run(player);
    }

    private Fighter GetEnemy()
    {
        var all = Object.FindObjectsByType<Fighter>(FindObjectsSortMode.None);
        foreach (var f in all)
        {
            if (f != player)
                return f;
        }
        return null;
    }

    private IEnumerator FinishAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.isTurnFinished = true;
    }

    public void Hide()
    {
        actionPanel.SetActive(false);
    }
}