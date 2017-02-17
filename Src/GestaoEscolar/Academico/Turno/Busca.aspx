<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Academico_Turno_Busca" Codebehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboTipoTurno.ascx" TagName="_UCComboTipoTurno"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsTurno" runat="server">
        <legend>Consulta de turnos</legend>
        <div id="_divPesquisa" runat="server">
            <uc1:_UCComboTipoTurno ID="_UCComboTipoTurno" runat="server" />
            <asp:Label ID="_lblDescricao" runat="server" Text="Descrição do turno" AssociatedControlID="_txtDescricao"></asp:Label>
            <asp:TextBox ID="_txtDescricao" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
        </div>
    </fieldset>
    <fieldset id="fdsResultado" runat="server">
        <legend>Resultados</legend>
        <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_dgvTurno" runat="server" AutoGenerateColumns="False" DataKeyNames="trn_id"
            DataSourceID="_odsTurno" AllowPaging="True" EmptyDataText="A pesquisa não encontrou resultados."
            ondatabound="_dgvTurno_DataBound" AllowSorting="true">
            <Columns>
                <asp:TemplateField HeaderText="Descrição do turno" SortExpression="trn_descricao">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" PostBackUrl="~/Academico/Turno/Cadastro.aspx"
                            Text='<%# Bind("trn_descricao") %>' CssClass="wrap600px"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ttn_nome" HeaderText="Tipo de turno" SortExpression="ttn_nome">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="trn_controleTempo" HeaderText="Controle de horas/aulas" SortExpression="trn_controleTempo">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="trn_situacao" HeaderText="Bloqueado" SortExpression="trn_situacao">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <uc2:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvTurno" />
    </fieldset>
    <asp:ObjectDataSource ID="_odsTurno" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Turno"
        DeleteMethod="Delete" SelectMethod="SelecionaTurno"
        TypeName="MSTech.GestaoEscolar.BLL.ACA_TurnoBO" >
    </asp:ObjectDataSource>
</asp:Content>
