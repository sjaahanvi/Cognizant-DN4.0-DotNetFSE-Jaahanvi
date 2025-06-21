using System;
using System.Collections.Generic;

class Forecast
{
    public static double CalculateFutureValue(double value, double rate, int years)
    {
        if (years == 0)
            return value;

        return (1 + rate) * CalculateFutureValue(value, rate, years - 1);
    }

    public static double CalculateWithMemo(double value, double rate, int years, Dictionary<int, double> memo)
    {
        if (years == 0) return value;

        if (memo.ContainsKey(years))
            return memo[years];

        double result = (1 + rate) * CalculateWithMemo(value, rate, years - 1, memo);
        memo[years] = result;
        return result;
    }

    static void Main()
    {
        double value = 1000;
        double rate = 0.1;
        int years = 5;

        Console.WriteLine($"Future Value (Recursion): {CalculateFutureValue(value, rate, years):C}");

        var memo = new Dictionary<int, double>();
        Console.WriteLine($"Future Value (Memoized): {CalculateWithMemo(value, rate, years, memo):C}");
    }
}
