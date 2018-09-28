using ClangPowerTools.Builder;
using ClangPowerTools.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ShellConstants = Microsoft.VisualStudio.Shell.Interop.Constants;

namespace ClangPowerTools.SilentFile
{
  public class SilentFileChangerBuilder : IAsyncBuilder<SilentFileChangerModel>
  {
    #region Members


    private SilentFileChangerModel mSilentFileChangerModel;
    private AsyncPackage mSite;


    #endregion


    #region Constructor


    /// <summary>
    /// Instance constructor
    /// </summary>
    /// <param name="aSite">Async package</param>
    /// <param name="aFileName">The file path of the file for which the changes will be ignored</param>
    /// <param name="aReloadDocument">True if the file will be reloaded. False otherwise</param>
    public SilentFileChangerBuilder(AsyncPackage aSite, string aFileName, bool aReloadDocument)
    {
      mSite = aSite;
      mSilentFileChangerModel = new SilentFileChangerModel()
      {
        DocumentFileName = aFileName,
        ReloadDocumentFlag = aReloadDocument,
        IsSuspended = true
      };
    }

    public async Task<object> AsyncBuild()
    {
      var docData = IntPtr.Zero;
      try
      {
        var rdtService = await mSite.GetServiceAsync(typeof(SVsRunningDocumentTableService)) as AsyncServiceProviderWrapper<SVsRunningDocumentTable>;
        var rdt = await rdtService.GetVsServiceAsync() as IVsRunningDocumentTable;

        if (rdt == null)
          return null;

        ErrorHandler.ThrowOnFailure(rdt.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_NoLock, mSilentFileChangerModel.DocumentFileName,
          out IVsHierarchy hierarchy, out uint itemId, out docData, out uint docCookie));

        if ((docCookie == (uint)ShellConstants.VSDOCCOOKIE_NIL) || docData == IntPtr.Zero)
          return null;

        var fileChangeService = await mSite.GetServiceAsync(typeof(SVsFileChangeService)) as AsyncServiceProviderWrapper<SVsFileChangeEx>;
        var fileChange = await fileChangeService.GetVsServiceAsync() as IVsFileChangeEx;

        if (fileChange == null)
          return null;

        ErrorHandler.ThrowOnFailure(fileChange.IgnoreFile(0, mSilentFileChangerModel.DocumentFileName, 1));
        if (docData == IntPtr.Zero)
          return null;

        var unknown = Marshal.GetObjectForIUnknown(docData);
        if (!(unknown is IVsPersistDocData))
          return null;

        mSilentFileChangerModel.PersistDocData = (IVsPersistDocData)unknown;
        if (!(mSilentFileChangerModel.PersistDocData is IVsDocDataFileChangeControl))
          return null;

        mSilentFileChangerModel.FileChangeControl = mSilentFileChangerModel.PersistDocData as IVsDocDataFileChangeControl;
      }
      catch (InvalidCastException e)
      {
        Trace.WriteLine("Exception" + e.Message); // FileChangeService
      }
      finally
      {
        if (docData != IntPtr.Zero)
          Marshal.Release(docData);
      }
      return null;
    }

    public SilentFileChangerModel GetAsyncResult() => mSilentFileChangerModel;


    #endregion


    #region IBuilder Implementation


    /// <summary>
    /// Create a new instance of silent file changer model
    /// </summary>
    //public async void Build()
    //{
    //  var docData = IntPtr.Zero;
    //  try
    //  {
    //    var rdtService = await mSite.GetServiceAsync(typeof(SVsRunningDocumentTableService)) as AsyncServiceProviderWrapper<SVsRunningDocumentTable>;
    //    var rdt = await rdtService.GetVsServiceAsync() as IVsRunningDocumentTable;

    //    if (rdt == null)
    //      return;

    //    ErrorHandler.ThrowOnFailure(rdt.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_NoLock, mSilentFileChangerModel.DocumentFileName,
    //      out IVsHierarchy hierarchy, out uint itemId, out docData, out uint docCookie));

    //    if ((docCookie == (uint)ShellConstants.VSDOCCOOKIE_NIL) || docData == IntPtr.Zero)
    //      return;

    //    var fileChangeService = await mSite.GetServiceAsync(typeof(SVsFileChangeService)) as AsyncServiceProviderWrapper<SVsFileChangeEx>;
    //    var fileChange = await fileChangeService.GetVsServiceAsync() as IVsFileChangeEx;

    //    if (fileChange == null)
    //      return;

    //    ErrorHandler.ThrowOnFailure(fileChange.IgnoreFile(0, mSilentFileChangerModel.DocumentFileName, 1));
    //    if (docData == IntPtr.Zero)
    //      return;

    //    var unknown = Marshal.GetObjectForIUnknown(docData);
    //    if (!(unknown is IVsPersistDocData))
    //      return;

    //    mSilentFileChangerModel.PersistDocData = (IVsPersistDocData)unknown;
    //    if (!(mSilentFileChangerModel.PersistDocData is IVsDocDataFileChangeControl))
    //      return;

    //    mSilentFileChangerModel.FileChangeControl = mSilentFileChangerModel.PersistDocData as IVsDocDataFileChangeControl;
    //  }
    //  catch (InvalidCastException e)
    //  {
    //    Trace.WriteLine("Exception" + e.Message); // FileChangeService
    //  }
    //  finally
    //  {
    //    if (docData != IntPtr.Zero)
    //      Marshal.Release(docData);
    //  }
    //}


    ///// <summary>
    ///// Get the silent file changer model constructed earlier
    ///// </summary>
    ///// <returns>Silent file changer model</returns>
    //public SilentFileChangerModel GetResult() => mSilentFileChangerModel;


    #endregion


  }
}
