using Microsoft.VisualStudio.Shell;
using System.Threading;

namespace ClangPowerTools.Services
{
  public class AsyncServiceProviderWrapper<TService> where TService : class
  {
    #region Members

    private IAsyncServiceProvider mServiceProvider;

    #endregion


    #region Constructor 

    public AsyncServiceProviderWrapper(IAsyncServiceProvider aAsyncServiceProvider)
      => mServiceProvider = aAsyncServiceProvider;

    #endregion


    #region IEnvDTEService implementation

    public async System.Threading.Tasks.Task<TService> GetServiceAsync()
    {
      
      return (TService)await mServiceProvider.GetServiceAsync(typeof(TService));
    }

    #endregion

  }
}
