<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCMovimentacao.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Movimentacao.UCMovimentacao" %>
<%@ Register Src="../Combos/UCComboTipoMovimentacaoMatricula.ascx" TagName="UCComboTipoMovimentacaoMatricula"
    TagPrefix="uc1" %>
<%@ Register Src="../Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc4" %>
<%@ Register Src="../Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc5" %>
<%@ Register Src="../Combos/UCComboTurma.ascx" TagName="UCComboTurma" TagPrefix="uc6" %>
<%@ Register Src="../Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc8" %>
<%@ Register Src="../EscolaOrigem/UCBuscaEscolaOrigem.ascx" TagName="UCBuscaEscolaOrigem"
    TagPrefix="uc3" %>
<%@ Register Src="../Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc7" %>
<%@ Register Src="../Combos/UCComboCurriculoPeriodoAvaliacao.ascx" TagName="UCComboCurriculoPeriodoAvaliacao"
    TagPrefix="uc9" %>
<%@ Register Src="../Combos/UCComboMotivoTransferencia.ascx" TagName="UCComboMotivoTransferencia"
    TagPrefix="uc10" %>
<%@ Register Src="../Combos/UCTipoMovimentacao_MatriculaAntesFechamento.ascx" TagName="UCTipoMovimentacao_MatriculaAntesFechamento"
    TagPrefix="uc2" %>

<asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
<br />
<asp:Panel ID="pnlDadosAtuais" runat="server" GroupingText="Dados atuais do aluno"
    Visible="false">
    <table style="display: inline-block; float: left;">
        <tr>
            <td>
                <asp:Label ID="lblDadosAluno" runat="server"></asp:Label>
                <br />
                <div id="divMatriculaEstadualAnterior" runat="server" visible="false">
                    <asp:Label ID="lblMatriculaEstadualAnterior" runat="server" AssociatedControlID="txtMatriculaEstadualAnterior"
                        Text="Matrícula estadual"></asp:Label>
                    <asp:TextBox ID="txtMatriculaEstadualAnterior" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMatriculaEstadualAnterior" runat="server" ControlToValidate="txtMatriculaEstadualAnterior"
                        Display="Dynamic" ValidationGroup="Aluno">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rexMatriculaEstadualAnterior" runat="server" ControlToValidate="txtMatriculaEstadualAtual"
                        ValidationGroup="Aluno" Display="Dynamic" ValidationExpression="">*</asp:RegularExpressionValidator>
                </div>
                <div id="divTextboxes" runat="server">
                    <asp:Label ID="lblMatriculaAnterior" runat="server" AssociatedControlID="txtMatriculaAnterior"
                        Text="<%$ Resources:Mensagens, MSG_NUMEROMATRICULA %>"></asp:Label>
                    <asp:TextBox ID="txtMatriculaAnterior" runat="server" MaxLength="50" SkinID="text30C"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMatriculaAnterior" runat="server" ControlToValidate="txtMatriculaAnterior"
                        ErrorMessage="<%$ Resources:UserControl, Movimentacao.UCMovimentacao.rfvMatriculaAnterior.ErrorMessage %>" ValidationGroup="Aluno" Display="Dynamic"
                        Visible="false">*</asp:RequiredFieldValidator>
                    <asp:Label ID="lblIDCenso" runat="server" AssociatedControlID="txtIDCenso" Text="ID Censo"
                        EnableViewState="false"></asp:Label>
                    <asp:TextBox ID="txtIDCenso" runat="server" MaxLength="50" SkinID="text30C"></asp:TextBox>
                </div>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>
