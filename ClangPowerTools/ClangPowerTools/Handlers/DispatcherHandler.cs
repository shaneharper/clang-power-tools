using EnvDTE80;
using System;
using System.Windows.Interop;
using System.Windows.Threading;

namespace ClangPowerTools
{
  /// <summary>
  /// Dispatcher wrapper class
  /// Contains all the logic of controlling and Invoke/BeginInvoke an action using a dispatcher 
  /// </summary>
  public class DispatcherHandler
  {

    #region Members

    /// <summary>
    /// The dispatcher object
    /// </summary>
    private static Dispatcher mDispatcher = null;

    #endregion

    #region Public Methods

    /// <summary>
    /// Create the dispatcher object
    /// </summary>
    /// <param name="aDte"></param>
    public static void Initialize(DTE2 aDte)
    {
      mDispatcher = HwndSource.FromHwnd((IntPtr)aDte.MainWindow.HWnd).RootVisual.Dispatcher;
    }

    
    /// <summary>
    /// Execute the specified action asynchronously at the specified priority(Normal) on the thread on which the Dispatcher is associated with.
    /// </summary>
    /// <param name="aAction">The action which will be invoke</param>
    public static void BeginInvoke(Action aAction, DispatcherPriority aPrioriry)
    {
      mDispatcher.BeginInvoke(aPrioriry, new Action(() =>
      {
        aAction.BeginInvoke(aAction.EndInvoke, null);
      }));
    }

    
    /// <summary>
    /// Execute the specified action synchronously at the specified priority(Normal) on the thread on which the Dispatcher is associated with.
    /// </summary>
    /// <param name="aAction">The action which will be invoke</param>
    public static void Invoke(Action aAction, DispatcherPriority aPrioriry)
    {
      mDispatcher.Invoke(aPrioriry, new Action(() =>
      {
        aAction.Invoke();
      }));
    }

    #endregion

  }
}
