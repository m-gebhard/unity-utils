using System;

public interface IPredicate
{
    public bool Evaluate();
}

public class FunctionPredicate : IPredicate
{
    private readonly Func<bool> predicate;

    public FunctionPredicate(Func<bool> predicate)
    {
        this.predicate = predicate;
    }

    public bool Evaluate() => predicate();
}