<asp:Panel ID="pnlMovimentacao" runat="server" GroupingText="Dados da movimentação do aluno">
    <asp:Label ID="lblInformacao" runat="server" Visible="false" CssClass="summary"></asp:Label>
    <uc1:UCComboTipoMovimentacaoMatricula ID="UCComboTipoMovimentacaoMatricula1" runat="server" />
    <uc2:UCTipoMovimentacao_MatriculaAntesFechamento ID="UCTipoMovimentacao_MatriculaAntesFechamento1"
        OnOnSelectedIndexChanged="UCComboTipoMovimentacaoMatricula1_IndexChanged" runat="server"
        Visible="false" />
    <div id="divEscolaPropriaRede" runat="server" visible="false">
        <asp:Label ID="lblUASuperior" runat="server" AssociatedControlID="ddlUASuperior"
            Text="Unidade administrativa superior *"></asp:Label>
        <asp:DropDownList ID="ddlUASuperior" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
            DataTextField="uad_nome" DataValueField="uad_id" OnSelectedIndexChanged="ddlUASuperior_SelectedIndexChanged"
            SkinID="text60C">
        </asp:DropDownList>
        <asp:CompareValidator ID="cpvUnidadeAdministrativa" runat="server" ErrorMessage="Unidade administrativa superior é obrigatório."
            ControlToValidate="ddlUASuperior" Operator="NotEqual" ValueToCompare="00000000-0000-0000-0000-000000000000"
            Display="Dynamic" Visible="false" ValidationGroup="Aluno">*</asp:CompareValidator>
        <asp:Label ID="lblUnidadeEscola" runat="server" AssociatedControlID="ddlUnidadeEscola"
            Text="Escola *"></asp:Label>
        <asp:DropDownList ID="ddlUnidadeEscola" runat="server" AppendDataBoundItems="True"
            AutoPostBack="True" DataTextField="uni_escolaNome" DataValueField="esc_uni_id"
            OnSelectedIndexChanged="ddlUnidadeEscola_SelectedIndexChanged" SkinID="text60C">
        </asp:DropDownList>
        <asp:CompareValidator ID="cpvUnidadeEscola" runat="server" ErrorMessage="Escola é obrigatório."
            ControlToValidate="ddlUnidadeEscola" Operator="NotEqual" ValueToCompare="-1;-1"
            Display="Dynamic" Visible="false" ValidationGroup="Aluno">*</asp:CompareValidator>
    </div>
    <div id="divCurso" runat="server" visible="false">
        <uc4:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" runat="server" CancelSelect="true"
            MostrarMessageSelecione="true" />
    </div>
    <div id="divPeriodo" runat="server" visible="false">
        <uc5:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo1" runat="server" CancelSelect="true"
            _MostrarMessageSelecione="true" />
    </div>
    <div id="divPeriodoAvaliacao" runat="server" visible="false">
        <uc9:UCComboCurriculoPeriodoAvaliacao ID="UCComboCurriculoPeriodoAvaliacao1" runat="server"
            MostrarMessageSelecione="true" />
        <br />
        <asp:Label ID="lblAvaliadoPeriodoCorrente" runat="server" Text="" AssociatedControlID="rdbAvaliadoPeriodoCorrente"></asp:Label>
        <asp:RadioButtonList ID="rdbAvaliadoPeriodoCorrente" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="Sim" Value="Sim"></asp:ListItem>
            <asp:ListItem Text="Não" Value="Nao"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div id="divTurma" runat="server" visible="false">
        <uc6:UCComboTurma ID="UCComboTurma1" runat="server" CancelSelect="true" _MostrarMessageSelecione="true"
            MostraDadosAdicionais="true" />
    </div>
    <div id="divEscolaAdequacaoReclassificao" runat="server" visible="false">
        <asp:Label ID="lblUASuperiorAdequacaoReclassificao" runat="server" AssociatedControlID="ddlUASuperiorAdequacaoReclassificao"
            Text="Unidade administrativa superior *"></asp:Label>
        <asp:DropDownList ID="ddlUASuperiorAdequacaoReclassificao" runat="server" AppendDataBoundItems="True"
            AutoPostBack="True" DataTextField="uad_nome" DataValueField="uad_id" OnSelectedIndexChanged="ddlUASuperiorAdequacaoReclassificao_SelectedIndexChanged"
            SkinID="text60C">
        </asp:DropDownList>
        <asp:CompareValidator ID="cpvUnidadeAdministrativaAdequacaoReclassificao" runat="server"
            ErrorMessage="Unidade administrativa superior é obrigatório." ControlToValidate="ddlUASuperiorAdequacaoReclassificao"
            Operator="NotEqual" ValueToCompare="00000000-0000-0000-0000-000000000000" Display="Dynamic"
            Visible="false" ValidationGroup="Aluno">*</asp:CompareValidator>
        <asp:Label ID="lblUnidadeEscolaAdequacaoReclassificao" runat="server" AssociatedControlID="ddlUnidadeEscolaAdequacaoReclassificao"
            Text="Escola *"></asp:Label>
        <asp:DropDownList ID="ddlUnidadeEscolaAdequacaoReclassificao" runat="server" AppendDataBoundItems="True"
            DataTextField="uni_escolaNome" DataValueField="esc_uni_id" SkinID="text60C">
        </asp:DropDownList>
        <asp:CompareValidator ID="cpvUnidadeEscolaAdequacaoReclassificao" runat="server"
            ErrorMessage="Escola é obrigatório." ControlToValidate="ddlUnidadeEscolaAdequacaoReclassificao"
            Operator="NotEqual" ValueToCompare="-1;-1" Display="Dynamic" Visible="false"
            ValidationGroup="Aluno">*</asp:CompareValidator>
    </div>
    <div id="divMatriculaNovo" runat="server">
        <div id="divMatriculaEstadualAtual" runat="server" visible="false">
            <asp:Label ID="lblMatriculaEstadualAtual" runat="server" AssociatedControlID="txtMatriculaEstadualAtual"
                Text="Matrícula estadual"></asp:Label>
            <asp:TextBox ID="txtMatriculaEstadualAtual" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvMatriculaEstadualAtual" runat="server" ControlToValidate="txtMatriculaEstadualAtual"
                ValidationGroup="Aluno" Display="Dynamic">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="rexMatriculaEstadualAtual" runat="server" ControlToValidate="txtMatriculaEstadualAtual"
                ValidationGroup="Aluno" Display="Dynamic" ValidationExpression="">*</asp:RegularExpressionValidator>
        </div>
        <asp:Label ID="lblMatriculaAtual" runat="server" AssociatedControlID="txtMatriculaAtual"
            Text="<%$ Resources:Mensagens, MSG_NUMEROMATRICULA %>"></asp:Label>
        <asp:TextBox ID="txtMatriculaAtual" runat="server" MaxLength="50" SkinID="text30C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvMatriculaAtual" runat="server" ControlToValidate="txtMatriculaAtual"
            ErrorMessage="<%$ Resources:UserControl, Movimentacao.UCMovimentacao.rfvMatriculaAtual.ErrorMessage %>" ValidationGroup="Aluno" Display="Dynamic"
            Visible="false">*</asp:RequiredFieldValidator>
    </div>
    <div id="divAvaliacao" runat="server">
        <asp:Label ID="lblAvaliacao" runat="server" Text="Avaliação" AssociatedControlID="txtAvaliacao"></asp:Label>
        <asp:TextBox ID="txtAvaliacao" runat="server" SkinID="text60C" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div id="divFrequencia" runat="server">
        <asp:Label ID="lblFrequencia" runat="server" Text="Frequência (%)" AssociatedControlID="txtFrequencia"></asp:Label>
        <asp:TextBox ID="txtFrequencia" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
        <asp:CompareValidator ID="cvFrequenciaMax" runat="server" ErrorMessage="Percentual de frequência deve ser menor ou igual a 100."
            ControlToValidate="txtFrequencia" Display="Dynamic" Operator="LessThanEqual"
            Type="Double" ValidationGroup="Aluno" ValueToCompare="100">*</asp:CompareValidator>
    </div>
    <div id="divRespLancNota" runat="server" visible="false">
        <asp:Label ID="lblLancamentoNotas" runat="server" Text="Responsável pelo lançamento das notas atuais *"
            AssociatedControlID="rblLancamentoNotas"></asp:Label>
        <div style="float: left;">
            <asp:RadioButtonList ID="rblLancamentoNotas" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Escola de origem " Value="1"></asp:ListItem>
                <asp:ListItem Text="Escola de destino" Value="2"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div style="margin-left: 20%;">
            <asp:RequiredFieldValidator ID="rfvLancamentoNotas" runat="server" ControlToValidate="rblLancamentoNotas" ErrorMessage="Responsável pelo lançamento das notas atuais é obrigatório."
                Display="Dynamic" ValidationGroup="Aluno">*</asp:RequiredFieldValidator>
        </div>
    </div>
    <div id="divUnidadeFederativa" runat="server" visible="false">
        <asp:Label ID="lblEstado" runat="server" Text="Estado" AssociatedControlID="ddlEstado"></asp:Label>
        <asp:DropDownList ID="ddlEstado" runat="server" DataSourceID="odsEstado" DataTextField="unf_nome"
            DataValueField="unf_id" AppendDataBoundItems="True" SkinID="text20C">
            <asp:ListItem Value="00000000-0000-0000-0000-000000000000">-- Selecione um estado --</asp:ListItem>
        </asp:DropDownList>
        <asp:CompareValidator ID="cpvEstado" runat="server" ErrorMessage="Estado é obrigatório."
            ControlToValidate="ddlEstado" Operator="NotEqual" ValueToCompare="00000000-0000-0000-0000-000000000000"
            Display="Dynamic" Visible="false" ValidationGroup="Aluno">*</asp:CompareValidator>
        <asp:ObjectDataSource ID="odsEstado" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.END_UnidadeFederativa"
            SelectMethod="GetSelect" TypeName="MSTech.CoreSSO.BLL.END_UnidadeFederativaBO">
            <DeleteParameters>
                <asp:Parameter Name="entity" Type="Object" />
                <asp:Parameter Name="banco" Type="Object" />
            </DeleteParameters>
        </asp:ObjectDataSource>
    </div>
    <div id="divMunicipio" runat="server" visible="false">
        <input id="txtCid_idMunicipio" runat="server" type="hidden" class="tbCid_idMunicipio_incremental" />
        <asp:Label ID="lblMunicipio" runat="server" Text="Município" AssociatedControlID="txtMunicipio"></asp:Label>
        <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="200" Width="210px" CssClass="tbMunicipio_incremental"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" ControlToValidate="txtMunicipio"
            ErrorMessage="Município é obrigatório." ValidationGroup="Aluno" Display="Dynamic"
            Visible="false">*</asp:RequiredFieldValidator>
        <asp:ImageButton ID="btnCadastrarMunicipio" runat="server" SkinID="btNovo" CausesValidation="false"
            ToolTip="Cadastrar nova cidade" Style="vertical-align: middle" OnClick="btnCadastrarMunicipio_Click" />
    </div>
    <div id="divEscolaDestino" runat="server" visible="false">
        <uc3:UCBuscaEscolaOrigem ID="UCBuscaEscolaOrigem1" runat="server" />
    </div>
    <asp:Label ID="lblInfoEscolaDestino" runat="server" Visible="false"></asp:Label>
    <div id="divObservacoes" runat="server" visible="false">
        <asp:Label ID="lblObservacao" runat="server" Text="Observação" AssociatedControlID="txtObservacao"></asp:Label>
        <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" SkinID="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvObservacao" runat="server" ControlToValidate="txtObservacao"
            ErrorMessage="Observacao é obrigatório." ValidationGroup="Aluno" Display="Dynamic"
            Visible="false">*</asp:RequiredFieldValidator>
    </div>
    <div id="divDataMovimentacao" runat="server" visible="false">
        <asp:Label ID="lblDataMovimentacao" runat="server" Text="Data da movimentação *"
            AssociatedControlID="txtDataMovimentacao" EnableViewState="false"></asp:Label>
        <asp:TextBox ID="txtDataMovimentacao" runat="server" SkinID="Data"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvDataMovimentacao" runat="server" ControlToValidate="txtDataMovimentacao"
            ErrorMessage="Data da movimentação é obrigatório." ValidationGroup="Aluno" Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:CustomValidator ID="ctvDataMovimentacao" runat="server" ControlToValidate="txtDataMovimentacao"
            ValidationGroup="Aluno" Display="Dynamic" ErrorMessage="Data da movimentação não está no formato dd/mm/aaaa ou é inexistente."
            OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
    </div>
