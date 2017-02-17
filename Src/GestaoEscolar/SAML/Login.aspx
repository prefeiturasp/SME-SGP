<%@ Page Language="C#" MasterPageFile="~/SAML/MasterPage.master" AutoEventWireup="true" Inherits="SAML_Login" ValidateRequest="false" Codebehind="Login.aspx.cs" %>

<%@ Register Src="../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
    <div style="margin: 0 20px;">
        <div id="divGrupos" runat="server" align="center" visible="false" class="area-selecao-grupo">
            <fieldset>
                <legend>Seleção de grupo</legend>
                <asp:UpdatePanel ID="_updGrupos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="_updGrupos" />
                        <asp:Repeater ID="rptGrupos" runat="server" OnItemCommand="_rptGrupos_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbGrupo" runat="server" Text='<%# Bind("gru_nome") %>' CommandArgument='<%# Bind("gru_id") %>'
                                    CommandName="Select" CausesValidation="false" />
                                <br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>
        <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CausesValidation="false"
            Visible="false" OnClick="btnVoltar_Click" />
    </div>
</asp:Content>
