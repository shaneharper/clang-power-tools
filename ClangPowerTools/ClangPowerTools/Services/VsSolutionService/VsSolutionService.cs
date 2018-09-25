//using System.Threading;
//using Microsoft.VisualStudio.Shell.Interop;

//namespace ClangPowerTools.Services
//{
//  public class VsSolutionService : SVsSolutionService, IVsSolutionService, IService
//  {
//    #region Members

//    private Microsoft.VisualStudio.Shell.IAsyncServiceProvider mServiceProvider;

//    #endregion


//    #region Constructor 

//    public VsSolutionService() { }


//    public VsSolutionService(Microsoft.VisualStudio.Shell.IAsyncServiceProvider aAsyncServiceProvider)
//      => mServiceProvider = aAsyncServiceProvider;

//    #endregion


//    #region IEnvDTEService implementation

//    public async System.Threading.Tasks.Task<IVsSolution> GetVsSolutionAsync()
//    {
//      return await mServiceProvider.GetServiceAsync(typeof(SVsSolution)) as IVsSolution;
//    }

//    #endregion

//  }
//}
