using System.Collections.Generic;

public interface IKeysProvider
{
    string Provide<TType>();
    IEnumerable<string> ProvideAll();
}