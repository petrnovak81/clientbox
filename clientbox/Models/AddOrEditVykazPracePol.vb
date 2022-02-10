Imports System
Imports System.Collections.Generic

Public Class AddOrEditVykazPracePol
    Public Property iDVykazPrace As Nullable(Of Integer)
    Public Property iDVykazPracePol As Nullable(Of Integer)
    Public Property iDUserUpravil As Nullable(Of Integer)
    Public Property rr_TypPolozkyPracaku As Nullable(Of Short)
    Public Property casOd As Nullable(Of System.TimeSpan)
    Public Property casDo As Nullable(Of System.TimeSpan)
    Public Property hodin As Nullable(Of Decimal)
    Public Property iDTechnika As Nullable(Of Integer)
    Public Property produkt As String
    Public Property textNaFakturu As String
    Public Property textInterniDoMailu As String
    Public Property pocetEMJ As Nullable(Of Decimal)
    Public Property cenaEMJ As Nullable(Of Decimal)
    Public Property vzdalenka As Nullable(Of Boolean)
    Public Property zdarma As Nullable(Of Boolean)
    Public Property navzdoryServisceUctovat As Nullable(Of Boolean)
    Public Property najetoKM As Nullable(Of Decimal)
    Public Property rr_HodinoveUctovani As Nullable(Of Short)
End Class
