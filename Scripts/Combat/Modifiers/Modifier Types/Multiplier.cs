using Godot;
using System;

/*

*/
public class Multiplier : Modifier
{
    private Value amount;

    public Multiplier(Value amount){
        this.amount = amount;
    }

    public float modify(float number, Combatant source){
        return number*amount.calculate(source);
    }
}