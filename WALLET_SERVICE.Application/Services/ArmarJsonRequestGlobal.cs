// <copyright file="ArmarJsonRequestGlobal.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

//using Dasync.Collections;
//using Newtonsoft.Json;
//using WALLET_SERVICE.Application.Common.Helpers;
//using WALLET_SERVICE.Application.Common.Interfaces.ArmarJson;
//using WALLET_SERVICE.Application.Common.Interfaces.Repository;
//using WALLET_SERVICE.Application.Common.Interfaces.Services.Serilog;
//using WALLET_SERVICE.Application.Common.Models.DTOs;
//using WALLET_SERVICE.Application.Common.Models.DTOs.DtoBase;
//using WALLET_SERVICE.Application.Common.Static;
//using WALLET_SERVICE.Application.Common.Struct;
//using System.Collections.Concurrent;
//using System.Diagnostics.CodeAnalysis;

//namespace WALLET_SERVICE.Application.Services
//{
//    [ExcludeFromCodeCoverage]
//    public class ArmarJsonRequestGlobal : IArmarJsonRequestGlobal
//    {
//        private readonly ISerilogImplements _serilogImplements;
//        private readonly IUnitOfWorkGspl _unitOfWorkGspl;
//        private readonly IUnitOfWorkProdUniProd _unitOfWorkProdUniProd;

//        public ArmarJsonRequestGlobal(ISerilogImplements serilogImplements, IUnitOfWorkGspl unitOfWorkGspl, IUnitOfWorkProdUniProd unitOfWorkProdUniProdl)
//        {
//            _serilogImplements = serilogImplements;
//            _unitOfWorkGspl = unitOfWorkGspl;
//            _unitOfWorkProdUniProd = unitOfWorkProdUniProdl;
//        }

//        public async Task<DtoGenericResponse> ArmarJsonRequestTrxCargueMasivo(DtoTransferJsonRequest request)
//        {


//            DtoGenericResponse resultNpedido = await _unitOfWorkGspl.OrquestadorVeRepository.ObtenerDataSalidaEntradaCrgVte(request);

//            if (!resultNpedido.EsExitoso)
//            {
//                return GenericHelpers.BuildResponse(false, resultNpedido.Resultado, resultNpedido.Mensaje);
//            }




//            List<DtoTrxDetalleMasivo> datosSalidaEntradaCrgVte = JsonConvert.DeserializeObject<List<DtoTrxDetalleMasivo>>(JsonConvert.SerializeObject(resultNpedido.Resultado));

//            datosSalidaEntradaCrgVte = await AsignarCostoPorSticker(datosSalidaEntradaCrgVte);



//            var groupedDetalles = datosSalidaEntradaCrgVte.GroupBy(d => d.Sticker).Select(g => new DtoTrxTransaccionMasivo
//            {
//                Sticker = g.Key,
//                LstDetalles = g.ToList()
//            }).ToList();


//            var responseDto = new DtoTrxCargueMasivo
//            {
//                Opcion_Menu = Constantes.OPCION_MENU,
//                Usuario_Login = Constantes.USUARIO_LOGIN,
//                LstTransacciones = groupedDetalles
//            };

//            _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(responseDto, Formatting.Indented), null, "\n\n **** ARMADO JSON REQUETS CARGUE MASIVO **** \n");

//            return GenericHelpers.BuildResponse(true, responseDto, UserTypeMessages.OKGEN01);
//        }

//        public async Task<DtoGenericResponse> ArmarJsonRequestTrxTrackingInventario(object? resjsonRequestCargueMasivo, object? responseHttpCargueMasivo)
//        {
//            DtoTrxCargueMasivo jsonRequestCargueMasivo = JsonConvert.DeserializeObject<DtoTrxCargueMasivo>(JsonConvert.SerializeObject(resjsonRequestCargueMasivo));
//            DtoGenericResponse resHttpCargueMasivo = JsonConvert.DeserializeObject<DtoGenericResponse>(responseHttpCargueMasivo.ToString());

//            List<DtoTrxResultCargaMasivo> trxCargueMasivo = JsonConvert.DeserializeObject<List<DtoTrxResultCargaMasivo>>(resHttpCargueMasivo.Resultado.ToString());


//            List<DtoTrxTrackingInventario> trxTracking = jsonRequestCargueMasivo.LstTransacciones
//                                                                                .SelectMany(trans => trans.LstDetalles)
//                                                                                .Where(detail => detail.IdTrasaccion.HasValue)
//                                                                                .GroupBy(detail => detail.IdTrasaccion.Value)
//                                                                                .Select(group => new DtoTrxTrackingInventario
//                                                                                {
//                                                                                    Id_Transaccion = (int?)group.Key,
//                                                                                    LstTransacciones = group.Select(detail => new DtoTrxStickerTracking
//                                                                                    {
//                                                                                        Trans_Session = trxCargueMasivo?.Where(x => x.Sticker == detail?.Sticker).Select(x => new string(x.Mensaje.Where(char.IsDigit).ToArray())).FirstOrDefault(),
//                                                                                        Sticker = detail.Sticker,
//                                                                                        Cantidad = (int?)detail.CANTIDAD,
//                                                                                        Sku = detail.PRD_LVL_NUMBER,
//                                                                                        Child_Origen = (int?)detail.ORG_LVL_CHILD
//                                                                                    }).ToList()
//                                                                                }).OrderBy(x => x.Id_Transaccion).ToList();

//            _serilogImplements.ObtainMessageDefault(ConfigurationMessageType.Information, JsonConvert.SerializeObject(trxTracking, Formatting.Indented), null, "\n\n ****  ARMADO JSON REQUETS TRACKING INVENTARIO  **** \n");

//            if (!trxTracking.Any())
//            {

//                return await Task.FromResult(GenericHelpers.BuildResponse(false, trxTracking, UserTypeMessages.ERRGEN07));
//            }

//            return await Task.FromResult(GenericHelpers.BuildResponse(true, trxTracking, UserTypeMessages.OKGEN01));
//        }




//        private async Task<List<DtoTrxDetalleMasivo>> AsignarCostoPorSticker(List<DtoTrxDetalleMasivo>? datosSalidaEntradaCrgVte)
//        {
//            ConcurrentDictionary<string, decimal?> costosPorSticker = new ConcurrentDictionary<string, decimal?>();

//            await datosSalidaEntradaCrgVte.ParallelForEachAsync(async detalle =>
//            {
//                string sticker = detalle.Sticker;

//                // Verificar si ya hemos obtenido el costo para este sticker
//                if (!costosPorSticker.TryGetValue(sticker, out decimal? costo))
//                {
//                    // Si no se ha obtenido el costo para este sticker, lo obtenemos
//                    costo = await _unitOfWorkProdUniProd.ProdUniProdRepository.ObtenerCostoProducto(detalle.PRD_LVL_CHILD, detalle.ORG_LVL_CHILD);

//                    // Agregar el costo al ConcurrentDictionary
//                    costosPorSticker.TryAdd(sticker, costo);
//                }

//                // Asignar el costo al detalle actual
//                detalle.COSTO = costo;

//            });




//            return datosSalidaEntradaCrgVte;
//        }
//    }
//}
