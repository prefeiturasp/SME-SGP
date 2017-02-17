<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_Aluno_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/Aluno/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Pessoa/UCCadastroPessoa.ascx" TagName="UCCadastroPessoa"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboReligiao.ascx" TagName="UCComboReligiao"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoResponsavelAluno.ascx" TagName="UCComboTipoResponsavelAluno"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/AlunoResponsavel/UCAlunoResponsavel.ascx" TagName="UCAlunoResponsavel"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Endereco/UCEnderecos.ascx" TagName="UCEnderecos"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Contato/UCContato.ascx" TagName="UCContato" TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/Documento/UCGridDocumento.ascx" TagName="UCGridDocumento"
    TagPrefix="uc10" %>
<%@ Register Src="~/WebControls/CertidaoCivil/UCCertidaoCivil.ascx" TagName="UCCertidaoCivil"
    TagPrefix="uc11" %>
<%@ Register Src="~/WebControls/Contato/UCGridContatoNomeTelefone.ascx" TagName="UCGridContatoNomeTelefone"
    TagPrefix="uc13" %>
<%@ Register Src="~/WebControls/Cidade/UCCadastroCidade.ascx" TagName="UCCadastroCidade"
    TagPrefix="uc14" %>
<%@ Register Src="~/WebControls/Busca/UCPessoasAluno.ascx" TagName="UCPessoasAluno"
    TagPrefix="uc15" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoRedeEnsino.ascx" TagName="UCComboTipoRedeEnsino"
    TagPrefix="uc19" %>
<%@ Register Src="~/WebControls/Movimentacao/UCMovimentacao.ascx" TagName="UCMovimentacao"
    TagPrefix="uc21" %>
<%@ Register Src="~/WebControls/Mensagens/UCConfirmacaoOperacao.ascx" TagName="UCConfirmacaoOperacao"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/InfoComplementarAluno/InfoComplementarAluno.ascx"
    TagName="UCInfoComplementarAluno" TagPrefix="uc26" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc28" %>
