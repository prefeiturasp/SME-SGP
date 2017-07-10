<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Alunos.aspx.cs" Inherits="GestaoEscolar.Academico.ControleTurma.Alunos" %>

<%@ PreviousPageType VirtualPath="~/Academico/ControleTurma/Busca.aspx" %>

<%@ Register Src="~/WebControls/ControleTurma/UCControleTurma.ascx" TagName="UCControleTurma" TagPrefix="uc10" %>
<%@ Register Src="~/WebControls/NavegacaoTelaPeriodo/UCNavegacaoTelaPeriodo.ascx" TagName="UCNavegacaoTelaPeriodo" TagPrefix="uc13" %>
<%@ Register Src="~/WebControls/BoletimCompletoAluno/UCBoletimAngular.ascx" TagName="UCBoletim" TagPrefix="uc" %>
<%@ Register src="~/WebControls/Mensagens/UCTotalRegistros.ascx" tagname="UCTotalRegistros" tagprefix="uc1" %>
<%@ Register src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" tagname="UCComboQtdePaginacao" tagprefix="uc2" %>
<%@ Register src="~/WebControls/ControleTurma/UCSelecaoDisciplinaCompartilhada.ascx" tagname="UCSelecaoDisciplinaCompartilhada" tagprefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:UpdatePanel id="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <uc10:UCControleTurma ID="UCControleTurma1" runat="server" />
        <div runat="server" id="divMessageTurmaAnterior"
            class="summaryMsgAnosAnteriores" style="<%$ Resources: Academico, ControleTurma.Busca.divMessageTurmaAnterior.Style %>">
            <asp:Label runat="server" ID="lblMessageTurmaAnterior" Text="<%$ Resources:Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Text %>"
                Style="<%$ Resources: Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Style %>"></asp:Label>
        </div>

        <!-- Botões de navegação -->
        <uc13:UCNavegacaoTelaPeriodo ID="UCNavegacaoTelaPeriodo" runat="server" />

        <div style="margin-top: 10px;" class="lista-alunos">
            <asp:Panel ID="pnlAlunos" runat="server" GroupingText="Alunos">
                <asp:UpdatePanel ID="UpdAlunos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                        <div class="divScrollResponsivo">
                        <asp:GridView ID="grvAluno" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataSourceID="odsAluno" DataKeyNames="alu_id,mtu_id" EmptyDataText="Não foram encontrados alunos."
                            OnPageIndexChanging="grvAluno_PageIndexChanging"
                            OnRowCommand="grvAluno_RowCommand" OnRowDataBound="grvAluno_RowDataBound" OnDataBound="grvAluno_DataBound" SkinID="GridResponsive">
                            <Columns>
                                <asp:TemplateField HeaderText="Nome do aluno" SortExpression="pes_nome">
                                    <ItemTemplate>
                                        <asp:Image ID="imgAlertaRelatorio" runat="server" Visible="false" SkinID="imgAlertaRelatorioAtendimento" />
                                        <asp:Label ID="lblNomeAluno" runat="server" Text='<%# Bind("pes_nome") %>'></asp:Label>
                                        <div class="dropdown-relatorio">
                                            <asp:LinkButton ID="btnRelatorioRP" runat="server" CausesValidation="False" CommandName="RelatorioRP"
                                                ToolTip="<%$ Resources:Academico, ControleTurma.Alunos.btnRelatorioRP.ToolTip %>" SkinID="btRelatorioRP" Visible="false" />
                                            <asp:LinkButton ID="btnRelatorioAEE" runat="server" CausesValidation="False" CommandName="RelatorioAEE"
                                                ToolTip="<%$ Resources:Academico, ControleTurma.Alunos.btnRelatorioAEE.ToolTip %>" SkinID="btRelatorioAEE" Visible="false" />
                                            <!-- botao dropdown -->
                                            <button title="Seleção de relatório" class="btn-dropdown-relatorio"></button>
                                        </div>                                        
                                    </ItemTemplate>
                                    <ItemStyle CssClass="td-relatorio" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="pes_dataNascimento" HeaderText="Data de nascimento" DataFormatString="{0:dd/MM/yyy}"
                                    SortExpression="pes_dataNascimento">
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Nome da mãe" SortExpression="pes_nomeMae">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("nomeMae") %>' CssClass="wrap150px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="pes_nome_abreviado" HeaderText="<%$ Resources:Mensagens, MSG_NOMEABREVIADOALUNO %>"
                                    SortExpression="pes_nome_abreviado">
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="alu_dataCriacao" HeaderText="Data de cadastro" DataFormatString="{0:dd/MM/yyy}"
                                    SortExpression="alu_dataCriacao">
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="alu_dataAlteracao" HeaderText="Data da última atualização"
                                    DataFormatString="{0:dd/MM/yyy}" SortExpression="alu_dataAlteracao">
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Anotações">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="_btnDetalhes" runat="server" CausesValidation="False" SkinID="btDetalhar"
                                            CommandName="Anotacoes" ToolTip="Anotações de aula do aluno" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Boletim">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="_btnBoletim" runat="server" CausesValidation="False" CommandName="Boletim"
                                            SkinID="btRelatorio" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Relatório pedagógico">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="textBoxRelPedagogico" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnRelatorioPedagogico" runat="server" CausesValidation="False" CommandName="RelatorioPedagogico"
                                            ToolTip="Relatório pedagógico do aluno" SkinID="btFormulario" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inserir/Alterar foto" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnCapturarFoto" runat="server" CommandName="Editar" CausesValidation="False"
                                            SkinID="btWebCam" ImageAlign="Middle" />
                                        <asp:Image ID="_imgPossuiImagem" runat="server" SkinID="imgConfirmar" ToolTip="Aluno possui imagem cadastrada"
                                            ImageAlign="Middle" Visible="false" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        </div>
                        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvAluno" />
                        <asp:ObjectDataSource ID="odsAluno" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.MTR_MatriculaTurmaDisciplina"
                            SelectMethod="SelecionaAlunosAtivosCOCPorTurmaDisciplina" TypeName="MSTech.GestaoEscolar.BLL.MTR_MatriculaTurmaDisciplinaBO"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsAluno_Selecting"></asp:ObjectDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>

    </fieldset>

     <div id="divBoletimCompleto" title="Boletim completo do aluno" class="hide">
        <asp:UpdatePanel ID="updBoletimCompleto" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <uc:UCBoletim ID="UCBoletim" runat="server" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%-- Disciplinas compartilhadas --%>
    <uc10:UCSelecaoDisciplinaCompartilhada ID="UCSelecaoDisciplinaCompartilhada1" runat="server"></uc10:UCSelecaoDisciplinaCompartilhada>
    <asp:HiddenField ID="hdnValorTurmas" runat="server" />

</asp:Content>
