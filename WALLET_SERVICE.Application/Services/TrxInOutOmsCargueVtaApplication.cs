// <copyright file="TrxInOutOmsCargueVtaApplication.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

//using Dasync.Collections;
//using Newtonsoft.Json;
//using WALLET_SERVICE.Application.Common.Enumerations;
//using WALLET_SERVICE.Application.Common.Helpers;
//using WALLET_SERVICE.Application.Common.Interfaces.ArmarJson;
//using WALLET_SERVICE.Application.Common.Interfaces.Repository;
//using WALLET_SERVICE.Application.Common.Interfaces.Services;
//using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
//using WALLET_SERVICE.Application.Common.Models.DTOs;
//using WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase;
//using WALLET_SERVICE.Application.Common.Static;
//using WALLET_SERVICE.Application.Common.Struct;
//using System.Data;

//namespace WALLET_SERVICE.Application.Services
//{
//    public class TrxInOutOmsCargueVtaApplication : ITrxInOutOmsCargueVtaApplication
//    {
//        private readonly ISerilogImplements _serilogImplements;
//        private readonly IArmarJsonRequestGlobal _armarJsonRequestGlobal;
//        private readonly HttpServiceManager _httpServiceManager;
//        private readonly IUnitOfWorkGspl _unitOfWorkGspl;


//        public TrxInOutOmsCargueVtaApplication(ISerilogImplements serilogImplements, IArmarJsonRequestGlobal armarJsonRequestGlobal, HttpServiceManager httpServiceManager, IUnitOfWorkGspl unitOfWorkGspl)
//        {
//            _serilogImplements = serilogImplements;
//            _armarJsonRequestGlobal = armarJsonRequestGlobal;
//            _httpServiceManager = httpServiceManager;
//            _unitOfWorkGspl = unitOfWorkGspl;
//        }

//        public async Task<DtoGenericResponse> ProcesaTrxOm11Om12(DtoTransferJsonRequest request)
//        {
//            string?[] stickers = [];
//            int?[] idsTransaccion = [(int)OmsTransaccionConfig.SALIDA_OMS_CRGVTA, (int)OmsTransaccionConfig.ENTRADA_DISPO_CRGVTA];
//            try
//            {
//                DtoGenericResponse responseJsonRequestMasivo = await _armarJsonRequestGlobal.ArmarJsonRequestTrxCargueMasivo(request);

//                if (!responseJsonRequestMasivo.EsExitoso)
//                {
//                    return GenericHelpers.BuildResponse(false, responseJsonRequestMasivo.Resultado, responseJsonRequestMasivo.Mensaje);
//                }

//                DtoTrxCargueMasivo datosSalidaEntradaCrgVte = JsonConvert.DeserializeObject<DtoTrxCargueMasivo>(JsonConvert.SerializeObject(responseJsonRequestMasivo.Resultado));
//                stickers = datosSalidaEntradaCrgVte.LstTransacciones.Select(x => x.Sticker).Distinct().ToArray();



//                await ActualizarEstadoProceso(stickers, idsTransaccion, (int)EstadosProceso.PendientePorProcesar, (short)EstadosProceso.EnProceso);


//                DtoGenericResponse responseCargueMasivo = await ConsumirCargueMasivoTrx(responseJsonRequestMasivo);

//                if (!responseCargueMasivo.EsExitoso)
//                {
//                    await ActualizarEstadoProceso(stickers, idsTransaccion, (int)EstadosProceso.EnProceso, (short)EstadosProceso.Error);
//                    return responseCargueMasivo;
//                }



//                DtoGenericResponse responseJsonRequestTracking = await _armarJsonRequestGlobal.ArmarJsonRequestTrxTrackingInventario(responseJsonRequestMasivo.Resultado, responseCargueMasivo.Resultado);

//                if (!responseJsonRequestTracking.EsExitoso)
//                {
//                    await ActualizarEstadoProceso(stickers, idsTransaccion, (int)EstadosProceso.EnProceso, (short)EstadosProceso.Error);
//                    return responseJsonRequestTracking;
//                }

//                List<DtoTrxResponseApi> responseTrakingInventario = await ConsumirTrackingInventario(responseJsonRequestTracking);


