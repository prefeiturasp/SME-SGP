using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.PermissaoDocente
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Estruturas 

        /// <summary>
        /// Classe que contém as disciplinas e suas permissões
        /// </summary>
        private struct TipoDocenteVisao
        {
            public string DescricaoTipo { get; set; }
            public byte IdTipoDocente { get; set; }
            public List<TipoDocentePermissao> DocentesPermissoesConsEdic { get; set; }
            public List<TipoDocentePermissao> DocentesPermissoesPermConsEdic { get; set; }
            public List<TipoDocentePermissao> DocentesPermissoesPermCons { get; set; }
            public List<TipoDocentePermissao> DocentesPermissoesCons { get; set; }
        }

        /// <summary>
        /// Classe que armazena as permissões de cada disciplina para visão consulta e edição
        /// </summary>
        private struct TipoDocentePermissao
        {
            public byte IdModuloPermissao { get; set; }

            public int PdcIdTitular { get; set; }
            public bool ConsultaTitular { get; set; }
            public bool EditarTitular { get; set; }

            public int PdcIdCompartilhado { get; set; }
            public bool ConsultaCompartilhado { get; set; }
            public bool EditarCompartilhado { get; set; }

            public int PdcIdProjeto { get; set; }
            public bool ConsultaProjeto { get; set; }
            public bool EditarProjeto { get; set; }

            public int PdcIdSubstituto { get; set; }
            public bool ConsultaSubstituto { get; set; }
            public bool EditarSubstituto { get; set; }

            public int PdcIdEspecial { get; set; }
            public bool ConsultaEspecial { get; set; }
            public bool EditarEspecial { get; set; }

            public int PdcIdSegundo { get; set; }
            public bool ConsultaSegundo { get; set; }
            public bool EditarSegundo { get; set; }
        }

        #endregion

        #region Propriedades

        private List<ACA_TipoDocente> lstTipoDocente;

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega as permissões dos tipos de docentes.
        /// </summary>
        private void Carregar()
        {
            try
            {
                if (lstTipoDocente == null)
                    lstTipoDocente = ACA_TipoDocenteBO.SelecionaAtivos(ApplicationWEB.AppMinutosCacheLongo);

                //Adiciona na lista o tipo do enum: Boletim e Indicadores
                List<Enum> lstEnumConsulta = new List<Enum>();
                lstEnumConsulta.Add(EnumModuloPermissao.Boletim);
                lstEnumConsulta.Add(EnumModuloPermissao.Indicadores);

                //Carrega na lista os valores da description do enum
                List<string> lstDisciplina = ObterValoresEnum(typeof(EnumModuloPermissao), lstEnumConsulta);

                //Lista os valores do description do enum, para carregar as disciplinas que contem apenas consulta.
                List<string> lstDisciplinaConsulta = new List<string>();
                foreach (var item in lstEnumConsulta)
                {
                    lstDisciplinaConsulta.Add(DescricaoEnum(item));
                }

                //Busca se já existem permissões definidas no banco
                List<CFG_PermissaoDocente> lstPermDoc = CFG_PermissaoDocenteBO.SelecionarPermissaoModulo();
                
                //Criando os dicionarios que contém as permissões para cada repeater
                Dictionary<EnumTipoDocente, List<TipoDocentePermissao>> dicPermissoesDocentesConsEdic = new Dictionary<EnumTipoDocente, List<TipoDocentePermissao>>();
                Dictionary<EnumTipoDocente, List<TipoDocentePermissao>> dicPermissoesDocentesPermConsEdic = new Dictionary<EnumTipoDocente, List<TipoDocentePermissao>>();
                Dictionary<EnumTipoDocente, List<TipoDocentePermissao>> dicPermissoesDocentesPermCons = new Dictionary<EnumTipoDocente, List<TipoDocentePermissao>>();
                Dictionary<EnumTipoDocente, List<TipoDocentePermissao>> dicPermissoesDocentesCons = new Dictionary<EnumTipoDocente, List<TipoDocentePermissao>>();
                
                //Carrega repeater rptSelecionarConsultaEdicao
                List<CFG_PermissaoDocente> lst = lstPermDoc.Where(p => p.pdc_modulo != Convert.ToByte(EnumModuloPermissao.Compensacoes) &&
                p.pdc_modulo != Convert.ToByte(EnumModuloPermissao.Efetivacao) && p.pdc_modulo != Convert.ToByte(EnumModuloPermissao.Boletim) &&
                p.pdc_modulo != Convert.ToByte(EnumModuloPermissao.Indicadores)).ToList();

                List<EnumModuloPermissao> lstEnum = new List<EnumModuloPermissao>();
                lstEnum.Add(EnumModuloPermissao.Aula);
                lstEnum.Add(EnumModuloPermissao.PlanoAula);
                lstEnum.Add(EnumModuloPermissao.Anotacoes);
                lstEnum.Add(EnumModuloPermissao.PlanejamentoAnual);
                lstEnum.Add(EnumModuloPermissao.Frequencia);
                lstEnum.Add(EnumModuloPermissao.Avaliacoes);
                
                dicPermissoesDocentesConsEdic = CarregarPermissoesDocenteModulo(lst, lstEnum);

                //Carrega repeater rptSelecionarPermissaoConsEdic
                lst.Clear();
                lst.AddRange(lstPermDoc.Where(p => p.pdc_modulo == Convert.ToByte(EnumModuloPermissao.Compensacoes)).ToList());
                lst.AddRange(lstPermDoc.Where(p => p.pdc_modulo == Convert.ToByte(EnumModuloPermissao.Efetivacao)).ToList());

                lstEnum.Clear();
                lstEnum.Add(EnumModuloPermissao.Compensacoes);
                lstEnum.Add(EnumModuloPermissao.Efetivacao);

                dicPermissoesDocentesPermConsEdic = CarregarPermissoesDocenteModulo(lst, lstEnum);
                
                //Carrega repeater rptSelecionarPermissaoCons
                lst.Clear();
                lst.AddRange(lstPermDoc.Where(p => p.pdc_modulo == Convert.ToByte(EnumModuloPermissao.Boletim)).ToList());
                
                lstEnum.Clear();
                lstEnum.Add(EnumModuloPermissao.Boletim);

                dicPermissoesDocentesPermCons = CarregarPermissoesDocenteModulo(lstPermDoc, lstEnum);
                
                //Carrega repeater rptSelecionarConsulta
                lst.Clear();
                lst = lstPermDoc.Where(p => p.pdc_modulo == Convert.ToByte(EnumModuloPermissao.Indicadores)).ToList();
                
                lstEnum.Clear();
                lstEnum.Add(EnumModuloPermissao.Indicadores);

                dicPermissoesDocentesCons = CarregarPermissoesDocenteModulo(lstPermDoc, lstEnum);
                
                List<TipoDocenteVisao> lstTpDocVisao = new List<TipoDocenteVisao>();

                //Percorre os tipos de docentes, carregando suas permissões.
                foreach (EnumTipoDocente enumValue in Enum.GetValues(typeof(EnumTipoDocente)))
                {
                    if (!lstTipoDocente.Any(p => p.tdc_id == (byte)enumValue))
                        continue;

                    switch (enumValue)
                    {
                        case EnumTipoDocente.Titular:
                            string descricaoTitular = DescricaoEnum(enumValue);

                            lstTpDocVisao.Add(new TipoDocenteVisao
                            {
                                DescricaoTipo = string.Concat(descricaoTitular, string.Concat(" ", string.Format("({0})",
                                    descricaoTitular.Substring(0, 1)))),
                                IdTipoDocente = Convert.ToByte(enumValue),
                                DocentesPermissoesConsEdic = dicPermissoesDocentesConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermConsEdic = dicPermissoesDocentesPermConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermCons = dicPermissoesDocentesPermCons.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesCons = dicPermissoesDocentesCons.FirstOrDefault(x => x.Key == enumValue).Value
                            });
                            break;
                        case EnumTipoDocente.Compartilhado:
                            string descricaoCompartilhado = DescricaoEnum(enumValue);
                            
                            lstTpDocVisao.Add(new TipoDocenteVisao
                            {
                                DescricaoTipo = string.Concat(descricaoCompartilhado, string.Concat(" ", string.Format("({0})",
                                    descricaoCompartilhado.Substring(0, 1)))),
                                IdTipoDocente = Convert.ToByte(enumValue),
                                DocentesPermissoesConsEdic = dicPermissoesDocentesConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermConsEdic = dicPermissoesDocentesPermConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermCons = dicPermissoesDocentesPermCons.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesCons = dicPermissoesDocentesCons.FirstOrDefault(x => x.Key == enumValue).Value
                            });
                            break;
                        case EnumTipoDocente.Projeto:
                            string descricaoProjeto = DescricaoEnum(enumValue);
                           
                            lstTpDocVisao.Add(new TipoDocenteVisao
                            {
                                DescricaoTipo = string.Concat(descricaoProjeto, string.Concat(" ", string.Format("({0})",
                                    descricaoProjeto.Substring(0, 1)))),
                                IdTipoDocente = Convert.ToByte(enumValue),
                                DocentesPermissoesConsEdic = dicPermissoesDocentesConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermConsEdic = dicPermissoesDocentesPermConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermCons = dicPermissoesDocentesPermCons.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesCons = dicPermissoesDocentesCons.FirstOrDefault(x => x.Key == enumValue).Value
                            });
                            break;
                        case EnumTipoDocente.Substituto:
                            string descricaoSubistituto = DescricaoEnum(enumValue);
                            
                            lstTpDocVisao.Add(new TipoDocenteVisao
                            {
                                DescricaoTipo = string.Concat(descricaoSubistituto, string.Concat(" ", string.Format("({0})",
                                    descricaoSubistituto.Substring(0, 1)))),
                                IdTipoDocente = Convert.ToByte(enumValue),
                                DocentesPermissoesConsEdic = dicPermissoesDocentesConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermConsEdic = dicPermissoesDocentesPermConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermCons = dicPermissoesDocentesPermCons.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesCons = dicPermissoesDocentesCons.FirstOrDefault(x => x.Key == enumValue).Value
                            });
                            break;
                        case EnumTipoDocente.Especial:
                            string descricaoEspecial = DescricaoEnum(enumValue);

                            lstTpDocVisao.Add(new TipoDocenteVisao
                            {
                                DescricaoTipo = string.Concat(descricaoEspecial, string.Concat(" ", string.Format("({0})",
                                    descricaoEspecial.Substring(0, 1)))),
                                IdTipoDocente = Convert.ToByte(enumValue),
                                DocentesPermissoesConsEdic = dicPermissoesDocentesConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermConsEdic = dicPermissoesDocentesPermConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermCons = dicPermissoesDocentesPermCons.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesCons = dicPermissoesDocentesCons.FirstOrDefault(x => x.Key == enumValue).Value
                            });
                            break;
                        case EnumTipoDocente.SegundoTitular:
                            string descricaoSegundo = DescricaoEnum(enumValue);

                            lstTpDocVisao.Add(new TipoDocenteVisao
                            {
                                DescricaoTipo = string.Concat(descricaoSegundo, " (ST)"),
                                IdTipoDocente = Convert.ToByte(enumValue),
                                DocentesPermissoesConsEdic = dicPermissoesDocentesConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermConsEdic = dicPermissoesDocentesPermConsEdic.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesPermCons = dicPermissoesDocentesPermCons.FirstOrDefault(x => x.Key == enumValue).Value,
                                DocentesPermissoesCons = dicPermissoesDocentesCons.FirstOrDefault(x => x.Key == enumValue).Value
                            });
                            break;
                        default:
                            break;
                    }
                }

                rptDiciplinasEdiCons.DataSource = lstDisciplina;
                rptDiciplinasEdiCons.DataBind();

                rptDisciplinasConsPerm.DataSource = lstDisciplinaConsulta;
                rptDisciplinasConsPerm.DataBind();

                rptVisaoEdicCons.DataSource = lstDisciplina;
                rptVisaoEdicCons.DataBind();
                
                rptVisaoCons.DataSource = lstDisciplinaConsulta;
                rptVisaoCons.DataBind();
                
                rptTipoDocente.DataSource = lstTpDocVisao;
                rptTipoDocente.DataBind();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar permissões do docente.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Retorna as permissões dos tipos de docentes para cada modulo
        /// </summary>
        /// <param name="lstPermDoc">Lista com as permissões</param>
        /// <param name="lstEnumModPerm">Lista com os modulos que devem ter suas permissões carregadas</param>
        /// <returns>Dicionario com as permissões definidas</returns>
        private Dictionary<EnumTipoDocente, List<TipoDocentePermissao>> CarregarPermissoesDocenteModulo
        (
            List<CFG_PermissaoDocente> lstPermDoc, 
            List<EnumModuloPermissao> lstEnumModPerm
        )
        {
            Dictionary<EnumTipoDocente, List<TipoDocentePermissao>> dic = new Dictionary<EnumTipoDocente, List<TipoDocentePermissao>>();

            //Percorre os tipos de docentes, carregando suas permissões.
            foreach (EnumTipoDocente enumValue in Enum.GetValues(typeof(EnumTipoDocente)))
            {
                switch (enumValue)
                {
                    case EnumTipoDocente.Titular:
                        dic.Add(enumValue, new List<TipoDocentePermissao>());

                        foreach (EnumModuloPermissao enumMod in lstEnumModPerm)
                        {
                            TipoDocentePermissao tipDoc = new TipoDocentePermissao { IdModuloPermissao = Convert.ToByte(enumMod) };
                            foreach (CFG_PermissaoDocente item in lstPermDoc.Where(p => p.tdc_id == Convert.ToByte(enumValue) && p.pdc_modulo == Convert.ToByte(enumMod)))
                            {
                                switch ((EnumTipoDocente)item.tdc_idPermissao)
                                {
                                    case EnumTipoDocente.Titular:
                                        tipDoc.PdcIdTitular = item.pdc_id;
                                        tipDoc.ConsultaTitular = item.pdc_permissaoConsulta;
                                        tipDoc.EditarTitular = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Compartilhado:
                                        tipDoc.PdcIdCompartilhado = item.pdc_id;
                                        tipDoc.ConsultaCompartilhado = item.pdc_permissaoConsulta;
                                        tipDoc.EditarCompartilhado = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Projeto:
                                        tipDoc.PdcIdProjeto = item.pdc_id;
                                        tipDoc.ConsultaProjeto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarProjeto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Substituto:
                                        tipDoc.PdcIdSubstituto = item.pdc_id;
                                        tipDoc.ConsultaSubstituto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSubstituto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Especial:
                                        tipDoc.PdcIdEspecial = item.pdc_id;
                                        tipDoc.ConsultaEspecial = item.pdc_permissaoConsulta;
                                        tipDoc.EditarEspecial = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.SegundoTitular:
                                        tipDoc.PdcIdSegundo = item.pdc_id;
                                        tipDoc.ConsultaSegundo= item.pdc_permissaoConsulta;
                                        tipDoc.EditarSegundo= item.pdc_permissaoEdicao;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            dic[enumValue].Add(tipDoc);
                        }
                        break;
                    case EnumTipoDocente.Compartilhado:
                        dic.Add(enumValue, new List<TipoDocentePermissao>());

                        foreach (EnumModuloPermissao enumMod in lstEnumModPerm)
                        {
                            TipoDocentePermissao tipDoc = new TipoDocentePermissao { IdModuloPermissao = Convert.ToByte(enumMod) };
                            foreach (CFG_PermissaoDocente item in lstPermDoc.Where(p => p.tdc_id == Convert.ToByte(enumValue) && p.pdc_modulo == Convert.ToByte(enumMod)))
                            {
                                switch ((EnumTipoDocente)item.tdc_idPermissao)
                                {
                                    case EnumTipoDocente.Titular:
                                        tipDoc.PdcIdTitular = item.pdc_id;
                                        tipDoc.ConsultaTitular = item.pdc_permissaoConsulta;
                                        tipDoc.EditarTitular = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Compartilhado:
                                        tipDoc.PdcIdCompartilhado = item.pdc_id;
                                        tipDoc.ConsultaCompartilhado = item.pdc_permissaoConsulta;
                                        tipDoc.EditarCompartilhado = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Projeto:
                                        tipDoc.PdcIdProjeto = item.pdc_id;
                                        tipDoc.ConsultaProjeto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarProjeto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Substituto:
                                        tipDoc.PdcIdSubstituto = item.pdc_id;
                                        tipDoc.ConsultaSubstituto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSubstituto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Especial:
                                        tipDoc.PdcIdEspecial = item.pdc_id;
                                        tipDoc.ConsultaEspecial = item.pdc_permissaoConsulta;
                                        tipDoc.EditarEspecial = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.SegundoTitular:
                                        tipDoc.PdcIdSegundo = item.pdc_id;
                                        tipDoc.ConsultaSegundo = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSegundo = item.pdc_permissaoEdicao;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            dic[enumValue].Add(tipDoc);
                        }
                        break;
                    case EnumTipoDocente.Projeto:
                        dic.Add(enumValue, new List<TipoDocentePermissao>());

                        foreach (EnumModuloPermissao enumMod in lstEnumModPerm)
                        {
                            TipoDocentePermissao tipDoc = new TipoDocentePermissao { IdModuloPermissao = Convert.ToByte(enumMod) };
                            foreach (CFG_PermissaoDocente item in lstPermDoc.Where(p => p.tdc_id == Convert.ToByte(enumValue) && p.pdc_modulo == Convert.ToByte(enumMod)))
                            {
                                switch ((EnumTipoDocente)item.tdc_idPermissao)
                                {
                                    case EnumTipoDocente.Titular:
                                        tipDoc.PdcIdTitular = item.pdc_id;
                                        tipDoc.ConsultaTitular = item.pdc_permissaoConsulta;
                                        tipDoc.EditarTitular = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Compartilhado:
                                        tipDoc.PdcIdCompartilhado = item.pdc_id;
                                        tipDoc.ConsultaCompartilhado = item.pdc_permissaoConsulta;
                                        tipDoc.EditarCompartilhado = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Projeto:
                                        tipDoc.PdcIdProjeto = item.pdc_id;
                                        tipDoc.ConsultaProjeto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarProjeto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Substituto:
                                        tipDoc.PdcIdSubstituto = item.pdc_id;
                                        tipDoc.ConsultaSubstituto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSubstituto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Especial:
                                        tipDoc.PdcIdEspecial = item.pdc_id;
                                        tipDoc.ConsultaEspecial = item.pdc_permissaoConsulta;
                                        tipDoc.EditarEspecial = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.SegundoTitular:
                                        tipDoc.PdcIdSegundo = item.pdc_id;
                                        tipDoc.ConsultaSegundo = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSegundo = item.pdc_permissaoEdicao;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            dic[enumValue].Add(tipDoc);
                        }
                        break;
                    case EnumTipoDocente.Substituto:
                        dic.Add(enumValue, new List<TipoDocentePermissao>());

                        foreach (EnumModuloPermissao enumMod in lstEnumModPerm)
                        {
                            TipoDocentePermissao tipDoc = new TipoDocentePermissao { IdModuloPermissao = Convert.ToByte(enumMod) };
                            foreach (CFG_PermissaoDocente item in lstPermDoc.Where(p => p.tdc_id == Convert.ToByte(enumValue) && p.pdc_modulo == Convert.ToByte(enumMod)))
                            {
                                switch ((EnumTipoDocente)item.tdc_idPermissao)
                                {
                                    case EnumTipoDocente.Titular:
                                        tipDoc.PdcIdTitular = item.pdc_id;
                                        tipDoc.ConsultaTitular = item.pdc_permissaoConsulta;
                                        tipDoc.EditarTitular = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Compartilhado:
                                        tipDoc.PdcIdCompartilhado = item.pdc_id;
                                        tipDoc.ConsultaCompartilhado = item.pdc_permissaoConsulta;
                                        tipDoc.EditarCompartilhado = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Projeto:
                                        tipDoc.PdcIdProjeto = item.pdc_id;
                                        tipDoc.ConsultaProjeto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarProjeto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Substituto:
                                        tipDoc.PdcIdSubstituto = item.pdc_id;
                                        tipDoc.ConsultaSubstituto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSubstituto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Especial:
                                        tipDoc.PdcIdEspecial = item.pdc_id;
                                        tipDoc.ConsultaEspecial = item.pdc_permissaoConsulta;
                                        tipDoc.EditarEspecial = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.SegundoTitular:
                                        tipDoc.PdcIdSegundo = item.pdc_id;
                                        tipDoc.ConsultaSegundo = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSegundo = item.pdc_permissaoEdicao;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            dic[enumValue].Add(tipDoc);
                        }
                        break;
                    case EnumTipoDocente.Especial:
                        dic.Add(enumValue, new List<TipoDocentePermissao>());

                        foreach (EnumModuloPermissao enumMod in lstEnumModPerm)
                        {
                            TipoDocentePermissao tipDoc = new TipoDocentePermissao { IdModuloPermissao = Convert.ToByte(enumMod) };
                            foreach (CFG_PermissaoDocente item in lstPermDoc.Where(p => p.tdc_id == Convert.ToByte(enumValue) && p.pdc_modulo == Convert.ToByte(enumMod)))
                            {
                                switch ((EnumTipoDocente)item.tdc_idPermissao)
                                {
                                    case EnumTipoDocente.Titular:
                                        tipDoc.PdcIdTitular = item.pdc_id;
                                        tipDoc.ConsultaTitular = item.pdc_permissaoConsulta;
                                        tipDoc.EditarTitular = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Compartilhado:
                                        tipDoc.PdcIdCompartilhado = item.pdc_id;
                                        tipDoc.ConsultaCompartilhado = item.pdc_permissaoConsulta;
                                        tipDoc.EditarCompartilhado = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Projeto:
                                        tipDoc.PdcIdProjeto = item.pdc_id;
                                        tipDoc.ConsultaProjeto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarProjeto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Substituto:
                                        tipDoc.PdcIdSubstituto = item.pdc_id;
                                        tipDoc.ConsultaSubstituto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSubstituto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Especial:
                                        tipDoc.PdcIdEspecial = item.pdc_id;
                                        tipDoc.ConsultaEspecial = item.pdc_permissaoConsulta;
                                        tipDoc.EditarEspecial = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.SegundoTitular:
                                        tipDoc.PdcIdSegundo = item.pdc_id;
                                        tipDoc.ConsultaSegundo = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSegundo = item.pdc_permissaoEdicao;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            dic[enumValue].Add(tipDoc);
                        }
                        break;
                    case EnumTipoDocente.SegundoTitular:
                        dic.Add(enumValue, new List<TipoDocentePermissao>());

                        foreach (EnumModuloPermissao enumMod in lstEnumModPerm)
                        {
                            TipoDocentePermissao tipDoc = new TipoDocentePermissao { IdModuloPermissao = Convert.ToByte(enumMod) };
                            foreach (CFG_PermissaoDocente item in lstPermDoc.Where(p => p.tdc_id == Convert.ToByte(enumValue) && p.pdc_modulo == Convert.ToByte(enumMod)))
                            {
                                switch ((EnumTipoDocente)item.tdc_idPermissao)
                                {
                                    case EnumTipoDocente.Titular:
                                        tipDoc.PdcIdTitular = item.pdc_id;
                                        tipDoc.ConsultaTitular = item.pdc_permissaoConsulta;
                                        tipDoc.EditarTitular = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Compartilhado:
                                        tipDoc.PdcIdCompartilhado = item.pdc_id;
                                        tipDoc.ConsultaCompartilhado = item.pdc_permissaoConsulta;
                                        tipDoc.EditarCompartilhado = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Projeto:
                                        tipDoc.PdcIdProjeto = item.pdc_id;
                                        tipDoc.ConsultaProjeto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarProjeto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Substituto:
                                        tipDoc.PdcIdSubstituto = item.pdc_id;
                                        tipDoc.ConsultaSubstituto = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSubstituto = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.Especial:
                                        tipDoc.PdcIdEspecial = item.pdc_id;
                                        tipDoc.ConsultaEspecial = item.pdc_permissaoConsulta;
                                        tipDoc.EditarEspecial = item.pdc_permissaoEdicao;
                                        break;
                                    case EnumTipoDocente.SegundoTitular:
                                        tipDoc.PdcIdSegundo = item.pdc_id;
                                        tipDoc.ConsultaSegundo = item.pdc_permissaoConsulta;
                                        tipDoc.EditarSegundo = item.pdc_permissaoEdicao;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            dic[enumValue].Add(tipDoc);
                        }
                        break;
                    default:
                        break;
                }
            }

            return dic;
        }

        /// <summary>
        /// Retorna uma lista com os valores do enumerador.
        /// </summary>
        /// <param name="enumerador">Enumerador que terá os valores listados.</param>
        /// <param name="enumCompare">Lista contendo valores de enum que não devem ser adicionados.</param>
        /// <returns>Lista com descrição e valores dos elementos do enumerador.</returns>
        private static List<string> ObterValoresEnum(Type enumerador, List<Enum> enumCompare)
        {
            List<string> lista = new List<string>();

            if (enumerador != null)
            {
                Array enumValores = Enum.GetValues(enumerador);

                foreach (Enum valor in enumValores)
                {
                    if (!enumCompare.Contains(valor))
                    {
                        lista.Add(DescricaoEnum(valor));
                    }
                }
            }
            return lista;
        }

        /// <summary>
        /// Retorna a descrição de um determinado elemento de um Enumerador.
        /// </summary>
        /// <param name="elemento">Elemento do enumerador de onde a descrição será retornada.</param>
        /// <returns>String com a descrição do elemento do Enumerador.</returns>
        private static string DescricaoEnum(Enum elemento)
        {
            FieldInfo infoElemento = elemento.GetType().GetField(elemento.ToString());
            DescriptionAttribute[] atributos = (DescriptionAttribute[])infoElemento.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (atributos.Length > 0)
            {
                if (atributos[0].Description != null)
                {
                    return atributos[0].Description;
                }

                return "Sem descrição";
            }

            return elemento.ToString();
        }

        /// <summary>
        /// Método para salvar as permissões dos docentes
        /// </summary>
        private void Salvar()
        {
            try
            {
                if (lstTipoDocente == null)
                    lstTipoDocente = ACA_TipoDocenteBO.SelecionaAtivos(ApplicationWEB.AppMinutosCacheLongo);

                List<CFG_PermissaoDocente> lstPermDoc = new List<CFG_PermissaoDocente>();

                //Percorre o repeater que contem os tipos de docentes
                foreach (RepeaterItem item in rptTipoDocente.Items)
                {
                    HiddenField hdfTipoDoc = (HiddenField)item.FindControl("hdfTipoDocente");
                    EnumTipoDocente tipoDoc = (EnumTipoDocente)Convert.ToByte(hdfTipoDoc.Value);

                    //Percorre o repeater que as permissões marcadas dos modulos: Aula; Plano de aula; Anotações; Planejamento anual; Frequência; Avaliações 
                    Repeater rptConsEdic = (Repeater)item.FindControl("rptSelecionarConsultaEdicao");

                    foreach (RepeaterItem valor in rptConsEdic.Items)
                    {
                        HiddenField hdfMod = (HiddenField)valor.FindControl("hdfModuloConsEdic");
                        EnumModuloPermissao modulo = (EnumModuloPermissao)Convert.ToByte(hdfMod.Value);

                        int hdfModuloConsEdicIdTitular = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsEdicIdTitular")).Value);
                        CheckBox chkConsTitu = (CheckBox)valor.FindControl("chkConsTitu");
                        CheckBox chkEdicTitu = (CheckBox)valor.FindControl("chkEdicTitu");
                        if (chkConsTitu != null && chkEdicTitu != null &&
                            chkConsTitu.Visible && chkEdicTitu.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsEdicIdTitular,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Titular),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = chkEdicTitu.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloConsEdicIdCompartilhado = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsEdicIdCompartilhado")).Value);
                        CheckBox chkConsComp = (CheckBox)valor.FindControl("chkConsComp");
                        CheckBox chkEdicComp = (CheckBox)valor.FindControl("chkEdicComp");
                        if (chkConsComp != null && chkEdicComp != null &&
                            chkConsComp.Visible && chkEdicComp.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsEdicIdCompartilhado,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Compartilhado),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsComp.Checked,
                                pdc_permissaoEdicao = chkEdicComp.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloConsEdicIdProjeto = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsEdicIdProjeto")).Value);
                        CheckBox chkConsProj = (CheckBox)valor.FindControl("chkConsProj");
                        CheckBox chkEdicProj = (CheckBox)valor.FindControl("chkEdicProj");
                        if (chkConsProj != null && chkEdicProj != null &&
                            chkConsProj.Visible && chkEdicProj.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsEdicIdProjeto,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Projeto),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsProj.Checked,
                                pdc_permissaoEdicao = chkEdicProj.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloConsEdicIdSubstituto = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsEdicIdSubstituto")).Value);
                        CheckBox chkConsSubs = (CheckBox)valor.FindControl("chkConsSubs");
                        CheckBox chkEdicSubs = (CheckBox)valor.FindControl("chkEdicSubs");
                        if (chkConsSubs != null && chkEdicSubs != null &&
                            chkConsSubs.Visible && chkEdicSubs.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsEdicIdSubstituto,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Substituto),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsSubs.Checked,
                                pdc_permissaoEdicao = chkEdicSubs.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloConsEdicIdEspecial = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsEdicIdEspecial")).Value);
                        CheckBox chkConsEspe = (CheckBox)valor.FindControl("chkConsEspe");
                        CheckBox chkEdicEspe = (CheckBox)valor.FindControl("chkEdicEspe");
                        if (chkConsEspe != null && chkEdicEspe != null &&
                            chkConsEspe.Visible && chkEdicEspe.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsEdicIdEspecial,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Especial),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsEspe.Checked,
                                pdc_permissaoEdicao = chkEdicEspe.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloConsEdicIdSegundo = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsEdicIdSegundo")).Value);
                        CheckBox chkConsSegu = (CheckBox)valor.FindControl("chkConsSegu");
                        CheckBox chkEdicSegu = (CheckBox)valor.FindControl("chkEdicSegu");
                        if (chkConsSegu != null && chkEdicSegu != null &&
                            chkConsSegu.Visible && chkEdicSegu.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsEdicIdSegundo,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.SegundoTitular),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsSegu.Checked,
                                pdc_permissaoEdicao = chkEdicSegu.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });
                    }

                    //Percorre o repeater que as permissões marcadas dos modulos: Compensações; Efetivação
                    Repeater rptPermConsEdic = (Repeater)item.FindControl("rptSelecionarPermissaoConsEdic");

                    foreach (RepeaterItem valor in rptPermConsEdic.Items)
                    {
                        HiddenField hdfMod = (HiddenField)valor.FindControl("hdfModuloPermConsEdic");
                        EnumModuloPermissao modulo = (EnumModuloPermissao)Convert.ToByte(hdfMod.Value);

                        CheckBox chkConsTitu = (CheckBox)valor.FindControl("chkConsTitu");
                        CheckBox chkEdicTitu = (CheckBox)valor.FindControl("chkEdicTitu");

                        int hdfModuloPermConsEdicIdTitular = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsEdicIdTitular")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.Titular)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsEdicIdTitular,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Titular),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = chkEdicTitu.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloPermConsEdicIdCompartilhado = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsEdicIdCompartilhado")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.Compartilhado)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsEdicIdCompartilhado,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Compartilhado),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = chkEdicTitu.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloPermConsEdicIdProjeto = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsEdicIdProjeto")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.Projeto)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsEdicIdProjeto,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Projeto),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = chkEdicTitu.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloPermConsEdicIdSubstituto = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsEdicIdSubstituto")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.Substituto)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsEdicIdSubstituto,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Substituto),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = chkEdicTitu.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloPermConsEdicIdEspecial = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsEdicIdEspecial")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.Especial)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsEdicIdEspecial,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Especial),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = chkEdicTitu.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloPermConsEdicIdSegundo = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsEdicIdSegundo")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.SegundoTitular)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsEdicIdSegundo,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.SegundoTitular),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = chkEdicTitu.Checked,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });
                    }

                    //Percorre o repeater que as permissões marcadas dos modulos: Boletim
                    Repeater rptPermCons = (Repeater)item.FindControl("rptSelecionarPermissaoCons");

                    foreach (RepeaterItem valor in rptPermCons.Items)
                    {
                        HiddenField hdfMod = (HiddenField)valor.FindControl("hdfModuloPermCons");
                        EnumModuloPermissao modulo = (EnumModuloPermissao)Convert.ToByte(hdfMod.Value);

                        CheckBox chkConsTitu = (CheckBox)valor.FindControl("chkConsTitu");

                        int hdfModuloPermConsModuloPermConsEdicIdTitular = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsModuloPermConsEdicIdTitular")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.Titular)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsModuloPermConsEdicIdTitular,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Titular),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloPermConsModuloPermConsEdicIdCompartilhado = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsModuloPermConsEdicIdCompartilhado")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.Compartilhado)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsModuloPermConsEdicIdCompartilhado,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Compartilhado),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloPermConsModuloPermConsEdicIdProjeto = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsModuloPermConsEdicIdProjeto")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.Projeto)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsModuloPermConsEdicIdProjeto,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Projeto),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloPermConsModuloPermConsEdicIdSubstituto = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsModuloPermConsEdicIdSubstituto")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.Substituto)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsModuloPermConsEdicIdSubstituto,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Substituto),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloPermConsModuloPermConsEdicIdEspecial = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsModuloPermConsEdicIdEspecial")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.Especial)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsModuloPermConsEdicIdEspecial,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Especial),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloPermConsModuloPermConsEdicIdSegundo = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloPermConsModuloPermConsEdicIdSegundo")).Value);
                        if (lstTipoDocente.Any(p => p.tdc_id == Convert.ToByte(EnumTipoDocente.SegundoTitular)))
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloPermConsModuloPermConsEdicIdSegundo,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.SegundoTitular),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            }); 
                    }

                    //Percorre o repeater que as permissões marcadas do modulo: Indicadores
                    Repeater rptCons = (Repeater)item.FindControl("rptSelecionarConsulta");

                    foreach (RepeaterItem valor in rptCons.Items)
                    {
                        HiddenField hdfMod = (HiddenField)valor.FindControl("hdfModuloCons");
                        EnumModuloPermissao modulo = (EnumModuloPermissao)Convert.ToByte(hdfMod.Value);

                        int hdfModuloConsModuloPermConsModuloPermConsEdicIdTitular = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsModuloPermConsModuloPermConsEdicIdTitular")).Value);
                        CheckBox chkConsTitu = (CheckBox)valor.FindControl("chkConsTitu");
                        if (chkConsTitu != null && chkConsTitu.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsModuloPermConsModuloPermConsEdicIdTitular,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Titular),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsTitu.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloConsModuloPermConsModuloPermConsEdicIdCompartilhado = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsModuloPermConsModuloPermConsEdicIdCompartilhado")).Value);
                        CheckBox chkConsComp = (CheckBox)valor.FindControl("chkConsComp");
                        if (chkConsComp != null && chkConsComp.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsModuloPermConsModuloPermConsEdicIdCompartilhado,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Compartilhado),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsComp.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloConsModuloPermConsModuloPermConsEdicIdProjeto = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsModuloPermConsModuloPermConsEdicIdProjeto")).Value);
                        CheckBox chkConsProj = (CheckBox)valor.FindControl("chkConsProj");
                        if (chkConsProj != null && chkConsProj.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsModuloPermConsModuloPermConsEdicIdProjeto,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Projeto),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsProj.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloConsModuloPermConsModuloPermConsEdicIdSubstituto = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsModuloPermConsModuloPermConsEdicIdSubstituto")).Value);
                        CheckBox chkConsSubs = (CheckBox)valor.FindControl("chkConsSubs");
                        if (chkConsSubs != null && chkConsSubs.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsModuloPermConsModuloPermConsEdicIdSubstituto,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Substituto),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsSubs.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloConsModuloPermConsModuloPermConsEdicIdEspecial = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsModuloPermConsModuloPermConsEdicIdEspecial")).Value);
                        CheckBox chkConsEspe = (CheckBox)valor.FindControl("chkConsEspe");
                        if (chkConsEspe != null && chkConsEspe.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsModuloPermConsModuloPermConsEdicIdEspecial,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.Especial),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsEspe.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            });

                        int hdfModuloConsModuloPermConsModuloPermConsEdicIdSegundo = Convert.ToInt32(((HiddenField)valor.FindControl("hdfModuloConsModuloPermConsModuloPermConsEdicIdSegundo")).Value);
                        CheckBox chkConsSegu = (CheckBox)valor.FindControl("chkConsSegu");
                        if (chkConsSegu != null && chkConsSegu.Visible)
                            lstPermDoc.Add(new CFG_PermissaoDocente
                            {
                                pdc_id = hdfModuloConsModuloPermConsModuloPermConsEdicIdSegundo,
                                tdc_id = Convert.ToByte(tipoDoc),
                                tdc_idPermissao = Convert.ToByte(EnumTipoDocente.SegundoTitular),
                                pdc_modulo = Convert.ToByte(modulo),
                                pdc_permissaoConsulta = chkConsSegu.Checked,
                                pdc_permissaoEdicao = false,
                                pdc_situacao = 1,
                                pdc_dataCriacao = DateTime.Now,
                                pdc_dataAlteracao = DateTime.Now
                            }); 
                    }
                }

                if (CFG_PermissaoDocenteBO.Salvar(lstPermDoc))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "Alteração nas permissões dos docentes");
                    lblMessage.Text = UtilBO.GetErroMessage("Permissões dos docentes alteradas com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
            }
            catch (ValidationException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException e)
            {
                lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar permissões dos docentes.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            if (!IsPostBack)
            {
                try
                {
                    Carregar();
 
                    //Page.Form.DefaultFocus = bntSalvar.ClientID;
                    Page.Form.DefaultButton = bntSalvar.UniqueID;

                    bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void bntSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Salvar();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("Cadastro.aspx");
        }

        protected void rptSelecionarConsultaEdicao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                if (lstTipoDocente == null)
                    lstTipoDocente = ACA_TipoDocenteBO.SelecionaAtivos(ApplicationWEB.AppMinutosCacheLongo);

                //Percorre os tipos de docentes, carregando suas permissões.
                foreach (EnumTipoDocente enumValue in Enum.GetValues(typeof(EnumTipoDocente)))
                {
                    if (!lstTipoDocente.Any(p => p.tdc_id == (byte)enumValue))
                        continue;

                    switch (enumValue)
                    {
                        case EnumTipoDocente.Titular:
                            CheckBox chkConsTitu = (CheckBox)e.Item.FindControl("chkConsTitu");
                            if (chkConsTitu != null)
                                chkConsTitu.Visible = true;
                            CheckBox chkEdicTitu = (CheckBox)e.Item.FindControl("chkEdicTitu");
                            if (chkEdicTitu != null)
                                chkEdicTitu.Visible = true;
                            break;
                        case EnumTipoDocente.Compartilhado:
                            CheckBox chkConsComp = (CheckBox)e.Item.FindControl("chkConsComp");
                            if (chkConsComp != null)
                                chkConsComp.Visible = true;
                            CheckBox chkEdicComp = (CheckBox)e.Item.FindControl("chkEdicComp");
                            if (chkEdicComp != null)
                                chkEdicComp.Visible = true;
                            break;
                        case EnumTipoDocente.Projeto:
                            CheckBox chkConsProj = (CheckBox)e.Item.FindControl("chkConsProj");
                            if (chkConsProj != null)
                                chkConsProj.Visible = true;
                            CheckBox chkEdicProj = (CheckBox)e.Item.FindControl("chkEdicProj");
                            if (chkEdicProj != null)
                                chkEdicProj.Visible = true;
                            break;
                        case EnumTipoDocente.Substituto:
                            CheckBox chkConsSubs = (CheckBox)e.Item.FindControl("chkConsSubs");
                            if (chkConsSubs != null)
                                chkConsSubs.Visible = true;
                            CheckBox chkEdicSubs = (CheckBox)e.Item.FindControl("chkEdicSubs");
                            if (chkEdicSubs != null)
                                chkEdicSubs.Visible = true;
                            break;
                        case EnumTipoDocente.Especial:
                            CheckBox chkConsEspe = (CheckBox)e.Item.FindControl("chkConsEspe");
                            if (chkConsEspe != null)
                                chkConsEspe.Visible = true;
                            CheckBox chkEdicEspe = (CheckBox)e.Item.FindControl("chkEdicEspe");
                            if (chkEdicEspe != null)
                                chkEdicEspe.Visible = true;
                            break;
                        case EnumTipoDocente.SegundoTitular:
                            CheckBox chkConsSegu = (CheckBox)e.Item.FindControl("chkConsSegu");
                            if (chkConsSegu != null)
                                chkConsSegu.Visible = true;
                            CheckBox chkEdicSegu = (CheckBox)e.Item.FindControl("chkEdicSegu");
                            if (chkEdicSegu != null)
                                chkEdicSegu.Visible = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected void rptSelecionarConsulta_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                if (lstTipoDocente == null)
                    lstTipoDocente = ACA_TipoDocenteBO.SelecionaAtivos(ApplicationWEB.AppMinutosCacheLongo);

                //Percorre os tipos de docentes, carregando suas permissões.
                foreach (EnumTipoDocente enumValue in Enum.GetValues(typeof(EnumTipoDocente)))
                {
                    if (!lstTipoDocente.Any(p => p.tdc_id == (byte)enumValue))
                        continue;

                    switch (enumValue)
                    {
                        case EnumTipoDocente.Titular:
                            CheckBox chkConsTitu = (CheckBox)e.Item.FindControl("chkConsTitu");
                            if (chkConsTitu != null)
                                chkConsTitu.Visible = true;
                            break;
                        case EnumTipoDocente.Compartilhado:
                            CheckBox chkConsComp = (CheckBox)e.Item.FindControl("chkConsComp");
                            if (chkConsComp != null)
                                chkConsComp.Visible = true;
                            break;
                        case EnumTipoDocente.Projeto:
                            CheckBox chkConsProj = (CheckBox)e.Item.FindControl("chkConsProj");
                            if (chkConsProj != null)
                                chkConsProj.Visible = true;
                            break;
                        case EnumTipoDocente.Substituto:
                            CheckBox chkConsSubs = (CheckBox)e.Item.FindControl("chkConsSubs");
                            if (chkConsSubs != null)
                                chkConsSubs.Visible = true;
                            break;
                        case EnumTipoDocente.Especial:
                            CheckBox chkConsEspe = (CheckBox)e.Item.FindControl("chkConsEspe");
                            if (chkConsEspe != null)
                                chkConsEspe.Visible = true;
                            break;
                        case EnumTipoDocente.SegundoTitular:
                            CheckBox chkConsSegu = (CheckBox)e.Item.FindControl("chkConsSegu");
                            if (chkConsSegu != null)
                                chkConsSegu.Visible = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion Eventos
    }
}