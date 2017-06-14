<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.CargaHorariaExtraclasse.Cadastro" %>

<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCCCurriculoPeriodo" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsSummary" runat="server" ValidationGroup="CargaHorariaExtraclasse" EnableViewState="false"></asp:ValidationSummary>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlFiltros" runat="server" GroupingText="Carga horária de atividades extraclasse">
        <uc:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <asp:UpdatePanel ID="updFiltros" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc:UCCCursoCurriculo ID="UCCCursoCurriculo" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" ValidationGroup="CargaHorariaExtraclasse" />
                <uc:UCCCalendario ID="UCCCalendario" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" ValidationGroup="CargaHorariaExtraclasse" />
                <uc:UCCCurriculoPeriodo ID="UCCCurriculoPeriodo" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" ValidationGroup="CargaHorariaExtraclasse" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlCadastro" runat="server" GroupingText="Disciplinas" Visible="false">
        <asp:UpdatePanel ID="updCadastro" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <asp:Repeater ID="rptDisciplinas" runat="server" OnItemDataBound="rptDisciplinas_ItemDataBound">
                    <HeaderTemplate>
                        <table id="tabela" class="grid grid-responsive-list" cellspacing="0">
                            <thead>
                                <tr class="gridHeader">
                                    <th rowspan="2">
                                        <asp:Label ID="lblDisciplina" runat="server" Text="Disciplina"></asp:Label>
                                    </th>
                                    <th id="thCargaHoraria" runat="server">
                                        <asp:Label ID="lblCargaHoraria" runat="server" Text="Carga horária de atividade extraclasse"></asp:Label>
                                    </th>
                                </tr>
                                <tr>
                                    <th></th>
                                    <asp:Repeater ID="rptPeriodoCalendario" runat="server">
                                        <ItemTemplate>
                                            <th>
                                                <asp:Label ID="lblPeriodoCalendario" runat="server" Text='<%#Bind("tpc_nome") %>'></asp:Label>
                                            </th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="gridRow">
                            <td>
                                <asp:Label ID="lblDisciplina" runat="server" Text='<%#Bind("dis_nome") %>'></asp:Label>
                            </td>
                            <asp:Repeater ID="rptPeriodoCalendario" runat="server">
                                <ItemTemplate>
                                    <td>
                                        <asp:HiddenField ID="hdnTpcId" runat="server" Value='<%#Bind("tpc_id") %>' />
                                        <asp:TextBox ID="txtCargaHoraria" runat="server" SkinID="Numerico"></asp:TextBox>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
