using System;
using UnityEngine;

public class CharacterStats
{
    public int Strength { get; private set; }
    public int Defense { get; private set; }
    public float Speed { get; private set; }
    public int MaxHealth { get; private set; }

    public CharacterStats(int strength, int defense, float speed, int maxHealth)
    {
        if (maxHealth <= 0) throw new ArgumentException("MaxHealth must be greater than zero");

        Strength = strength;
        Defense = defense;
        Speed = speed;
        MaxHealth = maxHealth;
    }

    public void SetStrength(int newStrength)
    {
        if (newStrength < 0) throw new ArgumentException("Strength cannot be negative");
        Strength = newStrength;
    }

    public void SetDefense(int newDefense)
    {
        if (newDefense < 0) throw new ArgumentException("Defense cannot be negative");
        Defense = newDefense;
    }

    public void SetSpeed(float newSpeed)
    {
        if (newSpeed < 0) throw new ArgumentException("Speed cannot be negative");
        Speed = newSpeed;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        if (newMaxHealth <= 0) throw new ArgumentException("MaxHealth must be greater than zero");
        MaxHealth = newMaxHealth;
    }
}
