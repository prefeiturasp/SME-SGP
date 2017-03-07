USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	/***************
		Copiar do exemplo abaixo.
	****************

        -- Insere resources. 
        EXEC MS_InsereResource 
            @rcr_chave = 'Relatorios.UCRelatorios.lblMessageLayout.MsgAviso' 
            , @rcr_NomeResource = 'WebControls'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'A visualização do texto na tela abaixo não corresponde, necessariamente, ao formato no qual ele será impresso. Este formato segue as normas estabelecidas pela Secretaria Municipal de Educação.'

	*/

	EXEC MS_InsereResource 
            @rcr_chave = 'UCAlunoEfetivacaoObservacaoGeral.lblFaltasExternas.Text' 
            , @rcr_NomeResource = 'UserControl'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Falta outra rede'


	EXEC MS_InsereResource 
            @rcr_chave = 'EfetivacaoNotas.UCEfetivacaoNotas.btnFaltasExternas.ToolTip'
            , @rcr_NomeResource = 'UserControl'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Exibir ausências de outras redes'

	EXEC MS_InsereResource 
            @rcr_chave = 'EfetivacaoNotas.UCEfetivacaoNotas.divFrequenciaExterna.title'
            , @rcr_NomeResource = 'UserControl'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Ausências de outras redes'

	EXEC MS_InsereResource 
		@rcr_chave = 'lit_9.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Lançamento de ausência em outras redes'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblNomeAlunoTitulo.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nome do aluno: '
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblTurmaTitulo.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Turma: '
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblNumeroChamadaTitulo.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'N° de chamada: '
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblCodigoEolTitulo.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Código EOL: '
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblQtdAulasPrevistas.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblQtdAulas.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aulas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblQtdFaltas.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Faltas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'imgAvisoAulasPrevistas.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de faltas maior que quantidade de aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'imgAvisoAulasPrevistas.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de faltas maior que quantidade de aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblQtdAulasPrevistas.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblQtdAulas.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aulas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblQtdFaltas.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Faltas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'imgAvisoAulasPrevistas.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de faltas maior que quantidade de aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'imgAvisoAulasPrevistas.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de faltas maior que quantidade de aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'imgAvisoAulasPrevistas.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de faltas maior que quantidade de aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'imgAvisoAulasPrevistas.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de faltas maior que quantidade de aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblEI.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Ensino Infantil'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblQtdAulasPrevistas.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblQtdAulas.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aulas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblQtdFaltas.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Faltas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'imgAvisoAulasPrevistas.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de faltas maior que quantidade de aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'imgAvisoAulasPrevistas.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de faltas maior que quantidade de aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'ctrl_394.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de faltas maior que quantidade de aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lit2.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de faltas maior que quantidade de aulas previstas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Salvar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Salvar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Cancelar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'pnlPesquisa.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Busca'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Lançamento de ausência em outras redes'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'chkTurmaExtinta.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Busca'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Marque essa opção para acessar turmas extintas'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Pesquisar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Pesquisar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.LimparPesquisa.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Limpar pesquisa'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Resultados.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Resultados'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.SemResultado.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'A pesquisa não encontrou resultados.'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'ctrl_45.HeaderText' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.LancamentoFrequenciaExterna.Busca'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nº'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Nome.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nome'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.DataNascimento.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data de nascimento'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.NomeMae.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nome da mãe'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Matricula.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Matrícula'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.DataMatricula.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data de matrícula'		
		
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Voltar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Voltar'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoHorarioBO.ACA_TurnoHorarioTipo.AulaForaPeriodo' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aula fora do período do turno'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoHorarioBO.ACA_TurnoHorarioTipo.AulaNormal' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aula normal'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoHorarioBO.ACA_TurnoHorarioTipo.IntervaloEntreAulas' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Intervalo entre aulas'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoHorarioBO.ACA_TurnoHorarioTipo.IntervaloEntrePeriodos' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Intervalo entre períodos'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.litLegendaQuadro.Text' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quadro de horários'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.btnCancelar.Text' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.btnCancelarAtribuicao.Text' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.btnFecharAtribuicao.Text' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Fechar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.btnVoltar.Text' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Voltar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.DisciplinaNaoInformada' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Disciplina não informada'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.divAtribuirDisciplina.title' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Atribuição de disciplina ao horário'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.ErroCarregar' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar carregar os dados.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.lblDados.Text' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = '<b>Turma:</b> {0}<br /><b>Escola:</b> {1}<br /><b>Calendário:</b> {2}<br /><b>Curso:</b> {3}<br /><b>Turno:</b> {4}<br /><br />'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.lblDiaSemana.Text' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Dia da semana'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.lblHoraFinal.Text' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Hora final'

	EXEC MS_InsereResource 
		@rcr_chave = 'Turma.QuadroHorarios.lblHoraInicial.Text' 
		, @rcr_NomeResource = 'Turma'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Hora inicial'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoBO.DiasSemana.Domingo' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Domingo'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoBO.DiasSemana.Quarta' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quarta-feira'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoBO.DiasSemana.Quinta' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quinta-feira'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoBO.DiasSemana.Sabado' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sábado'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoBO.DiasSemana.Segunda' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Segunda-feira'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoBO.DiasSemana.Sexta' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sexta-feira'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_TurnoBO.DiasSemana.Terca' 
		, @rcr_NomeResource = 'Enumerador'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Terça-feira'

	EXEC MS_InsereResource 
		@rcr_chave = 'Combos.UCComboTipoHorario.lblTipoHorario.Text' 
		, @rcr_NomeResource = 'UserControl'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Tipo de horário'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



