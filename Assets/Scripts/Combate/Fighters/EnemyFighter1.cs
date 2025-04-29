using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFighter : Fighter
{
    private List<SkillInteraction> attackSkills = new();
    private List<SkillInteraction> buffSkills = new();
    private List<SkillInteraction> healSkills = new();
    [SerializeField] private int Damage;
    [SerializeField] private int Health;

    void Awake()
    {
        this.stats = new Stats(Health, Damage);
    }

    protected override void Start()
    {
        base.Start();

        // Clasificar habilidades al iniciar
        foreach (var skill in skills)
        {
            switch (skill.enemySkillType)
            {
                case EnemySkillType.ATTACK: attackSkills.Add(skill); break;
                case EnemySkillType.BUFF: buffSkills.Add(skill); break;
                case EnemySkillType.HEAL: healSkills.Add(skill); break;
            }
        }
    }

    public override void InitTurn()
    {
        StartCoroutine(HandleBodyStatusCoroutine());
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1.5f);

        var target = FindPlayer();
        int decision = Random.Range(0, 100);

        if (decision < 30 && Stats.health < Stats.maxHealth * 0.8f && healSkills.Count > 0)
        {
            var heal = healSkills[Random.Range(0, healSkills.Count)];
            LogPanel.Write($"{idName} usó una habilidad de curación.");
            heal.Run(this);
            yield return new WaitForSeconds(1.2f);
            isTurnFinished = true;
            yield break;
        }

        if (decision < 60 && buffSkills.Count > 0 && !hasGuaranteedCrit)
        {
            var buff = buffSkills[Random.Range(0, buffSkills.Count)];
            LogPanel.Write($"{idName} entra en estado de furia.");
            buff.Run(this);
            yield return new WaitForSeconds(2.5f);
            isTurnFinished = true;
            yield break;
        }

        if (attackSkills.Count > 0)
        {
            var attack = attackSkills[Random.Range(0, attackSkills.Count)];
            attack.SetTarget(target);
            LogPanel.Write($"{idName} ataca a {target.idName}.");
            attack.Run(this);
        }

        yield return new WaitForSeconds(2.5f);
        isTurnFinished = true;
    }

    private Fighter FindPlayer()
    {
        foreach (var f in Object.FindObjectsByType<Fighter>(FindObjectsSortMode.None))
        {
            if (f != this)
                return f;
        }
        return null;
    }
}