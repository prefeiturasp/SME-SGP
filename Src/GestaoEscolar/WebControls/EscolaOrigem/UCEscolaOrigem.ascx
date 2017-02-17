<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEscolaOrigem.ascx.cs"
    Inherits="GestaoEscolar.WebControls.EscolaOrigem.UCEscolaOrigem" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoRedeEnsino.ascx" TagName="UCComboTipoRedeEnsino"
    TagPrefix="uc2" %>
<%@ Register Src="../Endereco/UCEnderecos.ascx" TagName="UCEnderecos" TagPrefix="uc3" %>
<!-- Busca de escola de origem -->
<div id="divBuscaEscolaOrigemDestino" title="Busca de escola" class="hide">
    <asp:UpdatePanel ID="updBuscaEscolaOrigemDestino" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlBuscaEscolaOrigem" runat="server" DefaultButton="btnPesquisarEscolaOrigemDestino">
                <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="updBuscaEscolaOrigemDestino" />
                <fieldset>
                    <uc2:UCComboTipoRedeEnsino ID="UCComboTipoRedeEnsinoBusca" runat="server" />
                    <asp:Label ID="lblMessageBuscaEscolaOrigemDestino" runat="server" Text="Escola" AssociatedControlID="txtBuscaEscolaOrigemDestino"
                        EnableViewState="false"></asp:Label>
                    <asp:TextBox ID="txtBuscaEscolaOrigemDestino" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                    <div class="right">
                        <asp:Button ID="btnPesquisarEscolaOrigemDestino" runat="server" Text="Pesquisar"
                            CausesValidation="false" OnClick="btnPesquisarEscolaOrigemDestino_Click" />
                        <asp:Button ID="btnNovoEscolaOrigemDestino" runat="server" Text="Nova escola" CausesValidation="False"
                            OnClick="btnNovoEscolaOrigemDestino_Click" />
                        <asp:Button ID="btnCancelarBuscaEscolaOrigemDestino" runat="server" Text="Cancelar"
                            CausesValidation="False" OnClientClick="$('#divBuscaEscolaOrigemDestino').dialog('close');return false;" />
                    </div>
                </fieldset>
            </asp:Panel>
            <fieldset id="fdsResultadosEscolaOrigemDestino" runat="server" visible="false">
                <legend>Resultados</legend>
                <asp:GridView ID="grvEscolaOrigemDestino" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    DataKeyNames="eco_id,eco_nome,cid_id,eco_codigoInep" DataSourceID="odsEscolaOrigemDestino" EmptyDataText="A pesquisa não encontrou resultados."
                    OnSelectedIndexChanging="grvEscolaOrigemDestino_SelectedIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Escola">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbSelecionar" runat="server" CausesValidation="False" CommandName="Select"
                                    Text='<%# Bind("eco_nome") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="eco_codigoInep" HeaderText="Código INEP" />
                        <asp:BoundField DataField="cid_nome" HeaderText="Cidade" />
                    </Columns>
                </asp:GridView>
            </fieldset>
            <asp:ObjectDataSource ID="odsEscolaOrigemDestino" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_AlunoEscolaOrigem"
                EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
                SelectMethod="Select_EscolasPor_RedeEnsino_Nome" StartRowIndexParameterName="currentPage"
                TypeName="MSTech.GestaoEscolar.BLL.ACA_AlunoEscolaOrigemBO" OnSelecting="odsEscolaOrigemDestino_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="tre_id" Type="Int32" />
                    <asp:Parameter Name="eco_nome" Type="String" />
                    <asp:Parameter Name="paginado" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<!-- Cadastro de escola de origem -->
<div id="divCadastroEscolaOrigemDestino" title="Cadastro de escola" class="hide">
    <asp:UpdatePanel ID="updCadastroEscolaOrigemDestino" runat="server">
        <ContentTemplate>
            <fieldset>
                <asp:Label ID="lblMessageCadastroEscolaOrigemDestino" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="vlsEscolaOrigemDestino" runat="server" ValidationGroup="EscolaOrigemDestino"
                    EnableViewState="False" />
                <uc2:UCComboTipoRedeEnsino ID="UCComboTipoRedeEnsino1" runat="server" />
                <asp:Label ID="lblNomeEscolaOrigemDestino" runat="server" Text="Escola *" AssociatedControlID="txtNomeEscolaOrigemDestino"
                    EnableViewState="False"></asp:Label>
                <asp:TextBox ID="txtNomeEscolaOrigemDestino" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNomeEscolaOrigemDestino" ControlToValidate="txtNomeEscolaOrigemDestino"
                    ValidationGroup="EscolaOrigemDestino" runat="server" ErrorMessage="Escola é obrigatório.">*</asp:RequiredFieldValidator>
                <asp:Label ID="lblCodigoInepEscolaOrigemDestino" runat="server" Text="Código INEP"
                    AssociatedControlID="txtCodigoInepEscolaOrigemDestino" EnableViewState="False"></asp:Label>
                <asp:TextBox ID="txtCodigoInepEscolaOrigemDestino" runat="server" SkinID="text20C"
                    MaxLength="20"></asp:TextBox>
                <input id="txtCid_idMunicipio" runat="server" type="hidden" class="tbCid_idMunicipio_incremental" />
                <asp:Label ID="lblCidade" runat="server" Text="Cidade" AssociatedControlID="txtMunicipioDaEscola"></asp:Label>
                <asp:TextBox ID="txtMunicipioDaEscola" runat="server" MaxLength="200" Width="180px"
                    CssClass="tbMunicipio_incremental"></asp:TextBox>
                <asp:ImageButton ID="btnCadastraCidade" runat="server" SkinID="btNovo" CausesValidation="false"
                    ToolTip="Cadastrar nova cidade" OnClick="btnCadastraCidade_Click" Style="vertical-align: middle" />
                <uc3:UCEnderecos ID="UCEnderecos1" runat="server" />
                <div class="right">
                    <asp:Button ID="btnIncluirEscolaOrigemDestino" runat="server" Text="Incluir" OnClick="btnIncluirEscolaOrigemDestino_Click"
                        ValidationGroup="EscolaOrigemDestino" />
                    <asp:Button ID="btnCancelarEscolaOrigemDestino" runat="server" Text="Cancelar" CausesValidation="False"
                        OnClientClick="$('#divCadastroEscolaOrigemDestino').dialog('close');return false;" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
