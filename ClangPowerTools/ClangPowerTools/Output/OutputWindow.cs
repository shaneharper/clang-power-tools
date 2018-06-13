using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using System.Text;

namespace ClangPowerTools
{
  /// <summary>
  /// Contains the all the necessary logic to manipulate the Visual Studio Output Window
  /// </summary>
  public class OutputWindow : TextWriter
  {
    #region Members

    /// <summary>
    /// Guid of the Output Window
    /// </summary>
    private static readonly Guid mPaneGuid = new Guid("AB9F45E4-2001-4197-BAF5-4B165222AF29");

    /// <summary>
    /// Output Window instance
    /// </summary>
    private static IVsOutputWindow mOutputWindow = null;

    /// <summary>
    /// The output pane which will be contained of Output Window
    /// </summary>
    private static IVsOutputWindowPane mOutputPane = null;

    #endregion

    #region Constructor

    /// <summary>
    /// Instance constructor
    /// </summary>
    /// <param name="aDte">The DTE is required to get the Service Provider reference </param>
    public OutputWindow(DTE2 aDte)
    {
      // If the Output Window instance dose not exists then create one 
      if( null == mOutputWindow )
      {
        // Get Service Provider using the DTE
        IServiceProvider serviceProvider = 
          new ServiceProvider(aDte as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);

        // Create the Output Window
        mOutputWindow = serviceProvider.GetService(typeof(SVsOutputWindow)) as IVsOutputWindow;
      }

      // If the pane of the Output Window dose not exists then create one
      if (null == mOutputPane)
      {
        // Get the pane
        Guid generalPaneGuid = mPaneGuid;
        mOutputWindow.GetPane(ref generalPaneGuid, out IVsOutputWindowPane pane);
        
        // If the pane dose not exist then create it manually
        if ( null == pane)
        {
          mOutputWindow.CreatePane(ref generalPaneGuid, OutputWindowConstants.kPaneName, 0, 1);
          mOutputWindow.GetPane(ref generalPaneGuid, out pane);
        }
        mOutputPane = pane;
      }
    }

    #endregion


    #region Properties

    /// <summary>
    /// Gets an encoding for the operating system's current ANSI code page
    /// </summary>
    public override Encoding Encoding => System.Text.Encoding.Default;

    #endregion


    #region Public Methods

    /// <summary>
    /// Write a string in the Output Window
    /// </summary>
    /// <param name="aMessage">The string which is going to be written</param>
    public override void Write(string aMessage) => mOutputPane.OutputString($"{aMessage}\n");

    /// <summary>
    /// Write just a single character in the Output Window
    /// </summary>
    /// <param name="aCharacter">The character which is going to be written</param>
    public override void Write(char aCharacter) => mOutputPane.OutputString(aCharacter.ToString());


    /// <summary>
    /// Show the Output Window in Visual Studio
    /// </summary>
    /// <param name="aDte"></param>
    public void Show(DTE2 aDte)
    {
      mOutputPane.Activate();
      aDte.ExecuteCommand("View.Output", string.Empty);
    }

    /// <summary>
    /// Clear all the text from the Output Window
    /// </summary>
    public void Clear() => mOutputPane.Clear();

    #endregion
  }
}
