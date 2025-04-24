using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BodyStatus
{
    [Header("Estructura física")]
    [Min(0)] public int armCount = 2;
    public bool hasHead = true;

    [Header("Estados alterados")]
    public int bleedingTurns = 0;

    public bool HasAnyArm => armCount > 0;
    public bool HasBothArms => armCount >= 2;

    public bool CanUseWeapons => HasAnyArm;
    public bool IsBleeding => bleedingTurns > 0;
    public bool IsDead => !hasHead;

    public float GetDamageMultiplierForArms()
    {
        if (armCount >= 2) return 1f;
        if (armCount == 1) return 0.5f;
        return 0f;
    }

    public void ApplyBleeding(int turns)
    {
        bleedingTurns = Mathf.Max(bleedingTurns, turns);
    }

    public void TickTurn()
    {
        if (bleedingTurns > 0) bleedingTurns--;
    }

    public void LoseArm()
    {
        if (armCount > 0) armCount--;
    }

    public void RestoreArm()
    {
        armCount++;
    }

    public void RestoreHead()
    {
        hasHead = true;
    }

    public bool RecoverRandomPart()
    {
        List<string> damaged = GetDamagedParts();
        if (damaged.Count == 0) return false;

        string selected = damaged[Random.Range(0, damaged.Count)];
        RestorePart(selected);
        return true;
    }

    public string RecoverMostUsefulPart(List<string> requiredPartNames)
    {
        Dictionary<string, int> priority = new();

        foreach (string part in requiredPartNames)
        {
            if (!IsPartFunctional(part))
            {
                if (!priority.ContainsKey(part))
                    priority[part] = 0;
                priority[part]++;
            }
        }

        if (priority.Count == 0) return null;

        string mostAffected = null;
        int maxCount = -1;

        foreach (var kvp in priority)
        {
            if (kvp.Value > maxCount)
            {
                mostAffected = kvp.Key;
                maxCount = kvp.Value;
            }
        }

        if (mostAffected != null)
        {
            RestorePart(mostAffected);
        }

        return mostAffected;
    }

    public List<string> GetDamagedParts()
    {
        List<string> parts = new();

        if (!hasHead) parts.Add("Head");
        if (armCount < 2) parts.Add("Arm");

        Debug.Log($"Partes dañadas detectadas: {string.Join(", ", parts)}");

        return parts;
    }

    public bool IsPartFunctional(string part)
    {
        return part switch
        {
            "Head" => hasHead,
            "Arm" => armCount > 0,
            _ => true,
        };
    }

    public void RestorePart(string part)
    {
        switch (part)
        {
            case "Head": RestoreHead(); break;
            case "Arm": RestoreArm(); break;
        }
    }
}