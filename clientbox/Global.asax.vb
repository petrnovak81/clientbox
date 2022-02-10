Imports System.Web.Http
Imports System.Web.OData.Extensions
Imports System.Web.Optimization
Imports System.Xml
Imports System.Xml.XPath

Public Class WebApiApplication
    Inherits System.Web.HttpApplication

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        GlobalConfiguration.Configure(Sub(config)
                                          config.EnableDependencyInjection()
                                          config.EnsureInitialized()
                                          config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                                          WebApiConfig.Register(config)
                                      End Sub)
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)

        ViewEngines.Engines.Clear()
        ViewEngines.Engines.Add(New RazorViewEngine())

    End Sub
End Class