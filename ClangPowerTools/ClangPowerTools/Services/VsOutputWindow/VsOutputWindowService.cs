﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;

namespace ClangPowerTools.Services
{
  public class VsOutputWindowService : SVsOutputWindowService, IVsOutputWindowService, IService
  {
    #region Members

    private Microsoft.VisualStudio.Shell.IAsyncServiceProvider mServiceProvider;

    #endregion


    #region Constructor 

    public VsOutputWindowService(Microsoft.VisualStudio.Shell.IAsyncServiceProvider aAsyncServiceProvider)
      => mServiceProvider = aAsyncServiceProvider;

    #endregion


    #region IEnvDTEService implementation

    public async Task<IVsOutputWindow> GetOutputWindowAsync(CancellationToken cancellationToken)
    {
      return await mServiceProvider.GetServiceAsync(typeof(SVsOutputWindow)) as IVsOutputWindow;
    }

    #endregion

  }
}
