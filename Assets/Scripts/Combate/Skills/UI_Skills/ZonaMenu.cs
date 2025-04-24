using UnityEngine;

public class ZonaMenu : MonoBehaviour
{
    private Fighter emitter;

    public void SetEmitter(Fighter fighter)
    {
        this.emitter = fighter;
    }

    public void OnAttackHead() => StartCoroutine(RunSkillByIndex(2));
    public void OnAttackBody() => StartCoroutine(RunSkillByIndex(3));
    public void OnAttackArm() => StartCoroutine(RunSkillByIndex(4));

    private System.Collections.IEnumerator RunSkillByIndex(int index)
    {
        if (emitter == null)
        {
            Debug.LogWarning("Emitter no asignado.");
            yield break;
        }

        var skill = emitter.GetSkillByIndex(index);
        if (skill == null) yield break;

        skill.SetTarget(GetEnemy());
        skill.Run(emitter);
        var group = GetComponent<CanvasGroup>();
        if (group != null)
        {
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        yield return new WaitForSeconds(2.5f);

        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;

        this.gameObject.SetActive(false);

        emitter.isTurnFinished = true;
    }

    private Fighter GetEnemy()
    {
        var all = Object.FindObjectsByType<Fighter>(FindObjectsSortMode.None);
        foreach (var f in all)
        {
            if (f != emitter)
                return f;
        }
        return null;
    }
}