using Microsoft.VisualStudio.Shell;

namespace ClangPowerTools.Services
{
  public class AsyncServiceProviderWrapper<TService> where TService : class
  {
    #region Members

    /// <summary>
    /// The service provider instance
    /// </summary>
    private IAsyncServiceProvider mServiceProvider;

    #endregion


    #region Constructor 

    /// <summary>
    /// Instance constructor
    /// </summary>
    /// <param name="aAsyncServiceProvider"></param>
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
