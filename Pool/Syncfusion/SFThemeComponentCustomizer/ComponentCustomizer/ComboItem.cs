using System.Windows.Controls;

namespace ComponentCustomizer;

public class ComboItem
{
    public ComboItem(UserControl control)
    {
        Control = control;
    }

    public UserControl Control { get; }

    public override string ToString()
    {
        return Control.ToString() ?? throw new InvalidOperationException($"Control.ToString() returned null for control type '{Control?.GetType().Name ?? "null"}'.");
    }
}