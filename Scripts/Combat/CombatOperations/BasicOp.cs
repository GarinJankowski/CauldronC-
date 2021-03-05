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
        {Ops.dealDamage, (tg, am) => tg.Stats.addHealth(-am)},
        
        {Ops.gainHealth, (tg, am) => tg.Stats.addHealth(am)},
        {Ops.loseHealth, (tg, am) => tg.Stats.addHealth(-am)},
        
        {Ops.gainEnergy, (tg, am) => tg.Stats.addEnergy(am)},
        {Ops.loseEnergy, (tg, am) => tg.Stats.addEnergy(-am)},
        
        {Ops.gainMana, (tg, am) => tg.Stats.addMana(am)},
        {Ops.loseMana, (tg, am) => tg.Stats.addMana(-am)},
    };

    private Ops operation;
    private Value value;

    public BasicOp(Tag tag, Ops operation, bool selfTarget, Value value): base(tag, selfTarget){
        this.operation = operation;
        this.value = value;
    }

    protected override void operate(Combatant source, Combatant target, Action parent){
        // gather Mods (should be ONE CALL for each Combatant)
        
        // execute operation
        
        // set action results

        // execute Auxes (should be ONE CALL for each Combatant)
    }
}