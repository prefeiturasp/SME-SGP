<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs"
    Inherits="GestaoEscolar.Configuracao.DiarioClasse.Protocolos.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UCLoader ID="UCLoader1" runat="server" />
    <asp:Label ID="lblMensagemErro" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Group1" />
    <fieldset>
        <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Configuracao, DiarioClasse.Protocolos.Busca.lblLegend.Text %>"></asp:Label></legend>
        <uc7:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" EnableViewState="False" />
        <div>

            <asp:Label ID="lblDataInicio" runat="server" Text="Período de envio do protocolo *"
                AssociatedControlID="txtDataInicio"></asp:Label>

            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="maskData" SkinID="Data" Style="display: inline;"></asp:TextBox>

            <asp:Label ID="lblDataFim" runat="server" Text="à" AssociatedControlID="txtDataFim" Style="display: inline;"></asp:Label>
            <asp:TextBox ID="txtDataFim" runat="server" CssClass="maskData" SkinID="Data" Style="display: inline;"></asp:TextBox>

            <asp:Label ID="lblStatus" runat="server" Text="Status do protocolo" AssociatedControlID="ddlStatus"></asp:Label>
            <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem Selected="True" Text="-- Selecione um status --" Value="-1" />
                <asp:ListItem Text="Não processado" Value="1" />
                <asp:ListItem Text="Processando" Value="2" />
                <asp:ListItem Text="Processado com sucesso" Value="3" />
                <asp:ListItem Text="Processado com erro" Value="4" />
                <asp:ListItem Text="Processado com inconsistência" Value="5" />
            </asp:DropDownList>
        </div>

        <div>
            <asp:Label ID="lblTipoProtocolo" runat="server" Text="Tipo do protocolo" AssociatedControlID="ddlTipoProtocolo"></asp:Label>
            <asp:DropDownList ID="ddlTipoProtocolo" runat="server">
                <asp:ListItem Selected="True" Text="-- Selecione um tipo --" Value="-1" />
                <asp:ListItem Text="Protocolos de aula" Value="1" />
                <%--
                  *** Não sendo utilizada no momento - Protocolo de justificativa ***
                  Foi retirado do Diario de Classes (Aplicativo Android), mas pedido para deixar as rotinas comentadas   
                <asp:ListItem Text="Protocolos de justificativa" Value="2" />
                --%>
                <asp:ListItem Text="Protocolos de planejamento" Value="3" />
                <asp:ListItem Text="Protocolos de log" Value="4" />
                <asp:ListItem Text="Protocolos de foto" Value="5" />
                <asp:ListItem Text="Protocolos de compensação de aula" Value="6" />
            </asp:DropDownList>
        </div>

        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar"
                OnClick="btnPesquisar_Click" />
        </div>
    </fieldset>

    <fieldset id="fsResultados" runat="server" visible="false">
        <legend>Resultados</legend>
        <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />

        <asp:GridView ID="grvProtocolos" runat="server" DataKeyNames="pro_id, pro_protocolo, pro_status, pro_tipo, pro_status_id" AutoGenerateColumns="False"
            DataSourceID="odsProtocolos" AllowPaging="True" BorderStyle="None"
            EmptyDataText="A pesquisa não encontrou resultados." OnRowDataBound="grvProtocolos_RowDataBound"
            AllowSorting="true" OnRowCommand="grvProtocolos_RowCommand" OnDataBound="grvProtocolos_DataBound">

            <Columns>
                
                <%--
                <asp:TemplateField HeaderText="Status" SortExpression="pro_status">
                    <ItemTemplate>
                        <asp:Label ID="_lblStatus" runat="server" Text='<%# Bind("pro_status") %>' CssClass="wrap100px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                --%>

                <asp:BoundField DataField="pro_status" HeaderText="Status" SortExpression="pro_status"/> 
                <asp:BoundField DataField="pro_dataCriacao" HeaderText="Data do pacote" SortExpression="pro_dataCriacao" />
                <asp:BoundField DataField="pro_protocolo" HeaderText="Número do protocolo" SortExpression="pro_protocolo" />
                <asp:BoundField DataField="pro_statusObservacao" HeaderText="Descrição do protocolo" SortExpression="pro_statusObservacao" />
                <asp:BoundField DataField="pro_tipoDescricao" HeaderText="Tipo do protocolo" SortExpression="pro_tipoDescricao"/>

                <asp:TemplateField HeaderText="Detalhes do protocolo">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDetalhes" SkinID="btDetalhar" runat="server" CommandName="btnDetalhes_Select"
                            ToolTip="Exibir detalhes do protocolo" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

        <uc2:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvProtocolos" />

        <asp:ObjectDataSource ID="odsProtocolos" runat="server"
            SelectMethod="SelectBy_Entidade_Data"
            TypeName="MSTech.GestaoEscolar.BLL.DCL_ProtocoloBO" OnSelecting="odsProtocolos_Selecting"></asp:ObjectDataSource>
    </fieldset>

    <!-- POPUP - Detalhes do tipo do protocolo Aula -->
    <div id="divDetalhesProtAula" title="Detalhes do protocolo" class="hide">
        <br />

        <asp:UpdatePanel ID="updDetalhesProtAula" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <fieldset id="fsDetalhesProtAula" runat="server">

                    <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao2" runat="server" OnIndexChanged="UCComboQtdePaginacao_ProtAula_IndexChanged" />

                    <asp:GridView ID="grvDetalhesProtAula" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsDetalhesProtAula" AllowPaging="True" BorderStyle="None"
                        EmptyDataText="Não foi encontrado dados para serem exibidos." AllowSorting="true">

                        <Columns>
                            <asp:BoundField DataField="tau_data" HeaderText="Data da aula" ItemStyle-Width="90px" HeaderStyle-Width="90px" />
                            <asp:BoundField DataField="esc_nome" HeaderText="Escola" />
                            <asp:BoundField DataField="tur_codigo" HeaderText="Turma" />
                            <asp:BoundField DataField="tud_nome" HeaderText="Disciplina" />
                            <asp:BoundField DataField="pes_nome" HeaderText="Professor" />
                        </Columns>

                    </asp:GridView>

                    <uc2:UCTotalRegistros ID="UCTotalRegistros2" runat="server" AssociatedGridViewID="grvDetalhesProtAula" />

                    <asp:ObjectDataSource ID="odsDetalhesProtAula" runat="server"
                        SelectMethod="SelectBy_Protocolo_TurmaAula"
                        TypeName="MSTech.GestaoEscolar.BLL.DCL_ProtocoloBO" OnSelecting="odsDetalhesProtAula_Selecting"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:Parameter DbType="Guid" Name="pro_id" />
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    <div class="right">
                        <asp:Button ID="btnFecharDetalhesProtAula" runat="server" Text="Fechar"
                            OnClick="btnFecharDetalhesProtAula_Click" />
                    </div>

                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- POPUP - Detalhes do tipo do protocolo de Justificativa -->
    <div id="divDetalhesProtJustificativa" title="Detalhes do protocolo" class="hide">
        <br />

        <%--
             *** Não sendo utilizada no momento - Protocolo de Justificativa ***
             Foi retirado do Diario de Classes (Aplicativo Android), mas pedido para deixar as rotinas comentadas   
        --%>

        <asp:UpdatePanel ID="updDetalhesProtJustificativa" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <fieldset id="fsDetalhesProtJustificativa" runat="server">

                    <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao3" runat="server" OnIndexChanged="UCComboQtdePaginacao_ProtJustificativa_IndexChanged" />

                    <asp:GridView ID="grvDetalhesProtJustificativa" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsDetalhesProtJustificativa" AllowPaging="True" BorderStyle="None"
                        EmptyDataText="Não foi encontrado dados para serem exibidos." AllowSorting="true">

                        <Columns>
                            <asp:BoundField DataField="pes_nome" HeaderText="Nome do aluno" />
                            <asp:BoundField DataField="esc_nome" HeaderText="Escola" />
                            <asp:BoundField DataField="tur_codigo" HeaderText="Turma" />
                            <asp:BoundField DataField="afj_dataInicio" HeaderText="Período inicial" />
                            <asp:BoundField DataField="afj_dataFim" HeaderText="Período final" />
                        </Columns>
                    </asp:GridView>

                    <uc2:UCTotalRegistros ID="UCTotalRegistros4" runat="server" AssociatedGridViewID="grvDetalhesProtJustificativa" />

                    <asp:ObjectDataSource ID="odsDetalhesProtJustificativa" runat="server"
                        SelectMethod="SelectBy_Protocolo_AlunoJustificativaFalta"
                        TypeName="MSTech.GestaoEscolar.BLL.DCL_ProtocoloBO" OnSelecting="odsDetalhesProtJustificativa_Selecting"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:Parameter DbType="Guid" Name="pro_id" />
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    <div class="right">
                        <asp:Button ID="btnFecharDetalhesProtJustificativa" runat="server" Text="Fechar"
                            OnClick="btnFecharDetalhesProtJustificativa_Click" />
                    </div>
                </fieldset>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- POPUP - Detalhes do tipo do protocolo de Planejamento -->
    <div id="divDetalhesProtPlanejamento" title="Detalhes do protocolo" class="hide">
        <br />

        <asp:UpdatePanel ID="updDetalhesProtPlanejamento" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <fieldset id="fsDetalhesProtPlanejamento" runat="server">

                    <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao4" runat="server" OnIndexChanged="UCComboQtdePaginacao_ProtPlanejamento_IndexChanged" />

                    <asp:GridView ID="grvDetalhesProtPlanejamento" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsDetalhesProtPlanejamento" AllowPaging="True" BorderStyle="None"
                        EmptyDataText="Não foi encontrado dados para serem exibidos." AllowSorting="true">

                        <Columns>
                            <asp:BoundField DataField="esc_nome" HeaderText="Escola" />
                            <asp:BoundField DataField="tur_codigo" HeaderText="Turma" />
                            <asp:BoundField DataField="tud_nome" HeaderText="Disciplina" />
                        </Columns>
                    </asp:GridView>

                    <uc2:UCTotalRegistros ID="UCTotalRegistros3" runat="server" AssociatedGridViewID="grvDetalhesProtPlanejamento" />

                    <asp:ObjectDataSource ID="odsDetalhesProtPlanejamento" runat="server"
                        SelectMethod="SelectBy_Protocolo_TurmaDisciplinaPlanejamento"
                        TypeName="MSTech.GestaoEscolar.BLL.DCL_ProtocoloBO" OnSelecting="odsDetalhesProtPlanejamento_Selecting"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:Parameter DbType="Guid" Name="pro_id" />
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    <div class="right">
                        <asp:Button ID="btnFecharDetalhesProtPlanejamento" runat="server" Text="Fechar"
                            OnClick="btnFecharDetalhesProtPlanejamento_Click" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

    <!-- POPUP - Detalhes do tipo do protocolo de Foto -->
    <div id="divDetalhesProtFoto" title="Detalhes do protocolo" class="hide">
        <br />
        <asp:UpdatePanel ID="updDetalhesProtFoto" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <fieldset id="fsDetalhesProtFoto" runat="server">

                    <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao5" runat="server" OnIndexChanged="UCComboQtdePaginacao_ProtFoto_IndexChanged" />

                    <asp:GridView ID="grvDetalhesProtFoto" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsDetalhesProtFoto" AllowPaging="True" BorderStyle="None"
                        EmptyDataText="Não foi encontrado dados para serem exibidos." AllowSorting="true">

                        <Columns>
                            <%--
                            <asp:BoundField DataField="pes_nome" HeaderText="Nome do aluno" />
                            <asp:BoundField DataField="alc_matricula" HeaderText="Matrícula" />
                            <asp:BoundField DataField="esc_nome" HeaderText="Escola" />
                            <asp:BoundField DataField="tur_codigo" HeaderText="Turma" />
                            --%>

                            <asp:BoundField DataField="uad_nome" HeaderText="Unidade administrativa" />
                            <asp:BoundField DataField="esc_nome" HeaderText="Escola" />
                            <asp:TemplateField HeaderText="Etapa de ensino">
                                <ItemTemplate>
                                    <asp:Label ID="lblNomeCurso" runat="server" Text='<%# Bind("nomeCurso") %>' CssClass="wrap150px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Grupamento de ensino">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrupEnsino" runat="server" Text='<%# Bind("crp_descricao") %>' CssClass="wrap150px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="tur_codigo" HeaderText="Turma" />

                        </Columns>
                    </asp:GridView>
                    <uc2:UCTotalRegistros ID="UCTotalRegistros5" runat="server" AssociatedGridViewID="grvDetalhesProtFoto" />

                    <asp:ObjectDataSource ID="odsDetalhesProtFoto" runat="server"
                        SelectMethod="SelectBy_Protocolo_ProtocoloAluno"
                        TypeName="MSTech.GestaoEscolar.BLL.DCL_ProtocoloBO" OnSelecting="odsDetalhesProtFoto_Selecting"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:Parameter DbType="Guid" Name="pro_id" />
                            <asp:Parameter DbType="Guid" Name="ent_id" />
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    <div class="right">
                        <asp:Button ID="btnFecharDetalhesProtFoto" runat="server" Text="Fechar"
                            OnClick="btnFecharDetalhesProtFoto_Click" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- POPUP - Detalhes do tipo do protocolo de compensação de ausencia de aula -->
    <div id="divDetalhesProtCompensacaoDeAula" title="Detalhes do protocolo" class="hide">
        <br />
        <asp:UpdatePanel ID="updDetalhesProtCompensacaoDeAula" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <fieldset id="fsDetalhesProtCompensacaoDeAula" runat="server">

                    <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao6" runat="server" OnIndexChanged="UCComboQtdePaginacao_ProtCompensacaoDeAula_IndexChanged" />

                    <asp:GridView ID="grvDetalhesProtCompensacaoDeAula" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsDetalhesProtCompensacaoDeAula" AllowPaging="True" BorderStyle="None"
                        EmptyDataText="Não foi encontrado dados para serem exibidos." AllowSorting="true">

                        <Columns>

                            <asp:BoundField DataField="pes_nome" HeaderText="Nome do professor" />
                            <asp:BoundField DataField="tud_nome" HeaderText="Disciplina" />
                            <asp:BoundField DataField="cpa_quantidadeAulasCompensadas" HeaderText="Aulas compensadas" />
                            <asp:BoundField DataField="cpa_atividadesDesenvolvidas" HeaderText="Atividades desenvolvidas" />

                        </Columns>

                    </asp:GridView>
                    <uc2:UCTotalRegistros ID="UCTotalRegistros6" runat="server" AssociatedGridViewID="grvDetalhesProtCompensacaoDeAula" />

                    <asp:ObjectDataSource ID="odsDetalhesProtCompensacaoDeAula" runat="server"
                        SelectMethod="SelectBy_Protocolo_CompensacaoDeAula"
                        TypeName="MSTech.GestaoEscolar.BLL.DCL_ProtocoloBO" OnSelecting="odsDetalhesProtCompensacaoDeAula_Selecting"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:Parameter DbType="Guid" Name="pro_id" />
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    <div class="right">
                        <asp:Button ID="btnFecharDetalhesProtCompensacaoDeAula" runat="server" Text="Fechar"
                            OnClick="btnFecharDetalhesProtCompensacaoDeAula_Click" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

</asp:Content>
