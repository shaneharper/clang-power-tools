using EnvDTE;

namespace ClangPowerTools
{
  /// <summary>
  /// Helper class for getting the project configuration
  /// </summary>
  public class ProjectConfigurationHandler
  {
    #region Public Methods

    /// <summary>
    /// Get the active platform configuration
    /// </summary>
    /// <param name="aProject">The project object from where the platform configuration will be extracted</param>
    /// <returns>Active platform</returns>
    public static string GetPlatform(Project aProject)
    {
      var succes = GetActiveConfiguration(aProject, out Configuration configuration);
      if (false == succes)
        return string.Empty;

      return configuration.PlatformName;
    }

    /// <summary>
    /// Get the active configuration
    /// </summary>
    /// <param name="aProject">The project object from where the configuration will be extracted</param>
    /// <returns>Active configuration</returns>
    public static string GetConfiguration(Project aProject)
    {
      var succes = GetActiveConfiguration(aProject, out Configuration configuration);
      if (false == succes)
        return string.Empty;

      return configuration.ConfigurationName;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Get the active configuration from a project
    /// </summary>
    /// <param name="aProject">The project which will be analized</param>
    /// <param name="aConfiguration">The founded configuration</param>
    /// <returns>True if the configuration was found. False otherwise</returns>
    private static bool GetActiveConfiguration(Project aProject, out Configuration aConfiguration)
    {
      aConfiguration = null;

      var configurationManager = aProject.ConfigurationManager;
      if (null == configurationManager)
        return false;

      aConfiguration = configurationManager.ActiveConfiguration;
      if (null == aConfiguration)
        return false;

      return true;
    }

    #endregion



  }
}
