// <copyright file="PromiseEngineContingencia.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

//using WALLET_SERVICE.Application.Common.Enumerations;
//using WALLET_SERVICE.Application.Common.Helpers;
//using WALLET_SERVICE.Application.Common.Interfaces.Bifurcacion;
//using WALLET_SERVICE.Application.Common.Interfaces.Services;
//using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
//using WALLET_SERVICE.Application.Common.Models.DTOs;
//using WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase;
//using WALLET_SERVICE.Application.Common.Static;
//using WALLET_SERVICE.Application.Common.Struct;
//using WALLET_SERVICE.Application.Common.Transversales;

//namespace WALLET_SERVICE.Application.Services
//{
//    public class PromiseEngineContingencia(ISerilogImplements serilogImplements, IBifurcacionDatosPromiseEngine _bifurcacionDatos) : IPromiseEngineContingencia
//    {
//        private readonly ISerilogImplements _serilogImplements = serilogImplements;
//        private readonly IBifurcacionDatosPromiseEngine _bifurcacionDatos = _bifurcacionDatos;


//        public async Task<DtoGenericResponse<DtoTransferJsonResponse>> ProcesarContingencia(DtoTransferJsonRequest request)
//        {


//            var response = new DtoTransferJsonResponse
//            {
//                GruposEntrega = new List<DtoGrupoEntregaCont>(),
//                SinRed = new DtoSinRedCont { IdGrupo = "-1", Productos = new List<DtoProductoCont>() }
//            };

//            List<DtoProductoConCobertura> poolDatosProductCobert = await _bifurcacionDatos.BifurcacionDatosProdCobert(request);



//            if (!poolDatosProductCobert.Any())
//            {
//                response.SinRed.Productos = request.Productos!
//                    .Select(p => new DtoProductoCont { Sku = p.Sku, Cantidad = p.Cantidad })
//                    .ToList();

//                return GenericHelpers.BuildResponse<DtoTransferJsonResponse>(false, response, UserTypeMessages.ERROR_NO_DATA_REDES);
//            }

//            List<string> poolSkusProductCobert = poolDatosProductCobert.Select(p => p.PrdLvlNumber).Distinct().ToList();
//            List<string> skusRequest = request.Productos!.Select(p => p.Sku).ToList()!;
//            List<string> skusFaltantes = skusRequest.Except(poolSkusProductCobert).ToList();
//            bool existeDomRf = poolDatosProductCobert.Exists(x => x.IdPromesaCliente == (int)TipoPromesa.DomicilioRangoFecha);

//            if (existeDomRf)
//            {

//                List<DtoGrupoProcesarCont> gruposProcesar = _bifurcacionDatos.BifurcacionArmarGrupos(poolDatosProductCobert.Where(x => x.IdPromesaCliente == (int)TipoPromesa.DomicilioRangoFecha).ToList());

//                if (gruposProcesar.Count > 1)
//                {
//                    _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, null, null, "\n\n **** PROCESA MAS DE UN GRUPOS **** \n");

//                    response.GruposEntrega = ProcesarGrupos(gruposProcesar, poolDatosProductCobert);
//                }
//                else if (gruposProcesar.Count == 1)
//                {
//                    _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, null, null, "\n\n **** PROCESA UN GRUPOS **** \n");

//                    response.GruposEntrega = ProcesarUnGrupo(gruposProcesar, poolDatosProductCobert);
//                }
//                else
//                {
//                    _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, null, null, "\n\n **** PROCESA SIN GRUPOS **** \n");

//                    response = ProcesarSinGrupo(poolDatosProductCobert, request);
//                }

//            }
//            else
//            {
//                _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, null, null, "\n\n **** NO ENCONTRO DOMICILIO RF **** \n");

//                List<DtoProductoConCobertura> poolDatosProdCobertDepto = poolDatosProductCobert
//                                                                        .Where(x => x.IdPromesaCliente == (int)TipoPromesa.RecogidaTdaProgramado)
//                                                                            .Select(x =>
//                                                                            {
//                                                                                x.IdGrupo = 1;
//                                                                                return x;
//                                                                            }).ToList();


//                response.GruposEntrega = _bifurcacionDatos.BifurcacionJsonResponse(poolDatosProdCobertDepto);


//            }


//            if (skusFaltantes.Count > 0)
//            {
//                _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, null, null, "\n\n **** NO ENCONTRO SKU/s, ARMA SIN RED **** \n");

//                response?.SinRed?.Productos?.AddRange(AutoMapperTransversales.MapperGenericListToList<DtoProductosRequestCont, DtoProductoCont>(request.Productos!.Where(p => skusFaltantes.Contains(p.Sku!)).ToList()));
//            }



//            return CrearRespuesta(response!);
//        }


