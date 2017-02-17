<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.MatrizHabilidades.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Academico/MatrizHabilidades/Busca.aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsMatriz" runat="server" ValidationGroup="MatrizHabilidades" />
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
            <asp:Panel ID="pnlCadastro" runat="server" GroupingText="<%$ Resources:Academico, MatrizHabilidades.Cadastro.pnlCadastro.GroupingText %>">
                <asp:Label ID="lblNome" runat="server" Text="<%$ Resources:Academico, MatrizHabilidades.Cadastro.lblNome.Text %>" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" SkinID="text60C"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNome"
                    Display="Dynamic" ErrorMessage="<%$Resources:Academico, MatrizHabilidades.Cadastro.rfvNome.ErrorMessage %>" ValidationGroup="MatrizHabilidades">*</asp:RequiredFieldValidator>
                <asp:CheckBox ID="ckbPadrao" Text="<%$Resources:Academico, MatrizHabilidades.Cadastro.ckbPadrao.Text %>" runat="server" />
                <div class="right">
                    <asp:Button ID="btnSalvar" OnClick="btnSalvar_Click" runat="server" Text="<%$Resources:Academico, MatrizHabilidades.Cadastro.btnSalvar.Text %>" ValidationGroup="MatrizHabilidades" />
                    <asp:Button ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" Text="<%$Resources:Academico, MatrizHabilidades.Cadastro.btnCancelar.Text %>" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
