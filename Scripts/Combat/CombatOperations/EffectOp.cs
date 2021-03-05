using Godot;
using System;
using System.Collections.Generic;

/*

*/
public abstract class EffectOp : CombatOp
{
    public enum Ops {
        gainEffect, loseEffect
    }

    private static Dictionary<Ops, Action<Combatant, string, int, int>> effectOperations = new Dictionary<Ops, Action<Combatant, string, int, int>>(){
        // todo
        {Ops.gainEffect, (target, name, amount, turns) => target.Stats.addHealth(0)},
        {Ops.loseEffect, (target, name, amount, turns) => target.Stats.addHealth(0)}
    };

    private Tag turnTag;

    private Ops operation;
    private string effectName;

    private Value value;
    private Value turnsValue;

    public EffectOp(Tag tag, Tag turnTag, Ops operation, bool selfTarget, string effectName, Value value, Value turnsValue): base(tag, selfTarget){
        this.turnTag = turnTag;
        this.operation = operation;
        this.effectName = effectName;
        this.value = value;
        this.turnsValue = turnsValue;
    }

    protected override void operate(Combatant source, Combatant target, Action parent){
        // gather Mods

        // gather Auxes

        // execute operation
        
        // set action results

        // execute Auxes
    }
}