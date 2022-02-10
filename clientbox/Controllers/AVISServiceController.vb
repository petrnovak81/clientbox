Imports System.Data.Entity.Core.Objects
Imports System.Net
Imports System.ServiceModel
Imports System.Web.Http

Public Class AVISServiceController
    Inherits ApiController

    Function _lastLapse(id As Integer) As String
        Using db As New Data4995Entities
            Dim dt = db.AG_tblRegisterRestrictions.FirstOrDefault(Function(e) e.Register = "LL_LastLapse" And e.IDOrder = id)
            If dt IsNot Nothing Then
                Return dt.Val
            Else
                Return Nothing
            End If
        End Using
    End Function

    Private Shared Function KontaktyDoctorumService() As ServiceReferenceDoctorumKontakty.KontaktyClient
        Dim sc = New ServiceReferenceDoctorumKontakty.KontaktyClient()
        sc.ClientCredentials.UserName.UserName = "Hanzl"
        sc.ClientCredentials.UserName.Password = "0256"
        Return sc
    End Function

    Private Shared Function DokladyDoctorumService() As ServiceReferenceDoctorumDoklady.DokladyClient
        Dim sc = New ServiceReferenceDoctorumDoklady.DokladyClient()
        sc.ClientCredentials.UserName.UserName = "Hanzl"
        sc.ClientCredentials.UserName.Password = "0256"
        Return sc
    End Function

    Private Shared Function KontaktyAgiloService() As ServiceReferenceAgiloKontakty.KontaktyClient
        Dim sc = New ServiceReferenceAgiloKontakty.KontaktyClient()
        sc.ClientCredentials.UserName.UserName = "Hanzl"
        sc.ClientCredentials.UserName.Password = "0256"
        Return sc
    End Function

    Private Shared Function DokladyAgiloService() As ServiceReferenceAgiloDoklady.DokladyClient
        Dim sc = New ServiceReferenceAgiloDoklady.DokladyClient()
        sc.ClientCredentials.UserName.UserName = "Hanzl"
        sc.ClientCredentials.UserName.Password = "0256"
        Return sc
    End Function

    'Private Shared Function VytvoritDoklad() As ServiceReferenceDoklady.DokladContract
    '    Dim doklad = New ServiceReferenceDoklady.DokladContract()
    '    doklad.Kniha = "Vydané doklady"
    '    doklad.TypZaznamu = "FV"
    '    doklad.Poznamka = "Demo AVIS"
    '    doklad.Mena = "CZK"
    '    doklad.Firma = "KVALIDENT s.r.o."
    '    doklad.Datum = DateTime.Today
    '    doklad.DatumZdanitelnehoPlneni = DateTime.Today
    '    doklad.DorucovaciAdresa = New ServiceReferenceDoklady.AdresaContract With {
    '    .Ulice = "Široká 45",
    '    .Okres = "Králíkárna",
    '    .PSC = "132 99",
    '    .Obec = "Králík"
    '}
    '    Dim polozky = New List(Of ServiceReferenceDoklady.DokladPolozkaContract)()
    '    Dim polozka = New ServiceReferenceDoklady.DokladPolozkaContract()
    '    polozka.MnozstviJednotek = 1
    '    polozka.NazevSazbyDPH = "Základní"
    '    polozka.Produkt = "eKompas"
    '    polozka.SazbaDPH = 0
    '    polozka.CelkemDPH = 0
    '    polozka.CenaCelkemBezDPH = 125
    '    polozka.CenaCelkemVcetneDPH = 125
    '    polozka.ExtendedProperties = New ServiceReferenceDoklady.ExtendedPropertyContract(1) {}
    '    polozka.ExtendedProperties(0) = New ServiceReferenceDoklady.ExtendedPropertyContract() With {
    '    .Name = "udaj_polozky_1",
    '    .StringValue = "Hodnota 1",
    '    .DBType = 10
    '}
    '    polozka.ExtendedProperties(1) = New ServiceReferenceDoklady.ExtendedPropertyContract() With {
    '    .Name = "udaj_polozky_2",
    '    .StringValue = "Hodnota 2",
    '    .DBType = 10
    '}
    '    polozky.Add(polozka)
    '    doklad.Polozky = polozky.ToArray()
    '    doklad.CelkemVcetneDPH = doklad.Polozky.Select(Function(p) p.CenaCelkemVcetneDPH).Sum()
    '    doklad.CelkemBezDPH = doklad.Polozky.Select(Function(p) p.CenaCelkemBezDPH).Sum()
    '    doklad.ZaokrouhlovatSoucet = 0.01D
    '    Return doklad
    'End Function

    '<HttpGet>
    'Public Function nacist_kontakt(firma As String) As Object
    '    Try
    '        Using kontakty = KontaktyService()
    '            Dim kontakt = kontakty.NacistKontakt(firma)
    '            Return New With {.data = kontakt, .error = Nothing}
    '        End Using
    '    Catch ex As Exception
    '        While ex.InnerException IsNot Nothing
    '            ex = ex.InnerException
    '        End While
    '        Return New With {.data = Nothing, .error = ex}
    '    End Try
    'End Function

    '<HttpGet>
    'Public Function nacist_kontakty() As Object
    '    Try
    '        Using kontakty = KontaktyService()
    '            Dim pocet = kontakty.NacistPocetVsechKontaktu()
    '            Dim list = kontakty.NacistVsechnyKontakty(1, 10)
    '            Return New With {.data = list, .error = Nothing}
    '        End Using
    '    Catch ex As Exception
    '        While ex.InnerException IsNot Nothing
    '            ex = ex.InnerException
    '        End While
    '        Return New With {.data = Nothing, .error = ex}
    '    End Try
    'End Function

    '<HttpGet>
    'Public Function nacist_doklad(cisloDokladu As String) As Object
    '    Try
    '        Using doklady = DokladyService()
    '            Dim doklad = doklady.NacistDoklad(cisloDokladu)
    '            Return New With {.data = doklad, .error = Nothing}
    '        End Using
    '    Catch ex As Exception
    '        While ex.InnerException IsNot Nothing
    '            ex = ex.InnerException
    '        End While
    '        Return New With {.data = Nothing, .error = ex}
    '    End Try
    'End Function

    '<HttpPost>
    'Public Function novy_doklad(doklad As ServiceReferenceDoklady.DokladContract) As Object
    '    Try
    '        Dim sc = DokladyService()

    '        doklad.CelkemVcetneDPH = doklad.Polozky.Select(Function(p) p.CenaCelkemVcetneDPH).Sum()
    '        doklad.CelkemBezDPH = doklad.Polozky.Select(Function(p) p.CenaCelkemBezDPH).Sum()
    '        doklad.ZaokrouhlovatSoucet = 0.01D

    '        '1.1. vložením dokladu získáme rowguid, který přidělilo Vario
    '        doklad.rowguid = sc.VlozitNovyDoklad(doklad)

    '        '1.2. načteme doklad, abychom získali i všechny hodnoty, které doplnilo Vario
    '        doklad = sc.NacistDokladRG(doklad.rowguid)

    '        Return New With {.data = doklad, .error = Nothing}
    '    Catch ex As Exception
    '        While ex.InnerException IsNot Nothing
    '            ex = ex.InnerException
    '        End While
    '        Return New With {.data = Nothing, .error = ex}
    '    End Try
    'End Function
    <HttpGet>
    Public Function dostupnost_sluzby() As Object
        Dim agilo = "http://192.168.10.98:64990/AGILO/Altus.COM.V12.BasicServices.Doklady?wsdl"
        Dim docto = "http://192.168.10.98:64999/DOCTORUM.CZ/Altus.COM.V12.BasicServices.Doklady?wsdl"
        Dim messages As New List(Of String)
        Try
            Dim req = CType(WebRequest.Create(agilo), HttpWebRequest)
            Dim res = CType(req.GetResponse(), HttpWebResponse)
            If res.StatusCode = HttpStatusCode.OK Then
                messages.Add("Služba pro AGILO je spuštěna.")
            Else
                messages.Add(String.Format("Služba pro AGILO vrací status: {0}. Kontaktuj Marka.", res.StatusDescription))
            End If
        Catch ex As Exception
            messages.Add(String.Format("Služba pro AGILO je nedostupná: {0}. Kontaktuj Marka.", ex.Message))
        End Try
        Try
            Dim req = CType(WebRequest.Create(docto), HttpWebRequest)
            Dim res = CType(req.GetResponse(), HttpWebResponse)
            If res.StatusCode = HttpStatusCode.OK Then
                messages.Add("Služba pro DOCTORUM je spuštěna.")
            Else
                messages.Add(String.Format("Služba pro DOCTORUM vrací status: {0}. Kontaktuj Marka.", res.StatusDescription))
            End If
        Catch ex As Exception
            messages.Add(String.Format("Služba pro DOCTORUM je nedostupná: {0}. Kontaktuj Marka.", ex.Message))
        End Try
        Return messages
    End Function

    <HttpGet>
    Public Function uvolnit_licence() As Object
        Try
            Using db As New Data4995Entities
                db.AGsp_UTIL_Do_UvolnitSekleLicence()
                Return New With {.data = Nothing, .error = Nothing}
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex}
        End Try
    End Function

    <HttpGet>
    Public Function sto_mega_getresult(resultType As String) As Object
        Try
            Dim result As System.Xml.XmlNode,
                xmlString = Nothing
            Dim sc = New ServiceReference100Mega.I6WebServiceSoapClient()
            sc.ClientCredentials.UserName.UserName = "9agiloex"
            sc.ClientCredentials.UserName.Password = "xe4261"

            Dim httpRequestProperty As Channels.HttpRequestMessageProperty = New Channels.HttpRequestMessageProperty()
            Using scope As OperationContextScope = New OperationContextScope(sc.InnerChannel)
                httpRequestProperty.Headers(System.Net.HttpRequestHeader.Authorization) = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(sc.ClientCredentials.UserName.UserName + ":" + sc.ClientCredentials.UserName.Password))
                OperationContext.Current.OutgoingMessageProperties(Channels.HttpRequestMessageProperty.Name) = httpRequestProperty
                result = sc.GetResult(resultType)
            End Using

            If result IsNot Nothing Then
                xmlString = result.InnerXml.ToString
                Using db As New Data4995Entities
                    If resultType = "Order" Then
                        db.AGsp_AddMegaObjednavkaPol(xmlString)
                    End If
                    If resultType = "DocTrInv" Then
                        db.AGsp_AddMegaFakturaPol(xmlString)
                    End If
                End Using
            End If

            Return New With {.data = result, .error = Nothing}
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex}
        End Try
    End Function

    <HttpGet>
    Public Function sto_mega_getresultbycode(resultType As String, code As String) As Object
        Try
            Dim result As System.Xml.XmlNode,
                xmlString = Nothing
            Dim sc = New ServiceReference100Mega.I6WebServiceSoapClient()
            sc.ClientCredentials.UserName.UserName = "9agiloex"
            sc.ClientCredentials.UserName.Password = "xe4261"

            Dim httpRequestProperty As Channels.HttpRequestMessageProperty = New Channels.HttpRequestMessageProperty()
            Using scope As OperationContextScope = New OperationContextScope(sc.InnerChannel)
                httpRequestProperty.Headers(System.Net.HttpRequestHeader.Authorization) = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(sc.ClientCredentials.UserName.UserName + ":" + sc.ClientCredentials.UserName.Password))
                OperationContext.Current.OutgoingMessageProperties(Channels.HttpRequestMessageProperty.Name) = httpRequestProperty
                result = sc.GetResultByCode(resultType, code)
            End Using

            Return New With {.data = result, .error = Nothing}
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex}
        End Try
    End Function

    <HttpGet>
    Public Function CilovaAVISFirma(iDVykazPrace As Integer) As Object
        Try
            Dim rr_FakturovatNaFirmu As New ObjectParameter("rr_FakturovatNaFirmu", GetType(Integer))
            Dim pocetDEalsichFaktur As New ObjectParameter("PocetDEalsichFaktur", GetType(Integer))
            Dim iDFirmy As New ObjectParameter("Firma", GetType(String))
            Using db As New Data4995Entities
                db.AGsp_GetFA_CilovaAVISFirma(iDVykazPrace, rr_FakturovatNaFirmu, pocetDEalsichFaktur, iDFirmy)
                Dim d = New With {
                .iDFirmy = iDFirmy.Value,
                .iDVykazPrace = iDVykazPrace,
                .rr_FakturovatNaFirmu = rr_FakturovatNaFirmu.Value,
                .pocetDEalsichFaktur = pocetDEalsichFaktur.Value
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

    <HttpGet>
    Public Function AVISFakturace(typ As Integer, iDVykazPrace As Integer, rr_FakturovatNaFirmu As Integer) As Object
        Try
            If typ = 2 Then
                Using db As New Data4995Entities
                    db.AGsp_GetFA_SloucitNaJednuFakturu(iDVykazPrace)
                End Using
            End If

            If rr_FakturovatNaFirmu = 1 Then
                'kdyz agilo
                Return novy_doklad_agilo(iDVykazPrace)
            Else
                'kdyz doctorum
                Return novy_doklad_doctorum(iDVykazPrace)
            End If
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex}
        End Try
    End Function

    <HttpGet>
    Public Function novy_doklad_doctorum(iDVykazPrace As Integer) As Object
        Try
            Dim lastLapse1 As New ObjectParameter("LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim hlavicka = db.AGsp_GetFA_Hlavicka(iDVykazPrace, lastLapse1).FirstOrDefault
                If hlavicka IsNot Nothing Then
                    If lastLapse1.Value = 0 Then
                        Dim sc = DokladyDoctorumService()
                        Dim doklad As New ServiceReferenceDoctorumDoklady.DokladContract
                        doklad.Firma = hlavicka.Firma
                        doklad.ICO = hlavicka.ICO
                        doklad.Email = hlavicka.Email
                        doklad.Telefon = hlavicka.Telefon
                        doklad.Kniha = hlavicka.Kniha
                        doklad.Text = hlavicka.Text
                        doklad.TypZaznamu = hlavicka.TypZaznamu
                        doklad.Mena = hlavicka.Mena
                        doklad.Datum = hlavicka.Datum.ToString("d")
                        doklad.DatumZdanitelnehoPlneni = hlavicka.DatumZdanitelnehoPlneni.ToString("d")

                        Dim produktyNaPolozky = db.AGsp_GetFA_Polozky(iDVykazPrace).ToList
                        Dim polozky As New List(Of ServiceReferenceDoctorumDoklady.DokladPolozkaContract)
                        For Each produkt In produktyNaPolozky
                            Dim polozka As New ServiceReferenceDoctorumDoklady.DokladPolozkaContract()
                            polozka.Poradi = produkt.Poradi
                            polozka.Produkt = produkt.Produkt
                            polozka.Popis = produkt.TextNaFakturu
                            polozka.MnozstviJednotek = produkt.PocetEMJ
                            polozka.Jednotky = produkt.Jednotky
                            polozka.NazevSazbyDPH = produkt.NazevSazbyDPH
                            polozka.SazbaDPH = Math.Round(produkt.SazbaDPH, 2)
                            polozka.CenaCelkemBezDPH = Math.Round(CDec(produkt.CenaCelkemBezDPH), 2)
                            polozka.CelkemDPH = Math.Round(CDec(produkt.CenaCelkemBezDPH * (produkt.SazbaDPH / 100)), 2)
                            polozka.CenaCelkemVcetneDPH = (polozka.CenaCelkemBezDPH + polozka.CelkemDPH)
                            polozka.Zakazka = produkt.Zakazka

                            polozky.Add(polozka)
                        Next

                        doklad.Polozky = polozky.ToArray
                        doklad.CelkemVcetneDPH = Math.Round(doklad.Polozky.Select(Function(p) p.CenaCelkemVcetneDPH).Sum(), 2)
                        doklad.CelkemBezDPH = Math.Round(doklad.Polozky.Select(Function(p) p.CenaCelkemBezDPH).Sum(), 2)
                        doklad.ZaokrouhlovatSoucet = 0.01D

                        '1.1. vložením dokladu získáme rowguid, který přidělilo Vario
                        doklad.rowguid = sc.VlozitNovyDoklad(doklad)

                        '1.2. načteme doklad, abychom získali i všechny hodnoty, které doplnilo Vario
                        doklad = sc.NacistDokladRG(doklad.rowguid)

                        Dim lastLapse2 As New ObjectParameter("LL_LastLapse", GetType(Integer))
                        db.AGsp_Run_Pracak20to30(iDVykazPrace, doklad.CisloDokladu, lastLapse2)
                        If lastLapse2.Value > 0 Then
                            Dim msg = _lastLapse(lastLapse2.Value)
                            Return New With {.data = Nothing, .error = msg}
                        Else
                            Return New With {.data = doklad.CisloDokladu, .error = Nothing}
                        End If
                    Else
                        Dim msg = _lastLapse(lastLapse1.Value)
                        Return New With {.data = Nothing, .error = msg}
                    End If
                Else
                    If lastLapse1.Value = 0 Then
                        Return New With {.data = Nothing, .error = "SQL Procedůra AGsp_GetFA_Hlavicka(@IDVykazPrace = " & iDVykazPrace & ", @LastLapse = 0) nevrátila data pro hlavičku dokladu. Informuj Marka."}
                    Else
                        Return New With {.data = Nothing, .error = _lastLapse(lastLapse1.Value)}
                    End If
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
    Public Function novy_doklad_agilo(iDVykazPrace As Integer) As Object
        Try
            Dim lastLapse1 As New ObjectParameter("LastLapse", GetType(Integer))
            Using db As New Data4995Entities
                Dim hlavicka = db.AGsp_GetFA_Hlavicka(iDVykazPrace, lastLapse1).FirstOrDefault
                If hlavicka IsNot Nothing Then
                    If lastLapse1.Value = 0 Then
                        Dim sc = DokladyAgiloService()
                        Dim doklad As New ServiceReferenceAgiloDoklady.DokladContract
                        doklad.Firma = hlavicka.Firma
                        doklad.ICO = hlavicka.ICO
                        doklad.Kniha = hlavicka.Kniha
                        doklad.Text = hlavicka.Text
                        doklad.TypZaznamu = hlavicka.TypZaznamu
                        doklad.Mena = hlavicka.Mena
                        doklad.Datum = hlavicka.Datum.ToString("d")
                        doklad.DatumZdanitelnehoPlneni = hlavicka.DatumZdanitelnehoPlneni.ToString("d")

                        Dim produktyNaPolozky = db.AGsp_GetFA_Polozky(iDVykazPrace).ToList
                        Dim polozky As New List(Of ServiceReferenceAgiloDoklady.DokladPolozkaContract)
                        For Each produkt In produktyNaPolozky
                            Dim polozka As New ServiceReferenceAgiloDoklady.DokladPolozkaContract()
                            polozka.Poradi = produkt.Poradi
                            polozka.Produkt = produkt.Produkt
                            polozka.Popis = produkt.TextNaFakturu
                            polozka.MnozstviJednotek = produkt.PocetEMJ
                            polozka.Jednotky = produkt.Jednotky
                            polozka.NazevSazbyDPH = produkt.NazevSazbyDPH
                            polozka.SazbaDPH = Math.Round(produkt.SazbaDPH, 2)
                            polozka.CenaCelkemBezDPH = Math.Round(CDec(produkt.CenaCelkemBezDPH), 2)
                            polozka.CelkemDPH = Math.Round(CDec(produkt.CenaCelkemBezDPH * produkt.SazbaDPH / 100), 2)
                            polozka.CenaCelkemVcetneDPH = (polozka.CenaCelkemBezDPH + polozka.CelkemDPH)
                            polozka.Zakazka = produkt.Zakazka

                            polozky.Add(polozka)
                        Next

                        doklad.Polozky = polozky.ToArray
                        doklad.CelkemVcetneDPH = Math.Round(doklad.Polozky.Select(Function(p) p.CenaCelkemVcetneDPH).Sum(), 2)
                        doklad.CelkemBezDPH = Math.Round(doklad.Polozky.Select(Function(p) p.CenaCelkemBezDPH).Sum(), 2)
                        doklad.ZaokrouhlovatSoucet = 0.01D

                        '1.1. vložením dokladu získáme rowguid, který přidělilo Vario
                        doklad.rowguid = sc.VlozitNovyDoklad(doklad)

                        '1.2. načteme doklad, abychom získali i všechny hodnoty, které doplnilo Vario
                        doklad = sc.NacistDokladRG(doklad.rowguid)

                        Dim lastLapse2 As New ObjectParameter("LL_LastLapse", GetType(Integer))
                        db.AGsp_Run_Pracak20to30(iDVykazPrace, doklad.CisloDokladu, lastLapse2)
                        If lastLapse2.Value > 0 Then
                            Dim msg = _lastLapse(lastLapse2.Value)
                            Return New With {.data = Nothing, .error = msg}
                        Else
                            Return New With {.data = doklad.CisloDokladu, .error = Nothing}
                        End If
                    Else
                        Dim msg = _lastLapse(lastLapse1.Value)
                        Return New With {.data = Nothing, .error = msg}
                    End If
                Else
                    If lastLapse1.Value = 0 Then
                        Return New With {.data = Nothing, .error = "SQL Procedůra AGsp_GetFA_Hlavicka(@IDVykazPrace = " & iDVykazPrace & ", @LastLapse = 0) nevrátila data pro hlavičku dokladu. Informuj Marka."}
                    Else
                        Return New With {.data = Nothing, .error = _lastLapse(lastLapse1.Value)}
                    End If
                End If
            End Using
        Catch ex As Exception
            While ex.InnerException IsNot Nothing
                ex = ex.InnerException
            End While
            Return New With {.data = Nothing, .error = ex.Message}
        End Try
    End Function


End Class