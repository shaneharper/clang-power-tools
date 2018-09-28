using ClangPowerTools.Builder;
using ClangPowerTools.Services;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ClangPowerTools.SilentFile
{
  /// <summary>
  /// Prevent visual studio to ask you if you want to reload the files
  /// </summary>
  public class SilentFileChangerController : IDisposable
  {
    #region Members


    /// <summary>
    /// Collection of all silent file changer models necessary when tidy-fix is executed
    /// </summary>
    private HashSet<SilentFileChangerModel> mSilentFileChangers =
          new HashSet<SilentFileChangerModel>(new SilentFileChangerEqualityComparer());

    /// <summary>
    /// The async package instance
    /// </summary>
    private AsyncPackage mAsyncPackage;


    #endregion


    #region Constructor 

    /// <summary>
    /// Instance Constructor
    /// </summary>
    /// <param name="aAsyncPackage"></param>
    public SilentFileChangerController(AsyncPackage aAsyncPackage) => mAsyncPackage = aAsyncPackage;


    #endregion


    #region Methods


    #region Public methods



    public async System.Threading.Tasks.Task<object> Add(IEnumerable<string> aFilesPath)
    {
      return await System.Threading.Tasks.Task.Run(async () =>
      {
        foreach (var filePath in aFilesPath)
        {
          var silentFile = await GetNewSilentFileChanger(filePath);
          mSilentFileChangers.Add(silentFile);
        }
        return new object();
      });
    }


    public async System.Threading.Tasks.Task<object> Add(Documents aDocuments)
    {
      return await System.Threading.Tasks.Task.Run(async () =>
      {
        foreach (Document doc in aDocuments)
        {
          var silentFile = await GetNewSilentFileChanger(Path.Combine(doc.Path, doc.Name));
          mSilentFileChangers.Add(silentFile);
        }
        return new object();
      });
    }


    public async System.Threading.Tasks.Task<object> SilentFiles()
    {
      return await System.Threading.Tasks.Task.Run(() =>
      {
        foreach (var silentFile in mSilentFileChangers)
          Silent(silentFile);
        return new object();
      });
    }


    /// <summary>
    /// Silent all files from a IEnumerable data collection
    /// </summary>
    /// <param name="aServiceProvider">Async package instance</param>
    /// <param name="aFilesPath">Files path collection for the files for which the changes will be ignored</param>
    //public void SilentFiles(IEnumerable<string> aFilesPath)
    //{
    //  foreach (var filePath in aFilesPath)
    //  {
    //    var silentFile = GetNewSilentFileChanger(filePath);
    //    mSilentFileChangers.Add(silentFile);
    //    Silent(silentFile);
    //  }
    //}


    /// <summary>
    /// Silent all open files extracted from a DTE documents data collection
    /// </summary>
    /// <param name="aServiceProvider">Async package instance</param>
    /// <param name="aDte">DTE instance to collect all the documents </param>
    //public void SilentFiles(Documents aDocuments)
    //{
    //  foreach (Document doc in aDocuments)
    //  {
    //    var silentFile = GetNewSilentFileChanger(Path.Combine(doc.Path, doc.Name));
    //    mSilentFileChangers.Add(silentFile);
    //    Silent(silentFile);
    //  }
    //}


    #region IDisposable Implementation

    /// <summary>
    /// Stop ignoring the file changes for all stored files
    /// </summary>
    public void Dispose()
    {
      foreach (var silentFileChanger in mSilentFileChangers)
        Resume(silentFileChanger);
    }


    #endregion


    #endregion


    #region Private Methods


    /// <summary>
    /// Create a new silent file changer model object
    /// </summary>
    /// <param name="aServiceProvider">Async package instance</param>
    /// <param name="aFilePath">The file path of the file for which the changes will be ignored</param>
    private async Task<SilentFileChangerModel> GetNewSilentFileChanger(string aFilePath)
    {
      return await System.Threading.Tasks.Task.Run(async () =>
      {
        IAsyncBuilder<SilentFileChangerModel> silentFileChangerBuilder = new SilentFileChangerBuilder(mAsyncPackage, aFilePath, true);
        await silentFileChangerBuilder.AsyncBuild();
        var silentFileModel = silentFileChangerBuilder.GetAsyncResult();
        return silentFileModel;
      });
    }


    /// <summary>
    /// Ignore all file changes of a file
    /// </summary>
    /// <param name="aSilentFileChanger"></param>
    private void Silent(SilentFileChangerModel aSilentFileChanger)
    {
      if (null != aSilentFileChanger.FileChangeControl)
        ErrorHandler.ThrowOnFailure(aSilentFileChanger.FileChangeControl.IgnoreFileChanges(1));
    }


    /// <summary>
    /// Stop ignoring the file changes for a certain files
    /// </summary>
    public async void Resume(SilentFileChangerModel aSilentFileChanger)
    {
      if (null == aSilentFileChanger)
        return;

      if (!aSilentFileChanger.IsSuspended || null == aSilentFileChanger.PersistDocData)
        return;

      if (null != aSilentFileChanger.PersistDocData && aSilentFileChanger.ReloadDocumentFlag)
        aSilentFileChanger.PersistDocData.ReloadDocData(0);

      var fileChangeService = await mAsyncPackage.GetServiceAsync(typeof(SVsFileChangeService)) as AsyncServiceProviderWrapper<SVsFileChangeEx>;
      var fileChange = await fileChangeService.GetVsServiceAsync() as IVsFileChangeEx;

      if (fileChange == null)
        return;

      aSilentFileChanger.IsSuspended = false;
      ErrorHandler.ThrowOnFailure(fileChange.SyncFile(aSilentFileChanger.DocumentFileName));
      ErrorHandler.ThrowOnFailure(fileChange.IgnoreFile(0, aSilentFileChanger.DocumentFileName, 0));
      if (aSilentFileChanger.FileChangeControl != null)
        ErrorHandler.ThrowOnFailure(aSilentFileChanger.FileChangeControl.IgnoreFileChanges(0));
    }


    #endregion


    #endregion

  }
}
