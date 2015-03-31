using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAPProduct
{
    public IAPProduct (string n, string d, string fp)
    {
        name = n;
        desc = d;
        formattedPrice = fp;
    }

    public string name {get; set;}
    public string desc {get; set;}
    public string formattedPrice {get; set;}
    public string currency { get; set; }
    public string price { get; set; }
}

public abstract class IIAPAdapter
{
    abstract public void Init();
    abstract public void Purchase(eIAP product);
    abstract public bool IsAvailable();
    abstract public void RestorePurchase();
    abstract public Dictionary<eIAP, IAPProduct> GetIAPs();

    protected virtual void OnPurchaseSuccess(eIAP product)
    {
        if (PluginManager.iap.OnPurchaseSuccess != null)
            PluginManager.iap.OnPurchaseSuccess(product);
    }

    protected virtual void OnPurchaseFailed(eIAP product, string error)
    {
        if (PluginManager.iap.OnPurchaseFailed != null)
            PluginManager.iap.OnPurchaseFailed(product, error);
    }

    protected virtual void OnPurchaseCancelled(eIAP product)
    {
        if (PluginManager.iap.OnPurchaseCancelled != null)
            PluginManager.iap.OnPurchaseCancelled(product);
    }

    protected virtual void OnProductsArrived(Dictionary<eIAP, IAPProduct> iaps)
    {
        if (PluginManager.iap.OnProductsArrived != null)
            PluginManager.iap.OnProductsArrived(iaps);
    }


}
