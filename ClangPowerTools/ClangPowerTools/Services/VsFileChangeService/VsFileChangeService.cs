using System.Threading;
using Microsoft.VisualStudio.Shell.Interop;

namespace ClangPowerTools.Services
{
  public class VsFileChangeService : SVsFileChangeService, IVsFileChangeService, IService
  {
    #region Members

    private Microsoft.VisualStudio.Shell.IAsyncServiceProvider mServiceProvider;

    #endregion


    #region Constructor 

    public VsFileChangeService(Microsoft.VisualStudio.Shell.IAsyncServiceProvider aAsyncServiceProvider)
      => mServiceProvider = aAsyncServiceProvider;

    #endregion


    #region IEnvDTEService implementation

    public async System.Threading.Tasks.Task<IVsFileChangeEx> GetVsFileChangeAsync(CancellationToken cancellationToken)
    {
      return await mServiceProvider.GetServiceAsync(typeof(SVsFileChangeEx)) as IVsFileChangeEx;
    }

    #endregion

  }
}
