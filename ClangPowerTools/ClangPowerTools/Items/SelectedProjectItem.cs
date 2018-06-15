using EnvDTE;

namespace ClangPowerTools
{
  /// <summary>
  /// Wrapper class for the project item object
  /// Contains all the logic for getting the project item info and save the item
  /// </summary>
  public class SelectedProjectItem : IItem
  {
    #region Members

    /// <summary>
    /// The Project item instance
    /// </summary>
    private ProjectItem mProjectItem;

    #endregion

    #region Constructor

    /// <summary>
    /// Instance constructor
    /// </summary>
    /// <param name="aProjectItem">The project item instance</param>
    public SelectedProjectItem(ProjectItem aProjectItem) => mProjectItem = aProjectItem;

    #endregion

    #region IItem implementation

    /// <summary>
    /// Get the project item name
    /// </summary>
    /// <returns>Project item name</returns>
    public string GetName() => mProjectItem.Name;

    /// <summary>
    /// Get the project item path
    /// </summary>
    /// <returns>Project item path</returns>
    public string GetPath() => mProjectItem.Properties.Item("FullPath").Value.ToString();

    /// <summary>
    /// Get the project item object
    /// </summary>
    /// <returns>Project item object</returns>
    public object GetObject() => mProjectItem;

    /// <summary>
    /// Save the item
    /// </summary>
    public void Save() => mProjectItem.Save("");

    #endregion

  }
}
