using Godot;
using System;
using System.Collections.Generic;

/*
    Represents an operation occurring during Combat (like dealing
    damage or gaining energy), like the Command Pattern.
    
    The tag represents the string that was entered for this operation
    all the way up from the spreadsheet. This is to keep a consistent identifier
    on the spreadsheet, so the resulting values from the operation can be sent back
    to the parent Action to be put into the TextLog. It essentially acts as a variable
    name for a CombatOp instance, except defined at the spreadsheet level. It is up
    to the spreadsheet readers to interpret/validate that variable name and turn
    it into an actual CombatOp.

    You may also add in a Value that indicates how many times to perform the operation.

    Subclasses typically take in one of its own enums upon initialization,
    based on the kind of combat operation desired. The reason for separation
    into multiple subclasses is because of differing parameters and number of
    values (BasicOps only take in the enum and a Value, but EffectOps take in
    the enum, a string for the effect name, an amount Value, and a turns Value,
    and has to modify these values accordingly).

    The reason this whole thing exists is to be preloaded with a Value and put into an Action
    at the start of the program. Then, when this is executed, it will use the
    source and target to Modify its operation and call any Auxiliaries added by
    them.
    
    The intention during execution is to:
        1. calculate the Value, adding Modifiers from the source and target
        4. execute the operation
        5. update the parent Action's results with the final amount from the operation
        6. execute the Auxiliaries from the source and target
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
        if(selfTarget){
            checkAndOperate(source, source, parent);
        }
        else{
            checkAndOperate(source, target, parent);
        }
    }

    private void checkAndOperate(Combatant source, Combatant target, Action parent){
        if(!hasLoop){
            operate(source, target, parent);
        }
        else{
            for(int i = 0; i < loop.calculate(source); i++){
                operate(source, target, parent);
            }
        }
    }
    
    public bool isHostile => !selfTarget;

    public void setLoop(Value loop){
        this.loop = loop;
        hasLoop = true;
    }

    protected abstract void operate(Combatant source, Combatant target, Action parent);
}