using Godot;
using System;
using System.Collections.Generic;

/*

*/
public class MapReader : SpreadsheetReader
{
    //private Dictionary<

    public MapReader() : base() {
        this.fileName = "Room Generation.txt";
    }

    protected override void readLine(string[] values){

    }
}