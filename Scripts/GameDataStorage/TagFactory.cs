using Godot;
using System;
using System.Collections.Generic;

/*
    The Tag class is basically just a value that is used
    as an id and can be compared to other Tags. The main
    thing about this value is that it has to be
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
    
    All of the uses for Tags stated above are slightly varied from each other.
    The normal tag (produced by the toTag() method) contains the following information about a CombatOp:
        - selfTargeted
        - BasicOp, EffectOp_Amount, or EffectOp_Turns
        - if it's a BasicOp:
            - which BasicOp.Ops it is
        - if it's not a BasicOp:
            - the particular Effect being applied
            - whether it's a gain or loss

    Variations:
        There are log tags for the TextLog. These tags do not take into account whether an EffectOp tag
        is Amount or Turns.

        There are mod tags for all Modifiers. These tags do not take into account whether a CombatOp
        is selfTargeted or not.
*/
public class TagFactory
{
    private Dictionary<string, int> effectPositions;

    public TagFactory(){
        // todo
    }

    public Tag getTag(string title){
        return new PrivateTag(toTag(title));
    }

    private int toTag(string title){
        int id = 0;
        bool negative = false;
        string tk = title;

        // selfTargeted = positive id
        // not selfTargeted = negative id
        if(tk.BeginsWith("!")){
            negative = true;
            tk = tk.Substring(1);
        }

        // BasicOp means first digit = 1 and all other digits are its BasicOp.Ops integer value
        if(Enum.TryParse<CombatOpManager.Basic>(tk, out var result)){
            id += (int)result*10 + 1;
        }
        // EffectOp has a bit more stuff going on
        else{
            
            // first digit is 2 by default
            id += 2;
            // if it is an Amount tag, nothing changes
            // if it is a Turns tag, first digit is 3
            // not being either of those is fine, too, since a "neither" tag will be used in a different place
            if(tk.EndsWith("_Amount")){
                    tk = tk.Substring(tk.Length-7);
                }
            else if(tk.EndsWith("_Turns")){
                    tk = tk.Substring(tk.Length-6);
                id += 1;
            }

            // try to see if the title is more than 4 characters at this point
            // (it should be, since it should start with either "gain" or "lose")
            // if it's not, then it's wrong
            string startPart;
            string effectPart;
            try{
                startPart = tk.Substring(0, 4);
                effectPart = tk.Substring(4);
            }
            catch(ArgumentOutOfRangeException){
                throw new ArgumentException("Illegal Tag string passed.");
            }

            // if it starts with lose, the second digit is 1
            // if it starts with gain, the second digit is 0
            // otherwise, throw exception
            if(startPart == "lose"){
                    id += 10;
                }
            else if(startPart != "gain"){
                throw new ArgumentException("Illegal Tag string passed.");
            }
            
            // if the resulting effect name is not an existing effect, then it is wrong
            // otherwise, the rest of the digits past the tens represent the which position
            // this effect is in our effect factory's effect list
            if(effectPositions.ContainsKey(effectPart)){
                id += effectPositions[effectPart]*100;
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
        The log tags are put into logOutputs and are used by the TextLog.
        It is a separate tag becuase, for EffectOps, the TextLog itself does not
        care whether it is an Amount tag or a Turn tag, so this removes that distinction.
    */
    private static int toLogTag(int id){
        int newid = id;
        // if the first digit is greater than 2, replace it with 2
        if(Math.Abs(id) % 10 > 2){
            newid = (newid/10)*10 + 2;
        }
        return newid;
    }

    /*
        Mod tags are used by modifiers of all kind to know which CombatOp it should modify.
        It is a separate tag because modifiers do not care whether an operation is selfTargeted
        or not, so this removes that distinction.
    */
    private static int toModTag(int id){
        // get the absolute value of the id
        int newid = Math.Abs(id);
        return newid;
        
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
            return new PrivateTag(toLogTag(id));
        }

        public override Tag getModTag()
        {
            return new PrivateTag(toModTag(id));
        }
    }
}