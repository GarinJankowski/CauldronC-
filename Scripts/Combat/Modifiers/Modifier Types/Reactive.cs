using Godot;
using System;

/*

*/
public class Reactive
{
    private Modifier modifier;
    private Action action;

    public Reactive(Modifier modifier, Action action){
        this.modifier = modifier;
        this.action = action;
    }


}