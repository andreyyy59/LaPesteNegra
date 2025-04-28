using UnityEngine;

public class PlayerFighter : Fighter
{
    [SerializeField] private int Damage;
    [SerializeField] private int Health;
    void Awake()
    {
        // Establece los stats base del jugador
        this.stats = new Stats(Health, Damage);
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