using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;

public class Util
{
	public static void Shuffle<T>(List<T> list)
	{
	    RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
	    int n = list.Count;
	    while (n > 1)
	    {
	        byte[] box = new byte[1];
	        do provider.GetBytes(box);
	        while (!(box[0] < n * (Byte.MaxValue / n)));
	        int k = (box[0] % n);
	        n--;
	        T value = list[k];
	        list[k] = list[n];
	        list[n] = value;
	    }
	}

    public static void CallWithDelay(System.Action action, float delay)
    {
        CGame.Instance.StartCoroutine(IE_CallWithDelay(action, delay));
    }

    private static IEnumerator IE_CallWithDelay(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
	
	public static Vector3 GetScreenPos(Vector3 pos)
	{
		Vector2 tmp = new Vector2(pos.x, pos.y);
		tmp = tmp / 768f * (float)Screen.height;
		return new Vector3(tmp.x, tmp.y, pos.z);
	}
	
	public static Vector3 GetWorldPos(Vector3 pos)
	{
		Vector2 tmp = new Vector2(pos.x, pos.y);
		tmp = tmp / (float)Screen.height * 768f;
		return new Vector3(tmp.x, tmp.y, pos.z);
	}
	
	public static float GetScreenPos(float pos)
	{
		return pos / 768f * (float)Screen.height;
	}
	
	public static float GetWorldPos(float pos)
	{
		return pos / (float)Screen.height * 768f;
    }

    public static string TimeToStr(float time)
    {
        if (time > 0)
        {
            int m = (int)time / 60;
            string sm = ((m < 10) ? "0" : "") + m;
            int s = (int)time % 60;
            string ss = ((s < 10) ? "0" : "") + s;
            return sm + ":" + ss;
        }
        return "00:00";
    }

    public static float Sinerp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
    }
	
	public static Vector3 Hermite(Vector3 V1, Vector3 T1, Vector3 V2, Vector3 T2, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float h1 = 2 * t3 - 3 * t2 + 1;
        float h2 = -2 * t3 + 3 * t2;
        float h3 = t3 - 2 * t2 + t;
        float h4 = t3 - t2;

        return V1 * h1 + V2 * h2 + T1 * h3 + T2 * h4;
    } 
    
    public static Vector3 CatmulRom(List<Vector3> points, float t)
    {
        int sp = Mathf.FloorToInt(t);
        return CatmulRom(points[sp], points[sp+1], points[sp+2], points[sp+3], t-sp);
    }
    public static Vector3 CatmulRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 result = new Vector3();
        
        float t0 = ((-t + 2f) * t - 1f) * t * 0.5f;
        float t1 = (((3f * t - 5f) * t) * t + 2f) * 0.5f;
        float t2 = ((-3f * t + 4f) * t + 1f) * t * 0.5f;
        float t3 = ((t - 1f) * t * t) * 0.5f;
        
        result.x = p0.x * t0 + p1.x * t1 + p2.x * t2 + p3.x * t3;
        result.y = p0.y * t0 + p1.y * t1 + p2.y * t2 + p3.y * t3;
        result.z = p0.z * t0 + p1.z * t1 + p2.z * t2 + p3.z * t3;
        
        return result;
    }

    public static string FormatCash(int cash)
    {
        string str = cash.ToString("#,##0");
        return str.Replace(",", ".");
    }

    public static List<int> CreateIntListForResults(int param)
    {
        List<int> ret = new List<int>();
        float step = param / (Mathf.Pow(param, 0.25f) * 4);
        for (float i = 0; i <= param; i += step)
        {
            ret.Add((int)Mathf.Min(i, param));
        }
        ret.Add(param);
        return ret;
    }

    public static float AngleDiff(float y0, float y1)
    {
        float diff = y1 - y0;
        if (diff >= 180f) diff = diff - 360f;
        if (diff <= -180f) diff = 360f + diff;
        return diff;
    }

	public static Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	public static string ColorToHexString(Color color) 
	{
		//Color32 color32 = new Color(color.r / 2f, color.g / 2f, color.b / 2f);
		Color32 color32 = color;
		return color32.r.ToString ("X2") + color32.g.ToString ("X2") + color32.b.ToString ("X2");
	}

    public static void SetLayerRecursively( Transform root, int layer)
    {
        root.gameObject.layer = layer;
        
        for (int i=0; i<root.childCount; i++)
        {
            SetLayerRecursively(root.GetChild(i), layer);
        }
    }
    
    public static DateTime GetIPhoneDocumentsFolderCreationDate()
    {
        string path = Application.dataPath.Substring( 0, Application.dataPath.Length - 5);
        path = path.Substring( 0, path.LastIndexOf('/'));
        path = path + "/Documents/";
                
        if (System.IO.Directory.Exists(path))
        {
            return System.IO.Directory.GetCreationTime(path);
        }
        else
        {
            return DateTime.MinValue;
        }
        
        
	}

	private static float lastTime = 0f;
	public static void LogTime()
	{
		if (lastTime == 0)
			Debug.Log ("-------start-------------");
		else
			Debug.Log ("time = " + (Time.realtimeSinceStartup - lastTime));

		lastTime = Time.realtimeSinceStartup;
	}


	public static void LogByteArray(byte[] array)
	{
		Debug.Log ( "Byte array log: ");
		string s = "";
		for (int i=0; i<array.Length; i++) {
			s += array[i].ToString() + " ";
		}
		Debug.Log (s);
	}

	static readonly string PasswordHash = "kutyaKUKTA25incs";
	static readonly string SaltKey = "s@1TkulcsG";
	static readonly string IVKey = "@1B2c3D4e5F6g7H8";
	public static string Encrypt(string plainText)
	{
		byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
		
		byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, System.Text.Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);

		var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
		var encryptor = symmetricKey.CreateEncryptor(keyBytes, System.Text.Encoding.ASCII.GetBytes(IVKey));

		byte[] cipherTextBytes;
		
		using (var memoryStream = new System.IO.MemoryStream())
		{
			using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
			{
				cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
				cryptoStream.FlushFinalBlock();
				cipherTextBytes = memoryStream.ToArray();
				cryptoStream.Close();
			}
			memoryStream.Close();
		}
		return Convert.ToBase64String(cipherTextBytes);
	}

	public static string Decrypt(string encryptedText)
	{
		byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
		byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, System.Text.Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
		var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
		
		var decryptor = symmetricKey.CreateDecryptor(keyBytes, System.Text.Encoding.ASCII.GetBytes(IVKey));
		var memoryStream = new System.IO.MemoryStream(cipherTextBytes);
		var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
		byte[] plainTextBytes = new byte[cipherTextBytes.Length];
		
		int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
		memoryStream.Close();
		cryptoStream.Close();
		return System.Text.Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
	}

}
