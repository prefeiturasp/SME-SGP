<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_Turma_AlunosTurma" CodeBehind="AlunosTurma.aspx.cs" %>

<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ PreviousPageType VirtualPath="~/Turma/Turma/Busca.aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="_uppMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="906px" />
    <fieldset>
        <legend>Dados da turma</legend>
        <asp:Label ID="_lblTurma" runat="server"></asp:Label>
        <asp:Label ID="_lblEscola" runat="server"></asp:Label>
        <asp:Label ID="_lblCalendario" runat="server"></asp:Label>
        <asp:Label ID="_lblCurso" runat="server"></asp:Label>
        <asp:Label ID="_lblTurno" runat="server"></asp:Label>
        <asp:Label ID="_lblSituacao" runat="server"></asp:Label>
        <asp:Label ID="_lblCapacidade" runat="server"></asp:Label>
        <asp:Label ID="_lblQtdMatriculados" runat="server"></asp:Label>
    </fieldset>
    <asp:UpdatePanel ID="updAlunosTurma" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <fieldset>
                <legend>Alunos por turma</legend>
                <div id="divLegenda" runat="server" style="width: 230px;" visible="false">
                    <b>Legenda:</b>
                    <div style="border-style: solid; border-width: thin;">
                        <table id="tbLegenda" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                            <tr runat="server" id="lnInativos">
                                <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;">
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="litInativo" Text="<%$ Resources:Mensagens, MSG_ALUNO_INATIVO %>"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <br />
                <asp:GridView ID="_dgvAlunos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    EmptyDataText="A pesquisa não encontrou resultados." DataKeyNames="alu_id,mtu_id"
                    OnRowDataBound="_dgvAlunos_RowDataBound" OnDataBound="_dgvAlunos_DataBound">
                    <Columns>
                        <asp:BoundField DataField="numeroChamada" HeaderText="Novo número de chamada">
                            <ControlStyle Width="130px" />
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" Width="135px" />
                            <ItemStyle HorizontalAlign="Center" Width="135px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="numeroChamadaReal" HeaderText="Número de chamada" SortExpression="mtu_numeroChamada">
                            <ControlStyle Width="130px" />
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" Width="135px" />
                            <ItemStyle HorizontalAlign="Center" Width="135px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="alunoNome" HeaderText="Nome do aluno" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" SortExpression="alunoNome">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="numeroMatricula" HeaderText="<%$ Resources:Mensagens, MSG_NUMEROMATRICULA %>" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" SortExpression="numeroMatricula">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Situação do aluno" SortExpression="mtuSituacao">
                            <ItemTemplate>
                                <asp:Label ID="lblSituacao" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblMovimentacaoSaida" runat="server" Visible="false" Text='<%# Bind("MovimentacaoSaida") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="mtuSituacao" HeaderText="mtuSituacao" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" SortExpression="mtuSituacao">
                            <HeaderStyle CssClass="hide" />
                            <ItemStyle CssClass="hide" />
                        </asp:BoundField>
                        <asp:BoundField DataField="mtu_dataMatricula" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data matrícula"
                            SortExpression="mtu_dataMatricula" />
                        <asp:BoundField DataField="mtu_dataSaida" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data saída"
                            SortExpression="mtu_dataSaida" />
                        <asp:BoundField DataField="mtu_numeroChamada" HeaderText="número de chamada ordenada" Visible="false">
                            <ControlStyle Width="130px" />
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" Width="135px" />
                            <ItemStyle HorizontalAlign="Center" Width="135px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsALunosTurma" runat="server" TypeName="MSTech.GestaoEscolar.BLL.MTR_MatriculaTurmaBO"
                    SelectMethod="MatriculaTurmaAlunosNumeroChamada"></asp:ObjectDataSource>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updBotoes" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <fieldset>
                <div align="right" class="divBtnCadastro">
                    <asp:Button ID="_btnCancelar" runat="server" Text="Voltar" OnClick="_btnCancelar_Click"
                        CausesValidation="false" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
