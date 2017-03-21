<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="BuscaDisciplina.aspx.cs" Inherits="GestaoEscolar.Academico.ObjetoAprendizagem.BuscaDisciplina" %>

<%@ Register Src="../../WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsEscola" runat="server">
        <legend><asp:Literal ID="ltrLegend" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>"></asp:Literal></legend>
        <div id="_divPesquisa" runat="server">
            <uc2:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino1" runat="server" />
            <asp:Label ID="_lblBase" runat="server" Text="Base" AssociatedControlID="_ddlBase"></asp:Label>
            <asp:DropDownList ID="_ddlBase" runat="server">
                <asp:ListItem Text="-- Selecione uma base --" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Nacional " Value="1"></asp:ListItem>
                <asp:ListItem Text="Diversificada" Value="2"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click"
                CausesValidation="False" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click"
                CausesValidation="False" />
        </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView ID="_grvTipoDisciplina" runat="server" AutoGenerateColumns="False" OnDataBound="_grvTipoDisciplina_DataBound"
                    DataKeyNames="tds_id" EmptyDataText="<%$ Resources:Configuracao, TipoDisciplina.Busca._dgvTipoDisciplina.EmptyDataText %>"
                    OnRowEditing="_grvTipoDisciplina_RowEditing" >
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_DISCIPLINA %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="_btnSelecionar" runat="server" CommandName="Edit" Text='<%# Bind("tds_nome") %>'
                                    PostBackUrl="~/Academico/ObjetoAprendizagem/Busca.aspx"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="tne_nome" HeaderText="Tipo de nível de ensino" />
                        <asp:BoundField DataField="tds_base_nome" HeaderText="Base" />
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:GridView>
                <uc4:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvTipoDisciplina" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
