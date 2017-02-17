<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_HistoricoObservacaoPadrao_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/HistoricoObservacaoPadrao/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Observacao" />
    <fieldset>
        <legend>Cadastro de observações do histórico escolar</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="_lblTipoObservacao" runat="server" Text="Tipo de observação *" AssociatedControlID="_ddlTipoObservacao"></asp:Label>
        <asp:DropDownList ID="_ddlTipoObservacao" runat="server" OnSelectedIndexChanged="_ddlTipoObservacao_SelectedIndexChanged" AutoPostBack="true">
        </asp:DropDownList>
        <asp:CompareValidator ID="_cpvTipoObservacao" runat="server" ErrorMessage="Tipo de observação é obrigatório."
            ControlToValidate="_ddlTipoObservacao" Operator="GreaterThan" ValueToCompare="0"
            Display="Dynamic" ValidationGroup="Observacao">*</asp:CompareValidator>
        <asp:Label ID="_lblNomeObservacao" runat="server" Text="Nome da observação *" AssociatedControlID="_txtNomeObservacao"></asp:Label>
        <asp:TextBox ID="_txtNomeObservacao" runat="server" MaxLength="100" SkinID="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvNomeObservacao" runat="server" ControlToValidate="_txtNomeObservacao"
            Display="Dynamic" ErrorMessage="Nome da observação é obrigatório." ValidationGroup="Observacao">*</asp:RequiredFieldValidator>
        <asp:Label ID="_lblDescricaoObservacao" runat="server" Text="Descrição da observação *"
            AssociatedControlID="_txtDescricaoObservacao"></asp:Label>
        <asp:TextBox ID="_txtDescricaoObservacao" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvDescricao" runat="server" ControlToValidate="_txtDescricaoObservacao"
            Display="Dynamic" ErrorMessage="Descrição da observação é obrigatório." ValidationGroup="Observacao">*</asp:RequiredFieldValidator>

        <div id="divEditorHTML" runat="server">
            <div id="divCamposAuxiliares" runat="server" visible="false">
                <div style="display: inline-block">
                    <asp:DropDownList ID="ddlCampoAuxiliar" runat="server"></asp:DropDownList>
                </div>
                <div style="display: inline-block">
                    <asp:Button ID="btnCamposAuxiliares" runat="server" Text="\/"
                        ValidationGroupTipo="Avisos" />
                </div>
            </div>
            <CKEditor:CKEditorControl ID="txtDescricaoObservacaoHTML" BasePath="/includes/ckeditor/" runat="server"></CKEditor:CKEditorControl>
        </div>

        <div align="right">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click"
                ValidationGroup="Observacao" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
