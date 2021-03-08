using Godot;
using System;
using System.Collections.Generic;

/*

*/
public class BasicOp : CombatOp
{
    private CombatOpManager.Basic operation;
    private Value amount;

    public BasicOp(Tag tag, CombatOpManager.Basic operation, bool selfTarget, Value amount): base(tag, selfTarget){
        this.operation = operation;
        this.amount = amount;
    }

    protected override void operate(Combatant source, Combatant target, Action parent){
        int result = source.CombatOps.basicOp(tag, operation, target, amount);
        parent.updateResult(tag, result);
    }
}