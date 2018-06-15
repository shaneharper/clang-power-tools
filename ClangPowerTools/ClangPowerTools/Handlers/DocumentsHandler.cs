using EnvDTE;

namespace ClangPowerTools
{
  /// <summary>
  /// Helper class for getting and saving the active document(s)
  /// </summary>
  public class DocumentsHandler
  {
    /// <summary>
    /// Get the active documents
    /// </summary>
    /// <param name="aDte">DTE reference</param>
    /// <returns>Active documents</returns>
    public static Documents GetActiveDocuments(DTE aDte) => aDte.Documents;

    /// <summary>
    /// Get the active document
    /// </summary>
    /// <param name="aDte">DTE reference</param>
    /// <returns>Active document</returns>
    public static Document GetActiveDocument(DTE aDte) => aDte.ActiveDocument;

    /// <summary>
    /// Save the active documents
    /// </summary>
    /// <param name="aDte">DTE reference</param>
    public static void SaveActiveDocuments(DTE aDte) => GetActiveDocuments(aDte).SaveAll();

    /// <summary>
    /// Save the active document
    /// </summary>
    /// <param name="aDte">DTE reference</param>
    public static void SaveActiveDocument(DTE aDte) => GetActiveDocument(aDte).Save();
  }
}
