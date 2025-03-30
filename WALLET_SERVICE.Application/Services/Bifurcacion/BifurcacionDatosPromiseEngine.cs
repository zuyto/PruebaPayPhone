// <copyright file="BifurcacionDatosPromiseEngine.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

//using Newtonsoft.Json;
//using System.Diagnostics.CodeAnalysis;
//using WALLET_SERVICE.Application.Common.Enumerations;
//using WALLET_SERVICE.Application.Common.Helpers;
//using WALLET_SERVICE.Application.Common.Interfaces.Bifurcacion;
//using WALLET_SERVICE.Application.Common.Interfaces.Repository;
//using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
//using WALLET_SERVICE.Application.Common.Models.DTOs;
//using WALLET_SERVICE.Application.Common.Struct;
//using WALLET_SERVICE.Domain.Entities.HcInvSgl;

//namespace WALLET_SERVICE.Application.Services.Bifurcacion
//{
//    [ExcludeFromCodeCoverage]
//    public class BifurcacionDatosPromiseEngine(ISerilogImplements serilogImplements, IUnitOfWorkWalletTransfer unitOfWorkHcInvSgl) : IBifurcacionDatosPromiseEngine
//    {

//        private readonly ISerilogImplements _serilogImplements = serilogImplements;
//        private readonly IUnitOfWorkWalletTransfer _unitOfWorkHcInvSgl = unitOfWorkHcInvSgl;


//        /// <summary>
//        /// Consulta en HCINV los productos y coberturaras existentes
//        /// </summary>
//        /// <param name="rqs">datos que envia en la peticion de entrada</param>
//        /// <returns></returns>
//        public async Task<List<DtoProductoConCobertura>> BifurcacionDatosProdCobert(DtoTransferJsonRequest rqs)
//        {
//            List<DtoProductoConCobertura> poolDatosSKusRedes;
//            List<DtoProductosRequestCont> skus = rqs.Productos?.Select(p => p).ToList()!;

//            if (rqs.Datos?.IdZona > 0)
//            {
//                var poolDatosSKusRedesDomicilioRf = await _unitOfWorkHcInvSgl.HcInvSglRepository.DispoSkusRedesPorIdZona(skus, rqs.Datos.IdZona);
//                var poolDatosSKusRedesRetiroTda = await _unitOfWorkHcInvSgl.HcInvSglRepository.DispoSkusRedesPorIdDepto(skus, rqs.Datos.IdDepartamento);

//                poolDatosSKusRedes = poolDatosSKusRedesDomicilioRf.Concat(poolDatosSKusRedesRetiroTda).ToList();

//            }
//            else
//            {
//                var poolDatosSKusRedesDomicilioRf = await _unitOfWorkHcInvSgl.HcInvSglRepository.DispoSkusRedesPorIdCiudad(skus, rqs.Datos!.IdCiudad);
//                var poolDatosSKusRedesRetiroTda = await _unitOfWorkHcInvSgl.HcInvSglRepository.DispoSkusRedesPorIdDepto(skus, rqs.Datos.IdDepartamento);

//                poolDatosSKusRedes = poolDatosSKusRedesDomicilioRf.Concat(poolDatosSKusRedesRetiroTda).ToList();
//            }

//            _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(poolDatosSKusRedes.Count, Formatting.Indented), null, "\n\n **** CANTIDAD DE DATOS EN EL POOL DE REGISTROS **** \n");

//            return poolDatosSKusRedes;

//        }


//        /// <summary>
//        /// Metodo encargado de crear un grupo de grupos
//        /// </summary>
//        /// <param name="poolDatosProductCobert"></param>
//        /// <returns></returns>
//        public List<DtoGrupoProcesarCont> BifurcacionArmarGrupos(List<DtoProductoConCobertura> poolDatosProductCobert)
//        {
//            try
//            {
//                List<DtoGrupoProcesarCont> grupos = poolDatosProductCobert
//                                                        .Where(p => p.IdPromesaCliente == (int)TipoPromesa.DomicilioRangoFecha &&
//                                                                poolDatosProductCobert.Select(x => x.PrdLvlNumber).Contains(p.PrdLvlNumber))
//                                                        .GroupBy(p => new { p.NumberInternoInicial, p.NumberInternoFinal, p.IdPromesaCliente })
//                                                        .Select(g => new DtoGrupoProcesarCont
//                                                        {
//                                                            GrupoKey = new DtoGrupoKeyCont
//                                                            {
//                                                                NumberInternoInicial = g.Key.NumberInternoInicial,
//                                                                NumberInternoFinal = g.Key.NumberInternoFinal,
//                                                                IdPromesaCliente = g.Key.IdPromesaCliente
//                                                            },
//                                                            Productos = g.ToList(),
//                                                            Skus = g.Select(x => x.PrdLvlNumber).ToList(),
//                                                            CantidadSKUs = g.Select(p => p.PrdLvlNumber).Distinct().Count()
//                                                        }).ToList();



