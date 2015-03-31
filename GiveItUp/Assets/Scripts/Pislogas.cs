using UnityEngine;
using System.Collections;

public class Pislogas : MonoBehaviour 
{
    public PackedSprite sprite;
    public bool isMenu = false;

    void Start()
    {
        StartCoroutine(Pislog());
    }

    IEnumerator Pislog()
    {
        yield return new WaitForSeconds(Random.RandomRange(1f, 3f));
        if (sprite.CurAnimIndex != 1 && sprite.CurAnimIndex != 2)
		{
			//Debug.Log ("pislog");
            sprite.PlayAnim(0);
			if(CGame.gamelogic != null && (CGame.gamelogic.state == Gamelogic.eState.results_fail))
				SoundManager.Instance.Play(SoundManager.eSoundClip.GUI_Blink_01, 1);
		}
        StartCoroutine(Pislog());
    }

    public void SetSinging(bool singing)
    {
        if (CGame.gamelogic == null || isMenu)
            return;
        if (singing && sprite.CurAnimIndex != 1)
            sprite.PlayAnim(1);
        else
            if (!singing && sprite.CurAnimIndex == 1)
                sprite.PlayAnim(0);
    }

    void Update()
    {
        if (CGame.gamelogic != null && CGame.gamelogic.state == Gamelogic.eState.ingame && 
		    ((transform.position.x >= 37f && transform.position.x < 100f) ||
            (transform.position.x >= 133f && transform.position.x < 196f) ||
            (transform.position.x >= 261f)))
            SetSinging(true);
        else
            SetSinging(false);
    }

    public void Ouch()
    {
        if (sprite.CurAnimIndex != 1)
            sprite.PlayAnim(2);
    }
}
