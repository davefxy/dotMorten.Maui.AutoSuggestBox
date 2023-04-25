﻿using AutoSuggestBox.Handlers;

namespace AutoSuggestBox;

/// <summary>
/// This class contains CustomSwitch <see cref="MauiAppBuilder"/> extensions.
/// </summary>
public static class AppBuilderExtensions
{
    /// <summary>
    /// Initializes the Switch control
    /// </summary>
    /// <param name="builder"><see cref="MauiAppBuilder"/> generated by <see cref="MauiApp"/>.</param>
    /// <returns><see cref="MauiAppBuilder"/> initialized for <see cref="AutoSuggestBox"/>.</returns>
    public static MauiAppBuilder UseAutoSuggestBox(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(h => h.AddHandler<IAutoSuggestBox, AutoSuggestBoxHandler>());

        return builder;
    }
}