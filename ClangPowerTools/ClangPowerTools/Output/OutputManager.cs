using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Threading;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;

namespace ClangPowerTools
{
  /// <summary>
  /// Manage all the Output Window logic, actions and data
  /// </summary>
  public class OutputManager
  {
    #region Members

    /// <summary>
    /// Instance of DTE
    /// It is used to show the Output view
    /// </summary>
    private DTE2 mDte = null;

    /// <summary>
    /// Output Window instance 
    /// </summary>
    private OutputWindow mOutputWindow = null;

    /// <summary>
    /// The maximum number of output lines that can be stored in the buffer
    /// </summary>
    private int kBufferSize = 5;

    /// <summary>
    /// The buffer which stores the output lines
    /// Each element represents a line from the output script
    /// </summary>
    private List<string> mMessagesBuffer = new List<string>();

    /// <summary>
    /// Instance of the class which is responsible with the errors, warnings and messages detection
    /// </summary>
    private ErrorParser mErrorParser = new ErrorParser();

    /// <summary>
    /// The LLVM wrong installation flag
    /// </summary>
    private bool mMissingLlvm = false;

    /// <summary>
    /// The list of the found errors, warnings and messages
    /// </summary>
    private HashSet<TaskError> mErrors = new HashSet<TaskError>();

    /// <summary>
    /// The list of the all PCH path that are created by the power shell script
    /// All of this must be deleted from the disk when the Stop Clang command is given
    /// </summary>
    private List<string> mPCHPaths = new List<string>();

    #endregion


    #region Properties

    /// <summary>
    /// The LLVM wrong installation getter property
    /// </summary>
    public bool MissingLlvm => mMissingLlvm;

    /// <summary>
    /// Get the buffer of output messages from the script
    /// </summary>
    public List<string> Buffer => mMessagesBuffer;

    /// <summary>
    /// Check if the buffer is empty
    /// </summary>
    public bool EmptyBuffer => mMessagesBuffer.Count == 0;

    /// <summary>
    /// Get the detected errors, warnings and messages 
    /// </summary>
    public HashSet<TaskError> Errors => mErrors;

    /// <summary>
    /// Check if any kind of error message was found
    /// </summary>
    public bool HasErrors => 0 != mErrors.Count;

    /// <summary>
    /// Get all the detected PCH paths
    /// </summary>
    public List<string> PCHPaths => mPCHPaths;

    /// <summary>
    /// The hierarchy element of the selected project or file
    /// </summary>
    public IVsHierarchy Hierarchy { get; set; }
    
    #endregion


    #region Constructor

