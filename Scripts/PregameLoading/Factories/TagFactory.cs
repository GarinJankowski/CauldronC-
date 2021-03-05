using Godot;
using System;
using System.Collections.Generic;

/*
    The Tag class is basically just an integer that is used
    as an id. The main thing about this is that it has to be
    validated and instantiated by TagFactory, so that only
    valid Tags exist. This is why the regular Tag class is
    abstract and this factory has its own PrivateTag class,
    so it cannot be instantiated outside of here.

    Tags exist to basically act as a variable for different
    CombatOps and Modifiers gathered from the spreadsheet.
    We need variables for these classes as an identifier for a few things:
        - tells the parent Action of a CombatOp which operation it is, so that operation results
          can be kept track of
        - tells the TextLog which CombatOp it is, so that it can retrieve those
          results
        - tells the ModifierManager which CombatOp this is, so that this
          gets modified by all of the source's and target's Modifier with that tag

    The reason we're not just using the Ops enums set up in BasicOp and EffectOp is
    because there are more variations than just those enums.
    First, there must be an indication of whether 

    todo
*/
public class TagFactory
{
    private Dictionary<string, int> effectPositions;

    public TagFactory(){

    }

    public Tag getTag(string token){
        return new PrivateTag(toTag(token));
    }

    private int toTag(string token){
        int id = 0;
        bool negative = false;
        string tk = token;

        if(tk.BeginsWith("!")){
            negative = true;
            tk = tk.Substring(1);
        }

        if(Enum.TryParse<BasicOp.Ops>(tk, out var result)){
            id += (int)result*10;
        }
        else if(Enum.TryParse<EffectOp.Ops>(tk, out var result2)){
            id += (int)result2*10 + 1;
        }
        else{
            string startPart;
            string effectPart;
            try{
                id += 2;
                startPart = tk.Substring(0, 4);
                effectPart = tk.Substring(4);
                if(effectPart.EndsWith("_Turns")){
                    effectPart = effectPart.Substring(effectPart.Length-6);
                }
                else if(effectPart.EndsWith("_Amount")){
                    effectPart = effectPart.Substring(effectPart.Length-7);
                    id += 1;
                }
            }
            catch(ArgumentOutOfRangeException){
                throw new ArgumentException("Illegal Tag string passed.");
            }

            if(effectPositions.ContainsKey(effectPart)){
                id += effectPositions[effectPart]*100;
                
                if(startPart == "lose"){
                    id += 10;
                }
                else if(startPart != "gain"){
                    throw new ArgumentException("Illegal Tag string passed.");
                }
            }
            else{
                throw new ArgumentException("Illegal Tag string passed.");
            }
        }
        
        if(negative){
            id *= -1;
        }
        return id;
    }

    /*
        This is here so the factory can actually instantiate a Tag.
        The regular Tag class is abstract so no one else can instantiatie it.
    */
    class PrivateTag : Tag
    {

        public PrivateTag(int id) : base(id){}

        public override Tag getLogTag()
        {
            int newid = id;
            if(Math.Abs(id) % 10 > 2){
                newid = (newid/10)*10 + 2;
            }
            return new PrivateTag(newid);
        }

        public override Tag getModTag()
        {
            int newid = Math.Abs(id);
            return new PrivateTag(newid);
        }
    }
}