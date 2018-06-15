using EnvDTE;
using System;
using System.Collections.Generic;

namespace ClangPowerTools
{
  /// <summary>
  /// Get the path from a document/item or the paths from a collection of documents/items 
  /// </summary>
  public class FilePathCollector
  {
    #region Public Methods

    /// <summary>
    /// Extract the path of the item collection
    /// </summary>
    /// <param name="aItems">A collection of items</param>
    /// <returns>A collection of paths</returns>
    public IEnumerable<string> Collect(IEnumerable<IItem> aItems)
    {
      var filesPath = new List<string>();
      try
      {
        foreach (var item in aItems)
          filesPath.Add(item.GetPath());
      }
      catch (Exception) { }

      return filesPath;
    }

    /// <summary>
    /// Extract the paths of the documents collection
    /// </summary>
    /// <param name="aDocuments">A collection of documents</param>
    /// <returns>A collection of paths</returns>
    public IEnumerable<string> Collect(Documents aDocuments)
    {
      var filesPath = new List<string>();
      foreach (Document doc in aDocuments)
        filesPath.Add(doc.FullName);
      return filesPath;
    }

    /// <summary>
    /// Get the path of the document
    /// </summary>
    /// <param name="aDocument">A specific document from where the full name will be extracted</param>
    /// <returns>The document full name</returns>
    public string Collect(Document aDocument) => aDocument.FullName;

    #endregion
  }
}
