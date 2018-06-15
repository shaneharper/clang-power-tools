using EnvDTE80;
using System.IO;

namespace ClangPowerTools
{
  /// <summary>
  /// Open a file in the Visual Studio Editor
  /// </summary>
  public class FileOpener
  {
    #region Members

    /// <summary>
    /// The name of the open file in editor command 
    /// </summary>
    private string kOpenCommand = "File.OpenFile";

    /// <summary>
    /// DTE reference used to execute the command
    /// </summary>
    private DTE2 mDte;

    #endregion


    #region Constructor

    /// <summary>
    /// Instance constructor
    /// </summary>
    /// <param name="aDte"></param>
    public FileOpener(DTE2 aDte) => mDte = aDte;

    #endregion


    #region Public methods

    /// <summary>
    /// Open the changed files in the Visual Studio Editor
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e">Argument event from where can be access de path to the changed file</param>
    public void FileChanged(object source, FileSystemEventArgs e) => 
      mDte.ExecuteCommand(kOpenCommand, e.FullPath);

    #endregion

  }
}
