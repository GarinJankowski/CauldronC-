using Godot;
using System;
using System.IO;

/*

*/
public abstract class SpreadsheetReader
{
    private const string DELIMITER = "/t";
    private const string PATH_START = @"res://Spreadsheets/";
    protected string fileName;

    public SpreadsheetReader(){
        readSheet();
    }

    private void readSheet(){
        using(var reader = new StreamReader(PATH_START + fileName))
        {
            reader.ReadLine();
            while(!reader.EndOfStream)
            {
                var values = reader.ReadLine().Split(DELIMITER);
                if(values.Length > 0){
                    readLine(values);
                }
            }
        }
    }

    protected abstract void readLine(string[] values);
}