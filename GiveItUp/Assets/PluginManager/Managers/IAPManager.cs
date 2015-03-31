using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IAPManager 
{
    private IIAPAdapter pAdapter = null;
    private Dictionary<eIAP, IAPProduct> iapList;
	
	public Action<Dictionary<eIAP, IAPProduct>> OnProductsArrived;
	public Action OnProductsSet;
    public Action<eIAP> OnPurchaseSuccess;
    public Action<eIAP, string> OnPurchaseFailed;
    public Action<eIAP> OnPurchaseCancelled;

    public IAPManager(IIAPAdapter adapter)
    {
        Init(adapter);
    }

    public void Init(IIAPAdapter adapter)
    {
        pAdapter = adapter;
        pAdapter.Init();
        OnProductsArrived += onProductsArrived;
    }

    public void PurchaseProduct(eIAP product)
    {
        if (pAdapter == null)
            Debug.LogError("Adapter is not found!");
        if (iapList == null)
            Debug.LogError("IAP is not available!");
        pAdapter.Purchase(product);
    }

    public Dictionary<eIAP, IAPProduct> GetIAPProducts()
    {
        if (pAdapter == null)
            Debug.LogError("Adapter is not found!");
        if (iapList == null)
            Debug.LogError("IAP is not available!");
        return iapList;
    }

	public IAPProduct GetIAPProduct(eIAP iapId)
	{
		if (iapList != null && iapList.ContainsKey (iapId))
			return iapList [iapId];
		return null;
	}

    public bool IsAvailable()
    {
        return iapList == null ? false : true;
    }

    public void RestorePurchase()
    {
        if (pAdapter == null)
            Debug.LogError("Adapter is not found!");
        if (iapList == null)
            Debug.LogError("IAP is not available!");
        pAdapter.RestorePurchase();
    }

    private void onProductsArrived(Dictionary<eIAP, IAPProduct> products)
    {
        iapList = products;
		if (OnProductsSet != null)
			OnProductsSet ();
    }
}
