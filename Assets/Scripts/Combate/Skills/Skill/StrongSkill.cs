using UnityEngine;

public class StrongSkill : SkillInteraction
{
    protected override void OnRun()
    {
        receiver.hasGuaranteedCrit = true;

        if (receiver is PlayerFighter)
        {
            LogPanel.Write($"{receiver.idName} ha bebido una poción de fuerza. El siguiente golpe será crítico.");
        }
        else if (receiver is EnemyFighter)
        {
            LogPanel.Write($"{receiver.idName} entra en estado de furia.");
        }
    }
}