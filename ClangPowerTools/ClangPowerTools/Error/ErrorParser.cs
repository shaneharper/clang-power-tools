using Microsoft.VisualStudio.Shell;
using System.Text.RegularExpressions;

namespace ClangPowerTools
{
  public class ErrorParser
  {
    #region Members

    /// <summary>
    /// The regex to detect errors, warnings and messages
    /// </summary>
    private const string kErrorMessageRegex = @"(.\:\\[ \S+\\\/.]*[c|C|h|H|cpp|CPP|cc|CC|cxx|CXX|c++|C++|cp|CP])(\r\n|\r|\n| |:)*(\d+)(\r\n|\r|\n| |:)*(\d+)(\r\n|\r|\n| |:)*(error|note|warning)[^s](\r\n|\r|\n| |:)*(?<=[:|\r\n|\r|\n| ])(.*?)(?=[\[|\r\n|\r|\n])(.*)";

    #endregion

    #region Public Methods

    /// <summary>
    /// Find the first error, warning or message from the data info using a regex
    /// </summary>
    /// <param name="aMessages">The data info which will be processed</param>
    /// <param name="aError">An error object will be created if any kind of error will be found.
    /// This object will be null if no error will be detected</param>
    /// <returns>True if a error is find. False otherwise</returns>
    public bool FindErrors(string aMessages, out TaskError aError)
    {
      // initially no kind of error is found
      aError = null;

      // Create the regex instance and try to find the matches 
      Regex regex = new Regex(kErrorMessageRegex);
      Match matchResult = regex.Match(aMessages);

      // Check if any match occurred
      if (!matchResult.Success)
        return false;

      // Get the regex match groups
      var groups = matchResult.Groups;

      // The group 9 represents the message description
      string messageDescription = groups[9].Value;

      // Check if the the error is valid
      // The description is mandatory
      if (string.IsNullOrWhiteSpace(messageDescription))
        return false;

      // The group 1 represents the file path where the error occurred
      string path = groups[1].Value;

      // The group 3 represents the line where the error occurred 
      int.TryParse(groups[3].Value, out int line);

      // The group 5 represents the column where the error occurred 
      int.TryParse(groups[5].Value, out int column);

      // The group 7 represents the category(error, warning or message) of the error
      string categoryAsString = groups[7].Value;

      // Get the object category the error
      TaskErrorCategory category = FindErrorCategory(ref categoryAsString);

      // The group 10 represents the clang-tidy checker which found the error
      string clangTidyChecker = groups[10].Value;

      // Get the full message which will be displayed in Output Window
      string fullMessage = CreateFullErrorMessage(path, line, categoryAsString, clangTidyChecker, messageDescription);

      // Add Clang info flag at the beginning of the description
      // In this way the clang errors will be easier to identify in error list
      messageDescription = messageDescription.Insert(0, ErrorParserConstants.kClangTag);

      // Create the error object
      aError = new TaskError(path, line, column, category, messageDescription, fullMessage );

      return true;
    }

    /// <summary>
    /// Find the error category
    /// </summary>
    /// <param name="aCategoryAsString">The error category as a String object</param>
    /// <returns>The error category</returns>
    private TaskErrorCategory FindErrorCategory(ref string aCategoryAsString)
    {
      TaskErrorCategory category;

      switch (aCategoryAsString)
      {
        case ErrorParserConstants.kErrorTag:
          category = TaskErrorCategory.Error;
          aCategoryAsString = ErrorParserConstants.kErrorTag;
          break;

        case ErrorParserConstants.kWarningTag:
          category = TaskErrorCategory.Warning;
          aCategoryAsString = ErrorParserConstants.kWarningTag;
          break;

        default:
          category = TaskErrorCategory.Message;
          aCategoryAsString = ErrorParserConstants.kMessageTag;
          break;
      }
      return category;
    }

    /// <summary>
    /// Concatenate all the error components in such way that the Visual Studio will know to automatically interpret it
    /// </summary>
    /// <param name="aPath">File path where the error occurred</param>
    /// <param name="aLine">The line number of code where the error occurred</param>
    /// <param name="aCategory">The category error</param>
    /// <param name="aClangTidyChecker">The clang-tidy checker which found the error</param>
    /// <param name="aDescription">The error description</param>
    /// <returns>A single string by concatenating all the parameters</returns>
    private string CreateFullErrorMessage(string aPath, int aLine, 
      string aCategory, string aClangTidyChecker, string aDescription)
    {
      return string.Format("{0}({1}): {2}{3}: {4}", aPath, aLine, aCategory,
        (true == string.IsNullOrWhiteSpace(aClangTidyChecker) ? string.Empty : $" {aClangTidyChecker.Trim(new char[] { ' ', '\n', '\r', '\t' })}"),
        aDescription);
    }

    public string Format(string aMessages, string aReplacement)
    {
      Regex regex = new Regex(kErrorMessageRegex);
      return regex.Replace(aMessages, aReplacement, 1);
    }

    /// <summary>
    /// Search the data for the wrong LLVM installation messages
    /// </summary>
    /// <param name="aMessages">The string info from the script which is going to be analized</param>
    /// <returns></returns>
    public bool LlvmIsMissing(string aMessages)
    {
      return aMessages.Contains(ErrorParserConstants.kCompileClangMissingFromPath) ||
        aMessages.Contains(ErrorParserConstants.kTidyClangMissingFromPath);
    }

    #endregion

  }
}
