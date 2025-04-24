using UnityEngine;
using System.Collections.Generic;

public class RegenerationSkill : SkillInteraction
{
    protected override void OnRun()
    {
        // Obtener todas las habilidades del personaje
        var skills = emitter.GetComponentsInChildren<SkillInteraction>();
        var requiredParts = new List<string>();

        foreach (var skill in skills)
        {
            // Requisitos de brazos
            for (int i = 0; i < skill.requiredArms; i++)
            {
                requiredParts.Add(i % 2 == 0 ? "LeftArm" : "RightArm");
            }

            // Requisito de cabeza
            if (skill.requiresHead)
            {
                requiredParts.Add("Head");
            }
        }

        /* Restaurar la parte m�s �til seg�n las habilidades bloqueadas
        string restored = emitter.bodyStatus.RecoverMostUsefulPart(requiredParts);

        if (string.IsNullOrEmpty(restored))
        {
            LogPanel.Write($"{emitter.idName} rez�, pero no ten�a partes que recuperar.");
        }
        else
        {
            LogPanel.Write($"{emitter.idName} rez� y recuper� su {restored}.");
        }*/
        bool recovered = emitter.bodyStatus.RecoverRandomPart();

        if (recovered)
            LogPanel.Write($"{emitter.idName} rez� y recuper� una parte.");
        else
            LogPanel.Write($"{emitter.idName} rez�, pero no ten�a partes que recuperar.");

    }
}