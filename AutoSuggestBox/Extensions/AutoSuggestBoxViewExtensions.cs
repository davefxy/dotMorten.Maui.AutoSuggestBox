using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSuggestBox.Extensions;


/// <summary>
/// Extension methods to support <see cref="AutoSuggestBox"/>
/// </summary>
public static class AutoSuggestBoxViewExtensions
{
    /// <summary>
    /// Set LineWidth
    /// </summary>
    /// <param name="autosuggestBoxView"><see cref="IAutoSuggestBox"/></param>
    /// <param name="textLength">line width</param>
    public static void SetLineWidth(this IAutoSuggestBox autosuggestBoxView, int textLength)
    {
        //autosuggestBoxView.LineWidth = textLength;
    }
}
