#nullable enable
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using AutoSuggestBoxView = Microsoft.UI.Xaml.Controls.AutoSuggestBox;
using XAutoSuggestBoxSuggestionChosenEventArgs = Microsoft.UI.Xaml.Controls.AutoSuggestBoxSuggestionChosenEventArgs;
using XAutoSuggestBoxTextChangedEventArgs = Microsoft.UI.Xaml.Controls.AutoSuggestBoxTextChangedEventArgs;
using XAutoSuggestBoxQuerySubmittedEventArgs = Microsoft.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs;
using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml.Controls;

namespace Maui.AutoSuggestBox.Handlers;

public partial class AutoSuggestBoxHandler : ViewHandler<IAutoSuggestBox, AutoSuggestBoxView>
{
    /// <inheritdoc />
    protected override AutoSuggestBoxView CreatePlatformView() => new AutoSuggestBoxView();
    protected override void ConnectHandler(AutoSuggestBoxView platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SuggestionChosen += OnPlatformViewSuggestionChosen;
        platformView.TextChanged += OnPlatformViewTextChanged;
        platformView.QuerySubmitted += OnPlatformViewQuerySubmitted;
        UpdateTextMemberPath(platformView);
        platformView.GotFocus += PlatformView_GotFocus;
    }
    protected override void DisconnectHandler(AutoSuggestBoxView platformView)
    {
        platformView.SuggestionChosen -= OnPlatformViewSuggestionChosen;
        platformView.TextChanged -= OnPlatformViewTextChanged;
        platformView.QuerySubmitted -= OnPlatformViewQuerySubmitted;
        platformView.GotFocus -= PlatformView_GotFocus;

        //platformView.Dispose();
        base.DisconnectHandler(platformView);
    }

    private void OnPlatformViewSuggestionChosen(object? sender, XAutoSuggestBoxSuggestionChosenEventArgs e)
    {
        AutoSuggestBoxSuggestionChosenEventArgs args = new AutoSuggestBoxSuggestionChosenEventArgs(e.SelectedItem);
        VirtualView?.RaiseSuggestionChosen(args);
    }
    private void OnPlatformViewTextChanged(object? sender, XAutoSuggestBoxTextChangedEventArgs e)
    {
        if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            AutoSuggestBoxTextChangedEventArgs args = new AutoSuggestBoxTextChangedEventArgs(((AutoSuggestBoxView)sender).Text, AutoSuggestBoxTextChangeReason.UserInput);
            VirtualView?.NativeControlTextChanged(args);
        }
    }
    private void OnPlatformViewQuerySubmitted(object? sender, XAutoSuggestBoxQuerySubmittedEventArgs e)
    {
        AutoSuggestBoxQuerySubmittedEventArgs args = new AutoSuggestBoxQuerySubmittedEventArgs(e.QueryText, e.ChosenSuggestion);
        VirtualView?.RaiseQuerySubmitted(args);
    }
    public static void MapText(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        if (handler.PlatformView.Text != view.Text)
            handler.PlatformView.Text = view.Text;
    }

    public static void MapTextColor(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        var color = view?.TextColor;
        handler.PlatformView.Foreground = color.ToPlatform();
    }
    public static void MapPlaceholderText(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.PlaceholderText = view.PlaceholderText;
    }
    public static void MapPlaceholderTextColor(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        //handler.PlatformView?.SetPlaceholderTextColor(view.PlaceholderTextColor);
    }
    public static void MapTextMemberPath(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.TextMemberPath = view.TextMemberPath;
    }
    public static void MapDisplayMemberPath(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.DisplayMemberPath = view?.DisplayMemberPath;
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
        handler.PlatformView.IsEnabled = view.IsEnabled;
    }
    public static void MapItemsSource(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.ItemsSource = view?.ItemsSource;
    }
    private void PlatformView_GotFocus(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        //if (Element?.ItemsSource?.Count > 0)
            VirtualView.IsSuggestionListOpen = true;
    }
    private void UpdateTextMemberPath(AutoSuggestBoxView platformView)
    {
        platformView.TextMemberPath = VirtualView.TextMemberPath;
    }

    private void UpdateTextColor(AutoSuggestBoxView platformView)
    {
        var color = VirtualView?.TextColor;
        platformView.Foreground = color.ToPlatform();
    }
    private void UpdateDisplayMemberPath(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.DisplayMemberPath = view?.DisplayMemberPath;
    }
    private void UpdatePlaceholderText(AutoSuggestBoxView platformView) => platformView.PlaceholderText = VirtualView?.PlaceholderText;
}