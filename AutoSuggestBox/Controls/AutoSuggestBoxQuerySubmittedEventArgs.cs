﻿namespace AutoSuggestBox
{
    /// <summary>
    /// Provides event data for the AutoSuggestBox.QuerySubmitted event.
    /// </summary>
    public sealed class AutoSuggestBoxQuerySubmittedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> class.
        /// </summary>
        /// <param name="queryText"></param>
        /// <param name="chosenSuggestion"></param>
        internal AutoSuggestBoxQuerySubmittedEventArgs(string queryText, object chosenSuggestion)
        {
            QueryText = queryText;
            ChosenSuggestion = chosenSuggestion;
        }

        /// <summary>
        /// Gets the suggested result that the use chose.
        /// </summary>
        /// <value>The suggested result that the use chose.</value>
        public object ChosenSuggestion { get; }

        /// <summary>
        /// The query text of the current search.
        /// </summary>
        /// <value>Gets the query text of the current search.</value>
        public string QueryText { get; }
    }
}