//                return GenericHelpers.BuildResponse(true, responseTrakingInventario, UserTypeMessages.OKGEN01);
//            }
//            catch (Exception ex)
//            {
//                await ActualizarEstadoProceso(stickers, idsTransaccion, (int)EstadosProceso.EnProceso, (short)EstadosProceso.Error);
//                _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Error, JsonConvert.SerializeObject(ex.HandleExceptionMessage(true), Formatting.Indented), null, "\n\n **** ERROR PRESENTADO EN  ProcesaTrxOm11Om12 APLICATTION **** \n");
//                throw;
//            }


//        }

//        private async Task ActualizarEstadoProceso(string?[] stickers, int?[] idsTransaccion, int estadoActual, short nuevoEstado)
//        {
//            await _unitOfWorkGspl.OrquestadorVeRepository.ActualizarEstadoProcesoAsync(stickers, idsTransaccion, estadoActual, nuevoEstado);
//        }

//        private async Task<DtoGenericResponse> ConsumirCargueMasivoTrx(DtoGenericResponse responseJsonRequestMasivo)
//        {
//            DtoGenericResponse resultHttpCargaMasivo;

//            string jsonRequestMasivoString = JsonConvert.SerializeObject(responseJsonRequestMasivo.Resultado);

//            DtoGenericResponseHttp responseHttpMasivo = await _httpServiceManager.PostHttpAsync(Constantes.CARGA_TRX_MASIVO, jsonRequestMasivoString);


//            if (!responseHttpMasivo.EsExitoso)
//            {
//                resultHttpCargaMasivo = await ProcesaErrorHttp<object>(responseHttpMasivo);
//            }
//            else
//            {
//                resultHttpCargaMasivo = await ProcesaOkHttp<List<DtoTrxResultCargaMasivo>>(responseHttpMasivo);
//            }

//            _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(resultHttpCargaMasivo, Formatting.Indented), null, "\n\n **** RESPONSE ENVIO PROCESAR TRANSACCION ODBMS **** \n");
//            return resultHttpCargaMasivo;


//        }


//        private async Task<List<DtoTrxResponseApi>> ConsumirTrackingInventario(DtoGenericResponse responseJsonRequestTracking)
//        {
//            string responseJsonRequestTrackingString = JsonConvert.SerializeObject(responseJsonRequestTracking.Resultado);

//            List<DtoTrxTrackingInventario> listJsonRequestTracking = JsonConvert.DeserializeObject<List<DtoTrxTrackingInventario>>(responseJsonRequestTrackingString);

//            List<DtoTrxResponseApi> result = new();


//            await listJsonRequestTracking.ParallelForEachAsync(async transaccion =>
//            {

//                string requestTraking = JsonConvert.SerializeObject(transaccion);


//                DtoGenericResponseHttp res = await _httpServiceManager.PostHttpAsync(Constantes.TRACKING_INVENTARIO, requestTraking);


//                DtoTrxResponseApi responseHttp = JsonConvert.DeserializeObject<DtoTrxResponseApi>(res.ResultadoHttp);


//                if (responseHttp.Codigo == 0)
//                {
//                    result.Add(responseHttp);
//                }
//                result.Add(responseHttp);
//            });

//            _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(result, Formatting.Indented), null, "\n\n **** RESPONSE ENVIO PROCESAR TRACKING INVENTARIO **** \n");
//            return result;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="respuestaHttp"></param>
//        /// <returns></returns>
//        private async Task<DtoGenericResponse> ProcesaOkHttp<T>(DtoGenericResponseHttp respuestaHttp) where T : class
//        {
//            bool validaEsquemaOk = GenericHelpers.ValidarEsquemaOk(respuestaHttp.ResultadoHttp);
//            object? erroRes = validaEsquemaOk ? GenericHelpers.TryDeserialize<T>(respuestaHttp.ResultadoHttp) : respuestaHttp.ResultadoHttp;

//            return await Task.FromResult(GenericHelpers.BuildResponse(true, erroRes, respuestaHttp.Mensaje, validaEsquemaOk));
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="respuestaHttp"></param>
//        /// <returns></returns>
//        private async Task<DtoGenericResponse> ProcesaErrorHttp<T>(DtoGenericResponseHttp respuestaHttp) where T : class
//        {
//            bool validaEsquemaErr = GenericHelpers.ValidarEsquemaErr(respuestaHttp.ResultadoHttp);
//            object? erroRes = validaEsquemaErr ? GenericHelpers.TryDeserialize<T>(respuestaHttp.ResultadoHttp) : respuestaHttp.ResultadoHttp;

//            return await Task.FromResult(GenericHelpers.BuildResponse(false, erroRes, respuestaHttp.Mensaje, validaEsquemaErr));
//        }
//    }
//}
