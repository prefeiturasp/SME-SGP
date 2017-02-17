<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCHabilidades.ascx.cs" Inherits="GestaoEscolar.WebControls.Habilidades.UCHabilidades" %>
<fieldset>
    <legend>
        <span style="position:relative;">
            <asp:Label ID="lblTituloFildSet" runat="server" style="float:left; text-align:left !important"></asp:Label>
            <asp:Label ID="lblLegendaCheck" runat="server" style="float:right; padding-right:8%; text-align:right !important"></asp:Label>
            <span style="position: absolute; right: 0; top: 4px;">
                <span style="display: inline-block; width: 145px; text-align: center; border-left: 1px solid #fff;"></span>
            </span>
        </span>
    </legend>
    <div></div>
    <div id="divArvoreCOC" class="divArvoreCOC" style="width: 145px;">
    </div>
    <div class="divTreeviewScrollCOC" style="z-index: 9999 !important;">
        <asp:Repeater ID="rptHabilidades" runat="server" OnItemDataBound="rptHabilidades_ItemDataBound">
            <ItemTemplate>
                <asp:Literal ID="litCabecalho" runat="server"></asp:Literal>
                <asp:HiddenField ID="hdnPermiteLancamento" runat="server" Value='<%# Eval("PermiteLancamento") %>' />
                <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%# Eval("Posicao") %>' />

                <asp:HiddenField ID="hdnOrientacaoAvaliacao" runat="server" />

                <span style="padding-right: 145px; display: block;">
                    <asp:Literal ID="litConteudo" runat="server"></asp:Literal></span>
                <div style="display: table-row;" id="divHabilidade" runat="server">
                    <asp:HiddenField ID="hdnChave" runat="server" Value='<%# Eval("Chave") %>' />
                    <span style="display: table-cell; text-align: left; vertical-align: top;">
                        <asp:Literal ID="lblHabilidade" runat="server"></asp:Literal>
                    </span><span style="display: table-cell; width: 145px; text-align: center; vertical-align: top; vertical-align: middle;" class="alcancadoAvaliacao">
                        <asp:CheckBox ID="chkAlcancado" runat="server"></asp:CheckBox><br />
                        <asp:Label ID="lblLegendaDiagInicialOrientacao" runat="server" Text="" Visible="true"
                            CssClass="nivelAprendizado"></asp:Label>
                    </span>
                </div>
                <asp:Literal ID="litRodape" runat="server"></asp:Literal>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</fieldset>