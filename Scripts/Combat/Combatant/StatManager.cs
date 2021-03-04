using System;
using System.Collections.Generic;

/*
    Keeps track of statistics like Health, Energy, and Strength. This class is assigned
    to a Combtant as a way to view and change their stats.

    Also keeps track of death and extra turns. It doesn't do anything with those, just
    keeps track.
*/
public class StatManager
{
    private bool alive = true;
    private Dictionary<Stat, Statistic> baseStats;

    MaxEnergy maxEnergy;

    private int currentHealth;
    private int currentEnergy;
    private int extraTurns;
    private int currentMana;

    public StatManager(int Str, int Dex, int Int, int Mut, int MHP, int MEP, int MMP){
            Statistic mutation = null;
            mutation = new Statistic(Mut, mutation);
    
            Statistic strength = new Statistic(Str, mutation);
            Statistic dexterity = new Statistic(Dex, mutation);
            Statistic intelligence = new Statistic(Int, mutation);
            Statistic maxHealth = new Statistic(MHP, mutation);
            maxEnergy = new MaxEnergy(MEP, mutation);
            Statistic maxMana = new Statistic(MMP, mutation);

            //for each stat, the full word and shorthand keys refer to the same value
            baseStats = new Dictionary<Stat, Statistic>{
            {Stat.Strength, strength},         {Stat.Str, strength},
            {Stat.Dexterity, dexterity},       {Stat.Dex, dexterity},
            {Stat.Intelligence, intelligence}, {Stat.Int, intelligence},
            {Stat.Mutation, mutation},         {Stat.Mut, mutation},
            {Stat.MaxHealth, maxHealth},       {Stat.MHP, maxHealth},
            {Stat.MaxEnergy, maxEnergy},       {Stat.MEP, maxEnergy},
            {Stat.MaxMana, maxMana},           {Stat.MMP, maxMana},
        };

        currentHealth = getStat(Stat.MaxHealth);
        currentEnergy = 0;
        extraTurns = 0;
        currentMana = getStat(Stat.MaxMana);
        
    }

    public void refresh(){
        foreach(KeyValuePair<Stat, Statistic> entry in baseStats){
            entry.Value.zeroTemp();
        }
        currentHealth = getStat(Stat.MaxHealth);
        currentEnergy = 0;
        currentMana = getStat(Stat.MaxMana);
    }

    public int getHealth() { return currentHealth; }
    public void addHealth(int amount) {
        currentHealth += amount;
        if(currentHealth <= 0){
            alive = false;
        }
    }

    public int getEnergy() { return currentEnergy; }
    public void addEnergy(int amount){
        currentEnergy += amount;
        if(currentEnergy >= getMaxEnergy()){
            extraTurns += 1;
            currentEnergy = 0;
        }
        else if(currentEnergy <= getMinEnergy()){
            extraTurns -= 1;
            currentEnergy = 0;
        }
    }

    public int getExtraTurns() { return extraTurns; }
    public void addExtraTurns(int amount) {
        extraTurns += amount;
    }

    public int getMana() { return currentMana; }
    public void addMana(int amount){
        currentMana += amount;
        currentMana = Math.Max(0, currentMana);
    }

    public int getStat(Stat stat){
        return baseStats[stat].Value;
    }
    
    public int getMaxEnergy(){
        return maxEnergy.Max;
    }

    public int getMinEnergy(){
        return maxEnergy.Min;
    }

    public void addStat(Stat stat, int amount){
        baseStats[stat].add(amount);
    }

    public void addTemp(Stat stat, int amount){
        baseStats[stat].addTemp(amount);
    }

    public void addScaling(Stat stat, float scaling){
        baseStats[stat].addScaling(scaling);
    }

    /*
    Each stat keeps track of their own temporary boosts and
    scaling off of their given mutation stat.

    Mutation scaling is a floating point value which indicates
    how much of the Mutation value is added to the value of this
    stat. Having 5 Strength, 4 Mutation, and 0.5 scaling on Strength
    means you get +(4*0.5) Strength, so your effective Strength is 7. Any
    uneven divisions are rounded down, as per usual.
    */
    private class Statistic
    {
        protected int currentAmount;

        private int amount;
        private int temporary;
        private float mutationScaling;
        private Statistic mut;

        public Statistic(int amount, Statistic mut){
            this.amount = amount;
            this.mut = mut;

            temporary = 0;
            mutationScaling = 0;
            updateCurrentAmount();
        }

        public void add(int amount){
            this.amount += amount;
            updateCurrentAmount();
        }

        public void addTemp(int amount){
            temporary += amount;
            updateCurrentAmount();
        }

        public void zeroTemp(){
            temporary = 0;
            updateCurrentAmount();
        }

        public void addScaling(float scaling){
            mutationScaling += scaling;
            updateCurrentAmount();
        }

        public int Value => currentAmount;

        protected int calculateAmount(){
            return (amount + temporary) + (int)(mut.Value*mutationScaling);
        }

        protected virtual void updateCurrentAmount(){
            currentAmount = Math.Max(0, calculateAmount());
        }
    }

    /*
    Max Energy is weird so it gets its own class.
    The standard starting value for MEP is 10, because the ENERGY_LIMIT is 10,
    I have decided. This means that at 10 or more current energy, an extra turn is gained.
    At -10 or less current energy, an extra turn is lost.

    Subtracting 1 MEP changes this to getting an extra turn at 9 or more
    current energy and losing a turn at -11 or less current energy. This means
    losing MEP is typically beneficial.

    Going lower than 1 MEP has no effect on how much current energy you require
    to get an extra turn (because it caps out at 1 energy). However, this still
    increases the amount of negative energy required to lose a turn. The reverse
    of this (going higher than 19 MEP) works the opposite. This makes a lot more
    sense visually.
    */
    private class MaxEnergy : Statistic
    {
        private const int ENERGY_LIMIT = 10;
        private int currentMin;

        public MaxEnergy(int value, Statistic mut) : base(value, mut){}

        protected override void updateCurrentAmount(){
            int val = calculateAmount();
            currentAmount = Math.Max(1,val);
            currentMin = Math.Min(-1, val-(ENERGY_LIMIT*2));
        }

        public int Max => currentAmount;
        public int Min => currentMin;
    }
}