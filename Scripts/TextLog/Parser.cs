
/*
    Represents anything that interprets
    and changes a token from a document going
    into the text log
*/
public interface Parser
{
    string parse(string token);
}