<%@ Register Src="../../WebControls/Combos/UCComboMeioTransporte.ascx" TagName="UCComboMeioTransporte" TagPrefix="uc12" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoRedeEnsino.ascx" TagName="UCComboTipoRedeEnsino"
    TagPrefix="uc31" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always" EnableViewState="False">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="vlsMessage" runat="server" ValidationGroup="Aluno" EnableViewState="False" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <uc26:UCInfoComplementarAluno runat="server" ID="UCInfoComplementarAluno1"></uc26:UCInfoComplementarAluno>
        <div class="clear">
        </div>
        <div class="right">
            <asp:Button ID="btnSalvarCima" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                ValidationGroup="Aluno" />
            <asp:Button ID="btnCancelarCima" runat="server" Text="Cancelar" CausesValidation="False"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
    <div id="divTabs">
        <ul class="hide">
            <li><a href="#divTabs-0">Dados pessoais</a></li>
            <li><a href="#divTabs-1">Endereço / contato</a></li>
            <li><a href="#divTabs-2">Documentação</a></li>
            <li><a href="#divTabs-4" runat="server" id="aTabsMovimentacao">Movimentação</a></li>
            <li><a href="#divTabs-6">Ficha médica</a></li>
        </ul>
        <!-- Dados pessoais -->
        <div id="divTabs-0">
            <fieldset id="fdsDadosPessoais" runat="server">
                <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" EnableViewState="false" />
                <uc2:UCCadastroPessoa ID="UCCadastroPessoa1" runat="server" _ComboSexoObrigatorio="true"
                    LabelNome="Nome do aluno *" ValidationGroup="Aluno" validaDataNascimento="true"
                    visibleMae="false" visiblePai="false" On_AbreJanelaCadastroCidade="AbreJanelaCadastroCidade" />
                <table class="table-padding">
                    <tr>
                        <td>
                            <uc3:UCComboReligiao ID="UCComboReligiao" runat="server" />
                        </td>
                        <td valign="bottom">
                            <asp:CheckBox ID="chbAulaReligiao" runat="server" Text="Deseja ter aula de Ensino Religioso?" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc12:UCComboMeioTransporte ID="UCComboMeioTransporte" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblTempoDeslocamento" runat="server" Text="Tempo de deslocamento"
                                AssociatedControlID="ddlTempoDeslocamento" EnableViewState="False"></asp:Label>
                            <asp:DropDownList ID="ddlTempoDeslocamento" runat="server" AppendDataBoundItems="True"
                                SkinID="text30C">
                                <asp:ListItem Value="-1">-- Selecione um tempo de deslocamento --</asp:ListItem>
                                <asp:ListItem Value="1">Até 15 minutos</asp:ListItem>
                                <asp:ListItem Value="2">Até 30 minutos</asp:ListItem>
                                <asp:ListItem Value="3">Até 1 hora</asp:ListItem>
                                <asp:ListItem Value="4">Mais de 1 hora </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td valign="bottom">
                            <asp:CheckBox ID="chbRegressaSozinho" runat="server" Text="Regressa sozinho" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblObservacao" runat="server" Text="Observação" AssociatedControlID="txtObservacao"
                                EnableViewState="False"></asp:Label>
                            <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblCodigoExterno" runat="server" Text="<%$ Resources:Academico, Aluno.Cadastro.lblCodigoExterno.Text %>"
                                AssociatedControlID="txtCodigoExterno" EnableViewState="False"></asp:Label>
                            <asp:TextBox ID="txtCodigoExterno" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbDadosIncompletos" runat="server" Text="Dados cadastrais incompletos" />
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chbHistoricoEscolarIncompleto" runat="server" Text="Histórico escolar incompleto" />
                        </td>
                    </tr>
                    <tr id="trPossuiGemeo" runat="server">
                        <td colspan="3">
                            <asp:CheckBox ID="chbPossuiGemeo" runat="server" Text="Possui irmão gêmeo?" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox ID="chkPossuiInformacaoSigilosa" runat="server" Text="<%$ Resources:Academico, Aluno.Cadastro.chbPossuiInformacaoSigilosa.Text %>" Enabled="false" />
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="updResponsavel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlResponsavel" runat="server" GroupingText="Responsável">
                            <div id="MsgResponsavel" class="msgInformacao" runat="server" style="margin-top: -10px; width: 180px;"
                                visible="false">
                                É necessário o preenchimento de pelo menos um dos documentos (CPF, RG ou NIS) do
                                responsável.
                            </div>
                            <uc6:UCComboTipoResponsavelAluno ID="UCComboTipoResponsavelAluno1" runat="server"
                                Titulo="Tipo de responsável" Obrigatorio="true" OnIndexChanged="UCComboTipoResponsavel_IndexChanged"
                                ValidationGroup="Aluno" />
                            <uc7:UCAlunoResponsavel ID="UCAlunoResponsavelOutro" runat="server" Visible="false"
                                VS_TipoResponsavel="Outro" ValidationGroup="Aluno" />
                            <uc7:UCAlunoResponsavel ID="UCAlunoResponsavelMae" runat="server" VS_TipoResponsavel="Mae"
                                Obrigatorio="false" ValidationGroup="Aluno" />
                            <uc7:UCAlunoResponsavel ID="UCAlunoResponsavelPai" runat="server" VS_TipoResponsavel="Pai"
                                Obrigatorio="false" ValidationGroup="Aluno" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <fieldset>
                    <legend>Responsável pelas informações</legend>
                    <table class="table-padding">
                        <tr>
                            <td width="280px">
                                <asp:Label ID="lblDataCadastroFisico" runat="server" Text="Data de cadastro físico"
                                    AssociatedControlID="txtDataCadastroFisico" EnableViewState="False"></asp:Label>
                                <asp:TextBox ID="txtDataCadastroFisico" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                <asp:CustomValidator ID="cvDataCadastroFisico" runat="server" ControlToValidate="txtDataCadastroFisico"
                                    ValidationGroup="Aluno" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblResponsavelInfo" runat="server" Text="Responsável pela prestação das informações"
                                    AssociatedControlID="txtResponsavelInfo" EnableViewState="False"></asp:Label>
                                <asp:TextBox ID="txtResponsavelInfo" runat="server" MaxLength="100" SkinID="text60C"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblResponsavelInfoDoc" runat="server" Text="Documento do responsável"
                                    AssociatedControlID="txtResponsavelInfoDoc" EnableViewState="False"></asp:Label>
                                <asp:TextBox ID="txtResponsavelInfoDoc" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblResponsavelInfoOrgaoEmissor" runat="server" Text="Órgão emissor do documento do responsável"
                                    AssociatedControlID="txtResponsavelInfoOrgaoEmissor" EnableViewState="False"></asp:Label>
                                <asp:TextBox ID="txtResponsavelInfoOrgaoEmissor" runat="server" MaxLength="20" SkinID="text10C"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </fieldset>
        </div>
        <!-- Endereço / contato -->
        <div id="divTabs-1" class="hide">
            <br />
            <fieldset id="fdsEndereco" runat="server">
                <legend>Cadastro de endereços</legend>
                <uc8:UCEnderecos ID="UCEnderecos1" runat="server" />
            </fieldset>
            <fieldset id="fdsContato" runat="server">
                <legend>Cadastro de contatos</legend>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkPossuiEmail" runat="server" Text="Não possui e-mail" /><br />
                        <uc9:UCContato ID="UCContato1" runat="server" _VS_ValidationGroup="Aluno" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>
        <!-- Documentação -->
        <div id="divTabs-2" class="hide">
            <br />
            <asp:UpdatePanel ID="updDocumentacao" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset id="fdsDocumento" runat="server">
                        <legend>Cadastro de documentos</legend>
                        <uc10:UCGridDocumento ID="UCGridDocumento1" runat="server" _isAluno="true" />
                    </fieldset>
                    <fieldset id="fdsCertidoes" runat="server">
                        <legend>Cadastro de certidão civil</legend>
                        <uc11:UCCertidaoCivil ID="UCCertidaoCivil1" runat="server" />
                    </fieldset>
                    <fieldset id="fdsAnexo" runat="server">
                        <legend>Anexos</legend>
                        <asp:GridView ID="grvAnexos" runat="server" AutoGenerateColumns="false" DataKeyNames="alu_id, aan_id,arq_id" OnRowDataBound="grvAnexos_RowDataBound"
                            OnDataBinding="grvAnexos_DataBinding" OnRowCommand="grvAnexos_RowCommand" OnRowDeleting="grvAnexos_RowDeleting" OnRowEditing="grvAnexos_RowEditing"
                            OnRowUpdating="grvAnexos_RowUpdating" OnRowCancelingEdit="grvAnexos_RowCancelingEdit">
                            <Columns>
                                <asp:TemplateField HeaderText="Descrição">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescricaoAnexoEdicao" runat="server" Text='<%# Bind("aan_descricao") %>' CssClass="wrap690px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescricaoAnexoEdicao" runat="server" Text='<%# Bind("aan_descricao") %>' CssClass="wrap690px"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nome do arquivo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblArquivoAnexo" runat="server" Text='<%# Bind("arq_nome") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                    <ItemStyle CssClass="center" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hplDownloadAnexo" runat="server" Text="Download" ToolTip="Realizar o download do arquivo"></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="center" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEditarAnexo" runat="server" CommandName="Edit" SkinID="btEditar" ToolTip="Editar descrição do anexo" />
                                        <asp:ImageButton ID="btnSalvarAnexo" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="Salvar anexo" Visible="false" />
                                        <asp:ImageButton ID="btnCancelarAnexo" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="Cancelar edição" Visible="false" />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="center" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnAdicionarAnexo" runat="server" CommandName="Add" SkinID="btNovo" ToolTip="Adicionar anexo" />
                                        <asp:ImageButton ID="btnExcluirAnexo" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="Excluir anexo" />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="center" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!-- Movimentação -->
        <div id="divTabs-4" class="hide">
            <fieldset id="fdsMovimentacao" runat="server">
                <asp:UpdatePanel ID="updMovimentacao" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc21:UCMovimentacao ID="UCMovimentacao1" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>
        <!-- Ficha médica -->
        <div id="divTabs-6" class="hide">
            <asp:UpdatePanel ID="UpdFichaMedica" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset id="fdsFichaMedica" runat="server">
                        <div id="divBtnVacinacao" class="left" runat="server">
                            <asp:Button ID="btnExibirVacinacao" runat="server" Text="<%$ Resources:Academico, Aluno.Cadastro.fdsFichaMedica.btnExibirVacinacao.Text %>"
                                OnClick="btnExibirVacinacao_Click" CausesValidation="false" />
                        </div>
                        <asp:Label ID="lblTipoSanguineo" runat="server" Text="Tipo sanguíneo" AssociatedControlID="txtTipoSanguineo"
                            EnableViewState="False"></asp:Label>
                        <asp:TextBox ID="txtTipoSanguineo" runat="server" SkinID="text2C" MaxLength="5"></asp:TextBox>
                        <asp:Label ID="lblFatorRH" runat="server" Text="Fator rh" AssociatedControlID="txtFatorRH"
                            EnableViewState="False"></asp:Label>
                        <asp:TextBox ID="txtFatorRH" runat="server" SkinID="text2C" MaxLength="5"></asp:TextBox>
                        <asp:Label ID="lblDoencasConhecidas" runat="server" Text="Doenças conhecidas" AssociatedControlID="txtDoencasConhecidas"
                            EnableViewState="False"></asp:Label>
                        <asp:TextBox ID="txtDoencasConhecidas" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                        <asp:Label ID="lblAlergias" runat="server" Text="Alergias" AssociatedControlID="txtAlergias"
                            EnableViewState="False"></asp:Label>
                        <asp:TextBox ID="txtAlergias" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                        <asp:Label ID="lblMedicacoesPodeUtilizar" runat="server" Text="Medicamentos que pode utilizar"
                            AssociatedControlID="txtMedicacoesPodeUtilizar" EnableViewState="False"></asp:Label>
                        <asp:TextBox ID="txtMedicacoesPodeUtilizar" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                        <asp:Label ID="lblMedicacoesUsoContinuo" runat="server" Text="Medicamentos de uso contínuo"
                            AssociatedControlID="txtMedicacoesUsoContinuo" EnableViewState="False"></asp:Label>
                        <asp:TextBox ID="txtMedicacoesUsoContinuo" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                        <asp:Label ID="lblConvenioMedico" runat="server" Text="Convênio médico" AssociatedControlID="txtConvenioMedico"
                            EnableViewState="False"></asp:Label>
                        <asp:TextBox ID="txtConvenioMedico" runat="server" SkinID="text60C" MaxLength="1000"></asp:TextBox>
                        <asp:Label ID="lblHospitalRemocao" runat="server" Text="Hospital para remoção" AssociatedControlID="txtHospitalRemocao"
                            EnableViewState="False"></asp:Label>
                        <asp:TextBox ID="txtHospitalRemocao" runat="server" SkinID="text60C" MaxLength="1000"></asp:TextBox>
                        <asp:Label ID="lblOutrasRecomendacoes" runat="server" Text="Outras recomendações"
                            AssociatedControlID="txtOutrasRecomendacoes" EnableViewState="False"></asp:Label>
                        <asp:TextBox ID="txtOutrasRecomendacoes" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                    </fieldset>
                    <fieldset id="fdsCasoEmergencia" runat="server">
                        <legend>Avisar em caso de emergência</legend>
                        <uc13:UCGridContatoNomeTelefone ID="UCGridContatoNomeTelefone1" runat="server" VS_Mostra_Tipo_Responsaveis="true" />
                    </fieldset>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExibirVacinacao" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
    </div>
    <asp:HiddenField ID="txtSelectedTab" runat="server" />
    <fieldset>
        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                ValidationGroup="Aluno" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
    <!-- Cadastro de cidades -->
    <div id="divCadastroCidade" title="Cadastro de cidades" class="hide">
        <asp:UpdatePanel ID="updCadastroCidade" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc14:UCCadastroCidade ID="UCCadastroCidade1" runat="server" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- Busca de responsáveis -->
    <div id="divBuscaResponsavel" title="Busca de pessoas" class="hide">
        <asp:UpdatePanel ID="updBuscaResponsavel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc15:UCPessoasAluno ID="UCBuscaPessoasAluno1" runat="server" ContainerName="divBuscaResponsavel" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- Mensagem de confirmação da matricula -->
    <div id="divUploadAnexo" title="Anexar arquivo" class="hide">
        <asp:ValidationSummary ID="vsAnexo" runat="server" ValidationGroup="Anexo" />
        <fieldset>
            <uc1:UCCamposObrigatorios ID="UCCamposObrigatoriosAnexo" runat="server" />
            <asp:Label ID="lblDescricaoAnexo" runat="server" Text="Descrição do anexo *" AssociatedControlID="txtDescricaoAnexo"></asp:Label>
            <asp:UpdatePanel ID="updDescricaoAnexo" runat="server">
                <ContentTemplate>
                    <asp:TextBox ID="txtDescricaoAnexo" runat="server" EnableViewState="false" MaxLength="500" SkinID="text60C"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDescricaoAnexo" runat="server" ValidationGroup="Anexo" ControlToValidate="txtDescricaoAnexo" ErrorMessage="Descrição do anexo é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <asp:Label ID="lblArquivoAnexo" runat="server" Text="Arquivo *" AssociatedControlID="fupAnexo"></asp:Label>
            <asp:FileUpload ID="fupAnexo" runat="server" ToolTip="Procurar arquivo" />
            <asp:RequiredFieldValidator ID="rfvAnexo" runat="server" ValidationGroup="Anexo" ControlToValidate="fupAnexo" ErrorMessage="Arquivo de anexo é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
            <div class="right">
                <asp:Button ID="btnSalvarAnexo" runat="server" Text="Salvar" ValidationGroup="Anexo" OnClick="btnSalvarAnexo_Click" />
                <asp:Button ID="btnCancelarAnexo" runat="server" Text="Cancelar" OnClientClick="$('#divUploadAnexo').dialog('close');" CausesValidation="false" />
            </div>
        </fieldset>
    </div>

    <div id="divCartVacinacao" title="Carteira de vacinação" class="hide">
        <asp:UpdatePanel ID="UpdCartVacinacao" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Panel ID="pnlCartVacinacao" runat="server" DefaultButton="btnCancelarVacinacao">
                    <fieldset>
                        <asp:Image ID="imgCartVacinacao" runat="server" ImageAlign="Middle" />
                        <div class="right">
                            <asp:Button ID="btnCancelarVacinacao" runat="server" Text="Voltar" OnClientClick="$('#divCartVacinacao').dialog('close');" />
                        </div>
                    </fieldset>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
