using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemySkillType
{
    ATTACK,
    BUFF,
    HEAL
}

public abstract class SkillInteraction : Interaction
{
    [Header("Base Skill")]
    public float animationDuration;
    public bool selfInflicted;
    public GameObject effectPrfb;

    [Header("Enemy Usage")]
    public EnemySkillType enemySkillType;

    [Header("Tipo de habilidad")]
    public bool isSpecial = false;

    [Header("Requisitos físicos")]
    [Min(0)] public int requiredArms = 0;     // 0 = no requiere brazos
    [Min(0)] public int requiredLegs = 0;     // 0 = no requiere piernas
    public bool requiresHead = false;

    protected Fighter emitter;
    protected Fighter receiver;

    public void SetTarget(Fighter receiver)
    {
        this.receiver = receiver;
    }

    public override void Run(Fighter emitter)
    {
        this.emitter = emitter;
        if (selfInflicted)
            receiver = emitter;

        // ✅ Validación de requisitos físicos
        if (!MeetsRequirements(emitter.bodyStatus))
        {
            LogPanel.Write($"{emitter.idName} no puede ejecutar {interactionName} por falta de condiciones físicas.");
            return;
        }

        Animate();
        OnRun();
    }

    private bool MeetsRequirements(BodyStatus status)
    {
        if (requiredArms > 0 && status.armCount < requiredArms)
            return false;

        /*if (requiredLegs > 0 && status.legCount < requiredLegs)
            return false;*/

        if (requiresHead && !status.hasHead)
            return false;

        return true;
    }

    private void Animate()
    {
        if (effectPrfb != null && receiver != null)
        {
            var go = Instantiate(effectPrfb, receiver.transform.position, Quaternion.identity);
            Destroy(go, animationDuration);
        }
    }

    protected abstract void OnRun();
}