﻿#nullable enable
using Microsoft.Maui.Handlers;
using Maui.AutoSuggestBox.Platforms.iOS;
using System.Drawing;
using UIKit;

namespace Maui.AutoSuggestBox.Handlers;

public partial class AutoSuggestBoxHandler : ViewHandler<IAutoSuggestBox, AutoSuggestBoxView>
{
    /// <inheritdoc />
    protected override AutoSuggestBoxView CreatePlatformView() => new AutoSuggestBoxView();

    protected override void ConnectHandler(AutoSuggestBoxView platformView)
    {
        base.ConnectHandler(platformView);

        PlatformView.Text = VirtualView.Text ?? string.Empty;
        PlatformView.Frame = new RectangleF(0, 20, 320, 50);

        UpdateTextColor(platformView);
        UpdatePlaceholderText(platformView);
        UpdatePlaceholderTextColor(platformView);
        UpdateDisplayMemberPath(platformView);
        UpdateIsEnabled(platformView);
        platformView.UpdateTextOnSelect = VirtualView.UpdateTextOnSelect;
        platformView.IsSuggestionListOpen = VirtualView.IsSuggestionListOpen;

        UpdateItemsSource(platformView);

        platformView.SuggestionChosen += OnPlatformViewSuggestionChosen;
        platformView.TextChanged += OnPlatformViewTextChanged;
        platformView.QuerySubmitted += OnPlatformViewQuerySubmitted;

        PlatformView.EditingDidBegin += Control_EditingDidBegin;
        PlatformView.EditingDidEnd += Control_EditingDidEnd;
    }
    protected override void DisconnectHandler(AutoSuggestBoxView platformView)
    {
        base.DisconnectHandler(platformView);
        platformView.SuggestionChosen -= OnPlatformViewSuggestionChosen;
        platformView.TextChanged -= OnPlatformViewTextChanged;
        platformView.QuerySubmitted -= OnPlatformViewQuerySubmitted;
        PlatformView.EditingDidBegin -= Control_EditingDidBegin;
        PlatformView.EditingDidEnd -= Control_EditingDidEnd;
    }

    private void OnPlatformViewSuggestionChosen(object? sender, AutoSuggestBoxSuggestionChosenEventArgs e)
    {
        VirtualView?.SuggestionChosen(e.SelectedItem);
    }
    private void OnPlatformViewTextChanged(object? sender, AutoSuggestBoxTextChangedEventArgs e)
    {
        VirtualView?.NativeControlTextChanged(PlatformView.Text, (AutoSuggestBoxTextChangeReason)e.Reason);
    }
    private void OnPlatformViewQuerySubmitted(object? sender, AutoSuggestBoxQuerySubmittedEventArgs e)
    {
        VirtualView?.RaiseQuerySubmitted(e.QueryText, e.ChosenSuggestion);
    }

    static readonly int baseHeight = 10;
    /// <inheritdoc />
    public override Microsoft.Maui.Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
    {
        var baseResult = base.GetDesiredSize(widthConstraint, heightConstraint);
        var testString = new Foundation.NSString("Tj");
        var testSize = testString.GetSizeUsingAttributes(new UIStringAttributes { Font = PlatformView.Font });
        double height = baseHeight + testSize.Height;
        height = Math.Round(height);
        if (double.IsInfinity(widthConstraint) || double.IsInfinity(heightConstraint))
        {
            // If we drop an infinite value into base.GetDesiredSize for the Editor, we'll
            // get an exception; it doesn't know what do to with it. So instead we'll size
            // it to fit its current contents and use those values to replace infinite constraints

            PlatformView.SizeToFit();
            var sz = new Microsoft.Maui.Graphics.Size(PlatformView.Frame.Width, PlatformView.Frame.Height);
            return sz;
        }

        return base.GetDesiredSize(baseResult.Width, height);
    }

    void Control_EditingDidBegin(object sender, EventArgs e)
    {
        VirtualView.IsFocused = true;
    }
    void Control_EditingDidEnd(object sender, EventArgs e)
    {
        VirtualView.IsFocused = false;
    }

    public static void MapText(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        if (handler.PlatformView.Text != view.Text)
            handler.PlatformView.Text = view.Text;
    }

    public static void MapTextColor(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView?.SetTextColor(view.TextColor);
    }
    public static void MapPlaceholderText(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.PlaceholderText = view.PlaceholderText;
    }
    public static void MapPlaceholderTextColor(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView?.SetPlaceholderTextColor(view.PlaceholderTextColor);
    }
    public static void MapTextMemberPath(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView?.SetItems(view.ItemsSource?.OfType<object>(), (o) => FormatType(o, view.DisplayMemberPath), (o) => FormatType(o, view.TextMemberPath));
    }
    public static void MapDisplayMemberPath(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view.DisplayMemberPath), (o) => FormatType(o, view.TextMemberPath));
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
        handler.PlatformView.UserInteractionEnabled = view.IsEnabled;
    }
    public static void MapItemsSource(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    private void UpdateTextColor(AutoSuggestBoxView platformView)
    {
        platformView.SetTextColor(VirtualView?.TextColor);
    }
    private void UpdateDisplayMemberPath(AutoSuggestBoxView platformView)
    {
        platformView.SetItems(VirtualView.ItemsSource?.OfType<object>(), (o) => FormatType(o, VirtualView.DisplayMemberPath), (o) => FormatType(o, VirtualView.TextMemberPath));
    }
    private void UpdatePlaceholderTextColor(AutoSuggestBoxView platformView)
    {
        platformView.SetPlaceholderTextColor(VirtualView?.PlaceholderTextColor);
    }
    private void UpdatePlaceholderText(AutoSuggestBoxView platformView) => platformView.PlaceholderText = VirtualView?.PlaceholderText;

    private void UpdateIsEnabled(AutoSuggestBoxView platformView)
    {
        platformView.UserInteractionEnabled = (bool)(VirtualView.IsEnabled);
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

