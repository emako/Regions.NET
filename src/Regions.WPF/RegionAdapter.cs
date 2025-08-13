using System;
using System.Windows;
using System.Windows.Controls;

namespace Regions;

public class RegionAdapter(IRegion region, IServiceProvider serviceProvider) : IRegionAdapter
{
    private readonly IRegion _region = region;

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void RequestNavigate(Uri uri, object navigationParameters)
    {
        object content = Resolve(uri);

        if (_region.Container is ContentControl contentControl)
        {
            contentControl.Content = content;
        }
        else if (_region.Container is Grid grid)
        {
            if (content is UIElement element)
            {
                if (!grid.Children.Contains(element))
                {
                    grid.Children.Add(element);
                }

                foreach (UIElement elementEach in grid.Children)
                {
                    elementEach.Visibility =
                        elementEach == element
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                }
            }
        }
        else if (_region.Container is StackPanel stackPanel)
        {
            if (content is UIElement element)
            {
                if (!stackPanel.Children.Contains(element))
                {
                    stackPanel.Children.Add(element);
                }

                foreach (UIElement elementEach in stackPanel.Children)
                {
                    elementEach.Visibility =
                        elementEach == element
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                }
            }
        }
        else if (_region.Container is ItemsControl itemsControl)
        {
            if (!itemsControl.Items.Contains(content))
            {
                itemsControl.Items.Add(content);
            }
        }
        else if (_region.Container is Frame frame)
        {
            frame.Navigate(content);
        }
    }

    public object Resolve(Uri uri)
    {
        object view = _region.GetView(uri?.OriginalString);

        if (view is not null)
        {
            return view;
        }
        else
        {
            if (_serviceProvider.GetService(typeof(INavigationRegistry)) is INavigationRegistry registry)
            {
                if (registry.GetViewType(uri?.OriginalString) is Type type)
                {
                    return _serviceProvider.GetService(type);
                }
            }
        }

        return null;
    }
}
