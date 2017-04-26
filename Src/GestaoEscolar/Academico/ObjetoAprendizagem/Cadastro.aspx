<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.ObjetoAprendizagem.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Academico/ObjetoAprendizagem/CadastroEixo.aspx" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="objetoAprendizagem" />
            <fieldset>
                <legend>Cadastro de objeto de conhecimento</legend>
                <uc5:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />

                <asp:Label ID="_lblDisciplina" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="txtDisciplina"></asp:Label>
                <asp:TextBox ID="txtDisciplina" runat="server" Enabled="false"></asp:TextBox>
                
                <asp:Label ID="lblAno" runat="server" Text="Ano" AssociatedControlID="txtAno"></asp:Label>
                <asp:TextBox ID="txtAno" runat="server" Enabled="false"></asp:TextBox>

                <asp:Label ID="_lblDescricao" runat="server" Text="Descrição *" AssociatedControlID="_txtDescricao"></asp:Label>
                <asp:TextBox ID="_txtDescricao" runat="server" Rows="4" TextMode="MultiLine" SkinID="text60C"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Descrição é obrigatório."
                    Display="Dynamic" ControlToValidate="_txtDescricao" ValidationGroup="objetoAprendizagem">*</asp:RequiredFieldValidator>

                <asp:CheckBox ID="_ckbBloqueado" runat="server" Text="Bloqueado" />
            </fieldset>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label ID="lblMessageCiclos" runat="server" EnableViewState="False"></asp:Label>
    <asp:UpdatePanel ID="updCiclos" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <fieldset>
                <legend>Ciclos <span class="asteriscoObrigatorio">*</span></legend>
                <div></div>
                <asp:Repeater ID="rptCampos" runat="server">
                    <HeaderTemplate>
                        <div class="checkboxlist-columns">
                    </HeaderTemplate>

                    <ItemTemplate>
                        <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("tci_id") %>' />
                        <asp:CheckBox ID="ckbCampo" runat="server" Text='<%# Eval("tci_nome") %>' />
                    </ItemTemplate>

                    <FooterTemplate>
                        </div> 
                    </FooterTemplate>
                </asp:Repeater>

            </fieldset>
            <asp:CustomValidator ID="cvCiclos" runat="server" ValidationGroup="objetoAprendizagem" Display="None" Text="*"
                OnServerValidate="cvCiclos_ServerValidate" ErrorMessage="É necessário selecionar pelo menos um ciclo." />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <div class="right">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click"
                ValidationGroup="objetoAprendizagem" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" OnClick="_btnCancelar_Click"
                CausesValidation="false" />
        </div>
    </fieldset>
</asp:Content>
