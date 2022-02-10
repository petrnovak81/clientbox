Imports System.Data.Entity
Imports System.Data.Entity.Core.Objects
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Mail
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading
Imports System.Web.Http
Imports System.Web.OData.Query
Imports System.Xml
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Calendar.v3
Imports Google.Apis.Calendar.v3.Data
Imports Google.Apis.Services
Imports Microsoft.AspNet.SignalR
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks
Imports System.Xml.Serialization

Module DateTimeExtensions
    <Extension()>
    Function StartOfWeek(ByVal _dt As DateTime, ByVal _startOfWeek As DayOfWeek) As DateTime
        Dim diff As Integer = (7 + (_dt.DayOfWeek - _startOfWeek)) Mod 7
        Return _dt.AddDays(-1 * diff).Date
    End Function
End Module


'Public Class RecordOnPage(Of T)
'    Public Function GetPage(Item As T, Items As List(Of T), Count As Integer) As Integer
'        Dim Index = Items.IndexOf(Item)
'        Dim Page As Integer = Index / Count
'        Return Page
'    End Function
'End Class

Public Class ServiceController
    Inherits ApiController

    Private Shared hubContext As IHubContext = GlobalHost.ConnectionManager.GetHubContext(Of AgiloHub)()

    <HttpPost>
    Public Sub PhoneRinging()
        Dim q = HttpContext.Current.Request.Form
        Dim login = ""
        If Not String.IsNullOrEmpty(q("login")) Then
            login = q("login")
        End If
        Dim clientName = ""
        If Not String.IsNullOrEmpty(q("clientName")) Then
            clientName = q("clientName")
        End If
        Dim clientNumber = ""
        If Not String.IsNullOrEmpty(q("clientNumber")) Then
            clientNumber = q("clientNumber")
        End If
        Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
        Dim new_IDCall As New ObjectParameter("New_IDCall", GetType(Integer))
        Using db As New Data4995Entities
            db.AGsp_Do_IPhoneCallToHistory(login, clientNumber, clientName, new_IDCall, lL_LastLapse)
            hubContext.Clients.All.phoneRinging(new_IDCall.Value, login, clientName, clientNumber)
        End Using
    End Sub

    <HttpGet>
    Public Overridable Function AGsp_Get_PhoneCallListUser() As Object
        Try
            Using db As New Data4995Entities
                Dim u = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name)
                If u IsNot Nothing Then
                    Dim data = db.AGsp_Get_PhoneCallListUser(u.IDUser).ToList
                    Return New With {.data = data, .total = data.Count, .error = Nothing}
                Else
                    Return New With {.data = New List(Of AGsp_Get_PhoneCallListUser_Result), .total = 0, .error = Nothing}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    Private Shared Function RemoveDiacritics(ByVal text As String) As String
        Dim normalizedString = text.Normalize(NormalizationForm.FormD)
        Dim stringBuilder = New StringBuilder()

        For Each c In normalizedString
            Dim unicodeCategory = Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
            If unicodeCategory <> unicodeCategory.NonSpacingMark Then
                stringBuilder.Append(c)
            End If
        Next

        Return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower
    End Function

    <HttpGet>
    Public Function GetMobileVersion() As Object
        Dim pt = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/output.json")
        If File.Exists(pt) Then
            Using sr As New StreamReader(pt)
                Dim jarr As JArray = JArray.Parse(sr.ReadToEnd)
                For Each content As JObject In jarr.Children(Of JObject)()
                    For Each prop As JProperty In content.Properties()
                        Dim propertyName = prop.Name
                        If propertyName = "apkInfo" Then
                            Dim versionCode = JObject.Parse(prop.Value.ToString).Properties().FirstOrDefault(Function(e) e.Name = "versionCode").Value.ToString
                            Return New With {.version = versionCode}
                        End If
                    Next
                    Exit For
                Next
                Return New With {.version = "20102"}
            End Using
        Else
            Return New With {.version = "20102"}
        End If
    End Function

    <HttpGet>
    Public Function Ping(sec As Date) As Object
        Return sec.ToString("ss.fff tt")
    End Function

    <HttpGet>
    <AuthorizationBasic>
    Public Function Connection(login As String) As Object
        Using db As New Data4995Entities
            Dim acc = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = login)
            Return acc
        End Using
    End Function

    <HttpGet>
    Public Function Ares(ico As String) As Object
        Try
            Dim dic As String = "",
                    firma As String = "",
                    mesto As String = "",
                    ulice As String = "",
                    cp As String = "",
                    psc As String = "",
                    obr As String = "",
                    xmlstr As String = "",
                    jsondata As String = "",
                    doc As XDocument
            Using wbc As New System.Net.WebClient()
                wbc.Encoding = ASCIIEncoding.UTF8
                xmlstr = wbc.DownloadString("http://wwwinfo.mfcr.cz/cgi-bin/ares/darv_bas.cgi?ico=" & ico)
                doc = XDocument.Parse(xmlstr)
                Dim desc = doc.Descendants().ToList
                Dim element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "ICO")
                If element IsNot Nothing Then
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "DIC")
                    If element IsNot Nothing Then
                        dic = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "OF")
                    If element IsNot Nothing Then
                        firma = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "N")
                    If element IsNot Nothing Then
                        mesto = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "NCO")
                    If element IsNot Nothing Then
                        If mesto Is String.Empty Then
                            mesto = element.Value
                        Else
                            If Not element.Value = mesto Then
                                mesto += "-" & element.Value
                            End If
                        End If
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "NU")
                    If element IsNot Nothing Then
                        ulice = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "CD")
                    If element IsNot Nothing Then
                        cp = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "PSC")
                    If element IsNot Nothing Then
                        psc = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "T")
                    If element IsNot Nothing Then
                        obr += "Zapsáno v obchodním rejstříku " & element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "OV")
                    If element IsNot Nothing Then
                        obr += ", oddíl a vložka " & element.Value
                    End If
                Else
                    Return Nothing
                End If

                Dim d = New With {
                    .ico = ico,
                    .dic = dic,
                    .firma = firma,
                    .ulice = ulice,
                    .cp = cp,
                    .psc = psc,
                    .mesto = mesto,
                    .or = obr,
                    .adresa = ulice & " " & cp & ", " & psc & " " & mesto
                    }

                Return d
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return Nothing
        End Try
    End Function

    <HttpGet>
    Public Function AG_tbRole_Read() As Object
        Using db As New Data4995Entities
            Return db.AG_tblRoles.Select(Function(e) New With {.IDRole = e.IDRole, .RoleName = e.RoleName}).ToList
        End Using
    End Function

    <HttpGet>
    Public Function AG_tblUsers_Read(options As ODataQueryOptions(Of AG_tblUsers)) As Object
        Try
            Using db As New Data4995Entities
                Dim model = db.AG_tblUsers.ToList
                Dim queryable As IQueryable(Of AG_tblUsers) = model.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                Dim result = TryCast(options.ApplyTo(queryable), IEnumerable(Of AG_tblUsers)).ToList
                Return result.AsEnumerable().Select(Function(r) New AG_tblUsers With {
                                                                  .IDRole = r.IDRole,
                                                                  .IDUser = r.IDUser,
                                                                  .UserFirstName = r.UserFirstName,
                                                                  .UserLastName = r.UserLastName,
                                                                  .UserLogin = r.UserLogin,
                                                                  .UserMobilePhone = r.UserMobilePhone,
                                                                  .UserAccountEnabled = r.UserAccountEnabled}).ToList
            End Using
        Catch ex As Exception
            Return New With {.total = 0, .data = {}, .message = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Function AG_tblUsers_Create(model As AG_tblUsers) As Object
        Try
            Using db As New Data4995Entities
                Dim item As New AG_tblUsers
                item.IDRole = model.IDRole
                item.UserFirstName = model.UserFirstName
                item.UserLastName = model.UserLastName
                item.UserLogin = model.UserLogin
                item.UserPWD = GetMd5Hash(model.UserLogin & "Agilo." & Now.Year)
                item.UserMobilePhone = model.UserMobilePhone
                item.UserAccountEnabled = model.UserAccountEnabled

                db.AG_tblUsers.Add(item)
                db.SaveChanges()
                Return model
            End Using
        Catch ex As Exception
            Return New With {.total = 0, .data = {}, .message = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Function AG_tblUsers_Update(model As AG_tblUsers) As Object
        Try
            Using db As New Data4995Entities
                Dim item = db.AG_tblUsers.FirstOrDefault(Function(e) e.IDUser = model.IDUser)
                If item IsNot Nothing Then
                    item.IDRole = model.IDRole
                    item.UserFirstName = model.UserFirstName
                    item.UserLastName = model.UserLastName
                    item.UserLogin = model.UserLogin
                    item.UserMobilePhone = model.UserMobilePhone
                    item.UserAccountEnabled = model.UserAccountEnabled

                    db.AG_tblUsers.Attach(item)
                    db.Entry(item).State = EntityState.Modified
                    db.SaveChanges()
                End If
                Return model
            End Using
        Catch ex As Exception
            Return New With {.total = 0, .data = {}, .message = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Function AG_tblUsers_Destroy(model As AG_tblUsers) As Object
        Try
            Using db As New Data4995Entities
                Dim item = db.AG_tblUsers.FirstOrDefault(Function(e) e.IDUser = model.IDUser)
                If item IsNot Nothing Then
                    db.AG_tblUsers.Remove(item)
                    db.SaveChanges()
                End If
                Return Nothing
            End Using
        Catch ex As Exception
            Return New With {.total = 0, .data = {}, .message = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Function AG_tblUsers_ResetPWD(model As AG_tblUsers) As Object
        Try
            Using db As New Data4995Entities
                Dim item = db.AG_tblUsers.FirstOrDefault(Function(e) e.IDUser = model.IDUser)
                If item IsNot Nothing Then
                    item.UserPWD = GetMd5Hash(model.UserLogin & "Agilo." & Now.Year)

                    db.AG_tblUsers.Attach(item)
                    db.Entry(item).State = EntityState.Modified
                    db.SaveChanges()
                End If
                Return New With {.total = 0, .data = {}, .message = Nothing}
            End Using
        Catch ex As Exception
            Return New With {.total = 0, .data = {}, .message = ex.Message}
        End Try
    End Function

    '***************

    'klienti

    <HttpGet>
    Public Function AGsp_GetFirmaAll(options As ODataQueryOptions(Of AGsp_GetFirmaAll_Result)) As Object
        Try
            Using db As New Data4995Entities
                Dim model = db.AGsp_GetFirmaAll.ToList
                Dim queryable As IQueryable(Of AGsp_GetFirmaAll_Result) = model.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                    Return New With {.total = model.Count, .data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_GetFirmaAll_Result)).ToList, .message = Nothing}
                Else
                    Return New With {.total = model.Count, .data = model.ToList, .message = Nothing}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.total = 0, .data = {}, .message = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Function AGvw_FirmyAdresyFakturacni() As Object
        Try
            Using db As New Data4995Entities
                Dim dt = db.AGvw_FirmyAdresyFakturacni.ToList
                Return New With {.data = dt, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    '*********************
    <HttpGet>
    Public Function RegRest(Register As String) As Object
        Try
            Using db As New Data4995Entities
                Dim dt = db.AG_tblRegisterRestrictions.Where(Function(e) e.Register = Register).Select(Function(e) New With {.value = e.IDOrder, .text = e.Val, .value2 = e.Val2}).ToList
                Return New With {.data = dt, .total = dt.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Function AGvwrr_TypykontaktnichUdaju() As Object
        Try
            Using db As New Data4995Entities
                Dim dt = db.AGvwrr_TypykontaktnichUdaju.Select(Function(e) New With {.value = e.Hodnota, .text = e.Hodnota}).ToList
                Return New With {.data = dt, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Function Uzivatele() As Object
        Try
            Using db As New Data4995Entities
                Dim dt = db.AG_tblUsers.Select(Function(e) New With {.value = e.IDUser, .text = e.UserFirstName & " " & e.UserLastName}).ToList
                Return New With {.data = dt, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetHledejGlobalFullText(hledej As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetHledejGlobalFullText(hledej).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_HledejSluzbaFullText(hledej As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_HledejSluzbaFullText(hledej).OrderBy(Function(e) e.Produkt).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetMojePracaky(iDUser As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetMojePracaky(iDUser).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetObjednavkyVse() As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetObjednavkyVse().ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaPracovisteSeznam(firma As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmaPracovisteSeznam(firma).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaPracovisteDetail(firma As String, pracoviste As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmaPracovisteDetail(firma, pracoviste).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_EditFirmaDetail(firma As AddNewFirma) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_EditFirmaDetail(firma.Firma, firma.Obor_cinnosti, firma.Poznamky)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_FirmaExist(firma As String) As Object
        Try
            Using db As New Data4995Entities
                Dim existuje As New ObjectParameter("Existuje", GetType(Int32))
                db.AGsp_FirmaExist(firma, existuje)
                Return New With {.data = existuje.Value, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_AddOrEditPracoviste(pracoviste As AddOrEditPracoviste) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_AddOrEditPracoviste(pracoviste.firma,
                                         pracoviste.rr_TypAdresy,
                                         pracoviste.nazev_firmy,
                                         pracoviste.ulice,
                                         pracoviste.pSC,
                                         pracoviste.mesto,
                                         pracoviste.vzdalenost,
                                         pracoviste.telefon_1,
                                         pracoviste.telefon_2,
                                         pracoviste.e_mail,
                                         pracoviste.poznamky,
                                         pracoviste.nadrizena_firma,
                                         pracoviste.titul,
                                         pracoviste.krestni,
                                         pracoviste.prijmeni,
                                         pracoviste.funkce,
                                         pracoviste.zasilat)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaDetail(firma As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmaDetail(firma).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Function AGsp_AddNewFirma(firma As AddNewFirma) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_AddNewFirma(firma.Firma,
                                    firma.Nazev_firmy,
                                    firma.Telefon_1,
                                    firma.Telefon_2,
                                    firma.E_mail,
                                    firma.ICO,
                                    firma.DIC,
                                    firma.Obor_cinnosti,
                                    firma.Poznamky,
                                    firma.Titul,
                                    firma.Krestni,
                                    firma.Prijmeni,
                                    firma.Funkce,
                                    firma.Zasilat,
                                    firma.ICZ)
            End Using
            Return New With {.data = "OK", .error = Nothing}
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaKontaktySeznam(firma As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmaKontaktySeznam(firma).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaKontaktyDetail(firma As String, kontaktni_udaj As Short) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmaKontaktyDetail(firma, kontaktni_udaj).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_AddOrEditFirmyKontaktniudaje(kontakt As AGsp_GetFirmaKontaktyDetail_Result) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_AddOrEditFirmyKontaktniudaje(kontakt.Kontakt,
                                                     kontakt.Kontaktni_udaj,
                                                     kontakt.Typ_KU,
                                                     kontakt.Nazev_KU,
                                                     kontakt.Hodnota_KU)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaInventarSeznam(firma As String, pracoviste As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmaInventarSeznam(firma, pracoviste).OrderByDescending(Function(e) e.DatumNaposledyZakoupeno).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaInventarDetail(iDInventare As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmaInventarDetail(iDInventare).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_AddOrEditFirmyInventar(inventar As AddOrEditFirmyInventar) As Object
        Try
            Using db As New Data4995Entities
                Dim newIDInventare As New ObjectParameter("NewIDInventare", GetType(Integer))
                db.AGsp_AddOrEditFirmyInventar(inventar.IDInventare,
                                               inventar.Firma,
                                               inventar.InventarPopis,
                                               inventar.InventarProdukt,
                                               inventar.DatumNaposledyZakoupeno,
                                               inventar.rr_TypInventare,
                                               inventar.DatumExpirace,
                                               newIDInventare)
                Dim id = newIDInventare.Value
                Return New With {.data = id, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaObjednavkySeznam(firma As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmaObjednavkySeznam(firma).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaObjednavkySeznamDlePracoviste(pracoviste As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmaObjednavkySeznamDlePracoviste(pracoviste).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmyObjednavkaDetail(iDObjednavkyPol As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmyObjednavkaDetail(iDObjednavkyPol).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_AddOrEditFirmyObjednavka(objednavka As AddOrEditFirmyObjednavka) As Object
        Try
            Using db As New Data4995Entities
                Dim oK As New ObjectParameter("OK", GetType(Boolean))
                Dim zprava As New ObjectParameter("Zprava", GetType(String))

                db.AGsp_AddOrEditFirmyObjednavka(objednavka.IDObjednavkyPol,
                                                 objednavka.FirmaObjednal,
                                                 objednavka.Produkt,
                                                 objednavka.Poznamka,
                                                 objednavka.ObjednanoEMJ,
                                                 objednavka.DomluvenaProdejniCena,
                                                 objednavka.rr_DeadLine,
                                                 objednavka.DeadLineDatum,
                                                 objednavka.IDUserObjednal,
                                                 Now,
                                                 objednavka.RadioObjednat,
                                                 objednavka.RadioObjednano,
                                                 objednavka.RadioZrusit,
                                                 oK,
                                                 zprava)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_DefaultAddPracoviste(firma As String) As Object
        Try
            Using db As New Data4995Entities
                Dim firmaPracoviste As New ObjectParameter("FirmaPracoviste", GetType(String))
                Dim rr_TypAdresy As New ObjectParameter("rr_TypAdresy", GetType(String))
                Dim nazev_firmy As New ObjectParameter("Nazev_firmy", GetType(String))
                db.AGsp_DefaultAddPracoviste(firma, firmaPracoviste, rr_TypAdresy, nazev_firmy)
                Return New With {.data = New With {
                    .Firma = firmaPracoviste.Value,
                    .rr_TypAdresy = rr_TypAdresy.Value,
                    .Nazev_firmy = nazev_firmy.Value}, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Function AGsp_AddFirmaInventarFoto() As Object
        Try
            Dim file As HttpPostedFile = HttpContext.Current.Request.Files("file")
            Dim iDInventare As Integer = HttpContext.Current.Request.Form("iDInventare")
            Dim dir = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/")
            Dim destination = Path.Combine(dir, file.FileName)

            If Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If

            If System.IO.File.Exists(destination) Then
                System.IO.File.Delete(destination)
            End If

            file.SaveAs(destination)

            Using db As New Data4995Entities
                db.AGsp_AddFirmaInventarFoto(iDInventare, "Files/" & file.FileName)
            End Using

            Return New With {.data = Nothing, .error = Nothing}
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_DelFirmaInventarFoto(iDFoto As Integer, path As String) As Object
        Try
            Dim f = System.Web.Hosting.HostingEnvironment.MapPath("~/" & path)
            Using db As New Data4995Entities
                IO.File.Delete(f)

                db.AGsp_DelFirmaInventarFoto(iDFoto)

                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaInventarFoto(iDInventare As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetFirmaInventarFoto(iDInventare).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_HledejProduktFullText(hledej As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_HledejProduktFullText(hledej).OrderBy(Function(e) e.Produkt).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_AddProduktZbozi(model As AGsp_GetProduktDetail_Result) As Object
        Try
            Dim povedloSe As New ObjectParameter("PovedloSe", GetType(Boolean)),
                hlaseni As New ObjectParameter("Hlaseni", GetType(String))
            Using db As New Data4995Entities
                db.AGsp_AddProduktZbozi(model.Produkt,
                                         model.Popis,
                                         model.Carovy_kod,
                                         model.Jednotky,
                                         model.Dodavatel,
                                         model.Cena,
                                         model.Mnozstvi_minimalni,
                                         model.Internet,
                                         model.Poznamka,
                                         Nothing,
                                         povedloSe,
                                         hlaseni)
                Return New With {.data = Nothing, .error = hlaseni.Value}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_AddProduktNaskladnit(model As AddProduktNaskladnit) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Dim iDPrijemkaPolNEW As New ObjectParameter("IDPrijemkaPolNEW", GetType(Integer))
            Using db As New Data4995Entities
                Dim u = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name)
                If u IsNot Nothing Then
                    db.AGsp_AddProduktNaskladnit(model.Produkt, Now, model.NaskladnenoEMJ, model.CenaNakup, model.CenaProdej, model.VSPrijmovehoDokladu, model.IDObjednavkyPol, u.IDUser, model.IDMega, iDPrijemkaPolNEW, lL_LastLapse)
                    If lL_LastLapse.Value > 0 Then
                        Return New With {.data = Nothing, .error = lastLapse(lL_LastLapse.Value)}
                    Else
                        Return New With {.data = iDPrijemkaPolNEW.Value, .error = Nothing}
                    End If
                Else
                    Return New With {.data = Nothing, .error = "Uživatel nenalezen"}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetProduktJednotliveNaskladneno(produkt As String) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_GetProduktJednotliveNaskladneno(produkt).ToList
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function


    <HttpPost>
    Public Overridable Function AGsp_EditProduktZbozi(model As AGsp_GetProduktDetail_Result) As Object
        Try
            Dim povedloSe As New ObjectParameter("PovedloSe", GetType(Boolean)),
                hlaseni As New ObjectParameter("Hlaseni", GetType(String))
            Using db As New Data4995Entities
                db.AGsp_EditProduktZbozi(model.Produkt,
                                         model.Popis,
                                         model.Carovy_kod,
                                         model.Jednotky,
                                         model.Cena,
                                         model.Mnozstvi_minimalni,
                                         model.Internet,
                                         model.Poznamka,
                                         Nothing,
                                         povedloSe,
                                         hlaseni)
                Return New With {.data = Nothing, .error = hlaseni.Value}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetProduktDetail(produkt As String, barcode As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetProduktDetail(produkt, barcode).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Function AGvwrr_Dodavatel() As Object
        Try
            Using db As New Data4995Entities
                Dim d = db.AGvwrr_Dodavatel.ToList
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Function AGvw_FirmyDodavatele() As Object
        Try
            Using db As New Data4995Entities
                Dim d = db.AGvw_FirmyDodavatele.ToList
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_AddEditTask(m As AddEditTask) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_AddEditTask(m.IDTask, m.IDUserResitel, m.rr_DeadLine, m.DatumDeadLine, m.Predmet, m.Telo, m.rr_TaskStav)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_AddNewTask(m As AddNewTask) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_AddNewTask(m.IDUserZadal, m.IDUserResitel, m.DatumResitelPrevzal, m.rr_DeadLine, m.DatumDeadLine, m.Predmet, m.Telo, m.rr_TaskStav)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetTaskDetail(iDTask As Nullable(Of Integer)) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetTaskDetail(iDTask).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetTaskSeznam(iDUserResitel As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetTaskSeznam(iDUserResitel).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetVykazPraceFirmaSeznam(iDUser As Integer, firma As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetVykazPraceFirmaSeznam(iDUser, firma).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetVykazPraceNedokoncene() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Using db As New Data4995Entities
                Dim iDUser = 0
                If Not String.IsNullOrEmpty(q("iDUser")) Then
                    iDUser = q("iDUser")
                Else
                    iDUser = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                End If
                Dim hledej = ""
                If Not String.IsNullOrEmpty(q("hledej")) Then
                    hledej = RemoveDiacritics(q("hledej"))
                End If

                Dim data = db.AGsp_GetVykazPraceNedokoncene(iDUser).Where(Function(e) RemoveDiacritics(e.Firma).Contains(hledej)).ToList
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_AddNewVykazPrace() As Object
        Try
            Dim iDVykazPrace As New ObjectParameter("IDVykazPrace", GetType(Integer))
            Dim q = HttpContext.Current.Request.QueryString
            Using db As New Data4995Entities
                Dim iDUser = 0
                If Not String.IsNullOrEmpty(q("iDUser")) Then
                    iDUser = q("iDUser")
                Else
                    iDUser = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                End If
                Dim firma = ""
                If Not String.IsNullOrEmpty(q("firma")) Then
                    firma = q("firma")
                End If
                db.AGsp_AddNewVykazPrace(firma, iDUser, iDVykazPrace)
                Return New With {.data = iDVykazPrace.Value, .total = 1, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetVykazPracePolDetail(iDVykazPracePol As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetVykazPracePolDetail(iDVykazPracePol).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetVykazPracePolSeznam() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Using db As New Data4995Entities
                Dim iDVykazPrace = 0
                If Not String.IsNullOrEmpty(q("iDVykazPrace")) Then
                    iDVykazPrace = q("iDVykazPrace")
                End If
                Dim data = db.AGsp_GetVykazPracePolSeznam(iDVykazPrace).ToList
                Return New With {.data = data, .total = 1, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetVykazPraceSeznam(iDUser As Integer, hledej As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetVykazPraceSeznam(iDUser, hledej).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetVykazPraceDetail() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Using db As New Data4995Entities
                Dim iDVykazPrace = 0
                If Not String.IsNullOrEmpty(q("iDVykazPrace")) Then
                    iDVykazPrace = q("iDVykazPrace")
                End If
                Dim data = db.AGsp_GetVykazPraceDetail(iDVykazPrace).FirstOrDefault
                Return New With {.data = data, .total = 1, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_EditVykazPrace(m As EditVykazPrace) As Object
        Try
            Using db As New Data4995Entities
                If m.iDUserUpravil = 0 Then
                    m.iDUserUpravil = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                End If
                db.AGsp_EditVykazPrace(m.iDVykazPrace, m.datVzniku, m.iDUserUpravil, m.poznamka, m.rr_StavPracaku)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_AddOrEditVykazPracePol(m As AddOrEditVykazPracePol) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim iDVykazPracePolNEW As New ObjectParameter("IDVykazPracePolNEW", GetType(Integer))
                If m.iDUserUpravil = 0 Or m.iDUserUpravil Is Nothing Then
                    m.iDUserUpravil = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                End If
                db.AGsp_AddOrEditVykazPracePol(m.iDVykazPrace,
                                               m.iDVykazPracePol,
                                               m.iDUserUpravil,
                                               m.rr_TypPolozkyPracaku,
                                               m.casOd,
                                               m.casDo,
                                               m.hodin,
                                               m.iDTechnika,
                                               m.produkt,
                                               m.textNaFakturu,
                                               m.textInterniDoMailu,
                                               m.pocetEMJ,
                                               m.cenaEMJ,
                                               m.vzdalenka,
                                               m.zdarma,
                                               m.navzdoryServisceUctovat,
                                               m.najetoKM,
                                               m.rr_HodinoveUctovani,
                                               iDVykazPracePolNEW, lL_LastLapse)

                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If

                Dim rr_JakeChybiDopravne As New ObjectParameter("rr_JakeChybiDopravne", GetType(Integer))
                db.AGsp_Get_JakeChybiDopravne(m.iDVykazPrace, rr_JakeChybiDopravne)

                Return New With {.data = rr_JakeChybiDopravne.Value, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_VykazPraceSeznamJizdVMesici() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim iDVykazPrace = 0
            If Not String.IsNullOrEmpty(q("iDVykazPrace")) Then
                iDVykazPrace = q("iDVykazPrace")
            End If
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_VykazPraceSeznamJizdVMesici(iDVykazPrace, lL_LastLapse).ToList()
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_VykazPraceCerpaneHodinyVMesici() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim iDVykazPrace = 0
            If Not String.IsNullOrEmpty(q("iDVykazPrace")) Then
                iDVykazPrace = q("iDVykazPrace")
            End If
            Dim sumaHodin As New ObjectParameter("SumaHodin", GetType(Decimal))
            Dim sumaCerpano As New ObjectParameter("SumaCerpano", GetType(Decimal))
            Dim volnychHodinNaSmlouve As New ObjectParameter("VolnychHodinNaSmlouve", GetType(Decimal))
            Dim nazevServisky As New ObjectParameter("NazevServisky", GetType(String))
            Dim lL_LastLapse1 As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Dim lL_LastLapse2 As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_VykazPraceCerpaneHodinyVMesici(iDVykazPrace, lL_LastLapse1).ToList()
                If lL_LastLapse1.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse1.Value)}
                End If
                db.AGsp_Get_VykazPraceCerpaneHodinyVMesiciSumy(iDVykazPrace, 0, sumaHodin, sumaCerpano, nazevServisky, volnychHodinNaSmlouve, lL_LastLapse2)
                If lL_LastLapse2.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse2.Value)}
                End If
                Dim zbyvaHod = (volnychHodinNaSmlouve.Value - sumaCerpano.Value)
                If zbyvaHod < 0 Then
                    zbyvaHod = 0
                End If
                Return New With {.data = data, .total = data.Count, .error = Nothing, .volnychHod = volnychHodinNaSmlouve.Value, .zbyvaHod = zbyvaHod}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message, .zbyvaHod = 0}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_DelVykazPracePol(iDVykazPracePol As Integer) As Object
        Try
            Dim oK As New ObjectParameter("Ok", GetType(Boolean))
            Dim zprava As New ObjectParameter("Zprava", GetType(String))
            Using db As New Data4995Entities
                db.AGsp_DelVykazPracePol(iDVykazPracePol, oK, zprava)
                Return New With {.data = oK.Value, .error = zprava.Value}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetPracakHodnotyDopravne(iDVykazPrace As Nullable(Of Integer), rr_TypPolozkyPracaku As Byte) As Object
        Try
            Dim pocetKM As New ObjectParameter("PocetKM", GetType(Decimal))
            Dim najetoKM As New ObjectParameter("NajetoKM", GetType(Decimal))
            Dim dopravneText As New ObjectParameter("DopravneText", GetType(String))
            Using db As New Data4995Entities
                db.AGsp_GetPracakHodnotyDopravne(iDVykazPrace, rr_TypPolozkyPracaku, pocetKM, dopravneText, najetoKM)
                Return New With {.data = New With {.km = pocetKM.Value, .text = dopravneText.Value, .najeto = najetoKM.Value}, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetPracakUdelatDopravne(iDVykazPrace As Integer) As Object
        Try
            Dim udelatPracak As New ObjectParameter("UdelatPracak", GetType(Boolean))
            Using db As New Data4995Entities
                db.AGsp_GetPracakUdelatDopravne(iDVykazPrace, udelatPracak)
                Return New With {.data = udelatPracak.Value, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Function AGvw_FA_PracakySeznam(options As ODataQueryOptions(Of AGvw_FA_PracakySeznam), stav As String) As Object
        Try
            Using db As New Data4995Entities

                Dim result As IEnumerable(Of AGvw_FA_PracakySeznam)
                If stav = 60 Then
                    Dim model = db.AGvw_FA_PracakySeznam.OrderByDescending(Function(e) e.IDVykazPrace).ToList
                    Dim queryable As IQueryable(Of AGvw_FA_PracakySeznam) = model.AsQueryable()
                    If options.Filter IsNot Nothing Then
                        queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                    End If
                    result = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGvw_FA_PracakySeznam)).ToList
                ElseIf stav = 55 Then
                    Dim model = db.AGvw_FA_PracakySeznam.Where(Function(e) e.rr_StavPracaku < 30).OrderByDescending(Function(e) e.IDVykazPrace).ToList
                    Dim queryable As IQueryable(Of AGvw_FA_PracakySeznam) = model.AsQueryable()
                    If options.Filter IsNot Nothing Then
                        queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                    End If
                    result = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGvw_FA_PracakySeznam)).ToList
                ElseIf stav = -5 Then
                    Dim model = db.AGvw_FA_PracakySeznam.OrderByDescending(Function(e) e.IDVykazPrace).Take(300).ToList
                    Dim queryable As IQueryable(Of AGvw_FA_PracakySeznam) = model.AsQueryable()
                    If options.Filter IsNot Nothing Then
                        queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                    End If
                    result = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGvw_FA_PracakySeznam)).ToList
                Else
                    Dim model = db.AGvw_FA_PracakySeznam.Where(Function(e) e.rr_StavPracaku = stav).OrderByDescending(Function(e) e.IDVykazPrace).ToList
                    Dim queryable As IQueryable(Of AGvw_FA_PracakySeznam) = model.AsQueryable()
                    If options.Filter IsNot Nothing Then
                        queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                    End If
                    result = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGvw_FA_PracakySeznam)).ToList
                End If

                'Dim idu = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                'If idu = 7 Or idu = 8 Then
                '    result = result.Where(Function(e) e.IDUserZalozil = idu).ToList
                'End If

                Return New With {.Total = result.Count, .Data = result, .Errors = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.Total = 0, .Data = {}, .Errors = New String() {ex.Message}}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_FA_FakturovatNa(iDVykazPrace As Integer, rr_FakturovatNaFirmu As Short, datumZasahu As Date) As Integer
        Try
            Using db As New Data4995Entities
                db.AGsp_Do_FA_FakturovatNa(iDVykazPrace, rr_FakturovatNaFirmu, datumZasahu)
            End Using
            Return rr_FakturovatNaFirmu
        Catch ex As Exception
            Return 0
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_FA_PracakyDetailHL(iDVykazPrace As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim d = db.AGsp_FA_PracakyDetailHL(iDVykazPrace).ToList
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_FA_PracakyDetailPol(iDVykazPrace As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim d = db.AGsp_FA_PracakyDetailPol(iDVykazPrace).ToList
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    'iDVykazPrace: iDVykazPrace, produkt: d.Produkt, blokovatEMJ: pocet, cenaEMJProdejni: d.Cena, iDUserUpravil: 0
    <HttpGet>
    Public Overridable Function AGsp_AddNewPracakyPolozkaProduktZablokovat(iDVykazPrace As Integer,
                                                                           produkt As String,
                                                                           blokovatEMJ As Decimal,
                                                                           cenaEMJProdejni As Decimal,
                                                                           iDUserUpravil As Integer,
                                                                           iDPrijemkaPol As Integer) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                If iDUserUpravil = 0 Then
                    iDUserUpravil = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                End If

                db.AGsp_AddNewPracakyPolozkaProduktZablokovat(iDVykazPrace, produkt, blokovatEMJ, cenaEMJProdejni, iDUserUpravil, iDPrijemkaPol, lL_LastLapse)

                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .error = lastLapse(lL_LastLapse.Value)}
                Else
                    Return New With {.data = Nothing, .error = Nothing}
                End If

            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetProduktSeznamHledaci(hledej As String) As Object
        Try
            Using db As New Data4995Entities
                Dim sp = db.AGsp_GetProduktSeznamHledaci(hledej).OrderBy(Function(e) e.Produkt).ToList
                Return New With {.data = sp, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    Public Overridable Function AGsp_GetPracakTextDoMailu(iDVykazPrace As Integer) As Object
        Try
            Dim emailKomu As New ObjectParameter("EmailKomu", GetType(String))
            Dim predmet As New ObjectParameter("Predmet", GetType(String))
            Dim body As New ObjectParameter("Body", GetType(String))
            Dim zapati As New ObjectParameter("Zapati", GetType(String))
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_GetPracakTextDoMailu(iDVykazPrace, emailKomu, predmet, body, zapati, lL_LastLapse)

                If lL_LastLapse.Value = 12 Then
                    Return New With {.data = Nothing, .error = "Požadujete vyskladnit větší množství Produktu, než máte skladem. Blokace se neprovede. Upravte množství a proveďte blokaci znovu"}
                Else
                    Return New With {.data = New With {.komu = emailKomu.Value, .predmet = predmet.Value, .body = body.Value, .zapati = zapati.Value}, .error = Nothing}
                End If

            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Function SendEmail(EmailTo As String, Subject As String, Body As String, Attachments As List(Of Attachment)) As Object
        Try
            Using mail As New MailMessage()
                For Each a In Attachments
                    mail.Attachments.Add(a)
                Next
                mail.From = New MailAddress("podpora@doctorum.cz", "DOCTORUM.CZ")
                mail.To.Add(New MailAddress(EmailTo))
                mail.Subject = Subject
                mail.Body = Body
                mail.IsBodyHtml = True

                Dim smtp As SmtpClient = New SmtpClient()
                smtp.Host = "smtp.forpsi.com"
                smtp.EnableSsl = True

                Dim networkCredential As NetworkCredential = New NetworkCredential("podpora@doctorum.cz", "Frolikova.321")
                smtp.UseDefaultCredentials = True
                smtp.Credentials = networkCredential
                smtp.Port = 587
                smtp.Send(mail)

                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaPracovisteSeznamHledej() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Using db As New Data4995Entities
                Dim hledej = ""
                If Not String.IsNullOrEmpty(q("hledej")) Then
                    hledej = q("hledej")
                End If
                Dim hledatDleGPS = False
                If Not String.IsNullOrEmpty(q("hledatDleGPS")) Then
                    hledatDleGPS = q("hledatDleGPS")
                End If
                Dim gPSLat = ""
                If Not String.IsNullOrEmpty(q("gPSLat")) Then
                    gPSLat = q("gPSLat")
                End If
                Dim gPSLng = ""
                If Not String.IsNullOrEmpty(q("gPSLng")) Then
                    gPSLng = q("gPSLng")
                End If
                Dim data = db.AGsp_GetFirmaPracovisteSeznamHledej(hledej, hledatDleGPS, gPSLat, gPSLng).ToList
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_EditFirmaParametry(m As AGsp_GetFirmaParametry_Result) As Object
        Try
            Using db As New Data4995Entities
                Dim IDUser = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                Dim d = db.AGsp_EditFirmaParametry(m.IDFirmaParametr, m.PlatnostOd, m.HodinovaSazba, m.SazbaKm, m.rr_TypServisniSmlouvy, m.ServiskaCenaMesicne, m.ServiskaNaposledyVyuctovana, m.ServiskaIntervalObnoveni, m.ServiskaVolneHodiny, m.SazbaMalyZasah, m.SazbaVelkyZasah, IDUser)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaParametry(firma As String) As Object
        Try
            Using db As New Data4995Entities
                Dim d = db.AGsp_GetFirmaParametry(firma).ToList
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_AddNewOrEditProduktProcentoProvize(IDProduktProvize As Integer,
                                                                        IDUser As Integer,
                                                                        Produkt As String,
                                                                        ProcentoProvize As Decimal) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_AddNewOrEditProduktProcentoProvize(IDProduktProvize, IDUser, Produkt, ProcentoProvize)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetProduktProcentoProvizeSeznam(iDUser As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim d = db.AGsp_GetProduktProcentoProvizeSeznam(iDUser).ToList
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_AddNewFirmaParametry(firma As String, iDUser As Integer) As Object
        Try
            Using db As New Data4995Entities
                If iDUser = 0 Then
                    iDUser = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                End If
                db.AGsp_AddNewFirmaParametry(firma, iDUser)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function ProduktSeznam_CBX() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim filter = Nothing
            If Not String.IsNullOrEmpty(q("filter")) Then
                filter = q("filter")
            End If
            Using db As New Data4995Entities
                Dim data = db.AGsp_GetProduktSeznamHledaci(filter).OrderBy(Function(e) e.Produkt).ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGvwrr_StavPracaku() As Object
        Try
            Using db As New Data4995Entities
                Dim d = db.AGvwrr_StavPracaku.Select(Function(e) New With {.text = e.rr_StavPracakuHodnota, .value = e.rr_StavPracaku}).ToList
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmaPracovisteDetailKontakty(firma As String, pracoviste As String) As Object
        Try
            Using db As New Data4995Entities
                Dim d = db.AGsp_GetFirmaPracovisteDetailKontakty(firma, pracoviste).ToList
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_EditFirmaDetailPodrobne(firma As String, nazev_firmy As String, vzdalenost As String, obor_cinnosti As String, poznamky As String, icz As String) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_EditFirmaDetailPodrobne(firma, nazev_firmy, vzdalenost, obor_cinnosti, poznamky, icz)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    Function lastLapse(id As Integer) As String
        Dim lastLapseText As New ObjectParameter("LastLapseText", GetType(String))
        Using db As New Data4995Entities
            db.AGsp_Get_LastLapse(id, lastLapseText)
            If Not IsDBNull(lastLapseText.Value) Then
                Return lastLapseText.Value
            Else
                Return "LL_LastLapse: " & id
            End If
        End Using
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Run_Pracak00to10(iDVykazPrace As Integer) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Run_Pracak00to10(iDVykazPrace, lL_LastLapse)

                If lL_LastLapse.Value > 0 Then
                    Dim msg = lastLapse(lL_LastLapse.Value)
                    Return New With {.data = Nothing, .error = msg}
                Else
                    Return New With {.data = Nothing, .error = Nothing}
                End If

            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Run_Pracak10to20(iDVykazPrace As Integer) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Run_Pracak10to20(iDVykazPrace, lL_LastLapse)

                If lL_LastLapse.Value > 0 Then
                    Dim msg = lastLapse(lL_LastLapse.Value)
                    Return New With {.data = Nothing, .error = msg, .ll = lL_LastLapse.Value}
                Else
                    Return New With {.data = Nothing, .error = Nothing, .ll = 0}
                End If

            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message, .ll = 0}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Run_Pracak20to30(iDVykazPrace As Integer, cisloFaktury As String) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Run_Pracak20to30(iDVykazPrace, cisloFaktury, lL_LastLapse)

                If lL_LastLapse.Value > 0 Then
                    Dim msg = lastLapse(lL_LastLapse.Value)
                    Return New With {.data = Nothing, .error = msg}
                Else
                    Return New With {.data = Nothing, .error = Nothing}
                End If

            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Run_Pracak00to50(iDVykazPrace As Integer) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Run_Pracak00to50(iDVykazPrace, lL_LastLapse)

                If lL_LastLapse.Value > 0 Then
                    Dim msg = lastLapse(lL_LastLapse.Value)
                    Return New With {.data = Nothing, .error = msg}
                Else
                    Return New With {.data = Nothing, .error = Nothing}
                End If

            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetObjednavkySeznam(hledej As String, zobrazitVse As Boolean) As Object
        Try
            Using db As New Data4995Entities
                Dim d = db.AGsp_GetObjednavkySeznam(hledej, zobrazitVse).ToList
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetDialogObjednavkaTlacitka(iDObjednavkyPol As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim d = db.AGsp_GetDialogObjednavkaTlacitka(iDObjednavkyPol).ToList
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Run_Objednavka0or1or3to4(iDObjednavkyPol As Integer) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Run_Objednavka0or1or3to4(iDObjednavkyPol, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Dim msg = lastLapse(lL_LastLapse.Value)
                    Return New With {.data = Nothing, .error = msg}
                Else
                    Return New With {.data = Nothing, .error = Nothing}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Run_Objednavka0or1or3to5(iDObjednavkyPol As Integer) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Run_Objednavka0or1or3to5(iDObjednavkyPol, lL_LastLapse)
                Return New With {.data = lL_LastLapse.Value, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Run_Objednavka0or1to3(iDObjednavkyPol As Integer) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Run_Objednavka0or1to3(iDObjednavkyPol, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Dim msg = lastLapse(lL_LastLapse.Value)
                    Return New With {.data = Nothing, .error = msg}
                Else
                    Return New With {.data = Nothing, .error = Nothing}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_EditFirmaGPS(firma As String, gPSLat As String, gPSLng As String, gPSValid As Boolean) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_EditFirmaGPS(firma, gPSLat, gPSLng, gPSValid)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_FirmaParametryValidni(firma As String, stavValidity As Short) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_Do_FirmaParametryValidni(firma, stavValidity)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function


    'iDUser: IDUser, lat: position.coords.latitude, lng: position.coords.longitude
    <HttpGet>
    Public Overridable Function AG_tblUsersLastPosition(iDUser As Integer, lat As String, lng As String) As Object
        Try
            Using db As New Data4995Entities
                Dim wkt = String.Format(String.Format(Globalization.CultureInfo.InvariantCulture, "POINT({0} {1})", lng, lat))
                Dim item = db.AG_tblUsersLastPosition.FirstOrDefault(Function(e) e.IDUser = iDUser)
                If item IsNot Nothing Then
                    item.GPSLat = lat
                    item.GPSLng = lng
                    item.GPSTime = Now
                Else
                    item = New AG_tblUsersLastPosition
                    item.IDUser = iDUser
                    item.GPSLat = lat
                    item.GPSLng = lng
                    item.GPSTime = Now
                    db.AG_tblUsersLastPosition.Add(item)
                End If
                db.SaveChanges()

                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AresBasicInfo(ico As String) As Object
        Try
            Dim dic As String = "",
                firma As String = "",
                mesto As String = "",
                ulice As String = "",
                cp As String = "",
                psc As String = "",
                xmlstr As String = "",
                jsondata As String = "",
                doc As XDocument
            Using wbc As New System.Net.WebClient()
                wbc.Encoding = ASCIIEncoding.UTF8
                xmlstr = wbc.DownloadString("http://wwwinfo.mfcr.cz/cgi-bin/ares/darv_bas.cgi?ico=" & ico)
                doc = XDocument.Parse(xmlstr)
                Dim desc = doc.Descendants().ToList
                Dim element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "ICO")
                If element IsNot Nothing Then
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "DIC")
                    If element IsNot Nothing Then
                        dic = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "OF")
                    If element IsNot Nothing Then
                        firma = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "N")
                    If element IsNot Nothing Then
                        mesto = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "NCO")
                    If element IsNot Nothing Then
                        If mesto Is String.Empty Then
                            mesto = element.Value
                        Else
                            If Not element.Value = mesto Then
                                mesto += "-" & element.Value
                            End If
                        End If
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "NU")
                    If element IsNot Nothing Then
                        ulice = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "CD")
                    If element IsNot Nothing Then
                        cp = element.Value
                    End If
                    element = desc.FirstOrDefault(Function(n) n.Name.LocalName = "PSC")
                    If element IsNot Nothing Then
                        psc = element.Value
                    End If
                Else
                    Return Nothing
                End If

                Dim d = New With {
                .ico = ico,
                .dic = dic,
                .firma = firma,
                .ulice = ulice,
                .cp = cp,
                .psc = psc,
                .mesto = mesto,
                .adresa = ulice & " " & cp & ", " & psc & " " & mesto
                }

                Return d
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_AddSluzbaPreddefinovana(d As AGsp_GetSluzbyPreddefinovane_Result) As Object
        Try
            Dim povedloSe As New ObjectParameter("PovedloSe", GetType(Boolean))
            Dim hlaseni As New ObjectParameter("Hlaseni", GetType(String))
            Using db As New Data4995Entities
                db.AGsp_AddSluzbaPreddefinovana(d.Produkt, d.Popis, d.Carovy_kod, d.Jednotky, Nothing, d.Cena, d.Poznamka, Nothing, povedloSe, hlaseni)
                If povedloSe.Value Then
                    Return New With {.data = Nothing, .error = Nothing}
                Else
                    Return New With {.data = Nothing, .error = hlaseni.Value}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_EditSluzbaPreddefinovana(d As AGsp_GetSluzbyPreddefinovane_Result) As Object
        Try
            Dim povedloSe As New ObjectParameter("PovedloSe", GetType(Boolean))
            Dim hlaseni As New ObjectParameter("Hlaseni", GetType(String))
            Using db As New Data4995Entities
                db.AGsp_EditSluzbaPreddefinovana(d.Produkt, d.Popis, d.Carovy_kod, d.Jednotky, d.Cena, d.Poznamka, Nothing, povedloSe, hlaseni)
                If povedloSe.Value Then
                    Return New With {.data = Nothing, .error = Nothing}
                Else
                    Return New With {.data = Nothing, .error = hlaseni.Value}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetSluzbyPreddefinovane() As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_GetSluzbyPreddefinovane.ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetMegaPolSeznam(iDMega As Long) As Object
        Try
            Using db As New Data4995Entities
                Dim data = Nothing

                If iDMega > 0 Then
                    data = db.AGsp_GetMegaPolSeznam.FirstOrDefault(Function(e) e.IDMega = iDMega)
                Else
                    data = db.AGsp_GetMegaPolSeznam.ToList
                End If

                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_DelMegaPolozku(iDMega As Long) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_DelMegaPolozku(iDMega)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_EditMegaPol(d As AGsp_GetMegaPolSeznam_Result) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_EditMegaPol(d.IDMega, d.MegaCelkovaCena, d.DohodnutaProdejniCenaEMJ, d.Poznamka)
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetPracakPolozkyNahled(iDVykazPrace As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_GetPracakPolozkyNahled(iDVykazPrace).ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetPuvodPolozkyNaPracaku(iDVykazPracePol As Integer) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_GetPuvodPolozkyNaPracaku(iDVykazPracePol).FirstOrDefault
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetInventurniStav() As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_GetInventurniStav().ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetFirmyRecordHistorySeznam() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim iDFirmy = ""
            If Not String.IsNullOrEmpty(q("iDFirmy")) Then
                iDFirmy = q("iDFirmy")
            End If
            Using db As New Data4995Entities
                Dim data = db.AGsp_GetFirmyRecordHistorySeznam(iDFirmy).ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_AddOrEditFirmyRecordHistory() As Object
        Try
            Dim newIDFirmyRecordHistory As New ObjectParameter("NewIDFirmyRecordHistory", GetType(Integer))
            Dim q = HttpContext.Current.Request.QueryString
            Dim iDFirmyRecordHistory = 0
            If Not String.IsNullOrEmpty(q("iDFirmyRecordHistory")) Then
                iDFirmyRecordHistory = q("iDFirmyRecordHistory")
            End If
            Dim iDFirmy = ""
            If Not String.IsNullOrEmpty(q("iDFirmy")) Then
                iDFirmy = q("iDFirmy")
            End If
            Dim recordCommentType = ""
            If Not String.IsNullOrEmpty(q("recordCommentType")) Then
                recordCommentType = q("recordCommentType")
            End If
            Dim recordCommentTxt = ""
            If Not String.IsNullOrEmpty(q("recordCommentTxt")) Then
                recordCommentTxt = q("recordCommentTxt")
            End If
            Using db As New Data4995Entities
                Dim _user = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name)
                db.AGsp_AddOrEditFirmyRecordHistory(iDFirmyRecordHistory, iDFirmy, recordCommentType, recordCommentTxt, _user.IDUser, newIDFirmyRecordHistory)
                Return New With {.data = newIDFirmyRecordHistory.Value, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Function EmailBodyPoValidaci(iDVykazPrace As Integer) As Object
        Try
            Dim emailKomu As New ObjectParameter("EmailKomu", GetType(String))
            Dim predmet As New ObjectParameter("Predmet", GetType(String))
            Dim body As New ObjectParameter("Body", GetType(String))
            Dim zapati As New ObjectParameter("Zapati", GetType(String))
            Dim sumaHodin As New ObjectParameter("SumaHodin", GetType(Decimal))
            Dim sumaCerpano As New ObjectParameter("SumaCerpano", GetType(Decimal))
            Dim volnychHodinNaSmlouve As New ObjectParameter("VolnychHodinNaSmlouve", GetType(Decimal))
            Dim nazevServisky As New ObjectParameter("NazevServisky", GetType(String))
            Dim lL_LastLapse1 As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Dim lL_LastLapse2 As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Dim lL_LastLapse3 As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_GetPracakTextDoMailu(iDVykazPrace, emailKomu, predmet, body, zapati, lL_LastLapse1)
                If lL_LastLapse1.Value > 0 Then
                    Return New With {.body = Nothing, .error = lastLapse(lL_LastLapse1.Value)}
                End If
                Dim polozky = db.AGsp_FA_MailTextyPolozek(iDVykazPrace).ToList

                db.AGsp_Get_VykazPraceCerpaneHodinyVMesiciSumy(iDVykazPrace, 0, sumaHodin, sumaCerpano, nazevServisky, volnychHodinNaSmlouve, lL_LastLapse2)
                If lL_LastLapse2.Value > 0 Then
                    Return New With {.data = Nothing, .error = lastLapse(lL_LastLapse2.Value)}
                End If

                Dim hodiny = db.AGsp_Get_VykazPraceCerpaneHodinyVMesici(iDVykazPrace, lL_LastLapse3)
                If lL_LastLapse3.Value > 0 Then
                    Return New With {.data = Nothing, .error = lastLapse(lL_LastLapse3.Value)}
                End If

                Dim html = <body style="font-family:Arial, Helvetica, sans-serif;margin:20px;">
                               <p><%= body.Value %></p>
                               <table style="margin-top:20px;margin-bottom:20px;border:1px solid silver" border="0" cellpadding="0" cellspacing="0">
                                   <tr style="background:silver;color:white;"><td style="padding:6px;border-right:1px solid white;">Odpovědná osoba</td><td style="padding:6px;border-right:1px solid white;">Množství</td><td style="padding:6px;border-right:1px solid white;">Forma zásahu</td><td style="padding:6px;">Popis</td></tr>
                               </table>
                               <div style="margin-bottom:20px;padding:10px;border: 1px solid silver;display:table">
                                   <table>
                                       <tr>
                                           <td width="200" style="text-align:right;">Typ servisní smlouvy:</td>
                                           <td style="padding-left:30px;padding-right:30px;"><b><%= nazevServisky.Value %></b></td>
                                       </tr>
                                       <tr>
                                           <td style="text-align:right;">Čerpáno volných hodin:</td>
                                           <td style="padding-left:30px;padding-right:30px;"><b><%= sumaCerpano.Value %></b></td>
                                       </tr>
                                       <tr>
                                           <td style="text-align:right;">Zbývá volných hodin:</td>
                                           <td style="padding-left:30px;padding-right:30px;"><b><%= If((volnychHodinNaSmlouve.Value - sumaHodin.Value) < 0, 0, (volnychHodinNaSmlouve.Value - sumaHodin.Value)) %></b></td>
                                       </tr>
                                   </table>
                               </div>
                               <p style="margin-top:20px;margin-bottom:3px;"><b>Rekapitulace zásahů v tomto měsíci</b></p>
                               <table style="margin-bottom:60px;border:1px solid silver" border="0" cellpadding="0" cellspacing="0">
                                   <tr style="background:silver;color:white;"><td style="padding:6px;border-right:1px solid white;">Datum</td><td style="padding:6px;border-right:1px solid white;">Technik</td><td style="padding:6px;border-right:1px solid white;">Hodin</td><td style="padding:6px;border-right:1px solid white;">Pracák</td><td style="padding:6px;border-right:1px solid white;">Typ</td><td style="padding:6px;">Text na fakturu</td></tr>
                               </table>
                               <p><%= zapati.Value %></p>
                               <table border="0" cellspacing="0" style="color:#231f20;width:700px;background:#ffffff;">
                                   <tr>
                                       <td valign="top" align="center" rowspan="4" style="border-right: 4px solid #ca2426;">
                                           <img src="data:image/jpeg;base64,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"/>
                                       </td>
                                       <td valign="top" style="padding: 0 6px 0 6px;">
                                           <b style="color:#ca2426;">Jaroslava Frolíková</b> | vedoucí technické podpory
                                      </td>
                                       <td rowspan="4" style="padding: 0 6px 0 6px;">
                                           <img src="data:image/png;base64,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"/>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td valign="bottom" style="padding: 0 6px 0 6px;">
                                           <span style="color:#ca2426;">t:</span> +420 725 144 164
                                      </td>
                                   </tr>
                                   <tr>
                                       <td valign="bottom" style="padding: 0 6px 0 6px;">
                                           <span style="color:#ca2426;">e:</span> frolikova@doctorum.cz | <span style="color:#ca2426;">w:</span> www.doctorum.cz
                                      </td>
                                   </tr>
                                   <tr>
                                       <td valign="bottom" style="padding: 0 6px 0 6px;">
                                           <span style="color:#ca2426;">a:</span> Huťská 366, 272 01 Kladno
                                      </td>
                                   </tr>
                               </table>
                           </body>
                For Each p In polozky
                    Dim tr = <tr><td style="padding:6px;border-right:1px solid silver;"><%= p.TechnikOdpovednaOsoba %></td><td style="padding:6px;border-right:1px solid silver;"><%= String.Format("{0:0.0}", p.PocetEMJ) & " " & p.Jednotky %></td><td style="padding:6px;border-right:1px solid silver;"><%= p.FormaZasahu %></td><td style="padding:6px;"><%= p.TextDoMailu %></td></tr>
                    html.<table>(0).Add(tr)
                Next
                For Each h In hodiny
                    Dim tr = <tr><td style="padding:6px;border-right:1px solid silver;"><%= h.DatVzniku.Value.ToString("dd.MM.yyyy") %></td><td style="padding:6px;border-right:1px solid silver;"><%= h.UserLastName %></td><td style="padding:6px;border-right:1px solid silver;"><%= h.Hodin %></td><td style="padding:6px;border-right:1px solid silver;">#<%= h.IDVykazPrace %></td><td style="padding:6px;border-right:1px solid silver;"><%= h.rr_HodinoveUctovaniText %></td><td style="padding:6px;"><%= h.TextNaFakturu %></td></tr>
                    html.<table>(1).Add(tr)
                Next
                Dim d = New With {
                .emailTo = emailKomu.Value,
                .emailSubject = predmet.Value,
                .emailBody = html.ToString
                }
                Return New With {.data = d, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    '<HttpGet>
    'Public Function EmailPoValidaci(iDVykazPrace As Integer) As Object
    '    Try
    '        Dim emailKomu As New ObjectParameter("EmailKomu", GetType(String))
    '        Dim predmet As New ObjectParameter("Predmet", GetType(String))
    '        Dim body As New ObjectParameter("Body", GetType(String))
    '        Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
    '        Using db As New Data4995Entities
    '            db.AGsp_GetPracakTextDoMailu(iDVykazPrace, emailKomu, predmet, body, lL_LastLapse)

    '            If lL_LastLapse.Value = 0 Then
    '                If Not String.IsNullOrWhiteSpace(emailKomu.Value) And Not String.IsNullOrWhiteSpace(predmet.Value) And Not String.IsNullOrWhiteSpace(body.Value) Then
    '                    Dim polozky = db.AGsp_FA_MailTextyPolozek(iDVykazPrace).ToList
    '                    Using mail As New MailMessage()
    '                        Dim html = <div><%= body.Value %>
    '                                       <table style="margin-top:20px;margin-bottom:20px;"></table>
    '                                       <table border="0" cellspacing="0" style="color:#231f20;width:700px;background:#ffffff;">
    '                                           <tr>
    '                                               <td valign="top" align="center" rowspan="4" style="border-right: 4px solid #ca2426;">
    '                                                   <img src="data:image/jpeg;base64,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"/>
    '                                               </td>
    '                                               <td valign="top" style="padding: 0 6px 0 6px;">
    '                                                   <b style="color:#ca2426;">Jaroslava Frolíková</b> | vedoucí technické podpory
    '    </td>
    '                                               <td rowspan="4" style="padding: 0 6px 0 6px;">
    '                                                   <img src="data:image/png;base64,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"/>
    '                                               </td>
    '                                           </tr>
    '                                           <tr>
    '                                               <td valign="bottom" style="padding: 0 6px 0 6px;">
    '                                                   <span style="color:#ca2426;">t:</span> +420 725 144 164
    '    </td>
    '                                           </tr>
    '                                           <tr>
    '                                               <td valign="bottom" style="padding: 0 6px 0 6px;">
    '                                                   <span style="color:#ca2426;">e:</span> frolikova@doctorum.cz | <span style="color:#ca2426;">w:</span> www.doctorum.cz
    '    </td>
    '                                           </tr>
    '                                           <tr>
    '                                               <td valign="bottom" style="padding: 0 6px 0 6px;">
    '                                                   <span style="color:#ca2426;">a:</span> Huťská 366, 272 01 Kladno
    '    </td>
    '                                           </tr>
    '                                       </table>
    '                                   </div>
    '                        html.<table>.First().Add(<tr style="background:silver;"><td style="width:120px;">Odpovědná osoba</td><td style="width:120px;">Množství</td><td>Forma zásahu</td><td>Popis</td></tr>)
    '                        For Each p In polozky
    '                            html.<table>.First().Add(<tr>
    '                                                         <td><%= p.TechnikOdpovednaOsoba %></td>
    '                                                         <td><%= String.Format("{0:0.0}", p.PocetEMJ) & " " & p.Jednotky %></td>
    '                                                         <td><%= p.FormaZasahu %></td>
    '                                                         <td><%= p.TextDoMailu %></td>
    '                                                     </tr>)
    '                        Next


    '                        mail.From = New MailAddress("podpora@doctorum.cz", "DOCTORUM.CZ")
    '                        mail.To.Add(New MailAddress(emailKomu.Value))
    '                        mail.Subject = predmet.Value
    '                        mail.Body = html.ToString
    '                        mail.IsBodyHtml = True

    '                        Dim smtp As SmtpClient = New SmtpClient()
    '                        smtp.Host = "smtp.forpsi.com"
    '                        smtp.EnableSsl = True

    '                        Dim networkCredential As NetworkCredential = New NetworkCredential("podpora@doctorum.cz", "Frolikova.321")
    '                        smtp.UseDefaultCredentials = True
    '                        smtp.Credentials = networkCredential
    '                        smtp.Port = 587
    '                        smtp.Send(mail)

    '                        Return New With {.data = Nothing, .error = Nothing}
    '                    End Using
    '                End If
    '            End If
    '            Return New With {.data = Nothing, .error = Nothing}
    '        End Using
    '    Catch ex As Exception
    '        While ex.InnerException IsNot Nothing
    '            ex = ex.InnerException
    '        End While
    '        Return New With {.data = Nothing, .error = ex.Message}
    '    End Try
    'End Function

    '<HttpGet>
    'Public Overridable Function AGsp_Get_TicketDetail(iDTicket As Nullable(Of Integer)) As Object
    '    Try
    '        Using db As New Data4995Entities
    '            Dim data = db.AGsp_Get_TicketDetail(iDTicket).FirstOrDefault
    '            Return New With {.data = data, .total = 1, .error = Nothing}
    '        End Using
    '    Catch ex As Exception
    '        While ex.InnerException IsNot Nothing
    '            ex = ex.InnerException
    '        End While
    '        Return New With {.data = Nothing, .total = 0, .error = ex.Message}
    '    End Try
    'End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_TicketHistorie(iDTicket As Nullable(Of Integer)) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_TicketHistorie(iDTicket).ToList
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_ITicketZapisDoHistorie(iDTicket As Nullable(Of Integer), komentar As String) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_ITicketZapisDoHistorie(iDTicket, komentar, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = Nothing, .error = "-"}
                End If
                Return New With {.data = Nothing, .total = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_IUTicket(model As AGsp_Get_TicketDetail_Result) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_IUTicket(model.IDTicket,
                                    model.Firma,
                                    model.EmailOdesilatele,
                                    model.EmailKopie,
                                    model.CasVytvoreni,
                                    model.IDUserVytvoril,
                                    model.IDUserResitel,
                                    model.CasResitelPrevzal,
                                    model.DatumDeadLine,
                                    model.Predmet,
                                    model.Telo,
                                    model.InterniPoznamka,
                                    model.rr_TicketStav,
                                    model.rr_TicketPriorita,
                                    lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = Nothing, .error = "-"}
                End If
                Return New With {.data = Nothing, .total = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_IU_SmlouvaServisni(model As AGsp_Get_SmlouvyServisniSeznam_Result1) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Dim newIDSmlouvy As New ObjectParameter("NewIDSmlouvy", GetType(Integer))
            Using db As New Data4995Entities
                Dim _user = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name)
                db.AGsp_Do_IU_SmlouvaServisni(model.IDSmlouvy, model.Firma, model.NazevSmlouvy, model.rr_TypServisniSmlouvy, model.rr_IntervalFakturaceSS, model.MesicniSazbaBezDPH, model.PlatiOd, model.UkoncenaKeDni, model.TextNafakturu, _user.IDUser, model.rr_FakturovatNaFirmu, newIDSmlouvy, lL_LastLapse)
                If Not IsDBNull(newIDSmlouvy.Value) Then
                    model.IDSmlouvy = newIDSmlouvy.Value
                End If
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = model, .total = 1, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_SmlouvyServisniSeznam(options As ODataQueryOptions(Of AGsp_Get_SmlouvyServisniSeznam_Result1)) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_SmlouvyServisniSeznam().ToList
                Dim cont = data.Count
                Dim queryable As IQueryable(Of AGsp_Get_SmlouvyServisniSeznam_Result1) = data.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_SmlouvyServisniSeznam_Result1)).ToList
                Return New With {.data = data, .total = cont, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_SmlouvyServisniSeznam_CBX() As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_SmlouvyServisniSeznam().Select(Function(e) New With {.value = e.IDSmlouvy, .text = e.NazevSmlouvy}).ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGvw_FirmyAPobocky_CBX() As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGvw_FirmyAPobocky().Select(Function(e) New With {.value = e.Firma, .text = e.Pobocka, .firma = e.Firma, .pobocka = e.Pobocka}).ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function PobockyFirmy(firma As String) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGvw_FirmyAPobocky.Where(Function(e) e.Firma = firma).Select(Function(e) New With {.value = e.Pobocka, .text = e.Pobocka}).ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGvwrr_TypServisniSmlouvy_CBX() As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGvwrr_TypServisniSmlouvy().Select(Function(e) New With {.value = e.rr_TypServisniSmlouvy, .text = e.rr_TypServisniSmlouvyHodnota}).ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_SmlouvyServisniDatumDalsihoObdobi_CBX() As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_SmlouvyServisniDatumDalsihoObdobi().Select(Function(e) New With {.value = e.Datum, .text = e.Obdobi}).ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGvwrr_IntervalFakturaceSS() As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGvwrr_IntervalFakturaceSS().Select(Function(e) New With {.value = e.rr_IntervalFakturaceSS, .text = e.rr_IntervalFakturaceSSText}).ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_VyuctovaniDoplnitUctovaciObdobi_CBX() As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGvwrr_TypSSProvize().Select(Function(e) New With {.value = e.rr_TypSSProvize, .text = e.rr_TypSSProvizeText}).ToList
                Return New With {.data = data, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_IU_SmlouvaServisniProvizeDef(model As AGsp_Get_SmlouvyServisniProvizeDefSeznam_Result) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Dim new_IDSSProvizeDef As New ObjectParameter("New_IDSSProvizeDef", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_IU_SmlouvaServisniProvizeDef(model.IDSSProvizeDef, model.IDSmlouvy, model.IDUserTechnika, model.rr_SSTypProvize, model.FixniCastka, model.ProcentoProvizeSS, new_IDSSProvizeDef, lL_LastLapse)
                If Not IsDBNull(new_IDSSProvizeDef.Value) Then
                    model.IDSSProvizeDef = new_IDSSProvizeDef.Value
                End If
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = model, .total = 1, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_SmlouvyServisniProvizeDefSeznam(options As ODataQueryOptions(Of AGsp_Get_SmlouvyServisniProvizeDefSeznam_Result)) As Object
        Try
            Dim idu = 0
            Using db As New Data4995Entities
                Dim _user = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name)
                If User.IsInRole("hanzl@agilo.cz") Or User.IsInRole("fakturace@agilo.cz") Or User.IsInRole("novak@agilo.cz") Then
                    idu = 0
                Else
                    idu = _user.IDUser
                End If
                Dim data = db.AGsp_Get_SmlouvyServisniProvizeDefSeznam(idu).ToList
                Dim cont = data.Count
                Dim queryable As IQueryable(Of AGsp_Get_SmlouvyServisniProvizeDefSeznam_Result) = data.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_SmlouvyServisniProvizeDefSeznam_Result)).ToList
                Return New With {.data = data, .total = cont, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_SmlouvyServisniGenerujMesicniPausalky(uctovanyMesic As Nullable(Of Date)) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim _user = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name)
                db.AGsp_Do_SmlouvyServisniGenerujMesicniPausalky(uctovanyMesic, _user.IDUser, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = Nothing, .total = 0, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_SmlouvyServisniGenerovanePausalky(options As ODataQueryOptions(Of AGsp_Get_SmlouvyServisniGenerovanePausalky_Result)) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_SmlouvyServisniGenerovanePausalky().ToList
                Dim cont = data.Count
                Dim queryable As IQueryable(Of AGsp_Get_SmlouvyServisniGenerovanePausalky_Result) = data.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_SmlouvyServisniGenerovanePausalky_Result)).ToList
                Return New With {.data = data, .total = cont, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_SmlouvyServisniPriznatTechnikoviProvize(iDMesicnihoVyuctovani As Nullable(Of Integer)) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities

            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_VyuctovaniDoplnitUctovaciObdobi() As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities

            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    'Public Overridable Function AGsp_GetFirmaParametryDlePracaku(iDVykazPrace As Nullable(Of Integer), hodinovaSazba As ObjectParameter, sazbaKm As ObjectParameter, rr_TypServisniSmlouvy As ObjectParameter, sazbaMalyZasah As ObjectParameter, sazbaVelkyZasah As ObjectParameter, parametryValidni As ObjectParameter) As Object
    '    Try
    '        Using db As New Data4995Entities
    '            db.AGsp_GetFirmaParametryDlePracaku(iDVykazPrace, hodinovaSazba, sazbaKm, rr_TypServisniSmlouvy, sazbaMalyZasah, sazbaVelkyZasah, parametryValidni)
    '            Return New With {.hodinovaSazba = hodinovaSazba.Value, .error = Nothing}
    '        End Using
    '    Catch ex As Exception
    '        While ex.InnerException IsNot Nothing
    '            ex = ex.InnerException
    '        End While
    '        Return New With {.error = ex.Message}
    '    End Try
    'End Function

    'HODINKY--------------------------------------------------------------------------------------------------------------------------------------------------------
    <HttpGet>
    Public Overridable Function UserLogin(login As String, password As String) As Object
        Dim q = HttpContext.Current.Request.QueryString
        Dim type = "json"
        If Not String.IsNullOrEmpty(q("type")) Then
            type = q("type")
        End If
        Try
            Using db As New Data4995Entities
                Dim acc = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = login)
                If type = "json" Then
                    If acc Is Nothing Then
                        Return New With {.data = Nothing, .total = 0, .error = "Nesprávné jméno nebo heslo"}
                    End If
                    Dim hashpwd As String = GetMd5Hash(login.ToLower & password)
                    If hashpwd <> acc.UserPWD Then
                        Return New With {.data = Nothing, .total = 0, .error = "Nesprávné jméno nebo heslo"}
                    End If
                    If Not acc.UserAccountEnabled Then
                        Return New With {.data = Nothing, .total = 0, .error = "Účet není povolen"}
                    End If
                    Return New With {.data = acc, .total = 0, .error = Nothing}
                Else
                    Dim xml As String = Nothing
                    If acc Is Nothing Then
                        xml = <result>
                                  <data></data>
                                  <total></total>
                                  <error>Nesprávné jméno nebo heslo</error>
                              </result>.ToString
                        Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
                    End If
                    Dim hashpwd As String = GetMd5Hash(login.ToLower & password)
                    If hashpwd <> acc.UserPWD Then
                        xml = <result>
                                  <data></data>
                                  <total></total>
                                  <error>Nesprávné jméno nebo heslo</error>
                              </result>.ToString
                        Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
                    End If
                    If Not acc.UserAccountEnabled Then
                        xml = <result>
                                  <data></data>
                                  <total></total>
                                  <error>Účet není povolen</error>
                              </result>.ToString
                        Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
                    End If
                    xml = <result>
                              <data><%= acc.IDUser %></data>
                              <total></total>
                              <error></error>
                          </result>.ToString
                    Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            If type = "json" Then
                Return New With {.data = Nothing, .total = 0, .error = ex.Message}
            Else
                Dim xml = <result>
                              <data></data>
                              <total></total>
                              <error><%= ex.Message %></error>
                          </result>.ToString
                Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
            End If
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_IUPraceNaProjektu(iDPraceNaProjektu As Nullable(Of Integer), iDUser As Nullable(Of Integer), iDProjektu As Nullable(Of Integer), placenaCinnost As Nullable(Of Boolean), mereniUkonceno As Nullable(Of Boolean), popisCinnosti As String) As Object
        Dim q = HttpContext.Current.Request.QueryString
        Dim type = "json"
        If Not String.IsNullOrEmpty(q("type")) Then
            type = q("type")
        End If
        Try
            Dim lL_LastLapseText As New ObjectParameter("LL_LastLapseText", GetType(String))
            Dim new_IDPraceNaProjektu As New ObjectParameter("New_IDPraceNaProjektu", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_IUPraceNaProjektu(iDPraceNaProjektu, iDUser, iDProjektu, placenaCinnost, mereniUkonceno, popisCinnosti, new_IDPraceNaProjektu, lL_LastLapseText)

                If type = "json" Then
                    If Not String.IsNullOrEmpty(lL_LastLapseText.Value) Then
                        Return New With {.data = Nothing, .total = 0, .error = lL_LastLapseText.Value}
                    End If
                    Return New With {.data = New With {.iDProjektu = new_IDPraceNaProjektu.Value}, .total = 0, .error = Nothing}
                Else
                    Dim xml As String = Nothing
                    If Not String.IsNullOrEmpty(lL_LastLapseText.Value) Then
                        xml = <result>
                                  <data></data>
                                  <total></total>
                                  <error><%= lL_LastLapseText.Value %></error>
                              </result>.ToString
                    End If
                    xml = <result>
                              <data><%= new_IDPraceNaProjektu.Value %></data>
                              <total></total>
                              <error></error>
                          </result>.ToString
                    Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
                End If

            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            If type = "json" Then
                Return New With {.data = Nothing, .total = 0, .error = ex.Message}
            Else
                Dim xml = <result>
                              <data></data>
                              <total></total>
                              <error><%= ex.Message %></error>
                          </result>.ToString
                Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
            End If
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_IUPraceNaProjektuProjekt(iDProjektu As Nullable(Of Integer), nazevProjektu As String, barvaProjektu As String, ukoncenyProjekt As Nullable(Of Boolean)) As Object
        Dim q = HttpContext.Current.Request.QueryString
        Dim type = "json"
        If Not String.IsNullOrEmpty(q("type")) Then
            type = q("type")
        End If
        If Not String.IsNullOrEmpty(barvaProjektu) Then
            barvaProjektu = "#" & barvaProjektu
        End If
        Try
            Dim lL_LastLapseText As New ObjectParameter("LL_LastLapseText", GetType(String))
            Dim new_IDProjektu As New ObjectParameter("New_IDProjektu", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_IUPraceNaProjektuProjekt(iDProjektu, nazevProjektu, ukoncenyProjekt, barvaProjektu, new_IDProjektu, lL_LastLapseText)

                If type = "json" Then
                    If Not String.IsNullOrEmpty(lL_LastLapseText.Value) Then
                        Return New With {.data = Nothing, .total = 0, .error = lL_LastLapseText.Value}
                    End If
                    Return New With {.data = New With {.iDProjektu = new_IDProjektu.Value}, .total = 0, .error = Nothing}
                Else
                    Dim xml As String = Nothing
                    If Not String.IsNullOrEmpty(lL_LastLapseText.Value) Then
                        xml = <result>
                                  <data></data>
                                  <total></total>
                                  <error><%= lL_LastLapseText.Value %></error>
                              </result>.ToString
                    End If
                    xml = <result>
                              <data><%= new_IDProjektu.Value %></data>
                              <total></total>
                              <error></error>
                          </result>.ToString
                    Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
                End If

            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            If type = "json" Then
                Return New With {.data = Nothing, .total = 0, .error = ex.Message}
            Else
                Dim xml = <result>
                              <data></data>
                              <total></total>
                              <error><%= ex.Message %></error>
                          </result>.ToString
                Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
            End If
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_PraceNaProjektuSeznamPoslednichProjektu(iDUser As Nullable(Of Integer)) As Object
        Dim q = HttpContext.Current.Request.QueryString
        Dim type = "json"
        If Not String.IsNullOrEmpty(q("type")) Then
            type = q("type")
        End If
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_PraceNaProjektuSeznamPoslednichProjektu(iDUser).ToList
                If type = "json" Then
                    Return New With {.data = data, .total = 0, .error = Nothing}
                Else
                    Dim xml = <result>
                                  <data><%= From i In data Select <item><IDProjektu><%= i.IDProjektu %></IDProjektu><NazevProjektu><%= i.NazevProjektu %></NazevProjektu></item> %></data>
                                  <total><%= data.Count %></total>
                                  <error></error>
                              </result>.ToString
                    Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            If type = "json" Then
                Return New With {.data = Nothing, .total = 0, .error = ex.Message}
            Else
                Dim xml = <result>
                              <data></data>
                              <total></total>
                              <error><%= ex.Message %></error>
                          </result>.ToString
                Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
            End If
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_PraceNaProjektuSeznamVsechProjektu() As Object
        Dim q = HttpContext.Current.Request.QueryString
        Dim type = "json"
        If Not String.IsNullOrEmpty(q("type")) Then
            type = q("type")
        End If
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_PraceNaProjektuSeznamVsechProjektu().ToList
                If type = "json" Then
                    Return New With {.data = data, .total = 0, .error = Nothing}
                Else
                    Dim xml = <result>
                                  <data><%= From i In data Select <item><IDProjektu><%= i.IDProjektu %></IDProjektu><NazevProjektu><%= i.NazevProjektu %></NazevProjektu><BarvaProjektu><%= i.Barva %></BarvaProjektu></item> %></data>
                                  <total><%= data.Count %></total>
                                  <error></error>
                              </result>.ToString
                    Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            If type = "json" Then
                Return New With {.data = Nothing, .total = 0, .error = ex.Message}
            Else
                Dim xml = <result>
                              <data></data>
                              <total></total>
                              <error><%= ex.Message %></error>
                          </result>.ToString
                Return New HttpResponseMessage() With {.Content = New Http.StringContent(xml, Encoding.UTF8, "application/xml")}
            End If
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_PraceNaProjektechGraficky(iDUser As Nullable(Of Integer), obdobiOd As Nullable(Of Date), obdobiDo As Nullable(Of Date)) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_PraceNaProjektechGraficky(iDUser, obdobiOd, obdobiDo).Select(Function(x) New With {
                            .IDPraceNaProjektu = x.IDPraceNaProjektu,
                            .IDProjektu = x.IDProjektu,
                            .Title = x.NazevProjektu,
                            .Start = x.CasZahajeni,
                            .End = x.CasUkonceni,
                            .StartTimezone = Nothing,
                            .EndTimezone = Nothing,
                            .Description = x.PopisCinnosti,
                            .RecurrenceId = Nothing,
                            .RecurrenceRule = Nothing,
                            .RecurrenceException = Nothing,
                            .IsAllDay = False,
                            .Color = x.Barva
                }).ToList
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function getGoogleCallendars() As Object
        Try
            Dim serverPath = System.Web.HttpContext.Current.Server.MapPath("/App_Data")
            Dim accEmail = "clientbox@clientbox-146911.iam.gserviceaccount.com"
            Dim keyPath = IO.Path.Combine(serverPath, "clientbox-146911-f38c96ec791e.p12")
            Dim scopes As String() = New String() {CalendarService.Scope.Calendar}
            Dim certificate = New X509Certificate2(keyPath, "notasecret", X509KeyStorageFlags.MachineKeySet Or X509KeyStorageFlags.PersistKeySet Or X509KeyStorageFlags.Exportable)
            Dim creditial As New ServiceAccountCredential(New ServiceAccountCredential.Initializer(accEmail) With {.Scopes = scopes, .ProjectId = "clientbox-146911"}.FromCertificate(certificate))
            Dim service As New CalendarService(New BaseClientService.Initializer With {.HttpClientInitializer = creditial, .ApplicationName = "clientbox"})
            Dim result = service.CalendarList.List.Execute
            Dim data = result.Items
            Return New With {.data = data, .total = data.Count, .error = Nothing}
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_TicketDetail(iDTicket As String) As Object
        Try
            If CInt(iDTicket) > 0 Then
                Using db As New Data4995Entities
                    Dim data = db.AGsp_Get_TicketSeznam(CInt(iDTicket), 0, 0).FirstOrDefault
                    Return New With {.data = data, .total = 1, .error = Nothing}
                End Using
            Else
                Return New With {.data = Nothing, .total = 0, .error = "Událost není v ticketech."}
            End If
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_TicketSeznam(iDUser As Integer, rr_TicketStav As Nullable(Of Byte)) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_TicketSeznam(0, iDUser, rr_TicketStav).OrderByDescending(Function(e) e.IDTicket).ToList
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_IUTicketRucne(model As AGsp_Get_TicketSeznam_Result) As Object
        Try
            Dim q = HttpContext.Current.Request.Form
            Dim action = 3
            If Not String.IsNullOrEmpty(q("savetype")) Then
                action = q("savetype")
            End If

            Dim cols As New StringCollection()
            Dim colors As String() = New String() {"#7986cb",
            "#33b679",
            "#8e24aa",
            "#e67c73",
            "#f6c026",
            "#f5511d",
            "#039be5",
            "#616161",
            "#3f51b5",
            "#0b8043",
            "#d60000"}
            cols.AddRange(colors)

            Dim IsNewTicket As Boolean = (model.IDTicket = 0)

            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Dim new_IDTicket As New ObjectParameter("New_IDTicket", GetType(Integer))
            Dim emailKlienta As New ObjectParameter("EmailKlienta", GetType(String))

            Using db As New Data4995Entities
                Dim iDUser = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                If action = 4 Then
                    deleteGEvent(model)
                    db.AGsp_Run_TicketXXto50(iDUser, model.IDTicket, lL_LastLapse)
                    If lL_LastLapse.Value > 0 Then
                        Return New With {.action = action, .data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                    End If
                    Return New With {.action = action, .data = Nothing, .total = 0, .error = Nothing}
                Else
                    If model.UdalostVGoogleCalend Then
                        Dim _colorId = 0
                        If Not String.IsNullOrEmpty(model.Barva) Then
                            _colorId = (cols.IndexOf(model.Barva) + 1)
                        End If
                        Dim _calendarID = "podpora@agilo.cz"
                        Select Case model.IDUserResitel
                            Case 6
                                _calendarID = "700nr6qvjl5a88nee07sl7oen8@group.calendar.google.com"
                            Case 7
                                _calendarID = "podpora@agilo.cz"
                            Case 8
                                _calendarID = "nezbeda@doctorum.cz"
                            Case 10
                                _calendarID = "0pq584pdtsp0i8ge18q6g9jb1s@group.calendar.google.com"
                        End Select
                        If model.IDTicket = 0 Then
                            'nový
                            db.AGsp_Do_IUTicketRucne(model.IDTicket, model.Firma, model.Pobocka, model.IDUserVytvoril, model.IDUserResitel, model.DomluvenyTerminCas, model.DomluvenyTerminCasDo, model.rr_DeadLine, model.DatumDeadLine, model.Predmet, model.Telo, model.InterniPoznamka, model.rr_TicketPriorita, model.UdalostVGoogleCalend, model.IDGoogleCaledar, model.IDGoogleEvent, model.rr_LokalitaBarva, model.Barva, model.rr_TypZasahu, model.OdesilatKlientoviEmaily, model.rr_FakturovatNaFirmu, new_IDTicket, emailKlienta, lL_LastLapse)
                            If lL_LastLapse.Value > 0 Then
                                Return New With {.action = action, .data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                            End If
                            model.IDTicket = new_IDTicket.Value
                            model.IDGoogleCaledar = _calendarID
                            model.IDGoogleEvent = insertGEvent(model, _calendarID, _colorId)
                        Else
                            'existujici
                            If String.IsNullOrEmpty(model.IDGoogleCaledar) Then
                                'ale neni jeste v kaledari
                                model.IDGoogleCaledar = _calendarID
                                model.IDGoogleEvent = insertGEvent(model, _calendarID, _colorId)
                            Else
                                If _calendarID <> model.IDGoogleCaledar Then
                                    'chci ho ridelit jinemu resiteli
                                    deleteGEvent(model)
                                    model.IDGoogleCaledar = _calendarID
                                    model.IDGoogleEvent = insertGEvent(model, _calendarID, _colorId)
                                Else
                                    updateGEvent(model, _colorId)
                                End If
                            End If
                        End If
                    Else
                        If Not String.IsNullOrEmpty(model.IDGoogleCaledar) Then
                            deleteGEvent(model)
                            model.IDGoogleCaledar = Nothing
                            model.IDGoogleEvent = Nothing
                            model.rr_LokalitaBarva = 0
                            model.rr_LokalitaBarvaText = "Určí google kalendář"
                            model.Barva = Nothing
                        End If
                    End If
                End If

                db.AGsp_Do_IUTicketRucne(model.IDTicket, model.Firma, model.Pobocka, model.IDUserVytvoril, model.IDUserResitel, model.DomluvenyTerminCas, model.DomluvenyTerminCasDo, model.rr_DeadLine, model.DatumDeadLine, model.Predmet, model.Telo, model.InterniPoznamka, model.rr_TicketPriorita, model.UdalostVGoogleCalend, model.IDGoogleCaledar, model.IDGoogleEvent, model.rr_LokalitaBarva, model.Barva, model.rr_TypZasahu, model.OdesilatKlientoviEmaily, model.rr_FakturovatNaFirmu, new_IDTicket, emailKlienta, lL_LastLapse)
                If model.IDTicket = 0 Then
                    model.IDTicket = new_IDTicket.Value
                End If
                If lL_LastLapse.Value > 0 Then
                    Return New With {.action = action, .data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If

                Dim ll As New ObjectParameter("LL_LastLapse", GetType(Integer))
                Dim id As New ObjectParameter("IDVykazPrace", GetType(Integer))
                Select Case action
                    Case 1 'ukoncit s pracakem
                        db.AGsp_Run_Ticket10to40(iDUser, model.IDTicket, True, id, ll)
                        If ll.Value > 0 Then
                            Return New With {.action = action, .data = Nothing, .total = 0, .error = lastLapse(ll.Value)}
                        End If
                        Dim resitel = db.AG_tblUsers.FirstOrDefault(Function(e) e.IDUser = model.IDUserResitel)
                        Dim title = "byl dokončen servisní případ #" & model.IDTicket
                        Dim data = db.AGsp_Get_TicketSeznam(model.IDTicket, 0, 0).FirstOrDefault
                        Dim subject = "Dokončení případu #" & model.IDTicket
                        Dim image = <img src="data:image/jpeg;base64,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"/>
                        Dim autotitle = "Automaticky generovaná zpráva servisní podpory AGILO.CZ s.r.o."
                        If model.rr_FakturovatNaFirmu = 2 Then
                            image = <img height="74" src="data:image/jpeg;base64,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sf8AahtJpfh7pN2ikxwaiA+O26N8E/iMfjV7RLb9oHWfEmmz+I7jTNC0i3uY5bq3gKFp41YFkBXeeQCPvAc16p4m8Oab4s8M3vh/Voy9peJtYrwyEHKsp9QQCPpQAnha/t9U8HaNqNo4aG4s4pFI90HH4dK2K+dNK8J/Hf4axyaP4Ql0/wAR6KHLQRzui+Xk5PDspXPUgMRnnua7zwCvxjuvEsuofEBrCy0sW7JHY2xQt5pZSGyu7gAMOX79KAOE8Qf8npeH/wDr2X/0RLXb/tA/8kQ1n/rpb/8Ao5Kpat4A8R3f7SWk+OYYITo1rCEkcygOD5Tr93r1YV6B408MQeMvBOqeGriXyVvYtqy4z5bghkbHfDKDigDI+En/ACRzwv8A9eS/1rt6+dfDOmftDeBtJXwvpWjaNqmnQMwguZ5lYRqSThf3iNjJJwyn+lfQ0HnfZovtGPO2Dft6bsc4/GgCSiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigDxnxZ+0Bo3h/WNS8O2Xh3Vb3W7SVoI0KIsMjjgEEMWI/wCA0nwR8Da1pkmr+O/F8LRa/rrlhFIMPFGzb23D+Es2Pl7BR7gFFAHs9FFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFebeP/jDofw71yHSdW0jUrqSe1FzFLaohQ5Zl2kswwflzxnqKKKAOK+A2j63feK/FnxD1bTpdOh1mVjbxyKQX3yGRiMgEqPlAPfn0r36iigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAP/2Q==" alt="DOCTORUM.CZ s.r.o."/>
                            autotitle = "Automaticky generovaná zpráva servisní podpory DOCTORUM.CZ s.r.o."
                        End If
                        Dim body = <div class="emailbody">
                                       <div style="width:800px;margin:0 auto;padding:1cm;border: 1px solid silver;font-size:16px;font-family:Arial;">
                                           <div class="contentbody">
                                               <div><%= autotitle %></div>
                                               <div style="margin-top:22px;">Vážený zákazníku,</div>
                                               <div><%= title %></div>
                                               <div style="margin-top:22px;"><i style="color:#808080">Zákazník: </i><b><%= model.Nazev_firmy %></b></div>
                                               <div><i style="color:#808080">Řešitel: </i><b><%= resitel.UserLastName & " - " %></b><%= If(model.rr_TypZasahu = 1, "provedl vzdálený zásah ", If(model.rr_TypZasahu = 2, "navštívil vaše pracoviště ", "")) %><b><%= model.DomluvenyTerminCas.Value.ToString("dddd d. MMMM HH:mm") %></b></div>
                                               <div><i style="color:#808080">Název případu: </i><%= model.Predmet %></div>
                                               <div style="margin-top:22px;"><i style="color:#808080">Popis problému:</i></div>
                                               <div style="margin-top:6px;"><%= model.Telo %></div>
                                               <table style="margin-top:22px;">
                                                   <tr>
                                                       <td style="padding-right:6px;">
                                                           <%= image %>
                                                       </td>
                                                       <td style="padding-left:6px;border-left:2px silver solid">
                                                           <div>S pozdravem,</div>
                                                           <div>Servisní podpora <%= If(model.rr_FakturovatNaFirmu = 1, "AGILO.CZ s.r.o.", "DOCTORUM.CZ s.r.o.") %></div>
                                                           <div>Servisní hotline: 725 144 164</div>
                                                           <div>e-mail: <%= If(model.rr_FakturovatNaFirmu = 1, <a href="mailto:podpora@agilo.cz">podpora@agilo.cz</a>, <a href="mailto:podpora@doctorum.cz">podpora@doctorum.cz</a>) %></div>
                                                       </td>
                                                   </tr>
                                               </table>
                                           </div>
                                       </div>
                                   </div>.ToString

                        If IsDBNull(emailKlienta.Value) Then
                            Return New With {.action = action, .data = Nothing, .total = 0, .error = Nothing}
                        Else
                            Return New With {.action = action, .data = New With {
                                        .id = id.Value,
                                        .action = action,
                                        .emailTo = emailKlienta.Value.ToString,
                                        .emailSubject = subject,
                                        .emailBody = body
                        }, .total = 0, .error = Nothing}
                        End If
                        'db.AGsp_Run_Ticket10to40(iDUser, model.IDTicket, True, id, ll)
                        'If ll.Value > 0 Then
                        '    Return New With {.action = action, .data = Nothing, .total = 0, .error = lastLapse(ll.Value)}
                        'End If
                        'Return New With {.action = action, .data = id.Value, .total = 0, .error = Nothing}
                    Case 2 'ukoncit bez pracaku
                        db.AGsp_Run_Ticket10to40(iDUser, model.IDTicket, False, id, ll)
                        If ll.Value > 0 Then
                            Return New With {.action = action, .data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                        End If
                        Dim resitel = db.AG_tblUsers.FirstOrDefault(Function(e) e.IDUser = model.IDUserResitel)
                        Dim title = "byl dokončen servisní případ #" & model.IDTicket
                        Dim data = db.AGsp_Get_TicketSeznam(model.IDTicket, 0, 0).FirstOrDefault
                        Dim subject = "Dokončení případu #" & model.IDTicket
                        Dim image = <img src="data:image/jpeg;base64,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"/>
                        Dim autotitle = "Automaticky generovaná zpráva servisní podpory AGILO.CZ s.r.o."
                        If model.rr_FakturovatNaFirmu = 2 Then
                            image = <img height="74" src="data:image/jpeg;base64,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sf8AahtJpfh7pN2ikxwaiA+O26N8E/iMfjV7RLb9oHWfEmmz+I7jTNC0i3uY5bq3gKFp41YFkBXeeQCPvAc16p4m8Oab4s8M3vh/Voy9peJtYrwyEHKsp9QQCPpQAnha/t9U8HaNqNo4aG4s4pFI90HH4dK2K+dNK8J/Hf4axyaP4Ql0/wAR6KHLQRzui+Xk5PDspXPUgMRnnua7zwCvxjuvEsuofEBrCy0sW7JHY2xQt5pZSGyu7gAMOX79KAOE8Qf8npeH/wDr2X/0RLXb/tA/8kQ1n/rpb/8Ao5Kpat4A8R3f7SWk+OYYITo1rCEkcygOD5Tr93r1YV6B408MQeMvBOqeGriXyVvYtqy4z5bghkbHfDKDigDI+En/ACRzwv8A9eS/1rt6+dfDOmftDeBtJXwvpWjaNqmnQMwguZ5lYRqSThf3iNjJJwyn+lfQ0HnfZovtGPO2Dft6bsc4/GgCSiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigDxnxZ+0Bo3h/WNS8O2Xh3Vb3W7SVoI0KIsMjjgEEMWI/wCA0nwR8Da1pkmr+O/F8LRa/rrlhFIMPFGzb23D+Es2Pl7BR7gFFAHs9FFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFebeP/jDofw71yHSdW0jUrqSe1FzFLaohQ5Zl2kswwflzxnqKKKAOK+A2j63feK/FnxD1bTpdOh1mVjbxyKQX3yGRiMgEqPlAPfn0r36iigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAP/2Q==" alt="DOCTORUM.CZ s.r.o."/>
                            autotitle = "Automaticky generovaná zpráva servisní podpory DOCTORUM.CZ s.r.o."
                        End If
                        Dim body = <div class="emailbody">
                                       <div style="width:800px;margin:0 auto;padding:1cm;border: 1px solid silver;font-size:16px;font-family:Arial;">
                                           <div class="contentbody">
                                               <div><%= autotitle %></div>
                                               <div style="margin-top:22px;">Vážený zákazníku,</div>
                                               <div><%= title %></div>
                                               <div style="margin-top:22px;"><i style="color:#808080">Zákazník: </i><b><%= model.Nazev_firmy %></b></div>
                                               <div><i style="color:#808080">Řešitel: </i><b><%= resitel.UserLastName & " - " %></b><%= If(model.rr_TypZasahu = 1, "provedl vzdálený zásah ", If(model.rr_TypZasahu = 2, "navštívil vaše pracoviště ", "")) %><b><%= model.DomluvenyTerminCas.Value.ToString("dddd d. MMMM HH:mm") %></b></div>
                                               <div><i style="color:#808080">Název případu: </i><%= model.Predmet %></div>
                                               <div style="margin-top:22px;"><i style="color:#808080">Popis problému:</i></div>
                                               <div style="margin-top:6px;"><%= model.Telo %></div>
                                               <table style="margin-top:22px;">
                                                   <tr>
                                                       <td style="padding-right:6px;">
                                                           <%= image %>
                                                       </td>
                                                       <td style="padding-left:6px;border-left:2px silver solid">
                                                           <div>S pozdravem,</div>
                                                           <div>Servisní podpora <%= If(model.rr_FakturovatNaFirmu = 1, "AGILO.CZ s.r.o.", "DOCTORUM.CZ s.r.o.") %></div>
                                                           <div>Servisní hotline: 725 144 164</div>
                                                           <div>e-mail: <%= If(model.rr_FakturovatNaFirmu = 1, <a href="mailto:podpora@agilo.cz">podpora@agilo.cz</a>, <a href="mailto:podpora@doctorum.cz">podpora@doctorum.cz</a>) %></div>
                                                       </td>
                                                   </tr>
                                               </table>
                                           </div>
                                       </div>
                                   </div>.ToString

                        If IsDBNull(emailKlienta.Value) Then
                            Return New With {.action = action, .data = Nothing, .total = 0, .error = Nothing}
                        Else
                            Return New With {.action = action, .data = New With {
                                        .id = id.Value,
                                        .action = action,
                                        .emailTo = emailKlienta.Value.ToString,
                                        .emailSubject = subject,
                                        .emailBody = body
                        }, .total = 0, .error = Nothing}
                        End If
                        'db.AGsp_Run_Ticket10to40(iDUser, model.IDTicket, False, id, ll)
                        'If ll.Value > 0 Then
                        '    Return New With {.action = action, .data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                        'End If
                        'Return New With {.action = action, .data = Nothing, .total = 0, .error = Nothing}
                    Case 3 'ulozit
                        Dim resitel = db.AG_tblUsers.FirstOrDefault(Function(e) e.IDUser = model.IDUserResitel)
                        Dim title = If(IsNewTicket, "byl založen servisní případ #" & model.IDTicket, "byl aktualizován servisní případ #" & model.IDTicket)
                        Dim data = db.AGsp_Get_TicketSeznam(model.IDTicket, 0, 0).FirstOrDefault
                        Dim subject = If(IsNewTicket, "Založen případ #" & model.IDTicket & " pro " & model.Nazev_firmy, "Aktualizace případu #" & model.IDTicket & " pro " & model.Nazev_firmy)
                        Dim image = <img src="data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEBLAEsAAD/4QAiRXhpZgAATU0AKgAAAAgAAQESAAMAAAABAAEAAAAAAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCAA5AMgDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD9/KKKKACis3xP4rtfCVrDNd+Zsmk8obF3EHBOSPTjt6iodO+IOi6qQIdRtwx4CyN5bH8GwaAPwB/ah/a5+L3/AA0L8QtNl+KfxIhs7DxNqllHZw+Jr2C3hjjvJkWNYkkCBVCgAY4Ara/4Jj/GTUrP/go78JNT8Qa5q2pLJrEtgHv76W4PmXdncWicux5LzqPqa53/AIKZfDmT4Wft+/FjTZA2281+bWom/hkS+C3oIPcA3BXjupHavF9D12+8La7Y6ppd1JY6ppdzFe2VzH9+2nicPHIvurqrD3FeI5OM9ejPxKpiKtDG80224Tvv/LL/AIB/UNRXi/7CP7aHh/8Abh+A2neKtJkht9Zt0S21/Sg37zSb0L86YzkxMctG5++hBOGDKvtFe1GSauj9ooVoVqaq03eL1TCiio7O8h1G0iuLeWOe3nQSRyRsGSRSMhgRwQRyCKZqSUUVl+KfG2i+BrJLnW9X0vR7eRtiy310lujN6AuQM+1AGpRVeHV7W4gtZI7q3eO+x9mdZAVuMqXGw5+bKgtxngE9BVigAooooAKKKq6zrdn4c0ua+1C7tbCyt13S3FxKsUUQ6ZZmIAH1oAtUVT0DxFp/ivSor7S76z1Kxmz5dxazLNFJjg4ZSQfwNXKACiis/WPFml+HbiCHUNS0+xmujthS4uEiaY+ihiM/hQJtLVmhRTGuY1uFhMiCWRS6oT8zKMAkD0G5cn3HrT6BhRRRQAUVHdXkNjA0k8scMa9Xdgqj8TXC+OPi9GsD2ukMZJGyrXOMKn+56n36fWgDH+MPiNdY8RLaxNuh08FWI7yH735YA+ua5BjheaWtrwB4cPifxRbwsu63hPnT+m0Hp+JwPoT6VnuB8I/8F5/2OLqz8H+DPjBpVq7pptnF4f8AEoRf9QryM9rcMAMBRLLLCzE5Jltx0HH5j1/T94z8G6X8RPCOp6Drlhb6po2tWsllfWdwm6K5hkUq6MPQqSK/D/8A4KL/APBJDxh+xtrOoeIvDNrf+K/heztLHfxKZrzQk5by7xRzsUAgXAGwgfPsYqG4cVQafPE/N+LMjqRqvG0VeL+K3R9/R9ezPnr9nf8AaS8afsp/Eu38WeBdam0bVoV8qZdvmW1/DnJguIj8skZ9DgqcMrKwDD9bf2Iv+C53gf8AaJ1bRfCnjjTLrwR441e4h061MMb3mlardSsscaRSKDJCzu3CSrtXKjzXJr8WFcOoZSGU8gjvX2T/AMELfgN/wuD9uux1y5hMml/DzT5takLLuja5cfZ7ZD6NmSSVT621Y4epNSUYnkcPZjjKWJhh8O9JNXT1Xm/LTXT5n6af8FTvif4k0D9m2L4e+ALk2vxP+OWpJ8P/AArOoY/2ZJdRSve6k2whlWy0+G9u9w/it0Xq4B8D/wCDa39qbU/iv+wtefCPxisln8SP2btXl8Ba3p07hri2tYHdLLcBwFjWOW0HJJbT3PcZ6S78WfFH9o//AIKQ+KviF8NvA/w/8ceEfgTaXHw30eXxN41uNAjTXLgW11rV1bCDSr5pdiCxsdzNF5b216g3+Y2z5Y1LxF4+/wCCZv8AwcAeD/iR8QvCvhHwL4J/a+tz4V1i18N+KLjX9Ni1iL7PFBdM8thYmKV52tAQ0bDbd3svmMSyp63W5+wrsfa3/Bbn/gpW3/BLz9hvVPGukWkOpeOvEF4nh7wnZzIZImv5Y5JDNIo5aOGGKWXaOHdI48r5gYJ+xb/wSc8E/Db4b6b4i+NGh6N8Zvjv4gtI7vxf4y8X2sWuXkl44LyWto06strZQlzFFDbrHHsjQlc5NfJP/B3x8L/EN7+yX8KfiRpFnNqGm/Dfxhu1SFVJjhW6iCwTSnGFj8+GODcf47tAM7q/VT4S/FHRPjh8LfDfjPw1eLqPh3xZpltrGmXS9Li2uIllifHbKMDjtR1DofMXiX/gll4W+G/7afwW+Knwl0mDwLpvhXXtRm8W+GNFn/s/QNVhutF1C0i1AaehFsL6G4niXzo0SR4rifez4QD42/4OfP2Zfh7rvxK/ZZ1u48FeGW1rxf8AFOy0LXNQTToo7rV7KVoQ8FxKqh5UwoADk7RnGMnO5/wUN0PXfDX/AAX6/Zj8B6P8TPjVovgj4p217qHiTQdP+JOvW2n3k1uLyZdkSXYEKMY41McWxNqABRk52P8Ag5YtFsNX/YxgjMjRw/GbSo1MkjSOQGjA3MxLMfUkknqSTSlsVHdHqf8AwXt/ZO+F9p/wR1+Ka2/w78EWY8FaKtz4fNtodtAdDk+2QuTalEBg3MW3CPaGDsDkMQe9/wCCGP7PPgT4Q/8ABMj4H614X8H+GtB1rxT4F0rUdY1Gx06KG81W4nto5pZJ5lUPITIxPzE44AwAAJP+C+f/ACh5+PX/AGLw/wDSmGuu/wCCO/8Ayij/AGcf+ycaF/6Qw0+pP2T1j9pf496N+yz+zz42+JHiITPovgXRLvXLuOEjzp0t4mk8qPcQDI5UIoJ5ZgO9fAn/AAR1+CH/AA8/+Dkn7Uv7Smn6X8SfEXjzV78eDvDesW63vh3wFpVtdNbLHY2MymJLh5reQtdspmeNYfmBMhk+j/8AgsL4Fn/aF/4J5/Gz4Y+HHkv/ABvq3gi81fT9Jto2lubxbVklWNVUHDTSIIkBxvYtjIRyvmP/AAbQ/E7S/iT/AMEavhTHp11bzXHhyTVNH1CKJtzWsyajcyIr+jPBLBKB/dmU96OodDgf+Cxn7N8X/BOL4LN+1R+zTpui/C/xx8N7+xk8TaNotitnoXj3SZrpbeW21GyhCRTOjXIkFwQJkjWTa4cRNH92fsk/tI6P+2B+zJ4E+KHh+OS30rx1ottq8VtKwaSyaVAZLdyOC8Um+NiONyHHFfNv/BxL8QdN+HX/AARt+NlxqU0MS6lptrpVursA0s1xe28ShQepXcXOOiozdFJrsv8Agif8DNc/Zx/4JVfBPwn4ktrix1y38PjULy0uEMc1k95NLe+RIrYKvGLgIynoyEdqOofZuU/+Cr37YviX9nD4e+EfB3w9aGP4m/FvVl0HQLiVAy6eC8UclwAQQzh7iCNQQQDNvIYJtb0D4Pf8E8PhX8LvCK2upeE9F8ceILyIf214l8T2Ueravr85A8yW4uLgPIwZskR7tiZwoAr5V/4LXWjfDj9qv9l74m6p5kfhDw34pii1S7YfurEx3tndcn+80MFww9rc1+jQOazj71SV+h4OHSr46uqyvycqin0TV216vr5W6Hzz8LP2M4fgD+2hH4q8I/bbPwFqng+80t9D+1s2m+HLtb2zliWxgZitvFOjTlooVWNGtwQMvgZvhb/gmr8EPBmi+KfE3xB8BfD3xBrWtaxq3ijXdb12xhvEh+03c9037y4XEUMMbhOAqgRljyWY/R1zrllZ6ra2M13axX18sj29u8oWWdY9u8oucsF3LkjpuGeor4L/AOCi3xc179t79obTP2UPhjqLW1vcFb/4ja5Au9NKskZHNtnufmjZlGAzyQRFgrThSajFXt6LzKxtPC4alzcik72jHT4pW0Xba/kr9DzH9iD9k3wN+3z+2hqHxh0f4b+FfBnwO+Hd2LHwrpmn6NDp48TX0Lb1u7hURWcIxEhDAAHyIvm8ucMV+jvgPwP4T/Ze+CVjomkw2vh/wf4L0whdxwltbxIXklkbqzHDO7nLMxZiSSTRRTjGC13HgcJQwdPlrOPPLVvRXfl5LZI868WWkll4lvoJmkkaGZlUyMWO0nK9fYis+vQPiV8PtQ1vxV9qsLXzo54l8xjIq7XGR3OfuhelV9H+B91O4a/u4YI+6QfO5/EgAfkaux7Jxum6bcaxfR21rE008h+VV/mfQD1Nflj/AMF7P+CuH7Rn/BPH9qNfhD8OdV8MeC9E1Pw5Y+IYNftdJS+1m9WZ54ZFZ7rzIERZbeVVCwBwuG3Zbj9u/DvhWx8LW3l2cIj3ffc/M8n1P9Ogr8Rv+Dyv4HeVF8C/ihDbhIlm1HwjqNwR995BFdWUefX91fkDvu4okrIuna+p+R/xP/bx+OXxqu5pvFnxm+K2viZi5guvFd8bVSeuyASiJB7IgHtXPeDv2lviZ8OdSW88O/Er4ieHrxeVuNL8T31lMv0eKVT+taHhn9jz4w+NtNW80X4Q/FjWrNhuW40/wbqV1Cw9Q8cJGPxrjvHXgXXvhbrMem+KdB1zwvqUmSlprGnzafcPjriOZVY4+lZHRoex/Dz9vnxFp3iETeO9MtfiFp8zf6UzTLpOrkHlnjvYY2VpmOCZLuC6zz8uSTX7x/8ABDvxL8JfjF+y18RH/Zv+JN1pHxE8QXFu2rQeMtGhvdZ8JRouyFJbSGWGO5T57l4rhHMJebDKWjkhr+aWu6/Zn/aW8bfsffHHQfiN8PNal0LxV4dm8y3mGWhuYzjzLaePI823kA2vGSMjBBVgrBRjFS5rHnf2ThI1vrEKaU11Stuf1uf8E7v2Q/E37DvwDh+Hut/ECD4jWOn3V1e2mqz6I1hq1xPeXlze3ct7L9plS4kkmuSQyxxEAHd5jNuHnP8AwVz/AOCV97/wVf8Ah14b8G33xCt/Avhrw3qsevRPZ+Hjeaq18kNzACty10iJCY7knYsW/fGreZj5a77/AIJk/wDBQvwr/wAFM/2TtF+JXhuP+zb5mOneIdFeUSS6DqcaqZrYsMb0+dZI3IUvFLGxVCxRfoKujRo6LtM8t8FfADVPEn7N918O/jPrehfF6HUrFtJ1O7l8PjTU1q0MSxt9qg86WNpnIdneLykJYbY028+Bfs4/8Ex/iP8AsCWV94b+Afxyh0/4XT3E13YeC/iH4Uk8V2/h2SWUySLYXcF9Y3McJJP7uZ5xlmfmR3dvs+imK58eaT/wSz1n4mft0eB/2hPjN8UIvGXjP4ZW01r4X0rwt4aXw3oVks0UscrTRzXN7dTufOZgTcqoIHy4yD5x/wAFif2IdY/bT+Lnwi0u5+J15pGqeG9an8T+CvDXhnwdBdancT2gtmmvLq4vL+O3aG3LRDJEC5ulQiV3jFfoVXzj+358DdA+PF/4MsfEfhfx8lvo66hqenfEDwPcyQ+IPAGoCOGGF7cW+65cXEc9wjIkM8TeUomiMfzLLWg09Tlf2jP2ePF//BUb9g248Dt8StJ8MaN44hkstev4vh9d2OoyCC+5jitbu/L2TboPKkSdZXwXx5TY2+J/BP4Q/F79lay8M/A3wl+03rN74b8D3Vr4EGrv8KdNurTw3dnT4ryz0+7mN4soZrSa1CS+U8ZM8EbyebIFMPiD4H/tA/EL4SeN7L4naPr/AIp8c+Jvhe+lfDnXdOtrfT5vD3ia31PXGtr6b7NK0ek6jc20+g3MtzDi3WWxmTePKhSS34y/Yp1K6+MvivxFffCS38X3i/HnQfFkl5FoFlFJrGmDwrp2l3t1D55Rdh1NLtnQsDhnmAZH3sD8j6k/Ys/Y68Sfs3eI/H3irx98UtY+MHj74gXVoLrXr/S4NLWy06ziZLTToLaAmKOGKSa8lGwLue7kZgXZnfgtD/4Jiaj+zP8AHDxb46/Zw+IFn8K1+Id0dR8U+D9b8OnxF4S1G+ww+3QWkdzaXFnctuIdobkRSBUDRHYpHlEP7D/xc8D/ABF0+x8PaLYf8Ip4Z1zX/h/a3Ru41ubrwf4hDai2oDeGLSaRcSWlpAjkPIkOo54uFc8F+1X+zJ8VPFQ+JkXg74ReL9AuNS8O/EjwjajT722lgvRdWEK+H5ftDXRleKRreB4UVIoNPDR24jX7O8zgj6b17/gmpq37Tfxa8L+LP2j/AIhWfxTsfAd8mreG/Beh+HT4d8I2eoKpC39zayXV5cXtygJCGa5MMYeQLCPMYmz/AMFJfFdx8Q/BerfD3wp41+JPgvxN4asrTxlr+q+C9Ne9u9H0gvdpG0scc8E8yzvaXWyG0Mk7taEeU6/I/l2tfsxapo/7VP8AZ9v8O/iVJ4N1qfRvFHgnXPDt1p8MPhvUEuWudSi1CW8JvbJ5ZyZriSHf9sgupYGErp5L9r/wUE+Adz8RvipeeKfC2kfFDwR8VvDPhJIfBXxF8DCKebVbuSa6k/sG/tXJguLNZobWYrqKLaqbpitxA+96Gk1ZmdajGrB057P1X5Wf3H0R8XPgr4U/aj+Btz4V8XaefEPh7XbSNpFlIjmYgB45ldNuyUMAwZMYPTjivm39hj4cfE66+AGh6h8MfjcLv4cTLNa6HpvxD8CDUtY0eC3nktzC1xaahbbwrRMF3qQqhQAFAQcno/wV+MXiTxjeSeMvDNxbfF23+JXhfxPpHizRpXOi2fhuOHSRrGn28+4vDbCKLWLT7C4zPLdJcBGM0s8fjVz+zP8AFzQ/2XfiDoVr8M/G9zrPjj4KeIvDek29sLdTY68mrard2bTF5lWKQx3ttLDKCTmNlUiRFQy4pu5jUwNGpP2kviStdNp27Npq69T7v0fwjN4V1DUvsfiPXvGnxJ8QXEnhe98UrZWtxD4KkOnyX0Re0Dxx29op8g+WoeWWS4tRK0gxInlv7MX/AATK8afsiw+Im8IfGWFtS8XX39o6zqmqeD477UNRl5I8yZrkFgGeV8H+OaRiSWNeUv8Asu/EXw38W9QuvB/gO68E+Irz42694pi8QCxtns2s9Q8G6tbWl3P9nZzNHFq19a+arqSJHkcB1SV15/xP+y58Q/E/hH4cahofgfxx4Taw0nwdZfEfSGupBd6rrdj4q0K8ubxJ4pC11Pb6ba6752oRyZulvYESW4kGyI9mm7mdTLcPOUZtO8b2d2nrvs931e59WePv2Rfip8XbOx0nxR8c2uPDK6ha3Oq6ZpvhC3sW1m3inSVrWSbzndI5Nm1tmMqxDblLKSvmXWP2bPiB4R1nWrXwl4J1JdP8N+OtfufCvhTVdDM/hfV9JvH0uU28EsUiy6Ncm4iupbS8QCG3P2rzY9skIYo5Y9UV/ZuHveSu/NuX5tn6TUUUVodQVHPZQ3UsMkkMckls5khZlDGJirLuU9jtZhkdmI6E1JRQAVifET4aeG/i/wCELzw/4s8P6J4o0HUF2XWm6vYxX1ncr6PFKrIw9iDW3RQB+JP/AAWK/wCDXzRP+EP1X4lfsw6Zc6fq2nrJd6n8Pllaa21GMfMzaYXJeKcfMfsxYxyAhYhEVCSfmv8A8ErP+CPfxO/4Kp/ESSLw/DJ4Y+Hui3Qt/EHjC+tma2snADNa28Z2m4vNpBMQIEYZTKyb4xJ/W9Xkf7Ev/JG9a/7H7xn/AOpRqtRyq5oqjSGfsR/sLfDX/gnt8DLHwB8MtCTStLt8TXt5MRLqGt3RUB7u7mwDLM2PQIihUjVI1RF9fooqzMKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigD//Z"/>
                        Dim autotitle = "Automaticky generovaná zpráva servisní podpory AGILO.CZ s.r.o."
                        If model.rr_FakturovatNaFirmu = 2 Then
                            image = <img height="74" src="data:image/jpeg;base64,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sf8AahtJpfh7pN2ikxwaiA+O26N8E/iMfjV7RLb9oHWfEmmz+I7jTNC0i3uY5bq3gKFp41YFkBXeeQCPvAc16p4m8Oab4s8M3vh/Voy9peJtYrwyEHKsp9QQCPpQAnha/t9U8HaNqNo4aG4s4pFI90HH4dK2K+dNK8J/Hf4axyaP4Ql0/wAR6KHLQRzui+Xk5PDspXPUgMRnnua7zwCvxjuvEsuofEBrCy0sW7JHY2xQt5pZSGyu7gAMOX79KAOE8Qf8npeH/wDr2X/0RLXb/tA/8kQ1n/rpb/8Ao5Kpat4A8R3f7SWk+OYYITo1rCEkcygOD5Tr93r1YV6B408MQeMvBOqeGriXyVvYtqy4z5bghkbHfDKDigDI+En/ACRzwv8A9eS/1rt6+dfDOmftDeBtJXwvpWjaNqmnQMwguZ5lYRqSThf3iNjJJwyn+lfQ0HnfZovtGPO2Dft6bsc4/GgCSiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigDxnxZ+0Bo3h/WNS8O2Xh3Vb3W7SVoI0KIsMjjgEEMWI/wCA0nwR8Da1pkmr+O/F8LRa/rrlhFIMPFGzb23D+Es2Pl7BR7gFFAHs9FFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFebeP/jDofw71yHSdW0jUrqSe1FzFLaohQ5Zl2kswwflzxnqKKKAOK+A2j63feK/FnxD1bTpdOh1mVjbxyKQX3yGRiMgEqPlAPfn0r36iigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAP/2Q==" alt="DOCTORUM.CZ s.r.o."/>
                            autotitle = "Automaticky generovaná zpráva servisní podpory DOCTORUM.CZ s.r.o."
                        End If
                        Dim body = <div class="emailbody">
                                       <div style="width:800px;margin:0 auto;padding:1cm;border: 1px solid silver;font-size:16px;font-family:Arial;">
                                           <div class="contentbody">
                                               <div><%= autotitle %></div>
                                               <div style="margin-top:22px;">Vážený zákazníku,</div>
                                               <div><%= title %></div>
                                               <div style="margin-top:22px;"><i style="color:#808080">Zákazník: </i><b><%= model.Nazev_firmy %></b></div>
                                               <div><i style="color:#808080">Řešitel: </i><b><%= resitel.UserLastName & " - " %></b><%= If(model.rr_TypZasahu = 1, "provede vzdálený zásah ", If(model.rr_TypZasahu = 2, "navštíví vaše pracoviště ", "")) %><b><%= model.DomluvenyTerminCas.Value.ToString("dddd d. MMMM HH:mm") %></b></div>
                                               <div><i style="color:#808080">Název případu: </i><%= model.Predmet %></div>
                                               <div style="margin-top:22px;"><i style="color:#808080">Popis problému:</i></div>
                                               <div style="margin-top:6px;"><%= model.Telo %></div>
                                               <table style="margin-top:22px;">
                                                   <tr>
                                                       <td style="padding-right:6px;">
                                                           <%= image %>
                                                       </td>
                                                       <td style="padding-left:6px;border-left:2px silver solid">
                                                           <div>S pozdravem,</div>
                                                           <div>Servisní podpora <%= If(model.rr_FakturovatNaFirmu = 1, "AGILO.CZ s.r.o.", "DOCTORUM.CZ s.r.o.") %></div>
                                                           <div>Servisní hotline: 725 144 164</div>
                                                           <div>e-mail: <%= If(model.rr_FakturovatNaFirmu = 1, <a href="mailto:podpora@agilo.cz">podpora@agilo.cz</a>, <a href="mailto:podpora@doctorum.cz">podpora@doctorum.cz</a>) %></div>
                                                       </td>
                                                   </tr>
                                               </table>
                                           </div>
                                       </div>
                                   </div>.ToString

                        If IsDBNull(emailKlienta.Value) Then
                            Return New With {.action = action, .data = Nothing, .total = 0, .error = Nothing}
                        Else
                            Return New With {.action = action, .data = New With {
                                        .id = 0,
                                        .action = action,
                                        .emailTo = emailKlienta.Value.ToString,
                                        .emailSubject = subject,
                                        .emailBody = body
                        }, .total = 0, .error = Nothing}
                        End If
                    Case Else
                        Return New With {.action = 0, .data = Nothing, .total = 0, .error = Nothing}
                End Select
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.action = 0, .data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function getGoogleEvents(<FromBody> calendars As String()) As Object
        Try
            Dim q = HttpContext.Current.Request.Form
            Dim weekdate = DateTime.Now
            If Not String.IsNullOrEmpty(q("weekdate")) Then
                weekdate = CDate(q("weekdate"))
            End If
            Dim serverPath = System.Web.HttpContext.Current.Server.MapPath("/App_Data")
            Dim accEmail = "clientbox@clientbox-146911.iam.gserviceaccount.com"
            Dim keyPath = IO.Path.Combine(serverPath, "clientbox-146911-f38c96ec791e.p12")
            Dim scopes As String() = New String() {CalendarService.Scope.Calendar}
            Dim certificate = New X509Certificate2(keyPath, "notasecret", X509KeyStorageFlags.MachineKeySet Or X509KeyStorageFlags.PersistKeySet Or X509KeyStorageFlags.Exportable)
            Dim creditial As New ServiceAccountCredential(New ServiceAccountCredential.Initializer(accEmail) With {.Scopes = scopes, .ProjectId = "clientbox-146911"}.FromCertificate(certificate))
            Dim service As New CalendarService(New BaseClientService.Initializer With {.HttpClientInitializer = creditial, .ApplicationName = "clientbox"})
            Dim pondeli = weekdate.StartOfWeek(DayOfWeek.Monday)
            Dim nedele = pondeli.AddDays(6)

            Dim allevents As New List(Of Object)
            For Each calendarId In calendars
                Dim request = service.Events.List(calendarId)
                request.TimeMin = pondeli
                request.TimeMax = nedele
                request.ShowDeleted = False

                Dim events = request.Execute()
                Dim data = events.Items.Select(Function(x) New With {
                                .Id = x.Id,
                                .IDT = If(x.ExtendedProperties IsNot Nothing, x.ExtendedProperties.Shared("IDTicket"), 0),
                                .Title = x.Summary,
                                .Start = x.Start.DateTime,
                                .End = x.End.DateTime,
                                .StartTimezone = Nothing,
                                .EndTimezone = Nothing,
                                .Description = x.Description,
                                .RecurrenceId = Nothing,
                                .RecurrenceRule = Nothing,
                                .RecurrenceException = Nothing,
                                .IsAllDay = False,
                                .ColorId = x.ColorId,
                                .CalendarId = calendarId,
                                .Location = x.Location
                    }).ToList

                allevents.AddRange(data)
            Next

            Return New With {.data = allevents, .total = allevents.Count, .error = Nothing}
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    Public Function insertGEvent(model As AGsp_Get_TicketSeznam_Result, _calendarID As String, _colorID As Integer) As String
        Dim serverPath = System.Web.HttpContext.Current.Server.MapPath("/App_Data")
        Dim accEmail = "clientbox@clientbox-146911.iam.gserviceaccount.com"
        Dim keyPath = IO.Path.Combine(serverPath, "clientbox-146911-f38c96ec791e.p12")
        Dim scopes As String() = New String() {CalendarService.Scope.Calendar}
        Dim certificate = New X509Certificate2(keyPath, "notasecret", X509KeyStorageFlags.MachineKeySet Or X509KeyStorageFlags.PersistKeySet Or X509KeyStorageFlags.Exportable)
        Dim creditial As New ServiceAccountCredential(New ServiceAccountCredential.Initializer(accEmail) With {.Scopes = scopes, .ProjectId = "clientbox-146911"}.FromCertificate(certificate))
        Dim service As New CalendarService(New BaseClientService.Initializer With {.HttpClientInitializer = creditial, .ApplicationName = "clientbox"})
        Dim ep As New Data.Event.ExtendedPropertiesData()
        ep.Shared = New System.Collections.Generic.Dictionary(Of String, String)
        ep.Shared.Add("IDTicket", model.IDTicket)

        Using db As New Data4995Entities
            Dim data = db.AGsp_Get_TicketSeznam(model.IDTicket, 0, 0).FirstOrDefault
            Dim summary = "⚑" & If(Not String.IsNullOrEmpty(data.Telo), "★ - ", " - ") & data.rr_TypZasahuText & " - " & data.Nazev_pobocka & " - " & data.Predmet
            Dim location = New String() {data.PobockaUlice, data.PobockaPSC & " " & data.PobockaMesto}
            Dim myEvent = New [Event] With {
        .Summary = summary,
        .Description = data.Telo,
        .ColorId = _colorID,
        .ICalUID = data.IDTicket,
        .ExtendedProperties = ep,
        .Location = String.Join(", ", location),
        .Start = New EventDateTime With {
            .DateTime = data.DomluvenyTerminCas
        },
        .[End] = New Google.Apis.Calendar.v3.Data.EventDateTime With {
            .DateTime = data.DomluvenyTerminCas.Value.AddHours(1)
          }
       }
            Dim request = service.Events.Import(myEvent, _calendarID).Execute()
            Return request.Id
        End Using
    End Function

    Public Sub updateGEvent(model As AGsp_Get_TicketSeznam_Result, _colorID As Integer)
        Dim serverPath = System.Web.HttpContext.Current.Server.MapPath("/App_Data")
        Dim accEmail = "clientbox@clientbox-146911.iam.gserviceaccount.com"
        Dim keyPath = IO.Path.Combine(serverPath, "clientbox-146911-f38c96ec791e.p12")
        Dim scopes As String() = New String() {CalendarService.Scope.Calendar}
        Dim certificate = New X509Certificate2(keyPath, "notasecret", X509KeyStorageFlags.MachineKeySet Or X509KeyStorageFlags.PersistKeySet Or X509KeyStorageFlags.Exportable)
        Dim creditial As New ServiceAccountCredential(New ServiceAccountCredential.Initializer(accEmail) With {.Scopes = scopes, .ProjectId = "clientbox-146911"}.FromCertificate(certificate))
        Dim service As New CalendarService(New BaseClientService.Initializer With {.HttpClientInitializer = creditial, .ApplicationName = "clientbox"})
        Dim ep As New Data.Event.ExtendedPropertiesData()
        ep.Shared = New System.Collections.Generic.Dictionary(Of String, String)
        ep.Shared.Add("IDTicket", model.IDTicket)

        Using db As New Data4995Entities
            Dim data = db.AGsp_Get_TicketSeznam(model.IDTicket, 0, 0).FirstOrDefault
            Dim summary = "⚑" & If(Not String.IsNullOrEmpty(data.Telo), "★ - ", " - ") & data.rr_TypZasahuText & " - " & data.Nazev_pobocka & " - " & data.Predmet
            Dim location = New String() {data.PobockaUlice, data.PobockaPSC & " " & data.PobockaMesto}
            Dim myEvent = New [Event] With {
       .Summary = summary,
       .ColorId = _colorID,
       .Description = data.Telo,
       .ExtendedProperties = ep,
       .Location = String.Join(", ", location),
       .Start = New EventDateTime With {
           .DateTime = data.DomluvenyTerminCas
       },
       .[End] = New Google.Apis.Calendar.v3.Data.EventDateTime With {
           .DateTime = data.DomluvenyTerminCas.Value.AddHours(1)
         }
      }
            service.Events.Update(myEvent, model.IDGoogleCaledar, model.IDGoogleEvent).Execute()
        End Using
    End Sub

    Public Sub deleteGEvent(model As AGsp_Get_TicketSeznam_Result)
        Try
            Dim serverPath = System.Web.HttpContext.Current.Server.MapPath("/App_Data")
            Dim accEmail = "clientbox@clientbox-146911.iam.gserviceaccount.com"
            Dim keyPath = IO.Path.Combine(serverPath, "clientbox-146911-f38c96ec791e.p12")
            Dim scopes As String() = New String() {CalendarService.Scope.Calendar}
            Dim certificate = New X509Certificate2(keyPath, "notasecret", X509KeyStorageFlags.MachineKeySet Or X509KeyStorageFlags.PersistKeySet Or X509KeyStorageFlags.Exportable)
            Dim creditial As New ServiceAccountCredential(New ServiceAccountCredential.Initializer(accEmail) With {.Scopes = scopes, .ProjectId = "clientbox-146911"}.FromCertificate(certificate))
            Dim service As New CalendarService(New BaseClientService.Initializer With {.HttpClientInitializer = creditial, .ApplicationName = "clientbox"})
            If Not String.IsNullOrEmpty(model.IDGoogleCaledar) And Not String.IsNullOrEmpty(model.IDGoogleEvent) Then
                service.Events.Delete(model.IDGoogleCaledar, model.IDGoogleEvent).Execute()
            End If
        Catch ex As Exception

        End Try
    End Sub

    <HttpGet>
    Public Overridable Function AGsp_Do_UFirmaZneviditelnitFirmu(zneviditelnit As Nullable(Of Boolean), firma As String) As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_Do_UFirmaZneviditelnitFirmu(zneviditelnit, firma)
            End Using
            Return New With {.data = Nothing, .total = 0, .error = Nothing}
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_LokalitaBarvaProGoogleKal() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim iDUser = 0
            If Not String.IsNullOrEmpty(q("iDUser")) Then
                iDUser = CInt(q("iDUser"))
            End If
            Using db As New Data4995Entities
                If iDUser > 0 Then
                    Dim data = db.AGsp_Get_LokalitaBarvaProGoogleKal(iDUser).Select(Function(e) New With {.value = e.rr_LokalitaBarva, .text = e.Popis, .color = e.BarvaHEX}).ToList()
                    Return New With {.data = data, .total = data.Count, .error = Nothing}
                Else
                    Dim data = New List(Of Object)
                    data.Add(New With {.value = 0, .text = "Určí google kalendář", .color = Nothing})
                    Return New With {.data = data, .total = 1, .error = Nothing}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_TicketTextTicketu(iDVykazPrace As Nullable(Of Integer)) As Object
        Try
            Dim textNaPracak As New ObjectParameter("TextNaPracak", GetType(String))
            Using db As New Data4995Entities
                db.AGsp_Get_TicketTextTicketu(iDVykazPrace, textNaPracak)
                Return New With {.data = textNaPracak.Value, .total = 0, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_ProduktRezervace(iDPrijemkaPol As Nullable(Of Integer), pocetEMJRezervovat As Nullable(Of Decimal), rezervovat As Nullable(Of Boolean), firma_RezervovatProFirmu As String) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim iDUser = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                db.AGsp_Do_ProduktRezervace(iDPrijemkaPol, pocetEMJRezervovat, rezervovat, firma_RezervovatProFirmu, iDUser, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = Nothing, .total = 0, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_GetProduktRezervovaneProduktyFirmy(firma As String) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_GetProduktRezervovaneProduktyFirmy(firma, "").ToList
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_DelNaseWeby(model As AGsp_Get_NaseWebySeznam_Result) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_DelNaseWeby(model.IDWebu, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = Nothing, .total = 0, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_IUNaseWeby(model As AGsp_Get_NaseWebySeznam_Result) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Dim new_IDWebu As New ObjectParameter("New_IDWebu", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_IUNaseWeby(model.IDWebu, model.NazevWebu, model.AdresaWebu, new_IDWebu, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                If Not IsDBNull(new_IDWebu.Value) Then
                    model.IDWebu = new_IDWebu.Value
                End If
                Return New With {.data = model, .total = 1, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_NaseWebySeznam() As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_NaseWebySeznam.OrderBy(Function(e) e.NazevWebu).ToList
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_IU_Doklad(model As AGsp_Get_DokladHLSeznam_Result) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Dim new_IDDokladu As New ObjectParameter("New_IDDokladu", GetType(Integer))
            Dim q = HttpContext.Current.Request.QueryString
            Dim rr_TypDokladu = 1
            If Not String.IsNullOrEmpty(q("rr_TypDokladu")) Then
                rr_TypDokladu = CInt(q("rr_TypDokladu"))
            End If
            Using db As New Data4995Entities
                Dim iDUser = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                db.AGsp_Do_IU_Doklad(model.IDDokladu, rr_TypDokladu, model.VSDokladu, model.FirmaDodavatel, iDUser, new_IDDokladu, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = new_IDDokladu.Value, .total = 0, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_DokladHLSeznam() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Using db As New Data4995Entities
                If Not String.IsNullOrEmpty(q("filter")) Then
                    Dim vs = q("filter")
                    Dim data = db.AGsp_Get_DokladHLSeznam().Where(Function(e) e.VSDokladu = vs).OrderByDescending(Function(e) e.IDDokladu).ToList
                    Return New With {.data = data, .total = data.Count, .error = Nothing}
                Else
                    Dim data = db.AGsp_Get_DokladHLSeznam().OrderByDescending(Function(e) e.IDDokladu).ToList
                    Return New With {.data = data, .total = data.Count, .error = Nothing}
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_PrijemkaPolSeznam() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim iDDokladu = 0
            If Not String.IsNullOrEmpty(q("iDDokladu")) Then
                iDDokladu = CInt(q("iDDokladu"))
            End If
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_PrijemkaPolSeznam(iDDokladu).ToList
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_DelPrijemkaProdukt(iDPrijemkaPol As Nullable(Of Integer)) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_DelPrijemkaProdukt(iDPrijemkaPol, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = Nothing, .total = 0, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Do_IPrijemkaProduktNaskladnit(iDDokladu As Nullable(Of Integer),
                                                                   rr_DruhNakladu As Nullable(Of Byte),
                                                                   produkt As String,
                                                                   datumPrijmu As Nullable(Of Date),
                                                                   naskladnenoEMJ As Nullable(Of Decimal),
                                                                   cenaNakup As Nullable(Of Decimal),
                                                                   cenaProdejni As Nullable(Of Decimal),
                                                                   vSPrijmovehoDokladu As String) As Object
        Try
            If iDDokladu > 0 Then
                Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
                Dim iDPrijemkaPolNEW As New ObjectParameter("IDPrijemkaPolNEW", GetType(Integer))
                Using db As New Data4995Entities
                    Dim iDUser = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                    db.AGsp_Do_IPrijemkaProduktNaskladnit(iDDokladu, rr_DruhNakladu, produkt, datumPrijmu, naskladnenoEMJ, cenaNakup, cenaProdejni, vSPrijmovehoDokladu, 0, iDUser, 0, iDPrijemkaPolNEW, lL_LastLapse)
                    If lL_LastLapse.Value > 0 Then
                        Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                    End If
                    Return New With {.data = iDPrijemkaPolNEW.Value, .total = 1, .error = Nothing}
                End Using
            Else
                Return New With {.data = Nothing, .total = 0, .error = "IDDokladu nemůže být 0. Zkus refrešnout prohlížeč, vybrat příjemku a znovu přidat položku."}
            End If
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_DashboardUcty() As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim id = Nothing
            If Not String.IsNullOrEmpty(q("id")) Then
                id = CInt(q("id"))
            End If
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_DashboardUcty(id, lL_LastLapse).ToList
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    Private Structure Pohyb
        Dim iDPohybu As String
        Dim iDBankovnihoUctu As Nullable(Of Integer)
        Dim datumPohybu As Nullable(Of Date)
        Dim objem As Nullable(Of Decimal)
        Dim vS As String
        Dim typ As String
    End Structure

    <HttpGet>
    Public Async Function GetFioTransactions() As Tasks.Task(Of Object)
        Try
            Using db As New Data4995Entities
                Dim _ucty = db.AG_tblIBBakovniUcty.ToList
                For Each _ucet In _ucty
                    Using client As New HttpClient()
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls Or SecurityProtocolType.Ssl3

                        Dim lL_LastLapse1 As New ObjectParameter("LL_LastLapse", GetType(Integer))
                        Dim posledniDatum As New ObjectParameter("PosledniDatum", GetType(Date))
                        db.AGsp_Ge_IBPosledniDatumVypisu(_ucet.IDBankovnihoUctu, posledniDatum, lL_LastLapse1)
                        If lL_LastLapse1.Value > 0 Then
                            Return lastLapse(lL_LastLapse1.Value)
                        End If

                        Dim _start = CDate(posledniDatum.Value).ToString("yyyy-MM-dd")
                        Dim _end = Now.ToString("yyyy-MM-dd")

                        Dim url = String.Format("https://www.fio.cz/ib_api/rest/periods/{0}/{1}/{2}/transactions.xml", _ucet.Token, _start, _end)
                        Dim response = Await client.GetAsync(url)
                        Dim result = Await response.Content.ReadAsStringAsync()
                        Dim doc As XDocument = XDocument.Parse(result)
                        Dim transactions = doc.Root.<TransactionList>.<Transaction>.ToList

                        For Each transaction In transactions
                            Dim p As New Pohyb
                            Dim column_22 = transaction.<column_22>.FirstOrDefault
                            If column_22 IsNot Nothing Then
                                p.iDPohybu = column_22.Value
                            End If
                            Dim column_0 = transaction.<column_0>.FirstOrDefault
                            If column_0 IsNot Nothing Then
                                p.datumPohybu = column_0.Value
                            End If
                            Dim column_1 = transaction.<column_1>.FirstOrDefault
                            If column_1 IsNot Nothing Then
                                p.objem = column_1.Value.Replace(".", ",")
                            End If
                            Dim column_5 = transaction.<column_5>.FirstOrDefault
                            If column_5 IsNot Nothing Then
                                p.vS = column_5.Value
                            End If
                            Dim column_8 = transaction.<column_8>.FirstOrDefault
                            If column_8 IsNot Nothing Then
                                p.typ = column_8.Value
                            End If

                            Dim lL_LastLapse2 As New ObjectParameter("LL_LastLapse", GetType(Integer))
                            db.AGsp_Do_IBankaPohyb(p.iDPohybu, _ucet.IDBankovnihoUctu, p.datumPohybu, p.objem, p.vS, p.typ, lL_LastLapse2)
                            If lL_LastLapse2.Value > 0 Then
                                Return lastLapse(lL_LastLapse2.Value)
                            End If
                        Next
                    End Using
                Next
            End Using
            Return Nothing
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return ex.Message
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_BackupProfileSeznam(options As ODataQueryOptions(Of AGsp_Get_BackupProfileSeznam_Result)) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_BackupProfileSeznam().OrderByDescending(Function(e) e.DatumZalozeni).ToList
                Dim cont = data.Count
                Dim queryable As IQueryable(Of AGsp_Get_BackupProfileSeznam_Result) = data.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_BackupProfileSeznam_Result)).ToList
                Return New With {.data = data, .total = cont, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_DoctorumBackupProfile(options As ODataQueryOptions(Of AGsp_Get_DoctorumBackupProfile_Result)) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_DoctorumBackupProfile().OrderByDescending(Function(e) e.DatumZalozeni).ToList
                Dim cont = data.Count
                Dim queryable As IQueryable(Of AGsp_Get_DoctorumBackupProfile_Result) = data.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_DoctorumBackupProfile_Result)).ToList
                Return New With {.data = data, .total = cont, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    'Public Overridable Function AGsp_Do_SpocitejProviziTechnika_G2(iDVykazPracePol As Nullable(Of Integer), lL_LastLapse As ObjectParameter) As Integer
    '    Dim iDVykazPracePolParameter As ObjectParameter = If(iDVykazPracePol.HasValue, New ObjectParameter("IDVykazPracePol", iDVykazPracePol), New ObjectParameter("IDVykazPracePol", GetType(Integer)))

    '    Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction("AGsp_Do_SpocitejProviziTechnika_G2", iDVykazPracePolParameter, lL_LastLapse)
    'End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_DoctorumBackupProfile_G2(options As ODataQueryOptions(Of AGsp_Get_DoctorumBackupProfile_G2_Result)) As Object
        Try
            Dim qs = HttpContext.Current.Request.QueryString
            Dim rr_BackupFilterProfile = 0
            If Not String.IsNullOrEmpty(qs("rr_BackupFilterProfile")) Then
                rr_BackupFilterProfile = qs("rr_BackupFilterProfile")
            End If
            Dim hledej = ""
            If Not String.IsNullOrEmpty(qs("hledej")) Then
                hledej = qs("hledej")
            End If
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_DoctorumBackupProfile_G2(rr_BackupFilterProfile, hledej).OrderByDescending(Function(e) e.DatumZalozeni).ToList
                Dim cont = data.Count
                Dim queryable As IQueryable(Of AGsp_Get_DoctorumBackupProfile_G2_Result) = data.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_DoctorumBackupProfile_G2_Result)).ToList
                Return New With {.data = data, .total = cont, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Function AGvwrr_BackupFilterProfile() As Object
        Try
            Using db As New Data4995Entities
                Dim dt = db.AGvwrr_BackupFilterProfile.Select(Function(e) New With {.index = e.rr_BackupFilterProfile, .value = e.rr_BackupFilterProfileHodnota}).ToList
                Return New With {.data = dt, .total = dt.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Function AGsp_GetSBGrafDostupnosti(iDBackupProfile As Nullable(Of Integer)) As Object
        Try
            Using db As New Data4995Entities
                Dim dt = db.AGsp_GetSBGrafDostupnosti(iDBackupProfile).ToList
                Return dt
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return Nothing
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_BackupZalohyJednohoProfilu(iDBackupProfile As Nullable(Of Integer)) As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_BackupZalohyJednohoProfilu(iDBackupProfile).OrderByDescending(Function(e) e.IDBackup).ToList
                Dim cont = data.Count
                Dim backuptimes As New List(Of Object)
                If cont > 0 Then
                    Dim doc = XDocument.Parse(data.FirstOrDefault.MetadataXML)
                    Dim times = doc.Root.<backuptime>
                    For Each r In times
                        Dim d = r.<day>.Value
                        Dim t = r.<time>.Value
                        backuptimes.Add(New With {.day = d, .time = CDate(t).ToString("HH:mmm")})
                    Next
                End If

                Return New With {.data = data, .total = cont, .error = Nothing, .backuptimes = backuptimes}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_BackupXMLConfig(iDBackupProfile As Nullable(Of Integer)) As Object
        Try
            Dim xMLConfig As New ObjectParameter("XMLConfig", GetType(String))
            Using db As New Data4995Entities
                db.AGsp_Get_BackupXMLConfig(iDBackupProfile, xMLConfig)

                Dim xe As XElement = XElement.Parse(xMLConfig.Value)
                If xe.Elements.Count > 0 Then
                    Dim json = New With {
                .starthour = xe.<starthour>.First.Value,
                .interval = xe.<interval>.First.Value,
                .timerpause = xe.<timerpause>.First.Value,
                .notification = xe.<notification>.First.Value,
                .password = xe.<password>.First.Value,
                .backup = xe.<backup>.First.Elements.Select(Function(e) New With {.item = New With {.type = e.<type>.Value, .description = e.<description>.Value, .extensions = e.<extensions>.Value, .path = e.<path>.Value}}),
                .processes = xe.<processes>.First.Elements.Select(Function(e) New With {.process = New With {.path = e.<path>.Value, .args = e.<args>.Value}}),
                .sendemail = New With {
                .address = xe.<sendemail>.<address>.First.Elements.Select(Function(e) New With {.to = e.Value}),
                .subject = xe.<sendemail>.<subject>.First.Value,
                .body = xe.<sendemail>.<body>.First.Value
                }
                }
                    Return json
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_UDoctorumBackupProfile() As Object
        Try
            Dim q = HttpContext.Current.Request.Form
            Dim IDBackupProfile = 0
            If Not String.IsNullOrEmpty(q("IDBackupProfile")) Then
                IDBackupProfile = CInt(q("IDBackupProfile"))
            End If
            Dim Firma = Nothing
            If Not String.IsNullOrEmpty(q("Firma")) Then
                Firma = q("Firma")
            End If
            Dim PopisZarizeni = Nothing
            If Not String.IsNullOrEmpty(q("PopisZarizeni")) Then
                PopisZarizeni = q("PopisZarizeni")
            End If
            Dim ServerovaSlozka = Nothing
            If Not String.IsNullOrEmpty(q("ServerovaSlozka")) Then
                ServerovaSlozka = q("ServerovaSlozka")
            End If
            Dim rr_StavBackupProfile = 1
            If Not String.IsNullOrEmpty(q("rr_StavBackupProfile")) Then
                rr_StavBackupProfile = CInt(q("rr_StavBackupProfile"))
            End If
            Dim DovolenaDo = Nothing
            If Not String.IsNullOrEmpty(q("DovolenaDo")) Then
                DovolenaDo = CDate(q("DovolenaDo"))
            End If
            Dim Poznamka = Nothing
            If Not String.IsNullOrEmpty(q("Poznamka")) Then
                Poznamka = q("Poznamka")
            End If
            Dim DoKdyZaplaceno = Nothing
            If Not String.IsNullOrEmpty(q("DoKdyZaplaceno")) Then
                DoKdyZaplaceno = CDate(q("DoKdyZaplaceno"))
            End If
            Dim PosledniFaktura = Nothing
            If Not String.IsNullOrEmpty(q("PosledniFaktura")) Then
                PosledniFaktura = q("PosledniFaktura")
            End If
            Dim rr_AmbulSW = 0
            If Not String.IsNullOrEmpty(q("rr_AmbulSW")) Then
                rr_AmbulSW = CInt(q("rr_AmbulSW"))
            End If
            Dim KdoToJe = ""
            If Not String.IsNullOrEmpty(q("KdoToJe")) Then
                KdoToJe = q("KdoToJe")
            End If
            Dim Telefon = ""
            If Not String.IsNullOrEmpty(q("Telefon")) Then
                Telefon = q("Telefon")
            End If
            Dim Email = ""
            If Not String.IsNullOrEmpty(q("Email")) Then
                Email = q("Email")
            End If
            Dim FakturovatNaICO = ""
            If Not String.IsNullOrEmpty(q("FakturovatNaICO")) Then
                FakturovatNaICO = q("FakturovatNaICO")
            End If
            Dim SBvRezimuServer = False
            If Not String.IsNullOrEmpty(q("SBvRezimuServer")) Then
                SBvRezimuServer = q("SBvRezimuServer")
            End If
            Dim SmluvniCenaMaintenance = 0
            If Not String.IsNullOrEmpty(q("SmluvniCenaMaintenance")) Then
                SmluvniCenaMaintenance = CDec(q("SmluvniCenaMaintenance"))
            End If
            Dim rr_UcetniStavSB = 1
            If Not String.IsNullOrEmpty(q("rr_UcetniStavSB")) Then
                rr_UcetniStavSB = q("rr_UcetniStavSB")
            End If
            Dim rr_UcetniStavSBText = ""
            If Not String.IsNullOrEmpty(q("rr_UcetniStavSBText")) Then
                rr_UcetniStavSBText = q("rr_UcetniStavSBText")
            End If

            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_UDoctorumBackupProfile(IDBackupProfile, Firma, PopisZarizeni, ServerovaSlozka, Poznamka, DoKdyZaplaceno, PosledniFaktura, rr_AmbulSW, rr_StavBackupProfile, KdoToJe, Telefon, Email, FakturovatNaICO, SBvRezimuServer, SmluvniCenaMaintenance, rr_UcetniStavSB, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = Nothing, .total = 0, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Sub SendNotification(cpuId As String, message As String)
        Try
            hubContext.Clients.All.ShowMyNotify(cpuId, message)
        Catch ex As Exception
        End Try
    End Sub

    <HttpGet>
    Public Sub GetConfig(id As String, caller As String)
        Try
            hubContext.Clients.All.GetConfig(id, caller)
        Catch ex As Exception
        End Try
    End Sub

    <HttpGet>
    Public Sub RemoteBackup(id As String)
        Try
            hubContext.Clients.All.RemoteBackup(id)
        Catch ex As Exception
        End Try
    End Sub

    <HttpPost>
    Public Sub UploadConfig()
        Try
            Dim q = HttpContext.Current.Request.Form
            Dim caller = Nothing
            If Not String.IsNullOrEmpty(q("caller")) Then
                caller = q("caller")
            End If
            Dim cpuId = Nothing
            If Not String.IsNullOrEmpty(q("cpuId")) Then
                cpuId = q("cpuId")
            End If

            Dim file As HttpPostedFile = HttpContext.Current.Request.Files("config")
            If file IsNot Nothing Then
                Dim name As String = cpuId & "/" & file.FileName
                Dim path = IO.Path.Combine(HttpContext.Current.Server.MapPath("~/Backups"), name)
                If IO.File.Exists(path) Then
                    IO.File.Delete(path)
                End If
                file.SaveAs(path)
                Dim xml = System.IO.File.ReadAllText(path)
                hubContext.Clients.All.uploadConfig(xml, caller)
            Else
                hubContext.Clients.All.uploadConfig(Nothing, caller)
            End If
        Catch ex As Exception
        End Try
    End Sub

    <HttpGet>
    Public Overridable Function AGsp_Get_DashboardPohledavkyPodrobne(options As ODataQueryOptions(Of AGsp_Get_DashboardPohledavkyPodrobne_Result1)) As Object
        Try
            Using db As New Data4995Entities
                Dim model = db.AGsp_Get_DashboardPohledavkyPodrobne(If(User.IsInRole("frolikova@agilo.cz"), True, False)).ToList
                Dim queryable As IQueryable(Of AGsp_Get_DashboardPohledavkyPodrobne_Result1) = model.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                Dim result = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_DashboardPohledavkyPodrobne_Result1)).ToList
                Return New With {.data = result, .total = model.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_ISafeBerryDownload() As Object
        Try
            Dim q = HttpContext.Current.Request.Form
            Dim ip = ""
            If Not String.IsNullOrEmpty(q("ip")) Then
                ip = q("ip")
            End If
            Dim country = ""
            If Not String.IsNullOrEmpty(q("country")) Then
                country = q("country")
            End If
            Dim city = ""
            If Not String.IsNullOrEmpty(q("city")) Then
                city = q("city")
            End If
            Using db As New Data4995Entities
                db.AGsp_Do_ISafeBerryDownload(ip, country, city)
                Return Nothing
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return ex.Message
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_UsersProFiltrVyuctovani() As Object
        Try
            Using db As New Data4995Entities
                Dim data = db.AG_tblUsers.Select(Function(e) New With {.value = e.IDUser, .text = e.UserLastName}).ToList
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_VyuctovaniNazvyComboBox() As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim result = db.AGsp_Get_VyuctovaniNazvyComboBox(lL_LastLapse).ToList
                Dim data As New List(Of Object)
                Dim id As Integer = 1
                For Each item In result
                    data.Add(New With {.value = item, .text = item})
                    id += 1
                Next
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = data, .total = data.Count, .error = Nothing}
            End Using
        Catch ex As Exception
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_VyuctovaniSeznam(options As ODataQueryOptions(Of AGsp_Get_VyuctovaniSeznam_Result)) As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim iDUserFiltr = 0
            If Not String.IsNullOrEmpty(q("iDUserFiltr")) Then
                iDUserFiltr = CInt(q("iDUserFiltr"))
            End If
            Dim vyuctovaniNazev = ""
            If Not String.IsNullOrEmpty(q("vyuctovaniNazev")) Then
                vyuctovaniNazev = q("vyuctovaniNazev")
            End If
            Dim rr_StavVyuctovani = 0
            If Not String.IsNullOrEmpty(q("rr_StavVyuctovani")) Then
                rr_StavVyuctovani = q("rr_StavVyuctovani")
            End If
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim idu = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                Dim data = db.AGsp_Get_VyuctovaniSeznam(iDUserFiltr, vyuctovaniNazev, rr_StavVyuctovani, idu, lL_LastLapse).ToList
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Dim count = data.Count
                Dim queryable As IQueryable(Of AGsp_Get_VyuctovaniSeznam_Result) = data.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_VyuctovaniSeznam_Result)).ToList
                Return New With {.data = data, .total = count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_VyuctovaniTechnikaPol(options As ODataQueryOptions(Of AGsp_Get_VyuctovaniTechnikaPol_Result)) As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim iDUserFiltr = 0
            If Not String.IsNullOrEmpty(q("iDUserFiltr")) Then
                iDUserFiltr = CInt(q("iDUserFiltr"))
            End If
            Dim vyuctovaniNazev = ""
            If Not String.IsNullOrEmpty(q("vyuctovaniNazev")) Then
                vyuctovaniNazev = q("vyuctovaniNazev")
            End If
            Dim rr_StavVyuctovani = 0
            If Not String.IsNullOrEmpty(q("rr_StavVyuctovani")) Then
                rr_StavVyuctovani = q("rr_StavVyuctovani")
            End If
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim idu = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
                Dim data = db.AGsp_Get_VyuctovaniTechnikaPol(iDUserFiltr, vyuctovaniNazev, rr_StavVyuctovani, idu, lL_LastLapse).OrderByDescending(Function(e) e.IDVykazPracePol).ToList
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Dim count = data.Count
                Dim queryable As IQueryable(Of AGsp_Get_VyuctovaniTechnikaPol_Result) = data.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_VyuctovaniTechnikaPol_Result)).ToList
                Dim sum = data.Where(Function(w) w.OdmenaTechnika IsNot Nothing).Sum(Function(s) s.OdmenaTechnika)
                Return New With {.data = data, .total = count, .error = Nothing, .sum = sum}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message, .sum = 0}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_DelFirma() As Object
        Try
            Dim q = HttpContext.Current.Request.Form
            Dim iDFirmy = 0
            If Not String.IsNullOrEmpty(q("iDFirmy")) Then
                iDFirmy = CInt(q("iDFirmy"))
            End If
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_DelFirma(iDFirmy, lL_LastLapse)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = Nothing, .total = 0, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_IUFirma(model As AGsp_Get_FirmySeznam_Result) As Object
        Try
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Dim newIDFirmy As New ObjectParameter("NewIDFirmy", GetType(Integer))
            Using db As New Data4995Entities
                db.AGsp_Do_IUFirma(model.IDFirmy, model.IDKnihy, model.FirmaNazev, model.Ulice, model.PSC, model.Mesto, model.ICO, model.DIC, model.EmailProObjednavky, model.EmailProFakturaci, model.UzNepouzivat, model.Odberatel, model.Dodavatel, model.Kategorie, model.Pozn, model.IDUserUpravil, model.CasVytvoril, model.CasUpravil, model.ZapisOR, model.ICP, model.ICPOsoba, model.ICPAdresa, model.ICPNazev1, model.ICPNazev2, model.ICPOdbornost, model.Jednatel, model.KontaktTelefon, lL_LastLapse, newIDFirmy)
                If lL_LastLapse.Value > 0 Then
                    Return New With {.data = Nothing, .total = 0, .error = lastLapse(lL_LastLapse.Value)}
                End If
                Return New With {.data = newIDFirmy.Value, .total = 0, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_FirmySeznam(options As ODataQueryOptions(Of AGsp_Get_FirmySeznam_Result)) As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim iDKnihy = 0
            If Not String.IsNullOrEmpty(q("iDKnihy")) Then
                iDKnihy = CInt(q("iDKnihy"))
            End If
            Dim hledej = ""
            If Not String.IsNullOrEmpty(q("hledej")) Then
                hledej = q("hledej")
            End If
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_FirmySeznam(iDKnihy, hledej).OrderByDescending(Function(e) e.FirmaNazev).ToList
                Dim count = data.Count
                Dim queryable As IQueryable(Of AGsp_Get_FirmySeznam_Result) = data.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_FirmySeznam_Result)).ToList
                Return New With {.data = data, .total = count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpPost>
    Public Overridable Function AGsp_Do_IHistory() As Object
        Try
            Dim q = HttpContext.Current.Request.Form
            Dim iDBackupProfile = 0
            If Not String.IsNullOrEmpty(q("iDBackupProfile")) Then
                iDBackupProfile = CInt(q("iDBackupProfile"))
            End If
            Dim predmet = ""
            If Not String.IsNullOrEmpty(q("predmet")) Then
                predmet = q("predmet")
            End If
            Dim text = ""
            If Not String.IsNullOrEmpty(q("text")) Then
                text = q("text")
            End If
            Using db As New Data4995Entities
                Dim iDUserUpravil = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser

                db.AGsp_Do_IHistory(iDBackupProfile, iDUserUpravil, predmet, text)

                Return New With {.data = Nothing, .total = 0, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function

    <HttpGet>
    Public Overridable Function AGsp_Get_Historie(options As ODataQueryOptions(Of AGsp_Get_Historie_Result)) As Object
        Try
            Dim q = HttpContext.Current.Request.QueryString
            Dim iDBackupProfile = 0
            If Not String.IsNullOrEmpty(q("iDBackupProfile")) Then
                iDBackupProfile = CInt(q("iDBackupProfile"))
            End If
            Using db As New Data4995Entities
                Dim data = db.AGsp_Get_Historie(iDBackupProfile).OrderByDescending(Function(e) e.IDHistory).ToList
                Dim count = data.Count
                Dim queryable As IQueryable(Of AGsp_Get_Historie_Result) = data.AsQueryable()
                If options.Filter IsNot Nothing Then
                    queryable = options.Filter.ApplyTo(queryable, New ODataQuerySettings())
                End If
                data = TryCast(options.ApplyTo(queryable), IEnumerable(Of AGsp_Get_Historie_Result)).ToList
                Return New With {.data = data, .total = count, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .total = 0, .error = ex.Message}
        End Try
    End Function
End Class
