using Godot;
using System;
using System.Collections.Generic;

/*

*/
public class TextInterpreter
{
    private ActionParser

    public TextInterpreter(){
        parsers = new List<Parser>(){
            new ActionParser(),
            new ColorParser()
        };
    }

    public string interpret(string document){
        string finalString = "";
        foreach(string token in document.Split(" ")){
            finalString += parseToken(token) + " ";
        }
        return finalString;
    }

    private string parseToken(string token){
        foreach(Parser parser in parsers){
            token = parser.parse(token);
        }
        return token;
    }
}