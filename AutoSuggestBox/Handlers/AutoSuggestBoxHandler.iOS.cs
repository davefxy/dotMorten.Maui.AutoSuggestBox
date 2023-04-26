#nullable enable
using Microsoft.Maui.Handlers;
using Maui.AutoSuggestBox.Platforms.iOS;
using Microsoft.Maui.Platform;
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

        //UpdateTextColor(platformView);
        //UpdatePlaceholderText(platformView);
        //UpdatePlaceholderTextColor(platformView);
        //UpdateIsEnabled(platformView);

        platformView.SuggestionChosen += OnPlatformViewSuggestionChosen;
        platformView.TextChanged += OnPlatformViewTextChanged;
        platformView.QuerySubmitted += OnPlatformViewQuerySubmitted;
        //platformView.UpdateTextOnSelect = e.NewElement.UpdateTextOnSelect;
        //platformView.IsSuggestionListOpen = e.NewElement.IsSuggestionListOpen;
        //Control.EditingDidBegin += Control_EditingDidBegin;
        //Control.EditingDidEnd += Control_EditingDidEnd;
        //Frame = new RectangleF(0, 20, 320, 40);
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

    static readonly int baseHeight = 10;
    /// <inheritdoc />
    //public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
    //{
    //    var baseResult = base.GetDesiredSize(widthConstraint, heightConstraint);
    //    var testString = new Foundation.NSString("Tj");
    //    var testSize = testString.GetSizeUsingAttributes(new UIStringAttributes { Font = Control.Font });
    //    double height = baseHeight + testSize.Height;
    //    height = Math.Round(height);

    //    return new SizeRequest(new global::Xamarin.Forms.Size(baseResult.Request.Width, height));
    //}

    //private void Control_EditingDidBegin(object sender, EventArgs e)
    //{
    //    Element?.SetValue(VisualElement.IsFocusedPropertyKey, true);
    //}
    //private void Control_EditingDidEnd(object sender, EventArgs e)
    //{
    //    Element?.SetValue(VisualElement.IsFocusedPropertyKey, false);
    //}

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
        //handler.PlatformView.Hint = view.PlaceholderText;
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
        handler.PlatformView.UserInteractionEnabled = view.IsEnabled;
    }
    public static void MapItemsSource(AutoSuggestBoxHandler handler, IAutoSuggestBox view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    private void UpdateTextColor(AutoSuggestBoxView platformView)
    {
        var color = VirtualView?.TextColor;
        platformView.SetTextColor(color);
    }
    private void UpdatePlaceholderTextColor(AutoSuggestBoxView platformView)
    {
        var placeholderColor = VirtualView?.PlaceholderTextColor;
        platformView.SetPlaceholderTextColor(placeholderColor);
    }
    private void UpdatePlaceholderText(AutoSuggestBoxView platformView) => platformView.PlaceholderText = VirtualView?.PlaceholderText;

    private void UpdateIsEnabled(AutoSuggestBoxView platformView)
    {
        platformView.UserInteractionEnabled = (bool)(VirtualView?.IsEnabled);
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

