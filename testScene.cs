using Godot;
using System;
using System.Text.RegularExpressions;

public class testScene : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("HELLO");

        String ex = "(5-2)*3^(6+-4)-2--2+-2^5";
        GD.Print(ex);
        GD.Print(Calculator.evaluateBasic(ex));
        
        ex = "-3+-3*-3/-3^-3d-3";
        GD.Print(ex);
        GD.Print(Calculator.evaluateBasic(ex));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

}
