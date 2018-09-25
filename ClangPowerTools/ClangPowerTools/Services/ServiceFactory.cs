using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClangPowerTools.Services
{
  /// <summary>
  /// Provides the custom service create operation and logic
  /// </summary>
  public class ServiceFactory
  {
    #region Members


    /// <summary>
    /// The service provider 
    /// </summary>
    private Microsoft.VisualStudio.Shell.IAsyncServiceProvider mServiceProvider;


    #endregion


    #region Constructor


    /// <summary>
    /// Instance constructor
    /// </summary>
    /// <param name="aServiceProvider">The service provider</param>
    public ServiceFactory(Microsoft.VisualStudio.Shell.IAsyncServiceProvider aServiceProvider)
      => mServiceProvider = aServiceProvider;


    #endregion


    #region Public Methods

    /// <summary>
    /// Create the wanted service
    /// </summary>
    /// <param name="container">Provides methods to add and remove services</param>
    /// <param name="cancellationToken">Propagate notification that operation should be canceled</param>
    /// <param name="serviceType">The current service type</param>
    /// <returns>The service of type serviceType</returns>
    public async Task<object> CreateService(IAsyncServiceContainer container, CancellationToken cancellationToken, Type serviceType)
    {
      return await System.Threading.Tasks.Task.Run(() =>
      {
        if (typeof(SEnvDTEService) == serviceType)
          return (object)new AsyncServiceProviderWrapper<DTE>(mServiceProvider);

        else if (typeof(SVsFileChangeService) == serviceType)
          return (object)new AsyncServiceProviderWrapper<SVsFileChangeEx>(mServiceProvider);

        else if (typeof(SVsRunningDocumentTableService) == serviceType)
          return (object)new AsyncServiceProviderWrapper<SVsRunningDocumentTable>(mServiceProvider);

        else if (typeof(SVsSolutionService) == serviceType)
          return (object)new AsyncServiceProviderWrapper<SVsSolution>(mServiceProvider);

        else if (typeof(SVsStatusBarService) == serviceType)
          return (object) new AsyncServiceProviderWrapper<SVsStatusbar>(mServiceProvider);

        else if (typeof(SVsOutputWindowService) == serviceType)
          return (object)new AsyncServiceProviderWrapper<SVsOutputWindow>(mServiceProvider);

        return null;
      });
    }

    #endregion

  }
}
