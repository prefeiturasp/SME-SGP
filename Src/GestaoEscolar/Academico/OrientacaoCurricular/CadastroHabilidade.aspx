<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CadastroHabilidade.aspx.cs" Inherits="GestaoEscolar.Academico.OrientacaoCurricular.CadastroHabilidade" %>

<%@ PreviousPageType VirtualPath="~/Academico/OrientacaoCurricular/Busca.aspx" %>

<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Mensagens/UCConfirmacaoOperacao.ascx" TagName="UCConfirmacaoOperacao"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional" EnableViewState="false">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup='<%=validationGroup %>'/>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlCabecalho" runat="server" GroupingText="Cadastro de orientações curriculares">
        <asp:Label ID="lblCabecalho" runat="server"></asp:Label>
    </asp:Panel>
    <asp:UpdatePanel ID="updOrientacaoCurricular" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlResuldados" runat="server" GroupingText="Resultados">
                <div>
                    <asp:Label ID="lblOrientacaoPai" runat="server"></asp:Label>
                </div><br />
                <div class="clear"></div>
                <asp:Button runat="server" ID="btnNovo" Text="Incluir nova orientação curricular" OnClick="btnNovo_Click" />  
                <div class="divOrientacaoCurricular">
                    <asp:GridView ID="grvOrientacaoCurricular" runat="server" AutoGenerateColumns="false" DataKeyNames="entOrientacao,nvl_ordem" 
                        OnRowDataBound="grvOrientacaoCurricular_RowDataBound" OnRowEditing="grvOrientacaoCurricular_RowEditing" 
                        OnDataBinding="grvOrientacaoCurricular_DataBinding" OnRowCancelingEdit="grvOrientacaoCurricular_RowCancelingEdit" 
                        OnRowDeleting="grvOrientacaoCurricular_RowDeleting" OnRowUpdating="grvOrientacaoCurricular_RowUpdating" 
                        OnRowCommand="grvOrientacaoCurricular_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Código">
                                <ItemTemplate>
                                    <asp:Label ID="lblCodigo" runat="server" Text='<%# Bind("entOrientacao.ocr_codigo") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCodigo" runat="server" Text='<%# Bind("entOrientacao.ocr_codigo") %>' MaxLength="20"
                                        SkinID="text20C"></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="200px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descrição *">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("entOrientacao.ocr_descricao") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescricao" runat="server" Text='<%# Bind("entOrientacao.ocr_descricao") %>'
                                        MaxLength="200" SkinID="limite4000" TextMode="MultiLine" CssClass="wrap250px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDescricao" runat="server" ErrorMessage="Descrição é obrigatório."
                                        ControlToValidate="txtDescricao" ValidationGroup='<%=validationGroup %>'>*</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <HeaderStyle Width="800px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Níveis de aprendizado">
                                <ItemTemplate>
                                    <table border="0">
                                        <tr>
                                            <asp:Repeater ID="rptNivelAprendizado" runat="server">
                                                <ItemTemplate>
                                                    <td style="text-align: center; vertical-align: middle;">
                                                        <asp:Label ID="lblSiglaNivel" runat="server" Text='<%# Bind("nap_sigla") %>' AssociatedControlID="chkNivel"></asp:Label>
                                                        <asp:CheckBox ID="chkNivel" runat="server" Text="" Enabled="false" />
                                                        <asp:HiddenField ID="hdnIdNivel" runat="server" Value='<%# Bind("nap_id") %>' />
                                                    </td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Replicar" HeaderStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgReplicar" runat="server" CommandName="Replicar" SkinID="btFormulario"
                                        ToolTip="Replicar orientação curricular" CausesValidation="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                        ToolTip="Editar orientação curricular" CausesValidation="false" />
                                    <asp:ImageButton ID="imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                        ToolTip="Cancelar edição" CausesValidation="false" Visible="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                        ToolTip="Salvar orientação curricular" ValidationGroup='<%=validationGroup %>' Visible="false" />
                                    <asp:ImageButton ID="imgCancelarOrientacao" runat="server" CommandName="Cancel"
                                        SkinID="btCancelar" ToolTip="Cancelar nova orientação curricular" CausesValidation="false"
                                        Visible="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detalhar orientação curricular" HeaderStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEditarFilhos" runat="server" CommandName="EditFilhos" SkinID="btDetalhar" CausesValidation="false" 
                                        ToolTip="Detalhar orientação curricular"/>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, OrientacaoCurricular.CadastroHabilidade.btnRelacionarHabilidades.ToolTip %>" HeaderStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnRelacionarHabilidades" runat="server" CommandName="RelacionarHabilidades" SkinID="btDetalhar" CausesValidation="false" 
                                        ToolTip="<%$ Resources:Academico, OrientacaoCurricular.CadastroHabilidade.btnRelacionarHabilidades.ToolTip %>" />
                                    <asp:Image ID="imgRelacionarHabilidadesSituacao" runat="server" SkinID="imgConfirmar" ToolTip="<%$ Resources:Academico, OrientacaoCurricular.CadastroHabilidade.imgRelacionarHabilidadesSituacao.ToolTip %>"
                                        Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                        ToolTip="Excluir orientação curricular" CausesValidation="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="hdnRowRelacionamento" runat="server" />        
                <div id="divLegenda" runat="server" style="width: 300px; padding: 6px;" visible="false">
                    <b>Legenda:</b>
                    <div style="border-style: solid; border-width: thin;">
                        <table id="tbLegenda" style="border-collapse: separate !important; border-spacing: 2px !important;">                    
                            <asp:Repeater ID="rptLegenda" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:Label ID="lblLegenda" runat="server" Text='<%# Bind("nivelAprendizado") %>'></asp:Label></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>

                <div class="right">
                    <asp:Button ID="btnVisualizar" runat="server" Text="Visualizar" OnClick="btnVisualizar_Click" />
                    <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Pop up com exibição de treeview de orientações curriculares -->
    <div id="divOrientacoes" title="Orientações curriculares" class="hide">
        <asp:UpdatePanel ID="updOrientacoes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <div class="divTreeviewScrollCOC">
                        <asp:Repeater ID="rptOrientacoes" runat="server" OnItemDataBound="rptOrientacoes_ItemDataBound">
                            <ItemTemplate>
                                <asp:Literal ID="litCabecalho" runat="server"></asp:Literal>
                                <asp:Literal ID="litConteudo" runat="server"></asp:Literal>
                                <asp:Literal ID="litRodape" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divReplicar" title="Replicar Orientação Curricular" class="hide">
        <asp:UpdatePanel ID="updReplicar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:ValidationSummary ID="vsReplicarOrientacao" runat="server" ValidationGroup="ReplicarOrientacao" />
                <asp:Label ID="lblMensagemReplicar" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblInfoReplicar" runat="server"></asp:Label>
                <br />
                <uc4:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" runat="server"
                    ValidationGroup="ReplicarOrientacao" Obrigatorio="true" />
                <uc1:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo1" runat="server"
                    ValidationGroup="ReplicarOrientacao" Obrigatorio="true" />
                <div class="right">
                    <asp:Button ID="btnReplicar" runat="server" Text="Replicar" OnClick="btnReplicar_Click" 
                        ValidationGroup="ReplicarOrientacao" />
                    <asp:Button ID="btnCancelarReplicacao" runat="server" Text="Cancelar" OnClientClick="$('#divReplicar').dialog('close');" 
                        CausesValidation="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- Pop up com as habilidades relacionadas -->
    <div id="divRelacionarHabilidades" title="Relacionamento de habilidades entre matrizes" class="hide">
        <asp:UpdatePanel ID="updRelacionarHabilidades" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblMensagemRelacionarHabilidade" runat="server" Visible="false"></asp:Label>
                <asp:Panel ID="pnlHabilidadeSelecionada" runat="server" GroupingText="<%$ Resources:Academico, OrientacaoCurricular.CadastroHabilidade.pnlHabilidadeSelecionada.GroupingText %>">
                    <asp:Label ID="lblHabilidadeSelecionada" runat="server"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="pnlMatrizCurricular" runat="server" GroupingText="<%$ Resources:Academico, OrientacaoCurricular.CadastroHabilidade.pnlMatrizCurricular.GroupingText %>">
                    <div>
                        <asp:DropDownList ID="ddlMatrizCurricular" runat="server" OnSelectedIndexChanged="ddlMatrizCurricular_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </asp:Panel> 
                <asp:Panel ID="pnlHabilidadesRelacionadas" runat="server" GroupingText="<%$ Resources:Academico, OrientacaoCurricular.CadastroHabilidade.pnlHabilidadesRelacionadas.GroupingText %>">
                    <div>
                        <div class="divTreeviewScrollCOC" style="z-index:0;margin-bottom:1px;">
                            <asp:Repeater ID="rptMatrizRelacionada" runat="server" OnItemDataBound="rptMatrizRelacionada_ItemDataBound">
                                <ItemTemplate>
                                    <asp:Literal ID="litCabecalho" runat="server"></asp:Literal>
                                    <div style="display: table-row;" id="divHabilidade" runat="server">
                                        <asp:HiddenField ID="hdnChave" runat="server" Value='<%# Eval("ocr_id") %>' />
                                        <asp:HiddenField ID="hdnRelacionada" runat="server" Value='<%# Eval("Relacionada") %>' />
                                        <span style="display: table-cell; text-align: left; vertical-align: top;">
                                            <asp:Literal ID="lblHabilidade" runat="server"></asp:Literal>
                                        </span>
                                        <span style="display: table-cell; width: 40px; text-align: center; vertical-align: top; vertical-align: middle;">
                                            <asp:CheckBox ID="chkRelacionada" runat="server" Visible='<%# Convert.ToBoolean(Eval("sheet")) %>' Checked='<%# Convert.ToBoolean(Eval("Relacionada")) %>'></asp:CheckBox><br />
                                        </span>
                                    </div>
                                    <asp:Literal ID="litRodape" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </asp:Panel>
                <div class="right">
                    <asp:Button ID="btnRelacionarHabilidades" runat="server" Text="<%$ Resources:Academico, OrientacaoCurricular.CadastroHabilidade.btnRelacionarHabilidades.Text %>" OnClick="btnRelacionarHabilidades_Click" />
                </div>
                <div id="divConfirmRelacionamento" title="Confirmação"><asp:Literal ID="litConfirmRelacionamento" runat="server" Text="<%$ Resources:Academico, OrientacaoCurricular.CadastroHabilidade.litConfirmRelacionamento.Text %>"></asp:Literal></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- Dialog confirmação padrão -->
    <uc3:UCConfirmacaoOperacao ID="UCConfirmacaoOperacao1" runat="server" />
</asp:Content>
