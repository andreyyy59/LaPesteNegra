using UnityEngine;

public class StrongSkill : SkillInteraction
{
    protected override void OnRun()
    {
        receiver.hasGuaranteedCrit = true;

        if (receiver is PlayerFighter)
        {
            LogPanel.Write($"{receiver.idName} ha bebido una poci�n de fuerza. El siguiente golpe ser� cr�tico.");
        }
        else if (receiver is EnemyFighter)
        {
            LogPanel.Write($"{receiver.idName} entra en estado de furia.");
        }
    }
}