USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	EXEC MS_InsereResource 
		@rcr_chave = 'Escola.Cadastro.ckbTerceirizada.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Escola terceirizada'

	EXEC MS_InsereResource 
		@rcr_chave = 'CalendarioAnual.Cadastro.ckbPermiteRecesso.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Permitir lançamento no recesso'

	EXEC MS_InsereResource 
		@rcr_chave = 'RecursosHumanos.Cargo.Cadastro.ckbControladoPelaIntegracao.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Controlado pela integração'
		
	EXEC MS_InsereResource 
		@rcr_chave = 'MSG_RODAPEBOLETIMCOMPLETOInfantil' 
		, @rcr_NomeResource = 'Mensagens'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'O boletim do aluno pode ser acessado pela internet através do endereço: <a target="_blank" href="http://infanciaonline.sme.prefeitura.sp.gov.br">http://infanciaonline.sme.prefeitura.sp.gov.br</a>. Pais/responsáveis, para entrar utilizem usuário: código EOL descrito acima e senha: data de nascimento do estudante.'

	EXEC MS_InsereResource 
		@rcr_chave = 'RecursosHumanos.Docente.Cadastro.lblMensagemFormacao.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Estou ciente de que o docente deve ser formado em pedagogia.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Areas.Documentos.lblNivelEnsino.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Nível de ensino'
		
	EXEC MS_InsereResource 
		@rcr_chave = 'RecursosHumanos.AtribuicaoDocentes.Busca._dgvTurma.HeaderText.Titular' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Titular'

	EXEC MS_InsereResource 
		@rcr_chave = 'RecursosHumanos.AtribuicaoDocentes.Busca._dgvTurma.HeaderText.SegundoTitular' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Segundo titular'

	EXEC MS_InsereResource 
		@rcr_chave = 'ControleTurma.DiarioClasse.MensagemEfetivadoRecesso' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'O período de fechamento do recesso está encerrado. Os registros ficam bloqueados, com exceção do plano de aula e das anotações dos alunos.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblTipoClassificacaoEscola.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Tipo de classificação de escola:'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblVigenciaInicial.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Vigência inicial'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.rfvVigenciaInicial.ErrorMessage' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Vigência inicial é obrigatório.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblVigenciaFinal.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Vigência final'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.cvDatas.ErrorMessage' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Vigência final deve ser maior ou igual a vigência inicial.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.btnAdicionar.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Adicionar'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.btnCancelarAdicao.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.gdvCargos.tvi_nome.HeaderText' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Tipo de vínculo'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.gdvCargos.lblCargo.HeaderText' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Cargo'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.gdvCargos.tcc_vigenciaInicial.HeaderText' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Vigência inicial'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.gdvCargos.lblVigenciaFinal.HeaderText' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Vigência final'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.gdvCargos.btnEditar.HeaderText' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Editar'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.gdvCargos.btnExcluir.HeaderText' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.btnSalvar.HeaderText' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Salvar'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.btnCancelar.HeaderText' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.ErroCarregarSistema' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Erro ao tentar carregar o sistema.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.ErroSalvarCargosAtribuicaoEsporadica' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Erro ao tentar salvar cargos para atribuição esporádica.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.CargoAtribuicaoEsporadicaAdicionadoSucesso' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Cargo para atribuição esporádica adicionado com sucesso.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.CargoAtribuicaoEsporadicaAlteradoSucesso' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Cargo para atribuição esporádica alterado com sucesso.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.ErroAdicionarCargoAtribuicaoEsporadica' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Erro ao tentar adicionar cargo para atribuição esporádica.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.ErroCarregarCargoAtribuicaoEsporadica' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Erro ao tentar carregar cargos para atribuição esporádica.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.CargoAtribuicaoEsporadicaExcluidoSucesso' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Cargo para atribuição esporádica excluído com sucesso.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.CargoAtribuicaoEsporadicaErroExcluir' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Erro ao tentar excluir o cargo para atribuição esporádica.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.CargoAtribuicaoEsporadicaErroEditar' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Erro ao tentar editar o cargo para atribuição esporádica.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.cvDataInicio.ErrorMessage' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Vigência inicial inválida.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.cvDataFim.ErrorMessage' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Vigência final inválida.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.ValidacaoVigenciaInicialDeveSerMaiorVigenciaFinal' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'A vigência inicial deve ser maior que a vigência final do registro que possui o mesmo cargo.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.JaExisteCargoVigenciaConflitante' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Já existe o mesmo cargo cadastrado com a vigência conflitante.'

	EXEC MS_InsereResource 
		@rcr_chave = 'TipoClassificacaoEscola.Cargos.lblMensagem.ValidacaoVigenciaInicialMaiorIgualInformada' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Já existe um cargo cadastrado com a vigência inicial maior ou igual a informada.'

	EXEC MS_InsereResource 
		@rcr_chave = 'MSG_PLANO_POLITICO_PEDAGOGICO' 
		, @rcr_NomeResource = 'Mensagens'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Plano Político e Pedagógico'
		
	EXEC MS_InsereResource 
		@rcr_chave = 'AtribuicaoEsporadica.Cadastro.lblMensagemDocenteNaoEncontradoCargoNaoPermitido.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'O cargo do docente não permite atribuição na escola selecionada.'

	EXEC MS_InsereResource
		@rcr_chave = 'RecursosHumanos.AtribuicaoDocentes.Busca.divConfirmacao.MensagemResponsavel.text', -- varchar(400)
		@rcr_NomeResource = 'Academico', -- varchar(100)
		@rcr_cultura = 'pt-br', -- varchar(10)
		@rcr_valorPadrao = 'Estou ciente de que o docente deve ser formado em pedagogia.'
		
	EXEC MS_InsereResource 
		@rcr_chave = 'UCDadosBoletim.lblEI.Text' 
		, @rcr_NomeResource = 'UserControl'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Ensino Infantil'
		
	EXEC MS_InsereResource 
		@rcr_chave = 'UCAlunoEfetivacaoObservacaoGeral.lblPorcentagemFreqEI.Text' 
		, @rcr_NomeResource = 'UserControl'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = '% Freq.'
		
	EXEC MS_InsereResource 
		@rcr_chave = 'RHU_Colaborador.ValidacaoCargoTipoClassificacao' 
		, @rcr_NomeResource = 'BLL'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'O cargo do docente não permite atribuição na escola selecionada.'
		
	EXEC MS_InsereResource 
		@rcr_chave = 'Areas.Documentos.lblDescricao.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Descrição *'
		
	EXEC MS_InsereResource 
		@rcr_chave = 'Areas.Documentos.lblLinkArquivo.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Link / Arquivo *'






	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.litCadastroPermissao.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Cadastro de permissões específicas'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.litOperacao.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Operação'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.litConsulta.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Consulta'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.litInserir.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Inclusão'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.litEditar.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Alteração'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.litExcluir.Text' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Exclusão'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao1' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Histórico escolar - dados aluno'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao2' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Histórico escolar - ensino fundamental'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao3' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Histórico escolar - EJA'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao4' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Histórico escolar - transferência'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao5' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Histórico escolar - informações complementares'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao6' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Diário de classe - exclusão de aulas'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao7' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Diário de classe - anotações do aluno'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao8' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Fechamento - visualização de observações'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao9' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Fechamento - exibição da aba parecer conclusivo'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao10' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Fechamento - exibição da aba justificativa pós conselho'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao11' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Fechamento - exibição da aba desempenho aprendizagem'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao12' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Fechamento - exibição da aba recomendação aluno'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao13' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Fechamento - exibição da aba recomendação responsável'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao14' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Fechamento - exibição da aba anotações aluno'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao15' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Diário de classe - lançamento de frequência'

	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.Operacao16' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Diário de classe - lançamento de frequência - infantil'


	EXEC MS_InsereResource 
		@rcr_chave = 'PermissoesEspecificas.Cadastro.grvOperacoes.EmptyDataText' 
		, @rcr_NomeResource = 'Configuracao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Não existem operações cadastradas.'

	EXEC MS_InsereResource 
		@rcr_chave = 'litNomeAluno.Text.Responsavel' 
		, @rcr_NomeResource = 'AreaAluno.MasterPageAluno'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Responsável por'

	EXEC MS_InsereResource 
		@rcr_chave = 'litNomeAluno.Text.Aluno' 
		, @rcr_NomeResource = 'AreaAluno.MasterPageAluno'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aluno'

	
		
-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



