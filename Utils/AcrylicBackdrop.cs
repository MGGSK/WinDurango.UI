using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace WinDurango.UI.Utils;

/// <summary>
/// Represents a system backdrop that applies Acrylic material to a supported XAML surface, such as a Window.
/// </summary>
public partial class AcrylicBackdrop : SystemBackdrop
{
    readonly DesktopAcrylicController m_controller;

    /// <summary>
    ///     Gets or sets a value that indicates what variant of Acrylic material is used.
    /// </summary>
    /// <returns>
    ///     A value of the enumeration that indicates what variant of Acrylic material is used.
    ///</returns>
    public DesktopAcrylicKind Kind
    {
        get => m_controller.Kind;
        set => m_controller.Kind = value;
    }

    /// <summary>
    /// Initializes a new instance of the AcrylicBackdrop class.
    /// </summary>
    public AcrylicBackdrop()
    {
        m_controller = new DesktopAcrylicController();
        m_controller.SetSystemBackdropConfiguration(new SystemBackdropConfiguration());
    }

    protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, XamlRoot xamlRoot)
    {
        base.OnTargetConnected(connectedTarget, xamlRoot);
        m_controller.AddSystemBackdropTarget(connectedTarget);
    }

    protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
    {
        base.OnTargetDisconnected(disconnectedTarget);
        m_controller.RemoveSystemBackdropTarget(disconnectedTarget);
    }
}
