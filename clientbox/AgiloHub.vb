Imports System.Data.Entity
Imports System.Globalization
Imports System.Threading.Tasks
Imports System.Timers
Imports System.Web.OData.Query
Imports Microsoft.AspNet.SignalR
Imports Microsoft.AspNet.SignalR.Hubs

Public Class ChatUser
    Public Property id As String
    Public Property idu As String
    Public Property connectionId As String
    Public Property name As String
    Public Property device As String
    Public Property iconUrl As String
    Public Property lat As Decimal
    Public Property lng As Decimal
    Public Property messages As List(Of ChatUser)
    Public Property lastActivity As Date
    Public Property online As Boolean
End Class

'Public Class UserProperty
'    Public Property IDUser As Integer
'    Public Property ConnectionId As String
'    Public Property Device As String
'    Public Property Name As String
'    Public Property Lat As Decimal
'    Public Property Lng As Decimal
'    Public Property LastActivity As Date
'    Public Property Online As Boolean
'End Class

<HubName("AgiloHub")>
Public Class AgiloHub
    Inherits Hub

    Private Shared ReadOnly _users As UsersMapping(Of ChatUser) = New UsersMapping(Of ChatUser)()
    Private Shared coonectedids As New List(Of Integer)

    Public Sub ShowNotify(cpuId As String, userName As String, message As String)
        Clients.All.ShowNotify(cpuId, userName, message)
    End Sub

    Public Sub ClientProgress(iDBackupProfile As Integer, text As String, value As Double, isIndeterminate As Boolean)
        Clients.All.ClientProgress(iDBackupProfile, text, value, isIndeterminate)
    End Sub

    Public Sub GetDeviceInformationCallback(caller As String, infos As String)
        Clients.All.GetDeviceInformationCallback(caller, infos)
    End Sub

    Public Sub GetTimesCallback(caller As String, xml As String, iDBackupProfile As String)
        Clients.All.GetTimesCallback(caller, xml, iDBackupProfile)
    End Sub

    Public Sub ClientMessageCallback(iDBackupProfile As Integer, caller As String, msgid As Integer, state As Boolean)

    End Sub

    Public Sub SendRemoteDesktop(base64 As String)
        Clients.All.SendRemoteDesktop(base64)
    End Sub

    Public Sub set_activity(id As Integer, lat As Decimal, lng As Decimal, device As String)
        Dim users = _users.GetUsers
        Dim user = users.Where(Function(e) e.idu = id).FirstOrDefault
        Dim name As String = ""
        Using db As New Data4995Entities
            Dim us = db.AG_tblUsers.FirstOrDefault(Function(e) e.IDUser = id)
            name = us.UserFirstName & " " & us.UserLastName
        End Using

        If user IsNot Nothing Then
            user.connectionId = Context.ConnectionId
            user.device = device
            user.lastActivity = Now
            user.lat = lat
            user.lng = lng
        Else
            Dim newUser As New ChatUser
            newUser.connectionId = Context.ConnectionId
            newUser.device = device
            newUser.idu = id
            newUser.lastActivity = Now
            newUser.name = name
            newUser.lat = lat
            newUser.lng = lng
            newUser.online = True
            newUser.iconUrl = "http://demos.telerik.com/kendo-ui/content/chat/avatar.png"
            newUser.messages = New List(Of ChatUser)
            _users.Add(newUser)
        End If

        For Each u In users
            Dim ts = DateDiff(DateInterval.Second, Now, u.lastActivity)
            u.online = ts > -60
        Next

        Clients.Caller.get_activity(users.Where(Function(e) e.idu <> id))
    End Sub

    Public Sub send_notifi(_for As String, _message As String)
        Clients.All.Send(_for, _message)
    End Sub

    Public Sub send_typing(toID As Integer, fromID As Integer)
        Dim sender = _users.GetUsers.FirstOrDefault(Function(u) u.idu = fromID)
        Clients.All.typing(toID, sender)
    End Sub

    Public Sub send_showing(toID As Integer, fromID As Integer)
        Dim sender = _users.GetUsers.FirstOrDefault(Function(u) u.idu = fromID)
        Clients.All.showing(toID, sender)
    End Sub

    Public Sub send_message(toID As Integer, fromID As Integer, text As String)
        Dim sender = _users.GetUsers.FirstOrDefault(Function(u) u.idu = fromID)
        Clients.All.message(toID, sender, text)
    End Sub

    Public Overrides Function OnConnected() As Task
        'Dim name As String = Context.User.Identity.Name
        'Groups.Add(Context.ConnectionId, name)
        Dim iDBackupProfile = Context.QueryString("iDBackupProfile")
        'If Not String.IsNullOrEmpty(iDBackupProfile) Then
        '    coonectedids.Add(iDBackupProfile)
        'End If
        If Not String.IsNullOrEmpty(iDBackupProfile) Then
            Clients.All.ClientConnected(iDBackupProfile, True)
        End If

        Return MyBase.OnConnected()
    End Function

    Public Overrides Function OnDisconnected(stopCalled As Boolean) As Task
        'Dim name As String = Context.User.Identity.Name
        'Groups.Remove(Context.ConnectionId, name)
        Dim iDBackupProfile = Context.QueryString("iDBackupProfile")
        'If Not String.IsNullOrEmpty(iDBackupProfile) Then
        '    coonectedids.Remove(iDBackupProfile)
        'End If
        If Not String.IsNullOrEmpty(iDBackupProfile) Then
            Clients.All.ClientConnected(iDBackupProfile, False)
        End If

        Return MyBase.OnDisconnected(stopCalled)
    End Function
End Class

Public Class UsersMapping(Of ChatUser)
    Private ReadOnly _users As List(Of ChatUser) = New List(Of ChatUser)

    Public ReadOnly Property Count As Integer
        Get
            Return _users.Count
        End Get
    End Property

    Public Sub Add(ByVal user As ChatUser)
        SyncLock _users
            _users.Add(user)
        End SyncLock
    End Sub

    Public Function GetUsers() As IEnumerable(Of ChatUser)
        Return _users
    End Function
End Class