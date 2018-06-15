using System.IO;

namespace ClangPowerTools
{
  /// <summary>
  /// Detect when a file inside  a given directory is changed
  /// </summary>
  public class FileChangerWatcher
  {
    #region Members

    /// <summary>
    /// File watcher object
    /// </summary>
    FileSystemWatcher mWatcher = new FileSystemWatcher();

    #endregion

    #region Properties

    /// <summary>
    /// File watcher handler
    /// </summary>
    public FileSystemEventHandler OnChanged { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Watch all the files inside a directory for changes
    /// </summary>
    /// <param name="aDirectoryPath"></param>
    public void Run(string aDirectoryPath)
    {
      // Check if the path exists
      if (null == aDirectoryPath || string.IsNullOrWhiteSpace(aDirectoryPath))
        return;

      // Set the path property of FileSystemWatcher
      mWatcher.Path = aDirectoryPath;

      // Watch for changes in LastWrite time
      mWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName;

      // Only watch .cpp files.

      foreach (var extension in ScriptConstants.kAcceptedFileExtensions)
        mWatcher.Filter = $"*{extension}";

      //Subdirectories will be also watched.
      mWatcher.IncludeSubdirectories = true;

      // Watch every file in the directory for changes
      mWatcher.Changed += OnChanged;
      mWatcher.Deleted += OnChanged;

      // Begin watching.
      mWatcher.EnableRaisingEvents = true;
    }

    #endregion

  }
}
