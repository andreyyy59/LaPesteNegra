using UnityEngine;
using System.Collections;

public enum HealthModType
{
    STAT_BASED,
    FIXED,
    PERCENTAGE
}

public enum BodyTargetZone
{
    NONE,
    HEAD,
    ARMS,
    TORSO
}

public enum AttackType
{
    NONE,
    PHYSICAL,
    MAGICAL
}

public class HealthModSkill : SkillInteraction
{
    [Header("Health Mod")]
    public float amount;
    public HealthModType modType;
    public BodyTargetZone bodyTarget = BodyTargetZone.NONE;
    public bool isOffensive = false;
    public AttackType attackType = AttackType.PHYSICAL;

    [Header("Probabilidades especiales")]
    [Range(0f, 1f)] public float specialHitChance = 0.6f;

    private bool wasCrit = false;

    protected override void OnRun()
    {
        StartCoroutine(RunEffect());
    }

    private IEnumerator RunEffect()
    {
        wasCrit = false;
        float intended = GetModification();

        if (attackType == AttackType.PHYSICAL && isOffensive)
        {
            intended *= emitter.bodyStatus.GetDamageMultiplierForArms();
        }

        var result = receiver.ModifyHealth(intended);
        int shown = Mathf.RoundToInt(Mathf.Abs(result.finalValue));

        if (!isSpecial)
        {
            if (result.finalValue < 0)
            {
                if (wasCrit)
                    LogPanel.Write($"¡Golpe crítico! {shown} de daño a {receiver.idName}.");
                else
                    LogPanel.Write($"{emitter.idName} infligió {shown} de daño a {receiver.idName}.");

                yield return LogPanel.WaitForMessage();
                yield return new WaitForSeconds(1.2f);

                if (result.blocked)
                {
                    LogPanel.Write($"{receiver.idName} bloqueó parcialmente el ataque.");
                    yield return LogPanel.WaitForMessage();
                    yield return new WaitForSeconds(1.2f);
                }
            }
            else if (Mathf.Approximately(result.finalValue, 0f))
            {
                LogPanel.Write($"{emitter.idName} intentó atacar, pero no tiene fuerza suficiente para causar daño.");
                yield return LogPanel.WaitForMessage();
                yield return new WaitForSeconds(1.2f);
            }
            else
            {
                LogPanel.Write($"{emitter.idName} se curó {shown} pts. de salud.");
                yield return LogPanel.WaitForMessage();
                yield return new WaitForSeconds(1.2f);
            }
        }

        if (isSpecial && Random.value < specialHitChance)
        {
            yield return ApplyBodyEffect();
        }
        else if (isSpecial)
        {
            Debug.Log("Falló el efecto especial, entrando en mensajes personalizados.");
            switch (bodyTarget)
            {
                case BodyTargetZone.HEAD:
                    LogPanel.Write($"{emitter.idName} intentó un golpe letal a la cabeza, pero solo provocó un rasguño.");
                    break;
                case BodyTargetZone.ARMS:
                    LogPanel.Write($"{emitter.idName} intentó cercenar un brazo, pero solo provocó un rasguño.");
                    break;
                case BodyTargetZone.TORSO:
                    LogPanel.Write($"{emitter.idName} intentó provocar una hemorragia, pero solo provocó un rasguño.");
                    break;
                default:
                    LogPanel.Write($"{emitter.idName} intentó aplicar un efecto especial pero falló.");
                    break;
            }
            yield return LogPanel.WaitForMessage();
            yield return new WaitForSeconds(1.2f);
        }
    }

    private float GetModification()
    {
        float rawValue;

        switch (modType)
        {
            case HealthModType.FIXED:
                rawValue = amount;
                break;
            case HealthModType.PERCENTAGE:
                Stats rStats = receiver.GetCurrentStats();
                rawValue = rStats.maxHealth * amount;
                break;
            case HealthModType.STAT_BASED:
                Stats eStats = emitter.GetCurrentStats();
                rawValue = eStats.attack * amount;
                break;
            default:
                throw new System.InvalidOperationException("Tipo de modificación no válido.");
        }

        return ApplyCritConditionally(rawValue);
    }

    private float ApplyCritConditionally(float value)
    {
        if (value < 0 && emitter.hasGuaranteedCrit)
        {
            emitter.hasGuaranteedCrit = false;
            wasCrit = true;
            return value * 2f;
        }

        return value;
    }

    private IEnumerator ApplyBodyEffect()
    {
        switch (bodyTarget)
        {
            case BodyTargetZone.HEAD:
                receiver.bodyStatus.hasHead = false;
                receiver.Stats.health = 0;
                receiver.statusPanel.SetHealth(0, receiver.Stats.maxHealth);
                LogPanel.Write($"{receiver.idName} fue decapitado.");
                break;
            case BodyTargetZone.ARMS:
                receiver.bodyStatus.LoseArm();
                LogPanel.Write($"{receiver.idName} perdió un brazo.");
                break;
            case BodyTargetZone.TORSO:
                receiver.bodyStatus.ApplyBleeding(3);
                LogPanel.Write($"{receiver.idName} comenzó a sangrar durante 3 turnos.");
                break;
            default:
                Debug.LogWarning("Zona del cuerpo no especificada para efecto especial.");
                break;
        }
        yield return LogPanel.WaitForMessage();
        yield return new WaitForSeconds(1.2f);
    }
}