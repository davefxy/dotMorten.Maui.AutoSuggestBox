//using Microsoft.Maui.Controls.Handlers.Compatibility;
//using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System.ComponentModel;

using System;
using System.Drawing;
using System.Linq;
#if ANDROID
using NativeAutoSuggestBox = AutoSuggestBox.Platforms.Android.AndroidAutoSuggestBox;
namespace AutoSuggestBox.Platforms.Android
#elif IOS
using NativeAutoSuggestBox = AutoSuggestBox.Platforms.iOS.iOSAutoSuggestBox;
using UIKit;
namespace AutoSuggestBox.Platforms.IOS
#endif
{
/// <summary>
/// Platform specific renderer for the <see cref="AutoSuggestBox"/>
/// </summary>
    public class AutoSuggestBoxHandler : ViewRenderer<AutoSuggestBox, NativeAutoSuggestBox>
{
#if !NETFX_CORE
    private bool suppressTextChangedEvent;
#endif

#if ANDROID
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoSuggestBoxHandler"/>
    /// </summary>
    /// <param name="context">Context</param>
    public AutoSuggestBoxHandler(global::Android.Content.Context context) : base(context)
    {
    }
#endif

#if IOS
        static readonly int baseHeight = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoSuggestBoxHandler"/>
        /// </summary>
        public AutoSuggestBoxHandler()
        {
            Frame = new RectangleF(0, 20, 320, 40);
        }

        /// <inheritdoc />
        //public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        //{
        //    var baseResult = base.GetDesiredSize(widthConstraint, heightConstraint);
        //    var testString = new Foundation.NSString("Tj");
        //    var testSize = testString.GetSizeUsingAttributes(new UIStringAttributes { Font = Control.Font });
        //    double height = baseHeight + testSize.Height;
        //    height = Math.Round(height);

        //    return new SizeRequest(Size(baseResult.Request.Width, height));
        //}
#endif

    /// <inheritdoc />
    protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestBox> e)
    {
        base.OnElementChanged(e);

        if (e.OldElement != null)
        {
            if (Control != null)
            {
                Control.SuggestionChosen -= AutoSuggestBox_SuggestionChosen;
                Control.TextChanged -= AutoSuggestBox_TextChanged;
                Control.QuerySubmitted -= AutoSuggestBox_QuerySubmitted;
#if IOS
                    Control.EditingDidBegin -= Control_EditingDidBegin;
                    Control.EditingDidEnd -= Control_EditingDidEnd;
#elif NETFX_CORE
                    Control.GotFocus -= Control_GotFocus;
#endif
            }
        }

        if (e.NewElement != null)
        {
            if (Control == null)
            {
                var box = CreateNativeControl();
                SetNativeControl(box);
            }
            Control.Text = e.NewElement.Text ?? string.Empty;
            UpdateTextColor();
            UpdatePlaceholderText();
            UpdatePlaceholderTextColor();
            UpdateTextMemberPath();
            UpdateDisplayMemberPath();
            UpdateIsEnabled();
            Control.UpdateTextOnSelect = e.NewElement.UpdateTextOnSelect;
            Control.IsSuggestionListOpen = e.NewElement.IsSuggestionListOpen;
            UpdateItemsSource();

            Control.SuggestionChosen += AutoSuggestBox_SuggestionChosen;
            Control.TextChanged += AutoSuggestBox_TextChanged;
            Control.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
#if IOS
                Control.EditingDidBegin += Control_EditingDidBegin;
                Control.EditingDidEnd += Control_EditingDidEnd;
#elif NETFX_CORE
                Control.GotFocus += Control_GotFocus;
#endif
        }
    }

#if IOS
        private void Control_EditingDidBegin(object sender, EventArgs e)
        {
            Element?.SetValue(VisualElement.IsFocusedPropertyKey, true);
        }
        private void Control_EditingDidEnd(object sender, EventArgs e)
        {
            Element?.SetValue(VisualElement.IsFocusedPropertyKey, false);
        }
#elif NETFX_CORE
        private void Control_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Element?.ItemsSource?.Count > 0)
                (sender as NativeAutoSuggestBox).IsSuggestionListOpen = true;
        }
#endif

    private void AutoSuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
    {
        MessagingCenter.Send(Element, "AutoSuggestBox_" + nameof(AutoSuggestBox.QuerySubmitted), (e.QueryText, e.ChosenSuggestion));
    }

    private void AutoSuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
    {
        MessagingCenter.Send(Element, "AutoSuggestBox_" + nameof(AutoSuggestBox.TextChanged), (Control.Text, (AutoSuggestBoxTextChangeReason)e.Reason));
    }

    private void AutoSuggestBox_SuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
    {
        MessagingCenter.Send(Element, "AutoSuggestBox_" + nameof(AutoSuggestBox.SuggestionChosen), e.SelectedItem);
    }

    /// <inheritdoc />
