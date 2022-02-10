<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:dt="urn:schemas-microsoft-com:datatypes" xmlns:fo="http://www.w3.org/1999/XSL/Format">
  <xsl:decimal-format name="cesky" decimal-separator="," grouping-separator=" " />
  <xsl:template match="/">
    <xsl:param name="mena" select="faktura/mena"/>
    <xsl:param name="dph" select="faktura/dph"/>
    <xsl:param name="cenacelkem" select="faktura/cenacelkem" />
    <xsl:param name="cenasdph" select="faktura/cenasdph" />
    <html xmlns="http://www.w3.org/1999/xhtml" xml:lang="cs" lang="cs">
      <head>
        <style type="text/css">
          * {
          font-family: Arial;
          font-size: 12px;
          }

          .w-100 {
          width: 100%;
          }

          .border-left {
          border-left: 1px solid #000000;
          }

          .border-top {
          border-top: 1px solid #000000;
          }

          .border-right {
          border-right: 1px solid #000000;
          }

          .border-bottom {
          border-bottom: 1px solid #000000;
          }

          .f1 {
          font-size: 12px;
          }

          .f2 {
          font-family: Arial;
          }

          b {
          font-size: 12px;
          }

          .fs1 {
          font-size: 48px;
          }

          .fs2 {
          font-size: 12px;
          }

          .m1 {
          transform: matrix(0.750000,0.000000,0.000000,0.375000,0,0);
          -ms-transform: matrix(0.750000,0.000000,0.000000,0.375000,0,0);
          -webkit-transform: matrix(0.750000,0.000000,0.000000,0.375000,0,0);
          }

          .page {
          background: white;
          width: 215mm;
          }

          .bg {
          background: #eeedee;
          }

          .text-right {
          text-align: right;
          }

          .fr {
          float: right;
          }

          table tr td {
          padding-top: 1px;
          padding-left: 10px;
          padding-right: 6px;
          }

          table tr:first-child td {
          padding-top: 10px;
          }

          .ptb {
          padding-top: 0;
          padding-bottom: 0;
          }

          .dot {
          border-bottom: 2px dotted #000000;
          }
        </style>
      </head>
      <body>
        <table class="w-100">
          <tr>
            <xsl:if test="faktura/logosrc != ''">
              <td width="40">
                <img width="30">
                  <xsl:attribute name="src">
                    <xsl:value-of select="faktura/logosrc"/>
                  </xsl:attribute>
                </img>
              </td>
            </xsl:if>
            <td class="f1">
              <b>FAKTURA</b>&#160;&#160;-&#160;&#160;DAŇOVÝ DOKLAD
            </td>
            <td></td>
            <td width="100">
              <b class="f1 fs2">číslo</b>
            </td>
            <td>
              <div class="f2 fs1 m1">
                <xsl:value-of select="faktura/dodavatel/vs" />
              </div>
            </td>
          </tr>
        </table>
        <table class="w-100 border-left border-top border-right" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="bottom" width="150" class="bg f1">
              <b class="f1">dodavatel</b>
              <div class="fr text-right">IČ</div>
            </td>
            <td valign="bottom" width="90" class="f2">
              <xsl:value-of select="faktura/dodavatel/ic" />
            </td>
            <td valign="bottom" width="40" class="bg f1">DIČ</td>
            <td valign="bottom" width="90" class="f2">
              <xsl:value-of select="faktura/dodavatel/dic" />
            </td>
            <td valign="bottom" width="150" class="bg f1 text-right">variabilní symbol</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:10px;">►</span>
              <b>
                <xsl:value-of select="faktura/dodavatel/vs" />
              </b>
            </td>
          </tr>
          <tr>
            <td valign="bottom"></td>
            <td valign="top" class="f2" rowspan="3" colspan="3">
              <xsl:value-of select="faktura/dodavatel/nazev" /><br />
              <xsl:value-of select="faktura/dodavatel/ulice" /><br />
              <xsl:value-of select="faktura/dodavatel/mesto" />
            </td>
            <td valign="bottom" class="bg f1 text-right">konstantní symbol</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:19px;">&#160;</span>
              <xsl:value-of select="faktura/dodavatel/ks" />
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="f2"></td>
            <td valign="bottom" class="bg f1 text-right">specifický symbol</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:19px;">&#160;</span>
              <xsl:value-of select="faktura/dodavatel/ss" />
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="f2"></td>
            <td valign="bottom" class="bg f1 text-right">částka</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:10px;">►</span>
              <b>=<xsl:value-of select="format-number($cenasdph,'### ###,##', 'cesky')"/>&#160;<xsl:value-of select="$mena" /></b>
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="bg f1 text-right">peněžní ústav</td>
            <td valign="bottom" class="f2" colspan="3">
              <xsl:value-of select="faktura/dodavatel/penezniustav" />
            </td>
            <td valign="bottom" class="bg f1 text-right">objednávka</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:19px;">&#160;</span>
              <xsl:value-of select="faktura/dodavatel/objednavka" />
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="bg f1 text-right">číslo účtu</td>
            <td valign="bottom" class="f2" colspan="3" style="padding-left:0;">
              <span style="margin-right:10px;">►</span>
              <b>
                <xsl:value-of select="faktura/dodavatel/cislouctu" />
              </b>
            </td>
            <td valign="bottom" class="bg f1 text-right">číslo odběratele</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:19px;">&#160;</span>
              <xsl:value-of select="faktura/dodavatel/cisloodberatele" />
            </td>
          </tr>
        </table>
        <table class="w-100 border-left border-top border-right" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="bottom" width="150" class="bg f1">
              <b class="f1">příjemce</b>
              <xsl:value-of select="faktura/dodavatel/prijemce" />
            </td>
            <td valign="bottom" width="150" rowspan="4" class="f2 border-right"></td>
            <td valign="bottom" width="150" class="bg f1">
              <b class="f1">odběratel</b>
              <div class="fr text-right">IČ</div>
            </td>
            <td valign="bottom" width="80" class="f2">
              <xsl:value-of select="faktura/odberatel/ic" />
            </td>
            <td valign="bottom" width="40" class="bg f1">DIČ</td>
            <td valign="bottom" class="f2">
              <xsl:value-of select="faktura/odberatel/dic" />
            </td>
          </tr>
          <tr>
            <td></td>
            <td></td>
            <td valign="bottom" class="f2" colspan="3" rowspan="3">
              <xsl:value-of select="faktura/odberatel/nazev" /><br />
              <xsl:value-of select="faktura/odberatel/ulice" /><br />
              <xsl:value-of select="faktura/odberatel/mesto" />
            </td>
          </tr>
          <tr>
            <td></td>
            <td></td>
          </tr>
          <tr>
            <td></td>
            <td></td>
          </tr>
        </table>
        <table class="w-100 border-left border-top border-right" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="bottom" width="150" class="f1"></td>
            <td valign="bottom" width="221" class="f2"></td>
            <td valign="bottom" width="180" class="bg f1 text-right">platební podmínky</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:19px;">&#160;</span>
              <xsl:value-of select="faktura/paltebnipodminky" />
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="f1"></td>
            <td valign="bottom" class="f2"></td>
            <td valign="bottom" class="bg f1 text-right">datum splatnosti</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:10px;">►</span>
              <b>
                <xsl:value-of select="faktura/datumsplatnosti" />
              </b>
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="f1"></td>
            <td valign="bottom" class="f2"></td>
            <td valign="bottom" class="bg f1 text-right"></td>
            <td valign="bottom" class="f2" style="padding-left:0;"></td>
          </tr>
          <tr>
            <td valign="bottom" class="f1"></td>
            <td valign="bottom" class="f2"></td>
            <td valign="bottom" class="bg f1 text-right">způsob úhrady</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:19px;">&#160;</span>
              <xsl:value-of select="faktura/zpusobuhrady" />
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="f1"></td>
            <td valign="bottom" class="f2"></td>
            <td valign="bottom" class="bg f1 text-right">datum vystavení dokladu</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:19px;">&#160;</span>
              <xsl:value-of select="faktura/datumvystaveni" />
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="f1"></td>
            <td valign="bottom" class="f2"></td>
            <td valign="bottom" class="bg f1 text-right">datum uskutečnění plnění</td>
            <td valign="bottom" class="f2" style="padding-left:0;">
              <span style="margin-right:19px;">&#160;</span>
              <xsl:value-of select="faktura/datumuskutecneni" />
            </td>
          </tr>
        </table>
        <table class="w-100 border-left border-right" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="bottom" class="bg f1">
              položka<div class="fr text-right">(ceny v Kč bez daně)</div>
            </td>
            <td valign="bottom" width="120" class="bg f1 text-right">jednotková cena</td>
            <td valign="bottom" width="120" class="bg f1 text-right">množství</td>
            <td valign="bottom" width="120" class="bg f1 text-right">celková cena</td>
            <td valign="bottom" width="120" class="bg f1 text-right">DPH</td>
          </tr>
        </table>
        <table class="w-100 f2" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="bottom" class="f2">
              <xsl:value-of select="faktura/textpred" />
            </td>
          </tr>
        </table>
        <table class="w-100" border="0" cellspacing="0" cellpadding="0">
          <xsl:for-each select="faktura/polozka">
            <tr>
            <td valign="bottom" class="f2" style="padding-top:10px;padding-bottom:10px">
              <xsl:value-of select="nazev" />
            </td>
            <td valign="bottom" width="120" class="f2 text-right">
              <xsl:value-of select="format-number(jenotkovacena,'### ###,##', 'cesky')" />&#160;<xsl:value-of select="$mena" />
            </td>
            <td valign="bottom" width="120" class="f2 text-right">
              <xsl:value-of select="format-number(mnozstvi,'### ###,##', 'cesky')" />&#160;<xsl:value-of select="mj" />
            </td>
            <td valign="bottom" width="120" class="f2 text-right">
              <xsl:value-of select="format-number(celkem,'### ###,##', 'cesky')" />&#160;<xsl:value-of select="$mena" />
            </td>
            <td valign="bottom" width="120" class="f2 text-right">
              <xsl:value-of select="$dph" />%
            </td>
          </tr>
          </xsl:for-each>
        </table>
        <table class="w-100" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="bottom" width="70" class="f2">celkem</td>
            <td valign="bottom" class="f2">
              <div style="border-bottom:1px dotted #000000;"></div>
            </td>
            <td valign="bottom" width="120" class="f2 text-right">
              <xsl:value-of select="format-number($cenacelkem,'### ###,##', 'cesky')" />&#160;<xsl:value-of select="$mena" />
            </td>
            <td valign="bottom" width="120" class="f2">Kč</td>
          </tr>
        </table>
        <table class="w-100" border="0" cellspacing="0" cellpadding="0" style="margin-top:10px;">
          <tr>
            <td valign="bottom" class="f2 text-right"></td>
            <td valign="bottom" width="120" class="f2 text-right border-left border-top">sazba</td>
            <td valign="bottom" width="120" class="f2 text-right border-top">bez daně</td>
            <td valign="bottom" width="120" class="f2 text-right border-top">DPH</td>
            <td valign="bottom" width="120" class="f2 text-right border-top">s daní</td>
            <td valign="bottom" width="120" class="f2 text-right border-top border-right"></td>
          </tr>
          <tr>
            <td valign="bottom" class="f2 text-right"></td>
            <td valign="bottom" width="120" class="f2 text-right border-left border-bottom">
              <xsl:value-of select="faktura/sazbatext" />
            </td>
            <td valign="bottom" width="120" class="f2 text-right border-bottom">
              <xsl:value-of select="format-number($cenacelkem,'### ###,##', 'cesky')" />
            </td>
            <td valign="bottom" width="120" class="f2 text-right border-bottom">
              <xsl:value-of select="format-number(faktura/dphcelkem,'### ###,##', 'cesky')" />
            </td>
            <td valign="bottom" width="120" class="f2 text-right border-bottom">
              <xsl:value-of select="format-number($cenasdph,'### ###,##', 'cesky')" />
            </td>
            <td valign="bottom" width="120" class="f2 text-right border-bottom border-right">
              <xsl:value-of select="faktura/dph" />%
            </td>
          </tr>
        </table>
        <table class="w-100" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="bottom" width="70" class="f2">k úhradě</td>
            <td valign="bottom" class="f2">
              <div style="border-bottom:1px dotted #000000;"></div>
            </td>
            <td valign="bottom" width="120" class="f2 text-right">
              <xsl:value-of select="format-number($cenasdph,'### ###,##', 'cesky')" />&#160;<xsl:value-of select="$mena" />
            </td>
            <td valign="bottom" width="120" class="f2">
              
            </td>
          </tr>
        </table>
        <table class="w-100" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="bottom" class="f2">
              <xsl:value-of select="faktura/textza" />
            </td>
          </tr>
        </table>
        <table class="f2" style="margin-top:10px;" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="bottom" class="f2 text-right">Počet stran</td>
            <td valign="bottom" class="f2">
              <xsl:value-of select="position()"/>
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="f2 text-right">Vystavil</td>
            <td valign="bottom" class="f2">
              <xsl:value-of select="faktura/vystavil" />
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="f2 text-right">Telefon</td>
            <td valign="bottom" class="f2">
              <xsl:value-of select="faktura/telefon" />
            </td>
          </tr>
          <tr>
            <td valign="bottom" class="f2 text-right">Email</td>
            <td valign="bottom" class="f2">
              <xsl:value-of select="faktura/email" />
            </td>
          </tr>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
