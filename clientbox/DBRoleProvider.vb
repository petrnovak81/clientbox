Public Class DBRoleProvider
    Inherits RoleProvider

    Public Overrides Property ApplicationName As String
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Overrides Sub AddUsersToRoles(usernames() As String, roleNames() As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub CreateRole(roleName As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub RemoveUsersFromRoles(usernames() As String, roleNames() As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Function DeleteRole(roleName As String, throwOnPopulatedRole As Boolean) As Boolean
        Throw New NotImplementedException()
    End Function

    Public Overrides Function FindUsersInRole(roleName As String, usernameToMatch As String) As String()
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetAllRoles() As String()
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetRolesForUser(username As String) As String()
        Using db As New Data4995Entities
            Dim allroles As String() = Nothing
            Dim user = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = username)
            If user IsNot Nothing Then
                Return New String() {user.IDRole, user.UserLogin}
            Else
                Return New String() {"0"}
            End If
        End Using
    End Function

    Public Overrides Function GetUsersInRole(roleName As String) As String()
        Throw New NotImplementedException()
    End Function

    Public Overrides Function IsUserInRole(username As String, roleName As String) As Boolean
        Throw New NotImplementedException()
    End Function

    Public Overrides Function RoleExists(roleName As String) As Boolean
        Throw New NotImplementedException()
    End Function
End Class
