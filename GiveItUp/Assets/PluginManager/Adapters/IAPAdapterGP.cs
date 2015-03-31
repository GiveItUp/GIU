#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAPAdapterGP : IIAPAdapter 
{
    private bool isInitied = false;
    private Dictionary<eIAP, IAPProduct> iap_products = null;

    public override void Init()
    {
		Debug.Log ("iab init");
        GoogleIAB.init(PluginManager.ANDROID_PublicKey);
        GoogleIABManager.billingSupportedEvent += OnBillingSupported;
        GoogleIABManager.queryInventorySucceededEvent += OnQueryInventory;
		GoogleIABManager.queryInventoryFailedEvent += (string sim) => {Debug.Log ("Query failed!" + sim); };
		GoogleIABManager.billingNotSupportedEvent += (string sim) => {Debug.Log ("Billing not supported!" + sim); };
        GoogleIABManager.purchaseSucceededEvent += OnPurchaseSuccessGP;
        GoogleIABManager.purchaseFailedEvent += (string error) => { OnPurchaseFailed(latest, error); };
    }

    public override bool IsAvailable()
    {
        return isInitied;
    }

    public override Dictionary<eIAP, IAPProduct> GetIAPs()
    {
        return iap_products;
    }

    public override void RestorePurchase()
    {
        if (isInitied)
            GoogleIAB.queryInventory(PluginManager.ANDROID_IAPs);
        else
        {
            OnPurchaseFailed(latest, "GoogleIAB is not initied!");
        }
    }

    private eIAP latest = (eIAP)0;

    public override void Purchase(eIAP product)
    {
        latest = product;
        if (isInitied)
            GoogleIAB.purchaseProduct(PluginManager.ANDROID_IAPs[(int)product]);
        else
            OnPurchaseFailed(latest, "GoogleIAB is not initied!");
    }

    private void OnBillingSupported()
    {
		Debug.Log ("billing is supported");
        GoogleIAB.queryInventory(PluginManager.ANDROID_IAPs);
    }

    private void OnPurchaseSuccessGP(GooglePurchase purchase)
    {
        if (purchase == null)
            return;
        
            for (int i = 0; i < PluginManager.ANDROID_IAPs.Length; i++)
                if (purchase.productId == PluginManager.ANDROID_IAPs[i])
                {
                    OnPurchaseSuccess((eIAP)i);
					//GoogleIAB.consumeProduct(purchase.productId);
					PlayerPrefs.SetInt("Continue", 1);
                    break;
                }
    }

    private void OnQueryInventory(List<GooglePurchase> purchases, List<GoogleSkuInfo> infos)
    {
		Debug.Log ("query");
        if (infos == null)
            return;
        iap_products = new Dictionary<eIAP, IAPProduct>();
        foreach (var info in infos)
        {
            IAPProduct product = new IAPProduct(info.title, info.description, info.price);
            for (int i = 0; i < PluginManager.ANDROID_IAPs.Length; i++)
                if (PluginManager.ANDROID_IAPs[i] == info.productId)
                {
					Debug.Log(info.productId);
                    iap_products.Add((eIAP)i, product);
                    break;
                }
        }
        isInitied = true;
        foreach (var purchase in purchases)
            OnPurchaseSuccessGP(purchase);
        OnProductsArrived(iap_products);
    }
}
#endif