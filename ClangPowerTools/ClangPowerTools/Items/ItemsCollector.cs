using EnvDTE;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.IO;

namespace ClangPowerTools
{
  /// <summary>
  /// Collect all the valid files from the selected ones
  /// </summary>
  public class ItemsCollector
  {
    #region Members

    /// <summary>
    /// The collection of the accepted extension files 
    /// </summary>
    private List<string> mAcceptedFileExtensions = new List<string>();

    /// <summary>
    /// The collection of items(SelectedProjects/SelectedProjectItems)
    /// </summary>
    private List<IItem> mItems = new List<IItem>();

    /// <summary>
    /// The service provider instance
    /// </summary>
    private IServiceProvider mServiceProvider;

    #endregion

    #region Constructor

    /// <summary>
    /// Instance constructor
    /// </summary>
    /// <param name="aServiceProvider">The service provider reference</param>
    /// <param name="aExtensions">The collection of the accepted extension files</param>
    public ItemsCollector(IServiceProvider aServiceProvider, List<string> aExtensions = null)
    {
      mServiceProvider = aServiceProvider;
      mAcceptedFileExtensions = aExtensions;
    }

    #endregion 

    #region Properties

    /// <summary>
    /// Get all the collected items
    /// </summary>
    public List<IItem> GetItems => mItems;

    /// <summary>
    /// Check if any item was collected
    /// </summary>
    public bool HaveItems => mItems.Count != 0;

    #endregion

    #region Public Methods

    /// <summary>
    /// Collect all the valid selected files from editor or solution explorer
    /// </summary>
    /// <param name="aDte">The DTE instance</param>
    /// <param name="aProjectItem"></param>
    public void CollectSelectedFiles(DTE2 aDte, ProjectItem aProjectItem)
    {
      try
      {
        // if the command has been given from tab file
        // Will be just one file selected
        if (null != aProjectItem)
        {
          AddProjectItem(aProjectItem);
          return;
        }

        // The command has been given from Solution Explorer or toolbar
        Array selectedItems = aDte.ToolWindows.SolutionExplorer.SelectedItems as Array;
        if (null == selectedItems || 0 == selectedItems.Length)
          return;

        // Collect the items
        foreach (UIHierarchyItem item in selectedItems)
        {
          if (item.Object is Solution)
            GetProjectsFromSolution(item.Object as Solution);

          else if (item.Object is Project)
            AddProject(item.Object as Project);

          else if (item.Object is ProjectItem)
            GetProjectItem(item.Object as ProjectItem);
        }
      }
      catch (Exception)
      {
      }

    }

    /// <summary>
    /// Store the project item
    /// </summary>
    /// <param name="aItem">The selected project item which will be stored</param>
    public void AddProjectItem(ProjectItem aItem)
    {
      // Get the extension file
      var fileExtension = Path.GetExtension(aItem.Name).ToLower();

      // Check if the extension file is valid
      if ( null != mAcceptedFileExtensions && false == mAcceptedFileExtensions.Contains(fileExtension))
        return;

      // Store the project item
      mItems.Add(new SelectedProjectItem(aItem));
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Collect all the projects inside of the specified solution
    /// </summary>
    /// <param name="aSolution">The solution object</param>
    private void GetProjectsFromSolution(Solution aSolution)
    {
      mItems = AutomationUtil.GetAllProjects(mServiceProvider, aSolution);
    }

    /// <summary>
    /// Collect the specified project 
    /// </summary>
    /// <param name="aProject">The project object</param>
    private void AddProject(Project aProject) => mItems.Add(new SelectedProject(aProject));

    /// <summary>
    /// Collect the specified project item
    /// </summary>
    /// <param name="aProjectItem">The project item object</param>
    private void GetProjectItem(ProjectItem aProjectItem)
    {
      // Project item contains projects
      if (null == aProjectItem.ProjectItems)
      {
        if (null != aProjectItem.SubProject)
          AddProject(aProjectItem.SubProject);
        return;
      }
      // Project item represents a folder or filter
      else if (0 != aProjectItem.ProjectItems.Count)
      {
        foreach (ProjectItem projItem in aProjectItem.ProjectItems)
          GetProjectItem(projItem);
      }
      // Project item represents a files
      else
      {
        AddProjectItem(aProjectItem);
      }
    }

    #endregion

  }
}
