Imports System.Timers
Imports Microsoft.AspNet.SignalR
Imports Microsoft.Owin
Imports Microsoft.Owin.Cors
Imports Owin

<Assembly: OwinStartup(GetType(SignalR.Startup))>
Namespace SignalR

    Public Class Startup

        Public Sub Configuration(ByVal app As IAppBuilder)

            'Dim idProvider = New SignaRIDUserProvider()
            'GlobalHost.DependencyResolver.Register(GetType(IUserIdProvider), Function() idProvider)

            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(120)
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(120)
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(30)

            Dim _timer As New System.Timers.Timer()
            AddHandler _timer.Elapsed, AddressOf _timer_elapsed
            _timer.Interval = 30000
            _timer.Enabled = True

            Dim config = New HubConfiguration()
            config.EnableJSONP = True
            app.MapSignalR(config)

            'app.Map("/signalr", Function(map)
            '                        map.UseCors(CorsOptions.AllowAll)
            '                        Dim hubConfiguration = New Microsoft.AspNet.SignalR.HubConfiguration With {.EnableJavaScriptProxies = True, .EnableJSONP = True}
            '                        map.RunSignalR(hubConfiguration)
            '                    End Function)

        End Sub

        Private Sub _timer_elapsed(sender As Object, e As ElapsedEventArgs)
            Dim context = GlobalHost.ConnectionManager.GetHubContext(Of AgiloHub)()
            context.Clients.All.tick()
        End Sub
    End Class
End Namespace