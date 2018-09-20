using Microsoft.VisualStudio.Shell.Interop;
using System.Threading;

namespace ClangPowerTools.Services
{
  public interface IVsStatusBarService
  {
    System.Threading.Tasks.Task<IVsStatusbar> GetVsStatusBarAsync();
  }
}
