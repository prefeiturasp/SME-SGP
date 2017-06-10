<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCurriculo.ascx.cs" Inherits="GestaoEscolar.WebControls.Curriculo.UCCurriculo" %>

<%@ Register Src="~/WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoModalidadeEnsino.ascx" TagName="UCComboTipoModalidadeEnsino" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoCurriculoPeriodo.ascx" TagName="UCComboTipoCurriculoPeriodo" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Curriculo/UCListaSugestoes.ascx" TagName="UCListaSugestoes" TagPrefix="uc4" %>

<asp:UpdatePanel runat="server" ID="updMessage" UpdateMode="Always">
    <ContentTemplate>
        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel runat="server" ID="updCadastro" UpdateMode="Always">
    <ContentTemplate>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="geral" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="disciplina" />
        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="eixo" />
        <fieldset class="fieldset-curriculo">
            <legend><asp:Literal runat="server" ID="litLegend" Text="" /></legend>
            <asp:Label ID="lblMsgEvento" runat="server"></asp:Label>
            <uc1:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino1" runat="server" Obrigatorio="true" MostrarMessageSelecione="true" TrazerComboCarregado="true" />
            <uc2:UCComboTipoModalidadeEnsino ID="UCComboTipoModalidadeEnsino1" runat="server" Obrigatorio="true" MostrarMessageSelecione="true" TrazerComboCarregado="true" />           
            
            <asp:Panel runat="server" ID="pnlCurriculo" style="margin-top:20px;" Visible="false" >
                <!-- Introdução -->
                <fieldset>
                    <legend>
                        <asp:Literal runat="server" ID="litIntroducao" Text="<%$ Resources:Academico, Curriculo.Cadastro.litIntroducao.Text %>" />
                        <asp:Button ID="btnNovoGeral" runat="server" CausesValidation="False" Text="<%$ Resources:Academico, Curriculo.Cadastro.btnNovoGeral.Text %>" OnClick="btnNovoGeral_Click"/>
                    </legend>               
                    <asp:GridView ID="grvGeral" runat="server" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.EmptyDataText %>"
                        DataKeyNames="crc_id,crc_ordem,tds_id"
                        OnRowDataBound="grvGeral_RowDataBound"
                        OnRowCommand="grvGeral_RowCommand"
                        OnDataBound="grvGeral_DataBound"
                        OnRowEditing="grvGeral_RowEditing" 
                        OnRowUpdating="grvGeral_RowUpdating" 
                        OnRowDeleting="grvGeral_RowDeleting"
                        OnRowCancelingEdit="grvGeral_RowCancelingEdit" ShowHeader="false" SkinID="GridTopico">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaTopico %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblTitulo" runat="server" Text='<%# Bind("crc_titulo") %>' SkinID="textTitulo"></asp:Label>
                                    <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("crc_descricao") %>' Font-Bold="false"></asp:Label>
                                    <asp:HiddenField ID="hdnAbertoSugestao" runat="server" Value="0" />
                                    <uc4:UCListaSugestoes ID="UCListaSugestoes1" runat="server"></uc4:UCListaSugestoes>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Panel ID="pnlItem" runat="server">
                                        <asp:Label ID="lblTitulo" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblTitulo.Text %>' AssociatedControlID="txtTitulo"></asp:Label>
                                        <asp:TextBox ID="txtTitulo" runat="server" Text='<%# Bind("crc_titulo") %>' MaxLength="200" SkinID="text60C"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ErrorMessage="<%$ Resources:Academico, Curriculo.Cadastro.rfvTitulo.ErrorMessage %>"
                                            ControlToValidate="txtTitulo" ValidationGroup="geral">*</asp:RequiredFieldValidator>
                                        <asp:Label ID="lblDescricao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblDescricao.Text %>' AssociatedControlID="txtDescricao"></asp:Label>
                                        <asp:TextBox ID="txtDescricao" runat="server" Text='<%# Bind("crc_descricao") %>' MaxLength="4000" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                                        <asp:HiddenField ID="hdnAbertoSugestao" runat="server" Value="0" />
                                        <uc4:UCListaSugestoes ID="UCListaSugestoes1" runat="server"></uc4:UCListaSugestoes>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlSugestao" runat="server">
                                        <asp:Label ID="lblTituloSugestao" runat="server" Text='<%# Bind("crc_titulo") %>' Font-Bold="true"></asp:Label>
                                        <br /><br />
                                        <asp:Label ID="lblDescricaoSugestao" runat="server" Text='<%# Bind("crc_descricao") %>' Font-Bold="false"></asp:Label>
                                        <br /><br />
                                        <asp:Label ID="lblSugestao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblSugestao.Text %>' AssociatedControlID="txtSugestao"></asp:Label>
                                        <asp:TextBox ID="txtSugestao" runat="server" MaxLength="400" TextMode="MultiLine" SkinID="limite400"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvSugestao" runat="server" ErrorMessage="<%$ Resources:Academico, Curriculo.Cadastro.rfvSugestao.ErrorMessage %>"
                                            ControlToValidate="txtSugestao" ValidationGroup="geral">*</asp:RequiredFieldValidator>
                                        <asp:Label ID="lblTipoSugestao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblTipoSugestao.Text %>' AssociatedControlID="ddlTipoSugestao"></asp:Label>
                                        <asp:DropDownList ID="ddlTipoSugestao" runat="server">
                                            <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Sugestao %>' Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Exclusao %>' Value="2" Selected="False"></asp:ListItem>
                                            <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Inclusao %>' Value="3" Selected="False"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnCrsId" runat="server" Value="-1" />
                                    </asp:Panel>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaSugestão %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnListaSugestoes" runat="server" CausesValidation="false" SkinID="btSugerir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnListaSugestoes.ToolTip %>" OnClientClick="ListarSugestoes($(this)); return false;" />                                
                                    <asp:ImageButton ID="btnIncluirSugestao" runat="server" CausesValidation="false" CommandName="Edit" SkinID="btSugerir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnIncluirSugestao.ToolTip %>" />
                                    <asp:ImageButton ID="btnSalvarSugestao" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="geral" Visible="false" />
                                    <asp:ImageButton ID="btnCancelarSugestao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnCancelarEdicao.ToolTip %>" CausesValidation="false" Visible="false" />
                                    <asp:ImageButton ID="btnExcluirSugestao" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnExcluirSugestao.ToolTip %>" CausesValidation="false" Visible="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaOrdem %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                <ItemTemplate>                                
                                    <asp:ImageButton ID="btnSubir" runat="server" CausesValidation="false" CommandName="Subir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSubir.ToolTip %>" Height="16" Width="16" />
                                    <asp:ImageButton ID="btnDescer" runat="server" CausesValidation="false" CommandName="Descer" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnDescer.ToolTip %>" Height="16" Width="16" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaEditar %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEditar" runat="server" CommandName="Edit" SkinID="btEditar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnEditar.ToolTip %>" CausesValidation="false" />
                                    <asp:ImageButton ID="btnSalvar" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="geral" Visible="false" />
                                    <asp:ImageButton ID="btnCancelarEdicao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnCancelarEdicao.ToolTip %>" CausesValidation="false" Visible="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaExcluir %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnExcluir.ToolTip %>" CausesValidation="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView> 
                </fieldset>

                <!-- Disciplinas -->
                <div class="selecao-disciplina">
                    <asp:RadioButtonList runat="server" ID="rblDisciplina" RepeatDirection="Vertical" DataValueField="tds_id" DataTextField="tds_nome" OnSelectedIndexChanged="rblDisciplina_SelectedIndexChanged" AutoPostBack="true"></asp:RadioButtonList>
                </div>                
                <br />
                <fieldset runat="server" id="fsDisciplina" visible="false">
                    <legend>
                        <asp:Literal runat="server" ID="litDisciplina" Text="" />
                        <asp:Button ID="btnNovoDisciplina" runat="server" CausesValidation="False" Text="<%$ Resources:Academico, Curriculo.Cadastro.btnNovoDisciplina.Text %>" OnClick="btnNovoDisciplina_Click"/>
                    </legend>              
                    <asp:GridView ID="grvDisciplina" runat="server" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:Academico, Curriculo.Cadastro.grvDisciplina.EmptyDataText %>"
                            DataKeyNames="crc_id,crc_ordem,tds_id"
                            OnRowDataBound="grvGeral_RowDataBound"
                            OnRowCommand="grvGeral_RowCommand"
                            OnDataBound="grvGeral_DataBound"
                            OnRowEditing="grvGeral_RowEditing" 
                            OnRowUpdating="grvGeral_RowUpdating" 
                            OnRowDeleting="grvGeral_RowDeleting"
                            OnRowCancelingEdit="grvGeral_RowCancelingEdit" ShowHeader="false" SkinID="GridTopico">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvDisciplina.ColunaTopico %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTitulo" runat="server" Text='<%# Bind("crc_titulo") %>' CssClass="text-titulo"></asp:Label>
                                        <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("crc_descricao") %>' Font-Bold="false"></asp:Label>
                                        <asp:HiddenField ID="hdnAbertoSugestao" runat="server" Value="0" />
                                        <uc4:UCListaSugestoes ID="UCListaSugestoes1" runat="server"></uc4:UCListaSugestoes>
                                    </ItemTemplate>
                                    <EditItemTemplate>
		                                <asp:Panel ID="pnlItem" runat="server">
			                                <asp:Label ID="lblTitulo" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblTitulo.Text %>' AssociatedControlID="txtTitulo"></asp:Label>
			                                <asp:TextBox ID="txtTitulo" runat="server" Text='<%# Bind("crc_titulo") %>' MaxLength="200" SkinID="text60C"></asp:TextBox>
			                                <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ErrorMessage="<%$ Resources:Academico, Curriculo.Cadastro.rfvTitulo.ErrorMessage %>"
				                                ControlToValidate="txtTitulo" ValidationGroup="disciplina">*</asp:RequiredFieldValidator>
			                                <asp:Label ID="lblDescricao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblDescricao.Text %>' AssociatedControlID="txtDescricao"></asp:Label>
			                                <asp:TextBox ID="txtDescricao" runat="server" Text='<%# Bind("crc_descricao") %>' MaxLength="4000" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
		                                    <asp:HiddenField ID="hdnAbertoSugestao" runat="server" Value="0" />
                                            <uc4:UCListaSugestoes ID="UCListaSugestoes1" runat="server"></uc4:UCListaSugestoes>
                                        </asp:Panel>
		                                <asp:Panel ID="pnlSugestao" runat="server">
			                                <asp:Label ID="lblTituloSugestao" runat="server" Text='<%# Bind("crc_titulo") %>' Font-Bold="true"></asp:Label>
			                                <br /><br />
			                                <asp:Label ID="lblDescricaoSugestao" runat="server" Text='<%# Bind("crc_descricao") %>' Font-Bold="false"></asp:Label>
			                                <br /><br />
			                                <asp:Label ID="lblSugestao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblSugestao.Text %>' AssociatedControlID="txtSugestao"></asp:Label>
			                                <asp:TextBox ID="txtSugestao" runat="server" MaxLength="400" TextMode="MultiLine" SkinID="limite400"></asp:TextBox>
			                                <asp:RequiredFieldValidator ID="rfvSugestao" runat="server" ErrorMessage="<%$ Resources:Academico, Curriculo.Cadastro.rfvSugestao.ErrorMessage %>"
				                                ControlToValidate="txtSugestao" ValidationGroup="disciplina">*</asp:RequiredFieldValidator>
			                                <asp:Label ID="lblTipoSugestao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblTipoSugestao.Text %>' AssociatedControlID="ddlTipoSugestao"></asp:Label>
                                            <asp:DropDownList ID="ddlTipoSugestao" runat="server">
                                                <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Sugestao %>' Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Exclusao %>' Value="2" Selected="False"></asp:ListItem>
                                                <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Inclusao %>' Value="3" Selected="False"></asp:ListItem>
                                            </asp:DropDownList>
			                                <asp:HiddenField ID="hdnCrsId" runat="server" Value="-1" />
		                                </asp:Panel>
	                                </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaSugestão %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                    <ItemTemplate>                                
		                                <asp:ImageButton ID="btnListaSugestoes" runat="server" CausesValidation="false" SkinID="btSugerir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnListaSugestoes.ToolTip %>" OnClientClick="ListarSugestoes($(this)); return false;" /> 
                                        <asp:ImageButton ID="btnIncluirSugestao" runat="server" CausesValidation="false" CommandName="Edit" SkinID="btSugerir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnIncluirSugestao.ToolTip %>" />
		                                <asp:ImageButton ID="btnSalvarSugestao" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="disciplina" Visible="false" />
		                                <asp:ImageButton ID="btnCancelarSugestao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnCancelarEdicao.ToolTip %>" CausesValidation="false" Visible="false" />
		                                <asp:ImageButton ID="btnExcluirSugestao" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnExcluirSugestao.ToolTip %>" CausesValidation="false" Visible="false" />
	                                </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaOrdem %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                    <ItemTemplate>                                
                                        <asp:ImageButton ID="btnSubir" runat="server" CausesValidation="false" CommandName="Subir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSubir.ToolTip %>" Height="16" Width="16" />
                                        <asp:ImageButton ID="btnDescer" runat="server" CausesValidation="false" CommandName="Descer" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnDescer.ToolTip %>" Height="16" Width="16" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaEditar %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEditar" runat="server" CommandName="Edit" SkinID="btEditar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnEditar.ToolTip %>" CausesValidation="false" />
                                        <asp:ImageButton ID="btnSalvar" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="disciplina" Visible="false" />
                                        <asp:ImageButton ID="btnCancelarEdicao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnCancelarEdicao.ToolTip %>" CausesValidation="false" Visible="false" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaExcluir %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnExcluir.ToolTip %>" CausesValidation="false" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    <!-- Conteúdos e habilidades -->
                    <fieldset>
                        <legend><asp:Literal runat="server" ID="litHabilidades" Text="<%$ Resources:Academico, Curriculo.Cadastro.litHabilidades.Text %>" /></legend>
                        <uc3:UCComboTipoCurriculoPeriodo ID="UCComboTipoCurriculoPeriodo1" runat="server" Obrigatorio="false" MostrarMessageSelecione="true" TrazerComboCarregado="true" />
                        <div class="painel-habilidades">
                            <asp:Panel runat="server" ID="pnlHabilidades" style="margin-top:20px;" Visible="false" >
                            
                            <!-- Eixo -->
                            <asp:Button ID="btnNovoEixo" runat="server" CausesValidation="False" Text="<%$ Resources:Academico, Curriculo.Cadastro.btnNovoEixo.Text %>" OnClick="btnNovoEixo_Click" />
                            <asp:GridView ID="grvEixo" runat="server" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:Academico, Curriculo.Cadastro.grvEixo.EmptyDataText %>"
                                DataKeyNames="cro_id,cro_ordem,cro_tipo,cro_idPai"
                                OnRowDataBound="grvEixo_RowDataBound"
                                OnRowCommand="grvEixo_RowCommand"
                                OnDataBound="grvEixo_DataBound"
                                OnRowEditing="grvEixo_RowEditing" 
                                OnRowUpdating="grvEixo_RowUpdating" 
                                OnRowDeleting="grvEixo_RowDeleting"
                                OnRowCancelingEdit="grvEixo_RowCancelingEdit" ShowHeader="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvEixo.ColunaEixo %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("cro_descricao") %>' Font-Bold="false"></asp:Label>
                                            <asp:HiddenField ID="hdnAbertoSugestao" runat="server" Value="0" />
                                            <uc4:UCListaSugestoes ID="UCListaSugestoes1" runat="server"></uc4:UCListaSugestoes>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Panel ID="pnlItem" runat="server">
                                                <asp:Label ID="lblDescricao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.grvEixo.lblDescricao.Text %>' AssociatedControlID="txtDescricao"></asp:Label>
                                                <asp:TextBox ID="txtDescricao" runat="server" Text='<%# Bind("cro_descricao") %>' MaxLength="500" SkinID="text60C"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDescricao" runat="server" ErrorMessage="<%$ Resources:Academico, Curriculo.Cadastro.grvEixo.rfvDescricao.ErrorMessage %>"
                                                    ControlToValidate="txtDescricao" ValidationGroup="eixo">*</asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hdnAbertoSugestao" runat="server" Value="0" />
                                                <uc4:UCListaSugestoes ID="UCListaSugestoes1" runat="server"></uc4:UCListaSugestoes>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlSugestao" runat="server">
                                                <asp:Label ID="lblDescricaoSugestao" runat="server" Text='<%# Bind("cro_descricao") %>' Font-Bold="false"></asp:Label>
                                                <br /><br />
                                                <asp:Label ID="lblSugestao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblSugestao.Text %>' AssociatedControlID="txtSugestao"></asp:Label>
			                                    <asp:TextBox ID="txtSugestao" runat="server" MaxLength="400" TextMode="MultiLine" SkinID="limite400"></asp:TextBox>
			                                    <asp:RequiredFieldValidator ID="rfvSugestao" runat="server" ErrorMessage="<%$ Resources:Academico, Curriculo.Cadastro.rfvSugestao.ErrorMessage %>"
				                                    ControlToValidate="txtSugestao" ValidationGroup="eixo">*</asp:RequiredFieldValidator>
			                                    <asp:Label ID="lblTipoSugestao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblTipoSugestao.Text %>' AssociatedControlID="ddlTipoSugestao"></asp:Label>
			                                    <asp:DropDownList ID="ddlTipoSugestao" runat="server">
				                                    <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Sugestao %>' Value="1" Selected="True"></asp:ListItem>
				                                    <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Exclusao %>' Value="2" Selected="False"></asp:ListItem>
				                                    <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Inclusao %>' Value="3" Selected="False"></asp:ListItem>
			                                    </asp:DropDownList>
			                                    <asp:HiddenField ID="hdnCrsId" runat="server" Value="-1" />
                                            </asp:Panel>
                                        </EditItemTemplate>
                                        <ItemStyle CssClass="accordion-head" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaSugestão %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
	                                    <ItemTemplate>                                
		                                    <asp:ImageButton ID="btnListaSugestoes" runat="server" CausesValidation="false" SkinID="btSugerir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnListaSugestoes.ToolTip %>" OnClientClick="ListarSugestoes($(this)); return false;" /> 
                                            <asp:ImageButton ID="btnIncluirSugestao" runat="server" CausesValidation="false" CommandName="Edit" SkinID="btSugerir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnIncluirSugestao.ToolTip %>" />
		                                    <asp:ImageButton ID="btnSalvarSugestao" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="eixo" Visible="false" />
		                                    <asp:ImageButton ID="btnCancelarSugestao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnCancelarEdicao.ToolTip %>" CausesValidation="false" Visible="false" />
		                                    <asp:ImageButton ID="btnExcluirSugestao" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnExcluirSugestao.ToolTip %>" CausesValidation="false" Visible="false" />
	                                    </ItemTemplate>
	                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaOrdem %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                        <ItemTemplate>                                
                                            <asp:ImageButton ID="btnSubir" runat="server" CausesValidation="false" CommandName="Subir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSubir.ToolTip %>" Height="16" Width="16" />
                                            <asp:ImageButton ID="btnDescer" runat="server" CausesValidation="false" CommandName="Descer" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnDescer.ToolTip %>" Height="16" Width="16" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaEditar %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEditar" runat="server" CommandName="Edit" SkinID="btEditar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnEditar.ToolTip %>" CausesValidation="false" />
                                            <asp:ImageButton ID="btnSalvar" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="eixo" Visible="false" />
                                            <asp:ImageButton ID="btnCancelarEdicao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnCancelarEdicao.ToolTip %>" CausesValidation="false" Visible="false" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaExcluir %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnExcluir.ToolTip %>" CausesValidation="false" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            
                                            <!-- Objetivo -->
                                            <div class="accordion-body">
                                                <asp:HiddenField ID="hdnAberto" runat="server" Value="0" />
                                                <asp:Button ID="btnNovoObjetivo" runat="server" CausesValidation="False" Text="<%$ Resources:Academico, Curriculo.Cadastro.btnNovoObjetivo.Text %>" OnClick="btnNovoObjetivo_Click" />
                                                <asp:GridView ID="grvObjetivo" runat="server" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:Academico, Curriculo.Cadastro.grvObjetivo.EmptyDataText %>"
                                                    DataKeyNames="cro_id,cro_ordem,cro_tipo,cro_idPai"
                                                    OnRowDataBound="grvEixo_RowDataBound"
                                                    OnRowCommand="grvEixo_RowCommand"
                                                    OnDataBound="grvEixo_DataBound"
                                                    OnRowEditing="grvEixo_RowEditing" 
                                                    OnRowUpdating="grvEixo_RowUpdating" 
                                                    OnRowDeleting="grvEixo_RowDeleting"
                                                    OnRowCancelingEdit="grvEixo_RowCancelingEdit" ShowHeader="false" SkinID="GridItensLeft">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvObjetivo.ColunaObjetivo %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("cro_descricao") %>' Font-Bold="false"></asp:Label>
                                                                <asp:HiddenField ID="hdnAbertoSugestao" runat="server" Value="0" />
                                                                <uc4:UCListaSugestoes ID="UCListaSugestoes1" runat="server"></uc4:UCListaSugestoes>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:Panel ID="pnlItem" runat="server">
                                                                    <asp:Label ID="lblDescricao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.grvEixo.lblDescricao.Text %>' AssociatedControlID="txtDescricao"></asp:Label>
                                                                    <asp:TextBox ID="txtDescricao" runat="server" Text='<%# Bind("cro_descricao") %>' MaxLength="500" SkinID="text60C"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvDescricao" runat="server" ErrorMessage="<%$ Resources:Academico, Curriculo.Cadastro.grvObjetivo.rfvDescricao.ErrorMessage %>"
                                                                        ControlToValidate="txtDescricao" ValidationGroup="eixo">*</asp:RequiredFieldValidator>
                                                                    <asp:HiddenField ID="hdnAbertoSugestao" runat="server" Value="0" />
                                                                    <uc4:UCListaSugestoes ID="UCListaSugestoes1" runat="server"></uc4:UCListaSugestoes>
                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlSugestao" runat="server">
			                                                        <asp:Label ID="lblDescricaoSugestao" runat="server" Text='<%# Bind("cro_descricao") %>' Font-Bold="false"></asp:Label>
			                                                        <br /><br />
			                                                        <asp:Label ID="lblSugestao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblSugestao.Text %>' AssociatedControlID="txtSugestao"></asp:Label>
			                                                        <asp:TextBox ID="txtSugestao" runat="server" MaxLength="400" TextMode="MultiLine" SkinID="limite400"></asp:TextBox>
			                                                        <asp:RequiredFieldValidator ID="rfvSugestao" runat="server" ErrorMessage="<%$ Resources:Academico, Curriculo.Cadastro.rfvSugestao.ErrorMessage %>"
				                                                        ControlToValidate="txtSugestao" ValidationGroup="eixo">*</asp:RequiredFieldValidator>
			                                                        <asp:Label ID="lblTipoSugestao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblTipoSugestao.Text %>' AssociatedControlID="ddlTipoSugestao"></asp:Label>
			                                                        <asp:DropDownList ID="ddlTipoSugestao" runat="server">
				                                                        <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Sugestao %>' Value="1" Selected="True"></asp:ListItem>
				                                                        <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Exclusao %>' Value="2" Selected="False"></asp:ListItem>
				                                                        <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Inclusao %>' Value="3" Selected="False"></asp:ListItem>
			                                                        </asp:DropDownList>
			                                                        <asp:HiddenField ID="hdnCrsId" runat="server" Value="-1" />
		                                                        </asp:Panel>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaSugestão %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
	                                                        <ItemTemplate>                                
		                                                        <asp:ImageButton ID="btnListaSugestoes" runat="server" CausesValidation="false" SkinID="btSugerir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnListaSugestoes.ToolTip %>" OnClientClick="ListarSugestoes($(this)); return false;" /> 
                                                                <asp:ImageButton ID="btnIncluirSugestao" runat="server" CausesValidation="false" CommandName="Edit" SkinID="btSugerir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnIncluirSugestao.ToolTip %>" />
		                                                        <asp:ImageButton ID="btnSalvarSugestao" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="eixo" Visible="false" />
		                                                        <asp:ImageButton ID="btnCancelarSugestao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnCancelarEdicao.ToolTip %>" CausesValidation="false" Visible="false" />
		                                                        <asp:ImageButton ID="btnExcluirSugestao" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnExcluirSugestao.ToolTip %>" CausesValidation="false" Visible="false" />
	                                                        </ItemTemplate>
	                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaOrdem %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                                            <ItemTemplate>                                
                                                                <asp:ImageButton ID="btnSubir" runat="server" CausesValidation="false" CommandName="Subir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSubir.ToolTip %>" Height="16" Width="16" />
                                                                <asp:ImageButton ID="btnDescer" runat="server" CausesValidation="false" CommandName="Descer" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnDescer.ToolTip %>" Height="16" Width="16" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="td-acao-dupla"/>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaEditar %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditar" runat="server" CommandName="Edit" SkinID="btEditar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnEditar.ToolTip %>" CausesValidation="false" />
                                                                <asp:ImageButton ID="btnSalvar" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="eixo" Visible="false" />
                                                                <asp:ImageButton ID="btnCancelarEdicao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnCancelarEdicao.ToolTip %>" CausesValidation="false" Visible="false" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaExcluir %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnExcluir.ToolTip %>" CausesValidation="false" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                            
                                                                <!-- Objetivo de aprendizagem -->
                                                                <asp:Button ID="btnNovoObjetivo" runat="server" CausesValidation="False" Text="<%$ Resources:Academico, Curriculo.Cadastro.btnNovoObjetivoAprendizagem.Text %>" OnClick="btnNovoObjetivoAprendizagem_Click" />
                                                                <asp:GridView ID="grvObjetivo" runat="server" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:Academico, Curriculo.Cadastro.grvObjetivoAprendizagem.EmptyDataText %>"
                                                                    DataKeyNames="cro_id,cro_ordem,cro_tipo,cro_idPai"
                                                                    OnRowDataBound="grvEixo_RowDataBound"
                                                                    OnRowCommand="grvEixo_RowCommand"
                                                                    OnDataBound="grvEixo_DataBound"
                                                                    OnRowEditing="grvEixo_RowEditing" 
                                                                    OnRowUpdating="grvEixo_RowUpdating" 
                                                                    OnRowDeleting="grvEixo_RowDeleting"
                                                                    OnRowCancelingEdit="grvEixo_RowCancelingEdit" ShowHeader="false" SkinID="GridItensRight">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvObjetivoAprendizagem.ColunaObjetivoAprendizagem %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("cro_descricao") %>' Font-Bold="false"></asp:Label>
                                                                                <asp:HiddenField ID="hdnAbertoSugestao" runat="server" Value="0" />
                                                                                <uc4:UCListaSugestoes ID="UCListaSugestoes1" runat="server"></uc4:UCListaSugestoes>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:Panel ID="pnlItem" runat="server">
                                                                                    <asp:Label ID="lblDescricao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.grvEixo.lblDescricao.Text %>' AssociatedControlID="txtDescricao"></asp:Label>
                                                                                    <asp:TextBox ID="txtDescricao" runat="server" Text='<%# Bind("cro_descricao") %>' MaxLength="500" SkinID="text60C"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvDescricao" runat="server" ErrorMessage="<%$ Resources:Academico, Curriculo.Cadastro.grvObjetivoAprendizagem.rfvDescricao.ErrorMessage %>"
                                                                                        ControlToValidate="txtDescricao" ValidationGroup="eixo">*</asp:RequiredFieldValidator>
                                                                                    <asp:HiddenField ID="hdnAbertoSugestao" runat="server" Value="0" />
                                                                                    <uc4:UCListaSugestoes ID="UCListaSugestoes1" runat="server"></uc4:UCListaSugestoes>
                                                                                </asp:Panel>
		                                                                        <asp:Panel ID="pnlSugestao" runat="server">
			                                                                        <asp:Label ID="lblDescricaoSugestao" runat="server" Text='<%# Bind("cro_descricao") %>' Font-Bold="false"></asp:Label>
			                                                                        <br /><br />
			                                                                        <asp:Label ID="lblSugestao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblSugestao.Text %>' AssociatedControlID="txtSugestao"></asp:Label>
			                                                                        <asp:TextBox ID="txtSugestao" runat="server" MaxLength="400" TextMode="MultiLine" SkinID="limite400"></asp:TextBox>
			                                                                        <asp:RequiredFieldValidator ID="rfvSugestao" runat="server" ErrorMessage="<%$ Resources:Academico, Curriculo.Cadastro.rfvSugestao.ErrorMessage %>"
				                                                                        ControlToValidate="txtSugestao" ValidationGroup="eixo">*</asp:RequiredFieldValidator>
			                                                                        <asp:Label ID="lblTipoSugestao" runat="server" Text='<%$ Resources:Academico, Curriculo.Cadastro.lblTipoSugestao.Text %>' AssociatedControlID="ddlTipoSugestao"></asp:Label>
			                                                                        <asp:DropDownList ID="ddlTipoSugestao" runat="server">
				                                                                        <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Sugestao %>' Value="1" Selected="True"></asp:ListItem>
				                                                                        <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Exclusao %>' Value="2" Selected="False"></asp:ListItem>
				                                                                        <asp:ListItem Text='<%$ Resources:Academico, Curriculo.Cadastro.ddlTipoSugestao.Inclusao %>' Value="3" Selected="False"></asp:ListItem>
			                                                                        </asp:DropDownList>
			                                                                        <asp:HiddenField ID="hdnCrsId" runat="server" Value="-1" />
		                                                                        </asp:Panel>
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaSugestão %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
	                                                                        <ItemTemplate>                                
		                                                                        <asp:ImageButton ID="btnListaSugestoes" runat="server" CausesValidation="false" SkinID="btSugerir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnListaSugestoes.ToolTip %>" OnClientClick="ListarSugestoes($(this)); return false;" /> 
                                                                                <asp:ImageButton ID="btnIncluirSugestao" runat="server" CausesValidation="false" CommandName="Edit" SkinID="btSugerir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnIncluirSugestao.ToolTip %>" />
		                                                                        <asp:ImageButton ID="btnSalvarSugestao" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="eixo" Visible="false" />
		                                                                        <asp:ImageButton ID="btnCancelarSugestao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnCancelarEdicao.ToolTip %>" CausesValidation="false" Visible="false" />
		                                                                        <asp:ImageButton ID="btnExcluirSugestao" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnExcluirSugestao.ToolTip %>" CausesValidation="false" Visible="false" />
	                                                                        </ItemTemplate>
	                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaOrdem %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                                                            <ItemTemplate>                                
                                                                                <asp:ImageButton ID="btnSubir" runat="server" CausesValidation="false" CommandName="Subir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSubir.ToolTip %>" Height="16" Width="16" />
                                                                                <asp:ImageButton ID="btnDescer" runat="server" CausesValidation="false" CommandName="Descer" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnDescer.ToolTip %>" Height="16" Width="16" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaEditar %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnEditar" runat="server" CommandName="Edit" SkinID="btEditar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnEditar.ToolTip %>" CausesValidation="false" />
                                                                                <asp:ImageButton ID="btnSalvar" runat="server" CommandName="Update" SkinID="btConfirmar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="eixo" Visible="false" />
                                                                                <asp:ImageButton ID="btnCancelarEdicao" runat="server" CommandName="Cancel" SkinID="btCancelar" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnCancelarEdicao.ToolTip %>" CausesValidation="false" Visible="false" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Curriculo.Cadastro.grvGeral.ColunaExcluir %>" HeaderStyle-CssClass="center" HeaderStyle-Width="50">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Delete" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Curriculo.Cadastro.btnExcluir.ToolTip %>" CausesValidation="false" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        </div>
                    </fieldset>
                </fieldset>
            </asp:Panel>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>