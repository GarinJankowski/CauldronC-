using Godot;
using System;

/*
    returns a 0, or really any integer value you want
    It doesn't take in a float because that probably will never be needed
*/
public class NullValue : Value
{
    int returnAmount;

    public NullValue() : base(""){
        this.returnAmount = 0;
    }

    public NullValue(int returnAmount) : base(""){
        this.returnAmount = returnAmount;
    }

    public override float calculate(){
        return returnAmount;
    }
}