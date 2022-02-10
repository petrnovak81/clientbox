
Imports System.Runtime.CompilerServices

Public Module HelpersExtensions

    <Extension()>
    Public Function CurAction(ByVal helper As HtmlHelper) As String
        Dim a = helper.ViewContext.RouteData.Values("Action").ToString()
        Return a
    End Function

End Module
