<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Academico_Aluno_CadastroAntigo" Codebehind="CadastroAntigo.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/Aluno/Busca.aspx" %>
<%@ Register Src="../../WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Pessoa/UCCadastroPessoa.ascx" TagName="UCCadastroPessoa"
    TagPrefix="uc3" %>
<%@ Register Src="../../WebControls/Contato/UCContato.ascx" TagName="UCContato" TagPrefix="uc42" %>
<%@ Register Src="../../WebControls/Documento/UCGridDocumento.ascx" TagName="UCGridDocumento"
    TagPrefix="uc6" %>
<%@ Register Src="../../WebControls/Combos/UCComboAlunoSituacao.ascx" TagName="UCComboAlunoSituacao"
    TagPrefix="uc14" %>
<%@ Register Src="../../WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc15" %>
<%@ Register Src="../../WebControls/Combos/UCComboAlunoCurriculoTipoIngresso.ascx"
    TagName="UCComboAlunoCurriculoTipoIngresso" TagPrefix="uc16" %>
<%@ Register Src="../../WebControls/Combos/UCComboAlunoCurriculoSituacao.ascx" TagName="UCComboAlunoCurriculoSituacao"
    TagPrefix="uc17" %>
<%@ Register Src="../../WebControls/CertidaoCivil/UCGridCertidaoCivil.ascx" TagName="UCGridCertidaoCivil"
    TagPrefix="uc21" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino"
    TagPrefix="uc27" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoModalidadeEnsino.ascx" TagName="UCComboTipoModalidadeEnsino"
    TagPrefix="uc28" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina"
    TagPrefix="uc29" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoRedeEnsino.ascx" TagName="UCComboTipoRedeEnsino"
    TagPrefix="uc30" %>
<%@ Register Src="../../WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas"
    TagPrefix="uc31" %>
<%@ Register Src="../../WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc32" %>
<%@ Register Src="../../WebControls/Endereco/UCEnderecos.ascx" TagName="UCEnderecos"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc36" %>
<%@ Register Src="../../WebControls/Combos/UCComboAlunoHistorico.ascx" TagName="UCComboAlunoHistorico"
    TagPrefix="uc37" %>
<%@ Register Src="../../WebControls/Combos/UCComboReligiao.ascx" TagName="UCComboReligiao"
    TagPrefix="uc38" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoAtendimentoEspecial.ascx" TagName="UCComboTipoAtendimentoEspecial"
    TagPrefix="uc39" %>
<%@ Register Src="../../WebControls/Contato/UCGridContatoNomeTelefone.ascx" TagName="UCGridContatoNomeTelefone"
    TagPrefix="uc40" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/AlunoResponsavel/UCAlunoResponsavel.ascx" TagName="UCAlunoResponsavel"
    TagPrefix="uc4" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoResponsavelAluno.ascx" TagName="UCComboTipoResponsavelAluno"
    TagPrefix="uc7" %>
<%@ Register Src="../../WebControls/Busca/UCPessoasAluno.ascx" TagName="UCPessoasAluno" TagPrefix="uc41" %>
<%@ Register Src="../../WebControls/Cidade/UCCadastroCidade.ascx" TagName="UCCadastroCidade"
    TagPrefix="uc8" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoMovimentacao.ascx" TagName="UCComboTipoMovimentacao"
    TagPrefix="uc9" %>
