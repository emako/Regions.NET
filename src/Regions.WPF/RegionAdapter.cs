using Regions.Abstractions;
using Regions.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Regions;

/// <summary>
/// RegionAdapter adapts region navigation to different WPF UI containers.
/// 区域适配器：将区域导航适配到不同的 WPF 容器控件。
/// Supported containers: ContentControl, Grid, StackPanel, ItemsControl, Frame, etc.
/// 支持的容器：ContentControl、Grid、StackPanel、ItemsControl、Frame 等。
/// To support a new container, add a new branch in <see cref="RequestNavigate(Uri, object)"/>.
/// 如需支持新的容器，请在 <see cref="RequestNavigate(Uri, object)"/> 中添加分支逻辑。
/// </summary>
public class RegionAdapter(IRegion region, IServiceProvider serviceProvider) : IRegionAdapter
{
    private readonly IRegion _region = region;

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    /// <summary>
    /// Removes the UIElement from its current parent to allow re-parenting.
    /// 从当前父元素中移除 UIElement，以允许重新父化。
    /// </summary>
    /// <param name="element">The UIElement to remove. 要移除的 UIElement。</param>
    private static void RemoveFromParent(UIElement element)
    {
        var parent = LogicalTreeHelper.GetParent(element);
        if (parent is Panel panel)
            panel.Children.Remove(element);
        else if (parent is ContentControl contentControl)
            contentControl.Content = null;
        else if (parent is ItemsControl itemsControl)
            itemsControl.Items.Remove(element);
        else if (parent is Frame frame)
            frame.Content = null;
    }

    /// <summary>
    /// Executes navigation for the region container to show the view resolved by the Uri.
    /// 执行区域容器的导航，展示由 Uri 解析得到的视图。
    /// </summary>
    /// <param name="uri">Target view Uri. 目标视图的 Uri。</param>
    /// <param name="navigationParameters">Optional parameters. 可选参数。</param>
    public void RequestNavigate(Uri uri, object navigationParameters)
    {
        object content = Resolve(uri);

        if (content is DependencyObject dp)
            RegionManager.SetRegionParameter(dp, uri.ToParameters());

        // ContentControl: set Content property to the resolved view
        // ContentControl：将解析出的视图赋值到 Content
        if (_region.Container is ContentControl contentControl)
        {
            if (contentControl.Content is INavigationAware navigationAwareView)
                navigationAwareView.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);
            else if (contentControl.Content is FrameworkElement frameworkElement
                && frameworkElement.DataContext is INavigationAware navigationAwareViewModel)
                navigationAwareViewModel.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);

            if (content is UIElement uiElement)
                RemoveFromParent(uiElement);

            contentControl.Content = content;
        }
        // Grid: add the view as a child and show only the target element
        // Grid：将视图作为子元素添加，仅显示目标元素
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

                RemoveFromParent(element);

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
        // StackPanel：将视图作为子元素添加，仅显示目标元素
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

                RemoveFromParent(element);

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
        // ItemsControl：将解析出的视图添加到 Items 集合
        else if (_region.Container is ItemsControl itemsControl)
        {
            if (content is UIElement uiElement)
                RemoveFromParent(uiElement);

            if (!itemsControl.Items.Contains(content))
                itemsControl.Items.Add(content);
        }
        // Frame: use Frame.Navigate to switch content
        // Frame：使用 Frame.Navigate 切换内容
        else if (_region.Container is Frame frame)
        {
            if (frame.Content is INavigationAware navigationAwareView)
                navigationAwareView.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);
            else if (frame.Content is FrameworkElement frameworkElement
                && frameworkElement.DataContext is INavigationAware navigationAwareViewModel)
                navigationAwareViewModel.OnNavigatedFrom(_region.NavigationService.Journal.CurrentEntry);

            if (content is UIElement uiElement)
                RemoveFromParent(uiElement);

            frame.Navigate(content);
        }

        // Notify the new view or its ViewModel about navigation
        // 通知新视图或其 ViewModel 已完成导航
        {
            if (content is INavigationAware navigationAwareView)
                navigationAwareView.OnNavigatedTo((uri, navigationParameters));
            else if (content is FrameworkElement frameworkElement
                && frameworkElement.DataContext is INavigationAware navigationAwareViewModel)
                navigationAwareViewModel.OnNavigatedTo((uri, navigationParameters));
        }
    }

    /// <summary>
    /// Resolve the view instance for the given URI.
    /// 根据给定的 URI 解析并返回视图实例。
    /// </summary>
    /// <param name="uri">Target view Uri. 目标视图的 Uri。</param>
    /// <returns>The view instance or null. 视图实例或 null。</returns>
    public object Resolve(Uri uri)
    {
        object view = _region.GetView(uri?.ToKey());

        if (view is not null)
            return view;
        else if (_serviceProvider.GetService(typeof(INavigationRegistry)) is INavigationRegistry registry)
            if (registry.GetViewType(uri?.ToKey()) is Type type)
            {
                view = _serviceProvider.GetService(type);
                _region.Add(view, uri.ToKey());
                return view;
            }
        return null;
    }
}
