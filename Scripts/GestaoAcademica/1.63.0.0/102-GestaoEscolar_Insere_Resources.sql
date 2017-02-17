USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	EXEC MS_InsereResource 
            @rcr_chave = 'ControleTurma.DiarioClasse.MensagemNaoEPossivelExcluirAula' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Não é possível excluir aulas que possuem dados lançados.'
						
	EXEC MS_InsereResource 
            @rcr_chave = 'ControleTurma.DiarioClasse.lblTipoJustificativaExclusaoAula.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Tipo de justificativa para exclusão de aulas *'

	EXEC MS_InsereResource
			@rcr_chave = 'ControleTurma.DiarioClasse.cpvTipoJustificativaExclusaoAula.ErrorMessage',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Tipo de justificativa para exclusão de aulas é obrigatório.'

	EXEC MS_InsereResource
			@rcr_chave = 'ControleTurma.DiarioClasse.lblObservacaoExclusaoAula.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Observação'

	EXEC MS_InsereResource
			@rcr_chave = 'ControleTurma.DiarioClasse.btnSalvarJustificativa.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Excluir aula'

	EXEC MS_InsereResource
			@rcr_chave = 'ControleTurma.DiarioClasse.btnSalvarJustificativa.ToolTip',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Excluir aula'

	EXEC MS_InsereResource
			@rcr_chave = 'ControleTurma.DiarioClasse.btnCancelarJustificativa.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource
			@rcr_chave = 'ControleTurma.DiarioClasse.btnCancelarJustificativa.ToolTip',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Busca.btnNovo.Text',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Incluir novo tipo de justificativa para exclusão de aulas'

	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Busca.grvTipoJustificativaExclusaoAulas.tje_nome.HeaderText',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Tipo de justificativa para exclusão de aulas'
						
	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Busca.grvTipoJustificativaExclusaoAulas.tje_codigo.HeaderText',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Código'	

	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Busca.grvTipoJustificativaExclusaoAulas.tje_situacao.HeaderText',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Situação'
			
	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Busca.grvTipoJustificativaExclusaoAulas.btnExcluir.HeaderText',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Excluir'	

	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Busca.ErroCarregarDados',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar carregar os dados.'
			
	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Busca.ErroExcluirTipoJustificativaExclusaoAulas',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar excluir o tipo de justificativa para exclusão de aulas.'

	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.lblJustificativaExclusaoAulas.Text',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Tipo de justificativa para exclusão de aulas *'

	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.rfvJustificativaExclusaoAulas.ErrorMessage',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Tipo de justificativa para exclusão de aulas é obrigatório.'
		
	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.lblCodigo.Text',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Código'	
	
	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.chkStuacao.Text',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Inativo'	
	
	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.bntSalvar.Text',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Salvar'	
	
	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.bntSalvar.ToolTip',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Salvar'	
	
	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.btnCancelar.Text',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Cancelar'
		
	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.btnCancelar.ToolTip',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.ErroCarregarDados',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar carregar os dados.'
	
	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.ErroCarregarDadosTipoJustificativaExclusaoAulas',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar carregar o tipo de justificativa para exclusão de aulas.'

	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.SucessoIncluirTipoJustificativaExclusaoAulas',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Tipo de justificativa para exclusão de aulas incluído com sucesso.'

	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.SucessoAlterarTipoJustificativaExclusaoAulas',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Tipo de justificativa para exclusão de aulas alterado com sucesso.'

	EXEC MS_InsereResource
			@rcr_chave = 'TipoJustificativaExclusaoAulas.Cadastro.ErroSalvarTipoJustificativaExclusaoAulas',
			@rcr_NomeResource = 'Configuracao',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar salvar o tipo de justificativa para exclusão de aulas.'

	EXEC MS_InsereResource
			@rcr_chave = 'ControleTurma.DiarioClasse.imgSemPlanoAula',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Plano de aula não preenchido'
		
	EXEC MS_InsereResource
			@rcr_chave = 'ControleTurma.Listao.lblAulasSemPlano',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Existe(m) aula(s) cadastrada(s) sem plano'
			
	EXEC MS_InsereResource
			@rcr_chave = 'ControleTurma.DiarioClasse.MensagemAulaSemPlanoAula',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Se esse ícone estiver ao lado de um plano de aula, significa que esse plano de aula não foi preenchido. Para regularizar, é necessário que o plano dessa aula seja preenchido.'
			
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.divCadastroJustificativaFalta.Title',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Justificativa de abono de falta'			

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.lblDataInicio.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data de início *'			
			
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.lblDataFim.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data de fim *'		

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.lblObservacao.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Observação *'

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.lblLgdJustificativaFalta.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Justificativa de abono de falta'					
			
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvJustificativaFalta.EmptyDataText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'A pesquisa não encontrou resultados.'
			
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.btnAddJustificativaFalta.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Adicionar nova justificativa de abono de falta'					

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.lblLgdDadosAluno.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Dados do aluno'				

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvJustificativaFalta.ajf_dataInicio.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data de início'			
			
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvJustificativaFalta.ajf_dataFim.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data de fim'		

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvJustificativaFalta.ajf_observacao.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Observação'			

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvJustificativaFalta.Editar.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Editar'	


	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvJustificativaFalta.Excluir.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Excluir'	

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.btnVoltar.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Voltar'	
			
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.btnSalvar.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Salvar'				
			
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.btnCancelar.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Cancelar'					
			
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.rfvDataInicio.ErrorMessage',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data de início é obrigatório.'	

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.cvDataInicio.ErrorMessage',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data de início não está no formato dd/mm/aaaa.'	

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.rfvDataFim.ErrorMessage',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data de fim é obrigatório.'	

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.cvDataFim.ErrorMessage',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data de fim não está no formato dd/mm/aaaa.'	

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.rfvObservacao.ErrorMessage',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Observação é obrigatório.'	

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.rfvObservacao.ErrorMessage',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Observação é obrigatório.'	

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.Mensagem.Erro',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar carregar os dados.'				

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.Mensagem.ErroExcluir',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar excluir a justificativa de abono de falta.'			

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.Mensagem.ErroSalvar',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao salvar justificativa de abono de falta.'	

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.Mensagem.ValidacaoData',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data de fim deve ser maior ou igual à data de início.'			

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.Mensagem.SucessoInsert',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Justificativa de abono de falta incluída com sucesso.'					

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.Mensagem.SucessoUpdate',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Justificativa de abono de falta atualizada com sucesso.'					

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.Mensagem.ValidacaoDataNascimento',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data de nascimento do aluno não está no formato dd/mm/aaaa ou é inexistente.'					
						
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.lblLgdResultados.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Resultados'						

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.lblPag.Text',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Itens por página'			

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvAluno.EmptyDataText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'A pesquisa não encontrou resultados.'			

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvAluno.alc_matriculaEstadual.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Matricula estadual'		

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvAluno.pes_nome.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Nome'					
						
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvAluno.tur_escolaUnidade.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Escola'		

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvAluno.tur_curso.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Curso'				
			
	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvAluno.tur_codigo.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Turma'			
			

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvAluno.tur_calendario.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Calendário'		

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvAluno.alu_situacao.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Situação'			

	EXEC MS_InsereResource
			@rcr_chave = 'JustificativaAbonoFalta.grvAluno.btnAbonoFalta.HeaderText',
			@rcr_NomeResource = 'Classe',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Incluir abono de falta.'	
			
	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.lblAno.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Ano'	

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.lblStatus.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Status'	


	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.btnPesquisar.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Pesquisar'	

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.btnPesquisar.ToolTip',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Pesquisar'	

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.btnLimparPesquisa.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Limpar pesquisa'	

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.btnLimparPesquisa.ToolTip',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Limpar pesquisa'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.btnNovo.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Incluir novo agendamento'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.btnNovo.ToolTip',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Incluir novo agendamento'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.Ano.HeaderText',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Ano'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.DRE.HeaderText',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Diretoria Regional de Educação'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.Escola.HeaderText',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Escola'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.DataInicial.HeaderText',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data inicial'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.DataFinal.HeaderText',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data final'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.Status.HeaderText',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Status'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.Excluir.HeaderText',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.lblAno.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Ano *'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.rfvAno.ErrorMessage',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Ano é obrigatório.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.lblDataInicial.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data inicial *'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.rfvDataInicial.ErrorMessage',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data inicial é obrigatório.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.cvDataInicial.ErrorMessage',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data inicial inválida.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.lblDataFinal.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data final'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.cvDataFinal.ErrorMessage',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data final inválida.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.btnSalvar.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Agendar'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.btnSalvar.ToolTip',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Agendar'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.btnCancelar.Text',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.btnCancelar.ToolTip',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.ErroCarregarSistema',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar carregar o sistema.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.AgendamentoExcluidoSucesso',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Agendamento excluído com sucesso.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.ErroExcluirAgendamento',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar excluir agendamento.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Busca.ErroConsultarAgendamentos',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar consultar agendamentos.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.ErroCarregarDados',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao tentar carregar a página.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.ErroCarregarDadosAgendamentos',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao carregar os dados.'
			
	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.SucessoIncluir',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Agendamento incluído com sucesso.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.SucessoAlterar',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Agendamento alterado com sucesso.'			
			
	EXEC MS_InsereResource
			@rcr_chave = 'Abertura de turmas de anos letivos anteriores incluída com sucesso.',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Agendamento alterado com sucesso.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.ErroSalvar',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Erro ao salvar agendamento.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.ValidacaoAnoMaiorIgual',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Ano deve ser maior ou igual a 2015.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.ValidacaoAnoMenorAnoAtual',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Ano deve ser menor que o ano atual.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.ValidacaoDataInicialMaiorIgualDataAtual',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data inicial deve ser maior ou igual a data atual.'

	EXEC MS_InsereResource
			@rcr_chave = 'AberturaTurmasAnosAnteriores.Cadastro.ValidacaoDataFinalMaiorIgualDataInicial',
			@rcr_NomeResource = 'Academico',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Data final deve ser maior ou igual a data inicial.'

	EXEC MS_InsereResource
			@rcr_chave = 'APIBO.BuscaBoletimEscolarDosAlunos.MensagemJustificativaAbonoFalta',
			@rcr_NomeResource = 'BLL',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Justificativa de abono em: {0}'		
		
	EXEC MS_InsereResource
			@rcr_chave = 'UCInformacoesComplementares.ddlAnoHistorico.Item0',
			@rcr_NomeResource = 'UserControl',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = '-- Selecione um ano/histórico --'		
	
	EXEC MS_InsereResource
			@rcr_chave = 'UCInformacoesComplementares.lblMessage.ErroAnoHistorico',
			@rcr_NomeResource = 'UserControl',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Para adicionar certificados é necessário ter pelo menos um histórico cadastrado.'	
			
	EXEC MS_InsereResource
			@rcr_chave = 'UCInformacoesComplementares.lblAnoHistorico.Text',
			@rcr_NomeResource = 'UserControl',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Ano/histórico *'		
			
	EXEC MS_InsereResource
			@rcr_chave = 'UCInformacoesComplementares.cpvAnoHistorico.ErrorMessage',
			@rcr_NomeResource = 'UserControl',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Ano/histórico é obrigatório.'		
			
	EXEC MS_InsereResource
			@rcr_chave = 'UCInformacoesComplementares.lblNumero.Text',
			@rcr_NomeResource = 'UserControl',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Nº *'			
							
	EXEC MS_InsereResource
			@rcr_chave = 'UCInformacoesComplementares.lblFolha.Text',
			@rcr_NomeResource = 'UserControl',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Folha *'					
			
	EXEC MS_InsereResource
			@rcr_chave = 'UCInformacoesComplementares.lblLivro.Text',
			@rcr_NomeResource = 'UserControl',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Livro *'	

	EXEC MS_InsereResource
			@rcr_chave = 'UCInformacoesComplementares.lblGdae.Text',
			@rcr_NomeResource = 'UserControl',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Nº GDAE *'	

	EXEC MS_InsereResource
			@rcr_chave = 'UCInformacoesComplementares.grvCertificado.HeaderTextColumnAnoHistorico',
			@rcr_NomeResource = 'UserControl',
			@rcr_cultura = 'pt-BR',
			@rcr_valorPadrao = 'Ano/histórico'	
			
-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



