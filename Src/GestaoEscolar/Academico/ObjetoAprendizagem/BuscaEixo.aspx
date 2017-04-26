<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="BuscaEixo.aspx.cs" Inherits="GestaoEscolar.Academico.ObjetoAprendizagem.BuscaEixo" %>

<%@ PreviousPageType VirtualPath="~/Academico/ObjetoAprendizagem/BuscaDisciplina.aspx" %>

<%@ Register src="~/WebControls/Combos/UCComboAnoLetivo.ascx" tagname="UCComboAnoLetivo" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="_updDadosBasicos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fds" runat="server">
                <legend>Consulta de eixos de objetos de conhecimento</legend>
                <div id="_divPesquisa" runat="server">
                    <asp:Label ID="_lblDisciplina" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="txtDisciplina"></asp:Label>
                    <asp:TextBox ID="txtDisciplina" runat="server" Enabled="false"></asp:TextBox>
                    <uc2:UCComboAnoLetivo ID="UCComboAnoLetivo1" runat="server" Obrigatorio="true" />
                </div>
                <div class="right">
                    <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo eixo de objeto de conhecimento" OnClick="_btnNovo_Click"
                        CausesValidation="false" />
                    <asp:Button ID="_btnCancelar" runat="server" Text="Voltar" OnClick="_btnCancelar_Click"
                        CausesValidation="false" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <asp:UpdatePanel ID="upd" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView ID="_grvEixoObjetoAprendizagem" runat="server" AutoGenerateColumns="False"
                    AllowPaging="False" DataKeyNames="oae_id" HorizontalAlign="Center"
                    OnRowCommand="_grvEixoObjetoAprendizagem_RowCommand" OnRowDataBound="_grvEixoObjetoAprendizagem_RowDataBound"
                    OnDataBound="_grvEixoObjetoAprendizagem_DataBound" AllowSorting="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Descrição">
                            <ItemTemplate>
                                <asp:LinkButton ID="_btnSelecionar" runat="server" CommandName="Alterar" Text='<%# Bind("oae_descricao") %>'
                                    CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="oae_situacaoText" HeaderText="Situação" />
                        <asp:TemplateField HeaderText="Ordem">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("oae_ordem") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                    Height="16" Width="16" />
                                <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                    Height="16" Width="16" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar"
                                    CausesValidation="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <div id="divInserir" class="hide">
        <asp:UpdatePanel runat="server" ID="updMessagePopUp" UpdateMode="Always">
            <ContentTemplate>
                <asp:Label ID="lblMessagePopUp" runat="server" EnableViewState="False"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server" ID="updPopUp" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:ValidationSummary ID="vsPopUp" runat="server" ValidationGroup="vgPopUp" />
                <fieldset>
                    <legend>Novo eixo de objeto de conhecimento</legend>
                    <asp:Label runat="server" ID="lblDescricao" Text="Descrição *" AssociatedControlID="txtDescricao" />
                    <asp:TextBox runat="server" ID="txtDescricao" SkinID="text60C" MaxLength="500" />
                    <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ControlToValidate="txtDescricao" ValidationGroup="vgPopUp"
                        Display="Dynamic" ErrorMessage="Descrição do eixo de objeto de conhecimento é obrigatória." Text="*" />
                    <div class="right">
                        <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar" ValidationGroup="vgPopUp" 
                            OnClick="btnAdicionar_Click" />
                        <asp:Button ID="btnCancelarItem" runat="server" Text="Cancelar" CausesValidation="false"
                            OnClientClick="$('#divInserir').dialog('close');" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
