namespace ClangPowerTools
{
  /// <summary>
  /// The selected items interface
  /// </summary>
  public interface IItem
  {
    string GetName();
    string GetPath();
    object GetObject();
    void Save();
  }
}
