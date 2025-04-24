using UnityEngine;

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

    private bool wasCrit = false;

    protected override void OnRun()
    {
        wasCrit = false;

        float intended = GetModification();

        // Penalización por brazos faltantes si es ataque físico ofensivo
        if (attackType == AttackType.PHYSICAL && isOffensive)
        {
            intended *= emitter.bodyStatus.GetDamageMultiplierForArms();
        }

        var result = receiver.ModifyHealth(intended);
        int shown = Mathf.RoundToInt(Mathf.Abs(result.finalValue));

        if (result.finalValue < 0)
        {
            if (wasCrit)
            {
                LogPanel.Write($"¡Golpe crítico! {shown} de daño a {receiver.idName}.");
            }
            else
            {
                LogPanel.Write($"{emitter.idName} infligió {shown} de daño a {receiver.idName}.");
            }

            if (result.blocked)
            {
                LogPanel.Write($"{receiver.idName} bloqueó parcialmente el ataque.");
            }

            if (isSpecial)
            {
                ApplyBodyEffect();
            }
        }
    else if (Mathf.Approximately(result.finalValue, 0f))
    {
        LogPanel.Write($"{emitter.idName} intentó atacar, pero no tiene fuerza suficiente para causar daño.");
    }
    else
    {
        LogPanel.Write($"{emitter.idName} se curó {shown} pts. de salud.");
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

    private void ApplyBodyEffect()
    {
        switch (bodyTarget)
        {
            case BodyTargetZone.HEAD:
                if (Random.value < 0.6f)
                {
                    receiver.bodyStatus.hasHead = false;
                    receiver.Stats.health = 0;
                    receiver.statusPanel.SetHealth(0, receiver.Stats.maxHealth);
                    LogPanel.Write($"{receiver.idName} fue decapitado.");
                }
                else
                {
                    LogPanel.Write($"{emitter.idName} falló el intento de golpe letal a la cabeza.");
                }
                break;

            case BodyTargetZone.ARMS:
                receiver.bodyStatus.LoseArm();
                LogPanel.Write($"{receiver.idName} perdió un brazo.");
                break;

            case BodyTargetZone.TORSO:
                receiver.bodyStatus.ApplyBleeding(3);
                LogPanel.Write($"{receiver.idName} comenzó a sangrar durante 3 turnos.");
                break;

            case BodyTargetZone.NONE:
            default:
                Debug.LogWarning("Zona del cuerpo no especificada para efecto especial.");
                break;
        }
    }
}