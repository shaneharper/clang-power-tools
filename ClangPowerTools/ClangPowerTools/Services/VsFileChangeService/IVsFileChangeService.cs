using Microsoft.VisualStudio.Shell.Interop;
using System.Threading;

namespace ClangPowerTools.Services
{
  public interface IVsFileChangeService
  {
    System.Threading.Tasks.Task<IVsFileChangeEx> GetVsFileChangeAsync(CancellationToken cancellationToken);
  }
}
