<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboTipoMovimentacaoMatricula.ascx.cs"
    Inherits="GestaoEscolar.WebControls.Combos.UCComboTipoMovimentacaoMatricula" %>
<asp:CheckBox ID="chkTipoMov" runat="server" Text="Realizar movimentação" Checked="true"
    OnCheckedChanged="chkTipoMov_CheckedChanged"
    AutoPostBack="True"></asp:CheckBox>
<asp:Panel ID="pTipoMovimentacao" runat="server">
    <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false"
        SkinID="SkinMsgErroCombo"></asp:Label>
    <br />
    <asp:Label ID="lblTitulo" runat="server" Text="Tipo de movimentação"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="pTipos" runat="server">
        <fieldset id="fdsTipoInclusao" runat="server" style="width: 30%; display: inline;
            vertical-align: top;" visible="false">
            <legend>Inclusão</legend>
            <div class="clear"></div>
            <asp:RadioButtonList ID="rblTipoInclusao" runat="server" AppendDataBoundItems="True"
                DataSourceID="odsTipoInclusao" DataTextField="tmo_nome" DataValueField="tmo_id_tmo_tipoMovimento"
                OnSelectedIndexChanged="rblTipoInclusao_SelectedIndexChanged">
            </asp:RadioButtonList>
            <asp:ObjectDataSource ID="odsTipoInclusao" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.MTR_TipoMovimentacao"
                SelectMethod="SelecionaTipoMovimentacao" TypeName="MSTech.GestaoEscolar.BLL.MTR_TipoMovimentacaoBO"
                OnSelected="odsMovimentacoes_Selected">
                <SelectParameters>
                    <asp:Parameter Name="ent_id" DbType="Guid" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </fieldset>
        <fieldset id="fdsTipoRealocacao" runat="server" style="width: 30%; display: inline;
            vertical-align: top;" visible="false">
            <legend>Realocação</legend>
            <div class="clear"></div>
            <asp:RadioButtonList ID="rblTipoRealocacao" runat="server" AppendDataBoundItems="True"
                DataSourceID="odsTipoRealocacao" DataTextField="tmo_nome" DataValueField="tmo_id_tmo_tipoMovimento"
                OnSelectedIndexChanged="rblTipoRealocacao_SelectedIndexChanged">
            </asp:RadioButtonList>
            <asp:ObjectDataSource ID="odsTipoRealocacao" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.MTR_TipoMovimentacao"
                SelectMethod="SelecionaTipoMovimentacao" TypeName="MSTech.GestaoEscolar.BLL.MTR_TipoMovimentacaoBO"
                OnSelected="odsMovimentacoes_Selected">
                <SelectParameters>
                    <asp:Parameter Name="ent_id" DbType="Guid" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </fieldset>
        <fieldset id="fdsTipoExclusao" runat="server" style="width: 30%; display: inline;
            vertical-align: top;" visible="false">
            <legend>Exclusão</legend>
            <div class="clear"></div>
            <asp:RadioButtonList ID="rblTipoExclusao" runat="server" AppendDataBoundItems="True"
                DataSourceID="odsTipoExclusao" DataTextField="tmo_nome" DataValueField="tmo_id_tmo_tipoMovimento"
                OnSelectedIndexChanged="rblTipoExclusao_SelectedIndexChanged">
            </asp:RadioButtonList>
            <asp:ObjectDataSource ID="odsTipoExclusao" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.MTR_TipoMovimentacao"
                SelectMethod="SelecionaTipoMovimentacao" TypeName="MSTech.GestaoEscolar.BLL.MTR_TipoMovimentacaoBO"
                OnSelected="odsMovimentacoes_Selected">
                <SelectParameters>
                    <asp:Parameter Name="ent_id" DbType="Guid" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </fieldset>
        <fieldset id="fdsTipoRenovacao" runat="server" style="width: 30%; display: inline;
            vertical-align: top;" visible="false">
            <legend>Renovação</legend>
            <div class="clear"></div>
            <asp:RadioButtonList ID="rblTipoRenovacao" runat="server" AppendDataBoundItems="True"
                DataSourceID="odsTipoRenovacao" DataTextField="tmo_nome" DataValueField="tmo_id_tmo_tipoMovimento"
                OnSelectedIndexChanged="rblTipoRenovacao_SelectedIndexChanged">
            </asp:RadioButtonList>
            <asp:ObjectDataSource ID="odsTipoRenovacao" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.MTR_TipoMovimentacao"
                SelectMethod="SelecionaTipoMovimentacao" TypeName="MSTech.GestaoEscolar.BLL.MTR_TipoMovimentacaoBO"
                OnSelected="odsMovimentacoes_Selected">
                <SelectParameters>
                    <asp:Parameter Name="ent_id" DbType="Guid" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </fieldset>
        <fieldset id="fdsTipoOutros" runat="server" style="width: 30%; display: inline; vertical-align: top;"
            visible="false">
            <legend>Outros</legend>
            <div class="clear"></div>
            <asp:RadioButtonList ID="rblTipoOutros" runat="server" AppendDataBoundItems="True"
                DataSourceID="odsTipoOutros" DataTextField="tmo_nome" DataValueField="tmo_id_tmo_tipoMovimento"
                OnSelectedIndexChanged="rblTipoOutros_SelectedIndexChanged">
            </asp:RadioButtonList>
            <asp:ObjectDataSource ID="odsTipoOutros" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.MTR_TipoMovimentacao"
                SelectMethod="SelecionaTipoMovimentacao" TypeName="MSTech.GestaoEscolar.BLL.MTR_TipoMovimentacaoBO"
                OnSelected="odsMovimentacoes_Selected">
                <SelectParameters>
                    <asp:Parameter Name="ent_id" DbType="Guid" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </fieldset>
    </asp:Panel>
</asp:Panel>
