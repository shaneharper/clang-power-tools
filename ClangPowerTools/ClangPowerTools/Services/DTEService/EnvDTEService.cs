using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace ClangPowerTools.Services
{
  public class EnvDTEService<T> : SEnvDTEService, IEnvDTEService<T>, IService where T : DTE
  {
    #region Members

    private IAsyncServiceProvider mServiceProvider;

    #endregion


    #region Constructor 

    public EnvDTEService(IAsyncServiceProvider aAsyncServiceProvider)
      => mServiceProvider = aAsyncServiceProvider;

    #endregion


    #region IEnvDTEService implementation


    //public async Task<T> GetServiceAsync()
    //{
    //  return await mServiceProvider.GetServiceAsync(typeof(T));
    //}

    public async Task<T> GetServiceAsync()
    {
      return await mServiceProvider.GetServiceAsync(T.GetType());
    }

    #endregion

  }
}
