<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_Escola_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/Escola/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoRedeEnsino.ascx" TagName="UCComboTipoRedeEnsino"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboEscolaSituacao.ascx" TagName="UCComboEscolaSituacao"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Contato/UCContato.ascx" TagName="UCContato" TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboUnidadeAdministrativa.ascx" TagName="UCComboUnidadeAdministrativa"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoUAEscola.ascx" TagName="UCComboTipoUAEscola"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Endereco/UCEnderecos.ascx" TagName="UCEnderecos"
    TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc10" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoTurno.ascx" TagName="UCComboTipoTurno"
    TagPrefix="uc11" %>
<%@ Register Src="~/WebControls/Combos/UCComboEntidadeGestao.ascx" TagName="UCComboEntidadeGestao"
    TagPrefix="uc12" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario"
    TagPrefix="uc14" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="_updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="escola" />
    <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="OrgaoSupervisao" />
    <div id="divTabs">
        <ul class="hide">
            <li><a href="#divTabs-1">Dados básicos</a></li>
            <li><a id="aCursos" runat="server" href="#divTabs-2">Cursos</a></li>
            <li><a href="#divTabs-4">Obs. gerais</a></li>
            <li id="liImportacao" runat="server" visible="false"><a href="#divTabs-7">Importação de Fechamento</a></li>
        </ul>
        <div id="divTabs-1">
            <asp:UpdatePanel ID="_updDadosBasicos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset id="fsDadosBasicos" runat="server">
                        <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />
                        <uc8:UCComboTipoUAEscola ID="UCComboTipoUAEscola1" runat="server" />
                        <asp:Label ID="LabelCodigoEscola" runat="server" Text="Código da escola" AssociatedControlID="_txtCodigoEscola"></asp:Label>
                        <asp:TextBox ID="_txtCodigoEscola" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
                        <asp:Label ID="LabelNomeEscola" runat="server" Text="Nome da escola *" AssociatedControlID="_txtNomeEscola"></asp:Label>
                        <asp:TextBox ID="_txtNomeEscola" runat="server" MaxLength="200" Width="480"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvNomeEscola" runat="server" ControlToValidate="_txtNomeEscola"
                            Display="Dynamic" ErrorMessage="Nome da escola é obrigatório." ValidationGroup="escola">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LabelCodigoInep" runat="server" Text="Código INEP" AssociatedControlID="_txtCodigoInep"></asp:Label>
                        <asp:TextBox ID="_txtCodigoInep" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
                        <asp:Label ID="LabelCodIntegracao" runat="server" Text="Código integração" AssociatedControlID="_txtCodigoInep"></asp:Label>
                        <asp:TextBox ID="_txtCodIntegracao" runat="server" MaxLength="20" SkinID="text20C" />
                        <uc3:UCComboTipoRedeEnsino ID="UCComboTipoRedeEnsino1" runat="server" />
                        <uc6:UCComboUnidadeAdministrativa ID="UCComboUnidadeAdministrativa1" runat="server" />
                        <asp:CompareValidator ID="cvUnidadeAdministrativaSuperior" runat="server" ErrorMessage=""
                            ControlToValidate="UCComboUnidadeAdministrativa1:_ddlUA" Operator="NotEqual"
                            Display="Dynamic" ValidationGroup="escola">*
                        </asp:CompareValidator>
                        <asp:Label ID="LabelFunIni" runat="server" Text="Data de início do funcionamento *"
                            AssociatedControlID="_txtFunIni"></asp:Label>
                        <asp:TextBox ID="_txtFunIni" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvFunIni" runat="server" ControlToValidate="_txtFunIni"
                            Display="Dynamic" ErrorMessage="Data de início do funcionamento é obrigatório."
                            ValidationGroup="escola">*</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvFunIni" runat="server" ControlToValidate="_txtFunIni"
                            ValidationGroup="escola" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                        <asp:Label ID="LabelFunFim" runat="server" Text="Data de fim do funcionamento" AssociatedControlID="_txtFunFim"></asp:Label>
                        <asp:TextBox ID="_txtFunFim" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                        <asp:CustomValidator ID="cvFunFim" runat="server" ControlToValidate="_txtFunFim"
                            ValidationGroup="escola" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                        <uc4:UCComboEscolaSituacao ID="UCComboEscolaSituacao1" runat="server" />
                        <asp:CompareValidator ID="_cpvEscolaSituacao" runat="server" ErrorMessage="Situação é obrigatório."
                            ControlToValidate="UCComboEscolaSituacao1:_ddlEscolaSituacao" Operator="GreaterThan"
                            ValueToCompare="0" Display="Dynamic" ValidationGroup="escola">*</asp:CompareValidator>
                        <br />
                        <asp:CheckBox ID="ckbTerceirizada" Checked="false" runat="server" Text="<%$ Resources:Academico, Escola.Cadastro.ckbTerceirizada.Text %>" />
                        <asp:CheckBox ID="_ckbControleSistema" Checked="true" runat="server" Text="Escola controlada pelo sistema" />
                        <asp:Label ID="lblRegulamentadaAutorizada" runat="server" AssociatedControlID="ddlRegulamentadaAutorizada"
                            Text="Regulamentada / Autorizada no conselho municipal, estadual ou federal de educação"></asp:Label>
                        <asp:DropDownList ID="ddlRegulamentadaAutorizada" runat="server" DataTextField="desc"
                            AppendDataBoundItems="true" DataValueField="tipo">
                        </asp:DropDownList>
                        <asp:Label ID="Label1" runat="server" Text="CEPs próximos (separados por vírgula, somente números)"
                            AssociatedControlID="_txtCepProx"></asp:Label>
                        <asp:TextBox ID="_txtCepProx" runat="server" Height="123px" TextMode="MultiLine"
                            Width="327px"></asp:TextBox>
                        <asp:CustomValidator ID="cv_CepProx" runat="server" ControlToValidate="_txtCepProx"
                            Display="Dynamic" OnServerValidate="cv_CepProx_ServerValidate" ValidationGroup="escola">*</asp:CustomValidator>
                        <br />
                        <asp:Label ID="lblAtoCricao" runat="server" Text="Ato de criação da escola" AssociatedControlID="_txtAtoCriacao"></asp:Label>
                        <asp:TextBox ID="_txtAtoCriacao" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                        <asp:Label ID="lblDataDiarioOficial" runat="server" Text="Data de publicação no diário oficial"
                            AssociatedControlID="_txtDataDiarioOficial"></asp:Label>
                        <asp:TextBox ID="_txtDataDiarioOficial" runat="server" SkinID="Data"></asp:TextBox>
                        <asp:CustomValidator ID="cvDataDiarioOficial" runat="server" ErrorMessage="" ControlToValidate="_txtDataDiarioOficial"
                            Display="Dynamic" OnServerValidate="ValidarData_ServerValidate" ValidationGroup="escola">*</asp:CustomValidator>
                        <asp:Label ID="lblCodigoNumeroChamada" runat="server" AssociatedControlID="_txtCodigoNumeroChamada"
                            Text="<%$ Resources:Academico, Escola.Cadastro.lblCodigoNumeroChamada.Text %>"></asp:Label>
                        <asp:TextBox ID="_txtCodigoNumeroChamada" runat="server" SkinID="Numerico" MaxLength="9"></asp:TextBox>
                        <div runat="server" id="divFundoVerso">
                            <asp:Label ID="lblFundoVerso" runat="server" AssociatedControlID="txtFundoVerso"
                                Text="<%$ Resources:Academico, Escola.Cadastro.lblFundoVerso.Text %>"></asp:Label>
                            <asp:TextBox ID="txtFundoVerso" runat="server" SkinID="text60C" MaxLength="260"></asp:TextBox>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
            <fieldset id="fsContatos" runat="server">
                <legend>Cadastro de contatos</legend>
                <uc5:UCContato ID="UCContato1" runat="server" _VS_ValidationGroup="escola" />
            </fieldset>
            <fieldset id="fsEndereco" runat="server">
                <legend>Prédio / endereço</legend>
                <uc9:UCEnderecos ID="UCEnderecos1" runat="server" ValidaCensoEscolar="true" />
            </fieldset>
            <asp:UpdatePanel ID="updClassificacao" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset id="fdsClassificacao" runat="server">
                        <legend>Classificação da escola</legend>
                        <div></div>
                        <asp:Repeater ID="rptCampos" runat="server">
                            <HeaderTemplate>
                                <div class="checkboxlist-columns">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ckbCampo" runat="server" Text='<%# Eval("tce_nome") %>' />
                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("tce_id") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                </div> 
                            </FooterTemplate>
                        </asp:Repeater>
                            <br />
                        <asp:Label ID="lblDataInicio" runat="server" Text="Vigência *" AssociatedControlID="txtDataInicioClass"
                            EnableViewState="false"></asp:Label>

                        <asp:TextBox ID="txtDataInicioClass" runat="server" SkinID="Data"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="rfvDataInicio" runat="server" Display="Dynamic" ErrorMessage="Data de início de vigência da classificação é obrigatório."
                            ControlToValidate="txtDataInicioClass" ValidationGroup="escola">*</asp:RequiredFieldValidator>

                        <asp:CustomValidator ID="ctvDataInicio" runat="server" ControlToValidate="txtDataInicioClass"
                            Display="Dynamic" ErrorMessage="Data de início de vigência da classificação não está no formato dd/mm/aaaa ou é inexistente."
                            OnServerValidate="ValidarData_ServerValidate" ValidationGroup="escola">*</asp:CustomValidator>

                        <asp:CompareValidator ID="cpvDataInicio" runat="server" ControlToValidate="txtDataInicioClass"
                            ErrorMessage="Data de início de vigência da classificação deve ser maior ou igual à data de início do funcionamento."
                            ValidationGroup="escola" ControlToCompare="_txtFunIni" Operator="GreaterThanEqual" Enabled="false"
                            Display="Dynamic" Type="Date">*</asp:CompareValidator>

                        <asp:Label ID="lblSeparador" runat="server" Text="à" Style='margin: 0 10px;' EnableViewState="False"></asp:Label>

                        <asp:TextBox ID="txtDataFimClass" runat="server" SkinID="Data"></asp:TextBox>

                        <asp:CompareValidator ID="cpvDataFim" runat="server" ControlToValidate="txtDataFimClass"
                            ErrorMessage="Data de fim de vigência da classificação deve ser maior ou igual à data de início de vigência."
                            ValidationGroup="escola" ControlToCompare="txtDataInicioClass" Operator="GreaterThanEqual"
                            Display="Dynamic" Type="Date">*</asp:CompareValidator>
                        <asp:CustomValidator ID="ctvDataFim" runat="server" ControlToValidate="txtDataFimClass"
                            Display="Dynamic" ErrorMessage="Data de fim de vigência da classificação não está no formato dd/mm/aaaa ou é inexistente."
                            OnServerValidate="ValidarData_ServerValidate" ValidationGroup="escola">*</asp:CustomValidator>

                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
            <fieldset id="fsOrgaoSupervisao" runat="server">
                <legend>Cadastro de orgãos de supervisão</legend>
                <asp:UpdatePanel ID="updOrgaoSupervisao" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="grvOrgaoSupervisao" runat="server" AutoGenerateColumns="False"
                            EmptyDataText="Não existem orgãos de supervisão cadastrados."
                            OnRowDataBound="grvOrgaoSupervisao_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="eos_id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEos_id" runat="server" Text='<%#Bind("eos_id") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDescricao" runat="server" Text="Descrição" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <div style="float: left">
                                                <asp:TextBox ID="txtDescricao" runat="server" Text='<%# Bind("eos_nome") %>' MaxLength="150"
                                                    SkinID="text30C" ValidationGroup="OrgaoSupervisao" />
                                                <asp:TextBox ID="txt_uad_id" runat="server" Text='<%# Bind("uad_id") %>' Visible="false" />
                                                <asp:RequiredFieldValidator runat="server" ID="rfvDescricao" ErrorMessage="Descrição é obrigatório...."
                                                    Display="Dynamic" ControlToValidate="txtDescricao" ValidationGroup="OrgaoSupervisao">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEntidade" runat="server" Text="Entidade" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <div style="float: left">
                                                <uc12:UCComboEntidadeGestao ID="UCComboEntidadeGestao1" runat="server" _MostrarMessageSelecione="true" />
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unidade administrativa ">
                                    <ItemTemplate>
                                        <div>
                                            <asp:TextBox ID="txtUa" runat="server" CausesValidation="False" MaxLength="150" SkinID="text30C"
                                                Text='<%# Bind("uad_nome") %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>
        <div id="divTabs-2" class="hide">
            <asp:UpdatePanel ID="_updCurso" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset id="fdsCurso" runat="server">
                        <asp:GridView ID="_grvCurso" runat="server" AutoGenerateColumns="False" OnRowDataBound="_grvCurso_RowDataBound"
                            OnRowCommand="_grvCurso_RowCommand" EmptyDataText="Não existem cursos cadastrados." DataKeyNames="vis_id">
                            <Columns>
                                <asp:BoundField DataField="ces_id" HeaderText="ces_id">
                                    <HeaderStyle CssClass="hide" />
                                    <ItemStyle CssClass="hide" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cur_id" HeaderText="cur_id">
                                    <HeaderStyle CssClass="hide" />
                                    <ItemStyle CssClass="hide" />
                                </asp:BoundField>
                                <asp:BoundField DataField="crr_id" HeaderText="crr_id">
                                    <HeaderStyle CssClass="hide" />
                                    <ItemStyle CssClass="hide" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Curso">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="_btnAlterarCurso" runat="server" CausesValidation="False" CommandName="Alterar"
                                            Text='<%# Bind("cur_nome") %>' CssClass="wrap400px"></asp:LinkButton>
                                        <asp:Label ID="_lblAlterarCurso" runat="server" Text='<%# Bind("cur_nome") %>' CssClass="wrap400px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Turnos">
                                    <ItemTemplate>
                                        <asp:Repeater ID="_rptTipoTurno" runat="server">
                                            <ItemTemplate>
                                                <asp:Label ID="Label9" runat="server" Text='<%# Bind("ttn_nome") %>'></asp:Label><br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ces_vigenciaInicio" HeaderText="Vigência Inicio">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle CssClass="hide" />
                                    <ItemStyle CssClass="hide" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Vigência Fim">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ces_vigenciaFim") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="_lblVigenciaFim" runat="server" Text='<%# Bind("ces_vigenciaFim") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="hide" HorizontalAlign="Center" />
                                    <ItemStyle CssClass="hide" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="ces_vigencia" HeaderText="Vigência">
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsCurriculoCurso" runat="server"></asp:ObjectDataSource>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divTabs-4" class="hide">
            <fieldset>
                <fieldset>
                    <legend>Observações gerais da escola</legend>
                    <asp:TextBox ID="_txtObs" runat="server" TextMode="MultiLine" SkinID="limite4000"
                        Style="overflow: auto; width: 100%; height: 150px;"></asp:TextBox>
                </fieldset>
            </fieldset>
        </div>
        <div id="divImportacao" visible="False" runat="server">
            <div id="divTabs-7" class="hide">
                <asp:UpdatePanel ID="updImportacao" runat="server">
                    <ContentTemplate>
                        <fieldset id="fdsImportacao" runat="server">
                            <uc14:UCCCalendario ID="UCCCalendario1" runat="server" />
                            <br />
                            <asp:CheckBoxList ID="chkTiposPeriodos" runat="server">
                            </asp:CheckBoxList>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <fieldset>
            <div class="right">
                <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click"
                    CausesValidation="False" />
                <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                    OnClick="_btnCancelar_Click" />
                <input id="txtSelectedTab" type="hidden" runat="server" />
            </div>
        </fieldset>
    </div>
    <div id="divCurriculoCurso" runat="server" title="Cadastro de cursos" class="hide divCurriculoCurso">
        <asp:UpdatePanel ID="_updGridTurno" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="_lblMessageCurso" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="Curso" />
                <fieldset>
                    <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios4" runat="server" />
                    <uc10:UCComboCursoCurriculo ID="_UCComboCursoCurriculo1" runat="server" />
                    <asp:Label ID="LabelVigenciaIniCurso" runat="server" Text="Vigência inicial *" AssociatedControlID="_txtVigenciaIniCurso"></asp:Label>
                    <asp:TextBox ID="_txtVigenciaIniCurso" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvVigenciaIniCurso" runat="server" ControlToValidate="_txtVigenciaIniCurso"
                        ValidationGroup="Curso" Display="Dynamic" ErrorMessage="Vigência inicial é obrigatório.">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvVigenciaIniCurso" runat="server" ControlToValidate="_txtVigenciaIniCurso"
                        ValidationGroup="Curso" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                    <asp:Label ID="LabelVigenciaFimCurso" runat="server" Text="Vigência final" AssociatedControlID="_txtVigenciaFimCurso"></asp:Label>
                    <asp:TextBox ID="_txtVigenciaFimCurso" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                    <asp:CustomValidator ID="cvVigenciaFimCurso" runat="server" ControlToValidate="_txtVigenciaFimCurso"
                        ValidationGroup="Curso" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                    <br />
                    <br />
                    <asp:Panel ID="pnlPeriodo" runat="server">
                        <asp:CheckBoxList ID="chkPeriodos" runat="server">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <fieldset id="_fdsTurnos" runat="server">
                        <legend>Cadastro de turnos</legend>
                        <asp:Label ID="_lblMessageTurno" runat="server" EnableViewState="False"></asp:Label>
                        <asp:GridView ID="_grvTurnos" runat="server" AutoGenerateColumns="False" EmptyDataText="Não existem turnos cadastrados."
                            DataKeyNames="crt_id">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTipoTurno" runat="server" Text="Tipo de turno *" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <uc11:UCComboTipoTurno ID="UCComboTipoTurno1" runat="server" Obrigatorio="true" MostrarMessageSelecione="true"
                                            LabelVisible="false" ValidationGroup="ValidarGridTurno" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblVigenciaInicial" runat="server" Text="Vigência inicial *" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="_txtVigenciaIniTurno" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="_rfvVigenciaIniTurno" runat="server" ControlToValidate="_txtVigenciaIniTurno"
                                            ValidationGroup="ValidarGridTurno" Display="Dynamic" ErrorMessage="Vigência inicial é obrigatório.">*</asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvVigenciaIniTurno" runat="server" ControlToValidate="_txtVigenciaIniTurno"
                                            ValidationGroup="ValidarGridTurno" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vigência final">
                                    <ItemTemplate>
                                        <asp:TextBox ID="_txtVigenciaFimTurno" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                                        <asp:CustomValidator ID="cvVigenciaFimTurno" runat="server" ControlToValidate="_txtVigenciaFimTurno"
                                            ValidationGroup="ValidarGridTurno" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                    <div class="right">
                        <input type="button" id="btnCancelarCurriculoCurso" value="Voltar" class="btn button" />
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <input id="hdnTab" type="hidden" runat="server" class="txtTabAux" />
</asp:Content>
