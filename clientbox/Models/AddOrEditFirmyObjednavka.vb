Imports System
Imports System.Collections.Generic

Public Class AddOrEditFirmyObjednavka
    Public Property IDObjednavkyPol As Nullable(Of Integer)
    Public Property FirmaObjednal As String
    Public Property Produkt As String
    Public Property Poznamka As String
    Public Property ObjednanoEMJ As Nullable(Of Decimal)
    Public Property DomluvenaProdejniCena As Nullable(Of Decimal)
    Public Property rr_DeadLine As Nullable(Of Short)
    Public Property DeadLineDatum As Nullable(Of Date)
    Public Property IDUserObjednal As Nullable(Of Integer)
    Public Property RadioObjednat As Boolean
    Public Property RadioObjednano As Boolean
    Public Property RadioZrusit As Boolean
End Class
