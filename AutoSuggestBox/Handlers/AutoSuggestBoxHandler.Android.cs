#nullable enable
using Microsoft.Maui.Handlers;
using AutoSuggestBox.Platforms.Android;
using Microsoft.Maui.Platform;

namespace AutoSuggestBox.Handlers;

public partial class AutoSuggestBoxHandler : ViewHandler<IAutoSuggestBox, AutoSuggestBoxView>
{
    /// <inheritdoc />
    //protected override AutoSuggestBoxView CreatePlatformView() => new AutoSuggestBoxView(Context);

    protected override AutoSuggestBoxView CreatePlatformView() => new(Context);
    protected override void ConnectHandler(AutoSuggestBoxView platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SuggestionChosen += OnPlatformViewSuggestionChosen;
        platformView.TextChanged += OnPlatformViewTextChanged;
        platformView.QuerySubmitted += OnPlatformViewQuerySubmitted;
        platformView.SetTextColor(VirtualView?.TextColor.ToPlatform() ?? VirtualView.TextColor.ToPlatform());
    }
    protected override void DisconnectHandler(AutoSuggestBoxView platformView)
    {
        platformView.SuggestionChosen -= OnPlatformViewSuggestionChosen;
        platformView.TextChanged -= OnPlatformViewTextChanged;
        platformView.QuerySubmitted -= OnPlatformViewQuerySubmitted;

        platformView.Dispose();
        base.DisconnectHandler(platformView);
    }

    private void OnPlatformViewSuggestionChosen(object? sender, AutoSuggestBoxSuggestionChosenEventArgs e)
    {
        VirtualView?.RaiseSuggestionChosen(e);
    }
    private void OnPlatformViewTextChanged(object? sender, AutoSuggestBoxTextChangedEventArgs e)
    {
        VirtualView?.NativeControlTextChanged(e);
    }
    private void OnPlatformViewQuerySubmitted(object? sender, AutoSuggestBoxQuerySubmittedEventArgs e)
    {
        VirtualView?.RaiseQuerySubmitted(e);
    }
    public static void MapText(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        if (handler.PlatformView.Text != view.Text)
            handler.PlatformView.Text = view.Text;
    }

    public static void MapTextColor(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView?.SetTextColor(view.TextColor.ToPlatform());
    }
    public static void MapPlaceholderText(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        //handler.PlatformView.Hint = view.PlaceholderText;
        handler.PlatformView.PlaceholderText = view.PlaceholderText;
    }
    public static void MapPlaceholderTextColor(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        //handler.PlatformView?.SetHintTextColor(view.PlaceholderTextColor.ToPlatform());
        handler.PlatformView?.SetPlaceholderTextColor(view.PlaceholderTextColor);
    }
    public static void MapTextMemberPath(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView?.SetItems(view.ItemsSource?.OfType<object>(), (o) => FormatType(o, view.DisplayMemberPath), (o) => FormatType(o, view.TextMemberPath));
    }
    public static void MapDisplayMemberPath(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }
    public static void MapIsSuggestionListOpen(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.IsSuggestionListOpen = view.IsSuggestionListOpen;
    }
    public static void MapUpdateTextOnSelect(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.UpdateTextOnSelect = view.UpdateTextOnSelect;
    }
    public static void MapIsEnabled(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.Enabled = view.IsEnabled;
    }
    public static void MapItemsSource(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    private void UpdateTextColor(AutoSuggestBoxView platformView)
    {
        var color = VirtualView?.TextColor;
#if NETFX_CORE
            Control.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255)));
#elif __ANDROID__ || __IOS__
        platformView.SetTextColor(color.ToPlatform());
#endif
    }
    private void UpdatePlaceholderTextColor(AutoSuggestBoxView platformView)
    {
        var placeholderColor = VirtualView?.PlaceholderTextColor;
#if NETFX_CORE
            // Not currently supported by UWP's control
            // UpdateColor(placeholderColor, ref _placeholderDefaultBrush,
            //     () => Control.PlaceholderForegroundBrush, brush => Control.PlaceholderForegroundBrush = brush);
            // UpdateColor(placeholderColor, ref _defaultPlaceholderColorFocusBrush,
            //     () => Control.PlaceholderForegroundFocusBrush, brush => Control.PlaceholderForegroundFocusBrush = brush);
#elif __ANDROID__ || __IOS__
        platformView.SetPlaceholderTextColor(placeholderColor);
#endif
    }

    private void UpdateDisplayMemberPath(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
#if NETFX_CORE
            Control.DisplayMemberPath = Element.DisplayMemberPath;
#elif __ANDROID__ || __IOS__
        handler.PlatformView.SetItems(view ?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
#endif
    }

    private void UpdateIsEnabled(AutoSuggestBoxView platformView)
    {
#if NETFX_CORE
            platformView.IsEnabled = Element.IsEnabled;
#elif __ANDROID__
        platformView.Enabled = (bool)(VirtualView?.IsEnabled);
#elif __IOS__
            platformView.UserInteractionEnabled = Element.IsEnabled;
#endif
    }

    private void UpdateItemsSource(AutoSuggestBoxView platformView)
    {
#if NETFX_CORE
            platformView.ItemsSource = Element?.ItemsSource;
#elif __ANDROID__ || __IOS__
        platformView.SetItems(VirtualView?.ItemsSource?.OfType<object>(), (o) => FormatType(o, VirtualView?.DisplayMemberPath), (o) => FormatType(o, VirtualView?.TextMemberPath));
#endif
    }

#if __ANDROID__ || __IOS__
    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
        else
            return instance?.ToString() ?? "";
    }
#endif

}
