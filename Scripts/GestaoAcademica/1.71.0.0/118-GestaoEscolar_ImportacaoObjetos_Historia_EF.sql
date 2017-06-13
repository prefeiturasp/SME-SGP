USE [GestaoPedagogica]
GO

DECLARE @cal_ano INT = 2017
DECLARE @tds_nome VARCHAR(100) = 'História'
DECLARE @nivelEnsino VARCHAR(100) = 'Ensino Fundamental'

DECLARE @tbObjetos TABLE
(
	ordemEixo INT
	, ordemSubEixo INT
	, ordemObjeto INT
	, descricaoEixo VARCHAR(500)
	, descricaoSubEixo VARCHAR(500)
	, descricaoObjeto VARCHAR(500)
)

DECLARE @ordemEixo INT = 1
DECLARE @ordemSubEixo INT = 1
DECLARE @ordemObjeto INT = 0
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'As pessoas e os grupos que compõem a cidade e o município', NULL, 'O “Eu”, o “Outro” e os diferentes grupos sociais e étnicos que compõem a cidade: os desafios sociais, culturais e ambientais da cidade em que se vive')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'As pessoas e os grupos que compõem a cidade e o município', NULL, 'Os patrimônios históricos e culturais da cidade em que se vive')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O lugar em que se vive', NULL, 'A produção dos marcos da memória: os lugares de memória (ruas, praças, escolas, monumentos, museus etc.)')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O lugar em que se vive', NULL, 'A produção dos marcos da memória: formação cultural da população')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O lugar em que se vive', NULL, 'A produção dos marcos da memória: a cidade e o campo, aproximações e diferenças')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A noção de espaço público e privado', NULL, 'A cidade e seus espaços: espaços públicos e espaços domésticos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A noção de espaço público e privado', NULL, 'A cidade e suas atividades: trabalho, cultura e lazer')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Transformações e permanências nas trajetórias dos grupos humanos', NULL, 'A ação das pessoas e grupos sociais no tempo e no espaço: grandes transformações da história da humanidade')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Transformações e permanências nas trajetórias dos grupos humanos', NULL, 'O passado e o presente: a noção de permanência e as lentas transformações sociais e culturais')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Circulação de pessoas, produtos e culturas', NULL, 'A circulação de pessoas e as transformações no meio natural')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Circulação de pessoas, produtos e culturas', NULL, 'A invenção do comércio e a circulação de produtos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Circulação de pessoas, produtos e culturas', NULL, 'As rotas terrestres, fluviais e marítimas e seus impactos para a formação de cidades e as transformações do meio natural')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Circulação de pessoas, produtos e culturas', NULL, 'O mundo da tecnologia: a integração de pessoas e as exclusões sociais e culturais')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'As questões históricas relativas às migrações', NULL, 'O surgimento da espécie humana na África e sua expansão pelo mundo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'As questões históricas relativas às migrações', NULL, 'Os processos migratórios para a formação do Brasil: os grupos indígenas, a presença portuguesa e a diáspora forçada dos africanos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'As questões históricas relativas às migrações', NULL, 'Os processos migratórios do final do século XIX e início do século XX no Brasil')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'As questões históricas relativas às migrações', NULL, 'As dinâmicas internas de migração no Brasil, a partir dos anos 1960')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Povos e culturas: meu lugar no mundo e meu grupo social', NULL, 'O que forma um povo?: da sedentarização aos primeiros povos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Povos e culturas: meu lugar no mundo e meu grupo social', NULL, 'As formas de organização social e política: a noção de Estado')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Povos e culturas: meu lugar no mundo e meu grupo social', NULL, 'O papel das religiões e da cultura para a formação dos povos antigos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Povos e culturas: meu lugar no mundo e meu grupo social', NULL, 'Cidadania, diversidade cultural e respeito às diferenças sociais, culturais e históricas')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Registros da história: linguagens e culturas', NULL, 'As tradições orais e a valorização da memória')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Registros da história: linguagens e culturas', NULL, 'O surgimento da escrita e a noção de fonte para a transmissão de saberes, culturas e histórias')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Registros da história: linguagens e culturas', NULL, 'Os patrimônios materiais e imateriais da humanidade')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'História: tempo, espaço e formas de registros', NULL, 'A questão do tempo, sincronias e diacronias: reflexões sobre o sentido das cronologias')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'História: tempo, espaço e formas de registros', NULL, 'Formas de registro da história e da produção do conhecimento histórico')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'História: tempo, espaço e formas de registros', NULL, 'As origens da humanidade, seus deslocamentos e os processos de sedentarização')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A invenção do mundo clássico e o contraponto com outras sociedades', NULL, 'Povos da Antiguidade na África (egípcios), no Oriente Médio (mesopotâmicos) e nas Américas (pré-colombianos)')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A invenção do mundo clássico e o contraponto com outras sociedades', NULL, 'O Ocidente Clássico: aspectos da cultura na Grécia e em Roma')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Lógicas de organização política', NULL, 'As noções de cidadania e política na Grécia e em Roma')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Lógicas de organização política', NULL, 'As diferentes formas de organização política na África: reinos, impérios, cidades-estados e sociedades linhageiras ou aldeias')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Lógicas de organização política', NULL, 'A passagem do mundo antigo para o mundo medieval')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Lógicas de organização política', NULL, 'A fragmentação do poder político na Idade Média')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Lógicas de organização política', NULL, 'O Mediterrâneo como espaço de interação entre as sociedades da Europa, da África e do Oriente Médio')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Trabalho e formas de organização social e cultural', NULL, 'Senhores e servos no mundo antigo e no medieval')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Trabalho e formas de organização social e cultural', NULL, 'Escravidão e trabalho livre em diferentes temporalidades e espaços (Roma Antiga, Europa medieval e África)')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Trabalho e formas de organização social e cultural', NULL, 'Lógicas comerciais na Antiguidade romana e no mundo medieval')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Trabalho e formas de organização social e cultural', NULL, 'O papel da religião cristã, dos mosteiros e da cultura na Idade Média')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Trabalho e formas de organização social e cultural', NULL, 'O papel da mulher na Grécia e em Roma, e no período medieval')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O mundo moderno e a conexão entre sociedades africanas, americanas e europeias', NULL, 'A construção da ideia de modernidade e seus impactos na concepção de História')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O mundo moderno e a conexão entre sociedades africanas, americanas e europeias', NULL, 'A ideia de “Novo Mundo” frente ao Mundo Antigo: permanências e rupturas de saberes e práticas na emergência do mundo moderno')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O mundo moderno e a conexão entre sociedades africanas, americanas e europeias', NULL, 'Saberes dos povos africanos e pré-colombianos expressos na cultura material e imaterial')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Humanismos, Renascimentos e o Novo Mundo', NULL, 'Humanismos: uma nova visão de ser humano e de mundo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Humanismos, Renascimentos e o Novo Mundo', NULL, 'Renascimentos artísticos e culturais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Humanismos, Renascimentos e o Novo Mundo', NULL, 'Reformas religiosas: a cristandade fragmentada')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Humanismos, Renascimentos e o Novo Mundo', NULL, 'As descobertas científicas e a expansão marítima')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A organização do poder e as dinâmicas do mundo colonial americano', NULL, 'A formação e o funcionamento das monarquias europeias: a lógica da centralização política e os conflitos na Europa')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A organização do poder e as dinâmicas do mundo colonial americano', NULL, 'A conquista da América: domínios e resistências')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A organização do poder e as dinâmicas do mundo colonial americano', NULL, 'Império: a grande expansão das fronteiras')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A organização do poder e as dinâmicas do mundo colonial americano', NULL, 'A estruturação dos vice-reinos nas Américas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A organização do poder e as dinâmicas do mundo colonial americano', NULL, 'Resistências, invasões e expansão na América portuguesa')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Lógicas comerciais e mercantis da modernidade', NULL, 'As lógicas mercantis e o domínio europeu sobre os mares e o contraponto Oriental')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Lógicas comerciais e mercantis da modernidade', NULL, 'As lógicas internas das sociedades africanas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Lógicas comerciais e mercantis da modernidade', NULL, 'As formas de organização das sociedades ameríndias')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Lógicas comerciais e mercantis da modernidade', NULL, 'A escravidão moderna e o tráfico de escravizados')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Lógicas comerciais e mercantis da modernidade', NULL, 'A emergência do capitalismo')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O mundo contemporâneo: o Antigo Regime em crise', NULL, 'A questão do iluminismo e da ilustração')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O mundo contemporâneo: o Antigo Regime em crise', NULL, 'As revoluções inglesas e os princípios do liberalismo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O mundo contemporâneo: o Antigo Regime em crise', NULL, 'Revolução Industrial e seus impactos na produção e circulação de povos, produtos e culturas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O mundo contemporâneo: o Antigo Regime em crise', NULL, 'Revolução Francesa e seus desdobramentos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O mundo contemporâneo: o Antigo Regime em crise', NULL, 'Rebeliões na América portuguesa: as conjurações mineira e baiana')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Os processos de independência nas Américas', NULL, 'Independência dos Estados Unidos da América')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Os processos de independência nas Américas', NULL, 'Independências na América espanhola')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Os processos de independência nas Américas', NULL, 'Os caminhos até a independência do Brasil')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Os processos de independência nas Américas', NULL, 'A tutela da população indígena, a escravidão dos negros e a tutela dos egressos da escravidão')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O Brasil no século XIX', NULL, 'Brasil: Primeiro Reinado')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O Brasil no século XIX', NULL, 'O Período Regencial e as contestações ao poder central')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O Brasil no século XIX', NULL, 'O Brasil do Segundo Reinado: política e economia')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O Brasil no século XIX', NULL, 'O escravismo no Brasil do século XIX: plantations e revoltas de escravizados, abolicionismo e políticas migratórias no Brasil Imperial')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O Brasil no século XIX', NULL, 'Políticas de extermínio do indígena durante o Império')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O Brasil no século XIX', NULL, 'A produção do imaginário nacional brasileiro: cultura popular, representações visuais, letras e o romantismo no Brasil')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Configurações do mundo no século XIX', NULL, 'Nacionalismo, revoluções e as novas nações europeias')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Configurações do mundo no século XIX', NULL, 'Uma nova ordem econômica: as demandas do capitalismo industrial e o lugar das economias africanas e asiáticas nas dinâmicas globais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Configurações do mundo no século XIX', NULL, 'Os Estados Unidos da América e a América Latina no século XIX')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Configurações do mundo no século XIX', NULL, 'O imperialismo europeu e a partilha da África e da Ásia')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Configurações do mundo no século XIX', NULL, 'Pensamento e cultura no século XIX: darwinismo e racismo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Configurações do mundo no século XIX', NULL, 'O discurso civilizatório nas Américas e a questão indígena')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'Experiências republicanas e práticas autoritárias: as tensões e disputas do mundo contemporâneo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'A proclamação da República e seus primeiros desdobramentos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'A questão da inserção dos negros no período republicano do pós-abolição')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'Os movimentos sociais e a imprensa negra; a cultura afro-brasileira como elemento de resistência e superação das discriminações')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'Primeira República e suas características')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'Contestações e dinâmicas da vida cultural no Brasil entre 1900 e 1930')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'O período varguista e suas contradições')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'A emergência da vida urbana e a segregação espacial')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'O trabalhismo e seu protagonismo político')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'A questão indígena durante a República (até 1964)')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'O nascimento da República no Brasil e os processos históricos até a metade do século XX', NULL, 'Questões de gênero, o anarquismo e protagonismos femininos')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Totalitarismos e conflitos mundiais', NULL, 'O mundo em conflito: a Primeira Guerra Mundial')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Totalitarismos e conflitos mundiais', NULL, 'A questão da Palestina')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Totalitarismos e conflitos mundiais', NULL, 'A Revolução Russa')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Totalitarismos e conflitos mundiais', NULL, 'A crise capitalista de 1929')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Totalitarismos e conflitos mundiais', NULL, 'A emergência do fascismo e do nazismo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Totalitarismos e conflitos mundiais', NULL, 'A Segunda Guerra Mundial')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Totalitarismos e conflitos mundiais', NULL, 'O colonialismo na África')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Totalitarismos e conflitos mundiais', NULL, 'As guerras mundiais, a crise do colonialismo e o advento dos nacionalismos africanos e asiáticos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Totalitarismos e conflitos mundiais', NULL, 'A Organização das Nações Unidas (ONU) e a questão dos Direitos Humanos')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Modernização, ditadura civil-militar e redemocratização: o Brasil após 1946', NULL, 'O Brasil da era JK e o ideal de uma nação moderna: a urbanização e seus desdobramentos em um país em transformação')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Modernização, ditadura civil-militar e redemocratização: o Brasil após 1946', NULL, 'Os anos 1960: revolução cultural?')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Modernização, ditadura civil-militar e redemocratização: o Brasil após 1946', NULL, 'A ditadura civil-militar e os processos de resistência')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Modernização, ditadura civil-militar e redemocratização: o Brasil após 1946', NULL, 'A questão indígena e a ditadura')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Modernização, ditadura civil-militar e redemocratização: o Brasil após 1946', NULL, 'O processo de redemocratização')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Modernização, ditadura civil-militar e redemocratização: o Brasil após 1946', NULL, 'A Constituição de 1988 e a emancipação das cidadanias (analfabetos, indígenas, jovens etc.)')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A história recente', NULL, 'A Guerra Fria: confrontos de dois modelos políticos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A história recente', NULL, 'A Revolução Chinesa e as tensões entre China e Rússia')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A história recente', NULL, 'A Revolução Cubana e as tensões entre Estados Unidos da América e Cuba')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A história recente', NULL, 'As experiências ditatoriais na América do Sul')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A história recente', NULL, 'Os processos de descolonização na África e na Ásia')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A história recente', NULL, 'O fim da Guerra Fria e o processo de globalização')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A história recente', NULL, 'Os conflitos do século XXI e a questão do terrorismo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'A história recente', NULL, 'Pluralidades e diversidades identitárias na atualidade')