<%@ Register Src="../../WebControls/Combos/UCComboTurma.ascx" TagName="UCComboTurma"
    TagPrefix="uc10" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divAlunoExistente" title="Possível duplicidade de alunos" class="hide">
        <fieldset>
            <asp:Label ID="lblAlunoExistente" runat="server" Text="Nome: "></asp:Label>
            <div class="right">
                <asp:Button ID="btnConfirmarAlunoExistente" runat="server" CausesValidation="False"
                    OnClick="btnCancelarAlunoExistente_Click" Text="Confirmar inclusão de novo aluno" />
                <asp:Button ID="btnCancelarAlunoExistente" runat="server" CausesValidation="False"
                    OnClientClick="$('#divAlunoExistente').dialog('close'); return false;" Text="Cancelar inclusão de novo aluno" />
            </div>
        </fieldset>
    </div>
    <div id="divDuplicidadeFonetica" title="Possível duplicidade de alunos na busca fonética"
        class="hide">
        <fieldset>
            <asp:Label ID="lblDuplicidadeFonetica" runat="server" Text="Nome: "></asp:Label>
            <asp:GridView ID="grvDuplicidadeFonetica" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="pes_nome" HeaderText="Nome" />
                    <asp:BoundField DataField="pes_dataNascimento" HeaderText="Data de nascimento">
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="pes_nomeMae" HeaderText="Nome da mãe" />
                </Columns>
            </asp:GridView>
            <div class="right">
                <asp:Button ID="btnConfirmarAluno" runat="server" CausesValidation="False" OnClick="btnConfirmarAluno_Click"
                    Text="Confirmar inclusão de novo aluno" />
                <asp:Button ID="btnCancelarAluno" runat="server" CausesValidation="False" OnClientClick="$('#divDuplicidadeFonetica').dialog('close'); return false;"
                    Text="Cancelar inclusão de novo aluno" />
            </div>
        </fieldset>
    </div>
    <div id="divHistorico" title="Cadastro de histórico escolar" class="hide">
        <asp:UpdatePanel ID="upnHistorico" runat="server">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader5" runat="server" AssociatedUpdatePanelID="upnHistorico" />
                <div id="divDialogHistorico" runat="server" visible="false">
                    <asp:Label ID="_lblMessageHistorico" runat="server" EnableViewState="False"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="Historico" />
                    <fieldset>
                        <uc36:UCCamposObrigatorios ID="UCCamposObrigatorios2" runat="server" />
                        <uc27:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino1" runat="server" />
                        <uc28:UCComboTipoModalidadeEnsino ID="UCComboTipoModalidadeEnsino1" runat="server" />
                        <asp:Label ID="LabelEscolaOrigem" runat="server" Text="Escola" AssociatedControlID="_txtEscolaOrigem"></asp:Label>
                        <asp:TextBox ID="_txtEscolaOrigem" runat="server" SkinID="text60C" MaxLength="200"
                            Enabled="false"></asp:TextBox>
                        <asp:ImageButton ID="_btnPesquisarEscolaOrigem" runat="server" CausesValidation="false"
                            SkinID="btPesquisar" OnClick="_btnPesquisarEscolaOrigem_Click" />
                        <asp:Label ID="LabelSerie" runat="server" Text="Período *" AssociatedControlID="_txtSerie"></asp:Label>
                        <asp:TextBox ID="_txtSerie" runat="server" SkinID="Numerico" MaxLength="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvSerie" ControlToValidate="_txtSerie" ValidationGroup="Historico"
                            runat="server" ErrorMessage="Período é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LabelAnoLetivo" runat="server" Text="Ano *" AssociatedControlID="_txtAnoLetivo"></asp:Label>
                        <asp:TextBox ID="_txtAnoLetivo" runat="server" SkinID="Numerico" MaxLength="4"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvAnoLetivo" ControlToValidate="_txtAnoLetivo"
                            ValidationGroup="Historico" runat="server" ErrorMessage="Ano é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="_revAnoLetivo" runat="server" ControlToValidate="_txtAnoLetivo"
                            ValidationGroup="Historico" Display="Dynamic" ErrorMessage="" ValidationExpression="^([0-9]){4}$">*</asp:RegularExpressionValidator>
                        <asp:Label ID="LabelResultado" runat="server" Text="Resultado *" AssociatedControlID="_ddlResultado"></asp:Label>
                        <asp:DropDownList ID="_ddlResultado" runat="server" AppendDataBoundItems="True" SkinID="text30C">
                            <asp:ListItem Value="-1">-- Selecione um resultado --</asp:ListItem>
                            <asp:ListItem Value="1">Aprovado</asp:ListItem>
                            <asp:ListItem Value="2">Aprovado com dependência</asp:ListItem>
                            <asp:ListItem Value="3">Aprovado pelo conselho</asp:ListItem>
                            <asp:ListItem Value="4">Reprovado </asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="_cpvResultado" runat="server" ErrorMessage="Resultado é obrigatório."
                            ControlToValidate="_ddlResultado" Operator="GreaterThan" ValueToCompare="0" Display="Dynamic"
                            ValidationGroup="Historico">*</asp:CompareValidator>
                        <asp:Label ID="LabelTipoControle" runat="server" Text="Tipo de controle *" AssociatedControlID="_ddlTipoControle"></asp:Label>
                        <asp:DropDownList ID="_ddlTipoControle" runat="server" AppendDataBoundItems="True"
                            AutoPostBack="True" OnSelectedIndexChanged="_ddlTipoControle_SelectedIndexChanged"
                            SkinID="text30C">
                            <asp:ListItem Value="-1">-- Selecione um tipo de controle --</asp:ListItem>
                            <asp:ListItem Value="1">Por disciplina</asp:ListItem>
                            <asp:ListItem Value="2">Global</asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="_cpvTipoControle" runat="server" ErrorMessage="Tipo de controle é obrigatório."
                            ControlToValidate="_ddlTipoControle" Operator="GreaterThan" ValueToCompare="0"
                            Display="Dynamic" ValidationGroup="Historico">*</asp:CompareValidator>
                        <div id="divHistoricoNotaGlobal" runat="server">
                            <asp:Label ID="LabelAvaliacao" runat="server" Text="Avaliação *" AssociatedControlID="_txtAvaliacao"></asp:Label>
                            <asp:TextBox ID="_txtAvaliacao" runat="server" SkinID="Numerico" CssClass="numeric"
                                MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_rfvAvaliacao" ControlToValidate="_txtAvaliacao"
                                ValidationGroup="Historico" runat="server" ErrorMessage="Avaliação é obrigatório.">*</asp:RequiredFieldValidator>
                            <asp:Label ID="LabelFrequencia" runat="server" Text="Frequência" AssociatedControlID="_txtFrequencia"></asp:Label>
                            <asp:TextBox ID="_txtFrequencia" runat="server" SkinID="Numerico" CssClass="numeric"
                                MaxLength="100"></asp:TextBox>
                        </div>
                    </fieldset>
                    <fieldset id="fdsHistoricoDisciplinas" runat="server">
                        <legend>Cadastro de disciplinas</legend>
                        <div>
                            <div>
                                <asp:Button ID="_btnNovoDisciplina" runat="server" Text="Adicionar disciplina" CausesValidation="false"
                                    OnClick="_btnNovoDisciplina_Click" />
                            </div>
                            <asp:GridView ID="_grvHistoricoDisciplinas" runat="server" AutoGenerateColumns="False"
                                EmptyDataText="Não existem disciplinas cadastradas." DataKeyNames="alh_id,ahd_id"
                                OnRowCommand="_grvHistoricoDisciplinas_RowCommand" OnRowDataBound="_grvHistoricoDisciplinas_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="tds_nome" HeaderText="Tipo de disciplina" />
                                    <asp:TemplateField HeaderText="Disciplina">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("alh_disciplina") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="_btnAlterar" runat="server" CausesValidation="False" CommandName="Alterar"
                                                Text='<%# Bind("ahd_disciplina") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ahd_resultadoDescricao" HeaderText="Resultado" />
                                    <asp:TemplateField HeaderText="Excluir">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="_btnExcluir" runat="server" CausesValidation="False" CommandName="Excluir"
                                                SkinID="btExcluir" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                    <fieldset id="fdsHistoricoObservacao" runat="server">
                        <legend>Cadastro de observações</legend>
                        <div>
                            <div>
                                <asp:Button ID="_btnNovoHistoricoObservacao" runat="server" Text="Adicionar observação"
                                    CausesValidation="false" OnClick="_btnNovoHistoricoObservacao_Click" />
                            </div>
                            <asp:GridView ID="_grvHistoricoObservacoes" runat="server" AutoGenerateColumns="False"
                                EmptyDataText="Não existem observações cadastradas." DataKeyNames="alh_id,aho_id"
                                OnRowCommand="_grvHistoricoObservacoes_RowCommand" OnRowDataBound="_grvHistoricoObservacoes_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Observação" ItemStyle-CssClass="wrap">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="_btnAlterar" runat="server" CausesValidation="False" CommandName="Alterar"
                                                Text='<%# Bind("aho_descricao") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Excluir">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="_btnExcluir" runat="server" CausesValidation="False" CommandName="Excluir"
                                                SkinID="btExcluir" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                    <div class="right">
                        <asp:Button ID="_btnIncluirHistorico" runat="server" Text="Incluir" OnClick="_btnIncluirHistorico_Click"
                            ValidationGroup="Historico" />
                        <asp:Button ID="_btnCancelarHistorico" runat="server" Text="Cancelar" CausesValidation="False"
                            OnClientClick="$('#divHistorico').dialog('close');return false;" />
                    </div>
                    </fieldset>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divBuscaEscolaOrigem" title="Busca de escola de origem" class="hide">
        <asp:UpdatePanel ID="upnBuscaEscolaOrigem" runat="server">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader6" runat="server" AssociatedUpdatePanelID="upnBuscaEscolaOrigem" />
                <div id="divDialogBuscaEscolaOrigem" runat="server" visible="false">
                    <fieldset>
                        <asp:Label ID="_lblNome" runat="server" Text="Escola de origem" AssociatedControlID="_txtBuscaEscolaOrigem"></asp:Label>
                        <asp:TextBox ID="_txtBuscaEscolaOrigem" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                        <div class="right">
                            <asp:Button ID="_btnBuscaEscolaOrigem" runat="server" Text="Pesquisar" CausesValidation="false"
                                OnClick="_btnBuscaEscolaOrigem_Click" />
                            <asp:Button ID="_btnNovoEscolaOrigem" runat="server" Text="Nova escola de origem"
                                CausesValidation="False" OnClick="_btnNovoEscolaOrigem_Click" />
                            <asp:Button ID="_btnCancelarBuscaEscolaOrigem" runat="server" Text="Cancelar" CausesValidation="False"
                                OnClientClick="$('#divBuscaEscolaOrigem').dialog('close');return false;" />
                        </div>
                    </fieldset>
                    <fieldset id="fdsResultadosEscolaOrigem" runat="server">
                        <legend>Resultados</legend>
                        <asp:GridView ID="_grvEscolaOrigem" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataKeyNames="eco_id,eco_nome" DataSourceID="odsEscolaOrigem" EmptyDataText="A pesquisa não encontrou resultados."
                            OnRowCommand="_grvEscolaOrigem_RowCommand" OnRowDataBound="_grvEscolaOrigem_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Escola de origem">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="_btnSelecionar" runat="server" CausesValidation="False" CommandName="Selecionar"
                                            Text='<%# Bind("eco_nome") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("eco_nome") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="eco_codigoInep" HeaderText="Código INEP" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                    <asp:ObjectDataSource ID="odsEscolaOrigem" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_AlunoEscolaOrigem"
                        EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
                        SelectMethod="GetSelect" StartRowIndexParameterName="currentPage" TypeName="MSTech.GestaoEscolar.BLL.ACA_AlunoEscolaOrigemBO"
                        OnSelecting="odsEscolaOrigem_Selecting"></asp:ObjectDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divEscolaOrigem" title="Cadastro de escola de origem" class="hide">
        <asp:UpdatePanel ID="upnEscolaOrigem" runat="server">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader7" runat="server" AssociatedUpdatePanelID="upnEscolaOrigem" />
                <div id="divDialogEscolaOrigem" runat="server" visible="false">
                    <fieldset>
                        <asp:Label ID="_lblMessageEscolaOrigem" runat="server" EnableViewState="False"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary7" runat="server" ValidationGroup="EscolaOrigem" />
                        <uc30:UCComboTipoRedeEnsino ID="UCComboTipoRedeEnsino1" runat="server" />
                        <asp:Label ID="LabelNomeEscolaOrigem" runat="server" Text="Escola de origem *" AssociatedControlID="_txtNomeEscolaOrigem"></asp:Label>
                        <asp:TextBox ID="_txtNomeEscolaOrigem" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvNomeEscolaOrigem" ControlToValidate="_txtNomeEscolaOrigem"
                            ValidationGroup="EscolaOrigem" runat="server" ErrorMessage="Escola de origem é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LabelCodigoInepEscolaOrigem" runat="server" Text="Código INEP" AssociatedControlID="_txtCodigoInepEscolaOrigem"></asp:Label>
                        <asp:TextBox ID="_txtCodigoInepEscolaOrigem" runat="server" SkinID="text20C" MaxLength="20"></asp:TextBox>
                        <uc1:UCEnderecos ID="UCEnderecos2" runat="server" />
                        <div class="right">
                            <asp:Button ID="_btnIncluirEscolaOrigem" runat="server" Text="Selecionar" OnClick="_btnIncluirEscolaOrigem_Click"
                                ValidationGroup="EscolaOrigem" />
                            <asp:Button ID="_btnCancelarEscolaOrigem" runat="server" Text="Cancelar" CausesValidation="False"
                                OnClientClick="$('#divEscolaOrigem').dialog('close');return false;" />
                        </div>
                    </fieldset>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDisciplina" title="Cadastro de disciplinas" class="hide">
        <asp:UpdatePanel ID="upnDisciplinas" runat="server">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader8" runat="server" AssociatedUpdatePanelID="upnDisciplinas" />
                <div id="divDialogDisciplina" runat="server" visible="false">
                    <asp:Label ID="_lblMessageDisciplina" runat="server" EnableViewState="False"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary6" runat="server" ValidationGroup="Disciplina" />
                    <fieldset>
                        <uc29:UCComboTipoDisciplina ID="UCComboTipoDisciplina1" runat="server" />
                        <asp:Label ID="LabelDisciplina" runat="server" Text="Disciplina *" AssociatedControlID="_txtDisciplina"></asp:Label>
                        <asp:TextBox ID="_txtDisciplina" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvDisciplina" ControlToValidate="_txtDisciplina"
                            ValidationGroup="Disciplina" runat="server" ErrorMessage="Disciplina é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LabelResultadoDisciplina" runat="server" Text="Resultado *" AssociatedControlID="_ddlResultadoDisciplina"></asp:Label>
                        <asp:DropDownList ID="_ddlResultadoDisciplina" runat="server" AppendDataBoundItems="True">
                            <asp:ListItem Value="-1">-- Selecione um resultado --</asp:ListItem>
                            <asp:ListItem Value="1">Aprovado</asp:ListItem>
                            <asp:ListItem Value="2">Aprovado em dependência</asp:ListItem>
                            <asp:ListItem Value="3">Reprovado </asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="_cpvResultadoDisciplina" runat="server" ErrorMessage="Resultado é obrigatório."
                            ControlToValidate="_ddlResultadoDisciplina" Operator="GreaterThan" ValueToCompare="0"
                            Display="Dynamic" ValidationGroup="Disciplina">*</asp:CompareValidator>
                        <asp:Label ID="LabelAvaliacaoDisciplina" runat="server" Text="Avaliação" AssociatedControlID="_txtAvaliacaoDisciplina"></asp:Label>
                        <asp:TextBox ID="_txtAvaliacaoDisciplina" runat="server" SkinID="text10C" MaxLength="100"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="_revAvaliacaoDisciplina" runat="server" ControlToValidate="_txtAvaliacaoDisciplina"
                            ValidationGroup="Disciplina" ValidationExpression="^\d*[0-9](\,\d*[0-9])?$" ErrorMessage="Avaliação inválida">*</asp:RegularExpressionValidator>
                        <asp:Label ID="LabelFrequenciaDisciplina" runat="server" Text="Frequência" AssociatedControlID="_txtFrequenciaDisciplina"></asp:Label>
                        <asp:TextBox ID="_txtFrequenciaDisciplina" runat="server" SkinID="text10C" MaxLength="100"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="_revFrequenciaDisciplina" runat="server" ControlToValidate="_txtFrequenciaDisciplina"
                            ValidationGroup="Disciplina" ValidationExpression="^\d*[0-9](\,\d*[0-9])?$" ErrorMessage="Freqüência inválida">*</asp:RegularExpressionValidator>
                        <div class="right">
                            <asp:Button ID="_btnIncluirDisciplina" runat="server" Text="Incluir" OnClick="_btnIncluirDisciplina_Click"
                                ValidationGroup="Disciplina" />
                            <asp:Button ID="_btnCancelarDisciplina" runat="server" Text="Cancelar" CausesValidation="False"
                                OnClientClick="$('#divDisciplina').dialog('close'); return false;" />
                        </div>
                    </fieldset>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divObservacao" title="Cadastro de observação" class="hide">
        <asp:UpdatePanel ID="upnHistoricoObservacao" runat="server">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader9" runat="server" AssociatedUpdatePanelID="upnHistoricoObservacao" />
                <div id="divDialogHistoricoObservacao" runat="server" visible="false">
                    <asp:Label ID="_lblMessageHistoricoObservacao" runat="server" EnableViewState="False"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="HistoricoObservacao" />
                    <fieldset>
                        <div>
                            <uc37:UCComboAlunoHistorico ID="UCComboAlunoHistorico" runat="server" />
                            <asp:Label ID="LabelObservacao1" runat="server" Text="Observação" AssociatedControlID="_txtObservacao1"></asp:Label>
                            <asp:TextBox ID="_txtObservacao1" runat="server" TextMode="MultiLine" SkinID="text60C"
                                MaxLength="1000"></asp:TextBox>
                        </div>
                        <div class="right">
                            <asp:Button ID="_btnIncluirHistoricoObservacao" runat="server" Text="Incluir" OnClick="_btnIncluirHistoricoObservacao_Click"
                                ValidationGroup="HistoricoObservacao" />
                            <asp:Button ID="_btnCancelarHistoricoObservacao" runat="server" Text="Cancelar" CausesValidation="False"
                                OnClientClick="$('#divObservacao').dialog('close'); return false;" />
                        </div>
                    </fieldset>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divBuscaResponsavel" title="Busca de pessoas" class="hide divBuscaResponsavel">
        <asp:UpdatePanel ID="_updBuscaPessoas" runat="server">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader3" runat="server" AssociatedUpdatePanelID="_updBuscaPessoas" />
                <uc41:UCPessoasAluno ID="UCBuscaPessoasAluno1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="upnMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Pessoa" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divCadastroCidade" title="Cadastro de cidades" class="hide">
        <asp:UpdatePanel ID="_updCidades" runat="server">
            <ContentTemplate>
                <uc1:UCLoader ID="loaderCadastroCidades" runat="server" AssociatedUpdatePanelID="_updCidades" />
                <uc8:UCCadastroCidade ID="UCCadastroCidade1" runat="server" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divTabs">
        <ul class="hide">
            <li><a href="#divTabs-0">Dados pessoais</a></li>
            <li><a href="#divTabs-1">Endereço / contato</a></li>
            <li><a href="#divTabs-2">Documentação</a></li>
            <li><a href="#divTabs-3">Movimentação</a></li>
            <li><a href="#divTabs-4">Histórico</a></li>
            <li><a href="#divTabs-5">Ficha médica</a></li>
            <li><a href="#divTabs-6">Usuários</a></li>
        </ul>
        <div id="divTabs-0">
            <div id="divTabDadosPessoais" runat="server">
                <fieldset id="fdsDadosPessoais" runat="server">
                    <asp:UpdatePanel ID="uppDadosPessoais" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc1:UCLoader ID="UCLoader4" runat="server" AssociatedUpdatePanelID="uppDadosPessoais" />
                            <uc36:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />
                            <uc3:UCCadastroPessoa ID="UCCadastroPessoa1" runat="server" />
                            <uc38:UCComboReligiao ID="UCComboReligiao" runat="server" />
                            <uc39:UCComboTipoAtendimentoEspecial ID="UCComboTipoAtendimentoEspecial1" runat="server" />
                            <asp:Label ID="LabelMeioTransporte" runat="server" Text="Meio de transporte" AssociatedControlID="_ddlMeioTransporte"></asp:Label>
                            <asp:DropDownList ID="_ddlMeioTransporte" runat="server" AppendDataBoundItems="True">
                                <asp:ListItem Value="-1">-- Selecione um meio de transporte --</asp:ListItem>
                                <asp:ListItem Value="1">Pedestre</asp:ListItem>
                                <asp:ListItem Value="2">Ônibus</asp:ListItem>
                                <asp:ListItem Value="3">Trem</asp:ListItem>
                                <asp:ListItem Value="4">Carro </asp:ListItem>
                                <asp:ListItem Value="5">Metrô </asp:ListItem>
                                <asp:ListItem Value="6">Outros </asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="LabelTempoDeslocamento" runat="server" Text="Tempo de deslocamento"
                                AssociatedControlID="_ddlTempoDeslocamento"></asp:Label>
                            <asp:DropDownList ID="_ddlTempoDeslocamento" runat="server" AppendDataBoundItems="True">
                                <asp:ListItem Value="-1">-- Selecione um tempo de deslocamento --</asp:ListItem>
                                <asp:ListItem Value="1">Até 15 minutos</asp:ListItem>
                                <asp:ListItem Value="2">Até 30 minutos</asp:ListItem>
                                <asp:ListItem Value="3">Até 1 hora</asp:ListItem>
                                <asp:ListItem Value="4">Mais de 1 hora </asp:ListItem>
                            </asp:DropDownList>
                            <asp:CheckBox ID="chkRegressaSozinho" runat="server" Text="Regressa sozinho" />
                            <asp:Label ID="LabelObservacao" runat="server" Text="Observação" AssociatedControlID="_txtObservacao"></asp:Label>
                            <asp:TextBox ID="_txtObservacao" runat="server" TextMode="MultiLine" SkinID="text60C"></asp:TextBox>
                            <uc14:UCComboAlunoSituacao ID="UCComboAlunoSituacao1" runat="server" />
                            <asp:CompareValidator ID="_cpvAlunoSituacao" runat="server" ErrorMessage="Situação do aluno é obrigatório."
                                ControlToValidate="UCComboAlunoSituacao1:ddlAlunoSituacao" Operator="GreaterThan"
                                ValueToCompare="0" Display="Dynamic" ValidationGroup="Pessoa">* </asp:CompareValidator>
                            <asp:CheckBox ID="chkDadosIncompletos" runat="server" Text="Dados cadastrais incompletos" />
                            <asp:CheckBox ID="chkHistoricoEscolarIncompleto" runat="server" Text="Histórico escolar incompleto" />
                            <br />
                            <fieldset id="fsProgramaSocial" runat="server">
                                <legend>Programas sociais</legend>
                                <asp:CheckBoxList ID="cblProgramaSocial" runat="server" DataSourceID="odsProgramaSocial"
                                    DataTextField="pso_descricao" DataValueField="pso_id">
                                </asp:CheckBoxList>
                                <asp:ObjectDataSource ID="odsProgramaSocial" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_ProgramaSocial"
                                    DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" SelectMethod="CarregarProgramaSocial"
                                    TypeName="MSTech.GestaoEscolar.BLL.ACA_ProgramaSocialBO" UpdateMethod="Save">
                                    <DeleteParameters>
                                        <asp:Parameter Name="entity" Type="Object" />
                                        <asp:Parameter Name="banco" Type="Object" />
                                    </DeleteParameters>
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="false" Name="paginado" Type="Boolean" />
                                        <asp:Parameter DefaultValue="1" Name="currentPage" Type="Int32" />
                                        <asp:Parameter DefaultValue="1" Name="pageSize" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </fieldset>
                            <asp:UpdatePanel ID="upnTipoResponsavel" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlResponsavel" runat="server" GroupingText="Responsável">
                                        <uc7:UCComboTipoResponsavelAluno ID="ucComboTipoResponsavel" runat="server" />
                                        <uc4:UCAlunoResponsavel ID="ucResponsavelOutro" runat="server" Visible="false" VS_TipoResponsavel="Outro"
                                            ValidationGroup="Pessoa" />
                                    </asp:Panel>
                                    <uc4:UCAlunoResponsavel ID="ucResponsavelMae" runat="server" VS_TipoResponsavel="Mae"
                                        ValidationGroup="Pessoa" />
                                    <uc4:UCAlunoResponsavel ID="ucResponsavelPai" runat="server" VS_TipoResponsavel="Pai"
                                        ValidationGroup="Pessoa" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>
        </div>
        <div id="divTabs-1" class="hide">
            <div id="divTabEnderecos" runat="server">
                <br />
                <fieldset id="fdsEndereco" runat="server">
                    <legend>Cadastro de endereços</legend>
                    <uc1:UCEnderecos ID="UCEnderecos1" runat="server" />
                </fieldset>
                <fieldset id="fdsContato" runat="server">
                    <legend>Cadastro de contatos</legend>
                    <uc42:UCContato ID="UCContato1" runat="server" />
                </fieldset>
            </div>
        </div>
        <div id="divTabs-2" class="hide">
            <div id="divTabsDocumento" runat="server">
                <br />
                <fieldset id="fdsDocumento" runat="server">
                    <legend>Cadastro de documentos</legend>
                    <uc6:UCGridDocumento ID="UCGridDocumento1" runat="server" />
                </fieldset>
                <fieldset id="fdsCertidoes" runat="server">
                    <legend>Cadastro de certidões civis</legend>
                    <uc21:UCGridCertidaoCivil ID="UCGridCertidaoCivil1" runat="server" />
                </fieldset>
            </div>
        </div>
        <div id="divTabs-3" class="hide">
            <div id="divMovimentacao" runat="server">
                <asp:UpdatePanel ID="uppMovimentacao" runat="server">
                    <ContentTemplate>
                        <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="uppMovimentacao" />
                        <fieldset id="fdsMovimentacao" runat="server">
                            <div id="divNovaMovimentacao" runat="server" visible="false">
                                <asp:Label ID="_lblMessageMatricula" runat="server" EnableViewState="False"></asp:Label>
                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Matricula" />
                                <fieldset>
                                    <uc36:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
                                    <uc9:UCComboTipoMovimentacao ID="UCComboTipoMovimentacaoEntrada" runat="server" />
                                    <uc31:UCFiltroEscolas ID="UCFiltroEscolas1" runat="server" />
                                    <br />
                                    <div id="divEscolasProximas" runat="server" style="position: absolute; top: 124px;
                                        right: 17px;" visible="false">
                                        <asp:Label ID="_lblEscProx" runat="server" AssociatedControlID="_lstEscProx" Text="Sugestões de escolas próximas"></asp:Label>
                                        <asp:ListBox ID="_lstEscProx" runat="server" AutoPostBack="True" OnSelectedIndexChanged="_lstEscProx_SelectedIndexChanged"
                                            Width="350px" Height="150px"></asp:ListBox>
                                        <br />
                                    </div>
                                    <uc15:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" runat="server" />
                                    <uc32:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo1" runat="server" />
                                    <uc10:UCComboTurma ID="UCComboTurma1" runat="server" />
                                    <asp:Label ID="LabelMatriculaNumero" runat="server" AssociatedControlID="_txtMatriculaNumero"
                                        Text="Número de matrícula"></asp:Label>
                                    <asp:TextBox ID="_txtMatriculaNumero" runat="server" MaxLength="50" SkinID="text30C"></asp:TextBox>
                                    <asp:Label ID="LabelCensoID" runat="server" AssociatedControlID="_txtCensoID" Text="ID Censo"></asp:Label>
                                    <asp:TextBox ID="_txtCensoID" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
                                    <div id="divMatriculaEstadual" runat="server">
                                        <asp:Label ID="_lblMatrEst" runat="server" AssociatedControlID="_txtMatriculaEstadual"
                                            Text="Matrícula estadual"></asp:Label>
                                        <asp:TextBox ID="_txtMatriculaEstadual" runat="server" SkinID="text20C"></asp:TextBox>
                                    </div>
                                    <asp:Label ID="LabelMatriculaDataPrimeira" runat="server" AssociatedControlID="_txtMatriculaDataPrimeira"
                                        Text="Data da primeira matrícula"></asp:Label>
                                    <asp:TextBox ID="_txtMatriculaDataPrimeira" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                    <asp:CustomValidator ID="cvMatriculaDataPrimeira" runat="server" ControlToValidate="_txtMatriculaDataPrimeira"
                                        ValidationGroup="Matricula" Display="Dynamic" ErrorMessage=""
                                        OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                                    <asp:Label ID="LabelMatriculaDataSaida" runat="server" AssociatedControlID="_txtMatriculaDataSaida"
                                        Text="Data de saída"></asp:Label>
                                    <asp:TextBox ID="_txtMatriculaDataSaida" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                    <asp:CustomValidator ID="cvMatriculaDataSaida" runat="server" ControlToValidate="_txtMatriculaDataSaida"
                                        ValidationGroup="Matricula" Display="Dynamic" ErrorMessage=""
                                        OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                                    <asp:Label ID="LabelMatriculaDataColacao" runat="server" AssociatedControlID="_txtMatriculaDataColacao"
                                        Text="Data de publicação no Diário Oficial"></asp:Label>
                                    <asp:TextBox ID="_txtMatriculaDataColacao" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                    <asp:CustomValidator ID="cvMatriculaDataColacao" runat="server" ControlToValidate="_txtMatriculaDataColacao"
                                        ValidationGroup="Matricula" Display="Dynamic" ErrorMessage=""
                                        OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                                </fieldset>
                            </div>
                            <div id="divVelhaMovimentacao" runat="server" visible="false">
                                <asp:Panel ID="pnlDadosAtuais" runat="server" GroupingText="Dados atuais do aluno">
                                    <asp:Label ID="lblDadosAluno" runat="server"></asp:Label>
                                    <asp:Label ID="Label3" runat="server" AssociatedControlID="_txtMatriculaNumero2"
                                        Text="Número de matrícula"></asp:Label>
                                    <asp:TextBox ID="_txtMatriculaNumero2" runat="server" MaxLength="50" SkinID="text30C"></asp:TextBox>
                                    <asp:Label ID="Label4" runat="server" AssociatedControlID="_txtCensoID2" Text="ID Censo"></asp:Label>
                                    <asp:TextBox ID="_txtCensoID2" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
                                    <div id="div1" runat="server">
                                        <asp:Label ID="_lblMatriculaEstadual2" runat="server" AssociatedControlID="_txtMatriculaEstadual2"
                                            Text="Matrícula estadual"></asp:Label>
                                        <asp:TextBox ID="_txtMatriculaEstadual2" runat="server" SkinID="text20C"></asp:TextBox>
                                    </div>
                                    <asp:Label ID="Label6" runat="server" AssociatedControlID="_txtMatriculaDataPrimeira2"
                                        Text="Data da primeira matrícula"></asp:Label>
                                    <asp:TextBox ID="_txtMatriculaDataPrimeira2" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="_txtMatriculaDataPrimeira"
                                        ValidationGroup="Matricula" Display="Dynamic" ErrorMessage=""
                                        OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                                    <asp:Label ID="Label7" runat="server" AssociatedControlID="_txtMatriculaDataSaida2"
                                        Text="Data de saída"></asp:Label>
                                    <asp:TextBox ID="_txtMatriculaDataSaida2" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="_txtMatriculaDataSaida"
                                        ValidationGroup="Matricula" Display="Dynamic" ErrorMessage=""
                                        OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                                    <asp:Label ID="Label8" runat="server" AssociatedControlID="_txtMatriculaDataColacao2"
                                        Text="Data de publicação no Diário Oficial"></asp:Label>
                                    <asp:TextBox ID="_txtMatriculaDataColacao2" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator3" runat="server" ControlToValidate="_txtMatriculaDataColacao"
                                        ValidationGroup="Matricula" Display="Dynamic" ErrorMessage=""
                                        OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                                </asp:Panel>
                                <asp:Panel ID="pnlDadosMovimentacao" runat="server" GroupingText="Dados da movimentação do aluno">
                                    <asp:Label ID="lblInformacao" runat="server"></asp:Label>
                                    <asp:Label ID="lblAcao" runat="server" Text="Ação"></asp:Label>
                                   <br />
                                   <br />
                                    <asp:CheckBox ID="ckbAltTurma" runat="server" Text="Incluir / Alterar aluno na turma"
                                        OnCheckedChanged="checkBox_CheckedChanged" AutoPostBack="True"></asp:CheckBox>
                                    <asp:CheckBox ID="ckbRecAluno" runat="server" Text="Reclassificar aluno" OnCheckedChanged="checkBox_CheckedChanged"
                                        AutoPostBack="True"></asp:CheckBox>
                                    <asp:CheckBox ID="ckbAltCurso" runat="server" Text="Alterar curso" OnCheckedChanged="checkBox_CheckedChanged"
                                        AutoPostBack="True"></asp:CheckBox>
                                    <asp:CheckBox ID="ckbTranfDentroRede" runat="server" Text="Transferir aluno dentro da rede"
                                        OnCheckedChanged="checkBox_CheckedChanged" AutoPostBack="True"></asp:CheckBox>
                                    <asp:CheckBox ID="ckbTranfForaRede" runat="server" Text="Transferir aluno para fora da rede"
                                        OnCheckedChanged="checkBox_CheckedChanged" AutoPostBack="True"></asp:CheckBox>
                                   <br />
                                   <br />
                                    <uc9:UCComboTipoMovimentacao ID="UCComboTipoMovimentacaoSaida" runat="server" />
                                    <uc9:UCComboTipoMovimentacao ID="UCComboTipoMovimentacaoEntrada1" runat="server" />
                                    
                                    <asp:Label ID="lblUASuperior" runat="server" AssociatedControlID="ddlUASuperior"
                                        Text="Unidade administrativa superior *"></asp:Label>
                                    <asp:DropDownList ID="ddlUASuperior" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                        DataTextField="uad_nome" DataValueField="uad_id" OnSelectedIndexChanged="ddlUASuperior_SelectedIndexChanged"
                                        SkinID="text60C">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblUnidadeEscola" runat="server" AssociatedControlID="ddlUnidadeEscola"
                                        Text="Escola *"></asp:Label>
                                    <asp:DropDownList ID="ddlUnidadeEscola" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="True" DataTextField="uni_escolaNome" DataValueField="esc_uni_id"
                                        OnSelectedIndexChanged="ddlUnidadeEscola_SelectedIndexChanged" SkinID="text60C">
                                    </asp:DropDownList>
                                    <uc15:UCComboCursoCurriculo ID="UCComboCursoCurriculo2" runat="server" />
                                    <uc32:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo2" runat="server" />
                                    <uc10:UCComboTurma ID="UCComboTurma2" runat="server" />
                                </asp:Panel>
                            </div>
                            <asp:Panel ID="pnlHistoricoMovimentacoes" runat="server" GroupingText="Histórico de movimentações">
                                <asp:GridView ID="grvHistoricoMovimentacao" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    EmptyDataText="Não existem movimentações para este aluno." 
                                    DataSourceID="odsHistoricoMovimentacao" 
                                    onpageindexchanging="grvHistoricoMovimentacao_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="mov_dataRealizacao" HeaderText="Data Movimentação" />
                                        <asp:BoundField DataField="tmv_nomeSaida" HeaderText="Tipo de movimentação de saída"
                                            NullDisplayText="-">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="tmv_nomeEntrada" HeaderText="Tipo de movimentação de entrada"
                                            NullDisplayText="-">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="escolaAnterior" HeaderText="Escola / etapa de ensino / grupamento de ensino anterior"
                                            NullDisplayText="-">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="escolaAtual" HeaderText="Escola / etapa de ensino / grupamento de ensino novo"
                                            NullDisplayText="-">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="turmaAnterior" HeaderText="Turma Anterior" NullDisplayText="-">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="turmaAtual" HeaderText="Turma atual" NullDisplayText="-">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsHistoricoMovimentacao" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.MTR_Movimentacao"
                                    DeleteMethod="Delete" EnablePaging="True" OldValuesParameterFormatString="original_{0}"
                                    OnSelecting="odsHistoricoMovimentacao_Selecting" TypeName="MSTech.GestaoEscolar.BLL.MTR_MovimentacaoBO"
                                    UpdateMethod="Save" SelectMethod="SelecionaMovimentacaoAluno" SelectCountMethod="GetTotalRecords"
                                    MaximumRowsParameterName="pageSize" StartRowIndexParameterName="currentPage">
                                    <DeleteParameters>
                                        <asp:Parameter Name="entity" Type="Object" />
                                        <asp:Parameter Name="banco" Type="Object" />
                                    </DeleteParameters>
                                </asp:ObjectDataSource>
                            </asp:Panel>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id="divTabs-4" class="hide">
            <div id="divTabHistorico" runat="server">
                <asp:UpdatePanel ID="upnGridHistorico" runat="server">
                    <ContentTemplate>
                        <uc1:UCLoader ID="UCLoader11" runat="server" AssociatedUpdatePanelID="upnGridHistorico" />
                        <fieldset id="fdsHistorico" runat="server">
                            <div>
                                <asp:Button ID="_btnNovoHistorico" runat="server" Text="Adicionar histórico escolar"
                                    CausesValidation="false" OnClick="_btnNovoHistorico_Click" />
                            </div>
                            <asp:GridView ID="_grvHistorico" runat="server" AutoGenerateColumns="False" EmptyDataText="Não existem históricos cadastrados."
                                DataKeyNames="alh_id,mtu_id" OnRowCommand="_grvHistorico_RowCommand" OnRowDataBound="_grvHistorico_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="mtu_id" HeaderText="mtu_id">
                                        <HeaderStyle CssClass="hide" />
                                        <ItemStyle CssClass="hide" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="tne_nome" HeaderText="Nível de ensino" />
                                    <asp:TemplateField HeaderText="Período">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("alh_serie") %>'></asp:TextBox></EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="_btnAlterar" runat="server" CausesValidation="False" CommandName="Alterar"
                                                Text='<%# Bind("alh_serie") %>'></asp:LinkButton><asp:Label ID="_lblAlterar" runat="server"
                                                    Text='<%# Bind("alh_serie") %>' Visible="False"></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="alh_anoLetivo" HeaderText="Ano" />
                                    <asp:BoundField DataField="eco_nome" HeaderText="Escola" />
                                    <asp:BoundField DataField="alh_resultadoDescricao" HeaderText="Resultado" />
                                    <asp:TemplateField HeaderText="Excluir">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="_btnExcluir" runat="server" CausesValidation="False" CommandName="Excluir"
                                                SkinID="btExcluir" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id="divTabs-5" class="hide">
            <div id="divTabFichaMedica" runat="server">
                <fieldset id="fdsFichaMedica" runat="server">
                    <asp:Label ID="_lbTipoSanguineo" runat="server" Text="Tipo sanguíneo" AssociatedControlID="_txtTipoSanguineo"></asp:Label>
                    <asp:TextBox ID="_txtTipoSanguineo" runat="server" SkinID="text2C" MaxLength="5"></asp:TextBox>
                    <asp:Label ID="_lbFatorRH" runat="server" Text="Fator rh" AssociatedControlID="_txtFatorRH"></asp:Label>
                    <asp:TextBox ID="_txtFatorRH" runat="server" SkinID="text2C" MaxLength="5"></asp:TextBox>
                    <asp:Label ID="_lbDoencaConhecidas" runat="server" Text="Doenças conhecidas" AssociatedControlID="_txtDoencaConhecidas"></asp:Label>
                    <asp:TextBox ID="_txtDoencaConhecidas" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <asp:Label ID="_lbAlergias" runat="server" Text="Alergias" AssociatedControlID="_txtAlergias"></asp:Label>
                    <asp:TextBox ID="_txtAlergias" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <asp:Label ID="_lbMedicacoesPodeUtilizar" runat="server" Text="Medicamentos que pode utilizar"
                        AssociatedControlID="_txtMedicacoesPodeUtilizar"></asp:Label>
                    <asp:TextBox ID="_txtMedicacoesPodeUtilizar" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <asp:Label ID="_lbMedicacoesUsoContinuo" runat="server" Text="Medicamentos de uso contínuo"
                        AssociatedControlID="_txtMedicacoesUsoContinuo"></asp:Label>
                    <asp:TextBox ID="_txtMedicacoesUsoContinuo" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <asp:Label ID="_lbConvenioMedico" runat="server" Text="Convênio médico" AssociatedControlID="_txtConvenioMedico"></asp:Label>
                    <asp:TextBox ID="_txtConvenioMedico" runat="server" SkinID="text60C" MaxLength="1000"></asp:TextBox>
                    <asp:Label ID="_lbHospitalRemocao" runat="server" Text="Hospital para remoção" AssociatedControlID="_txtHospitalRemocao"></asp:Label>
                    <asp:TextBox ID="_txtHospitalRemocao" runat="server" SkinID="text60C" MaxLength="1000"></asp:TextBox>
                    <asp:Label ID="_lbOutrasRecomendacoes" runat="server" Text="Outras recomendações"
                        AssociatedControlID="_txtOutrasRecomendacoes"></asp:Label>
                    <asp:TextBox ID="_txtOutrasRecomendacoes" runat="server" TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
                </fieldset>
                <fieldset id="fdsCasoEmergencia" runat="server">
                    <legend>Avisar em caso de emergência</legend>
                    <uc40:UCGridContatoNomeTelefone ID="UCGridContatoNomeTelefone1" runat="server" />
                </fieldset>
            </div>
        </div>
        <div id="divTabs-6" class="hide">
            <div id="divTabUsuario" runat="server">
                <asp:UpdatePanel ID="upnUsuario" runat="server">
                    <ContentTemplate>
                        <uc1:UCLoader ID="UCLoader12" runat="server" AssociatedUpdatePanelID="upnUsuario" />
                        <fieldset id="fdsCriarUsuario" runat="server">
                            <asp:CheckBox ID="_chbCriarUsuario" runat="server" AutoPostBack="True" OnCheckedChanged="_chbCriarUsuario_CheckedChanged"
                                Text="Criar usuário" />
                        </fieldset>
                        <div id="divUsuarios" runat="server" visible="false">
                            <fieldset id="fdsUsuario" runat="server">
                                <legend>Cadastro de usuário do aluno</legend>
                                <uc36:UCCamposObrigatorios ID="UCCamposObrigatorios4" runat="server" />
                                <asp:CheckBox ID="_chbIntegrarUsuarioLive" runat="server" AutoPostBack="True" OnCheckedChanged="_chbIntegrarUsuarioLive_CheckedChanged"
                                    Text="Integrar usuário live" />
                                <asp:Label ID="Label1" runat="server" Text="Login *" AssociatedControlID="_txtLogin"></asp:Label>
                                <asp:TextBox ID="_txtLogin" runat="server" MaxLength="100" SkinID="text30C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvLogin" runat="server" ControlToValidate="_txtLogin"
                                    ValidationGroup="Pessoa" ErrorMessage="Login é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator><br />
                                <asp:Label ID="_lblEmail" runat="server" Text="E-mail *" AssociatedControlID="_txtEmail"></asp:Label>
                                <asp:Label ID="_lblMsgUsuarioLive" runat="server" AssociatedControlID="_txtEmail"></asp:Label><br />
                                <asp:TextBox ID="_txtEmail" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvEmail" runat="server" ControlToValidate="_txtEmail"
                                    ValidationGroup="Pessoa" ErrorMessage="E-mail é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="_revEmail" runat="server" ControlToValidate="_txtEmail"
                                    ValidationGroup="Pessoa" ErrorMessage="E-MAIL está fora do padrão ( seuEmail@seuProvedor ) (aba Usuários)."
                                    Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                                <asp:CheckBox ID="_chkSenhaAutomatica" Text="Gerar senha e enviar para o e-mail"
                                    runat="server" AutoPostBack="True" OnCheckedChanged="_chkSenhaAutomatica_CheckedChanged" />
                                <asp:Label ID="_lblSenha" runat="server" Text="Senha *" AssociatedControlID="_txtSenha"></asp:Label>
                                <asp:TextBox ID="_txtSenha" runat="server" MaxLength="256" TextMode="Password" SkinID="text20C"
                                    EnableViewState="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvSenha" runat="server" ControlToValidate="_txtSenha"
                                    ValidationGroup="Pessoa" ErrorMessage="Senha é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revSenhaTamanho" runat="server" ControlToValidate="_txtSenha"
                                    Display="Dynamic" ValidationGroup="Pessoa" ErrorMessage="A senha deve conter {0}."
                                    Enabled="false">*</asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="revSenha" runat="server" ControlToValidate="_txtSenha"
                                    ValidationGroup="Pessoa" Display="Dynamic" ErrorMessage="A senha não pode conter espaços em branco."
                                    ValidationExpression="[^\s]+" Enabled="false">*</asp:RegularExpressionValidator>
                                <asp:Label ID="_lblConfirmacao" runat="server" Text="Confirmar senha *" AssociatedControlID="_txtConfirmacao"></asp:Label>
                                <asp:TextBox ID="_txtConfirmacao" runat="server" MaxLength="256" TextMode="Password"
                                    SkinID="text20C" EnableViewState="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvConfirmarSenha" runat="server" ControlToValidate="_txtConfirmacao"
                                    ValidationGroup="Pessoa" ErrorMessage="Confirmar senha é obrigatório." Display="Dynamic">* </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="_cpvConfirmarSenha" runat="server" ControlToCompare="_txtSenha"
                                    ValidationGroup="Pessoa" ControlToValidate="_txtConfirmacao" ErrorMessage="Senha não confere."
                                    Display="Dynamic">* </asp:CompareValidator>
                                <asp:CheckBox ID="_chkExpiraSenha" runat="server" Text="Expira senha" />
                                <asp:CheckBox ID="_chkBloqueado" runat="server" Text="Bloqueado" />
                            </fieldset>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <fieldset>
        <div class="right" class="divBtnCadastro">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click"
                ValidationGroup="Pessoa" />
            <asp:Button ID="_btnNovo" runat="server" Text="Novo" CausesValidation="false" OnClick="_btnNovo_Click" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                OnClick="_btnCancelar_Click" />
            <input id="txtSelectedTab" type="hidden" class="txtSelectedTab" runat="server" />
        </div>
    </fieldset>
</asp:Content>
