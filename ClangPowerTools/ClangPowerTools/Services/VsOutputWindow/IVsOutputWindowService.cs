using Microsoft.VisualStudio.Shell.Interop;
using System.Threading;

namespace ClangPowerTools.Services
{
  public interface IVsOutputWindowService
  {
    System.Threading.Tasks.Task<IVsOutputWindow> GetOutputWindowAsync();
  }
}
