using System;
using System.Windows;
using System.Windows.Controls;

namespace Regions;

/// <summary>
/// RegionAdapter adapts region navigation to different UI containers in WPF.
/// Supported containers: ContentControl, Grid, StackPanel, ItemsControl, Frame, etc.
/// To support a new container, add a new branch in RequestNavigate.
/// </summary>
public class RegionAdapter(IRegion region, IServiceProvider serviceProvider) : IRegionAdapter
{
    private readonly IRegion _region = region;

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void RequestNavigate(Uri uri, object navigationParameters)
    {
        object content = Resolve(uri);

        // ContentControl: set Content property to the resolved view
        if (_region.Container is ContentControl contentControl)
        {
            if (contentControl.Content is INavigationAware navigationAwareView)
                navigationAwareView.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);
            else if (contentControl.Content is FrameworkElement frameworkElement
                && frameworkElement.DataContext is INavigationAware navigationAwareViewModel)
                navigationAwareViewModel.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);

            contentControl.Content = content;
        }
        // Grid: add the view as a child and show only the target element
        else if (_region.Container is Grid grid)
        {
            if (content is UIElement element)
            {
                foreach (UIElement elementEach in grid.Children)
                    if (elementEach.Visibility == Visibility.Visible)
                        if (elementEach is INavigationAware navigationAwareView)
                            navigationAwareView.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);
                        else if (elementEach is FrameworkElement frameworkElement
                            && frameworkElement.DataContext is INavigationAware navigationAwareViewModel)
                            navigationAwareViewModel.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);

                if (!grid.Children.Contains(element))
                    grid.Children.Add(element);

                foreach (UIElement elementEach in grid.Children)
                    elementEach.Visibility =
                        elementEach == element
                            ? Visibility.Visible
                            : Visibility.Collapsed;

                foreach (UIElement elementEach in grid.Children)
                    if (elementEach.Visibility == Visibility.Visible)
                        if (elementEach is INavigationAware navigationAwareView)
                            navigationAwareView.OnNavigatedTo(_region.NavigationService.Journal.CurrentEntry);
                        else if (elementEach is FrameworkElement frameworkElement
                            && frameworkElement.DataContext is INavigationAware navigationAwareViewModel)
                            navigationAwareViewModel.OnNavigatedTo(_region.NavigationService.Journal.CurrentEntry);
            }
        }
        // StackPanel: add the view as a child and show only the target element
        else if (_region.Container is StackPanel stackPanel)
        {
            if (content is UIElement element)
            {
                foreach (UIElement elementEach in stackPanel.Children)
                    if (elementEach.Visibility == Visibility.Visible)
                        if (elementEach is INavigationAware navigationAwareView)
                            navigationAwareView.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);
                        else if (elementEach is FrameworkElement frameworkElement
                            && frameworkElement.DataContext is INavigationAware navigationAwareViewModel)
                            navigationAwareViewModel.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);

                if (!stackPanel.Children.Contains(element))
                    stackPanel.Children.Add(element);

                foreach (UIElement elementEach in stackPanel.Children)
                    elementEach.Visibility =
                        elementEach == element
                            ? Visibility.Visible
                            : Visibility.Collapsed;

                foreach (UIElement elementEach in stackPanel.Children)
                    if (elementEach.Visibility == Visibility.Visible)
                        if (elementEach is INavigationAware navigationAwareView)
                            navigationAwareView.OnNavigatedTo(_region.NavigationService.Journal.CurrentEntry);
                        else if (elementEach is FrameworkElement frameworkElement
                            && frameworkElement.DataContext is INavigationAware navigationAwareViewModel)
                            navigationAwareViewModel.OnNavigatedTo(_region.NavigationService.Journal.CurrentEntry);
            }
        }
        // ItemsControl: add the resolved view to Items collection
        else if (_region.Container is ItemsControl itemsControl)
        {
            if (!itemsControl.Items.Contains(content))
                itemsControl.Items.Add(content);
        }
        // Frame: use Frame.Navigate to switch content
        else if (_region.Container is Frame frame)
        {
            if (frame.Content is INavigationAware navigationAwareView)
                navigationAwareView.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);
            else if (frame.Content is FrameworkElement frameworkElement
                && frameworkElement.DataContext is INavigationAware navigationAwareViewModel)
                navigationAwareViewModel.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);

            frame.Navigate(content);
        }

        // Notify the new view or its ViewModel about navigation
        {
            if (content is INavigationAware navigationAwareView)
                navigationAwareView.OnNavigatedTo((uri, navigationParameters));
            else if (content is FrameworkElement frameworkElement
                && frameworkElement.DataContext is INavigationAware navigationAwareViewModel)
                navigationAwareViewModel.OnNavigatedTo((uri, navigationParameters));
        }
    }

    /// <summary>
    /// Resolve the view instance for the given URI
    /// </summary>
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
