using UnityEngine;

public class PlayerFighter : Fighter
{
    void Awake()
    {
        // Establece los stats base del jugador
        this.stats = new Stats(1000, 10);
    }

    public override void InitTurn()
    {
        StartCoroutine(HandleBodyStatusCoroutine());


        // Aquí no se inicia ninguna acción directamente; se activa el menú desde CombatManager
    }

    protected override void Start()
    {
        base.Start();
        ListSkills(); // Muestra las habilidades en la consola al iniciar (opcional, útil para debug)
    }
}