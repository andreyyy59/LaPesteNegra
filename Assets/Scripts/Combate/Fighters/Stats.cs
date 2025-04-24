public class Stats
{
    public float health;
    public float maxHealth;

    public float attack;

    public Stats(float maxHealth, float attack)
    { 
        this.maxHealth = maxHealth;
        this.health = maxHealth;

        this.attack = attack;
    }

    public Stats Clone()
    {
        return new Stats(this.maxHealth, this.attack);
    }
}