#if NETFX_CORE
        protected NativeAutoSuggestBox CreateNativeControl()
#else
    protected override NativeAutoSuggestBox CreateNativeControl()
#endif
    {
#if ANDROID
        return new AndroidAutoSuggestBox(this.Context);
#elif IOS
            return new iOSAutoSuggestBox();
#elif NETFX_CORE
            return new Windows.UI.Xaml.Controls.AutoSuggestBox();
#else
            throw new NotImplementedException();
#endif
    }

    /// <inheritdoc />
    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (Control == null)
        {
            return;
        }
        if (e.PropertyName == nameof(AutoSuggestBox.Text))
        {
            if (Control.Text != Element.Text)
                Control.Text = Element.Text;
        }
        else if (e.PropertyName == nameof(AutoSuggestBox.TextColor))
        {
            UpdateTextColor();
        }
        else if (e.PropertyName == nameof(AutoSuggestBox.PlaceholderText))
        {
            UpdatePlaceholderText();
        }
        else if (e.PropertyName == nameof(AutoSuggestBox.PlaceholderTextColor))
        {
            UpdatePlaceholderTextColor();
        }
        else if (e.PropertyName == nameof(AutoSuggestBox.TextMemberPath))
        {
            UpdateTextMemberPath();
        }
        else if (e.PropertyName == nameof(AutoSuggestBox.DisplayMemberPath))
        {
            UpdateDisplayMemberPath();
        }
        else if (e.PropertyName == nameof(AutoSuggestBox.IsEnabled))
        {
            UpdateIsEnabled();
        }
        else if (e.PropertyName == nameof(AutoSuggestBox.IsSuggestionListOpen))
        {
            Control.IsSuggestionListOpen = Element.IsSuggestionListOpen;
        }
        else if (e.PropertyName == nameof(AutoSuggestBox.UpdateTextOnSelect))
        {
            Control.UpdateTextOnSelect = Element.UpdateTextOnSelect;
        }
        else if (e.PropertyName == nameof(AutoSuggestBox.ItemsSource))
        {
            UpdateItemsSource();
        }
        base.OnElementPropertyChanged(sender, e);
    }

    private void UpdateTextColor()
    {
        var color = Element.TextColor;
#if NETFX_CORE
            Control.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255)));
#elif ANDROID || IOS
        Control.SetTextColor(color.ToPlatform());
#endif
    }

#if NETFX_CORE
        // Brush _placeholderDefaultBrush;
        // Brush _defaultPlaceholderColorFocusBrush;
#endif
    private void UpdatePlaceholderTextColor()
    {
        var placeholderColor = Element.PlaceholderTextColor;
#if NETFX_CORE
            // Not currently supported by UWP's control
            // UpdateColor(placeholderColor, ref _placeholderDefaultBrush,
            //     () => Control.PlaceholderForegroundBrush, brush => Control.PlaceholderForegroundBrush = brush);
            // UpdateColor(placeholderColor, ref _defaultPlaceholderColorFocusBrush,
            //     () => Control.PlaceholderForegroundFocusBrush, brush => Control.PlaceholderForegroundFocusBrush = brush);
#elif ANDROID || IOS
        Control.SetPlaceholderTextColor(placeholderColor);
#endif
    }

    private void UpdatePlaceholderText() => Control.PlaceholderText = Element.PlaceholderText;

    private void UpdateTextMemberPath()
    {
#if NETFX_CORE
            Control.TextMemberPath = Element.TextMemberPath;
#endif
    }

    private void UpdateDisplayMemberPath()
    {
#if NETFX_CORE
            Control.DisplayMemberPath = Element.DisplayMemberPath;
#elif ANDROID || IOS
        Control.SetItems(Element.ItemsSource?.OfType<object>(), (o) => FormatType(o, Element.DisplayMemberPath), (o) => FormatType(o, Element.TextMemberPath));
#endif
    }

    private void UpdateIsEnabled()
    {
#if NETFX_CORE
            Control.IsEnabled = Element.IsEnabled;
#elif ANDROID
        Control.Enabled = Element.IsEnabled;
#elif IOS
            Control.UserInteractionEnabled = Element.IsEnabled;
#endif
    }

    private void UpdateItemsSource()
    {
#if NETFX_CORE
            Control.ItemsSource = Element?.ItemsSource;
#elif ANDROID || IOS
        Control.SetItems(Element?.ItemsSource?.OfType<object>(), (o) => FormatType(o, Element?.DisplayMemberPath), (o) => FormatType(o, Element?.TextMemberPath));
#endif
    }

#if ANDROID || IOS
    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
        else
            return instance?.ToString() ?? "";
    }
#endif
}
}