//        /// <summary>
//        /// Metodo encargado de procesar cuando se genera mas de un grupo Domicilio Rf 
//        /// </summary>
//        /// <param name="gruposProcesar"></param>
//        /// <param name="poolDatosProductCobert"></param>
//        /// <returns></returns>
//        private List<DtoGrupoEntregaCont> ProcesarGrupos(List<DtoGrupoProcesarCont> gruposProcesar, List<DtoProductoConCobertura> poolDatosProductCobert)
//        {
//            List<DtoProductoConCobertura> datosArmarResponseDomRfRecogTda = _bifurcacionDatos.BufurcacionDatosArmarResponse(gruposProcesar, poolDatosProductCobert);

//            return _bifurcacionDatos.BifurcacionJsonResponse(datosArmarResponseDomRfRecogTda);
//        }

//        /// <summary>
//        /// Metodo encargado de procesar cuado silo existe un grupo Domicilio Rf y busca sus respectivos Recogida Tda Programado
//        /// </summary>
//        /// <param name="gruposProcesar"></param>
//        /// <param name="poolDatosProductCobert"></param>
//        /// <returns></returns>
//        private List<DtoGrupoEntregaCont> ProcesarUnGrupo(List<DtoGrupoProcesarCont> gruposProcesar, List<DtoProductoConCobertura> poolDatosProductCobert)
//        {
//            int idGrupo = 1;
//            List<DtoProductoConCobertura> datosArmarResponseDomRf = gruposProcesar.SelectMany(grupo => grupo.Productos!
//                                                                                        .Select(producto =>
//                                                                                        {
//                                                                                            producto.IdGrupo = idGrupo;
//                                                                                            return producto;
//                                                                                        })).ToList();

//            List<string> skus = datosArmarResponseDomRf.Select(p => p.PrdLvlNumber).ToList();
//            List<DtoProductoConCobertura> datosArmarResponseRecogTda = poolDatosProductCobert.Where(x => x.IdPromesaCliente == (int)TipoPromesa.RecogidaTdaProgramado && skus.Contains(x.PrdLvlNumber))
//                                                                                                .Select(x =>
//                                                                                                {
//                                                                                                    x.IdGrupo = idGrupo;
//                                                                                                    return x;
//                                                                                                }).ToList();

//            List<DtoProductoConCobertura> datosArmarResponse = datosArmarResponseDomRf.Concat(datosArmarResponseRecogTda).ToList();

//            return _bifurcacionDatos.BifurcacionJsonResponse(datosArmarResponse);
//        }


//        /// <summary>
//        /// Metodo encargado de procesar cuando no existen grupos Domicilio Rf y busca sus respectivos Recogida Tda Programado
//        /// </summary>
//        /// <param name="poolDatosProductCobert"></param>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        private DtoTransferJsonResponse ProcesarSinGrupo(List<DtoProductoConCobertura> poolDatosProductCobert, DtoTransferJsonRequest request)
//        {

//            var response = new DtoTransferJsonResponse
//            {
//                GruposEntrega = new List<DtoGrupoEntregaCont>(),
//                SinRed = new DtoSinRedCont { IdGrupo = "-1", Productos = new List<DtoProductoCont>() }
//            };

//            int idGrupo = 1;

//            List<DtoProductoConCobertura> datosArmarResponseRecogTda = poolDatosProductCobert.Where(x => x.IdPromesaCliente == (int)TipoPromesa.RecogidaTdaProgramado)
//                                                                                                .Select(x =>
//                                                                                                {
//                                                                                                    x.IdGrupo = idGrupo;
//                                                                                                    return x;
//                                                                                                }).ToList();

//            if (datosArmarResponseRecogTda.Count != 0)
//                response.GruposEntrega = _bifurcacionDatos.BifurcacionJsonResponse(datosArmarResponseRecogTda);
//            else
//                response.SinRed.Productos.AddRange(AutoMapperTransversales.MapperGenericListToList<DtoProductosRequestCont, DtoProductoCont>(request.Productos!).ToList());



//            return response;


//        }

//        /// <summary>
//        /// Metodo encargado de crear el Json response
//        /// </summary>
//        /// <param name="response"></param>
//        /// <returns></returns>
//        private static DtoGenericResponse<DtoTransferJsonResponse> CrearRespuesta(DtoTransferJsonResponse response)
//        {
//            bool tieneGruposEntrega = response.GruposEntrega?.Count != 0;
//            bool tieneSinRed = response.SinRed?.Productos?.Count != 0;

//            var (exito, mensaje) = (tieneGruposEntrega, tieneSinRed)
//            switch
//            {
//                (true, true) => (false, UserTypeMessages.OKERRGEN01),
//                (false, true) => (false, UserTypeMessages.ERROR_NO_DATA_REDES),
//                (true, false) => (true, UserTypeMessages.OKGEN01),
//                _ => (false, string.Empty)
//            };

//            return GenericHelpers.BuildResponse<DtoTransferJsonResponse>(exito, response, mensaje);
//        }




//    }
//}

