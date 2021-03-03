using System;
using Godot;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/*
    This class evaluates infix expressions that are in the form
    of a string, returning a value with float precision. Things
    are kept as floats until the final application of the value
    to keep things relatively accurate (the value is to be converted
    to an integer right before the application). However, it's important
    to note that the rtd() operation treats everything like an
    integer.

    If the expression has any variable tokens like Str or MHP, use the
    version of evaluate() that takes in an owner, or else the result
    will not be as intended. evaluate() needs that owner so they can
    actually get their corresponding stats to fill in the variables.
    One may use evaluateBasic() otherwise, but be careful that there
    are no stat constants.

    Acceptable tokens include:
        -Parentheses: (, )
        -Operators: +, -, *, /, %, d, ^
            (d means dice roll, rtd() method below)
        -Numbers (supports negative symbol)
        -Terms that represent a Stat: Strength, Str, MaxHealth, MHP
            (all supported terms can be found in the Stat file)
    Whitespace will be ignored.

    Examples:
    -3+-3*-3/-3^-3d-3%-3
    1dStr
    Dex/2+2
    (Int-2)*4d2
    Str
    -Str
*/
public static class Calculator
{
    // PEMDAS, excluding parentheses. I've decided dice rolls are higher priority than multiplies but lower than exponents.
    private static Dictionary<String, int> PEMDAS = new Dictionary<String, int>(){
        {"+", 1}, {"-", 1},
        {"*", 2}, {"/", 2}, {"%", 2},
        {"d", 3},
        {"^", 4}
    };

    private static Dictionary<String, Func<float, float, float>> operations = new Dictionary<String, Func<float, float, float>>(){
        {"+", (a, b) => a+b},
        {"-", (a, b) => a-b},
        {"*", (a, b) => a*b},
        {"/", (a, b) => a/b},
        {"%", (a, b) => a%b},
        {"d", (a, b) => rtd(a,b)},
        {"^", (a, b) => (float)Math.Pow(a, b)}
    };

    // this abomination splits an expression into constants, operators, and parenthesis, also accounting for negative values
    private static Regex expressionSplitter = new Regex(@"(?=(\+|\-|\*|\/|%|d|\^|\(|\)))|(?<=(\+|\*|\/|%|d|\^|\(|\)))|(?<=\-)(?<!(\+|\-|\*|\/|%|d|\^|\()\-)(?<!^\-)");

    private static Random random = new Random();

    // public stuff
    public static int rng(int max){
        return random.Next(max);
    }

    public static float evaluate(string expression, Combatant owner){
        return calculate(convertExpression(expression), owner);
    }

    public static float evaluateBasic(string expression){
        return calculate(convertExpression(expression), null);
    }

    // private stuff
    private static string[] convertExpression(String expression){
        return expressionSplitter.Replace(expression.Trim(), ";").Split(";");
    }

    private static float calculate(string[] infix, Combatant owner=null){
        if(infix.Length==0){
            return 0;
        }
        
        Stack<float> valueStack = new Stack<float>();
        Stack<string> opStack = new Stack<string>();

        foreach(string token in infix){
            if(token == "("){
                opStack.Push(token);
            }
            else if(token == ")"){
                while(opStack.Peek() != "("){
                    calcNextOperation(opStack, valueStack);
                }
                opStack.Pop();
            }
            else if(PEMDAS.ContainsKey(token)){
                while(opStack.Count > 0 && PEMDAS.ContainsKey(opStack.Peek()) && PEMDAS[opStack.Peek()] >= PEMDAS[token]){
                    calcNextOperation(opStack, valueStack);
                }
                opStack.Push(token);
            }
            else{
                if(owner != null){
                    valueStack.Push(valueOfConstant(token, owner));
                }
                else {
                    valueStack.Push(int.Parse(token));
                }
            }
        }
        while(opStack.Count > 0){
            calcNextOperation(opStack, valueStack);
        }
        return valueStack.Peek();
    }

    private static void calcNextOperation(Stack<String> opStack, Stack<float> valueStack){
        float b = valueStack.Pop();
        float a = valueStack.Pop();
        valueStack.Push(operations[opStack.Pop()](a, b));
    }

    private static int valueOfConstant(String constant, Combatant owner){
        int negative = 1;
        if(constant.Substring(0,1) == "-"){
            negative = -1;
            constant = constant.Substring(1);
        }
        int value;
        if(!int.TryParse(constant, out value)){
            Stat stat;
            if(Enum.TryParse<Stat>(constant, out stat)){
                return owner.stats().getStat(stat);
            }
            return 0;
        }
        return negative*int.Parse(constant);
    }

    /*
        Rolls a specificed amount of dice with a specified
        number of sides.
        (amount)d(sides)
    */
    private static float rtd(float amount, float sides){
        int negative = 1;
        if(sides == 0){
            return 0;
        }
        else if (sides < 0){
            sides *= -1;
            negative *= -1;
        }
        if(amount < 0){
            amount *= -1;
            negative *= -1;
        }

        int val = 0;
        for(int i = 0; i < amount; i++){
            val += rng((int)sides)+1;
        }
        return (float)(val*negative);
    }
}