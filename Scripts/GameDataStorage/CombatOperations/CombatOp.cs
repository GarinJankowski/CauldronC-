using Godot;
using System;
using System.Collections.Generic;

/*

*/
public abstract class CombatOp
{
    protected bool selfTarget;
    protected Tag tag;

    private Value loop;
    private bool hasLoop = false;

    public CombatOp(Tag tag, bool selfTarget){
        this.tag = tag;
        this.selfTarget = selfTarget;
    }

    public void execute(Combatant source, Combatant target, Action parent){
        Combatant actualTarget = chooseTarget(source, target);

        if(!hasLoop){
            operate(source, actualTarget, parent);
        }
        else{
            for(int i = 0; i < loop.calculate(source); i++){
                operate(source, actualTarget, parent);
            }
        }
    }

    private Combatant chooseTarget(Combatant source, Combatant target){
        if(selfTarget)
            return source;
        return target;
    }
    
    public bool isHostile => !selfTarget;

    public void setLoop(Value loop){
        this.loop = loop;
        hasLoop = true;
    }

    protected abstract void operate(Combatant source, Combatant target, Action parent);
}