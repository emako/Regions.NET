using System;

namespace Regions;

public interface INavigationAware
{
    public void OnNavigatedFrom((Uri, object) entry);

    public void OnNavigatedTo((Uri, object) entry);
}