//                // Identificar los grupos con la mayor cantidad de SKUs
//                int maxCantidadSkus = grupos.Max(g => g.CantidadSKUs);
//                List<DtoGrupoProcesarCont> gruposConMaxSkus = grupos.Where(g => g.CantidadSKUs == maxCantidadSkus).OrderByDescending(p => p.Productos!.Max(x => x.PrdLvlNumber)).ToList();

//                //Armar el grupo a procesar
//                List<string> skusMaxCant = grupos.Select(g => g.Skus).Distinct().FirstOrDefault()!;
//                List<DtoGrupoProcesarCont> gruposConSkusFaltantes = grupos.Where(g => !g.Productos!.Exists(p => skusMaxCant.Contains(p.PrdLvlNumber))).ToList();
//                List<DtoGrupoProcesarCont> grupoProcesar = [.. gruposConMaxSkus.OrderByDescending(p => p.GrupoKey!.NumberInternoInicial), .. gruposConSkusFaltantes];



//                List<DtoGrupoProcesarCont> gruposReagrupadosPorCantidadSKUs = grupoProcesar.GroupBy(g => g.CantidadSKUs)
//                                                                                        .Select(grupo => new DtoGrupoProcesarCont
//                                                                                        {
//                                                                                            CantidadSKUs = grupo.Key,
//                                                                                            Productos = grupo.SelectMany(g => g.Productos!).Select(p => p).ToList(),
//                                                                                        }).ToList();
//                _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(gruposReagrupadosPorCantidadSKUs.Count, Formatting.Indented), null, "\n\n **** CANTIDAD DE GRUPOS DE GRUPOS **** \n");


//                return gruposReagrupadosPorCantidadSKUs;
//            }
//            catch (Exception ex)
//            {
//                _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** BufurcacionDatosArmarResponse **** \n");

//                throw;
//            }



//        }

//        /// <summary>
//        /// Metodo encargado de armar los datos para crear el json response
//        /// </summary>
//        /// <param name="gruposProcesar"></param>
//        /// <returns></returns>
//        public List<DtoProductoConCobertura> BufurcacionDatosArmarResponse(List<DtoGrupoProcesarCont> gruposProcesar, List<DtoProductoConCobertura> poolDatosProductCobert)
//        {
//            try
//            {
//                List<DtoProductoConCobertura> resultado = new List<DtoProductoConCobertura>();
//                int idGrupo = 1;

//                for (var i = 0; i < gruposProcesar.Count; i++)
//                {
//                    var grupo = gruposProcesar[i];
//                    // Procesar los productos del grupo
//                    List<DtoProductoConCobertura> productos = grupo.Productos!;

//                    // Aplicar el filtro según las prioridades
//                    List<DtoProductoConCobertura> filtroDomRt = productos.Where(p => p.IdTipoNodo == (int)TipoNodo.CD && p.NumberInternoInicial == (int)PrioridadNodoCd.Funza).ToList();

//                    if (!filtroDomRt.Any())
//                    {
//                        filtroDomRt = productos
//                            .Where(p => p.IdTipoNodo == (int)TipoNodo.CD)
//                            .OrderBy(p => p.IdRedZona)
//                            .ToList();
//                    }

//                    if (!filtroDomRt.Any())
//                    {
//                        filtroDomRt = productos
//                            .Where(p => p.IdTipoNodo == (int)TipoNodo.TDA)
//                            .OrderBy(p => p.IdRedZona)
//                            .ToList();
//                    }

//                    if (!filtroDomRt.Any() && productos.Exists(p => p.IdTipoNodo == (int)TipoNodo.TDA))
//                    {
//                        filtroDomRt = productos
//                            .OrderBy(p => p.IdRedZona)
//                            .ToList();
//                    }

//                    List<string> skus = filtroDomRt.Select(p => p.PrdLvlNumber).ToList();


//                    List<DtoProductoConCobertura> filtroRecogTda = poolDatosProductCobert.Where(x => x.IdPromesaCliente == (int)TipoPromesa.RecogidaTdaProgramado && skus.Contains(x.PrdLvlNumber)).ToList();

//                    List<DtoProductoConCobertura> filtroFinal = filtroDomRt.Concat(filtroRecogTda).ToList();


//                    // Asignar el IdGrupo a todos los productos del filtro del grupo actual
//                    filtroFinal.ForEach(p => p.IdGrupo = idGrupo);

//                    // Agregar los productos filtrados al resultado
//                    resultado.AddRange(filtroFinal);

