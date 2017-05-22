<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_Aluno_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDeficiencia.ascx" TagName="UCComboTipoDeficiencia" TagPrefix="uc5" %>
<%--<%@ Register Src="~/WebControls/BoletimCompletoAluno/UCBoletimCompletoAluno.ascx" TagName="UCBoletimCompletoAluno" TagPrefix="uc6" %>--%>
<%@ Register Src="~/WebControls/BoletimCompletoAluno/UCBoletimAngular.ascx" TagName="UCBoletim" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="aluno" />
    <fieldset id="fdsConsulta" runat="server">
        <legend>Consulta de alunos</legend>
        <div id="_divPesquisa" runat="server">
            <asp:CheckBox ID="chkPesquisarTodasEscolas" runat="server" Visible="false" Text="Pesquisar alunos em todas as escolas da rede"
                AutoPostBack="true" OnCheckedChanged="chkPesquisarTodasEscolas_CheckedChanged"></asp:CheckBox>
            <div>
                <asp:UpdatePanel ID="updBusca" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc2:UCComboUAEscola ID="uccUaEscola" runat="server" MostrarMessageSelecioneUA="true"
                            MostrarMessageSelecioneEscola="true" CarregarEscolaAutomatico="true" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNomeAluno" runat="server">
                <div id="_divEscolhaBusca" runat="server">
                    <asp:Label ID="_lblEscolhaBusca" runat="server" Text="Tipo de busca por nome do aluno"
                        AssociatedControlID="_rblEscolhaBusca"></asp:Label>
                    <asp:RadioButtonList ID="_rblEscolhaBusca" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Começa por" Value="2" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Contém" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Fonética" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div>
                    <asp:Label ID="_lblNome" runat="server" Text="Nome do aluno" AssociatedControlID="_txtNome"></asp:Label>
                    <asp:TextBox ID="_txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                </div>
            </div>
            <asp:Label ID="_lblDataNascimento" runat="server" Text="Data de nascimento" AssociatedControlID="_txtDataNascimento"></asp:Label>
            <asp:TextBox ID="_txtDataNascimento" runat="server" MaxLength="10" SkinID="DataSemCalendario"></asp:TextBox>
            <asp:CustomValidator ID="cvDataNascimento" runat="server" ControlToValidate="_txtDataNascimento"
                ValidationGroup="aluno" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
            <asp:Label ID="_lblMae" runat="server" Text="Nome da mãe" AssociatedControlID="_txtMae"></asp:Label>
            <asp:TextBox ID="_txtMae" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
            <asp:Label ID="_lblMatricula" runat="server" Text="<%$ Resources:Mensagens, MSG_NUMEROMATRICULA %>" AssociatedControlID="_txtMatricula"></asp:Label>
            <asp:TextBox ID="_txtMatricula" runat="server" MaxLength="50" SkinID="text30C"></asp:TextBox>
            <asp:Label ID="_lblMatrEst" runat="server" AssociatedControlID="_txtMatriculaEstadual"
                Text="Matrícula estadual" Visible="False"></asp:Label>
            <asp:TextBox ID="_txtMatriculaEstadual" runat="server" MaxLength="50" SkinID="text30C"
                Visible="False"></asp:TextBox>
            <div id="divSituacao" runat="server">
                <asp:Label ID="lblSituacao" runat="server" Text="Situação" AssociatedControlID="ddlSituacao"></asp:Label>
                <asp:DropDownList ID="ddlSituacao" runat="server" AppendDataBoundItems="True" SkinID="text30C">
                </asp:DropDownList>
            </div>
            <asp:Label ID="_lblDataCriacao" runat="server" Text="Data de cadastro" AssociatedControlID="_txtDataCriacao"></asp:Label>
            <asp:TextBox ID="_txtDataCriacao" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
            <asp:CustomValidator ID="cvData" runat="server" ControlToValidate="_txtDataCriacao"
                ValidationGroup="aluno" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
            <asp:Label ID="_lblDataAlteracao" runat="server" Text="Data da última atualização"
                AssociatedControlID="_txtDataAlteracao"></asp:Label>
            <asp:TextBox ID="_txtDataAlteracao" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="_txtDataAlteracao"
                ValidationGroup="aluno" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
            <asp:CheckBox ID="ckbApenasDeficiencia" runat="server" 
                Text="<%$ Resources:Academico, Aluno.Busca.ckbApenasDeficiencia.Text %>"
                AutoPostBack="true" OnCheckedChanged="ckbApenasDeficiencia_CheckedChanged" />
            <asp:UpdatePanel ID="upApenasDeficiencia" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc5:UCComboTipoDeficiencia ID="UCComboTipoDeficiencia" runat="server" PermiteEditar="false"
                        Visible="false" _MostrarMessageTodas="true" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ckbApenasDeficiencia" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
            <asp:CheckBox ID="chkApenasGemeos" runat="server" Text="Apenas com irmão gêmeo" Visible="False" />
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click"
                ValidationGroup="aluno" />
            <asp:Button ID="_btnLimparPesquisar" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
        </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <div id="divLegenda" runat="server" style="width: 300px;" visible="false">
            <b>Legenda:</b>
            <div style="border-style: solid; border-width: thin;">
                <table id="tbLegenda" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                    <tr runat="server" id="lnInativos">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td><asp:Literal runat="server" ID="litInativo" Text="<%$ Resources:Mensagens, MSG_ALUNO_INATIVO %>"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <br />
        <asp:GridView ID="_grvAluno" runat="server" AllowPaging="True" AllowCustomPaging="true" AutoGenerateColumns="False"
            BorderStyle="None" DataKeyNames="alu_id,permissao,alu_situacaoID" EmptyDataText="A pesquisa não encontrou resultados."
            OnRowCommand="_grvAluno_RowCommand" OnRowDataBound="_grvAluno_RowDataBound" OnDataBound="_grvAluno_DataBound"
            AllowSorting="True" OnPageIndexChanging="_grvAluno_PageIndexChanging" OnSorting="_grvAluno_Sorting" OnRowEditing="_grvAluno_RowEditing" SkinID="GridResponsive">
            <Columns>
                <asp:TemplateField HeaderText="Mais info.">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnExpandir" runat="server" ToolTip="Informações sobre o aluno"
                            CssClass="ui-icon ui-icon-circle-triangle-s" OnClientClick="ExpandCollapse('.trExpandir', this); return false;" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" CssClass="grid-responsive-item-inline grid-responsive-no-header"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nome do aluno" SortExpression="pes_nome">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap200px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap200px"></asp:Label>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" PostBackUrl="~/Academico/Aluno/Cadastro.aspx"
                            Text='<%# Bind("pes_nome") %>' CssClass="wrap200px"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" CssClass="grid-responsive-item-inline grid-responsive-no-header"/>
                </asp:TemplateField>
                <asp:BoundField DataField="pes_dataNascimento" HeaderText="Data de nascimento" DataFormatString="{0:dd/MM/yyy}"
                    SortExpression="pes_dataNascimento">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Nome da mãe" SortExpression="pes_nomeMae">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("pes_nomeMae") %>' CssClass="wrap150px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="alc_matricula" HeaderText="<%$ Resources:Mensagens,MSG_NUMEROMATRICULA %>" Visible="False"
                    SortExpression="alc_matricula" />
                <asp:BoundField DataField="alc_matriculaEstadual" HeaderText="Matrícula estadual"
                    Visible="false" SortExpression="alc_matriculaEstadual" />
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
                            PostBackUrl="~/Academico/Aluno/Anotacoes.aspx" CommandName="Edit" ToolTip="Anotações de aula do aluno" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Inserir/Alterar foto" ItemStyle-VerticalAlign="Middle">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnCapturarFoto" runat="server" CommandName="Edit" CausesValidation="False"
                            PostBackUrl="~/Academico/Aluno/CapturaFoto/Default.aspx" SkinID="btWebCam" ImageAlign="Middle" />
                        <asp:Image ID="_imgPossuiImagem" runat="server" SkinID="imgConfirmar" ToolTip="Aluno possui imagem cadastrada"
                            ImageAlign="Middle" Visible="false" />
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
                            ToolTip="Boletim do aluno" SkinID="btRelatorio" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Relatório pedagógico">
                    <EditItemTemplate>
                        <asp:TextBox ID="textBoxRelPedagogico" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnRelatórioPedagogico" runat="server" CausesValidation="False" CommandName="RelatorioPedagogico"
                            ToolTip="Relatório pedagógico do aluno" SkinID="btFormulario" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <tr class="trExpandir" style="display: none;">
                            <td align="left" colspan="100%">
                                <div class="divExpandeLinha">
                                    <asp:Label ID="lblEscola" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("cur_nome") %>'></asp:Label>
                                    <asp:Label ID="lblPeriodo" runat="server" Text='<%# Bind("crp_descricao") %>'></asp:Label><br />
                                    <asp:Label ID="lblTurma" runat="server" Text='<%# Bind("tur_codigo") %>'></asp:Label>
                                    <asp:Label ID="lblAvaliacao" runat="server" Text='<%# Bind("crp_nomeAvaliacao") %>'></asp:Label>
                                    <asp:Label ID="lblNChamada" runat="server" Text='<%# Bind("mtu_numeroChamada") %>'></asp:Label><br />
                                </div>
                                <div class="clear">
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <HeaderStyle CssClass="hide" />
                    <ItemStyle CssClass="hide" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvAluno" />
    </fieldset>
    <!-- Boletim completo do aluno -->
    <div id="divBoletimCompleto" title="Boletim completo do aluno" class="hide">
        <asp:UpdatePanel ID="updBoletimCompleto" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    
                    <uc:UCBoletim ID="ucBoletim" runat="server" />
                   
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
