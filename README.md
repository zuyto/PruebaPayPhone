# Wallet API - Sistema de Transferencias

## 📌 Descripción

Esta API REST permite gestionar billeteras digitales y realizar transferencias de saldo entre ellas. Implementa operaciones CRUD para billeteras y operaciones de consulta y registro para el historial de movimientos.

## 🛠️ Stack Tecnológico

- **Framework:** .NET 8
- **Arquitectura:** Clean Architecture - Onion Architecture
- **ORM:** Entity Framework Core
- **Base de Datos:** SQL Server
- **Principios de Diseño:** SOLID
- **Pruebas:** Unitarias con XUnit, Moq y AutoFixture

## 📂 Estructura del Proyecto

```
WalletAPI/
│── Wallet.API/               # Proyecto principal (API)
│── Wallet.Application/       # Lógica de negocio (Servicios, DTOs, Validaciones)
│── Wallet.Domain/            # Entidades y contratos
│── Wallet.Infrastructure/    # Repositorios, DbContext, Persistencia
│── Wallet.Tests/             # Pruebas unitarias e integración
└── README.md                 # Documentación del proyecto
```

## 🚀 Instalación y Configuración

### 1️⃣ Clonar el Repositorio

```sh
git clone https://github.com/zuyto/PruebaPayPhone.git
cd PruebaPayPhone
```

### 2️⃣ Configurar Base de Datos

1. Asegúrate de tener **SQL Server** instalado y en ejecución.
2. La cadena de conexión está en `launchSettings.json` bajo la clave `WALLET_DB_CONEXION`.


## 📌 Endpoints Principales

### 🔹 Transferencias

| Método | Endpoint                | Descripción      |
| ------ | ----------------------- | ---------------- |
| POST   | `/api/wallets/transfer` | Transferir saldo |

**Ejemplo de petición:**

```json
{
  "fromWalletId": 1,
  "toWalletId": 2,
  "amount": 50.00
}
```

## 📦 Lógica de Transferencias

### 🔹 Servicio `WalletService`

Este servicio maneja la lógica de transferencias entre billeteras.

- **Validaciones Implementadas:**

  - Monto debe ser mayor a 0.
  - La billetera de origen y destino deben existir.
  - La billetera de origen debe tener saldo suficiente.

- **Flujo de transferencia:**

  1. Se valida el monto y la existencia de las billeteras.
  2. Se descuenta el saldo de la billetera de origen.
  3. Se registra la transacción como "Débito".
  4. Se incrementa el saldo en la billetera de destino.
  5. Se registra la transacción como "Crédito".
  6. Se guardan los cambios en la base de datos.

## 🧪 Pruebas

Las pruebas se realizan con **XUnit**, **Moq** y **AutoFixture**.

### 🔹 Casos de prueba implementados:

1. **Monto inválido**: Retorna error si el monto es 0 o negativo.
2. **Billetera de origen inexistente**: Retorna error si la billetera de origen no existe.
3. **Billetera de destino inexistente**: Retorna error si la billetera de destino no existe.
4. **Saldo insuficiente**: Retorna error si la billetera de origen no tiene saldo suficiente.
5. **Transferencia exitosa**: Verifica que los cambios se aplican correctamente.

Para ejecutar las pruebas:

```sh
dotnet test
```

## 📜 Licencia

Este proyecto está bajo la licencia MIT. ¡Úsalo libremente! 🚀

Este proyecto está bajo la licencia Apache 2.0. ¡Úsalo libremente! 🚀

