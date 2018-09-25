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
          return new AsyncService<DTE>(mServiceProvider) as IBaseService<DTE>;

        else if (typeof(SVsFileChangeService) == serviceType)
          return new AsyncService<SVsFileChangeEx>(mServiceProvider) as IBaseService<SVsFileChangeEx>;

        else if (typeof(SVsRunningDocumentTableService) == serviceType)
          return new AsyncService<SVsRunningDocumentTable>(mServiceProvider) as IBaseService<SVsRunningDocumentTable>;

        else if (typeof(SVsSolutionService) == serviceType)
          return new AsyncService<SVsSolution>(mServiceProvider) as IBaseService<SVsSolution>;

        else if (typeof(SVsStatusBarService) == serviceType)
          return new AsyncService<SVsStatusbar>(mServiceProvider) as IBaseService<SVsStatusbar>;

        else if (typeof(SVsOutputWindowService) == serviceType)
          return new AsyncService<SVsOutputWindow>(mServiceProvider) as IBaseService<SVsOutputWindow>;

        return null; 

      });
      return null;
    }

    #endregion


  }
}
