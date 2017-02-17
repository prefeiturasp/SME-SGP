<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCConfirmacaoOperacao.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Mensagens.UCConfirmacaoOperacao" %>
<%@ Register Src="UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<div id="divConfirmacao" title="Confirmação" class="hide">
    <asp:UpdatePanel ID="updConfirmacaoOperacao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ConfirmacaoPadrao" />
            <fieldset>
                <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" Visible="false" />
                <asp:Label ID="lblConfirmacao" runat="server" Text="Confirmação"></asp:Label>
                <div id="divObservacao" runat="server" visible="false">
                    <br />
                    <asp:Label ID="lblObservacao" runat="server" Text="Observação"></asp:Label>
                    <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvObservacao" runat="server" ErrorMessage="Observação é obrigatório."
                        ControlToValidate="txtObservacao" ValidationGroup="ConfirmacaoPadrao" Visible="false">*</asp:RequiredFieldValidator>
                </div>
                <div class="right">
                    <asp:Button ID="btnSim" runat="server" ValidationGroup="ConfirmacaoPadrao" OnClick="btnSim_Click"
                        Text="Sim" />
                    <asp:Button ID="btnNao" runat="server" CausesValidation="False" OnClientClick="$('#divConfirmacao').dialog('close'); return false;"
                        Text="Não" />
                    <asp:Button ID="btnNaoClick" runat="server" CausesValidation="False" OnClick="btnNao_Click"
                        Text="Não" Visible="false" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>