﻿namespace SampleApp;
public class SamplePriorityAttribute : Attribute
{
    public SamplePriorityAttribute(int priority = 1) { Priority = priority; }
    public int Priority { get; set; }
}

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
        LoadSamples();
    }

    private class SampleInfo
    {
        public SampleInfo(Type type)
        {
            PageType = type;
        }
        public Type PageType { get; }
        public string DisplayName
        {
            get
            {
                var desc = PageType.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false).FirstOrDefault() as System.ComponentModel.DescriptionAttribute;
                if (desc != null)
                    return desc.Description;
                return PageType.Name;
            }
        }
        public int Priority
        {
            get
            {
                var desc = PageType.GetCustomAttributes(typeof(SamplePriorityAttribute), false).FirstOrDefault() as SamplePriorityAttribute;
                if (desc != null)
                    return desc.Priority;
                return int.MaxValue;
            }
        }

        public string Category
        {
            get
            {
                var name = PageType.Namespace.Substring(PageType.Namespace.LastIndexOf('.') + 1);
                if (name.EndsWith("Samples"))
                    name = name.Substring(0, name.Length - 7);
                return name;
            }
        }
    }
    private void LoadSamples()
    {
        var samples = typeof(MainPage).Assembly.GetExportedTypes()
                        .Where(t => t.IsSubclassOf(typeof(Page)) && t.Namespace.Contains("Views"))
                        .Select(t => new SampleInfo(t));

        foreach (var group in samples.GroupBy(t => t.Category))
        {
            Label header = new Label() { Text = group.Key, FontSize = 24, TextColor = Colors.White, Margin = new Thickness(0, 10) };
            samplelist.Children.Add(header);
            foreach (var sample in group.OrderBy(t => t.Priority))
            {
                Button b = new Button() { Text = sample.DisplayName, TextColor = Colors.White, HorizontalOptions = LayoutOptions.Fill, BackgroundColor = Colors.Black };
                var t = sample.PageType;
                b.Clicked += (s, e) => Navigation.PushAsync(Activator.CreateInstance(t) as Page);
                samplelist.Children.Add(b);
            }
        }
    }
}

