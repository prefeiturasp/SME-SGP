<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Classe_Efetivacao_Cadastro"
    CodeBehind="Cadastro.aspx.cs" %>

<%@ Register Src="../../WebControls/Combos/UCComboOrdenacao.ascx" TagName="UCComboOrdenacao"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/NavegacaoLancamentoClasse/UCNavegacaoLancamentoClasse.ascx"
    TagName="UCNavegacaoLancamentoClasse" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboAvaliacao.ascx" TagName="UCComboAvaliacao"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/AlunoEfetivacaoObservacao/UCAlunoEfetivacaoObservacao.ascx" TagName="UCAlunoEfetivacaoObservacao" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/BoletimCompletoAluno/UCBoletimCompletoAluno.ascx" TagName="UCBoletimCompletoAluno" TagPrefix="uc3" %>
<%@ Register src="../../WebControls/EfetivacaoNotas/UCEfetivacaoNotas.ascx" tagname="UCEfetivacaoNotas" tagprefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc6:UCEfetivacaoNotas ID="UCEfetivacaoNotas1" runat="server" />
</asp:Content>