//                    // Incrementar el IdGrupo para el siguiente grupo
//                    idGrupo++;
//                }
//                _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(resultado.Count, Formatting.Indented), null, "\n\n **** CANTIDAD DE DATOS A ARMAR EN EL RESPONSE**** \n");

//                return resultado;
//            }
//            catch (Exception ex)
//            {
//                _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** BufurcacionDatosArmarResponse **** \n");

//                throw;
//            }


//        }


//        /// <summary>
//        /// Metodo encargado de armar el Json response
//        /// </summary>
//        /// <param name="datosArmarResponse"></param>
//        /// <returns></returns>
//        public List<DtoGrupoEntregaCont> BifurcacionJsonResponse(List<DtoProductoConCobertura> datosArmarResponse)
//        {

//            var (fechasxPromesa, fechaAlistamiento) = ArmadoFechas();

//            try
//            {
//                var gruposEntrega = (from p in datosArmarResponse
//                                     group p by p.IdGrupo into grupo
//                                     select new DtoGrupoEntregaCont
//                                     {
//                                         IdGrupo = grupo.Key.ToString(),
//                                         Productos = (from prod in grupo
//                                                      group prod by prod.PrdLvlNumber into prodGroup
//                                                      select new DtoProductoCont
//                                                      {
//                                                          Sku = prodGroup.Key,
//                                                          Cantidad = prodGroup.Select(x => x.CantidadSku).FirstOrDefault()
//                                                      }).ToList(),
//                                         PromesasEntrega = (from prom in grupo
//                                                            group prom by new
//                                                            {
//                                                                prom.IdPromesaCliente,

//                                                            } into promGroup
//                                                            select new DtoPromesaEntregaCont
//                                                            {
//                                                                IdPromesa = promGroup.Key.IdPromesaCliente.ToString()!,
//                                                                FechaTransito = DateTime.Now,
//                                                                FechaAlistamiento = fechaAlistamiento,
//                                                                FuenteInventario = promGroup.Select(x => x.NumberInternoInicial).Distinct().First().ToString()!,
//                                                                FuenteDespacho = promGroup.Select(x => x.NumberInternoFinal).Distinct().First().ToString()!,
//                                                                IdRedZona = promGroup.Select(x => x.IdRedZona).Min().ToString(),
//                                                                Fechas = promGroup.Key.IdPromesaCliente == (int)TipoPromesa.RecogidaTdaProgramado ? fechasxPromesa.Fechas : [],
//                                                                FechasDisponibles = promGroup.Key.IdPromesaCliente == (int)TipoPromesa.DomicilioRangoFecha ? fechasxPromesa.FechasDisponibles : []
//                                                            }).ToList()
//                                     }).ToList();

//                _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(gruposEntrega, Formatting.Indented), null, "\n\n **** BifurcacionJsonResponse **** \n");


//                return gruposEntrega;
//            }
//            catch (Exception ex)
//            {
//                _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** Armando Fechas **** \n");
//                throw;
//            }


//        }


//        /// <summary>
//        /// Metodo encargado de armar las fechas para cada IdPromesa 2 y 4 
//        /// </summary>
//        /// <returns></returns>
//        private (DtoFechasResponseCont, DateTime?) ArmadoFechas()
//        {
//            try
//            {
//                List<TblOmsParametrosContingencium> parametrosFechas = _unitOfWorkHcInvSgl.HcInvSglRepository.ConsultaParametrosFechas().GetAwaiter().GetResult();

//                int DiasTransito = (int)parametrosFechas.Find(t => t.IdParametro == 3)?.Valor!;
//                int DiasRango = (int)parametrosFechas.Find(t => t.IdParametro == 4)?.Valor!;

//                DateTime fechaAlistamiento = DateTime.Now.AddDays(DiasTransito);
//                DateTime fechafin = fechaAlistamiento.AddDays(DiasRango);


//                List<DtoFechaCont> fechas = new List<DtoFechaCont>();
//                List<DtoFechaCont> fechasDisponibles = new List<DtoFechaCont> { new DtoFechaCont { FechaInicial = fechaAlistamiento.Date.AddHours(8), FechaFinal = fechafin.Date.AddHours(19) } };

//                for (DateTime fecha = fechaAlistamiento; fecha <= fechafin; fecha = fecha.AddDays(1))
//                {
//                    fechas.Add(new DtoFechaCont
//                    {
//                        FechaInicial = fecha.Date,
//                        FechaFinal = fecha.Date
//                    });
//                }


//                return (new DtoFechasResponseCont
//                {

//                    Fechas = fechas,
//                    FechasDisponibles = fechasDisponibles

//                }, fechaAlistamiento);
//            }
//            catch (Exception ex)
//            {
//                _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** Armando Fechas **** \n");

//                throw;
//            }

//        }
//    }
//}
