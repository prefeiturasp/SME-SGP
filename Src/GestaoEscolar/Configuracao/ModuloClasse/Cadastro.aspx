<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.ModuloClasse.Cadastro" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional" EnableViewState="False">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Config" />
    <fieldset>
        <legend>Configuração de ícones da Área do aluno</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:UpdatePanel ID="updConfig" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="updConfig" />
                <asp:GridView ID="grvConfig" runat="server" AutoGenerateColumns="False" DataKeyNames="mod_id, mdc_id, mdc_classe, mdc_situacao"
                    EmptyDataText="Não existem configurações cadastradas." OnRowEditing="grvConfig_RowEditing"
                    OnDataBinding="grvConfig_DataBinding" OnRowUpdating="grvConfig_RowUpdating"
                    OnRowDataBound="grvConfig_RowDataBound" OnRowCancelingEdit="grvConfig_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl" runat="server" Text="Módulo" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblChave" runat="server" Text='<%# Bind("mod_nome") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblChaveEdit" runat="server" Text='<%# Bind("mod_nome") %>'></asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="320px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl2" runat="server" Text="Classe ícone *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblClasse" runat="server" Text='<%# Bind("mdc_classe") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlClasseIcone" AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddlClasseIcone_SelectedIndexChanged">
                                    <asp:ListItem Text="Boletim online" Value="boletim"></asp:ListItem>
                                    <asp:ListItem Text="Alteração de cadastro" Value="cadastro"></asp:ListItem>
                                    <%--<asp:ListItem Text"Ta na rede" Value="taNaRede"></asp:ListItem>--%>
                                    <asp:ListItem Text="Sair" Value="sair"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ícone">
                            <ItemTemplate>
                                <div runat="server" id="divIcone" class=""/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div runat="server" id="divIconeEdit" class=""/>
                            </EditItemTemplate>
                            <HeaderStyle Width="320px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                    ToolTip="Editar" CausesValidation="false" />
                                <asp:ImageButton ID="imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                    ToolTip="Cancelar edição" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                    ToolTip="Salvar" ValidationGroup="Config" Visible="false" />
                                <asp:ImageButton ID="imgCancelarConfg" runat="server" CommandName="Cancel" SkinID="btCancelar"
                                    ToolTip="Cancelar" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>

