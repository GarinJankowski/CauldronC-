using System;

/*
Represents anything that can be fought in combat.
Extenders of this class have:
    -stats like Health and Strength
    -a Deck of Cards
    -the ability to have Effects applied to them
    -the ability to have Mutations
    -the ability for these Effects/Mutations to Modify
     Cards and CombatOperations
*/
public abstract class Combatant
{
    private StatManager statManager;

    public StatManager stats() { return statManager; }
}