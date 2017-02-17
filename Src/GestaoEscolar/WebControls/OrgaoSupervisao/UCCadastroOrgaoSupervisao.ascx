<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_OrgaoSupervisao_UCCadastroOrgaoSupervisao" Codebehind="UCCadastroOrgaoSupervisao.ascx.cs" %>
<%@ Register Src="../Combos/UCComboEntidade.ascx" TagName="UCComboEntidade" TagPrefix="uc1" %>
<%@ Register Src="../Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc2" %>
<%@ Register Src="../Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:UpdatePanel ID="updCadastroOrgaoSupervisao" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="updCadastroOrgaoSupervisao" />
        <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="OrgaoSupervisao" />
        <fieldset>
            <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />
            <asp:Label ID="LabelDescricao" runat="server" Text="Descrição *" AssociatedControlID="txtDescricao"></asp:Label>
            <asp:TextBox ID="txtDescricao" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvDescricao" ControlToValidate="txtDescricao" ValidationGroup="OrgaoSupervisao"
                runat="server" ErrorMessage="Descrição é obrigatório.">*</asp:RequiredFieldValidator>
            <uc1:UCComboEntidade ID="UCComboEntidade1" runat="server" />
            <asp:Label ID="LabelUA" runat="server" Text="Unidade administrativa" AssociatedControlID="txtUA"></asp:Label>
            <asp:TextBox ID="txtUA" runat="server" MaxLength="200" SkinID="text60C" Enabled="False"></asp:TextBox>
            <asp:ImageButton ID="_btnPesquisarUA" runat="server" CausesValidation="False" SkinID="btPesquisar"
                OnClick="_btnPesquisarUA_Click" />
            <div class="right">
                <asp:Button ID="_btnIncluir" runat="server" Text="Incluir" OnClick="_btnIncluir_Click"
                    ValidationGroup="OrgaoSupervisao" />
                <asp:Button ID="_btnCancelar" runat="server" CausesValidation="False" Text="Cancelar"
                    OnClientClick="$('#divOrgaoSupervisao').dialog('close'); return false;" />
            </div>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
