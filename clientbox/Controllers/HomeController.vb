
Imports System.Data.Entity.Core.Objects
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Mail
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading.Tasks

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Public context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of AgiloHub)()

    Function WindowTask(id As Integer) As ActionResult
        Return View(id)
    End Function

    <AcceptVerbs(HttpVerbs.Post), ValidateInput(False)>
    Function WindowPostTask() As Integer
        'Dim form = Request.Form
        'Dim user = form("user")
        'Dim id = form("id")
        'Dim from = form("from")
        'Dim subject = form("subject")
        'Dim recepients = form("recepients")
        'Dim message = form("message")

        'Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
        'Dim iDTicket As New ObjectParameter("IDTicket", GetType(Integer))
        'Using db As New Data4995Entities
        '    db.AGsp_Do_ITicketZEmailu(id, from, recepients, subject, message, user, iDTicket, lL_LastLapse)
        'End Using

        'Dim idtask = iDTicket.Value

        'Return idtask
    End Function

    <HttpGet>
    <AllowAnonymous>
    Public Function GetAccLogin(username As String, password As String)
        Try
            Using db As New Data4995Entities
                Dim acc = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = username)
                If acc IsNot Nothing Then
                    Dim hashpwd As String = GetMd5Hash(username.ToLower & password)
                    If hashpwd = acc.UserPWD Then
                        If acc.UserAccountEnabled Then
                            Dim c = Content("Přístup povolen")
                            c.ContentEncoding = Encoding.UTF8
                            Return c
                        Else
                            'ucet neni povolen
                            Dim c = Content("Přístup odepřen")
                            c.ContentEncoding = Encoding.UTF8
                            Return c
                        End If
                    Else
                        'zadal spatne heslo k uctu
                        Dim c = Content("Chybně zadané heslo nebo uživatelské jméno")
                        c.ContentEncoding = Encoding.UTF8
                        Return c
                    End If
                Else
                    'ucet neexistuje
                    Dim c = Content("Účet neexistuje")
                    c.ContentEncoding = Encoding.UTF8
                    Return c
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Dim c = Content(ex.Message)
            c.ContentEncoding = Encoding.UTF8
            Return c
        End Try
    End Function

    <HttpGet>
    Public Function GetAccInfo()
        Dim q = Request.QueryString
        Dim version = "1.0.0.0."
        If Not String.IsNullOrEmpty(q("version")) Then
            version = q("version")
        Else
            Return New HttpStatusCodeResult(401, "Nenalezeno")
        End If
        Dim userName = ""
        If Not String.IsNullOrEmpty(q("userName")) Then
            userName = q("userName")
        Else
            Return New HttpStatusCodeResult(401, "Nenalezeno")
        End If
        Dim cpuId = ""
        If Not String.IsNullOrEmpty(q("cpuId")) Then
            cpuId = q("cpuId")
        Else
            Return New HttpStatusCodeResult(401, "Nenalezeno")
        End If
        Dim lastBackupDate = Nothing
        If Not String.IsNullOrEmpty(q("lastBackupDate")) Then
            lastBackupDate = q("lastBackupDate")
        End If
        Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
        Dim iDBackupProfile As New ObjectParameter("IDBackupProfile", GetType(Integer))
        Dim rr_StavBackupProfile As New ObjectParameter("rr_StavBackupProfile", GetType(Integer))
        Dim rr_StavBackupProfileText As New ObjectParameter("rr_StavBackupProfileText", GetType(String))
        Dim datumExpirace As New ObjectParameter("DatumExpirace", GetType(DateTime))
        Using db As New Data4995Entities
            db.AGsp_Get_DoctorumBackupConfig(cpuId, userName, version, iDBackupProfile, rr_StavBackupProfile, rr_StavBackupProfileText, datumExpirace, lL_LastLapse)

            Dim returnXml = <d>
                                <i><%= iDBackupProfile.Value %></i>
                                <si><%= rr_StavBackupProfile.Value %></si>
                                <st><%= rr_StavBackupProfileText.Value %></st>
                                <d><%= CDate(datumExpirace.Value).ToString("yyyy-MM-ddTHH:mm:ss") %></d>
                                <ll><%= lL_LastLapse.Value %></ll>
                            </d>

            Dim latestversion As String = Server.MapPath("/App_Data/Download/Backup/latestversion.xml")
            If IO.File.Exists(latestversion) Then
                Dim versiondoc As XDocument = XDocument.Load(latestversion)
                Dim item = versiondoc.Root
                If item IsNot Nothing Then
                    Dim vers = item.Element("version")
                    If vers IsNot Nothing Then
                        returnXml.Add(<v><%= vers.Value %></v>)
                    End If
                End If
            End If

            Return New ContentResult With
            {
                .Content = returnXml.ToString(),
                .ContentType = "text/xml"
            }
        End Using
    End Function

    Public Function GetWeekNumber(ByVal datum As Date) As Integer
        Return (datum.Day - 1) \ 7 + 1
    End Function

    <HttpGet>
    Public Function DownloadSWBackup()
        Try
            Dim q = Request.QueryString
            Dim version = Nothing
            If Not String.IsNullOrEmpty(q("version")) Then
                version = q("version")
            End If
            Dim path As String = Server.MapPath("/App_Data/Download/Backup/" & version & "/Setup.msi")
            Dim fileBytes As Byte() = System.IO.File.ReadAllBytes(path)
            Return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Setup.msi")
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New HttpStatusCodeResult(404, ex.Message)
        End Try
    End Function

    <HttpPost>
    Public Function UploadTestSpeed()
        Return New HttpStatusCodeResult(200, "OK")
    End Function

    <HttpPost>
    <ValidateInput(False)>
    Public Function BackupInfo()
        Try
            Dim q = Request.Form
            Dim userName = ""
            If Not String.IsNullOrEmpty(q("userName")) Then
                userName = q("userName")
            Else
                Return New HttpStatusCodeResult(401, "Nenalezeno")
            End If
            Dim cpuId = ""
            If Not String.IsNullOrEmpty(q("cpuId")) Then
                cpuId = q("cpuId")
            Else
                Return New HttpStatusCodeResult(401, "Nenalezeno")
            End If
            Dim bcFileName = ""
            If Not String.IsNullOrEmpty(q("bcFileName")) Then
                bcFileName = q("bcFileName")
            End If

            Dim config = ""
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim item = db.AGsp_Get_BackupServerovaSlozka(cpuId).FirstOrDefault
                If item IsNot Nothing Then
                    If item.rr_StavBackupProfile < 3 And item.ServerovaSlozka IsNot Nothing Then
                        Dim dir = "~/" & item.ServerovaSlozka
                        Dim serverdir = Server.MapPath(dir)

                        If Not IO.Directory.Exists(serverdir) Then
                            IO.Directory.CreateDirectory(serverdir)
                        End If

                        Dim day = Now.ToString("dddd")
                        Dim dayDir = IO.Path.Combine(serverdir, day)

                        If Not IO.Directory.Exists(dayDir) Then
                            IO.Directory.CreateDirectory(dayDir)
                        End If

                        Dim bcdir = Guid.NewGuid().ToString("D")
                        Dim dest As String = IO.Path.Combine(dayDir, bcdir)
                        If Not IO.Directory.Exists(dest) Then
                            IO.Directory.CreateDirectory(dest)
                        End If

                        For Each key In Request.Files
                            Dim file As HttpPostedFileWrapper = Request.Files(key)
                            Using reader As New StreamReader(file.InputStream)
                                config = reader.ReadToEnd
                            End Using
                            Dim newbc = (dir & "/" & day & "/" & bcdir & "/" & bcFileName)
                            Try
                                db.AGsp_Do_INovyBackup(item.IDBackupProfile, newbc, 1, config, lL_LastLapse)
                            Catch ex As Exception
                                db.AGsp_Do_INovyBackup(item.IDBackupProfile, newbc, 2, config, lL_LastLapse)
                            End Try
                        Next

                        Return IO.Path.Combine(dest, bcFileName)
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            End Using

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    <HttpPost>
    <ValidateInput(False)>
    Public Function BackupInfo2()
        Try
            Dim q = Request.Form
            Dim userName = ""
            If Not String.IsNullOrEmpty(q("userName")) Then
                userName = q("userName")
            Else
                Return New HttpStatusCodeResult(401, "Nenalezeno")
            End If
            Dim cpuId = ""
            If Not String.IsNullOrEmpty(q("cpuId")) Then
                cpuId = q("cpuId")
            Else
                Return New HttpStatusCodeResult(401, "Nenalezeno")
            End If
            Dim bcFileName = ""
            If Not String.IsNullOrEmpty(q("bcFileName")) Then
                bcFileName = q("bcFileName")
            End If

            Dim config = ""
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim item = db.AGsp_Get_BackupServerovaSlozka(cpuId).FirstOrDefault
                If item IsNot Nothing Then
                    If item.rr_StavBackupProfile < 3 And item.ServerovaSlozka IsNot Nothing Then
                        Dim dir = "~/" & item.ServerovaSlozka
                        Dim serverdir = Server.MapPath(dir)

                        If Not IO.Directory.Exists(serverdir) Then
                            IO.Directory.CreateDirectory(serverdir)
                        End If

                        Dim day = Now.ToString("dddd")
                        Dim dayDir = IO.Path.Combine(serverdir, day)
                        If Not IO.Directory.Exists(dayDir) Then
                            IO.Directory.CreateDirectory(dayDir)
                        End If

                        Dim newbc = (dir & "/" & day & "/" & Now.ToString("HHmmss") & "_" & bcFileName)
                        For Each key In Request.Files
                            Dim file As HttpPostedFileWrapper = Request.Files(key)
                            Using reader As New StreamReader(file.InputStream)
                                config = reader.ReadToEnd
                            End Using
                            Try
                                db.AGsp_Do_INovyBackup(item.IDBackupProfile, newbc, 1, config, lL_LastLapse)
                            Catch ex As Exception
                                db.AGsp_Do_INovyBackup(item.IDBackupProfile, newbc, 2, config, lL_LastLapse)
                            End Try
                        Next

                        Return Server.MapPath(newbc)
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            End Using

        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Using db As New Data4995Entities
                db.AGsp_Do_AddError("AGsp_Get_BackupServerovaSlozka/AGsp_Do_INovyBackup", 315, ex.Message)
            End Using
            Return New HttpStatusCodeResult(500, ex.Message)
        End Try
    End Function

    <HttpPost>
    Public Function FileUpload()
        Try
            Dim q = Request.QueryString
            Dim path = ""
            If Not String.IsNullOrEmpty(q("path")) Then
                path = q("path")
            End If
            Dim fi As New FileInfo(path)
            Dim di As New DirectoryInfo(fi.DirectoryName)

            For Each key In Request.Files
                Dim file As HttpPostedFileWrapper = Request.Files(key)
                Dim fileName = IO.Path.Combine(di.FullName, file.FileName)
                file.SaveAs(fileName)
            Next
            Return New HttpStatusCodeResult(200, "OK")
        Catch ex As Exception
            Return New HttpStatusCodeResult(500, ex.Message)
        End Try
    End Function

    Public Function GetUserBackups(id As Integer)
        Using db As New Data4995Entities
            Dim data = db.AGsp_Get_BackupZalohyJednohoProfilu(id).Where(Function(e) e.rr_BackupUspesnost = 1).ToList
            Dim xml As XElement = New XElement("CloudBackups", data.Select(Function(e) New XElement("item", New List(Of XElement) From {New XElement("BackupDate", e.BackupDate.ToString("dd.MM.yyyy HH:mm")), New XElement("BackupFileName", e.BackupFileName)})))
            Return xml
        End Using
    End Function

    <HttpPost>
    <ValidateInput(False)>
    Public Function ChunksUpload()
        Try
            Dim q = Request.Form
            Dim path = ""
            If Not String.IsNullOrEmpty(q("path")) Then
                path = q("path")
            End If
            Dim isLast = False
            If Not String.IsNullOrEmpty(q("isLast")) Then
                isLast = True
            End If

            Dim fi As New FileInfo(path)
            Dim di As New DirectoryInfo(fi.DirectoryName)
            For Each key In Request.Files
                Dim file As HttpPostedFileWrapper = Request.Files(key)
                Dim fileName = IO.Path.Combine(di.FullName, file.FileName)
                file.SaveAs(fileName)
            Next

            If isLast Then
                Dim fileList = di.GetFiles
                If fileList.Count > 0 Then
                    Using outputStream = IO.File.Create(fi.FullName)
                        For i As Integer = 1 To fileList.Count
                            Dim file = di.GetFiles("*." & i).FirstOrDefault
                            If file IsNot Nothing Then
                                Using inputStream = IO.File.OpenRead(file.FullName)
                                    inputStream.CopyTo(outputStream)
                                End Using
                                IO.File.Delete(file.FullName)
                            End If
                        Next
                    End Using
                End If
            End If

            Return New HttpStatusCodeResult(200, "OK")
        Catch ex As Exception
            Return New HttpStatusCodeResult(500, ex.Message)
        End Try
    End Function

    <HttpPost>
    <ValidateInput(False)>
    Public Function Encrypt_Upload()
        Try
            Dim q = Request.Form
            'Dim version = "1.0.0.0"
            Dim userName = ""
            If Not String.IsNullOrEmpty(q("userName")) Then
                userName = q("userName")
            Else
                Return New HttpStatusCodeResult(401, "Nenalezeno")
            End If
            Dim cpuId = ""
            If Not String.IsNullOrEmpty(q("cpuId")) Then
                cpuId = q("cpuId")
            Else
                Return New HttpStatusCodeResult(401, "Nenalezeno")
            End If
            Dim config = ""
            If Not String.IsNullOrEmpty(q("config")) Then
                config = q("config")
            End If

            Dim today = Now
            Dim lL_LastLapse As New ObjectParameter("LL_LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim item = db.AGsp_Get_BackupServerovaSlozka(cpuId).FirstOrDefault
                If item IsNot Nothing Then
                    If item.rr_StavBackupProfile < 3 And item.ServerovaSlozka IsNot Nothing Then
                        Dim dir = "~/" & item.ServerovaSlozka
                        Dim serverdir = Server.MapPath(dir)
                        If Not IO.Directory.Exists(serverdir) Then
                            IO.Directory.CreateDirectory(serverdir)
                        End If
                        Dim day = today.ToString("dddd")
                        Dim dayDir = IO.Path.Combine(serverdir, day)
                        If Not IO.Directory.Exists(dayDir) Then
                            IO.Directory.CreateDirectory(dayDir)
                        End If

                        For Each key In Request.Files
                            Dim file As HttpPostedFileWrapper = Request.Files(key)
                            Dim name As String = "(" & today.ToString("yyMMddHHmmss") & ")_" & file.FileName
                            Dim path = IO.Path.Combine(dayDir, name)
                            Try
                                If IO.File.Exists(path) Then
                                    IO.File.Delete(path)
                                End If
                                file.SaveAs(path)
                                db.AGsp_Do_INovyBackup(item.IDBackupProfile, dir & "/" & day & "/" & name, 1, config, lL_LastLapse)
                            Catch ex As Exception
                                db.AGsp_Do_INovyBackup(item.IDBackupProfile, dir & "/" & day & "/" & name, 2, config, lL_LastLapse)
                            End Try
                        Next

                        context.Clients.All.uploadChenge()

                        Return New HttpStatusCodeResult(200, "Záloha dokončena")
                    Else
                        Return New HttpStatusCodeResult(401, "Nepovoleno")
                    End If
                Else
                    Return New HttpStatusCodeResult(401, "Vypnutý")
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New HttpStatusCodeResult(500, ex.Message)
        End Try
    End Function

    <AcceptVerbs(HttpVerbs.Post), ValidateInput(False)>
    Public Function SendEmailTicket(EmailTo As String, Subject As String, Body As String) As Object
        Try
            Using mail As New MailMessage()
                mail.From = New MailAddress("podpora@doctorum.cz", "DOCTORUM.CZ")
                mail.To.Add(New MailAddress(EmailTo))
                mail.Bcc.Add(New MailAddress("podpora@doctorum.cz"))
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

    '<Authorize>
    'Function Index() As ActionResult

    '    'Using client = New Net.WebClient()
    '    '    client.DownloadFile("http://www.milankarpisek.cz/index.php", "c:\Users\Petr\source\repos\index.php")
    '    'End Using


    '    Return View()
    'End Function


    'Public Shared Function KendoModelType(ByVal possiblyNullableType As Type) As String
    '    Dim nullableType = Nullable.GetUnderlyingType(possiblyNullableType)
    '    Dim isNullableType As Boolean = nullableType IsNot Nothing
    '    Dim name = ""
    '    If isNullableType Then
    '        name = nullableType.Name.ToLower
    '    Else
    '        name = possiblyNullableType.Name.ToLower
    '    End If
    '    If name = "integer" Or name = "decimal" Or name = "double" Or name = "byte" Or name = "int32" Or name = "int16" Or name = "short" Or name = "float" Then
    '        name = "number"
    '    End If
    '    If name = "datetime" Then
    '        name = "date"
    '    End If
    '    Return name
    'End Function

    'Public Sub Create_kendoEntityModel(controllerName As String, entitiesName As String)
    '    Dim ass = Reflection.Assembly.GetExecutingAssembly().GetName().Name
    '    Dim type As Type = Type.GetType(ass & "." & controllerName & "Controller")
    '    Dim baseType = type.BaseType.Name
    '    Dim u = System.Web.HttpContext.Current.Request.Url

    '    Dim myuri As Uri = New Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri)
    '    Dim pathQuery As String = myuri.PathAndQuery
    '    Dim hostName As String = myuri.ToString().Replace(pathQuery, "")

    '    If baseType = "ApiController" Then
    '        hostName += "/Api/" & controllerName
    '    Else
    '        hostName += "/" & controllerName
    '    End If

    '    Dim sb As New StringBuilder()
    '    sb.AppendLine("var " & entitiesName & " = {")

    '    Dim props = Type.GetType(ass & "." & entitiesName).GetProperties.Where(Function(c) c.Name <> "Database" And c.Name <> "ChangeTracker" And c.Name <> "Configuration")
    '    sb.AppendLine("tblvw: {")
    '    For Each p In props
    '        sb.AppendLine(p.Name & ": {")

    '        Dim cls = Type.GetType(ass & "." & p.Name).GetProperties.Where(Function(c) c.PropertyType.Namespace = "System")
    '        sb.AppendLine("service: '" & hostName & "/" & p.Name & "',")
    '        sb.AppendLine("model: {")
    '        sb.AppendLine("id: '" & cls.First.Name & "',")
    '        sb.AppendLine("fields: {")
    '        For Each i In cls
    '            sb.AppendLine(i.Name & ": { type: '" & KendoModelType(i.PropertyType) & "' }" & If(cls.Last = i, "", ","))
    '        Next
    '        sb.AppendLine("}")
    '        sb.AppendLine("}")

    '        sb.AppendLine("}" & If(props.Last = p, "", ","))
    '    Next
    '    sb.AppendLine("},")
    '    sb.AppendLine("}")

    '    Dim x = sb.ToString

    '    For Each p In Type.GetType(ass & "." & entitiesName).GetMethods.ToList
    '        Dim IsFnNotRes = p.ReturnParameter.Name Is GetType(System.Int32).Name
    '        Dim IsFnRes = p.ReturnParameter.Name.Contains("ObjectResult")
    '        Dim IsDbSet = p.ReturnParameter.Name.Contains("DbSet")

    '        If IsFnNotRes Then

    '        End If
    '        Dim yy = 0
    '    Next

    '    'sb.AppendFormat("GHI{0}{1}", 'J', 'k')

    '    'Dim x = From p In Type.GetType(ass & "." & "AG_tblRoles").GetProperties Where p.PropertyType.Namespace = "System" Select p.PropertyType

    '    'Dim doc As XNode = <dbContext>
    '    '                       <<%= entitiesName.ToLower %>>
    '    '                           <tables_views>
    '    '                               <%= From c In Type.GetType(ass & "." & entitiesName).GetProperties Where c.Name <> "Database" And c.Name <> "ChangeTracker" And c.Name <> "Configuration" Select <<%= c.Name %>>
    '    '                                                                                                                                                                                                    <%= From p In Type.GetType(ass & "." & c.Name).GetProperties.ToList Where p.PropertyType.Namespace = "System" Select <<%= p.Name %>><type><%= KendoModelType(p.PropertyType) %></type></> %>
    '    '                                                                                                                                                                                                </> %>
    '    '                           </tables_views>
    '    '                           <procedures>
    '    '                               <%= From fn In Type.GetType(ass & "." & entitiesName).GetMethods Select <<%= fn.Name %>>
    '    '                                                                                                           <%= From p In fn.GetParameters Select <<%= p.Name %>></> %>
    '    '                                                                                                       </> %>
    '    '                           </procedures>
    '    '                       </>
    '    '                   </dbContext>

    '    'Dim x = Newtonsoft.Json.JsonConvert.SerializeXNode(doc)


    '    For Each fn In Type.GetType(ass & "." & entitiesName).GetMethods.Where(Function(e) e.ReturnParameter.Name <> "Int32").ToList
    '        Dim fn_params = fn.GetParameters.ToList
    '        Dim a = 0
    '    Next

    '    'Dim doc As XDocument = <?xml version="1.0"?>
    '    '                       <%= entitiesName %></>



    '    Dim y = 0
    'End Sub

    <Authorize>
    Function Fakturace() As ActionResult


        'Using mail As New MailMessage()
        '    mail.From = New MailAddress("podpora@doctorum.cz", "DOCTORUM.CZ")
        '    mail.To.Add(New MailAddress("hanzl@agilo.cz"))
        '    mail.Subject = "Webové stránky pro lékaře ordinace ceník"
        '    mail.Body = <div style="margin:20px;color: rgba(0,0,0,0.87);font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;">
        '                    <h3>Webové stránky pro lékaře ordinace ceník</h3>
        '                    <p>Dobrý den paní doktorko,</p>
        '                    <p>zasílám informace ohledně webových stránek. Podívejte se na naší práci např. na <a href="https://www.mudrkucera.cz/" target="_blank">www.mudrkucera.cz</a> nebo <a href="https://www.ambulancevpl.cz/" target="_blank">www.ambulancevpl.cz</a> nebo <a href="https://www.practicus-vseobecnylekar.cz/" target="_blank">www.practicus-vseobecnylekar.cz</a></p>
        '                    <p>Jedná se o jednoduché stránky v responzivním designu (můžete si vyzkoušet odkazy i v mobilním telefonu), které mají za úkol informovat klienty rychle o novinkách, kontaktech, ordinačních hodinách, úrovni ordinace a nabízených specialitách. Pro běžnou praxi lékaře není třeba dělat nic složitějšího. Obsahují inteligentní nastavení aktualit s dobou jejich platnosti/(rozsvícení) na stránce - typicky pro účely dovolené, zástupu, zkrácení ordinačních hodin atd.</p>
        '                    <p>Tyto webové stránky vám vyhotovíme komplet "na klíč" bez jakýchkoli dalších poplatků. Potřebujeme pouze součinnost při přípravě textů případně fotografií.</p>
        '                    <p>Pro funkčnost webových stránek zajistíme tři věci:</p>
        '                    <ol>
        '                        <li>webovou adresu (doménu) na které budou vždy stránky k nalezení</li>
        '                        <li>umístění stránek na server (fyzické místo, kde Vaše stránky "sedí", kde jsou uloženy</li>
        '                        <li>naprogramovat vzhled a obsah Vašich stránek</li>
        '                    </ol>
        '                    <b>ad 1)</b>
        '                    <p>doménu třetího řádu, např. <b>pertlova.doctorum.cz</b> poskytujeme zdarma</p>
        '                    <p>doména druhého řádu, např. <b>pertlova.cz</b> obnáší roční poplatek národnímu správci domén 155,-Kč/rok. Roční pravidelné obnovování webhostingu a domény za vás hlídáme a spravujeme.</p>
        '                    <b>ad 2)</b>
        '                    <p>Stránky musejí být někde umístěny, nejvýhodnější je pronájem webhostingu, za který se platí 50,- až 200,- Kč/měs, pro Vaše účely by to bylo za <b>71,-Kč/měsíc</b>.</p>
        '                    <b>ad 3)</b>
        '                    <p>Níže uvádíme ceny (jsou již včetně DPH) za vyhotovení, vás by se prakticky týkaly pouze položky 1, 2, 5, 6.</p>
        '                    <table border="1" cellspacing="0" cellpadding="10">
        '                        <tr>
        '                            <td>1.</td>
        '                            <td>Jednorázové vytvoření webových stránek s uživatelským přístupem pro editaci aktualit</td>
        '                            <td>8600</td>
        '                        </tr>
        '                        <tr>
        '                            <td>2.</td>
        '                            <td>Texty dodá objednatel</td>
        '                            <td>0</td>
        '                        </tr>
        '                        <tr>
        '                            <td>3.</td>
        '                            <td>Texty navrhne a vytvoří dodavatel, objednatel provede korekturu</td>
        '                            <td>1452-4840</td>
        '                        </tr>
        '                        <tr>
        '                            <td>4.</td>
        '                            <td>Pořízení fotografií profesionálním fotografem - orient. cena dle ceníku fotografa</td>
        '                            <td>2000-3000</td>
        '                        </tr>
        '                        <tr>
        '                            <td>5.</td>
        '                            <td>Roční poplatek za webhosting</td>
        '                            <td>850</td>
        '                        </tr>
        '                        <tr>
        '                            <td>6.</td>
        '                            <td>Roční poplatek za doménu</td>
        '                            <td>155</td>
        '                        </tr>
        '                    </table>
        '                    <p>
        '        Pokud vám to takto vyhovuje, další postup by byl takový, že společně vybereme nějaký vzhled a barevné ladění stránek, připravíme texty a vytvoříme beta verzi k otestování a připomínkování. Pak nasadíme ostrý provoz.
        '    </p>
        '                    <p>Jednoduché webové stánky pro lékaře (typu MUDr. KUČERA)</p>
        '                    <p>
        '        Roční obnovování webhostingu a domény za vás hlídáme a spravujeme.
        '    </p>
        '                    <p>
        '        Otázky na objednatele + šablona na texty webu:<br/>
        '        Zde je návodná šablona, podle které můžete vypsat a vytvořit texty. Není nutné dodržet vše, pokud např. všichni snadno ordinaci najdou, je zbytečné popisovat "jak se k nám dostanete"
        '    </p>
        '                    <p>
        '        Ordinační doba<br/>
        '        Kontakty - telefon do ordinace, objednávací telefon, email, přesná adresa ordinace, slovní popis příjezdu (jak se k nám dostanete) autem, MHD, pěšky<br/>
        '        Možnosti parkování<br/>
        '        Něco "O nás" - dosažené vzdělání, praxe, úspěchy, publikace, stáže, další vzdělání (lékař i sestra)<br/>
        '        Poskytované služby, nezapomenout na něco, co vás odlišuje od ostatních ordinací<br/>
        '        Používané přístroje/vybavení<br/>
        '        Foto vchodu do ordinace, foto čekárny, ordinace, přístrojů, vybavení....
        '    </p>
        '                    <hr/>
        '                    <p>
        '        Ceny domén prvního řádu<br/>
        '        (registrátor JOKER.COM)
        '    </p>
        '                    <p>
        '        .doctor<br/>
        '        per month<br/>
        '                        <span style="margin: 0; font-size: 1.17em; font-weight: bold; ">EUR 0.71 *</span><br/>
        '        ( <strike>EUR 121.53</strike><br/>
        '        EUR 8.55 / year * )
        '</p>
        '                    <p>
        '        .education<br/>
        '        per month<br/>
        '                        <span style="margin: 0; font-size: 1.17em; font-weight: bold; ">EUR 2.00 *</span><br/>
        '        ( EUR 23.93 / year * )
        '    </p>
        '                    <p>
        '        .expert<br/>
        '        per month<br/>
        '                        <span style="margin: 0; font-size: 1.17em; font-weight: bold; ">EUR 0.71 *</span><br/>
        '        ( <strike>EUR 60.77</strike><br/>
        '        EUR 8.55 / year * )
        '    </p>
        '                    <p>
        '        .software<br/>
        '        per month<br/>
        '                        <span style="margin: 0; font-size: 1.17em; font-weight: bold; ">EUR 0.71 *</span><br/>
        '        ( <strike>EUR 36.83</strike><br/>
        '        EUR 8.55 / year * )
        '    </p>
        '                    <p>
        '        .dental<br/>
        '        per month<br/>
        '                        <span style="margin: 0; font-size: 1.17em; font-weight: bold; ">EUR 5.07 *</span><br/>
        '        ( EUR 60.77 / year * )
        '    </p>
        '                </div>.ToString
        '    mail.IsBodyHtml = True

        '    Dim smtp As SmtpClient = New SmtpClient()
        '    smtp.Host = "smtp.forpsi.com"
        '    smtp.EnableSsl = True

        '    Dim networkCredential As NetworkCredential = New NetworkCredential("podpora@doctorum.cz", "Frolikova.321")
        '    smtp.UseDefaultCredentials = True
        '    smtp.Credentials = networkCredential
        '    smtp.Port = 587
        '    smtp.Send(mail)
        'End Using

        Return View()
    End Function

    <Authorize>
    Function Klienti() As ActionResult

        'Using mail As New MailMessage()
        '    mail.From = New MailAddress("podpora@doctorum.cz", "DOCTORUM.CZ")
        '    mail.To.Add(New MailAddress("novak@agilo.cz"))
        '    mail.Bcc.Add(New MailAddress("podpora@doctorum.cz"))
        '    mail.Subject = "DOCTORUM.CZ - měsíční přehled zálohování SafeBerry"

        '    Dim html = <div style="margin: 40px auto; display: table; color: rgba(0,0,0,0.87);font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; box-shadow: 0px 2px 2px 0px rgb(0 0 0 / 14%), 0px 3px 1px -2px rgb(0 0 0 / 12%), 0px 1px 5px 0px rgb(0 0 0 / 20%); border-radius: 2px; padding: 20px; ">
        '                   <a href="https://www.doctorum.cz/Navigace/zalohovani">
        '                       <img height="40" src="data:image/png;base64,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"/>
        '                   </a>
        '                   <h4 style="margin-left: 10px; margin-right: 10px; font: 400 20px/28px Roboto,RobotoDraft,Helvetica,Arial,sans-serif;">Měsíční report o provedených zálohách služby <b>Safe<span style="color:red;">Berry</span></b></h4>
        '                   <table style="border-collapse: collapse;font-size:14px;">
        '                       <tr style="border-bottom: 1px solid rgba(0,0,0,0.12); color: rgba(0,0,0,0.54);"><td style="padding:10px;">Datum</td><td style="padding:10px;">Čas</td><td style="padding:10px;">Úspěšná A/N</td><td style="padding:10px;">Trvání</td><td style="padding:10px;">Velikost zálohy MB</td><td style="padding:10px;">Přírůstek MB</td><td style="padding:10px;">Datum nejstaršího souboru</td><td style="padding:10px;">Datum nejmladšího souboru</td></tr>
        '                   </table>
        '                   <p style="margin-left: 10px; margin-right: 10px; color: rgba(0,0,0,0.54);font-size:14px;">
        '                               O vaše data je správně postaráno...
        '                            </p>
        '                   <div style="color:rgba(0,0,0,0.54);text-align:right;padding:10px 0 0 0;">
        '                       <a style="font-size:11px;color:rgba(0,0,0,0.54);text-decoration: none;" href="https://www.doctorum.cz">© <%= Now.Year %> DOCTORUM.CZ s.r.o.</a>
        '                   </div>
        '               </div>

        '    Dim table = html.<table>(0)
        '    For i As Integer = 0 To 10
        '        table.Add(<tr style="border-bottom: 1px solid rgba(0,0,0,0.12);"><td style="padding:10px;">03.11.2021</td><td style="padding:10px;">11:00</td><td style="padding:10px;">ANO</td><td style="padding:10px;">0:21:53</td><td style="padding:10px;">2 058 MB</td><td style="padding:10px;">1.5 MB</td><td style="padding:10px;">29.10.2021</td><td style="padding:10px;">03.11.2021</td></tr>)
        '    Next

        '    mail.Body = html.ToString
        '    mail.IsBodyHtml = True

        '    Dim smtp As SmtpClient = New SmtpClient()
        '    smtp.Host = "smtp.forpsi.com"
        '    smtp.EnableSsl = True

        '    Dim networkCredential As NetworkCredential = New NetworkCredential("podpora@doctorum.cz", "Frolikova.321")
        '    smtp.UseDefaultCredentials = True
        '    smtp.Credentials = networkCredential
        '    smtp.Port = 587
        '    smtp.Send(mail)
        'End Using

        Return View()
    End Function

    <Authorize>
    Function Zalohy() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function GrafDostupnosti(IDBackupProfile As Nullable(Of Integer)) As ActionResult
        Return View(IDBackupProfile)
    End Function

    <Authorize>
    Function Sklad() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function StoMega() As ActionResult
        Return View()
    End Function

    <Authorize(Roles:="3")>
    Function Mapa() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function Ciselniky() As ActionResult
        Return View()
    End Function

    <Authorize(Roles:="3")>
    Function Uzivatele() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function Objednavky() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function InventurniStav() As ActionResult
        Return View()
    End Function

    <Authorize(Roles:="hanzl@agilo.cz, fakturace@agilo.cz, novak@agilo.cz, frolikova@agilo.cz")>
    Function ServiskySeznam() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function MojeServisky() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function FakturaceServisek() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function MojeOdmeny() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function Kalendar() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function Tikety() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function Weby() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function Prijemky() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function Pohyby() As ActionResult
        Return View()
    End Function

    <Authorize>
    Function Adresar() As ActionResult
        Return View()
    End Function

    Public Function DownloadBackupZip(ByVal path As String) As ActionResult
        Dim p As String = Server.MapPath(path)
        If IO.File.Exists(p) Then
            Dim fi As New IO.FileInfo(p)
            Return New FilePathResult(path, "application/octet-stream") With {.FileDownloadName = fi.Name}
        End If
        Return Nothing
    End Function

    Public Function Download(ByVal file As String) As ActionResult
        Dim sp As String = Server.MapPath("~/App_Data")
        Dim p As String = Path.Combine(sp, file)
        Dim fi As New FileInfo(p)

        If Not System.IO.File.Exists(p) Then
            Return HttpNotFound()
        End If

        Dim fileBytes = System.IO.File.ReadAllBytes(p)
        Dim response = New FileContentResult(fileBytes, "application/octet-stream") With {
            .FileDownloadName = fi.Name
        }
        Return response
    End Function
End Class
