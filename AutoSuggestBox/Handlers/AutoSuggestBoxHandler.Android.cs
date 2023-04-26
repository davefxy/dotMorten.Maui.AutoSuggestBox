﻿#nullable enable
using Microsoft.Maui.Handlers;
using Maui.AutoSuggestBox.Platforms.Android;
using Microsoft.Maui.Platform;

namespace Maui.AutoSuggestBox.Handlers;

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
        platformView.SetTextColor(color.ToPlatform());
    }
    private void UpdatePlaceholderTextColor(AutoSuggestBoxView platformView)
    {
        var placeholderColor = VirtualView?.PlaceholderTextColor;
        platformView.SetPlaceholderTextColor(placeholderColor);
    }

    private void UpdateDisplayMemberPath(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.SetItems(view ?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    private void UpdateIsEnabled(AutoSuggestBoxView platformView)
    {
        platformView.Enabled = (bool)(VirtualView?.IsEnabled);
    }

    private void UpdateItemsSource(AutoSuggestBoxView platformView)
    {
        platformView.SetItems(VirtualView?.ItemsSource?.OfType<object>(), (o) => FormatType(o, VirtualView?.DisplayMemberPath), (o) => FormatType(o, VirtualView?.TextMemberPath));
    }

    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
        else
            return instance?.ToString() ?? "";
    }
}
