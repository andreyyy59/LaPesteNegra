using UnityEngine;

public class DefenseBuffSkill : SkillInteraction
{
    protected override void OnRun()
    {
        // Verificación de brazos antes de aplicar defensa
        if (emitter.bodyStatus.armCount == 0)
        {
            LogPanel.Write($"{emitter.idName} no puede defenderse: no tiene brazos.");
            return;
        }

        receiver.hasDefenseBuff = true;

        if (receiver is PlayerFighter)
        {
            LogPanel.Write($"{receiver.idName} se prepara para bloquear un ataque.");
        }
        else if (receiver is EnemyFighter)
        {
            LogPanel.Write($"{receiver.idName} ha endurecido su piel.");
        }
    }
}