using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Linq;

public partial class Academico_FormatoAvaliacao_Cadastro : MotherPageLogado
{
    #region Enumeradores

    private enum TipoConceitoMaximo : byte
    {
        QualquerUm = 1,
        ParecerMinimoAprovacao = 2
    }

    #endregion Enumeradores
   
    #region Propriedades

    /// <summary>
    /// Propiedade que armazena valor em ViewState do campo fav_id
    /// Utilizado para armazenar o valor em caso de Edição(Atualização)
    /// de um Formato de Avaliação e atribuir valor deste campo em novos
    /// registros de Avaliação e Avaliação Relacionada.
    /// </summary>
    private int _VS_fav_id
    {
        get
        {
            if (ViewState["_VS_fav_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_fav_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_fav_id"] = value;
        }
    }

    /// <summary>
    /// Propiedade que armazena valor em ViewState do campo ava_id
    /// Utilizado atribuir valor deste campo em novos
    /// registros de Avaliação e Avaliação Relacionada.
    /// </summary>
    private int _VS_ava_id
    {
        get
        {
            if (ViewState["_VS_ava_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_ava_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_ava_id"] = value;
        }
    }

    /// <summary>
    /// Propiedade que armazena valor em ViewState do campo avr_id
    /// Utilizado atribuir valor deste campo em novos
    /// registros de Avaliação Relacionada.
    /// </summary>
    private int _VS_avr_id
    {
        get
        {
            if (ViewState["_VS_avr_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_avr_id"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_avr_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade auxiliar que armazena valor de _VS_fav_id
    /// Usada para referencia na edição(atualização) de Avaliação e Avaliação Relacionada,
    /// onde seus registros se encontram em DataTable no ViewState.
    /// </summary>
    private int _VS_fav_id_alteracao
    {
        get
        {
            if (ViewState["_VS_fav_id_alteracao"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_fav_id_alteracao"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_fav_id_alteracao"] = value;
        }
    }

    /// <summary>
    /// Propriedade auxiliar que armazena valor de _VS_ava_id
    /// Usada para referencia na edição(atualização) de Avaliação,
    /// onde seus registros se encontram em DataTable no ViewState.
    /// </summary>
    private int _VS_ava_id_alteracao
    {
        get
        {
            if (ViewState["_VS_ava_id_alteracao"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_ava_id_alteracao"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_ava_id_alteracao"] = value;
        }
    }

    /// <summary>
    /// Propriedade auxiliar que armazena valor de _VS_avr_id
    /// Usada para referencia na edição(atualização) de Avaliação Relacionada,
    /// onde seus registros se encontram em DataTable no ViewState.
    /// </summary>
    private int _VS_avr_id_alteracao
    {
        get
        {
            if (ViewState["_VS_avr_id_alteracao"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_avr_id_alteracao"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_avr_id_alteracao"] = value;
        }
    }

    /// <summary>
    /// Propriedade auxiliar que armazena valor Inteiro na qual se referencia
    /// a posição da linha de um DataTable em ViewState (_VS_Avaliacao).
    /// Esta variavel é utilizada no carregamento e na edição de registros de Avaliação.
    /// </summary>
    private int _VS_linha_Avaliacao
    {
        get
        {
            if (ViewState["_VS_linha_Avaliacao"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_linha_Avaliacao"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_linha_Avaliacao"] = value;
        }
    }

    /// <summary>
    /// Propriedade auxiliar que armazena valor Inteiro na qual se referencia
    /// a posição da linha de um DataTable em ViewState (_VS_AvaliacaoRelacionada).
    /// Esta variavel é utilizada no carregamento e na edição de registros de Avaliação Relacionada.
    /// </summary>
    private int _VS_linha_AvaliacaoRelacionada
    {
        get
        {
            if (ViewState["_VS_linha_AvaliacaoRelacionada"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_linha_AvaliacaoRelacionada"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_linha_AvaliacaoRelacionada"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de Avaliação
    /// Retorno e atribui valores para o DataTable de Avaliação
    /// Propiedade usada como datasource da grid de Avaliação.
    /// </summary>
    public DataTable _VS_Avaliacao
    {
        get
        {
            if (ViewState["_VS_Avaliacao"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("fav_id");
                dt.Columns.Add("ava_id");
                dt.Columns.Add("ava_nome");
                dt.Columns.Add("ava_peso");
                dt.Columns.Add("ava_tipo");
                dt.Columns.Add("tpc_id");
                dt.Columns.Add("ava_apareceBoletim");
                dt.Columns.Add("ava_situacao");
                dt.Columns.Add("ava_tipo_periodo_grid");
                dt.Columns.Add("ava_apareceBoletim_grid");
                dt.Columns.Add("ava_avaliacao_relacionada");
                dt.Columns.Add("ava_conceitoGlobalObrigatorio");
                dt.Columns.Add("ava_conceitoGlobalObrigatorioFrequencia");
                dt.Columns.Add("ava_disciplinaObrigatoria");
                dt.Columns.Add("ava_baseadaConceitoGlobal");
                dt.Columns.Add("ava_baseadaNotaDisciplina");
                dt.Columns.Add("ava_baseadaAvaliacaoAdicional");
                dt.Columns.Add("ava_mostraBoletimConceitoGlobalNota");
                dt.Columns.Add("ava_mostraBoletimConceitoGlobalFrequencia");
                dt.Columns.Add("ava_mostraBoletimConceitoGlobalAvaliacaoAdicional");
                dt.Columns.Add("ava_mostraBoletimDisciplinaNota");
                dt.Columns.Add("ava_mostraBoletimDisciplinaFrequencia");
                dt.Columns.Add("ava_recFinalConceitoMaximoAprovacao");
                dt.Columns.Add("ava_recFinalConceitoGlobalMinimoNaoAtingido");
                dt.Columns.Add("ava_recFinalFrequenciaMinimaFinalNaoAtingida");
                dt.Columns.Add("ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido");
                dt.Columns.Add("ava_exibeNaoAvaliados");
                dt.Columns.Add("ava_exibeSemProfessor");
                dt.Columns.Add("ava_exibeObservacaoDisciplina");
                dt.Columns.Add("ava_exibeObservacaoConselhoPedagogico");
                dt.Columns.Add("ava_exibeFrequencia");
                dt.Columns.Add("ava_exibeNotaPosConselho");
                dt.Columns.Add("ava_ocultarAtualizacao");

                ViewState["_VS_Avaliacao"] = dt;
            }

            return (DataTable)ViewState["_VS_Avaliacao"];
        }

        set
        {
            ViewState["_VS_Avaliacao"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de Avaliação Relacionada
    /// Retorno e atribui valores para o DataTable de Avaliação Relacionada
    /// Propiedade usada como datasource da grid de Avaliação Relacionada.
    /// </summary>
    public DataTable _VS_AvaliacaoRelacionada
    {
        get
        {
            if (ViewState["_VS_AvaliacaoRelacionada"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("fav_id");
                dt.Columns.Add("ava_id");
                dt.Columns.Add("avr_id");
                dt.Columns.Add("ava_idRelacionada");
                dt.Columns.Add("avr_substituiNota");
                dt.Columns.Add("avr_mantemMaiorNota");
                dt.Columns.Add("avr_obrigatorioNotaMinima");
                dt.Columns.Add("avr_situacao");
                dt.Columns.Add("ava_rel_nome");
                dt.Columns.Add("ava_rel_opcoes_grid");
                dt.Columns.Add("IsNew");
                ViewState["_VS_AvaliacaoRelacionada"] = dt;
            }

            return (DataTable)ViewState["_VS_AvaliacaoRelacionada"];
        }

        set
        {
            ViewState["_VS_AvaliacaoRelacionada"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable vazio (new DataTable)
    /// Propiedade usada para receber DataTable de Avaliação Relacionada
    /// servindo como referente de comparação e backup de mudanças no
    /// DataTable de Avaliação Relacionada. Utilizada no caso de cancelar
    /// procedimentos feitos referente a Avaliações Relacionadas.
    /// </summary>
    public DataTable _VS_AvaliacaoRelacionada_TEMP
    {
        get
        {
            if (ViewState["_VS_AvaliacaoRelacionada_TEMP"] == null)
            {
                DataTable dt = new DataTable();
                ViewState["_VS_AvaliacaoRelacionada_TEMP"] = dt;
            }

            return (DataTable)ViewState["_VS_AvaliacaoRelacionada_TEMP"];
        }

        set
        {
            ViewState["_VS_AvaliacaoRelacionada_TEMP"] = value;
        }
    }

    /// <summary>
    /// Guarda se o formato de avaliação possui avaliação final analítica.
    /// </summary>
    public bool _VS_AvaliacaoFinalAnalitica
    {
        get
        {
            return ViewState["_VS_AvaliacaoFinalAnalitica"] == null ? false : (bool)ViewState["_VS_AvaliacaoFinalAnalitica"];
        }

        set
        {
            ViewState["_VS_AvaliacaoFinalAnalitica"] = value;
        }
    }

    #endregion Propriedades

    #region Métodos

    /// <summary>
    /// Seta os checkboxes referentes as opções de exibição no boletim.
    /// </summary>
    private void SetaCheckboxesExibicaoBoletim(bool checado)
    {
        chkConceitoGlobalAdicional.Checked =
        chkConceitoGlobalFrequencia.Checked =
        chkConceitoGlobalNota.Checked =
        chkDisciplinaFrequencia.Checked =
        chkDisciplinaNotas.Checked = checado;
    }

    private void _LimpaTelaTipoProgressaoParcial()
    {
        _txtValorMinimoProgressaoParcialPorDisciplina.Text = string.Empty;
        _txtQtdeMaxDisciplinasProgressaoParcial.Text = string.Empty;
    }

    private void _LimpaTelaTipoAvaliacao()
    {
        _txtValorMinimoAprovacaoConceitoGlobal.Text = string.Empty;
        _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor = new[] { -1, -1 };

        chkUtilizarAvaliacaoAdicional.Checked = false;
        _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Visible = false;
        _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Valor = new[] { -1, -1 };

        _txtValorMinimoAprovacaoPorDisciplina.Text = string.Empty;
        _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor = new[] { -1, -1 };

        _txtValorMinimoAprovacaoDocente.Text = string.Empty;
        _UCComboEscalaAvaliacao_Esa_idDocente.Valor = new[] { -1, -1 };

        _UCComboEscalaAvaliacao_Esa_idDocente__OnSelectedIndexChange();
    }

    /// <summary>
    /// Propriedade na qual Limpa a Tela (DIV) de Nova Avaliação
    /// </summary>
    private void _LimpaTelaAvaliacao()
    {
        _txtNomeAvaliacao.Text = "";
        _ckbApareceBoletimAvaliacao.Checked = false;
        _ddlTipoAvaliacao.SelectedValue = "0";
        _UCComboTipoPeriodoCalendario.Valor = -1;
        SetaCheckboxesExibicaoBoletim(true);
        txtPeso.Text = "";
    }

    /// <summary>
    /// Propriedade na qual Limpa a Tela (DIV) de Nova Avaliação Relacionada
    /// </summary>
    private void _LimpaTelaAvaliacaoRelacionada()
    {
        _ddlAvaliacao.SelectedValue = "0";
        _ckbSubstituirNota_AvaliacaoRelacionada.Checked = false;
        _ckbManterMaiorNota_AvaliacaoRelacionada.Checked = false;
        _ckbObrigatorioNaoAtingirNotaMinima_AvaliacaoRelacionada.Checked = false;
    }

    /// <summary>
    /// Formata o visible dos checkbox das avalições de base da recuperação
    /// >> chkBaseadaNotaDisciplina é visível quando o tipo de formato for "Notas por disciplina" ou "Global + notas por disciplina"
    /// >> chkBaseadaConceitoGlobal é visível quando o tipo de formato for "Global" ou "Global + notas por disciplina"
    /// >> chkBaseadaAvaliacaoAdicional é visível quando o tipo de formato for "Global" ou "Global + notas por disciplina" E
    /// quando o chkUtilizarAvaliacaoAdicional está checkado
    /// </summary>
    private void FormataChkBaseada()
    {
        divRecBaseada.Visible = true;

        if (ddlTipoFormatoAvaliacao.SelectedValue.Equals("1"))
        {
            chkBaseadaNotaDisciplina.Visible = false;
            chkBaseadaNotaDisciplina.Checked = false;
            chkBaseadaConceitoGlobal.Visible = true;
            if (chkUtilizarAvaliacaoAdicional.Checked)
            {
                chkBaseadaAvaliacaoAdicional.Visible = true;
            }
            else
            {
                chkBaseadaAvaliacaoAdicional.Visible = false;
                chkBaseadaAvaliacaoAdicional.Checked = false;
            }

            lblMessageAvisoRecBaseada.Visible = false;
        }
        else if (ddlTipoFormatoAvaliacao.SelectedValue.Equals("2"))
        {
            chkBaseadaNotaDisciplina.Visible = true;
            chkBaseadaConceitoGlobal.Visible = false;
            chkBaseadaConceitoGlobal.Checked = false;
            chkBaseadaAvaliacaoAdicional.Visible = false;
            chkBaseadaAvaliacaoAdicional.Checked = false;
            lblMessageAvisoRecBaseada.Visible = false;
        }
        else if (ddlTipoFormatoAvaliacao.SelectedValue.Equals("3"))
        {
            chkBaseadaNotaDisciplina.Visible = true;
            chkBaseadaConceitoGlobal.Visible = true;
            if (chkUtilizarAvaliacaoAdicional.Checked)
            {
                chkBaseadaAvaliacaoAdicional.Visible = true;
            }
            else
            {
                chkBaseadaAvaliacaoAdicional.Visible = false;
                chkBaseadaAvaliacaoAdicional.Checked = false;
            }

            lblMessageAvisoRecBaseada.Visible = false;
        }
        else if (ddlTipoFormatoAvaliacao.SelectedValue.Equals("-1"))
        {
            chkBaseadaNotaDisciplina.Visible = false;
            chkBaseadaNotaDisciplina.Checked = false;
            chkBaseadaConceitoGlobal.Visible = false;
            chkBaseadaConceitoGlobal.Checked = false;
            chkBaseadaAvaliacaoAdicional.Visible = false;
            chkBaseadaAvaliacaoAdicional.Checked = false;

            lblMessageAvisoRecBaseada.Visible = true;
            lblMessageAvisoRecBaseada.Text = UtilBO.GetErroMessage("Selecione um tipo para o formato de avaliação.", UtilBO.TipoMensagem.Alerta);
        }
    }

    /// <summary>
    /// Esconde a divRecBaseada e "deschecka" os chekbox dela
    /// </summary>
    private void EscondeDivRecBaseada()
    {
        divRecBaseada.Visible = false;
        chkBaseadaAvaliacaoAdicional.Checked = false;
        chkBaseadaConceitoGlobal.Checked = false;
        chkBaseadaNotaDisciplina.Checked = false;
    }

    /// <summary>
    /// Propriedade na qual formata as telas DIVs de Nova Avaliação
    /// e Nova Avaliação Relacionada de acordo com o tipo de
    /// avaliação selecionada na DIV de nova avaliação.
    /// </summary>
    private void _FormataTelaAvaliacaoPorTipo()
    {
        ckbGlobalNaoObrig.Visible = false;
        ckbGlobalNaoObrigFrequencia.Visible = false;
        ckbDiscNaoObrig.Visible = false;
        chkExibeNaoAvaliados.Visible = false;
        chkExibeSemProfessor.Visible = false;
        chkExibeObservacaoDisciplina.Visible = false;
        chkExibeObservacaoConselho.Visible = false;
        lblMessageExibicaoBoletim.Visible = false;

        divExibicaoBoletim.Visible = false;

        chkConsideraPeriodoLetivo.Visible = false;
        divConceitoMaximo.Visible = false;
        divRegraRecuperacao.Visible = false;

        chkAvaliacaoFinalAnalitica.Visible = false;

        if (_ddlTipoAvaliacao.SelectedValue == "0")
        {
            //não mostra Tipo Periodo e Ordem periodo assim como suas validações de campos requeridos
            _UCComboTipoPeriodoCalendario.ExibeCombo = false;
            _UCComboTipoPeriodoCalendario.Valor = -1;

            //não mostra Avaliação Relacionada assim como sua grid
            _fdsAvaliacaoRelacionada.Visible = false;
            _dgvAvaliacaoRelacionada.Visible = false;

            EscondeDivRecBaseada();
            chkConsideraPeriodoLetivo.Checked = false;
            chkBaseadaConceitoGlobal.AutoPostBack = false;

            chkRegraConceito.Checked = false;
            chkRegraFrequencia.Checked = false;
            chkRegraNotas.Checked = false;
            divPeso.Visible = false;
        }

        // Caso tipo for "Periódica"
        else if (_ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.Periodica).ToString()
            || _ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.PeriodicaFinal).ToString()
            || _ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.ProvaPeriodicaSecretaria).ToString())
        {
            // mostra Tipo Periodo e Ordem periodo assim como suas validações de campos requeridos
            _UCComboTipoPeriodoCalendario.ExibeCombo = true;

            // não mostra Avaliação Relacionada assim como sua grid
            _fdsAvaliacaoRelacionada.Visible = false;
            _dgvAvaliacaoRelacionada.Visible = false;
            divExibicaoBoletim.Visible = true;
            chkExibeObservacaoDisciplina.Visible = true;
            chkExibeObservacaoConselho.Visible = true;

            EscondeDivRecBaseada();

            divPeso.Visible = 
                (_ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.Periodica).ToString()
                || _ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.PeriodicaFinal).ToString())
                && rdlTipoCalculoMediaFinal.SelectedValue == "1";

            if (ddlTipoFormatoAvaliacao.SelectedValue == "-1")
            {
                lblMessageExibicaoBoletim.Visible = true;
                lblMessageExibicaoBoletim.Text = UtilBO.GetErroMessage("Selecione um tipo para o formato de avaliação.", UtilBO.TipoMensagem.Alerta);
            }

            // Exibir parâmetro: Conceito Global Não Obrigatório
            // Quando a avaliação for do tipo periódica e o formato do tipo Conceito Global ou Conceito Global + nota por disciplina
            if (_ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.Periodica).ToString())
            {
                chkExibeNaoAvaliados.Visible = ckbGlobalNaoObrig.Visible = ckbGlobalNaoObrigFrequencia.Visible = (ddlTipoFormatoAvaliacao.SelectedValue == "1" || ddlTipoFormatoAvaliacao.SelectedValue == "3") ? true : false;
                chkExibeSemProfessor.Visible = true;
            }

            // Exibir disciplina não obrigatória
            ckbDiscNaoObrig.Visible = !(_ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.ProvaPeriodicaSecretaria).ToString());

            switch (ddlTipoFormatoAvaliacao.SelectedValue)
            {
                case "1":
                    chkConceitoGlobalNota.Visible = true;
                    chkConceitoGlobalFrequencia.Visible = true;
                    chkConceitoGlobalAdicional.Visible = true;
                    chkDisciplinaNotas.Visible = false;
                    chkDisciplinaFrequencia.Visible = false;
                    break;

                case "2":
                    chkConceitoGlobalNota.Visible = false;
                    chkConceitoGlobalFrequencia.Visible = false;
                    chkConceitoGlobalAdicional.Visible = false;
                    chkDisciplinaNotas.Visible = true;
                    chkDisciplinaFrequencia.Visible = true;
                    break;

                case "3":
                    chkConceitoGlobalNota.Visible = true;
                    chkConceitoGlobalFrequencia.Visible = true;
                    chkConceitoGlobalAdicional.Visible = true;
                    chkDisciplinaNotas.Visible = true;
                    chkDisciplinaFrequencia.Visible = true;
                    break;
            }
        }

        // Caso tipo for "Recuperação" ou "Conselho de Classe"
        else if (_ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.Recuperacao).ToString()
            || _ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.ConselhoClasse).ToString())
        {
            // não mostra Tipo Periodo e Ordem periodo assim como suas validações de campos requeridos
            _UCComboTipoPeriodoCalendario.ExibeCombo = false;
            _UCComboTipoPeriodoCalendario.Valor = -1;

            // mostra Avaliação Relacionada assim como sua grid
            _fdsAvaliacaoRelacionada.Visible = true;
            _dgvAvaliacaoRelacionada.Visible = true;
            divExibicaoBoletim.Visible = false;

            //mostra checkbox´s na DIV Nova Avaliação Relacionada
            _ckbSubstituirNota_AvaliacaoRelacionada.Visible = true;
            _ckbManterMaiorNota_AvaliacaoRelacionada.Visible = true;
            _ckbObrigatorioNaoAtingirNotaMinima_AvaliacaoRelacionada.Visible = true;

            if (_ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.Recuperacao).ToString())
            {
                FormataChkBaseada();
            }
            else
            {
                EscondeDivRecBaseada();
            }

            divPeso.Visible = false;
        }

        // Caso tipo for "Final"
        else if (_ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.Final).ToString())
        {
            // não mostra Tipo Periodo e Ordem periodo assim como suas validações de campos requeridos
            _UCComboTipoPeriodoCalendario.ExibeCombo = false;
            _UCComboTipoPeriodoCalendario.Valor = -1;

            // mostra Avaliação Relacionada assim como sua grid
            _fdsAvaliacaoRelacionada.Visible = false;
            _dgvAvaliacaoRelacionada.Visible = false;
            divExibicaoBoletim.Visible = false;
            EscondeDivRecBaseada();

            //não mostra checkbox´s na DIV Nova Avaliação Relacionada
            _ckbSubstituirNota_AvaliacaoRelacionada.Visible = false;
            _ckbManterMaiorNota_AvaliacaoRelacionada.Visible = false;
            _ckbObrigatorioNaoAtingirNotaMinima_AvaliacaoRelacionada.Visible = false;

            chkAvaliacaoFinalAnalitica.Visible = true;
            divPeso.Visible = false;
        }
        else if (_ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.RecuperacaoFinal).ToString())
        {
            ckbGlobalNaoObrig.Visible = false;
            ckbGlobalNaoObrigFrequencia.Visible = false;
            divExibicaoBoletim.Visible = false;
            _fdsAvaliacaoRelacionada.Visible = true;
            _dgvAvaliacaoRelacionada.Visible = true;

            chkConsideraPeriodoLetivo.Visible = true;
            divRecBaseada.Visible = true;
            chkBaseadaConceitoGlobal.Visible = true;
            chkBaseadaNotaDisciplina.Visible = true;
            chkBaseadaAvaliacaoAdicional.Visible = chkUtilizarAvaliacaoAdicional.Checked;

            divRegraRecuperacao.Visible = true;
            chkRegraConceito.Visible = true;
            chkRegraFrequencia.Visible = true;
            chkRegraNotas.Visible = true;

            FormataChkBaseada();

            _UCComboTipoPeriodoCalendario.ExibeCombo = chkConsideraPeriodoLetivo.Checked;
            _UCComboTipoPeriodoCalendario.Valor = -1;
            divConceitoMaximo.Visible = chkBaseadaConceitoGlobal.Checked;
            chkBaseadaConceitoGlobal.AutoPostBack = true;
            divPeso.Visible = false;
        }
    }

    /// <summary>
    /// propriedade para carregar entidades no combo de escala avaliação
    /// utiliza outro metodo para carregar.
    /// </summary>
    private void _CarregaComboEscalaAvaliacao()
    {
        try
        {
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Titulo = "Escala de avaliação do conceito global";
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.CarregarEscalaAvaliacao();
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Obrigatorio = true;
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.PermiteEditar = true;

            _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Titulo = "Escala de avaliação adicional do conceito global";
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.CarregarEscalaAvaliacaoPorTipo(true, true, false);
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Obrigatorio = true;
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.PermiteEditar = true;

            _UCComboEscalaAvaliacao_Esa_idDocente.Titulo = "Escala de avaliação do docente";
            _UCComboEscalaAvaliacao_Esa_idDocente.CarregarEscalaAvaliacao();
            _UCComboEscalaAvaliacao_Esa_idDocente.Obrigatorio = true;
            _UCComboEscalaAvaliacao_Esa_idDocente.PermiteEditar = true;

            _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Titulo = "Escala de avaliação do(a) " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA");
            _UCComboEscalaAvaliacao_Esa_idPorDisciplina.CarregarEscalaAvaliacao();
            _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Obrigatorio = true;
            _UCComboEscalaAvaliacao_Esa_idPorDisciplina.PermiteEditar = true;

            ddlTipoFormatoAvaliacao.Enabled = true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Propriedade que carrega o combo de avaliação usada na DIV
    /// de Nova Avaliação Relacionada. Carrega todas as avaliações periódicas
    /// cadastradas.
    /// </summary>
    private void _CarregaComboAvaliacao()
    {
        //1 - Periódica, 2 - Recuperação, 3 - Final, 4 - Conselho de Classe, 5 - Periódica + Final
        _ddlAvaliacao.Items.Clear();
        _ddlAvaliacao.Items.Insert(0, new ListItem("-- Selecione uma avaliação --", "0", true));
        DataView dv = new DataView(_VS_Avaliacao);

        switch (_ddlTipoAvaliacao.SelectedValue)
        {
            case "2":
                dv.RowFilter = "ava_tipo = 1 or ava_tipo = 5";
                break;

            case "4":
                dv.RowFilter = "ava_tipo = 1 or ava_tipo = 3 or ava_tipo = 5";
                break;

            case "7":
                dv.RowFilter = "ava_tipo = 3 or ava_tipo = 5";
                break;

            default:
                dv.RowFilter = "ava_tipo = 1";
                break;
        }

        dv.Sort = "ava_nome ASC";
        _ddlAvaliacao.DataSource = dv;
        _ddlAvaliacao.DataBind();
    }

    /// <summary>
    /// Propiredade na qual carrega grid de avaliação
    /// usando o DataTable em ViewState (_VS_Avaliacao) como dataSource.
    /// </summary>
    private void _CarregaGridAvaliacao()
    {
        DataView dv = new DataView(_VS_Avaliacao) { Sort = "ava_nome ASC" };
        _dgvAvaliacao.DataSource = dv;
        _dgvAvaliacao.DataBind();
        SetaFormula();
        _uppCadastroFormatoAvaliacao.Update();
    }

    /// <summary>
    /// Propriedade na qual carrega grid de avaliação relacionada
    /// usando o DataTable em ViewState (_VS_AvaliacaoRelacionada) como dataSource.
    /// Recebe ava_id, para realizar filtragem de avaliações relacionadas a uma avaliação.
    /// Caso receba um valor menor que 0, considera como um novo registro de avaliação (futuro _VS_ava_id + 1)
    /// </summary>
    /// <param name="ava_id">Campo ID de ava_id</param>
    private void _CarregaGridAvaliacaoRelacionada(int ava_id)
    {
        DataView dv = new DataView(_VS_AvaliacaoRelacionada);
        if (ava_id > 0)
        {
            // caso ava_id maior que zero, registro de avaliação existente.
            dv.RowFilter = "ava_id = " + ava_id;
        }
        else
        {
            // caso ava_id menor que zero, novo registro de avaliação, sem id ainda.
            // considera-se o futuro id (_VS_ava_id + 1)
            dv.RowFilter = "ava_id = " + Convert.ToString(_VS_ava_id + 1);
        }

        dv.Sort = "ava_rel_nome ASC";
        _dgvAvaliacaoRelacionada.DataSource = dv;
        _dgvAvaliacaoRelacionada.DataBind();
        _uppAvaliacao.Update();
    }

    /// <summary>
    /// Propriedade na qual faz verificação se para uma avaliação em especifico (ava_id) houve alguma alteração de exclusão, adição ou edição.
    /// </summary>
    /// <param name="ava_id">ID de ava_id para filtro</param>
    /// <returns>True - caso houve alguma modificação / False - caso não houve modificação</returns>
    private bool _AlteracaoAvaliacaoRelacionada(int ava_id)
    {
        try
        {
            for (int i = 0; i < _VS_AvaliacaoRelacionada.Rows.Count; i++)
            {
                if (_VS_AvaliacaoRelacionada.Rows[i].RowState == DataRowState.Deleted)
                {
                    if (_VS_AvaliacaoRelacionada.Rows[i]["ava_id", DataRowVersion.Original].ToString() == Convert.ToString(ava_id) && _VS_AvaliacaoRelacionada.Rows[i].RowState != DataRowState.Unchanged)
                        return true;
                }
                else
                {
                    if (_VS_AvaliacaoRelacionada.Rows[i]["ava_id"].ToString() == Convert.ToString(ava_id) && _VS_AvaliacaoRelacionada.Rows[i].RowState != DataRowState.Unchanged)
                        return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            throw;
        }
    }

    /// <summary>
    /// Propriedade na qual seta valor de _VS_ava_id.
    /// Realiza uma busca no _VS_Avaliacao buscando o maior valor para ava_id.
    /// Propriedade utilizada após Metodo _Carregar() na qual retorna do banco datable para _VS_Avaliacao
    /// proporcionando continuidade na sequencia do ava_id em ViewState.
    /// </summary>
    private void _SelecionaMax_ava_id_from_VS_Avaliacao()
    {
        for (int i = 0; i < _VS_Avaliacao.Rows.Count; i++)
        {
            if (i == 0)
                _VS_ava_id = Convert.ToInt32(_VS_Avaliacao.Rows[i]["ava_id"]);
            else if (Convert.ToInt32(_VS_Avaliacao.Rows[i]["ava_id"]) > _VS_ava_id)
                _VS_ava_id = Convert.ToInt32(_VS_Avaliacao.Rows[i]["ava_id"]);
        }
    }

    /// <summary>
    /// Propriedade na qual seta valor de _VS_avr_id.
    /// Realiza uma busca no _VS_AvaliacaoRelacionada buscando o maior valor para avr_id.
    /// Propriedade utilizada após Metodo _Carregar() na qual retorna do banco datable para _VS_AvaliacaoRelacionada
    /// proporcionando continuidade na sequencia do avr_id em ViewState.
    /// </summary>
    private void _SelecionaMax_avr_id_from_VS_AvaliacaoRelacionada()
    {
        for (int i = 0; i < _VS_AvaliacaoRelacionada.Rows.Count; i++)
        {
            if (i == 0)
            {
                _VS_avr_id = Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[i]["avr_id"]);
            }
            else if (Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[i]["avr_id"]) > _VS_avr_id)
            {
                _VS_avr_id = Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[i]["avr_id"]);
            }
        }
    }

    /// <summary>
    /// Propriedade na qual adequa informações de nomes de avaliações realcionadas no DataTable em ViewState (_VS_Avaliacao)
    /// </summary>
    private void _AdequaInformacao_VS_Avaliacao()
    {
        for (int i = 0; i < _VS_Avaliacao.Rows.Count; i++)
        {
            if (_VS_Avaliacao.Rows[i].RowState != DataRowState.Deleted)
            {
                for (int j = 0; j < _VS_AvaliacaoRelacionada.Rows.Count; j++)
                {
                    if (_VS_AvaliacaoRelacionada.Rows[j].RowState != DataRowState.Deleted)
                    {
                        if (Convert.ToInt32(_VS_Avaliacao.Rows[i]["ava_id"]) == Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[j]["ava_id"]))
                        {
                            _VS_Avaliacao.Rows[i]["ava_avaliacao_relacionada"] += _VS_AvaliacaoRelacionada.Rows[j]["ava_rel_nome"] + "<br /> ";
                        }
                    }
                }
            }
        }
    }

    private void CarregarComboCriterioAprovacao(int tipoFormatoAvaliacao)
    {
        // Habilita e configura o combo de critério de aprovação.
        divCriterioAprovacaoResultadoFinal.Visible = true;
        ddlCriterioAprovacaoResultadoFinal.Items.Clear();
        ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("-- Selecione um critério de aprovação --", "-1"));
        switch (tipoFormatoAvaliacao)
        {
            case (int)ACA_FormatoAvaliacaoTipo.ConceitoGlobal:
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Conceito global e frequência", "1"));
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Conceito global", "2"));
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Apenas frequência", "4"));
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Todos aprovados", "5"));
                break;

            case (int)ACA_FormatoAvaliacaoTipo.Disciplina:
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Nota por " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA"), "3"));
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem(GetGlobalResourceObject("Academico", "FormatoAvaliacao.Cadastro.ddlCriterioAprovacaoResultadoFinal.FrequenciaFinalAjustadaDisciplina").ToString(), "6"));
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Todos aprovados", "5"));
                break;

            case (int)ACA_FormatoAvaliacaoTipo.GlobalDisciplina:
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Conceito global e frequência", "1"));
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Conceito global", "2"));
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Nota por " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA"), "3"));
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Apenas frequência", "4"));
                ddlCriterioAprovacaoResultadoFinal.Items.Add(new ListItem("Todos aprovados", "5"));
                break;

            default:
                divCriterioAprovacaoResultadoFinal.Visible = false;
                break;
        }
    }

    #region Avaliação

    /// <summary>
    /// Exibe um texto informando a fórmula calculada pelos pesos informados nas avaliações.
    /// </summary>
    private void SetaFormula()
    {
        if (chkCalcularMediaAvaliacaoFinal.Checked && 
            rdlTipoCalculoMediaFinal.SelectedValue != "3")
        {
            var avaliacaoFinal =
                (from DataRow dr in _VS_Avaliacao.Select().Where(p => p.RowState != DataRowState.Deleted)
                 let ava_tipo = Convert.ToByte(dr["ava_tipo"])
                 where ava_tipo == (byte)AvaliacaoTipo.Final
                 select dr["ava_nome"].ToString());

            if (avaliacaoFinal.Count() > 0)
            {
                // Somente exibe a fórmula caso exista uma avaliação do tipo final ou periódica + final.
                var avaliacoesPesos =
                    (from DataRow dr in _VS_Avaliacao.Select().Where(p => p.RowState != DataRowState.Deleted)
                     let ava_tipo = Convert.ToByte(dr["ava_tipo"])
                     let ava_peso = Convert.ToDecimal(
                        string.IsNullOrEmpty(dr["ava_peso"].ToString())
                        ? "0" : dr["ava_peso"].ToString())
                     // Avaliações periódicas
                     where
                        (ava_tipo == (byte)AvaliacaoTipo.Periodica || ava_tipo == (byte)AvaliacaoTipo.PeriodicaFinal)
                        && ava_peso > 0
                     select new 
                     {
                         nomePeso = 
                            "([" + dr["ava_nome"].ToString() + "]" + 
                            (rdlTipoCalculoMediaFinal.SelectedValue == "2" ? ""
                            : " × " + ava_peso.ToString("0.##") + "%)")
                         , ava_peso
                     }
                     ).ToList();

                if ((avaliacoesPesos.Count() > 0 && avaliacoesPesos.Sum(p=>p.ava_peso) == 100)
                    || (rdlTipoCalculoMediaFinal.SelectedValue == "2"))
                {
                    lblFormulaMedia.Text = UtilBO.GetErroMessage(
                            "Fórmula para cálculo de " + avaliacaoFinal.FirstOrDefault() + ": <br/>" +
                            avaliacaoFinal.FirstOrDefault() + " = " 
                            + string.Join(" + ", avaliacoesPesos.Select(p=>p.nomePeso).ToArray()) + ")"
                            , UtilBO.TipoMensagem.Informacao);
                    lblFormulaMedia.Visible = true;
                }
                else
                {
                    lblFormulaMedia.Text = UtilBO.GetErroMessage(
                            "A soma dos pesos das avaliações periódicas deve ser igual a 100%."
                            , UtilBO.TipoMensagem.Alerta);
                    lblFormulaMedia.Visible = true;
                }
            }
            else
            {
                lblFormulaMedia.Text = UtilBO.GetErroMessage(
                            "É necessário criar a a avaliação do tipo \"Final\"."
                            , UtilBO.TipoMensagem.Informacao);
                lblFormulaMedia.Visible = true;
            }
        }
        else
        {
            lblFormulaMedia.Visible = false;
        }
    }

    /// <summary>
    /// Faz busca de uma avaliação em especifico no DataTable em ViewState (_VS_Avaliacao)
    /// e carrega seus dados na DIV de nova Avaliação para alteração de seu registro.
    /// </summary>
    /// <param name="ava_id">ID de ava_id</param>
    private void _CarregarAvaliacao(int ava_id)
    {
        try
        {
            _VS_AvaliacaoRelacionada_TEMP = _VS_AvaliacaoRelacionada;
            for (int i = 0; i < _VS_Avaliacao.Rows.Count; i++)
            {
                if (_VS_Avaliacao.Rows[i].RowState != DataRowState.Deleted && ava_id == Convert.ToInt32(_VS_Avaliacao.Rows[i]["ava_id"]))
                {
                    SetaCheckboxesExibicaoBoletim(true);
                    _VS_linha_Avaliacao = i;

                    _txtNomeAvaliacao.Text = Convert.ToString(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_nome"]);
                    _ddlTipoAvaliacao.SelectedValue = Convert.ToString(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_tipo"]);

                    txtPeso.Text = _VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_peso"].ToString().Replace(".",",");

                    _ckbApareceBoletimAvaliacao.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_apareceBoletim"]);
                    if (Convert.ToString(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_tipo"]) == "1"
                        || Convert.ToString(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_tipo"]) == "5"
                        || Convert.ToString(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_tipo"]) == "6"
                        || Convert.ToString(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_tipo"]) == "7")
                    {
                        _UCComboTipoPeriodoCalendario.Valor = Convert.ToInt32(string.IsNullOrEmpty(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["tpc_id"].ToString()) ? -1 : _VS_Avaliacao.Rows[_VS_linha_Avaliacao]["tpc_id"]);

                        if (_UCComboTipoPeriodoCalendario.Valor > 0)
                        {
                            chkConsideraPeriodoLetivo.Checked = true;
                        }
                        else
                        {
                            chkConsideraPeriodoLetivo.Checked = false;
                        }
                    }

                    ckbGlobalNaoObrig.Checked = !Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_conceitoGlobalObrigatorio"]);
                    ckbGlobalNaoObrigFrequencia.Checked = !Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_conceitoGlobalObrigatorioFrequencia"]);
                    chkExibeNaoAvaliados.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_exibeNaoAvaliados"]) &&
                                                    (ACA_FormatoAvaliacaoTipo)Convert.ToInt32(ddlTipoFormatoAvaliacao.SelectedValue) != ACA_FormatoAvaliacaoTipo.Disciplina;
                    chkExibeSemProfessor.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_exibeSemProfessor"]);

                    chkExibeObservacaoDisciplina.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_exibeObservacaoDisciplina"]);

                    chkExibeObservacaoConselho.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_exibeObservacaoConselhoPedagogico"]);

                    chkExibeFrequencia.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_exibeFrequencia"]);
                    chkOcultarAtualizacao.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_ocultarAtualizacao"]);

                    chkExibeNotaPosConselho.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_exibeNotaPosConselho"]);

                    if (!string.IsNullOrEmpty(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_disciplinaObrigatoria"].ToString()))
                    {
                        ckbDiscNaoObrig.Checked = !Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_disciplinaObrigatoria"]);
                    }
                    chkBaseadaConceitoGlobal.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_baseadaConceitoGlobal"]);
                    chkBaseadaNotaDisciplina.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_baseadaNotaDisciplina"]);
                    chkBaseadaAvaliacaoAdicional.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_baseadaAvaliacaoAdicional"]);

                    chkConceitoGlobalAdicional.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_mostraBoletimConceitoGlobalAvaliacaoAdicional"]);
                    chkConceitoGlobalFrequencia.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_mostraBoletimConceitoGlobalFrequencia"]);
                    chkConceitoGlobalNota.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_mostraBoletimConceitoGlobalNota"]);
                    chkDisciplinaNotas.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_mostraBoletimDisciplinaNota"]);
                    chkDisciplinaFrequencia.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_mostraBoletimDisciplinaFrequencia"]);

                    if (!string.IsNullOrEmpty(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_recFinalConceitoMaximoAprovacao"].ToString())
                        && (Convert.ToInt32(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_recFinalConceitoMaximoAprovacao"]) > 0))
                    {
                        chkBaseadaConceitoGlobal.Checked = true;
                        ddlConceitoMaximo.SelectedValue = _VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_recFinalConceitoMaximoAprovacao"].ToString();
                    }

                    chkRegraConceito.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_recFinalConceitoGlobalMinimoNaoAtingido"]);
                    chkRegraFrequencia.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_recFinalFrequenciaMinimaFinalNaoAtingida"]);
                    chkRegraNotas.Checked = Convert.ToBoolean(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido"]);
                    chkAvaliacaoFinalAnalitica.Checked = _VS_AvaliacaoFinalAnalitica;

                    _VS_ava_id_alteracao = Convert.ToInt32(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["ava_id"]);
                    _VS_fav_id_alteracao = Convert.ToInt32(_VS_Avaliacao.Rows[_VS_linha_Avaliacao]["fav_id"]);

                    _FormataTelaAvaliacaoPorTipo();
                    _CarregaGridAvaliacaoRelacionada(ava_id);
                    _uppAvaliacao.Update();
                    _uppAvaliacaoRelacionada.Update();

                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroAvaliacao", "$('#divAvaliacao').dialog('open');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar avaliação.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Configura o checkbox de cálculo de média na avaliação final, de acordo com os tipos de escala.
    /// Só habilita o campo caso a escala do conceito global ou da disciplina for numérica.
    /// </summary>
    private void SetaMediaFinal()
    {
        int esa_tipoGlobal = _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[1];
        int esa_tipoDisciplina = _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[1];

        chkCalcularMediaAvaliacaoFinal.Visible = (esa_tipoDisciplina == 1 || esa_tipoGlobal == 1);
        if (!chkCalcularMediaAvaliacaoFinal.Visible)
        {
            chkCalcularMediaAvaliacaoFinal.Checked = false;
        }

        chkCalcularMediaAvaliacaoFinal_CheckedChanged(chkCalcularMediaAvaliacaoFinal, null);
    }

    #endregion Avaliação

    #region Avaliação Relacionada

    /// <summary>
    /// Propriedade na qual realiza verificação de registros no DataTable Avaliação Relacionada (_VS_AvaliacaoRelacionada)
    /// </summary>
    /// <returns>True - caso exista algum registro / False - caso não exista (vazio)</returns>
    private bool _VerificaExistenciaAvaliacaoRelacionada(int ava_id)
    {
        DataTable dt = _VS_AvaliacaoRelacionada.Copy();
        dt.AcceptChanges();
        DataView dv = new DataView(dt);
        dv.RowFilter = "ava_id = " + ava_id;
        return dv.Count > 0;
    }

    /// <summary>
    /// Verifica se há apenas uma avaliação relacionada
    /// </summary>
    /// <param name="ava_id">Id da avaliação</param>
    /// <returns>True - caso haja apenas 1 - falso caso contrário</returns>
    private bool VerificaExistenciaApenasUmaAvaliacaoRelacionada(int ava_id)
    {
        DataTable dt = _VS_AvaliacaoRelacionada.Copy();
        dt.AcceptChanges();
        DataView dv = new DataView(dt);
        dv.RowFilter = "ava_id = " + ava_id;

        return dv.Count == 1;
    }

    /// <summary>
    /// Verifica se existe uma Avaliação Relacionada igual já cadastrada para esta avaliação.
    /// </summary>
    /// <returns>true - caso exista / false - caso não exista</returns>
    private bool _VerificaMesmaAvaliacaoRelacionada()
    {
        int ava_id = _VS_ava_id_alteracao;

        //Caso de ser um registro de avaliação relacionada já existente (edição/atualização), realiza comparação com todos os registro, menos ele mesmo, no dataTable em ViewState (_VS_AvaliacaoRelacionada)
        for (int i = 0; i < _VS_AvaliacaoRelacionada.Rows.Count; i++)
        {
            if (_VS_AvaliacaoRelacionada.Rows[i].RowState != DataRowState.Deleted)
            {
                if (Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[i]["ava_id"]) == ava_id)
                {
                    if (Convert.ToString(_VS_AvaliacaoRelacionada.Rows[i]["ava_idRelacionada"]) == _ddlAvaliacao.SelectedValue && Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[i]["avr_id"]) != _VS_avr_id_alteracao)
                        return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Faz busca de uma avaliação relacionada em especifico no DataTable em ViewState (_VS_AvaliacaoRelacionada)
    /// e carrega seus dados na DIV de nova Avaliação Relacionada para alteração de seu registro.
    /// </summary>
    /// <param name="avr_id">ID de avr_id</param>
    private void _CarregarAvaliacaoRelacionada(int avr_id, int ava_id)
    {
        try
        {
            for (int i = 0; i < _VS_AvaliacaoRelacionada.Rows.Count; i++)
            {
                if (_VS_AvaliacaoRelacionada.Rows[i].RowState != DataRowState.Deleted
                    && avr_id == Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[i]["avr_id"])
                    && ava_id == Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[i]["ava_id"]))
                {
                    _CarregaComboAvaliacao();
                    _VS_linha_AvaliacaoRelacionada = i;

                    _ddlAvaliacao.SelectedValue = Convert.ToString(_VS_AvaliacaoRelacionada.Rows[_VS_linha_AvaliacaoRelacionada]["ava_idRelacionada"]);

                    _ckbSubstituirNota_AvaliacaoRelacionada.Checked = Convert.ToBoolean(_VS_AvaliacaoRelacionada.Rows[_VS_linha_AvaliacaoRelacionada]["avr_substituiNota"]);

                    _ckbManterMaiorNota_AvaliacaoRelacionada.Checked = Convert.ToBoolean(_VS_AvaliacaoRelacionada.Rows[_VS_linha_AvaliacaoRelacionada]["avr_mantemMaiorNota"]);

                    _ckbObrigatorioNaoAtingirNotaMinima_AvaliacaoRelacionada.Checked = Convert.ToBoolean(_VS_AvaliacaoRelacionada.Rows[_VS_linha_AvaliacaoRelacionada]["avr_obrigatorioNotaMinima"]);

                    _VS_ava_id_alteracao = Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[_VS_linha_AvaliacaoRelacionada]["ava_id"]);
                    _VS_fav_id_alteracao = Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[_VS_linha_AvaliacaoRelacionada]["fav_id"]);
                    _VS_avr_id_alteracao = Convert.ToInt32(_VS_AvaliacaoRelacionada.Rows[_VS_linha_AvaliacaoRelacionada]["avr_id"]);

                    _FormataTelaAvaliacaoPorTipo();

                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroAvaliacaoRelacionada", "$('#divAvaliacaoRelacionada').dialog('open');", true);
                    _uppAvaliacaoRelacionada.Update();
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessageInsert1.Text = UtilBO.GetErroMessage("Erro ao tentar carregar avaliação relacionada.", UtilBO.TipoMensagem.Erro);
            _uppAvaliacao.Update();
        }
    }

    #endregion Avaliação Relacionada

    #region Formato de avaliação

    /// <summary>
    /// Usado para carregar um formato de avaliação em especifico, suas avaliações e as avaliações relacionadas referentes.
    /// </summary>
    /// <param name="fav_id">ID de fav_id para filtro.</param>
    private void _Carregar(int fav_id)
    {
        try
        {
            #region Escolas_Atribui

            // Carrega Formato Avaliacao
            ACA_FormatoAvaliacao _FormatoAvaliacao = new ACA_FormatoAvaliacao { fav_id = fav_id };
            ACA_FormatoAvaliacaoBO.GetEntity(_FormatoAvaliacao);
            if (_FormatoAvaliacao.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("O formato de avaliação não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/FormatoAvaliacao/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            // Seta valor de fav_id carregado em ViewState.
            _VS_fav_id = _FormatoAvaliacao.fav_id;

            if (!_FormatoAvaliacao.fav_padrao)
            {
                // Select dos dados de Unidade Superior e UnidadeEscola.
                ESC_Escola EntidadeEscola = new ESC_Escola { esc_id = _FormatoAvaliacao.esc_id };
                ESC_EscolaBO.GetEntity(EntidadeEscola);

                SYS_UnidadeAdministrativa EntidadeUA = new SYS_UnidadeAdministrativa
                {
                    ent_id = EntidadeEscola.ent_id,
                    uad_id = EntidadeEscola.uad_id
                };
                SYS_UnidadeAdministrativaBO.GetEntity(EntidadeUA);
            }

            _txtNomeFormatoAvaliacao.Text = _FormatoAvaliacao.fav_nome;
            ckbBloqueiaFrequenciaEfetivacao.Checked = _FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacao;
            ckbBloqueiaFrequenciaEfetivacaoDisciplina.Checked = _FormatoAvaliacao.fav_bloqueiaFrequenciaEfetivacaoDisciplina;
            ckbPlanejamentoAulasNotasConjunto.Checked = _FormatoAvaliacao.fav_planejamentoAulasNotasConjunto;
            ddlTipoLancamentoFrequencia.SelectedValue = Convert.ToString(_FormatoAvaliacao.fav_tipoLancamentoFrequencia);
            ddlTipoLancamentoFrequencia.Enabled = false;
            chkFechamentoAutomatico.Checked = _FormatoAvaliacao.fav_fechamentoAutomatico;
            chkFechamentoAutomatico_CheckedChanged(chkFechamentoAutomatico, null);
            if (chkSugerirResultadoFinalDisciplina.Enabled)
            {
                chkSugerirResultadoFinalDisciplina.Checked = _FormatoAvaliacao.fav_sugerirResultadoFinalDisciplina;
            }

            ddlTipoApuracaoFrequencia.SelectedValue = Convert.ToString(_FormatoAvaliacao.fav_tipoApuracaoFrequencia);
            ddlTipoApuracaoFrequencia.Enabled = false;

            ddlCalculoQtdeAulasDadas.SelectedValue = Convert.ToString(_FormatoAvaliacao.fav_calculoQtdeAulasDadas);
            ddlCalculoQtdeAulasDadas.Enabled = false;

            chkEfetivacaoDocente.Visible =
                (ACA_FormatoAvaliacaoTipo)_FormatoAvaliacao.fav_tipo == ACA_FormatoAvaliacaoTipo.ConceitoGlobal ||
                (ACA_FormatoAvaliacaoTipo)_FormatoAvaliacao.fav_tipo == ACA_FormatoAvaliacaoTipo.GlobalDisciplina;
            chkEfetivacaoDocente.Checked = _FormatoAvaliacao.fav_conceitoGlobalDocente;

            chkObrigatorioRelatorio.Visible =
                (ACA_FormatoAvaliacaoTipo)_FormatoAvaliacao.fav_tipo == ACA_FormatoAvaliacaoTipo.ConceitoGlobal ||
                (ACA_FormatoAvaliacaoTipo)_FormatoAvaliacao.fav_tipo == ACA_FormatoAvaliacaoTipo.GlobalDisciplina;
            chkObrigatorioRelatorio.Checked = _FormatoAvaliacao.fav_obrigatorioRelatorioReprovacao;

            if (_FormatoAvaliacao.fav_situacao == 2)
                _ckbBloqueado.Checked = true;

            _VS_AvaliacaoFinalAnalitica = _FormatoAvaliacao.fav_avaliacaoFinalAnalitica;

            #endregion Escolas_Atribui

            #region Tipo_Formato_atribui

            if (_FormatoAvaliacao.fav_tipo == 0)
            {
                ddlTipoFormatoAvaliacao.SelectedValue = "-1";
            }
            else
            {
                ddlTipoFormatoAvaliacao.SelectedValue = _FormatoAvaliacao.fav_tipo.ToString();
                if (_FormatoAvaliacao.fav_tipo == 1)
                {
                    _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Visible = true;
                    _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Visible = false;
                    _UCComboEscalaAvaliacao_Esa_idDocente.Visible = true;
                    ckbBloqueiaFrequenciaEfetivacao.Visible = true;
                    ckbBloqueiaFrequenciaEfetivacaoDisciplina.Visible = false;

                    _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Visible = _FormatoAvaliacao.fav_conceitoGlobalAdicional;
                }
                else if (_FormatoAvaliacao.fav_tipo == 2)
                {
                    _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Visible = false;
                    _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Visible = true;
                    _UCComboEscalaAvaliacao_Esa_idDocente.Visible = true;
                    ckbBloqueiaFrequenciaEfetivacao.Visible = false;
                    ckbBloqueiaFrequenciaEfetivacaoDisciplina.Visible = true;

                    _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Visible = false;
                }
                else if (_FormatoAvaliacao.fav_tipo == 3)
                {
                    _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Visible = true;
                    _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Visible = true;
                    _UCComboEscalaAvaliacao_Esa_idDocente.Visible = true;
                    ckbBloqueiaFrequenciaEfetivacao.Visible = true;
                    ckbBloqueiaFrequenciaEfetivacaoDisciplina.Visible = true;

                    _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Visible = _FormatoAvaliacao.fav_conceitoGlobalAdicional;
                }
            }

            CarregarComboCriterioAprovacao(Convert.ToInt32(ddlTipoFormatoAvaliacao.SelectedValue));

            ddlCriterioAprovacaoResultadoFinal.SelectedValue = Convert.ToString(_FormatoAvaliacao.fav_criterioAprovacaoResultadoFinal);

            txtPercentualMinimoFrequenciaFinalAjustadaDisciplina.Text = _FormatoAvaliacao.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina.ToString();
            chkMostraSomaMedia.Checked = _FormatoAvaliacao.fav_exibirBotaoSomaMedia;
            chkPermiteRecuperacaoQualquerNota.Checked = _FormatoAvaliacao.fav_permiteRecuperacaoQualquerNota;
            chkPermiteRecuperacaoForaPeriodo.Checked = _FormatoAvaliacao.fav_permiteRecuperacaoForaPeriodo;

            ddlCriterioAprovacaoResultadoFinal_SelectedIndexChanged(null, null);

            #endregion Tipo_Formato_atribui

            #region Escalas_atribui

            //ATRIBUI VALORES PARA OS COMBOS DE ESCALA
            //-----Conceito Global
            if (_FormatoAvaliacao.esa_idConceitoGlobal > 0)
            {
                ACA_EscalaAvaliacao ObjAvaliacao = new ACA_EscalaAvaliacao { esa_id = _FormatoAvaliacao.esa_idConceitoGlobal };
                ACA_EscalaAvaliacaoBO.GetEntity(ObjAvaliacao);

                _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor = new[] { ObjAvaliacao.esa_id, ObjAvaliacao.esa_tipo };
            }
            else
            {
                _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor = new[] { -1, -1 };
            }

            //-----Conceito Global Adicional
            chkUtilizarAvaliacaoAdicional.Checked = _FormatoAvaliacao.fav_conceitoGlobalAdicional;
            if (_FormatoAvaliacao.esa_idConceitoGlobalAdicional > 0)
            {
                ACA_EscalaAvaliacao ObjAvaliacao = new ACA_EscalaAvaliacao { esa_id = _FormatoAvaliacao.esa_idConceitoGlobalAdicional };
                ACA_EscalaAvaliacaoBO.GetEntity(ObjAvaliacao);

                _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Valor = new[] { ObjAvaliacao.esa_id, ObjAvaliacao.esa_tipo };
            }
            else
            {
                _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Valor = new[] { -1, -1 };
            }

            //----Por Disciplina
            if (_FormatoAvaliacao.esa_idPorDisciplina > 0)
            {
                ACA_EscalaAvaliacao ObjAvaliacao = new ACA_EscalaAvaliacao { esa_id = _FormatoAvaliacao.esa_idPorDisciplina };
                ACA_EscalaAvaliacaoBO.GetEntity(ObjAvaliacao);

                _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor = new[] { ObjAvaliacao.esa_id, ObjAvaliacao.esa_tipo };
            }
            else
            {
                _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor = new[] { -1, -1 };
            }

            //----Docente
            if (_FormatoAvaliacao.esa_idDocente > 0)
            {
                ACA_EscalaAvaliacao ObjAvaliacao = new ACA_EscalaAvaliacao { esa_id = _FormatoAvaliacao.esa_idDocente };
                ACA_EscalaAvaliacaoBO.GetEntity(ObjAvaliacao);

                _UCComboEscalaAvaliacao_Esa_idDocente.Valor = new[] { ObjAvaliacao.esa_id, ObjAvaliacao.esa_tipo };
            }
            else
                _UCComboEscalaAvaliacao_Esa_idDocente.Valor = new[] { -1, -1 };

            //----Progressão Parcial
            ddlTipoProgressaoParcial.SelectedValue = _FormatoAvaliacao.tipoProgressaoParcial.ToString();

            #endregion Escalas_atribui

            // esse é fixo sempre vai existir
            _txtPercentualMinimoFrequencia.Text = _FormatoAvaliacao.percentualMinimoFrequencia.ToString();
            _txtPercentualBaixaFrequencia.Text = _FormatoAvaliacao.percentualBaixaFrequencia > 0 ? _FormatoAvaliacao.percentualBaixaFrequencia.ToString() : "";
            _txtVariacao.Text = _FormatoAvaliacao.fav_variacao.ToString();



            #region Conceito_Global_Atribui

            //--------------------CONCEITO GLOBAL ATRIBUI VALORES CAMPOS E COMBOS
            //_fieldProgressaoparcial.Visible = false;
            _fieldConceitoGlobal.Visible = false;
            _fieldPorDisciplina.Visible = false;
            _fieldDocente.Visible = true;
            _fieldsetPercentual.Visible = true;

            int esa_id = 0;
            int esa_tipo;

            if (_UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[0] == -1 || _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[1] == -1)
            {
                esa_tipo = -1;
            }
            else
            {
                esa_id = _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[0];
                esa_tipo = _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[1];
                _fieldConceitoGlobal.Visible = true;
            }

            if (esa_tipo == 1) //numérico
            {
                _txtValorMinimoAprovacaoConceitoGlobal.Text = _FormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal;
                _txtValorMinimoAprovacaoConceitoGlobal.Visible = true;
                _lblValorMinimoAprovacaoConceitoGlobal.Visible = true;
                UCComboEscalaAvaliacaoParecerConceitoGlobal.Visible = false;
                UCComboEscalaAvaliacaoParecerConceitoGlobal._Combo.SelectedValue = "-1";
            }
            else if (esa_tipo == 2) //parecer
            {
                UCComboEscalaAvaliacaoParecerConceitoGlobal._Combo.Items.Clear();
                UCComboEscalaAvaliacaoParecerConceitoGlobal._VS_isa_id = esa_id;
                UCComboEscalaAvaliacaoParecerConceitoGlobal._MostrarMessageSelecione = true;
                UCComboEscalaAvaliacaoParecerConceitoGlobal._Combo.DataBind();

                if (!string.IsNullOrEmpty(_FormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal))
                    UCComboEscalaAvaliacaoParecerConceitoGlobal._Combo.SelectedValue = _FormatoAvaliacao.valorMinimoAprovacaoConceitoGlobal;

                UCComboEscalaAvaliacaoParecerConceitoGlobal.Visible = true;
                _txtValorMinimoAprovacaoConceitoGlobal.Visible = false;
            }
            else
            {
                _txtValorMinimoAprovacaoConceitoGlobal.Visible = false;
                UCComboEscalaAvaliacaoParecerConceitoGlobal.Visible = false;
                UCComboEscalaAvaliacaoParecerConceitoGlobal._Combo.SelectedValue = "-1";
            }

            #endregion Conceito_Global_Atribui

            #region Por_Docente

            //--------------------DOCENTE
            if (_UCComboEscalaAvaliacao_Esa_idDocente.Valor[0] == -1 || _UCComboEscalaAvaliacao_Esa_idDocente.Valor[1] == -1)
            {
                esa_id = -1;
                esa_tipo = -1;
            }
            else
            {
                esa_id = _UCComboEscalaAvaliacao_Esa_idDocente.Valor[0];
                esa_tipo = _UCComboEscalaAvaliacao_Esa_idDocente.Valor[1];
                _fieldDocente.Visible = true;
            }

            if (esa_tipo == 1)
            {
                _txtValorMinimoAprovacaoDocente.Text = _FormatoAvaliacao.valorMinimoAprovacaoDocente;
                _txtValorMinimoAprovacaoDocente.Visible = true;
                _lblValorMinimoAprovacaoDocente.Visible = true;
                UCComboEscalaAvaliacaoParecerDocente.Visible = false;
                UCComboEscalaAvaliacaoParecerDocente._Combo.SelectedValue = "-1";
            }
            else
            {
                if (esa_tipo == 2)
                {
                    UCComboEscalaAvaliacaoParecerDocente._Combo.Items.Clear();
                    UCComboEscalaAvaliacaoParecerDocente._VS_isa_id = esa_id;
                    UCComboEscalaAvaliacaoParecerDocente._MostrarMessageSelecione = true;
                    UCComboEscalaAvaliacaoParecerDocente._Combo.DataBind();

                    if (!string.IsNullOrEmpty(_FormatoAvaliacao.valorMinimoAprovacaoDocente))
                        UCComboEscalaAvaliacaoParecerDocente._Combo.SelectedValue = _FormatoAvaliacao.valorMinimoAprovacaoDocente;

                    UCComboEscalaAvaliacaoParecerDocente.Visible = true;
                    _txtValorMinimoAprovacaoDocente.Visible = false;
                    _txtValorMinimoAprovacaoDocente.Text = string.Empty;
                    _lblValorMinimoAprovacaoDocente.Visible = false;
                }
                else
                {
                    _lblValorMinimoAprovacaoDocente.Visible = false;
                    _txtValorMinimoAprovacaoDocente.Visible = false;
                    _txtValorMinimoAprovacaoDocente.Text = string.Empty;
                    UCComboEscalaAvaliacaoParecerDocente.Visible = false;
                    UCComboEscalaAvaliacaoParecerDocente._Combo.SelectedValue = "-1";
                }
            }

            #endregion Por_Docente

            #region Por_Disciplina_Atribui

            //--------------------DISCIPLINA
            if (_UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[0] == -1 || _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[1] == -1)
            {
                esa_id = -1;
                esa_tipo = -1;
            }
            else
            {
                esa_id = _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[0];
                esa_tipo = _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[1];
                _fieldPorDisciplina.Visible = true;
            }

            if (esa_tipo == 1)
            {
                _txtValorMinimoAprovacaoPorDisciplina.Text = _FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina;
                _txtValorMinimoAprovacaoPorDisciplina.Visible = true;
                _lblValorMinimoAprovacaoPorDisciplina.Visible = true;
                UCComboEscalaAvaliacaoParecerPorDisciplina.Visible = false;
                UCComboEscalaAvaliacaoParecerPorDisciplina._Combo.SelectedValue = "-1";
            }
            else
            {
                if (esa_tipo == 2)
                {
                    UCComboEscalaAvaliacaoParecerPorDisciplina._Combo.Items.Clear();
                    UCComboEscalaAvaliacaoParecerPorDisciplina._VS_isa_id = esa_id;
                    UCComboEscalaAvaliacaoParecerPorDisciplina._MostrarMessageSelecione = true;
                    UCComboEscalaAvaliacaoParecerPorDisciplina._Combo.DataBind();

                    if (!string.IsNullOrEmpty(_FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina))
                        UCComboEscalaAvaliacaoParecerPorDisciplina._Combo.SelectedValue = _FormatoAvaliacao.valorMinimoAprovacaoPorDisciplina;

                    UCComboEscalaAvaliacaoParecerPorDisciplina.Visible = true;
                    _txtValorMinimoAprovacaoPorDisciplina.Visible = false;
                    _txtValorMinimoAprovacaoPorDisciplina.Text = string.Empty;
                    _lblValorMinimoAprovacaoPorDisciplina.Visible = false;
                }
                else
                {
                    _lblValorMinimoAprovacaoPorDisciplina.Visible = false;
                    _txtValorMinimoAprovacaoPorDisciplina.Visible = false;
                    _txtValorMinimoAprovacaoPorDisciplina.Text = string.Empty;
                    UCComboEscalaAvaliacaoParecerPorDisciplina.Visible = false;
                    UCComboEscalaAvaliacaoParecerPorDisciplina._Combo.SelectedValue = "-1";
                }
            }

            #endregion Por_Disciplina_Atribui

            #region Progressao_Parcial_Atribui

            //progressao parcial
            UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.Items.Clear();
            UCComboEscalaAvaliacaoParecerProgressaoParcial._VS_isa_id = esa_id;
            UCComboEscalaAvaliacaoParecerProgressaoParcial._MostrarMessageSelecione = true;
            UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.DataBind();

            //_fieldProgressaoparcial.Visible = true;

            if (_FormatoAvaliacao.tipoProgressaoParcial != 0 && _FormatoAvaliacao.tipoProgressaoParcial != 3)
            {
                ddlTipoProgressaoParcial.SelectedValue = _FormatoAvaliacao.tipoProgressaoParcial.ToString();

                //esa_tipo da disciplina tem q ser verificado para ver se é parecer
                if (esa_tipo == 1)
                {
                    UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.SelectedValue = "-1";
                    UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
                    _txtValorMinimoProgressaoParcialPorDisciplina.Visible = true;
                    _txtQtdeMaxDisciplinasProgressaoParcial.Visible = true;
                    _txtValorMinimoProgressaoParcialPorDisciplina.Text = _FormatoAvaliacao.valorMinimoProgressaoParcialPorDisciplina;
                    _txtQtdeMaxDisciplinasProgressaoParcial.Text = _FormatoAvaliacao.qtdeMaxDisciplinasProgressaoParcial.ToString();
                    _lblValorMinimoProgressaoParcialPorDisciplina.Visible = true;
                    _lblQtdeMaxDisciplinasProgressaoParcial.Visible = true;
                }
                else if (esa_tipo == 2)
                {
                    UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.Items.Clear();
                    UCComboEscalaAvaliacaoParecerProgressaoParcial._VS_isa_id = esa_id;
                    UCComboEscalaAvaliacaoParecerProgressaoParcial._MostrarMessageSelecione = true;
                    UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.DataBind();

                    if (!string.IsNullOrEmpty(_FormatoAvaliacao.valorMinimoProgressaoParcialPorDisciplina))
                    {
                        UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.SelectedValue = _FormatoAvaliacao.valorMinimoProgressaoParcialPorDisciplina;
                        UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = true;
                    }
                    else
                    {
                        UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
                    }

                    _lblQtdeMaxDisciplinasProgressaoParcial.Visible = true;
                    _txtQtdeMaxDisciplinasProgressaoParcial.Visible = true;
                    _lblValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                    _txtValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                    _txtValorMinimoProgressaoParcialPorDisciplina.Text = string.Empty;

                    _txtQtdeMaxDisciplinasProgressaoParcial.Text = _FormatoAvaliacao.qtdeMaxDisciplinasProgressaoParcial.ToString();
                }
                else
                {
                    //ddlTipoProgressaoParcial.Enabled = true;
                    UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.SelectedValue = "-1";
                    UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
                    _txtValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                    _txtQtdeMaxDisciplinasProgressaoParcial.Visible = false;
                    _txtValorMinimoProgressaoParcialPorDisciplina.Text = string.Empty;
                    _txtQtdeMaxDisciplinasProgressaoParcial.Text = string.Empty;
                    _lblValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                    _lblQtdeMaxDisciplinasProgressaoParcial.Visible = false;
                }
            }
            else
            {
                UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.SelectedValue = "-1";
                UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
                _txtValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                _txtQtdeMaxDisciplinasProgressaoParcial.Visible = false;
                _txtValorMinimoProgressaoParcialPorDisciplina.Text = string.Empty;
                _txtQtdeMaxDisciplinasProgressaoParcial.Text = string.Empty;
                _lblValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                _lblQtdeMaxDisciplinasProgressaoParcial.Visible = false;

                //if (_FormatoAvaliacao.fav_tipo == 1)
                //ddlTipoProgressaoParcial.Enabled = false;
            }

            #endregion Progressao_Parcial_Atribui

            //==================================================================================
            // Executa carregamento dos dados de Avaliação referente a este formato de avaliação
            try
            {
                SetaMediaFinal();
                if (chkCalcularMediaAvaliacaoFinal.Visible)
                {
                    chkCalcularMediaAvaliacaoFinal.Checked = _FormatoAvaliacao.fav_calcularMediaAvaliacaoFinal;
                }

                // Carrega Avaliacao, seta datable em ViewState
                _VS_Avaliacao = ACA_AvaliacaoBO.GetSelectBy_fav_id(fav_id, false, 1, 1);
                if (_VS_Avaliacao.Rows.Count == 0)
                {
                    // inicia vazio _VS_Avaliacao caso não exista registro
                    _VS_Avaliacao = null;
                }
                else
                {
                    // Seta valor maximo de ava_id a fim de dar continuidade no valor incremental
                    _SelecionaMax_ava_id_from_VS_Avaliacao();
                }

                chkCalcularMediaAvaliacaoFinal_CheckedChanged(chkCalcularMediaAvaliacaoFinal, null);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Erro ao tentar carregar avaliações referente a este formato de avaliação.", UtilBO.TipoMensagem.Erro);
                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/FormatoAvaliacao/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            //=============================================================================================
            // Executa carregamento dos dados de Avaliação Relacionada referente a este formato de avaliação
            try
            {
                // Carrega Avaliacao Relacionada, seta datable em ViewState
                _VS_AvaliacaoRelacionada = ACA_AvaliacaoRelacionadaBO.GetSelectBy_fav_id(fav_id, false, 1, 1);
                if (_VS_AvaliacaoRelacionada.Rows.Count == 0)
                {
                    // inicia vazio _VS_AvaliacaoRelacionada caso não exista registro
                    _VS_AvaliacaoRelacionada = null;
                }
                else
                {
                    // Seta valor maximo de avr_id a fim de dar continuidade no valor incremental
                    _SelecionaMax_avr_id_from_VS_AvaliacaoRelacionada();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Erro ao tentar carregar avaliações relacionadas referente a este formato de avaliação.", UtilBO.TipoMensagem.Erro);
                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/FormatoAvaliacao/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            HabilitaControles(fdsFormatoAvaliacao.Controls, false);

            _AdequaInformacao_VS_Avaliacao();
            _CarregaGridAvaliacao();
            _AtivaValidacaoEscalas(_FormatoAvaliacao.fav_tipo);
            _AtivaValidacaoEscalasFilhos();

            HabilitaControles(divDadosAvaliacao.Controls, false);
            HabilitaControles(_uppAvaliacao.Controls, false);
            HabilitaControles(_uppAvaliacaoRelacionada.Controls, false);
            _btnCancelarAvaliacaoRelacionada.Enabled = _btnCancelarAvaliacao.Enabled = _btnCancelarFormatoAvaliacao.Enabled = true;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o formato de avaliação.", UtilBO.TipoMensagem.Erro);
        }
    }

    public void _MostraCamposTipoProgressaoParcial()
    {
        int esa_tipo = _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[1];

        if (ddlTipoProgressaoParcial.SelectedValue == "-1" || ddlTipoProgressaoParcial.SelectedValue == "3")
        {
            _lblValorMinimoProgressaoParcialPorDisciplina.Visible = false;
            _lblQtdeMaxDisciplinasProgressaoParcial.Visible = false;
            _txtValorMinimoProgressaoParcialPorDisciplina.Visible = false;
            _txtQtdeMaxDisciplinasProgressaoParcial.Visible = false;
            UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
        }
        else if (esa_tipo == 2) //parecer
        {
            _lblValorMinimoProgressaoParcialPorDisciplina.Visible = false;
            _txtValorMinimoProgressaoParcialPorDisciplina.Visible = false;
            _lblQtdeMaxDisciplinasProgressaoParcial.Visible = true;
            _txtQtdeMaxDisciplinasProgressaoParcial.Visible = true;
            UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = true;
        }
        else
        {
            _lblValorMinimoProgressaoParcialPorDisciplina.Visible = true;
            _lblQtdeMaxDisciplinasProgressaoParcial.Visible = true;
            _txtValorMinimoProgressaoParcialPorDisciplina.Visible = true;
            _txtQtdeMaxDisciplinasProgressaoParcial.Visible = true;
            UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
        }
    }

    public void _MostraCamposTipo()
    {
        _lblValorMinimoAprovacaoConceitoGlobal.Visible = false;
        _txtValorMinimoAprovacaoConceitoGlobal.Visible = false;
        _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Visible = false;

        _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Visible = false;

        _lblValorMinimoAprovacaoPorDisciplina.Visible = false;
        _txtValorMinimoAprovacaoPorDisciplina.Visible = false;
        _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Visible = false;

        UCComboEscalaAvaliacaoParecerConceitoGlobal.Visible = false;
        UCComboEscalaAvaliacaoParecerDocente.Visible = false;
        UCComboEscalaAvaliacaoParecerPorDisciplina.Visible = false;
        UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;

        //_fieldProgressaoparcial.Visible = false;
        _fieldDocente.Visible = false;
        _fieldConceitoGlobal.Visible = false;
        _fieldPorDisciplina.Visible = false;
        _fieldsetPercentual.Visible = false;

        //ddlTipoProgressaoParcial.Enabled = true;
        if (ddlTipoFormatoAvaliacao.SelectedValue == "1")
        {
            //_fieldProgressaoparcial.Visible = true;
            _fieldDocente.Visible = true;
            _fieldConceitoGlobal.Visible = true;
            _fieldPorDisciplina.Visible = false;
            _fieldsetPercentual.Visible = true;

            //Conceito global;
            //ddlTipoProgressaoParcial.SelectedValue = "3";
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Visible = true;
            //ddlTipoProgressaoParcial.Enabled = false;
            ckbBloqueiaFrequenciaEfetivacao.Visible = true;
            ckbBloqueiaFrequenciaEfetivacaoDisciplina.Visible = false;
        }
        else if (ddlTipoFormatoAvaliacao.SelectedValue == "2")
        {
            //Por disciplina;
            //_fieldProgressaoparcial.Visible = true;
            _fieldDocente.Visible = true;
            _fieldConceitoGlobal.Visible = false;
            _fieldPorDisciplina.Visible = true;
            _fieldsetPercentual.Visible = true;

            _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Visible = true;

            //ddlTipoProgressaoParcial.Enabled = true;
            //ddlTipoProgressaoParcial.SelectedValue = "-1";

            ckbBloqueiaFrequenciaEfetivacao.Visible = false;
            ckbBloqueiaFrequenciaEfetivacaoDisciplina.Visible = true;

            if (ddlTipoProgressaoParcial.SelectedValue == "-1" || ddlTipoProgressaoParcial.SelectedValue == "3")
            {
                _lblValorMinimoAprovacaoPorDisciplina.Visible = false;
                _txtValorMinimoAprovacaoPorDisciplina.Visible = false;
            }
            else
            {
                _lblValorMinimoAprovacaoPorDisciplina.Visible = true;
                _txtValorMinimoAprovacaoPorDisciplina.Visible = true;
            }
        }
        else if (ddlTipoFormatoAvaliacao.SelectedValue == "3")
        {
            //Conceito global + por disciplina;
            //_fieldProgressaoparcial.Visible = true;
            _fieldDocente.Visible = true;
            _fieldConceitoGlobal.Visible = true;
            _fieldPorDisciplina.Visible = true;
            _fieldsetPercentual.Visible = true;
            //ddlTipoProgressaoParcial.Enabled = true;
            //ddlTipoProgressaoParcial.SelectedValue = "-1";
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Visible = true;
            _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Visible = true;
            ckbBloqueiaFrequenciaEfetivacao.Visible = true;
            ckbBloqueiaFrequenciaEfetivacaoDisciplina.Visible = true;
        }
    }

    /// <summary>
    /// Obrigatoriedade conforme o tipo de formato escolhido
    /// </summary>
    /// <param name="tipoFormato"></param>
    private void _AtivaValidacaoEscalas(int tipoFormato)
    {
        _UCComboEscalaAvaliacao_Esa_idDocente.Obrigatorio = true;
        _UCComboEscalaAvaliacao_Esa_idDocente.ValidationGroup = "_ValidationFormatoAvaliacao";

        _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Obrigatorio = true;
        _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.ValidationGroup = "_ValidationFormatoAvaliacao";

        _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Obrigatorio = true;
        _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.ValidationGroup = "_ValidationFormatoAvaliacao";

        _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Obrigatorio = true;
        _UCComboEscalaAvaliacao_Esa_idPorDisciplina.ValidationGroup = "_ValidationFormatoAvaliacao";

        if (tipoFormato == 1)
        {
            _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Obrigatorio = false;
        }

        if (tipoFormato == 2)
        {
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Obrigatorio = false;
            _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Obrigatorio = false;
        }
    }

    private void _AtivaValidacaoEscalasFilhos()
    {
        _rqfValorMinimoGlobal.Visible = false;
        cvMinConceitoGlobal.Visible = false;
        UCComboEscalaAvaliacaoParecerConceitoGlobal._CPVParecer.Visible = false;
        if (_UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[1] == 1)
        {
            _rqfValorMinimoGlobal.Visible = true;
            cvMinConceitoGlobal.Visible = true;
        }
        else
        {
            if (_UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[1] == 2)
            {
                UCComboEscalaAvaliacaoParecerConceitoGlobal._CPVParecer.Visible = true;
                UCComboEscalaAvaliacaoParecerConceitoGlobal._CPVParecer.ErrorMessage = UCComboEscalaAvaliacaoParecerConceitoGlobal._Label.Text.Replace("*", "") + " é obrigatório.";
                UCComboEscalaAvaliacaoParecerConceitoGlobal._CPVParecer.ValidationGroup = "_ValidationFormatoAvaliacao";
            }
        }

        _cpvMinPorDisciplina.Visible = false;
        _rfvValorMinimoDisciplina.Visible = false;
        UCComboEscalaAvaliacaoParecerPorDisciplina._CPVParecer.Visible = false;
        if (_UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[1] == 1)
        {
            _cpvMinPorDisciplina.Visible = true;
            _rfvValorMinimoDisciplina.Visible = true;
        }
        else
        {
            if (_UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[1] == 2)
            {
                UCComboEscalaAvaliacaoParecerPorDisciplina._CPVParecer.Visible = true;
                UCComboEscalaAvaliacaoParecerPorDisciplina._CPVParecer.ErrorMessage = UCComboEscalaAvaliacaoParecerPorDisciplina._Label.Text.Replace("*", "") + " é obrigatório.";
                UCComboEscalaAvaliacaoParecerPorDisciplina._CPVParecer.ValidationGroup = "_ValidationFormatoAvaliacao";
            }
        }

        _cpvMinPorDocente.Visible = false;
        //_rfvValorMinimoDocente.Visible = false;
        UCComboEscalaAvaliacaoParecerDocente._CPVParecer.Visible = false;
        if (_UCComboEscalaAvaliacao_Esa_idDocente.Valor[1] == 1)
        {
            _cpvMinPorDocente.Visible = true;
            //_rfvValorMinimoDocente.Visible = true;
        }
        else
        {
            if (_UCComboEscalaAvaliacao_Esa_idDocente.Valor[1] == 2)
            {
                UCComboEscalaAvaliacaoParecerDocente._CPVParecer.Visible = true;
                UCComboEscalaAvaliacaoParecerDocente._CPVParecer.ErrorMessage = UCComboEscalaAvaliacaoParecerDocente._Label.Text.Replace("*", "") + " é obrigatório.";
                UCComboEscalaAvaliacaoParecerDocente._CPVParecer.ValidationGroup = "_ValidationFormatoAvaliacao";
            }
        }
    }

    #endregion Formato de avaliação
    
    private void _Combo_EscalaAvaliacaoConceitoGlobal__OnSelectedIndexChange()
    {
        _txtValorMinimoAprovacaoConceitoGlobal.Text = string.Empty;
        int esa_tipo;
        int esa_id = -1;

        if (_UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[0] == -1 || _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[1] == -1)
            esa_tipo = -1;
        else
        {
            esa_id = _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[0];
            esa_tipo = _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.Valor[1];
        }

        UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
        if (esa_tipo == 1) //numerico
        {
            UCComboEscalaAvaliacaoParecerConceitoGlobal.Visible = false;
            _lblValorMinimoAprovacaoConceitoGlobal.Visible = true;
            _txtValorMinimoAprovacaoConceitoGlobal.Visible = true;
        }
        else if (esa_tipo == 2) //parecer
        {
            UCComboEscalaAvaliacaoParecerConceitoGlobal._Combo.Items.Clear();
            UCComboEscalaAvaliacaoParecerConceitoGlobal._VS_isa_id = esa_id;
            UCComboEscalaAvaliacaoParecerConceitoGlobal._MostrarMessageSelecione = true;
            UCComboEscalaAvaliacaoParecerConceitoGlobal._Combo.DataBind();

            UCComboEscalaAvaliacaoParecerConceitoGlobal.Visible = true;
            _lblValorMinimoAprovacaoConceitoGlobal.Visible = false;
            _txtValorMinimoAprovacaoConceitoGlobal.Visible = false;
        }
        else if (esa_tipo == 3) //relatorio
        {
            UCComboEscalaAvaliacaoParecerConceitoGlobal.Visible = false;
            _lblValorMinimoAprovacaoConceitoGlobal.Visible = false;
            _txtValorMinimoAprovacaoConceitoGlobal.Visible = false;
        }
        else
        {
            UCComboEscalaAvaliacaoParecerConceitoGlobal.Visible = false;
            _lblValorMinimoAprovacaoConceitoGlobal.Visible = false;
            _txtValorMinimoAprovacaoConceitoGlobal.Visible = false;
        }

        SetaMediaFinal();

        _AtivaValidacaoEscalasFilhos();
    }

    private void _Combo_EscalaAvaliacaoPorDisciplina__OnSelectedIndexChange()
    {
        int esa_tipo;
        int esa_id = -1;

        if (_UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[1] == -1)
            esa_tipo = -1;
        else
        {
            esa_id = _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[0];
            esa_tipo = _UCComboEscalaAvaliacao_Esa_idPorDisciplina.Valor[1];
        }

        //ddlTipoProgressaoParcial.Enabled = true;
        if (esa_tipo == 1) //numerico
        {
            _lblValorMinimoAprovacaoPorDisciplina.Visible = true;
            _txtValorMinimoAprovacaoPorDisciplina.Visible = true;
            UCComboEscalaAvaliacaoParecerPorDisciplina.Visible = false;
            _lblQtdeMaxDisciplinasProgressaoParcial.Visible = false;

            if (ddlTipoProgressaoParcial.SelectedValue == "-1" || ddlTipoProgressaoParcial.SelectedValue == "3")
            {
                _lblValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                _lblQtdeMaxDisciplinasProgressaoParcial.Visible = false;
                _txtValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                _txtQtdeMaxDisciplinasProgressaoParcial.Visible = false;
                UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
                //ddlTipoProgressaoParcial.Enabled = true;
                _txtValorMinimoProgressaoParcialPorDisciplina.Text = string.Empty;
                _txtQtdeMaxDisciplinasProgressaoParcial.Text = string.Empty;
            }
            else
            {
                _lblValorMinimoProgressaoParcialPorDisciplina.Visible = true;
                _lblQtdeMaxDisciplinasProgressaoParcial.Visible = true;
                _txtValorMinimoProgressaoParcialPorDisciplina.Visible = true;
                _txtQtdeMaxDisciplinasProgressaoParcial.Visible = true;
                UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
                //ddlTipoProgressaoParcial.Enabled = true;
            }
        }
        else if (esa_tipo == 2) //parecer
        {
            UCComboEscalaAvaliacaoParecerPorDisciplina._Combo.Items.Clear();
            UCComboEscalaAvaliacaoParecerPorDisciplina._VS_isa_id = esa_id;
            UCComboEscalaAvaliacaoParecerPorDisciplina._MostrarMessageSelecione = true;
            UCComboEscalaAvaliacaoParecerPorDisciplina._Combo.DataBind();
            UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.Items.Clear();
            UCComboEscalaAvaliacaoParecerProgressaoParcial._VS_isa_id = esa_id;
            UCComboEscalaAvaliacaoParecerProgressaoParcial._MostrarMessageSelecione = true;
            UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.DataBind();

            UCComboEscalaAvaliacaoParecerPorDisciplina.Visible = true;
            _lblValorMinimoAprovacaoPorDisciplina.Visible = false;
            _txtValorMinimoAprovacaoPorDisciplina.Visible = false;

            if (ddlTipoProgressaoParcial.SelectedValue == "-1" || ddlTipoProgressaoParcial.SelectedValue == "3")
            {
                _lblValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                _txtValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                _lblQtdeMaxDisciplinasProgressaoParcial.Visible = true;
                _txtQtdeMaxDisciplinasProgressaoParcial.Visible = true;
                UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
                _txtValorMinimoProgressaoParcialPorDisciplina.Text = string.Empty;
                _txtQtdeMaxDisciplinasProgressaoParcial.Text = string.Empty;
                //ddlTipoProgressaoParcial.Enabled = true;
            }
            else
            {
                UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = true;
                //ddlTipoProgressaoParcial.Enabled = true;
                _lblValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                _txtValorMinimoProgressaoParcialPorDisciplina.Visible = false;
                _lblQtdeMaxDisciplinasProgressaoParcial.Visible = true;
                _txtQtdeMaxDisciplinasProgressaoParcial.Visible = true;
                _txtValorMinimoProgressaoParcialPorDisciplina.Text = string.Empty;
                _txtQtdeMaxDisciplinasProgressaoParcial.Text = string.Empty;
            }
        }
        else if (esa_tipo == 3) //relatorio
        {
            UCComboEscalaAvaliacaoParecerPorDisciplina.Visible = false;
            _lblValorMinimoAprovacaoPorDisciplina.Visible = false;
            _txtValorMinimoAprovacaoPorDisciplina.Visible = false;

            //_lblValorMinimoAprovacaoPorDisciplina.Text = string.Empty;
            _txtValorMinimoAprovacaoPorDisciplina.Text = string.Empty;

            //ddlTipoProgressaoParcial.SelectedValue = "3";
            //ddlTipoProgressaoParcial.Enabled = false;
            _lblValorMinimoProgressaoParcialPorDisciplina.Visible = false;
            _lblQtdeMaxDisciplinasProgressaoParcial.Visible = true;
            _txtValorMinimoProgressaoParcialPorDisciplina.Visible = false;
            _txtQtdeMaxDisciplinasProgressaoParcial.Visible = true;
            UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;

            _txtValorMinimoProgressaoParcialPorDisciplina.Text = string.Empty;
            _txtQtdeMaxDisciplinasProgressaoParcial.Text = string.Empty;
        }
        else
        {
            UCComboEscalaAvaliacaoParecerPorDisciplina.Visible = false;
            _lblValorMinimoAprovacaoPorDisciplina.Visible = false;
            _txtValorMinimoAprovacaoPorDisciplina.Visible = false;

            _txtValorMinimoAprovacaoPorDisciplina.Text = string.Empty;
            UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;
            UCComboEscalaAvaliacaoParecerProgressaoParcial._Combo.SelectedValue = "-1";

            //ddlTipoProgressaoParcial.Enabled = true;
            _lblValorMinimoProgressaoParcialPorDisciplina.Visible = false;
            _lblQtdeMaxDisciplinasProgressaoParcial.Visible = true;
            _txtValorMinimoProgressaoParcialPorDisciplina.Visible = false;
            _txtQtdeMaxDisciplinasProgressaoParcial.Visible = true;
            UCComboEscalaAvaliacaoParecerProgressaoParcial.Visible = false;

            _txtValorMinimoProgressaoParcialPorDisciplina.Text = string.Empty;
            _txtQtdeMaxDisciplinasProgressaoParcial.Text = string.Empty;
        }
        SetaMediaFinal();
        _AtivaValidacaoEscalasFilhos();
    }

    private void _UCComboEscalaAvaliacao_Esa_idDocente__OnSelectedIndexChange()
    {
        int esa_tipo;
        int esa_id = -1;

        if (_UCComboEscalaAvaliacao_Esa_idDocente.Valor[1] == -1)
            esa_tipo = -1;
        else
        {
            esa_id = _UCComboEscalaAvaliacao_Esa_idDocente.Valor[0];
            esa_tipo = _UCComboEscalaAvaliacao_Esa_idDocente.Valor[1];
        }

        if (esa_tipo == 1) //numerico
        {
            _lblValorMinimoAprovacaoDocente.Visible = true;
            _txtValorMinimoAprovacaoDocente.Visible = true;
            UCComboEscalaAvaliacaoParecerDocente.Visible = false;
        }
        else if (esa_tipo == 2) //parecer
        {
            UCComboEscalaAvaliacaoParecerDocente._Combo.Items.Clear();
            UCComboEscalaAvaliacaoParecerDocente._VS_isa_id = esa_id;
            UCComboEscalaAvaliacaoParecerDocente._MostrarMessageSelecione = true;
            UCComboEscalaAvaliacaoParecerDocente._Combo.DataBind();

            UCComboEscalaAvaliacaoParecerDocente.Visible = true;
            _lblValorMinimoAprovacaoDocente.Visible = false;
            _txtValorMinimoAprovacaoDocente.Visible = false;
        }
        else if (esa_tipo == 3) //relatorio
        {
            UCComboEscalaAvaliacaoParecerDocente.Visible = false;
            _lblValorMinimoAprovacaoDocente.Visible = false;
            _txtValorMinimoAprovacaoDocente.Visible = false;

            _txtValorMinimoAprovacaoDocente.Text = string.Empty;
        }
        else
        {
            UCComboEscalaAvaliacaoParecerDocente.Visible = false;
            _lblValorMinimoAprovacaoDocente.Visible = false;
            _txtValorMinimoAprovacaoDocente.Visible = false;

            _txtValorMinimoAprovacaoDocente.Text = string.Empty;
        }

        _AtivaValidacaoEscalasFilhos();
        
    }

    #endregion Métodos

    #region Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference("~/Includes/JS-ModuloAcademico.js"));
        }

        if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EFETIVACAO_PERMITIR_ALTERAR_RESULTADO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            cpvCriterioAprovacaoResultadoFinal.ValidationGroup = "";
            lblCriterioAprovacaoResultadoFinal.Text = lblCriterioAprovacaoResultadoFinal.Text.Replace(" *", "");
            cpvCriterioAprovacaoResultadoFinal.Visible = false;
        }

        if (!IsPostBack)
        {
            try
            {
                _CarregaComboAvaliacao();
                _CarregaComboEscalaAvaliacao();

                _UCComboTipoPeriodoCalendario.Obrigatorio = true;
                _UCComboTipoPeriodoCalendario.ValidationGroup = "_ValidationAvaliacao";
                _UCComboTipoPeriodoCalendario.CarregarTipoPeriodoCalendario();
                _UCComboTipoPeriodoCalendario.ExibeCombo = false;
                SetaCheckboxesExibicaoBoletim(true);
                _MostraCamposTipo();
                _MostraCamposTipoProgressaoParcial();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }

            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
            {
                _Carregar(PreviousPage.EditItem);
                Page.Form.DefaultFocus = _txtNomeAvaliacao.ClientID;
            }
            else
            {
                _ckbBloqueado.Visible = false;
                chkCalcularMediaAvaliacaoFinal.Visible = false;

                _dgvAvaliacao.DataSource = new DataTable();
                _dgvAvaliacao.DataBind();
             

                chkCalcularMediaAvaliacaoFinal_CheckedChanged(chkCalcularMediaAvaliacaoFinal, null);

                Page.Form.DefaultFocus = _txtNomeFormatoAvaliacao.ClientID;
            }
            
            _UCComboEscalaAvaliacao_Esa_idDocente__OnSelectedIndexChange();
        }

        _cpvMinPorDisciplina.ErrorMessage = "Valor mínimo para aprovação por " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " deve ser maior que 0.";
        _rfvValorMinimoDisciplina.ErrorMessage = "Valor mínimo para aprovação por " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " é obrigatório.";
        cvMinPorgressaoParcial.ErrorMessage = "Valor mínimo de progressão parcial por " + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " deve ser maior que 0.";

        _cpvMinPorDocente.ErrorMessage = "Valor mínimo para aprovação pelo docente deve ser maior que 0.";
        //_rfvValorMinimoDocente.ErrorMessage = "Valor mínimo para aprovação pelo docente é obrigatório.";

        _UCComboEscalaAvaliacao_Esa_idConceitoGlobal.IndexChanged += _Combo_EscalaAvaliacaoConceitoGlobal__OnSelectedIndexChange;
        _UCComboEscalaAvaliacao_Esa_idPorDisciplina.IndexChanged += _Combo_EscalaAvaliacaoPorDisciplina__OnSelectedIndexChange;
        _UCComboEscalaAvaliacao_Esa_idDocente.IndexChanged += _UCComboEscalaAvaliacao_Esa_idDocente__OnSelectedIndexChange;
    }

    #endregion Page Life Cycle

    #region Eventos

    protected void ddlTipoFormatoAvaliacao_SelectedIndexChanged(object sender, EventArgs e)
    {
        _LimpaTelaTipoAvaliacao();
        _MostraCamposTipo();
        _MostraCamposTipoProgressaoParcial();
        _AtivaValidacaoEscalas(Convert.ToInt32(ddlTipoFormatoAvaliacao.SelectedValue));
        _AtivaValidacaoEscalasFilhos();
        chkEfetivacaoDocente.Checked = false;
        chkObrigatorioRelatorio.Checked = false;
        chkEfetivacaoDocente.Visible =
            (ACA_FormatoAvaliacaoTipo)Convert.ToInt32(ddlTipoFormatoAvaliacao.SelectedValue) == ACA_FormatoAvaliacaoTipo.ConceitoGlobal ||
            (ACA_FormatoAvaliacaoTipo)Convert.ToInt32(ddlTipoFormatoAvaliacao.SelectedValue) == ACA_FormatoAvaliacaoTipo.GlobalDisciplina;
        chkObrigatorioRelatorio.Visible =
            (ACA_FormatoAvaliacaoTipo)Convert.ToInt32(ddlTipoFormatoAvaliacao.SelectedValue) == ACA_FormatoAvaliacaoTipo.ConceitoGlobal ||
            (ACA_FormatoAvaliacaoTipo)Convert.ToInt32(ddlTipoFormatoAvaliacao.SelectedValue) == ACA_FormatoAvaliacaoTipo.GlobalDisciplina;

        CarregarComboCriterioAprovacao(Convert.ToInt32(ddlTipoFormatoAvaliacao.SelectedValue));
        chkSugerirResultadoFinalDisciplina.Checked = false;
        txtPercentualMinimoFrequenciaFinalAjustadaDisciplina.Text = String.Empty;

        ddlCriterioAprovacaoResultadoFinal_SelectedIndexChanged(null, null);
    }

    protected void ddlTipoProgressaoParcial_SelectedIndexChanged(object sender, EventArgs e)
    {
        _LimpaTelaTipoProgressaoParcial();
        _MostraCamposTipoProgressaoParcial();
    }
    
    protected void _btnCancelarAvaliacao_Click(object sender, EventArgs e)
    {
        try
        {
            _LimpaTelaAvaliacao();
            _FormataTelaAvaliacaoPorTipo();
            if (_AlteracaoAvaliacaoRelacionada(_VS_ava_id_alteracao))
            {
                _VS_AvaliacaoRelacionada = _VS_AvaliacaoRelacionada_TEMP;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroAvaliacao", "$('#divAvaliacao').dialog('close');", true);
            _uppAvaliacao.Update();
            _uppAvaliacaoRelacionada.Update();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar cancelar avaliação.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void _btnCancelarAvaliacaoRelacionada_Click(object sender, EventArgs e)
    {
        _LimpaTelaAvaliacaoRelacionada();
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CadastroAvaliacaoRelacionada", "$('#divAvaliacaoRelacionada').dialog('close');", true);
        _uppAvaliacaoRelacionada.Update();
    }

    protected void _ddlTipoAvaliacao_SelectedIndexChanged(object sender, EventArgs e)
    {
        _FormataTelaAvaliacaoPorTipo();
        _uppAvaliacaoRelacionada.Update();
    }

    protected void _dgvAvaliacao_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = int.Parse(e.CommandArgument.ToString());
        int id = Convert.ToInt32(_dgvAvaliacao.DataKeys[index].Values[1]);

        if (e.CommandName == "Editar")
        {
            _CarregarAvaliacao(id);
        }
    }

    protected void _dgvAvaliacao_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lkbAlterar = (LinkButton)e.Row.FindControl("_lkbAlterar");
            if (lkbAlterar != null)
            {
                lkbAlterar.CommandArgument = e.Row.RowIndex.ToString();
                lkbAlterar.Enabled = true;
            }
        }
    }

    protected void _dgvAvaliacaoRelacionada_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = int.Parse(e.CommandArgument.ToString());
        int id = Convert.ToInt32(_dgvAvaliacaoRelacionada.DataKeys[index].Value);

        if (e.CommandName == "Editar")
        {
            _CarregarAvaliacaoRelacionada(id, _VS_ava_id_alteracao);
        }
    }

    protected void _dgvAvaliacaoRelacionada_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lkbAlterar = (LinkButton)e.Row.FindControl("_lkbAlterar");
            if (lkbAlterar != null)
            {
                lkbAlterar.CommandArgument = e.Row.RowIndex.ToString();
                lkbAlterar.Enabled = true;
            }
        }
    }

    protected void grvAvaliacaoSecretaria_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string avs_id = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "avs_id"));
            string tpc_id = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "tpc_id"));
            string tds_id = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "tds_id"));
            bool avs_naoConstaMedia = (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "avs_naoConstaMedia")) == "True" ? true : false);
            bool avs_apareceBoletim = (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "avs_apareceBoletim")) == "True" ? true : false);

            bool permiteEditar = false;
            int iAvs_id = String.IsNullOrEmpty(avs_id) ? -1 : Convert.ToInt32(avs_id);

            WebControls_Combos_UCComboTipoPeriodoCalendario comboTipoPeriodoCalendarioSecretaria = (WebControls_Combos_UCComboTipoPeriodoCalendario)e.Row.FindControl("UCComboTipoPeriodoCalendarioSecretaria");
            if (comboTipoPeriodoCalendarioSecretaria != null)
            {
                comboTipoPeriodoCalendarioSecretaria.Obrigatorio = true;
                comboTipoPeriodoCalendarioSecretaria.ValidationGroup = "AvaliacaoSecretaria";
                comboTipoPeriodoCalendarioSecretaria.Combo_CssClass = "text30C";
                comboTipoPeriodoCalendarioSecretaria.ExibeTitulo = false;
                comboTipoPeriodoCalendarioSecretaria.CarregarTipoPeriodoCalendario();

                if (!string.IsNullOrEmpty(tpc_id))
                    comboTipoPeriodoCalendarioSecretaria.Valor = Convert.ToInt32(tpc_id);

                comboTipoPeriodoCalendarioSecretaria.PermiteEditar = permiteEditar;
            }

            WebControls_Combos_UCComboTipoDisciplina comboTipoDisciplinaSecretaria = (WebControls_Combos_UCComboTipoDisciplina)e.Row.FindControl("UCComboTipoDisciplinaSecretaria");
            if (comboTipoDisciplinaSecretaria != null)
            {
                comboTipoDisciplinaSecretaria.Obrigatorio = true;
                comboTipoDisciplinaSecretaria.ValidationGroup = "AvaliacaoSecretaria";
                comboTipoDisciplinaSecretaria.Combo_CssClass = "text30C";
                comboTipoDisciplinaSecretaria.ExibeTitulo = false;
                comboTipoDisciplinaSecretaria.CarregarNivelEnsinoTipoDisciplina();

                if (!string.IsNullOrEmpty(tds_id))
                    comboTipoDisciplinaSecretaria.Valor = Convert.ToInt32(tds_id);

                comboTipoDisciplinaSecretaria.PermiteEditar = permiteEditar;
            }

            CheckBox chkNaoConstaMedia = (CheckBox)e.Row.FindControl("chkNaoConstaMedia");
            if (chkNaoConstaMedia != null)
            {
                chkNaoConstaMedia.Checked = avs_naoConstaMedia;
                chkNaoConstaMedia.Enabled = permiteEditar;
            }

            CheckBox chkApareceBoletim = (CheckBox)e.Row.FindControl("chkApareceBoletim");
            if (chkApareceBoletim != null)
            {
                chkApareceBoletim.Checked = avs_apareceBoletim;
                chkApareceBoletim.Enabled = permiteEditar;
            }
        }
    }

    protected void _btnCancelarFormatoAvaliacao_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/FormatoAvaliacao/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void chkUtilizarAvaliacaoAdicional_CheckedChanged(object sender, EventArgs e)
    {
        _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Valor = new[] { -1, -1 };
        _UCComboEscalaAvaliacao_Esa_idConceitoGlobalAdicional.Visible = chkUtilizarAvaliacaoAdicional.Checked;
    }

    protected void chkConsideraPeriodoLetivo_CheckedChanged(object sender, EventArgs e)
    {
        _UCComboTipoPeriodoCalendario.ExibeCombo = chkConsideraPeriodoLetivo.Checked;
        _UCComboTipoPeriodoCalendario.Valor = -1;
        _uppAvaliacao.Update();
    }

    protected void chkBaseadaConceitoGlobal_CheckedChanged(object sender, EventArgs e)
    {
        if (_ddlTipoAvaliacao.SelectedValue == ((byte)AvaliacaoTipo.RecuperacaoFinal).ToString())
        {
            divConceitoMaximo.Visible = chkBaseadaConceitoGlobal.Checked;
            ddlConceitoMaximo.SelectedValue = "0";
            _uppAvaliacao.Update();
        }
    }

    protected void ddlCriterioAprovacaoResultadoFinal_SelectedIndexChanged(object sender, EventArgs e)
    {
        // se o criterio de aprovacao for Frequência final ajustada da disciplina,
        // exibir o check que indica se sugere o parecer final automático
        // e exibir o textBox para configuração da frequência mínima
        lblPercentualMinimoFrequenciaFinalAjustadaDisciplina.Visible =
            txtPercentualMinimoFrequenciaFinalAjustadaDisciplina.Visible =
            rfvPercentualMinimoFrequenciaFinalAjustadaDisciplina.Enabled =
            cvFrequenciaFinalAjustadaMin.Enabled =
            cvFrequenciaFinalAjustadaMax.Enabled =
            chkSugerirResultadoFinalDisciplina.Visible = ddlCriterioAprovacaoResultadoFinal.SelectedValue == "6";
    }

    protected void chkCalcularMediaAvaliacaoFinal_CheckedChanged(object sender, EventArgs e)
    {
        divFormulaCalculoMediaFinal.Visible = chkCalcularMediaAvaliacaoFinal.Checked &&
            ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.HABILITAR_PESO_AVALIACOESPERIODICAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        var Pesos =
            (from DataRow dr in _VS_Avaliacao.Select().Where(p => p.RowState != DataRowState.Deleted)
             where
             Convert.ToByte(dr["ava_tipo"]) == (byte)AvaliacaoTipo.Periodica
             select Convert.ToDecimal(
                    string.IsNullOrEmpty(dr["ava_peso"].ToString())
                    ? "0" : dr["ava_peso"].ToString())
                ).ToList();

        if (divFormulaCalculoMediaFinal.Visible)
        {
            rdlTipoCalculoMediaFinal.SelectedValue =
                // Se os pesos forem = 100%, seleciona a opção "soma".
                (Pesos.Count() > 0 && Pesos.Count() == Pesos.Count(p=>p == 100))
                ? "2"
                : 
                // Se tiver alimentado algum peso, seleciona média por peso.
                ((Pesos.Count() > 0 && Pesos.Max() > 0)
                    ? "1" : "3");
            }
        
        SetaFormula();
    }

    protected void rdlTipoCalculoMediaFinal_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Altera todos os pesos pra 100.
        foreach (DataRow dr in _VS_Avaliacao.Rows)
        {
            if (Convert.ToByte(dr["ava_tipo"]) == (byte)AvaliacaoTipo.Periodica
                || Convert.ToByte(dr["ava_tipo"]) == (byte)AvaliacaoTipo.PeriodicaFinal)
            {
                dr["ava_peso"] = rdlTipoCalculoMediaFinal.SelectedValue == "2"
                    ? 100
                    : 0;
            }
        }

        SetaFormula();
    }

    protected void chkFechamentoAutomatico_CheckedChanged(object sender, EventArgs e)
    {
        chkSugerirResultadoFinalDisciplina.Enabled = !chkFechamentoAutomatico.Checked;
        if (!chkSugerirResultadoFinalDisciplina.Enabled)
        {
            chkSugerirResultadoFinalDisciplina.Checked = false;
        }
    }

    #endregion Eventos
}