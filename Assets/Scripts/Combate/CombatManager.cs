using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Fighter[] fighters;
    private Queue<Fighter> turnQueue;
    private bool isCombatActive;

    private PlayerActionMenu playerActionMenu;

    void Start()
    {
        LogPanel.Write("Battle Initiated.");
        isCombatActive = true;

        playerActionMenu = Object.FindAnyObjectByType<PlayerActionMenu>();
        InitializeTurnQueue();

        StartCoroutine(CombatLoop());
    }

    void Awake()
    {
        Debug.LogWarning($"🟡 CombatManager activo en: {gameObject.name}", this);
    }

    void InitializeTurnQueue()
    {
        turnQueue = new Queue<Fighter>();
        foreach (var f in fighters)
        {
            if (!turnQueue.Contains(f))
                turnQueue.Enqueue(f);
        }

        PrintTurnQueue();
    }

    IEnumerator CombatLoop()
    {
        while (isCombatActive && turnQueue.Count > 0)
        {
            Fighter current = turnQueue.Dequeue();
            Fighter opponent = GetOpponent(current);

            LogPanel.Write($"{current.idName} tiene el turno.");
            current.isTurnFinished = false;
            current.InitTurn();

            if (current is PlayerFighter)
                playerActionMenu.StartPlayerTurn();
            else
                playerActionMenu.Hide();

            yield return new WaitUntil(() => current.isTurnFinished);

            if (!current.isAlive || !opponent.isAlive)
            {
                EndCombat();
                yield break;
            }

            // 🔒 Evitar duplicados en la cola
            if (!turnQueue.Contains(opponent))
                turnQueue.Enqueue(opponent);

            PrintTurnQueue();
        }
    }

    Fighter GetOpponent(Fighter current)
    {
        foreach (var f in fighters)
        {
            if (f != current)
                return f;
        }
        return null;
    }

    void EndCombat()
    {
        isCombatActive = false;
        LogPanel.Write("¡Combate finalizado!");
    }

    private void PrintTurnQueue()
    {
        Debug.Log("Cola actual de turnos: [ " + string.Join(" ", turnQueue.Select(f => f.idName)) + " ]");
    }
}