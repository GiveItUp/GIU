#if UNITY_IPHONE
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAPAdapterIOS : IIAPAdapter
{
    private Dictionary<eIAP, IAPProduct> iap_products = null;
    private bool initied = false;
	public override void Init()
    {
		//Debug.Log ("IAPAdapter - Init - 1");
         if (Application.internetReachability == NetworkReachability.NotReachable || initied)
		{
			//Debug.Log ("IAPAdapter - Init - 2");
			PluginManager.OnConnectionOpened += Init;
                    return;
		}
		//Debug.Log ("IAPAdapter - Init - 3");
		PluginManager.OnConnectionOpened -= Init;
        StoreKitManager.productListReceivedEvent += OnIOSProdutcsArrived;
		StoreKitManager.productListRequestFailedEvent += OnIOSProdutcsArrivedFailed;
        StoreKitManager.purchaseSuccessfulEvent += OnIOSPurchaseSuccessfull;
        StoreKitManager.purchaseFailedEvent += OnIOSPurchaseFailed;
        StoreKitManager.purchaseCancelledEvent += OnIOSPurchaseCancelled;
        StoreKitBinding.requestProductData(PluginManager.IOS_IAPs);
        initied = true;
    }

    public override Dictionary<eIAP, IAPProduct> GetIAPs()
    {
        return iap_products;
    }

    public override bool IsAvailable()
    {
        return iap_products != null;
    }

    private eIAP lastIap = (eIAP)0;
    public override void Purchase(eIAP product)
    {
        lastIap = product;
        StoreKitBinding.purchaseProduct(PluginManager.IOS_IAPs[(int)product], 1);
    }

    public override void RestorePurchase()
    {
        StoreKitBinding.restoreCompletedTransactions();
    }

    private void OnIOSPurchaseSuccessfull(StoreKitTransaction skt)
    {
        //ReceiptValidator.VerifyTransaction(skt);
    
        for (int i = 0; i < PluginManager.IOS_IAPs.Length; i++)
        {
            if (PluginManager.IOS_IAPs[i] == skt.productIdentifier)
                OnPurchaseSuccess((eIAP)i);
        }
    }

    private void OnIOSPurchaseFailed(string error)
    {
        OnPurchaseFailed(lastIap, error);
    }

    private void OnIOSPurchaseCancelled(string error)
    {
        OnPurchaseCancelled(lastIap);
    }

    private void OnIOSProdutcsArrived(List<StoreKitProduct> products)
	{
		//Debug.Log ("IAPAdapter - OnIOSProdutcsArrived - 1 - " + products.Count);
		iap_products = new Dictionary<eIAP, IAPProduct>();
        foreach (var product in products)
        {
            for (int i = 0; i < PluginManager.IOS_IAPs.Length; i++)
            {
                if (product.productIdentifier == PluginManager.IOS_IAPs[i])
                {
                    iap_products.Add((eIAP)i, new IAPProduct(product.title, product.description, product.formattedPrice));
                    iap_products[(eIAP)i].price = product.price;
                    iap_products[(eIAP)i].currency = product.currencyCode;
                }
            }
        }
        OnProductsArrived(iap_products);
    }

	private float productListCounter = 0;
	private void OnIOSProdutcsArrivedFailed(string message)
	{
		//Debug.Log ("message = " + message); 
		if(productListCounter < 10)
		{
			StoreKitBinding.requestProductData(PluginManager.IOS_IAPs);
			productListCounter++;
		}
	}

}
#endif