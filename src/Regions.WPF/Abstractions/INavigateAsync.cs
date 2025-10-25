using System;

namespace Regions;

public interface INavigateAsync
{
    public void RequestNavigate(Uri target, object navigationParameters);
}
