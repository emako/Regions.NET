using System;

namespace Regions;

public interface INavigationRegistry
{
    public void Register<TView>(string key);

    public void Register(Type type, string key);

    public Type GetViewType(string key);
}
