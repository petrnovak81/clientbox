Imports System.Web.Http.Controllers
Imports System.Web.Http.Filters
Imports System.Net.Http
Imports System.Threading
Imports System.Security.Principal
Imports System.Net
Imports System.Security.Cryptography
Imports Newtonsoft.Json

Public Class AuthorizationBasic
    Inherits AuthorizationFilterAttribute
    'Public db As New TRACEEntities

    Public Overrides Sub OnAuthorization(actionContext As HttpActionContext)

        Dim authHeader = actionContext.Request.Headers.Authorization
        If authHeader IsNot Nothing AndAlso Not String.IsNullOrEmpty(authHeader.Parameter) AndAlso authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) Then
            Dim encoding__1 = Encoding.GetEncoding("iso-8859-1")
            Dim credentialstring = encoding__1.GetString(Convert.FromBase64String(authHeader.Parameter))
            Dim credentials = credentialstring.Split(":"c)
            Dim user = Validate(credentials(0), credentials(1))

            If user IsNot Nothing Then
                Dim identity = New GenericIdentity(credentials(0))
                Dim principal = New GenericPrincipal(identity, Nothing)
                Thread.CurrentPrincipal = principal
                If HttpContext.Current IsNot Nothing Then
                    HttpContext.Current.User = principal
                End If

                Return
            End If
        End If

        HandleUnauthorizedRequest(actionContext)
    End Sub

    Protected Sub HandleUnauthorizedRequest(actionContext As HttpActionContext)
        'Modify if required.
        Dim realm = "dotnetthoughts"
        Dim result = New HttpResponseMessage() With {
            .StatusCode = HttpStatusCode.Unauthorized,
            .RequestMessage = actionContext.ControllerContext.Request
        }
        'tohle vyvola okno pro prihlaseni
        'result.Headers.Add("WWW-Authenticate", String.Format("Basic realm=""{0}""", realm))

        actionContext.Response = result
    End Sub

    Private Function Validate(username As String, password As String) As AG_tblUsers
        Try
            Using db As New Data4995Entities
                Dim login = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = username)
                If login IsNot Nothing Then
                    Dim pwdHash = GetMd5Hash(username & password)
                    Dim user As AG_tblUsers = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = username And e.UserPWD = pwdHash)
                    If user IsNot Nothing Then
                        If user.UserAccountEnabled Then
                            Return user
                        End If
                    End If
                End If
                Return Nothing
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Shared Function GetMd5Hash(ByVal input As String) As String
        Dim md5Hash = MD5.Create()
        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input))

        ' Create a new Stringbuilder to collect the bytes
        ' and create a string.
        Dim sBuilder As New StringBuilder()

        ' Loop through each byte of the hashed data 
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        ' Return the hexadecimal string.
        Return sBuilder.ToString()

    End Function 'GetMd5Hash

End Class

