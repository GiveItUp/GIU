using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelEditor : MonoBehaviour 
{
    public Camera camera;
    public Storage storage;
    private int actualPos = 0;
    private int worldId = -1;
    public static bool testmode = false;
    public string filename = "";

#if UNITY_EDITOR
    private List<int> stage = new List<int>();
    public World world;

    private List<Column> i_colums = new List<Column>();
	private List<Column> info_colums = new List<Column>();
    Stage s_stage = null;
	private GameObject box = null;


	private void CreateInfo()
	{
		box = new GameObject();
		int i=0;
		float p = -6;
		foreach (Column item in world.p_columns) {
			Column column = GameObject.Instantiate(item) as Column;
			info_colums.Add(column);
			column.name = column.name+"___"+i;
			column.transform.parent = box.transform;
			column.transform.position = new Vector3(p, 5f, 0f);
			column.transform.localScale= new Vector3(0.5f,0.5f,0.5f);

			i++;
			p+=0.5f;
		}

		ReadAllStage();

	}

	private void ReadAllStage()
	{
		string str="";
		for (int i = 0; i < storage._worlds.Count; i++) {
			str+="\n"+i+"__"+storage._worlds[i].name;
			foreach (int item in storage._worlds[i].stage) {
				str+=","+item;
			}
		}
		Debug.Log(storage._worlds.Count);
		Debug.Log(str);
	}

    private void LoadMap()
    {
		string stagePath = UnityEditor.EditorUtility.OpenFilePanel("Select a stage", Application.dataPath+"/Prefabs/Stages/NewResources/", "prefab");
		s_stage = (Stage)UnityEditor.AssetDatabase.LoadAssetAtPath(stagePath.Substring(stagePath.IndexOf("Assets/")), typeof(Stage));

        if (s_stage == null)
        {
            Debug.Log("Can't load the stage!");
            return;
        }
        stage.Clear();
		string str="";
        foreach (var i in s_stage.stage)
		{
            stage.Add(i);
			str+=","+i;
		}
		Debug.Log(stagePath+"\n"+str);

       
        foreach (var col in i_colums)
        {
            GameObject.Destroy(col.gameObject);
        }
        i_colums.Clear();
        for (int i = 0; i < stage.Count; i++ )
        {
            Column col = GameObject.Instantiate(world.p_columns[stage[i]]) as Column;
            //col.transform.parent = this.transform;
            col.transform.position = new Vector3((float)i, 0f, 0f);
            i_colums.Add(col);
        }
    }

    private void CreateNewMap()
    {
        string stagePath = UnityEditor.EditorUtility.SaveFilePanel("Save stage", Application.dataPath, "new_stage", "prefab");
        s_stage = UnityEditor.PrefabUtility.CreatePrefab(stagePath.Substring(stagePath.IndexOf("Assets/")), (new GameObject("stage_" + worldId)).AddComponent<Stage>().gameObject).GetComponent<Stage>();
        stage.Clear();
        stage.Add(0);
        foreach (var col in i_colums)
        {
            GameObject.Destroy(col.gameObject);
        }
        i_colums.Clear();
        for (int i = 0; i < stage.Count; i++)
        {
            Column col = GameObject.Instantiate(world.p_columns[stage[i]]) as Column;
            col.transform.position = new Vector3((float)i, 0f, 0f);
            i_colums.Add(col);
        }
     
        actualPos = 0;
        camera.transform.position = new Vector3((float)actualPos, 2f, -10f);
    }

    private void ChangeElement(int pos)
    {
        GameObject.DestroyImmediate(i_colums[pos].gameObject);
        i_colums[pos] = GameObject.Instantiate(world.p_columns[stage[pos]]) as Column;
        i_colums[pos].transform.position = new Vector3((float)pos, 0f, 0f);
    }

    private void CreateNewElement()
    {
        stage.Add(0);
        Column col = GameObject.Instantiate(world.p_columns[stage[actualPos]]) as Column;
        col.transform.position = new Vector3((float)actualPos, 0f, 0f);
        i_colums.Add(col);
    }

    Stage CreateStagePrefab(bool test)
    {
        Stage stage = null;
        if (test)
        {
            if (storage.testStage == null)
                stage = UnityEditor.PrefabUtility.CreatePrefab("Assets/Prefabs/Stages/Resources/testStage.prefab", (new GameObject("testStage")).AddComponent<Stage>().gameObject).GetComponent<Stage>();
            else
                stage = storage.testStage;
        }
        else
        {
            if (worldId == -1)
            {
                worldId = storage._worlds.Count;
                stage = UnityEditor.PrefabUtility.CreatePrefab("Assets/Prefabs/Stages/Resources/stage_" + worldId + ".prefab", (new GameObject("stage_" + worldId)).AddComponent<Stage>().gameObject).GetComponent<Stage>();
            }
            else
                stage = storage._worlds[worldId];
        }

        return stage;
    }

    private void SaveMap(bool test = false)
    {
        if (test)
        {
            s_stage = CreateStagePrefab(true);
            s_stage.stage.Clear();
            foreach (var i in stage)
                s_stage.stage.Add(i);
            storage.testStage = s_stage;
            return;
      
        }

        s_stage.stage.Clear();
		string str="";
        foreach (var i in stage)
		{
            s_stage.stage.Add(i);
			str+=","+i;
		}

        UnityEditor.AssetDatabase.Refresh();
		Debug.Log(str);
        Debug.Log("Saved");
    }

    private void SaveMapPart()
    {
        s_stage = CreateStagePrefab(true);
        s_stage.stage.Clear();
        for (int i = actualPos; i < stage.Count; i++)
            s_stage.stage.Add(stage[i]);
        storage.testStage = s_stage;
    }

    void Start()
    {
        LoadMap();
		CreateInfo();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Create New"))
        {
            CreateNewMap();
        }

        if (GUI.Button(new Rect(10, 50, 100, 30), "Load Map"))
        {
            LoadMap();
        }

        if (GUI.Button(new Rect(10, 90, 100, 30), "Save Map"))
        {
            SaveMap();
        }

        if (GUI.Button(new Rect(10, 130, 100, 30), "Test Full"))
        {
            SaveMap(true);
            foreach (var col in i_colums)
                GameObject.Destroy(col.gameObject);
            testmode = true;
            Application.LoadLevel("Game");
        }

        if (GUI.Button(new Rect(10, 170, 100, 30), "Test From Here"))
        {
            SaveMapPart();
            foreach (var col in i_colums)
                GameObject.Destroy(col.gameObject);
            testmode = true;
            Application.LoadLevel("Game");
        }

		if (GUI.Button(new Rect(10, 210, 100, 30), "Copy"))
		{
			Copy();
		}

        GUI.Label(new Rect(Screen.width - 200, 10, 100, 30), "Lenght: " + stage.Count.ToString());
        GUI.Label(new Rect(Screen.width - 200, 40, 100, 30), "Current: " + actualPos.ToString());
        GUI.Label(new Rect(Screen.width - 200, 70, 150, 50), "Name: " + world.p_columns[stage[actualPos]].name);
		string index = GUI.TextField(new Rect(Screen.width - 200, 110, 150, 50), stage[actualPos]+"");
		if(!string.IsNullOrEmpty(index))
		{
			int indexTemp = int.Parse(index);
			if(indexTemp>=0&& indexTemp<19)
			{
				stage[actualPos]=int.Parse(index);
				ChangeElement(actualPos);
			}
		}
    }

	void Copy()
	{
		string stagePath = UnityEditor.EditorUtility.OpenFilePanel("Select a stage", Application.dataPath+"/Prefabs/Stages/NewResources/", "prefab");
		Stage stageResouces = (Stage)UnityEditor.AssetDatabase.LoadAssetAtPath(stagePath.Substring(stagePath.IndexOf("Assets/")), typeof(Stage));
		
		if (stageResouces == null)
		{
			Debug.Log("Can't load the stage!");
			return;
		}
		stagePath = UnityEditor.EditorUtility.OpenFilePanel("Select a stage", Application.dataPath+"/Prefabs/Stages/NewResources/", "prefab");
		Stage stageTarget = (Stage)UnityEditor.AssetDatabase.LoadAssetAtPath(stagePath.Substring(stagePath.IndexOf("Assets/")), typeof(Stage));
		if (stageTarget == null)
		{
			Debug.Log("Can't load the stage!");
			return;
		}
		stageTarget.stage.Clear();
		foreach (var i in stageResouces.stage)
		{
			stageTarget.stage.Add(i);
		}
		//GameObject.Destroy(stageResouces);
		UnityEditor.AssetDatabase.Refresh();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            stage[actualPos]--;
            if (stage[actualPos] < 0)
                stage[actualPos] = world.p_columns.Length - 1;
            if (stage[actualPos] >= world.p_columns.Length)
                stage[actualPos] = 0;
            ChangeElement(actualPos);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            stage[actualPos]++;
            if (stage[actualPos] < 0)
                stage[actualPos] = world.p_columns.Length - 1;
            if (stage[actualPos] >= world.p_columns.Length)
                stage[actualPos] = 0;
            ChangeElement(actualPos);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            actualPos--;
            actualPos = Mathf.Max(actualPos, 0);
            camera.transform.position = new Vector3((float)actualPos, 2f, -10f);
			box.transform.position = new Vector3((float)actualPos, 0, -10f);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            actualPos++;
            if (actualPos == stage.Count)
            {
                CreateNewElement();
            }
            camera.transform.position = new Vector3((float)actualPos, 2f, -10f);
			box.transform.position = new Vector3((float)actualPos, 0, -10f);
        }
    }
#endif
}
