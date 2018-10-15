using ClangPowerTools.DialogPages;
using Microsoft.VisualStudio.Shell;
using System;

namespace ClangPowerTools.Providers
{
  public static class SettingsProvider
  {
    #region Members

    private static Package mAsyncPackage;

    #endregion

    #region 

    public static void Initialize(Package aAsyncPackage) => mAsyncPackage = aAsyncPackage;

    #endregion

    #region 

    public static DialogPage GetPage(Type aType)
    {
      if (aType == typeof(ClangGeneralOptionsView))
        return mAsyncPackage.GetDialogPage(aType);

      if (aType == typeof(ClangTidyOptionsView))
        return mAsyncPackage.GetDialogPage(aType);

      if (aType == typeof(ClangTidyCustomChecksOptionsView))
        return mAsyncPackage.GetDialogPage(aType);

      if (aType == typeof(ClangTidyPredefinedChecksOptionsView))
        return mAsyncPackage.GetDialogPage(aType);

      if (aType == typeof(ClangFormatOptionsView))
        return mAsyncPackage.GetDialogPage(aType);

      return null;
    }

    #endregion
  }
}
