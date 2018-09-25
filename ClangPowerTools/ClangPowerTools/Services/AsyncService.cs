using Microsoft.VisualStudio.Shell;

namespace ClangPowerTools.Services
{
  public class AsyncService<TService> : IBaseService<TService>
  {
    #region Properties

    public TService GetService
    {
      get;
      private set;
    }

    #endregion


    #region Constructor

    public AsyncService(IAsyncServiceProvider aAsyncServiceProvider)
    {
      Initialize(aAsyncServiceProvider);
    }

    #endregion


    #region Private Methods

    private async void Initialize(IAsyncServiceProvider aAsyncServiceProvider)
    {
      GetService = (TService)await aAsyncServiceProvider.GetServiceAsync(typeof(TService));
    }

    #endregion

  }
}
