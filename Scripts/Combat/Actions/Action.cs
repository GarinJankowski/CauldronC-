using System;
using System.Collections.Generic;

/*

*/
public class Action
{
    private List<CombatOp> combatOps = new List<CombatOp>();
    private Dictionary<string, int> results = new Dictionary<string, int>();

    private bool hostile;
    private string logOutput;

    public Action(){}

    public Action(string logOutput){
        this.logOutput = logOutput;
    }

    public void execute(Combatant source, Combatant target){
        // reserve log
        
        foreach(CombatOp op in combatOps){
            op.execute(source, target, this);
        }

        if(logOutput != null){
            // todo
        }
        clearResults();
    }

    public void addCombatOp(CombatOp op){
        combatOps.Add(op);
        if(op.isHostile)
            hostile = true;
    }

    public void updateResult(string tag, int amount){
        if(!results.ContainsKey(tag))
            results.Add(tag, 0);
        results[tag] += amount;
    }

    public bool isHostile => hostile;

    private void clearResults(){
        foreach (string key in results.Keys){
            results[key] = 0;
        }
    }
}