Imports System.Xml.Serialization
Imports Newtonsoft.Json

Namespace Xml
    <XmlRoot("servicetasks")>
    Public Class servicetasks
        <XmlElement("starthour")>
        Public Property starthour As Integer
        <XmlElement("interval")>
        Public Property interval As Integer
        <XmlElement("timerpause")>
        Public Property timerpause As Integer
        <XmlElement("notification")>
        Public Property notification As String
        <XmlElement("backup")>
        Public Property backup As List(Of backup)
        <XmlElement("sendemail")>
        Public Property sendemail As email
    End Class

    Public Class backup
        <XmlElement("item")>
        Public Property item As item
    End Class

    Public Class item
        <XmlElement("type")>
        Public Property type As String
        <XmlElement("description")>
        Public Property description As String
        <XmlElement("extensions")>
        Public Property extensions As String
        <XmlElement("path")>
        Public Property path As String
    End Class

    Public Class email
        <XmlElement("address")>
        Public Property address As List(Of address)
        <XmlElement("subject")>
        Public Property subject As String
        <XmlElement("body")>
        Public Property body As String
    End Class

    Public Class address
        <XmlArrayItem("to")>
        Public Property [to] As String
    End Class
End Namespace

Namespace Js
    Public Class servicetasks
        <JsonProperty("starthour")>
        Public Property starthour As Integer
        <JsonProperty("interval")>
        Public Property interval As Integer
        <JsonProperty("timerpause")>
        Public Property timerpause As Integer
        <JsonProperty("notification")>
        Public Property notification As String
        <JsonProperty("backup")>
        <JsonConverter(GetType(List(Of item)))>
        Public Property backup As List(Of item)
        <JsonProperty("sendemail")>
        Public Property sendemail As email
    End Class

    Public Class backup
        <JsonProperty("item")>
        Public Property item As item
    End Class

    Public Class item
        <JsonProperty("type")>
        Public Property type As String
        <JsonProperty("description")>
        Public Property description As String
        <JsonProperty("extensions")>
        Public Property extensions As String
        <JsonProperty("path")>
        Public Property path As String
    End Class

    Public Class email
        <JsonProperty("address")>
        Public Property address As List(Of address)
        <JsonProperty("subject")>
        Public Property subject As String
        <JsonProperty("body")>
        Public Property body As String
    End Class

    Public Class address
        <XmlElement("to")>
        Public Property [to] As String
    End Class
End Namespace