-- Encontra a disciplina
DECLARE @tbDisciplina TABLE
(
	tds_id INT
)
IF (@nivelEnsino IS NULL)
BEGIN
	INSERT INTO @tbDisciplina
	SELECT tds.tds_id
	FROM ACA_TipoDisciplina tds WITH(NOLOCK)
	WHERE tds.tds_nome = @tds_nome AND tds.tds_situacao <> 3
END
ELSE 
BEGIN
	INSERT INTO @tbDisciplina
	SELECT tds.tds_id
	FROM ACA_TipoDisciplina tds WITH(NOLOCK)
	INNER JOIN ACA_TipoNivelEnsino tne WITH(NOLOCK)
		ON tne.tne_id = tds.tne_id
		AND tne.tne_nome = @nivelEnsino
		AND tne.tne_situacao <> 3
	WHERE tds.tds_nome = @tds_nome AND tds.tds_situacao <> 3
END

DECLARE @tds_id INT
WHILE (EXISTS (SELECT TOP 1 1 FROM @tbDisciplina))
BEGIN
	
	SELECT TOP 1 @tds_id = tds_id
	FROM @tbDisciplina

	DECLARE @dataAtual DATETIME = GETDATE();

	/*
	-- Apaga os registros existentes
	UPDATE ACA_ObjetoAprendizagem
	SET oap_situacao = 3, oap_dataAlteracao = @dataAtual
	WHERE tds_id = @tds_id AND cal_ano = @cal_ano AND oap_situacao <> 3

	UPDATE ACA_ObjetoAprendizagemEixo
	SET oae_situacao = 3, oae_dataAlteracao = @dataAtual
	WHERE tds_id = @tds_id AND cal_ano = @cal_ano AND oae_situacao <> 3
	--
	*/

	-- Verifica se os objetos de aprendizagem ainda não foram cadastrados
	-- para a disciplina e ano.
	IF (NOT EXISTS (
		SELECT TOP 1 1 
		FROM ACA_ObjetoAprendizagem WITH(NOLOCK) 
		WHERE tds_id = @tds_id AND cal_ano = @cal_ano AND oap_situacao <> 3
		))
	BEGIN
		--Iniciar transação
		BEGIN TRANSACTION
		SET XACT_ABORT ON

			-- Cadastra os eixos
			INSERT INTO ACA_ObjetoAprendizagemEixo (tds_id, oae_descricao, oae_ordem, oae_situacao, oae_dataCriacao, oae_dataAlteracao, cal_ano)
			SELECT 
				@tds_id
				, descricaoEixo
				, ordemEixo
				, 1
				, @dataAtual
				, @dataAtual
				, @cal_ano
			FROM @tbObjetos
			WHERE descricaoEixo IS NOT NULL
			GROUP BY ordemEixo, descricaoEixo
			ORDER BY ordemEixo

			-- Cadastra os subeixos
			;WITH Subeixos AS
			(
				SELECT ordemEixo, descricaoEixo, ordemSubEixo, descricaoSubEixo
				FROM @tbObjetos
				WHERE descricaoSubEixo IS NOT NULL
				GROUP BY ordemEixo, descricaoEixo, ordemSubEixo, descricaoSubEixo
			)
			INSERT INTO ACA_ObjetoAprendizagemEixo (tds_id, oae_descricao, oae_ordem, oae_idPai, oae_situacao, oae_dataCriacao, oae_dataAlteracao, cal_ano)
			SELECT 
				@tds_id
				, descricaoSubEixo
				, ordemSubEixo
				, CASE WHEN descricaoEixo IS NOT NULL THEN
					(
						SELECT TOP 1 oae_id
						FROM ACA_ObjetoAprendizagemEixo
						WHERE 
						tds_id = @tds_id 
						AND cal_ano = @cal_ano
						AND oae_descricao = descricaoEixo
						AND oae_ordem = ordemEixo
						AND oae_situacao = 1
					)
					ELSE NULL END
				, 1
				, @dataAtual
				, @dataAtual
				, @cal_ano
			FROM Subeixos
			ORDER BY ordemEixo, ordemSubEixo

			-- Cadastra os objetos
			INSERT INTO ACA_ObjetoAprendizagem (tds_id, oap_descricao, oap_situacao, oap_dataCriacao, oap_dataAlteracao, cal_ano, oae_id)
			SELECT 
				@tds_id
				, descricaoObjeto
				, 1
				, @dataAtual
				, @dataAtual
				, @cal_ano
				, CASE WHEN descricaoSubEixo IS NOT NULL THEN
					(
						SELECT TOP 1 oae_id
						FROM ACA_ObjetoAprendizagemEixo
						WHERE 
						tds_id = @tds_id 
						AND cal_ano = @cal_ano
						AND oae_descricao = descricaoSubEixo
						AND oae_ordem = ordemSubEixo
						AND oae_situacao = 1
					)
					ELSE
						CASE WHEN descricaoEixo IS NOT NULL THEN
						(
							SELECT TOP 1 oae_id
							FROM ACA_ObjetoAprendizagemEixo
							WHERE 
							tds_id = @tds_id 
							AND cal_ano = @cal_ano
							AND oae_descricao = descricaoEixo
							AND oae_ordem = ordemEixo
							AND oae_situacao = 1
						)
						ELSE NULL END 			
					END
			FROM @tbObjetos
			WHERE descricaoObjeto IS NOT NULL
			ORDER BY ordemObjeto

			-- Cadastra os tipos de ciclo dos objetos
			INSERT INTO ACA_ObjetoAprendizagemTipoCiclo (oap_id, tci_id)
			SELECT oap.oap_id, tci.tci_id
			FROM @tbObjetos obj
			INNER JOIN ACA_ObjetoAprendizagem oap
				ON oap.tds_id = @tds_id 
				AND oap.cal_ano = @cal_ano
				AND oap.oap_descricao = obj.descricaoObjeto
				AND oap.oap_situacao = 1
				AND 
				(
					(oap.oae_id IS NULL AND obj.descricaoSubEixo IS NULL AND obj.descricaoEixo IS NULL)
					OR 
					(
						obj.descricaoSubEixo IS NOT NULL
						AND oap.oae_id = 
						(
							SELECT TOP 1 oae_id
							FROM ACA_ObjetoAprendizagemEixo
							WHERE 
							tds_id = @tds_id 
							AND cal_ano = @cal_ano
							AND oae_descricao = obj.descricaoSubEixo
							AND oae_ordem = obj.ordemSubEixo
							AND oae_situacao = 1 
						)
					)
					OR 
					(
						obj.descricaoEixo IS NOT NULL
						AND oap.oae_id = 
						(
							SELECT TOP 1 oae_id
							FROM ACA_ObjetoAprendizagemEixo
							WHERE 
							tds_id = @tds_id 
							AND cal_ano = @cal_ano
							AND oae_descricao = obj.descricaoEixo
							AND oae_ordem = obj.ordemEixo
							AND oae_situacao = 1 
						)
					)
				)
			LEFT JOIN ACA_TipoCiclo tci WITH(NOLOCK) 
				ON tci.tci_situacao = 1

		-- Fechar transação	
		SET XACT_ABORT OFF
		COMMIT TRANSACTION
	END	
	ELSE
	BEGIN
		PRINT 'Já existem objetos de conhecimento cadastrados para a disciplina.'
	END 
	
	DELETE TOP(1)
	FROM @tbDisciplina
END