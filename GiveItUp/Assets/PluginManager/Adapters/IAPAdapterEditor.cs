using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAPAdapterEditor : IIAPAdapter
{
    private bool initied = false;
    private Dictionary<eIAP, IAPProduct> products = null;

    public override void Init()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PluginManager.OnConnectionOpened += Init;
            return;
        }

        PluginManager.OnConnectionOpened -= Init;

        products = new Dictionary<eIAP, IAPProduct>();
        for (int i = 0; i < System.Enum.GetValues(typeof(eIAP)).Length; i++)
        {
            products.Add((eIAP)i, new IAPProduct("iap" + i.ToString(), "desc" + i.ToString(), "huf" + Random.Range(1, 10)));
        }
        initied = true;
        PluginManager.Instance.StartCoroutine(IE_OnProductsArrived());
    }

    public override Dictionary<eIAP, IAPProduct> GetIAPs()
    {
        return null;
    }

    IEnumerator IE_OnProductsArrived()
    {
        yield return new WaitForSeconds(1f);
        OnProductsArrived(products);
    }

    public override bool IsAvailable()
    {
        return (products != null) && (Application.internetReachability != NetworkReachability.NotReachable);
    }

    public override void Purchase(eIAP product)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnPurchaseFailed(product, "No Connection");
            return;
        }

        if (!initied)
        {
            Debug.LogError("Not initied!");
            return;
        }

        OnPurchaseSuccess(product);
    }

    public override void RestorePurchase()
    {
        
    }
    
}
