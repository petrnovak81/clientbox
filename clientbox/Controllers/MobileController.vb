Imports System.Web.Mvc
Imports Newtonsoft.Json

Namespace Controllers
    Public Class MobileController
        Inherits Controller


        Function Index() As ActionResult
            If Not User.Identity.IsAuthenticated Then
                Return Redirect("Login")
            End If
            Return View()
        End Function

        Function Login() As ActionResult
            Return View()
        End Function

        <HttpPost>
        Function Login(email As String, password As String) As ActionResult
            Using db As New Data4995Entities
                Dim acc = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = email)
                If acc IsNot Nothing Then
                    Dim hashpwd As String = GetMd5Hash(email.ToLower & password)
                    If hashpwd = acc.UserPWD Then
                        If acc.UserAccountEnabled Then
                            Dim authTicket = New FormsAuthenticationTicket(1,
                                                               acc.UserLogin,
                                                               DateTime.Now,
                                                               DateTime.Now.AddYears(100),
                                                               True,
                                                               JsonConvert.SerializeObject(acc),
                                                               FormsAuthentication.FormsCookiePath)
                            Dim cookie As New HttpCookie(FormsAuthentication.FormsCookieName,
                                     FormsAuthentication.Encrypt(authTicket))
                            Response.Cookies.Add(cookie)
                            Return Redirect("Index")
                        Else
                            'ucet neni povolen
                            ModelState.AddModelError("error", "Přístup Vám byl odepřen")
                        End If
                    Else
                        'zadal spatne heslo k uctu
                        ModelState.AddModelError("error", "Chybně zadané heslo nebo uživatelské jméno")
                    End If
                Else
                    'ucet neexistuje
                    ModelState.AddModelError("error", "Uživatel nenalezen")
                End If
            End Using
            Return View()
        End Function
    End Class
End Namespace