using UnityEngine;
using System.Collections;

public abstract class Fighter : MonoBehaviour
{
    public string idName;
    public StatusPanel statusPanel;

    public CombatManager combatManager;

    protected Stats stats;
    public Stats Stats => stats;


    protected SkillInteraction[] skills;

    public BodyStatus bodyStatus = new BodyStatus();


    public bool isAlive
    {
        get => this.stats.health > 0;
    }

    public bool isTurnFinished = false;

    public bool hasGuaranteedCrit = false;
    public bool hasDefenseBuff = false;

    protected virtual void Start()
    { 
        this.statusPanel.SetStats(this.idName, this.stats);
        this.skills = this.GetComponentsInChildren<SkillInteraction>();
    }

    public struct HealthResult
    {
        public float finalValue;
        public bool blocked;
    }

    public HealthResult ModifyHealth(float amount)
    {
        HealthResult result = new HealthResult();
        result.finalValue = amount;
        result.blocked = false;

        if (amount < 0 && this.hasDefenseBuff)
        {
            result.finalValue = amount * 0.5f;
            this.hasDefenseBuff = false;
            result.blocked = true;
        }

        this.stats.health = Mathf.Clamp(this.stats.health + result.finalValue, 0f, this.stats.maxHealth);
        this.statusPanel.SetHealth(this.stats.health, this.stats.maxHealth);

        return result;
    }

    public Stats GetCurrentStats()
    {
        return this.stats;
    }

    public SkillInteraction GetSkillByIndex(int index)
    {
        if (index >= 0 && index < skills.Length)
            return skills[index];

        Debug.LogWarning($"{idName} intentó acceder a una habilidad inválida en el índice {index}.");
        return null;
    }
    public void ListSkills()
    {
        Debug.Log($"--- Habilidades de {idName} ---");

        for (int i = 0; i < skills.Length; i++)
        {
            string nombre = string.IsNullOrEmpty(skills[i].interactionName) ? "(Sin nombre)" : skills[i].interactionName;
            Debug.Log($"[{i}] {nombre}");
        }

        Debug.Log("-------------------------------");
    }

    public IEnumerator HandleBodyStatusCoroutine()
    {
        if (bodyStatus.IsDead)
        {
            LogPanel.Write($"{idName} ha muerto y no puede actuar.");
            isTurnFinished = true;
            yield break;
        }

        // Sangrado con delay para mostrar en Log por separado
        if (bodyStatus.IsBleeding)
        {
            yield return new WaitForSeconds(2.5f);
            ModifyHealth(-5);
            LogPanel.Write($"{idName} sufre 5 de daño por sangrado.");
        }

        bodyStatus.TickTurn();
        yield break;
    }

    public abstract void InitTurn();
}