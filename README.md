# Regions.NET

一个轻量级、Prism 风格的 WPF 区域（Region）导航框架，支持 DI、导航历史、生命周期事件、redirect 语义等。

## 特性
- 支持 WPF XAML 区域声明（propdp）
- 支持 DI 容器（Microsoft.Extensions.DependencyInjection）自动解析页面
- 支持导航历史（GoBack/GoForward）
- 支持 redirect 语义（类比 uniapp 的 redirectTo）
- 支持 INavigationAware 生命周期事件（OnNavigatedFrom/OnNavigatedTo）
- 支持页面持有策略（AddSingleton/AddScoped/AddTransient）

## 页面策略
- **AddSingleton**：页面实例全局唯一，始终持有，即使`重定向导航`也不会释放。
- **AddScoped**：同一区域页面唯一，`回退导航`/`前进导航`都能维持页面实例。
- **AddTransient**：每次导航`新建实例`，回退后不再持有，`重定向导航`后页面可被释放。

## 区域适配

支持多种容器类型的区域适配：可扩展适配 ContentControl、ItemsControl、TabControl 等，注册机制自动关联容器与适配器，满足不同 UI 需求。

`src/Regions.WPF/RegionAdapter.cs` 是本库的核心组件之一，负责将区域（Region）机制适配到不同类型的容器控件（如 ContentControl、ItemsControl、TabControl 等）。

通过自定义 RegionAdapter，可以让区域管理器灵活支持多种 UI 容器，极大提升了扩展性和兼容性。

## 快速用法

### 1. 注册服务
```csharp
var services = new ServiceCollection();
services.AddSingleton<IRegionManager, RegionManager>();
services.AddSingleton<IRegionNavigationJournal, RegionNavigationJournal>();
services.AddSingleton<IRegionNavigationService, RegionNavigationService>();
services.AddTransient<PageA>(); // 或 AddScoped/AddSingleton
services.AddTransient<PageB>();
```

### 2. XAML 声明 Region

* 可使用 `RegionName` 附加属性声明 Region。

```xml
<ContentControl regions:RegionManager.RegionName="MainRegion" />
```

添加的 Region 可以用 `IRegionManager.RemoveRegion` / `IRegionManager.ClearRegions` 手动清理。

* 可使用 `AddToRegion` 附加属性将带有名称的控件添加到指定 Region。

```xaml
<Grid regions:RegionManager.RegionName="MainRegion">
    <Grid regions:RegionManager.AddToRegion="MainRegion,/ContentRegion"
              regions:RegionManager.RegionName="ContentRegion" />
</Grid>
```

### 3. 导航页面

```csharp
// 普通导航
_regionManager.RequestNavigate("MainRegion", new Uri("/PageA", UriKind.Relative));
// 重定向导航
_regionManager.RequestRedirect("MainRegion", new Uri("/PageB", UriKind.Relative));
// 回退导航
_regionManager.RequestGoBack("MainRegion");
```

### 4. 页面生命周期
页面或 ViewModel 实现 INavigationAware：
```csharp
public class PageA : UserControl, INavigationAware
{
   public void OnNavigatedFrom(object? parameter = null) { /* 离开时 */ }
   public void OnNavigatedTo(object? parameter = null) { /* 进入时 */ }
}
```

## 适用场景
- 需要 MVVM/DI/导航解耦的 WPF 应用
- 需要页面生命周期、导航历史、redirect 等高级导航特性的桌面应用

