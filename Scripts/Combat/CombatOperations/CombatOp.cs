using Godot;
using System;
using System.Collections.Generic;

/*
    Represents an operation occurring during Combat (like dealing
    damage or gaining energy), like the Command Pattern.

    Realizers typically take in one of its own enums upon initialization,
    based on the kind of combat operation desired. The reason for separation
    into multiple realizers is beceause of differing parameters and number of
    values (BasicOps only take in the enum and a Value, but EffectOps take in
    the enum, a string for the effect name, an amount Value, and a turns Value,
    and has to modify these values accordingly).

    The reason this whole thing exists is to be preloaded with a Value and put into an Action
    at the start of the program. Then, when this is executed, it will use the
    source and target to Modify its operation and call any Auxiliaries added by
    them.
    
    The intention during execution is to:
        1. check the source for Modifiers and Auxiliaries and gather them
        2. check the target for Modifiers and Auxiliaries and gather them
        3. calculate the Value with Modifiers
        4. execute the operation
        5. update the parent Action's results with the final amount from the operation
        6. execute the Auxiliaries
*/
public interface CombatOp
{
    void execute(Combatant source, Combatant target, Action parent);
}