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
        {Ops.gainEffect, (target, name, amount, turns) => target.stats().addHealth(0)},
        {Ops.loseEffect, (target, name, amount, turns) => target.stats().addHealth(0)}
    };

    private Ops operation;
    private string effectName;
    private Value value;
    private Value turnsValue;

    public EffectOp(Ops operation, string effectName, Value value, Value turnsValue){
        this.operation = operation;
        this.effectName = effectName;
        this.value = value;
        this.turnsValue = turnsValue;
    }

    public void execute(Combatant source, Combatant target, Action parent){
        // gather Mods
        int adds = 0;
        int multiplier = 1;
        // gather Auxes
        // execute operation
        int amount = Math.Max(0, (int)((value.calculate()+adds)*multiplier));
        int turns = (int)turnsValue.calculate();
        effectOperations[operation](target, effectName, amount, turns);
        // set action results

        // execute Auxes
    }
}