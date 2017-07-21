<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCLancamentoRelatorioAtendimento.ascx.cs" Inherits="GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento" %>

<%@ Register Src="~/WebControls/Combos/UCComboRacaCor.ascx" TagName="UCCRacaCor" TagPrefix="uc" %>
<asp:UpdatePanel ID="updLancamentoRelatorio" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divInformacao">
            <asp:Label ID="lblInformacaoAluno" runat="server"></asp:Label>
            <div class="row-custom">
                <div class="m-col-custom-4 p-col-custom-12" id="divRacaCor" runat="server" visible="false">
                    <uc:UCCRacaCor ID="UCCRacaCor" runat="server" />
                </div>
                <div class="m-col-custom-4 p-col-custom-12 relatorio-manual" id="divDownloadAnexo" runat="server" visible="false">
                    <label>
                        <asp:Label ID="lblDownloadAnexo" runat="server" Text="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, lblDownloadAnexo.Text %>"></asp:Label></label>
                    <asp:HyperLink ID="hplDownloadAnexo" runat="server" SkinID="hplAnexo" ToolTip="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, hplDownloadAnexo.ToolTip %>" Width="22px"></asp:HyperLink>
                </div>
            </div>
        </div>

        <div class="clear"></div>
        <div id="divTabsRelatorio">
            <ul class="hide">
                <li runat="server" id="liHipoteseDiagnostica"><a href="#divTabs-0">
                    <asp:Literal ID="lit_22" runat="server" Text="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, lit_22.Text %>"></asp:Literal></a></li>
                <li runat="server" id="liAcoesRealizadas" visible="false"><a href="#divTabs-01">
                    <asp:Literal ID="litAcoesRealizadas" runat="server" Text="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, litAcoesRealizadas.Text %>"></asp:Literal></a></li>
                <asp:Repeater ID="rptAbaQuestionarios" runat="server">
                    <ItemTemplate>
                        <li><a href='#<%# RetornaTabID((int)Eval("qst_id"))%>'><%# Eval("qst_titulo") %></a></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div id="divTabs-0">
                <fieldset id="fdsHipoteseDiagnostica" runat="server">
                    <asp:Repeater ID="rptTipoDeficiencia" runat="server" OnItemDataBound="rptTipoDeficiencia_ItemDataBound">
                        <ItemTemplate>
                            <fieldset>
                                <legend>
                                    <asp:Literal ID="litTipoDef" runat="server" Text='<%# Eval("tde_nome") %>'></asp:Literal></legend>
                                <div class="clear"></div>
                                <asp:HiddenField ID="hdnTdeId" runat="server" Value='<%# Eval("tde_id") %>' />
                                <asp:Repeater ID="rptHipoteseDiagnostica" runat="server">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnDfdId" runat="server" Value='<%# Eval("dfd_id") %>' />
                                        <asp:CheckBox ID="chkDeficienciaDetalhe" Text='<%# Eval("dfd_nome") %>' runat="server" Checked='<%# Eval("possuiDeficienciaDetalhe") %>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </fieldset>
                        </ItemTemplate>
                    </asp:Repeater>
                </fieldset>
            </div>
            <div id="divTabs-01">
                <asp:ValidationSummary ID="vsGeral" runat="server" ValidationGroup="geral"/>
                <fieldset id="fdsAcoesRealizadas" runat="server">
                    <legend>
                        <asp:Literal runat="server" ID="litFdsAcoesRealizadas" Text="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, litAcoesRealizadas.Text %>" />
                    </legend>
                    <div style="float: right" id="divNovaAcao" runat="server">
                        <asp:Button ID="btnNovaAcao" runat="server" CausesValidation="False" Text="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, btnNovaAcao.Text %>" OnClick="btnNovaAcao_Click" Style="font-size: 0.9em;" />
                        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CausesValidation="false" OnClick="btnImprimir_Click" />
                        <br />
                        <br />
                    </div>                    
                    <asp:GridView ID="grvAcoes" runat="server" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, grvAcoes.EmptyDataText %>"
                        DataKeyNames="idTemp,rpa_id"
                        OnRowDataBound="grvAcoes_RowDataBound"
                        OnDataBound="grvAcoes_DataBound"
                        OnRowEditing="grvAcoes_RowEditing"
                        OnRowUpdating="grvAcoes_RowUpdating"
                        OnRowDeleting="grvAcoes_RowDeleting"
                        OnRowCancelingEdit="grvAcoes_RowCancelingEdit" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="25" ItemStyle-CssClass="imgColumnGrid">
                                <ItemTemplate>
                                    <asp:Image ID="imgImpressao" runat="server" ImageUrl="~/App_Themes/IntranetSME/images/imprimir.png" Width="20" Height="20" Visible='<%# Bind("rpa_impressao") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="questionario-resposta">
                                <ItemTemplate>
                                    <asp:Label ID="lblData" runat="server" Text='<%# Bind("rpa_data") %>' SkinID="textTitulo"></asp:Label>
                                    <asp:Label ID="lblAcao" runat="server" Text='<%# Bind("rpa_acao") %>' Font-Bold="false"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblData" runat="server" Text='<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, lblData.Text %>' AssociatedControlID="txtData"></asp:Label>
                                    <asp:TextBox ID="txtData" runat="server" Text='<%# Bind("rpa_data") %>' SkinID="Data" Style="width: 100px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvData" runat="server" ErrorMessage="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, rfvData.ErrorMessage %>"
                                        ControlToValidate="txtData" ValidationGroup="geral">*</asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="ctvDataFormato" runat="server" ControlToValidate="txtData"
                                        Display="Dynamic" ErrorMessage="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, ctvDataFormato.ErrorMessage %>"
                                        OnServerValidate="ValidarData_ServerValidate" Text="*" ValidationGroup="geral" />
                                    <asp:Label ID="lblAcao" runat="server" Text='<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, lblAcao.Text %>' AssociatedControlID="txtAcao"></asp:Label>
                                    <asp:TextBox ID="txtAcao" runat="server" Text='<%# Bind("rpa_acao") %>' TextMode="MultiLine" CssClass="questionario-conteudo-resposta-texto"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvAcao" runat="server" ErrorMessage="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, rfvAcao.ErrorMessage %>"
                                        ControlToValidate="txtAcao" ValidationGroup="geral">*</asp:RequiredFieldValidator>
                                    <asp:CheckBox ID="ckbImpressao" runat="server" Text="<%$ Resources:GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento, ckbImpressao.Text %>" Checked='<%# Bind("rpa_impressao") %>' CssClass="questionario-conteudo-resposta-multi-selecao" />
                                    <br />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEditar" runat="server" CommandName="Edit" SkinID="btEditar" ToolTip="<%$ Resources:Padrao, Padrao.Editar.Text %>" CausesValidation="false" />
                                    <asp:ImageButton ID="btnSalvar" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Padrao, Padrao.Salvar.Text %>" ValidationGroup="geral" Visible="false" />
                                    <asp:ImageButton ID="btnCancelarEdicao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Padrao, Padrao.Cancelar.Text %>" CausesValidation="false" Visible="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Padrao, Padrao.Excluir.Text %>" CausesValidation="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </div>
            <asp:Repeater ID="rptQuestionario" runat="server" OnItemDataBound="rptQuestionario_ItemDataBound">
                <ItemTemplate>
                    <div id='<%# RetornaTabID((int)Eval("qst_id"))%>'>
                        <fieldset>
                            <asp:HiddenField ID="hdnRaqId" runat="server" Value='<%# Eval("raq_id") %>' />
                            <asp:Repeater ID="rptConteudo" runat="server" OnItemDataBound="rptConteudo_ItemDataBound">
                                <ItemTemplate>
                                    <div class="questionario-conteudo">
                                        <asp:HiddenField ID="hdnTipo" runat="server" Value='<%# Eval("qtc_tipo") %>' />
                                        <asp:HiddenField ID="hdnTipoResposta" runat="server" Value='<%# Eval("qtc_tipoResposta") %>' />
                                        <asp:HiddenField ID="hdnQtcId" runat="server" Value='<%# Eval("qtc_id") %>' />
                                        <asp:Label ID="lblTextoConteudo" runat="server" Text='<%# Eval("qtc_texto") %>' CssClass='<%# RetornaClasseQuestionarioConteudo((byte)Eval("qtc_tipo")) %>'></asp:Label>
                                        <asp:TextBox ID="txtResposta" runat="server" CssClass="questionario-conteudo-resposta-texto" Visible="false" TextMode="MultiLine" MaxLength="4000"></asp:TextBox>
                                        <div class="clear"></div>
                                        <asp:Repeater ID="rptResposta" runat="server" OnItemDataBound="rptResposta_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="questionario-resposta">
                                                    <asp:HiddenField ID="hdnPeso" runat="server" Value='<%# Eval("qtr_peso") %>' />
                                                    <asp:HiddenField ID="hdnQtrId" runat="server" Value='<%# Eval("qtr_id") %>' />
                                                    <asp:CheckBox ID="chkResposta" runat="server" Text='<%# Eval("qtr_texto") %>'
                                                        CssClass="questionario-conteudo-resposta-multi-selecao" Visible="false" />
                                                    <asp:RadioButton ID="rdbResposta" runat="server" Text='<%# Eval("qtr_texto") %>'
                                                        CssClass="questionario-conteudo-resposta-selecao-unica" Visible="false" />
                                                    <asp:TextBox ID="txtRespostaTextoAdicional" runat="server"
                                                        CssClass="questionario-conteudo-resposta-texto-adicional" Visible="false" MaxLength="500"></asp:TextBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div class="questionario-conteudo-titulo2" id="divCalculoSoma" runat="server" visible="false">
                                <br />
                                <asp:Label ID="lblNomeCalculo" runat="server" Text='<%# Eval("qst_tituloCalculo") %>'></asp:Label>:
                                <span id="resultadoCalculo"></span>
                            </div>
                        </fieldset>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <asp:HiddenField ID="txtSelectedTab" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
