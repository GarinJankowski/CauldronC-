using Godot;
using System;
using System.Collections.Generic;

/*

*/
public class EffectOp : CombatOp
{
    private Tag turnTag;

    private CombatOpManager.Effect operation;
    private string effectName;

    private Value amount;
    private Value turns;

    public EffectOp(Tag tag, Tag turnTag, CombatOpManager.Effect operation, bool selfTarget, string effectName, Value amount, Value turns): base(tag, selfTarget){
        this.turnTag = turnTag;
        this.operation = operation;
        this.effectName = effectName;
        this.amount = amount;
        this.turns = turns;
    }

    protected override void operate(Combatant source, Combatant target, Action parent){
        (int, int) result = source.CombatOps.effectOp(tag, operation, target, effectName, amount, turns);
        parent.updateResult(tag, result.Item1);
        parent.updateResult(turnTag, result.Item2);
    }
}