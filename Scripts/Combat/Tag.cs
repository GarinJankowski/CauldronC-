using Godot;
using System;

/*
    Holds an integer and acts like an id
    for CombatOps and Modifiers. See
    TagFactory for more info.
*/
public abstract class Tag : IEquatable<Tag>
{
    protected int id;

    public Tag(int id){
        this.id = id;
    }

    public int ID => id;

    public bool Equals(Tag other){
        return null != other && id == other.ID;
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as Tag);
    }
    public override int GetHashCode()
    {
        return id;
    }

    public abstract Tag getLogTag();
    public abstract Tag getModTag();
}