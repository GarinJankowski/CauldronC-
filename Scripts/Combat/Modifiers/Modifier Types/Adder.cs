using Godot;
using System;

/*

*/
public class Adder : Modifier
{
    private Value amount;

    public Adder(Value amount){
        this.amount = amount;
    }

    public float modify(float number, Combatant source){
        return number+amount.calculate(source);
    }
}