//using EnvDTE;
//using EnvDTE80;
//using Microsoft.VisualStudio.Shell;
//using System.Threading.Tasks;

//namespace ClangPowerTools.Services
//{
//  public class EnvDTEService : SEnvDTEService, IBaseService<DTE2>
//  {
//    #region Properties

//    public DTE2 DteService { get; private set; }

//    #endregion


//    #region Public Methods

//    public async void Initialize(IAsyncServiceProvider aAsyncServiceProvider)
//    {
//      DteService = await aAsyncServiceProvider.GetServiceAsync(typeof(DTE)) as DTE2;
//    }



//    #region IEnvDTEService implementation

//    public DTE2 GetService
//    {
//      get => DteService; 
//      private set => throw new System.NotImplementedException();
//    }



//    public DTE2 GetService<DTE2>()
//    {
//      return DteService;
//    }

//    #endregion

//    #endregion

//  }
//}
