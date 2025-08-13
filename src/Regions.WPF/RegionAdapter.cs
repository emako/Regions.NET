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
        if (navigationParameters is INavigationParameters parameters)
            if (parameters.Redirect)
                _region.Clear();

        object content = Resolve(uri);

        if (_region.Container is ContentControl contentControl)
        {
            if (contentControl.Content is INavigationAware navFrom)
                navFrom.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);
            else if (contentControl.Content is FrameworkElement fe
                && fe.DataContext is INavigationAware navFromVM)
                navFromVM.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);

            contentControl.Content = content;
        }
        else if (_region.Container is Grid grid)
        {
            if (content is UIElement element)
            {
                foreach (UIElement elementEach in grid.Children)
                    if (elementEach.Visibility == Visibility.Visible)
                        if (elementEach is INavigationAware navFrom)
                            navFrom.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);
                        else if (elementEach is FrameworkElement fe
                            && fe.DataContext is INavigationAware navFromVM)
                            navFromVM.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);

                if (!grid.Children.Contains(element))
                    grid.Children.Add(element);

                foreach (UIElement elementEach in grid.Children)
                    elementEach.Visibility =
                        elementEach == element
                            ? Visibility.Visible
                            : Visibility.Collapsed;

                foreach (UIElement elementEach in grid.Children)
                    if (elementEach.Visibility == Visibility.Visible)
                        if (elementEach is INavigationAware navFrom)
                            navFrom.OnNavigatedTo(_region.NavigationService.Journal.CurrentEntry);
                        else if (elementEach is FrameworkElement fe
                            && fe.DataContext is INavigationAware navFromVM)
                            navFromVM.OnNavigatedTo(_region.NavigationService.Journal.CurrentEntry);
            }
        }
        else if (_region.Container is StackPanel stackPanel)
        {
            if (content is UIElement element)
            {
                foreach (UIElement elementEach in stackPanel.Children)
                    if (elementEach.Visibility == Visibility.Visible)
                        if (elementEach is INavigationAware navFrom)
                            navFrom.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);
                        else if (elementEach is FrameworkElement fe
                            && fe.DataContext is INavigationAware navFromVM)
                            navFromVM.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);

                if (!stackPanel.Children.Contains(element))
                    stackPanel.Children.Add(element);

                foreach (UIElement elementEach in stackPanel.Children)
                    elementEach.Visibility =
                        elementEach == element
                            ? Visibility.Visible
                            : Visibility.Collapsed;

                foreach (UIElement elementEach in stackPanel.Children)
                    if (elementEach.Visibility == Visibility.Visible)
                        if (elementEach is INavigationAware navFrom)
                            navFrom.OnNavigatedTo(_region.NavigationService.Journal.CurrentEntry);
                        else if (elementEach is FrameworkElement fe
                            && fe.DataContext is INavigationAware navFromVM)
                            navFromVM.OnNavigatedTo(_region.NavigationService.Journal.CurrentEntry);
            }
        }
        else if (_region.Container is ItemsControl itemsControl)
        {
            if (!itemsControl.Items.Contains(content))
                itemsControl.Items.Add(content);
        }
        else if (_region.Container is Frame frame)
        {
            if (frame.Content is INavigationAware navFrom)
                navFrom.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);
            else if (frame.Content is FrameworkElement fe
                && fe.DataContext is INavigationAware navFromVM)
                navFromVM.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);

            frame.Navigate(content);
        }

        if (content is INavigationAware navTo)
            navTo.OnNavigatedTo((uri, navigationParameters));
        else if (content is FrameworkElement fe2 && fe2.DataContext is INavigationAware navToVM)
            navToVM.OnNavigatedTo((uri, navigationParameters));
    }

    public object Resolve(Uri uri)
    {
        object view = _region.GetView(uri?.OriginalString);

        if (view is not null)
            return view;
        else if (_serviceProvider.GetService(typeof(INavigationRegistry)) is INavigationRegistry registry)
            if (registry.GetViewType(uri?.OriginalString) is Type type)
            {
                view = _serviceProvider.GetService(type);
                _region.Add(view, uri.OriginalString);
                return view;
            }
        return null;
    }
}
