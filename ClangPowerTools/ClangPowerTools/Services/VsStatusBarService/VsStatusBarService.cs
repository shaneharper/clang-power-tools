﻿//using System.Threading;
//using Microsoft.VisualStudio.Shell.Interop;

//namespace ClangPowerTools.Services
//{
//  public class VsStatusBarService : SVsStatusBarService, IVsStatusBarService, IService
//  {
//    #region Members

//    private Microsoft.VisualStudio.Shell.IAsyncServiceProvider mServiceProvider;

//    #endregion


//    #region Constructor 

//    public VsStatusBarService() { }


//    public VsStatusBarService(Microsoft.VisualStudio.Shell.IAsyncServiceProvider aAsyncServiceProvider)
//      => mServiceProvider = aAsyncServiceProvider;

//    #endregion


//    #region IEnvDTEService implementation

//    public async System.Threading.Tasks.Task<IVsStatusbar> GetVsStatusBarAsync()
//    {
//      return await mServiceProvider.GetServiceAsync(typeof(SVsStatusbar)) as IVsStatusbar;
//    }

//    #endregion

//  }
//}
