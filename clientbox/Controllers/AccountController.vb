Imports System.Data.Entity
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Security.Cryptography
Imports System.ServiceModel
Imports System.Web.Mvc
Imports System.Web.OData.Query
Imports Newtonsoft.Json

Namespace Controllers
    Public Class AccountController
        Inherits Controller

        ' GET: Account
        <HttpGet>
        Function Login() As ActionResult
            'Dim hashpwd As String = GetMd5Hash("suchel@agilo.cz" & "1")

            'If User.Identity.IsAuthenticated Then
            '    Return RedirectToAction("Index", "Home")
            'End If

            'https://www.fio.cz/ib_api/rest/periods/{TOKEN}/{DATE_FROM}/{DATE_TO}/transactions.{FORMAT}

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
                            Return RedirectToAction("Klienti", "Home")
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

        Function LogOut()
            FormsAuthentication.SignOut()
            Return RedirectToAction("Login")
        End Function

        <HttpGet>
        Function GetUser() As ActionResult
            Return View()
        End Function

        <HttpPost>
        Function GetUser(email As String) As ActionResult
            Using db As New Data4995Entities
                Dim acc = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = email)
                If acc IsNot Nothing Then
                    Return RedirectToAction("ResetPassword", New With {.email = email})
                Else
                    ModelState.AddModelError("error", "Uživatel nenalezen")
                End If
            End Using
            Return View()
        End Function

        <HttpGet>
        Function ResetPassword(email As String) As ActionResult
            Using db As New Data4995Entities
                Dim acc = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = email)
                If acc IsNot Nothing Then
                    If acc IsNot Nothing Then
                        acc.UserPWD = GetMd5Hash(acc.UserLogin & "Agilo." & Now.Year)
                        db.AG_tblUsers.Attach(acc)
                        db.Entry(acc).State = EntityState.Modified
                        db.SaveChanges()

                        Return View(acc)
                    End If
                Else
                    ModelState.AddModelError("error", "Uživatel nenalezen")
                End If
            End Using
            Return View(New AG_tblUsers)
        End Function

        <HttpPost>
        Function ResetPassword(id As Integer, password1 As String, password2 As String) As ActionResult
            Using db As New Data4995Entities
                Dim acc = db.AG_tblUsers.FirstOrDefault(Function(e) e.IDUser = id)
                If acc IsNot Nothing Then
                    If acc IsNot Nothing Then
                        If password1 <> password2 Then
                            ModelState.AddModelError("error", "Hesla se neshodují")
                            Return View(acc)
                        Else
                            acc.UserPWD = GetMd5Hash(acc.UserLogin.ToLower & password2)
                            db.AG_tblUsers.Attach(acc)
                            db.Entry(acc).State = EntityState.Modified
                            db.SaveChanges()

                            Dim authTicket = New FormsAuthenticationTicket(1,
                                                               acc.UserLogin,
                                                               DateTime.Now,
                                                               DateTime.Now.AddYears(100),
                                                               True,
                                                               JsonConvert.SerializeObject(New With {.IDUser = acc.IDUser, .IDRole = acc.IDRole}),
                                                               FormsAuthentication.FormsCookiePath)
                            Dim cookie As New HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
                            Response.Cookies.Add(cookie)
                            Return RedirectToAction("Klienti", "Home")
                        End If
                    End If
                Else
                    ModelState.AddModelError("error", "Uživatel nenalezen")
                End If
            End Using
            Return View(New AG_tblUsers)
        End Function
    End Class
End Namespace