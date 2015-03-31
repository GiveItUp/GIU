using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{   
    public Column[] p_columns;
    List<Column> columns = new List<Column>();
    public int mapId = 0;


    public void Init(Stage stage)
    {
        Column c;
        List<int> actualMap;
        
        if (LevelEditor.testmode)
            actualMap = Storage.Instance.testStage.stage;
        else
            actualMap = stage.stage;

        actualMap[actualMap.Count - 10] = 19;

        for (int i = 0; i < actualMap.Count; i++)
        {
            c = GameObject.Instantiate(p_columns[actualMap[i]]) as Column;
            c.transform.parent = transform;
            c.transform.position = Vector3.right * i;
            
			if (c.isCatchesJumpDelegate)
			{
				if (CGame.gamelogic == null)
                    Debug.Log("nyúl");
				CGame.gamelogic.OnJumped += c.OnJumped;
			}

            columns.Add(c);
        }

        Debug.Log("World size: " + (columns.Count - 10));

        ClearColumnColor();
    }

    public Column GetFirstColumn()
    {
        return columns[0];
    }
    public Column GetCurrentColumn(float x)
    {
        return columns[Mathf.RoundToInt(x)];
    }
    public Column GetNextColumn(float x, int i)
    {
        int idx = Mathf.RoundToInt(x) + i;
        if (idx < 0)
        {
        	return null;
        }
        return idx < columns.Count ? columns[idx] : null;
    }
    public float GetProgress(float x)
    {
        return x / (columns.Count - 10);
    }
    public bool IsItTheEnd(float x)
    {
        return Mathf.RoundToInt(x) >= columns.Count - 10;
    }
    public void ClearColumnColor()
    {
        foreach (var c in columns)
            c.SetColor(0);
    }

    public void FallPlatforms(bool myplatform)
    {
        int startPos = (int)CGame.gamelogic.ball.transform.position.x - 10;
        startPos = Mathf.Max(0, startPos);
        for (int i = startPos; i < columns.Count; i++)
            if (!myplatform || (i != (int)CGame.gamelogic.ball.transform.position.x + 1))
                CGame.Instance.StartCoroutine(IE_FallPlatform(i));
    }

    private IEnumerator IE_FallPlatform(int i)
    {
        yield return new WaitForSeconds(Random.value * 0.3f);
        columns[i].PlayFallAnim();
    }
}
