using Godot;
using System;
using System.Collections.Generic;

/*

*/
public class CombatOpManager
{
    // basic operations
    public enum Basic {
        dealDamage,
        gainHealth, loseHealth,
        gainEnergy, loseEnergy,
        gainMana, loseMana,
    }
    private static Dictionary<Basic, Action<Combatant, int>> basicOperations = new Dictionary<Basic, Action<Combatant, int>>(){
        {Basic.dealDamage, (tg, am) => tg.Stats.addHealth(-am)},
        
        {Basic.gainHealth, (tg, am) => tg.Stats.addHealth(am)},
        {Basic.loseHealth, (tg, am) => tg.Stats.addHealth(-am)},
        
        {Basic.gainEnergy, (tg, am) => tg.Stats.addEnergy(am)},
        {Basic.loseEnergy, (tg, am) => tg.Stats.addEnergy(-am)},
        
        {Basic.gainMana, (tg, am) => tg.Stats.addMana(am)},
        {Basic.loseMana, (tg, am) => tg.Stats.addMana(-am)},
    };

    // effect operations
    public enum Effect {
        gainEffect, loseEffect
    }
    private static Dictionary<Effect, Action<Combatant, string, int, int>> effectOperations = new Dictionary<Effect, Action<Combatant, string, int, int>>(){
        // todo
        {Effect.gainEffect, (target, name, amount, turns) => target.Stats.addHealth(0)},
        {Effect.loseEffect, (target, name, amount, turns) => target.Stats.addHealth(0)}
    };


    private ModifierManager modManager;

    public CombatOpManager(){}

    public int basicOp(Tag tag, Basic operation, Combatant target, Value amount){
        // execute source Mods
        // execute source Reactives
        
        // set action results

        // execute target Mods
        // execute target Reactives
        
        // execute operation
        
        // execute source Auxes
        // execute target Auxes
        return 0;
    }

    public (int, int) effectOp(Tag tag, Effect operation, Combatant target, string effectName, Value amount, Value turns){
        return (0, 0);
    }
}