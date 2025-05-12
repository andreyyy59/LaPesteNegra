using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Fighter[] fighters;
    private Queue<Fighter> turnQueue;
    private bool isCombatActive;

    private PlayerActionMenu playerActionMenu;

    void Start()
    {
        StartCoroutine(StartupSequence());
    }

    private IEnumerator StartupSequence()
    {
        LogPanel.Write("Iniciando Batalla.");
        yield return LogPanel.WaitForMessage();
        yield return new WaitForSeconds(1.2f);

        isCombatActive = true;
        playerActionMenu = Object.FindAnyObjectByType<PlayerActionMenu>();
        InitializeTurnQueue();

        StartCoroutine(CombatLoop());
        Debug.Log("TurnQueue inicial (Start): [ " + string.Join(" ", turnQueue.Select(x => x.idName)) + " ]");
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
    }

    IEnumerator CombatLoop()
    {
        while (isCombatActive && turnQueue.Count > 0)
        {
            Fighter current = turnQueue.Dequeue();
            Fighter opponent = GetOpponent(current);
            LogPanel.Write($"{current.idName} tiene el turno.");
            yield return LogPanel.WaitForMessage();
            yield return new WaitForSeconds(1.2f);
            

            Debug.Log($"[Turno] {current.idName} (vida: {current.Stats.health})");
            current.isTurnFinished = false;
            current.InitTurn();

            if (current is PlayerFighter)
            {
                playerActionMenu.StartPlayerTurn();
            }
            else
            {
                playerActionMenu.Hide();
            }

            yield return new WaitUntil(() => current.isTurnFinished);

            if (!current.isAlive || !opponent.isAlive)
            {
                yield return EndCombat();
                yield break;
            }

            // Cola básica sin turnos extra
            if (!turnQueue.Contains(opponent))
            {
                turnQueue.Enqueue(opponent);
            }

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

    private IEnumerator EndCombat()
    {
        isCombatActive = false;
        LogPanel.Write("¡Combate finalizado!");
        yield return LogPanel.WaitForMessage();
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("Celdas 1"); 
    }

    private void PrintTurnQueue()
    {
        string contenido = "Cola actual de turnos: [ ";

        foreach (var fighter in turnQueue)
        {
            contenido += fighter.idName + " ";
        }

        contenido += "]";
        Debug.Log("Cola actual de turnos: [ " + string.Join(" ", turnQueue.Select(f => f.idName)) + " ]");
    }
}