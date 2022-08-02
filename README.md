[![codecov](https://codecov.io/gh/zotapay/dotNet-sdk/branch/master/graph/badge.svg?token=E49V7QD7XW)](https://codecov.io/gh/zotapay/dotNet-sdk)
![.NET Core Lib Test](https://github.com/zotapay/dotNet-sdk/workflows/.NET%20Core%20Build/badge.svg)

![c#-github](https://user-images.githubusercontent.com/174284/106497440-b6e9a600-64c6-11eb-9a12-83ec95725cea.jpg)

# Official .NET REST API SDK
This is the **official** page of the [Zotapay](http://www.zotapay.com) .NET SDK. It is intended to be used by 
developers who run modern .NET applications and would like to integrate our next generation payments platform.

> This SDK is also available as a NuGet package here: https://www.nuget.org/packages/Zotapay.
 
## Developer Resources

Documentation on the developer tools, various other SDKs and the API documentation this SDK implements are available on the [Zotapay Developers Portal](https://developers.zotapay.com/).

## Introduction
This .NET SDK provides all the necessary methods for integrating the Zotapay Merchant API. 
This SDK is to be used by clients, as well as all the related eCommerce plugins for .NET applications.

The SDK covers all available functionality that ZotaPay's Merchant API exposes.

### Requirements
* A functioning Zotapay Sandbox or Production account and related credentials
* netstandard2.0 (or higher)

### Supported .NET versions
|  Version  |  Release Date | Support Level | Supported Patch Version | End of Support |
| :-- | :-- | :-- | :-- | :-- |
| [.NET 5.0](https://devblogs.microsoft.com/dotnet/announcing-net-5-0/) | November 10, 2020 | Current | [5.0.1](https://dotnet.microsoft.com/download/dotnet-core/5.0) | 3 Months after .NET 6 is released |
| [.NET Core 3.1](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-1/) | December 3, 2019 | LTS | [3.1.10](https://dotnet.microsoft.com/download/dotnet-core/3.1) | December 3, 2022 |
| [.NET Core 2.1](https://blogs.msdn.microsoft.com/dotnet/2018/05/30/announcing-net-core-2-1) | May 30, 2018 | LTS | [2.1.23](https://dotnet.microsoft.com/download/dotnet-core/2.1) | August 21, 2021 |

### Installation
```sh
dotnet package add zotapay
```


## Configuration

[API CONFIGURATION DOCS](https://doc.zotapay.com/deposit/1.0/?.NET#before-you-begin)

Credentials for the SDK can be passed in 2 different ways:
1. To the `MGClient` itself
2. Through environment variables

This part of the documentation will guide you on how to configure and use this SDK.

### Before you begin

To use this API, obtain the following credentials from Zotapay:

```
MerchantID	        A merchant unique identifier, used for identification.
MerchantSecretKey	A secret key to keep privately and securely, used for authentication.
EndpointID	        One or more unique endpoint identifiers to use in API requests.
```

Contact [Zotapay](https://zotapay.com/contact/) to start your onboarding process and obtain all the credentials.

### API Url
There are two environments to use with the Zotapay API:

> Sandbox environment, used for integration and testing purposes.
`https://api.zotapay-sandbox.com`

> Live environment.
`https://api.zotapay.com`

### Configuration in the code

The implementation fo the Zotapay API SDK depends on creating an instance of the `MGClient`. First priority 
configuration is the one passed to the client itself.

Example:
```csharp
using Zotapay;

MGClient client = new MGClient(merchantSecret: "merchant-secret-key",
                                    endpointId: "400009",
                                    merchantId: "merchant-id",
                                    requestUrl: "https://secure.zotapay-stage.com");
```

Passing configuration to the client itself is best when supporting multiple clients.

### Environment variables configuration

There are 4 environment variables that need to be set for the API SDK to be configured correctly:

```
ZOTAPAY_MERCHANT_ID             - MerchantID as received from Zotapay
ZOTAPAY_MERCHANT_SECRET_KEY     - MerchantSecretKey as received from Zotapay
ZOTAPAY_ENDPOINT_ID             - EndpointID as received from Zotapay
ZOTAPAY_REQUEST_URL             - https://api.zotapay-sandbox.com or https://api.zotapay.com
```


## Usage
In order to use the SDK we need to instantiate a client:
```csharp
using Zotapay;

MGClient client = new MGClient(useConstantUrl: true, environment: Constants.MGEnvironment.Sandbox);
```

### Deposit

Construct the deposit request:

```csharp
using Zotapay.Models;

MGDepositRequest DepositOrderRequest = new MGDepositRequest
{
    MerchantOrderID = "QvE8dZshpKhaOmHY1",
    OrderAmount = "100.00",
    CustomerEmail = "customer@test.com",
    OrderCurrency = "USD",
    MerchantOrderDesc = "desc",
    CustomerFirstName = "John",
    CustomerLastName = "Doe",
    CustomerAddress = "The Moon, hill 42",
    CustomerCity = "Sofia",
    CustomerCountryCode = "BG",
    CustomerZipCode = "1303",
    CustomerPhone = "123",
    CustomerIP = "127.0.0.1",
    RedirectUrl = "https://example-merchant.com/payment/return",
    CheckoutUrl = "https://example-merchant.com/deposit",
    CallbackUrl = "https://example-merchant.com/payment/callback",
};
```

or alternatively set its properties after instantiating to see the intellisense hints with description for each parameter:


```csharp
using Zotapay.Models;

MGDepositRequest DepositOrderRequest = new MGDepositRequest();

DepositOrderRequest.MerchantOrderID = "QvE8dZshpKhaOmHY1";
DepositOrderRequest.OrderAmount = "100.00";
DepositOrderRequest.CustomerEmail = "customer@test.com";
DepositOrderRequest.OrderCurrency = "USD";
DepositOrderRequest.MerchantOrderDesc = "desc";
DepositOrderRequest.CustomerFirstName = "John";
DepositOrderRequest.CustomerLastName = "Doe";
DepositOrderRequest.CustomerAddress = "The Moon, hill 42";
DepositOrderRequest.CustomerCity = "Sofia";
DepositOrderRequest.CustomerCountryCode = "BG";
DepositOrderRequest.CustomerZipCode = "1303";
DepositOrderRequest.CustomerPhone = "123";
DepositOrderRequest.CustomerIP = "127.0.0.1";
DepositOrderRequest.RedirectUrl = "https://example-merchant.com/payment/return";
DepositOrderRequest.CheckoutUrl = "https://example-merchant.com/deposit";
DepositOrderRequest.CallbackUrl = "https://example-merchant.com/payment/callback";

```

Sending the request to Zotapay happens through the client:

```csharp
using Zotapay.Models;

MGDepositResult depositResponse = await client.InitDeposit(DepositOrderRequest);

// Check the request status
if (!depositResponse.IsSuccess) 
{
    // handle unsuccessful request
    string reasonForFailure = depositResponse.Message;
    // ...
}

// Redirect the end user to the payment url
string paymentUrl = depositResponse.Data.DepositUrl;
// ...
```


### Working with `Deposit Response`
Each deposit attempt against a Zotapay returns either a `MGDepositResult` or `MGDepositCardResult`.

The above objects are simply a wrapper around the standard HTTP response as described [here](https://doc.zotapay.com/deposit/1.0/?.NET#issue-a-deposit-request).
 
## Payout
Sending a payout request is almost identical to sending a deposit request.

The request is built:

```csharp
using Zotapay.Models;

MGPayoutRequest payoutRequest = new MGPayoutRequest
{
    MerchantOrderID = "Q44mHY18",
    CustomerBankCode = "BBL",
    OrderAmount = "100.00",
    CustomerBankAccountNumber = "1234567",
    CustomerBankAccountName = "Don Jhoe",
    CustomerEmail = "customer@test.com",
    CustomerBankBranch = "bankBranch",
    CustomerBankProvince = "province",
    CustomerBankArea = "bankArea",
    CustomerBankRoutingNumber = "000",
    OrderCurrency = "USD",
    MerchantOrderDesc = "desc",
    CustomerFirstName = "John",
    CustomerLastName = "Doe",
    CustomerBankAddress = "The Moon, hill 42",
    CustomerBankZipCode = "1303",
    CustomerPhone = "123",
    CustomerIP = "127.0.0.1",
    RedirectUrl = "https://example-merchant.com/payment/return",
    CheckoutUrl = "https://example-merchant.com/deposit",
    CallbackUrl = "https://ens39ypv7jld8.x.pipedream.net",
    CustomerCountryCode = "TH",
    CustomerPersonalID = "12345678",
    CustomerBankAccountNumberDigit = "02",
    CustomerBankAccountType = "03",
    CustomerBankSwiftCode = "123456789",
    CustomerBankBranchDigit = "04",
};

// Initiate deposit order request
MGPayoutResult resp = await client.InitPayout(payoutRequest);

// Check the request status
if (!resp.IsSuccess) 
{
    // handle unsuccessful request
    string reasonForFailure = resp.Message;
    // ...
}
```

The client returns `MGPayoutResult` which is again a wrapper around the standard HTTP response.

## Query Transaction Status

Construct the request containing the associated ids to the order: 
```csharp
MGQueryTxnRequest queryStatusCheckRequest = new MGQueryTxnRequest { MerchantOrderID = "QvE8dZshpKhaOmHY1", OrderID = "32453550" };
MGQueryTxnResult orderResponse = await client.CheckOrderStatus(queryStatusCheckRequest);

if (!orderResponse.IsSuccess) {
    // Check reason and handle failure
    string reason = orderResponse.Message;
    // ...
}

// Order status
string status = orderResponse.Data.status;
```

## Callbacks
`MGClient` has a `Parse()` method, that will validate the raw HTTP Request payload sent from Zotapay to the configured endpoint and return `MGCallback` object. 
Its purpose is to make working with callbacks manageable. Example:

```csharp
// your http server code that listens for callback ...
MGCallback callback = client.Parse(callbackJsonString);

// handle errors or unverified signatures
if (!callback.IsVerified) {
    string reason = callback.ErrorMessage;
    // ...
}
```


## Validations
All request objects passed to the `MGClient` will get validated. If an invalid member is found, the message will contain a full description and no 
actual call to the API will be made, hence the `Code` property showing the HTTP will be null. The purpose is to check whether all the values passsed to the different
parameters is in-line with what Zotapay's endpoint expects. See the API DOCS for more info and guidance about the
format of the different parameters.

## Test Coverage

[![codecov](https://codecov.io/gh/zotapay/dotNet-sdk/graphs/tree.svg?width=650&height=150&src=pr&token=E49V7QD7XW)](https://codecov.io/gh/zotapay/dotNet-sdk/)
