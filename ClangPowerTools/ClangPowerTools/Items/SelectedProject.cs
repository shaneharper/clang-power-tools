using EnvDTE;

namespace ClangPowerTools
{
  /// <summary>
  /// Wrapper class for the project object
  /// Contains all the logic for getting the project info and save the project 
  /// </summary>
  public class SelectedProject : IItem
  {
    #region Members

    /// <summary>
    /// The project instance
    /// </summary>
    private Project mProject;

    #endregion

    #region Constructor

    /// <summary>
    /// Instance constructor
    /// </summary>
    /// <param name="aProject">The project object</param>
    public SelectedProject(Project aProject) => mProject = aProject;

    #endregion

    #region IItem implementation

    /// <summary>
    /// Get the project name
    /// </summary>
    /// <returns>Project name</returns>
    public string GetName() => mProject.FullName.Substring(mProject.FullName.LastIndexOf('\\') + 1);

    /// <summary>
    /// Get the project path
    /// </summary>
    /// <returns>Project path</returns>
    public string GetPath() => mProject.FullName;

    /// <summary>
    /// Get the project object
    /// </summary>
    /// <returns>Project object</returns>
    public object GetObject() => mProject;

    /// <summary>
    /// Save the project
    /// </summary>
    public void Save() => mProject.Save(mProject.FullName);

    #endregion

  }
}
