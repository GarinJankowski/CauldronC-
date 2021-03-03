using Godot;
using System;
using System.Collections.Generic;

/*

*/
public abstract class BasicOp : CombatOp
{
    public enum Ops {
        dealDamage,
        gainHealth, loseHealth,
        gainEnergy, loseEnergy,
        gainMana, loseMana,
    }

    private static Dictionary<Ops, Action<Combatant, int>> basicOperations = new Dictionary<Ops, Action<Combatant, int>>(){
        {Ops.dealDamage, (tg, am) => tg.stats().addHealth(-am)},
        
        {Ops.gainHealth, (tg, am) => tg.stats().addHealth(am)},
        {Ops.loseHealth, (tg, am) => tg.stats().addHealth(-am)},
        
        {Ops.gainEnergy, (tg, am) => tg.stats().addEnergy(am)},
        {Ops.loseEnergy, (tg, am) => tg.stats().addEnergy(-am)},
        
        {Ops.gainMana, (tg, am) => tg.stats().addMana(am)},
        {Ops.loseMana, (tg, am) => tg.stats().addMana(-am)},
    };

    private Ops operation;
    private Value value;

    public BasicOp(Ops operation, Value value){
        this.operation = operation;
        this.value = value;
    }

    public void execute(Combatant source, Combatant target, Action parent){
        // gather Mods
        int adds = 0;
        int multiplier = 1;
        // gather Auxes
        // execute operation
        int amount = Math.Max(0, (int)((value.calculate()+adds)*multiplier));
        basicOperations[operation](target, amount);
        // set action results

        // execute Auxes
    }
}