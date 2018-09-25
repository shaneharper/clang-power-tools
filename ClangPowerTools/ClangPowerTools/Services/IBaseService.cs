namespace ClangPowerTools.Services
{
  public interface IBaseService<TService>
  {
    TService GetService{ get; }
  }
}
