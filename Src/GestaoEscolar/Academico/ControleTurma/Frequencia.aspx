<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Frequencia.aspx.cs" Inherits="GestaoEscolar.Academico.ControleTurma.Frequencia" %>

<%@ PreviousPageType VirtualPath="~/Academico/ControleTurma/Busca.aspx" %>

<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/ControleTurma/UCControleTurma.ascx" TagName="UCControleTurma" TagPrefix="uc10" %>
<%@ Register Src="~/WebControls/NavegacaoTelaPeriodo/UCNavegacaoTelaPeriodo.ascx" TagName="UCNavegacaoTelaPeriodo" TagPrefix="uc13" %>
<%@ Register Src="~/WebControls/LancamentoFrequencia/UCLancamentoFrequencia.ascx" TagName="UCLancamentoFrequencia" TagPrefix="uc1" %>
<%@ Register src="~/WebControls/ControleTurma/UCSelecaoDisciplinaCompartilhada.ascx" tagname="UCSelecaoDisciplinaCompartilhada" tagprefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var idbtnCompensacaoAusencia = '#<%=btnCompensacaoAusencia.ClientID%>';
        var idhdbtnCompensacaoAusenciaVisible = '#<%=hdbtnCompensacaoAusenciaVisible.ClientID%>';
        var idDdlOrdenacaoFrequencia = '#<%=UCLancamentoFrequencia.ClientIdComboOrdenacao%>';
        var idDdlOrdenacaoAvaliacao = "";
        var idhdnOrdenacaoFrequencia = '#<%=UCLancamentoFrequencia.ClientIdHdnOrdenacao%>';
        var idhdnOrdenacaoAvaliacao = "";
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <asp:Label ID="lblPeriodoEfetivado" runat="server" EnableViewState="false" Visible="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <uc10:UCControleTurma ID="UCControleTurma1" runat="server" />
        <asp:HiddenField ID="hdnOrdenacaoAvaliacao" runat="server" />

        <div runat="server" id="divMessageTurmaAnterior"
            class="summaryMsgAnosAnteriores" style="<%$ Resources: Academico, ControleTurma.Busca.divMessageTurmaAnterior.Style %>">
            <asp:Label runat="server" ID="lblMessageTurmaAnterior" Text="<%$ Resources:Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Text %>"
                Style="<%$ Resources: Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Style %>"></asp:Label>
        </div>

        <!-- Botões de navegação -->
        <uc13:UCNavegacaoTelaPeriodo ID="UCNavegacaoTelaPeriodo" runat="server" />

        <div style="margin-top: 10px;">
            <asp:Panel ID="pnlListao" runat="server">
                <asp:UpdatePanel ID="updListao" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hdnListaoSelecionado" runat="server" />
                        <asp:Label ID="lblMessage2" runat="server" EnableViewState="false"></asp:Label>
                        <div id="divListao" runat="server">
                            <div class="right" style="border-bottom-right-radius: 0px; border-bottom-left-radius: 0px;">
                                <asp:Button ID="btnSalvarCima" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                                    ValidationGroup="Frequencia" OnClientClick="scrollToTop();" />
                            </div>
                            <br />
                            <asp:Panel ID="pnlListaoLancamentoFrequencias" runat="server" Visible="false">
                                <uc1:UCLancamentoFrequencia ID="UCLancamentoFrequencia" runat="server"></uc1:UCLancamentoFrequencia>
                            </asp:Panel>
                            <div class="right divBtnCadastro">
                                <asp:HiddenField runat="server" ID="hdbtnCompensacaoAusenciaVisible" Value="True" />
                                <asp:Button ID="btnCompensacaoAusencia" runat="server" Text="Incluir nova compensação de ausência"
                                    OnClick="btnCompensacaoAusencia_Click" />
                                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                                    ValidationGroup="Frequencia" OnClientClick="scrollToTop();" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </fieldset>
    <input id="txtSelectedTab" type="hidden" runat="server" />

    <div id="divCompensacao" title="Compensação de ausência" class="hide">
        <asp:UpdatePanel ID="upnCompensacao" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset id="fdsResultados" runat="server" visible="false">
                    <uc9:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                    <asp:GridView ID="gvCompAusencia" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText='<%$ Resources: Classe, MinhasTurmas.Frequencia.Compensacao.SemCompensacaoAusencia %>' OnDataBound="gvCompAusencia_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Atividades desenvolvidas" SortExpression="atividadesDesenv">
                                <ItemTemplate>
                                    <asp:Label ID="lblAtividadesDesenv" runat="server" Text='<%# Bind("cpa_atividadesDesenvolvidas") %>'
                                        ToolTip='<%# Bind("cpa_atividadesDesenvolvidas") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="cpa_quantidadeAulasCompensadas" HeaderText="Qtde. aulas compensadas">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="tpc_nome" HeaderText="">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsCompAusencia" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="SelecionaPorAluno" TypeName="MSTech.GestaoEscolar.BLL.CLS_CompensacaoAusenciaBO"></asp:ObjectDataSource>
                    <uc8:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="gvCompAusencia" />
                </fieldset>
                <div class="right">
                    <asp:Button ID="btnFecharConsultaCompensacao" runat="server" Text="Voltar" OnClientClick="$('#divCompensacao').dialog('close'); return false;" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%-- Disciplinas compartilhadas --%>
    <uc10:UCSelecaoDisciplinaCompartilhada ID="UCSelecaoDisciplinaCompartilhada1" runat="server"></uc10:UCSelecaoDisciplinaCompartilhada>
    <asp:HiddenField ID="hdnValorTurmas" runat="server" />

</asp:Content>