    /// <summary>
    /// Instance constructor
    /// </summary>
    /// <param name="aDte"></param>
    public OutputManager(DTE2 aDte)
    {
      mDte = aDte;
      mOutputWindow = new OutputWindow(mDte);
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Clear the Output Window
    /// </summary>
    public void Clear()
    {
      DispatcherHandler.BeginInvoke(() =>
      {
        mOutputWindow.Clear();
      }, DispatcherPriority.Normal);
    }

    /// <summary>
    /// Show the Output Window
    /// </summary>
    public void Show()
    {
      DispatcherHandler.BeginInvoke(() =>
      {
        mOutputWindow.Show(mDte);
      }, DispatcherPriority.Normal);
    }

    /// <summary>
    /// Write a string in the Output Window
    /// </summary>
    /// <param name="aMessage">The string which is going to be written</param>
    public void AddMessage(string aMessage)
    {
      DispatcherHandler.BeginInvoke(() =>
      {
        if (String.IsNullOrWhiteSpace(aMessage))
          return;
        mOutputWindow.Write(aMessage);
      }, DispatcherPriority.Normal);
    }

    /// <summary>
    /// Process the output messages from the power shell script
    /// to search for errors, warnings and messages from the clang compiler
    /// </summary>
    /// <param name="aMessage">The information which is going to be analized</param>
    private void ProcessOutput(string aMessage)
    {
      try
      {
        // Check if the LLVM is properly installed 
        if (mErrorParser.LlvmIsMissing(aMessage))
        {
          mMissingLlvm = true;
        }
        // If the LLVM is properly installed then parse the data
        else if (!mMissingLlvm)
        {
          // join all the data from the buffer
          string text = String.Join("\n", mMessagesBuffer) + "\n";

          // Check if any error, warning or message occurred until now
          if (mErrorParser.FindErrors(text, out TaskError aError))
          {
            // Set the error hierarchy
            aError.HierarchyItem = Hierarchy;

            // Add the new error
            List<TaskError> errors = new List<TaskError>();
            errors.Add(aError);

            // Get the text to display in Output Window
            StringBuilder output = new StringBuilder(
              GetOutput(ref text, aError.FullMessage));

            // Repeat the process for all remaining errors
            while (mErrorParser.FindErrors(text, out aError))
            {
              aError.HierarchyItem = Hierarchy;
              errors.Add(aError);
              output.Append(GetOutput(ref text, aError.FullMessage));
            }

            // Write the processed text in the Output Window
            AddMessage(output.ToString());
            output.Clear();

            // Clear the buffer 
            if (0 != mMessagesBuffer.Count)
              mMessagesBuffer.Clear();

            // Save the found errors 
            SaveErrorsMessages(errors);
          }
          else if (kBufferSize <= mMessagesBuffer.Count)
          {
            AddMessage(mMessagesBuffer[0]);
            mMessagesBuffer.RemoveAt(0);
          }
        }
      }
      catch (Exception)
      {

      }

    }

    public void OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      if (null == e.Data)
        return;
      mMessagesBuffer.Add(e.Data);
      ProcessOutput(e.Data);
    }

    public void OutputDataErrorReceived(object sender, DataReceivedEventArgs e)
    {
      if (null == e.Data)
        return;
      mMessagesBuffer.Add(e.Data);
      ProcessOutput(e.Data);
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Save all the founded errors
    /// </summary>
    /// <param name="aErrorCollection">The collection of founded errors</param>
    private void SaveErrorsMessages(List<TaskError> aErrorCollection)
    {
      if (0 == aErrorCollection.Count)
        return;

      foreach (var newError in aErrorCollection)
      {
        if (null == newError)
          continue;
        mErrors.Add(newError);
      }
    }

    /// <summary>
    /// Replace the regex error matching with a form that Visual Studio knows to interpret it automatically
    /// </summary>
    /// <param name="aText">All data info gathered in buffer until now </param>
    /// <param name="aSearchedSubstring">The error substring which will be searched</param>
    /// <returns>The data info before the error concatenated with the error properly formated</returns>
    private string GetOutput(ref string aText, string aSearchedSubstring)
    {
      // Format the error in such a way that VS knows to interpret it
      aText = mErrorParser.Format(aText, aSearchedSubstring);

      // Get the string before and the string after the error
      GetBeforeAndAfterSubstrings(aText, aSearchedSubstring,
        out string substringBefore, out string substringAfter);

      // The text after the error can contains another error so it is saved
      aText = substringAfter;

      return substringBefore + aSearchedSubstring;
    }

    /// <summary>
    /// Find the substring before and the substring after the error message
    /// </summary>
    /// <param name="aText">All the text info from the buffer</param>
    /// <param name="aSearchedSubstring">The error string which must be found</param>
    /// <param name="aTextBefore">The text before the error</param>
    /// <param name="aTextAfter">The text after the error</param>
    private void GetBeforeAndAfterSubstrings(string aText, string aSearchedSubstring, out string aTextBefore, out string aTextAfter)
    {
      aTextBefore = StringExtension.SubstringBefore(aText, aSearchedSubstring);
      aTextAfter = StringExtension.SubstringAfter(aText, aSearchedSubstring);
    }

    #endregion

  }
}