</asp:Panel>
<asp:Panel ID="pnlResponsavelTransferencia" runat="server" GroupingText="Responsável pela solicitação de transferência"
    Visible="false">
    <asp:Label ID="lblNomeSolicitante" runat="server" Text="Responsável pela solicitação de transferência"
        AssociatedControlID="tbNomeSolicitante"></asp:Label>
    <asp:TextBox ID="tbNomeSolicitante" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
    <asp:Label ID="lblRGSolicitante" runat="server" Text="RG" AssociatedControlID="tbRGSolicitante"></asp:Label>
    <asp:TextBox ID="tbRGSolicitante" runat="server" SkinID="text30C" MaxLength="200"></asp:TextBox>
    <asp:Label ID="lblContatoSolicitante" runat="server" Text="Contato" AssociatedControlID="tbContatoSolicitante"></asp:Label>
    <asp:TextBox ID="tbContatoSolicitante" runat="server" SkinID="text30C" MaxLength="200"></asp:TextBox>
    <uc10:UCComboMotivoTransferencia ID="UCComboMotivoTransferencia1" runat="server" />
    <div id="divOutroMotivoTransferencia" runat="server" visible="false">
        <asp:Label ID="LabelOutroMotivoTransferencia" runat="server" Text="Outro motivo de transferência *"
            AssociatedControlID="tbOutroMotivoTransferencia">
        </asp:Label>
        <asp:TextBox ID="tbOutroMotivoTransferencia" runat="server" MaxLength="200" SkinID="text60C">
        </asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvOutroMotivoTransferencia" runat="server" ControlToValidate="tbOutroMotivoTransferencia"
            ErrorMessage="Outro motivo de transferência é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
    </div>
