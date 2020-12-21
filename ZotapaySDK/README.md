# ZotaPay DotNetSDK

### Installation
TODO: tweak the actual command
```sh
nuget install zotapaysdk
```

### How to use
The main engine of ZotaPay is its MetaGate Client. It can be instantiated with environment variables or with the second explicit constructor.
- For use with variables the following must be set in the environment (provided by ZotaPay):
```
"ZOTAPAY_MERCHANT_ID"
"ZOTAPAY_MERCHANT_SECRET_KEY"
"ZOTAPAY_ENDPOINT_ID"
"ZOTAPAY_REQUEST_URL"
```

Example code:
```C#
// Environment variables are set in the only code for showcase purposes
// For security common sense this method of setting is not to be used on live
Environment.SetEnvironmentVariable(Constants.ENV.ENDPOINT_ID, "400009");
Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_ID, "merchant-id");
Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_SECRET_KEY, "merchant-secret-key");

// ZotaPay's Meta Gate client constructed with environment variables
MGClient client = new MGClient(useConstantUrl: true, environment: Constants.MGEnvironment.Sandbox);
```

- For convenience there is an explicit contructor used as such:
```C#
MGClient clientWithConfig = new MGClient(merchantSecret: "merchant-secret-key",
                                    endpointId: "400009",
                                    merchantId: "merchant-id",
                                    requestUrl: "https://secure.zotapay-stage.com");
```

### Deposit Order Request
Example Code:
```C#    
// Assemble deposit order data
var DepositOrderRequest = new MGDepositRequest
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

// Initiate deposit order request
MGDepositResult resp = await client.InitDeposit(DepositOrderRequest);

// Check the request status
if (!resp.IsSuccess) 
{
    // handle unsuccessful request
    string reasonForFailure = resp.Message;
    // ...
}

// Redirect the end user to the payment url
string paymentUrl = resp.Data.DepositUrl;
// ...
```

### Order Status Query

```C#
// Initialize query status payload & send request
var queryStatusCheckRequest = new MGQueryTxnRequest { MerchantOrderID = "QvE8dZshpKhaOmHY1", OrderID = "32453550" };
MGQueryTxnResult orderResponse = await client.CheckOrderStatus(queryStatusCheckRequest);

if (!orderResponse.IsSuccess) {
    // Check reason and handle failure
    string reason = orderResponse.Message;
    // ...
}

// Order status
string status = orderResponse.Data.status;
```

### Payout Order Request
```C#
// Assemble payout order data
var payoutRequest = new MGPayoutRequest
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