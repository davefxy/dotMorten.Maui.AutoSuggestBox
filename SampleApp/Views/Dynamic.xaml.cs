﻿using Maui.AutoSuggestBox;
using Microsoft.Maui.Controls.Platform;

namespace SampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [System.ComponentModel.Description("Dynamic data lookup")]
    [SamplePriority(2)]
    public partial class Dynamic : ContentPage
	{
        public Dynamic()
		{
			InitializeComponent();
		}

        private async void SuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestBox box = (AutoSuggestBox)sender;
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrWhiteSpace(box.Text) || box.Text.Length < 3)
                    box.ItemsSource = null;
                else
                {
                    var suggestions = await GetSuggestions(box.Text);
                    box.ItemsSource = suggestions.ToList();
                }
            }
        }

        public class City
        {
            public string Name { get; set; }
            public string State { get; set; }
            public string DisplayName => $"{Name}, {State}";
            public string FullDisplayName => $"{Name}, {State}, USA";
            public override string ToString() => FullDisplayName;
        }

        /// <summary>
        /// Simulates querying a server for a large list of data
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private async Task<IEnumerable<City>> GetSuggestions(string text)
        {
            var result = await Task.Run<IEnumerable<City>>(() =>
            {
                List<City> suggestions = new List<City>();
                using (var s = typeof(Dynamic).Assembly.GetManifestResourceStream("SampleApp.Data.USCities.txt"))
                {
                    using (var sr = new StreamReader(s))
                    {
                        while (!sr.EndOfStream && suggestions.Count < 20)
                        {
                            var data = sr.ReadLine().Split('\t');
                            var city = new City() { Name = data[0], State = data[1] };
                            if (city.FullDisplayName.StartsWith(text, StringComparison.InvariantCultureIgnoreCase))
                            {
                                suggestions.Add(city);
                            }
                        }
                    }
                }
                return suggestions;
            });
            await Task.Delay(1000); //Simulate slow web service response
            return result;
        }

        private void SuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            if(e.ChosenSuggestion == null)
                status.Text = "Query submitted: " + e.QueryText;
            else
                status.Text = "Suggestion chosen: " + e.ChosenSuggestion;
        }
    }
}
