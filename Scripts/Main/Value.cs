using System;
using System.Collections.Generic;

/*
    A Value holds a string expression that is to be
    evaluated when needed. The expression format is
    described in the Calculator class file.
*/
public class Value
{
    public static readonly NullValue ZERO = new NullValue(0);
    public static readonly NullValue ONE = new NullValue(1);

    private string expression;
    private Combatant owner;

    public Value(string expression){
        this.expression = expression;
    }

    public Value(string expression, Combatant owner){
        this.owner = owner;
        this.expression = expression;
    }

    public virtual float calculate(){
        return Calculator.evaluate(expression, owner);
    }
}