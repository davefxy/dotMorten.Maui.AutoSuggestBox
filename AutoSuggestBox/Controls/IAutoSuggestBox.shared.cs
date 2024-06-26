﻿namespace Maui.AutoSuggestBox;

public interface IAutoSuggestBox : IView
{
    string Text { get; set; }
    Color TextColor { get; set; }
    string PlaceholderText { get; set; }
    Color PlaceholderTextColor { get; set; }
    string TextMemberPath { get; set; }
    string DisplayMemberPath { get; set; }
    bool IsSuggestionListOpen { get; set; }
    bool UpdateTextOnSelect { get; set; }
    System.Collections.IList ItemsSource { get; set; }

    void SuggestionChosen(object selectedItem);
    void NativeControlTextChanged(string? text, AutoSuggestBoxTextChangeReason reason);
    void RaiseQuerySubmitted(string? queryText, object? chosenSuggestion);
}
