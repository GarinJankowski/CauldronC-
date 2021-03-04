using System;

/*

*/
public class Auxiliary
{
    private Action action;
    
    public Auxiliary(Action action){
        this.action = action;
    }

    public void execute(Combatant source, Combatant target){
        action.execute(source, target);
    }
}