</asp:Panel>
<asp:Panel ID="pnlHistoricoMovimentacoes" runat="server" GroupingText="Histórico de movimentações"
    Visible="false">
    <asp:GridView ID="grvHistoricoMovimentacao" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        EmptyDataText="Não existem movimentações para este aluno." DataSourceID="odsHistoricoMovimentacao"
        OnDataBound="grvHistoricoMovimentacao_DataBound" AllowSorting="true">
        <Columns>
            <asp:BoundField DataField="mov_dataRealizacao" HeaderText="Data da movimentação"
                SortExpression="mov_dataRealizacao" DataFormatString="{0:dd/MM/yyyy}">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="tmo_nome" HeaderText="Tipo de movimentação" NullDisplayText="-"
                SortExpression="tmo_tipoMovimento" />
            <asp:TemplateField HeaderText="Escola / etapa de ensino / grupamento de ensino anterior"
                SortExpression="escolaAnterior">
                <ItemTemplate>
                    <asp:Label ID="lblEscolaAnterior" runat="server" Text='<%# Bind("escolaAnterior") %>'
                        CssClass="wrap150px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Escola / etapa de ensino / grupamento de ensino atual"
                SortExpression="escolaAtual">
                <ItemTemplate>
                    <asp:Label ID="lblEscolaAtual" runat="server" Text='<%# Bind("escolaAtual") %>' CssClass="wrap150px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="turmaAnterior" HeaderText="Turma Anterior" NullDisplayText="-"
                SortExpression="turmaAnterior"></asp:BoundField>
            <asp:BoundField DataField="turmaAtual" HeaderText="Turma atual" NullDisplayText="-"
                SortExpression="turmaAtual"></asp:BoundField>
        </Columns>
    </asp:GridView>
    <uc7:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvHistoricoMovimentacao" />
    <asp:ObjectDataSource ID="odsHistoricoMovimentacao" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.MTR_Movimentacao"
        TypeName="MSTech.GestaoEscolar.BLL.MTR_MovimentacaoBO" SelectMethod="SelecionaMovimentacaoAluno"
        MaximumRowsParameterName="">
        <SelectParameters>
            <asp:Parameter Name="alu_id" Type="Int64" />
            <asp:Parameter Name="ent_id" DbType="Guid" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="entity" Type="Object" />
            <asp:Parameter Name="banco" Type="Object" />
        </DeleteParameters>
    </asp:ObjectDataSource>
</asp:Panel>
