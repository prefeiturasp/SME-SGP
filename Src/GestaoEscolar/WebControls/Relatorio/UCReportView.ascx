<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Relatorio_UCReportView" CodeBehind="UCReportView.ascx.cs" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<script type="text/javascript">
    var dir;
    var imprimir = '<%= this.HabilitarImpressaoRel %>';
    var ControlName = '<%= this.ReportViewerRel.ClientID %>';

    if (imprimir.toLowerCase() == 'true') {
        $(document).ready(function () {
            try {
                dir = '<%= Page.ResolveClientUrl("~")%>';
                var innerScript = '<scr' + 'ipt type="text/javascript">document.getElementById("' + ControlName + '_print").Controller = new ReportViewerHoverButton("' + ControlName + '_print", false, "", "", "", "#ECE9D8", "#DDEEF7", "#99BBE2", "1px #ECE9D8 Solid", "1px #336699 Solid", "1px #336699 Solid");</scr' + 'ipt>';
                var innerTbody = '<tbody><tr><td><input id="botao" type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Imprimir relatório" src="' + dir + 'Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Imprimir relatório"></td></tr></tbody>';
                var innerTable = '<table title="Imprimir relatório" onmouseout="this.Controller.OnNormal();" onmouseover="this.Controller.OnHover();" onclick="PrintFunc(); return false;" id="' + ControlName + '_print" style="border: 1px solid rgb(236, 233, 216); background-color: rgb(236, 233, 216); cursor: default;">' + innerScript + innerTbody + '</table>'
                var outerScript = '<scr' + 'ipt type="text/javascript">document.getElementById("' + ControlName + '_print").Controller.OnNormal();</scr' + 'ipt>';
                var outerDiv = '<div style="display: inline; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + outerScript + '</td></tr></tbody></table></div>';
                $("#" + ControlName + " > div > div").append(outerDiv);
            }
            catch (e) { }
        });
    }

    $(document).ready(function () {
        if ($.browser.safari) {
            $('#' + ControlName + ' table:not([title="Voltar"])').each(function (i, item) {
                $(item).css('display', 'inline-block');
            });
        }
    });

    function PrintFunc() {
        var urlReport = dir + 'report.ashx';
        $('#ifPrint').attr('src', urlReport + '?' + query);
    }
</script>

<asp:Label ID="_lblMensagem" runat="server" EnableViewState="False"></asp:Label>
<div align="center" style="height: 100%;">
    <rsweb:ReportViewer ID="ReportViewerRel" runat="server" Width="900px"
        Height="470px" ShowParameterPrompts="False" ShowFindControls="False"
        ShowPrintButton="False" AsyncRendering="True" ShowRefreshButton="False" CssClass="rel-responsive">
    </rsweb:ReportViewer>
</div>
<div id="divPdf" runat="server" style="width: 0; height: 0;">
    <iframe id="ifPrint" height="0" width="0" frameborder="0"></iframe>
</div>

