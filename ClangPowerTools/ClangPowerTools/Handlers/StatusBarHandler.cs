using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Windows.Threading;

namespace ClangPowerTools
{
  /// <summary>
  /// The Visual Studio status bar wrapper class
  /// Contains all the logic of controlling the VS status bar 
  /// </summary>
  public class StatusBarHandler
  {
    #region Members

    /// <summary>
    /// Status bar instance
    /// </summary>
    private static IVsStatusbar mStatusBar = null;

    #endregion

    #region Public Methods

    /// <summary>
    /// Create the status bar
    /// </summary>
    /// <param name="aServiceProvider">The service provider reference for getting the VS status bar</param>
    public static void Initialize(IServiceProvider aServiceProvider)
    {
      if (null == mStatusBar)
        mStatusBar = (IVsStatusbar)aServiceProvider.GetService(typeof(SVsStatusbar));
    }

    /// <summary>
    /// Set the text of the status bar
    /// </summary>
    /// <param name="aText">The text which will appear on the status bar</param>
    /// <param name="aFreezeStatusBar">True tells the environment to place a freeze on the status bar. False releases the freeze</param>
    public static void Text(string aText, int aFreezeStatusBar)
    {
      DispatcherHandler.BeginInvoke(() =>
      {
        // Make sure the status bar is not frozen
        if (VSConstants.S_OK != mStatusBar.IsFrozen(out int frozen))
          return;

        if (0 != frozen)
          mStatusBar.FreezeOutput(0);

        // Set the status bar text
        mStatusBar.SetText(aText);

        // Freeze the status bar.  
        mStatusBar.FreezeOutput(aFreezeStatusBar);

        // Clear the status bar text.
        if (0 == aFreezeStatusBar)
          mStatusBar.Clear();
      }, DispatcherPriority.Normal);
    }

    /// <summary>
    /// Set the animation of the status bar
    /// </summary>
    /// <param name="aAnimation">The animation which will appear on the status bar</param>
    /// <param name="aEnableAnimation">Set to true to turn on animation, or set to false to turn it off</param>
    public static void Animation(vsStatusAnimation aAnimation, int aEnableAnimation)
    {
      DispatcherHandler.BeginInvoke(() =>
      {
        // Use the standard Visual Studio icon for building.  
        object icon = (short)Microsoft.VisualStudio.Shell.Interop.Constants.SBAI_Build;

        // Display the icon in the Animation region.  
        mStatusBar.Animation(aEnableAnimation, ref icon);
      }, DispatcherPriority.Normal);
    }

    /// <summary>
    /// Set the status of the status bar
    /// </summary>
    /// <param name="aText">The text which will appear on the status bar</param>
    /// <param name="aFreezeStatusBar">True tells the environment to place a freeze on the status bar. False releases the freeze</param>
    /// <param name="aAnimation">The animation which will appear on the status bar</param>
    /// <param name="aEnableAnimation">Set to true to turn on animation, or set to false to turn it off</param>
    public static void Status(string aText, int aFreezeStatusBar, vsStatusAnimation aAnimation, int aEnableAnimation)
    {
      // Set the text
      Text(aText, aFreezeStatusBar);

      // Set the animation
      Animation(aAnimation, aEnableAnimation);
    }

    #endregion

  }
}
