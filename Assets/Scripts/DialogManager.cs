using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public enum DialogType
    {
        tutorial_heal,
        tutorial_box,
        introduce,
        tutorial_entrance,
        tutorial_weightpad,
        tutorial_combat,
        chichat,
        levelTwo_hint
    }

    public struct WhoSayWhat
    {
        public CharactorBase who;
        public string what;

        public WhoSayWhat(CharactorBase c, string w)
        {
            who = c;
            what = w;
        }
    }

    public CharactorBase odetta;
    public CharactorBase ice;
    public DialogType pendingDialogType;
    public float timePerChar = 2f / 30f;

    Dictionary<DialogType, List<WhoSayWhat>> dialogDic;

    // Data used in heal tutorial
    public bool eatIce, isHalfHealed;
    // Data used in box tutorial
    public bool isJumped;
    // Data used in door tutorial
    public bool isDoorOpen, isDoorClosed, isDoorVisible, isPassTheDoor;
    // Data used in combat tutorial
    public bool isPassMonsters;

    protected Text topHint;
    protected int popIndex;

    void Awake ()
    {
        timePerChar = 2f / 30f;
        // Initialization for data 
        eatIce = false;
        isHalfHealed = false;
        isJumped = false;
        isDoorClosed = false;
        isDoorOpen = false;
        isDoorVisible = false;
        isPassTheDoor = false;
        isPassMonsters = false;

        topHint = GameObject.Find("TopHint").GetComponent<Text>();
        topHint.enabled = false;
        popIndex = 0;

        dialogDic = new Dictionary<DialogType, List<WhoSayWhat>> ();

        // Tutorial of healing
        dialogDic[DialogType.tutorial_heal] = new List<WhoSayWhat>();
        // randomly, repeatly, until odetta eat a ice
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(odetta, "Ice..."));
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(odetta, "I need ice..."));
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(odetta, "Help me please..."));

        // randomly, repeatly, after odetta eat a ice
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(odetta, "Feel better."));
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(odetta, "Can I have more ice?"));

        // When ice get closer
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(ice, "(Ice? I can make an ice.)"));

        // After odetta got 1/2 healed
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(odetta, "Thank you for saving me."));
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(ice, "Not a big deal. We both want to escape from here."));
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(odetta, "This is not a right place for me. I need ice to make me alive."));
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(ice, "You are so lucky. I can make ice for you. This is my only ability."));
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(odetta, "Thanks."));
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(ice, "But I can't beat all monster out there. You look powerful. I need your help."));
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(odetta, "No problem. We can group up."));
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(ice, "Great! Let's go!"));

        // When odetta die
        dialogDic[DialogType.tutorial_heal].Add(new WhoSayWhat(ice, "She died..."));

        // Tutorial for box
        dialogDic[DialogType.tutorial_box] = new List<WhoSayWhat>();
        dialogDic[DialogType.tutorial_box].Add(new WhoSayWhat(ice, "How do we get up to that platform?"));
        dialogDic[DialogType.tutorial_box].Add(new WhoSayWhat(odetta, "Don't worry. I can grab that box over here."));
        dialogDic[DialogType.tutorial_box].Add(new WhoSayWhat(ice, "Oh, great!"));

        // Introduce
        dialogDic[DialogType.introduce] = new List<WhoSayWhat>();
        dialogDic[DialogType.introduce].Add(new WhoSayWhat(odetta, "Er, we haven't introduce ourselves yet."));
        dialogDic[DialogType.introduce].Add(new WhoSayWhat(odetta, "My name is Odetta."));
        dialogDic[DialogType.introduce].Add(new WhoSayWhat(ice, "My name..."));
        dialogDic[DialogType.introduce].Add(new WhoSayWhat(ice, "... just call me Ice."));
        dialogDic[DialogType.introduce].Add(new WhoSayWhat(odetta, "... oh, well."));
        dialogDic[DialogType.introduce].Add(new WhoSayWhat(ice, "..."));

        // Tutorial for entrance
        dialogDic[DialogType.tutorial_entrance] = new List<WhoSayWhat>();
        dialogDic[DialogType.tutorial_entrance].Add(new WhoSayWhat(odetta, "There is the entrance to next area. You ready?"));
        dialogDic[DialogType.tutorial_entrance].Add(new WhoSayWhat(ice, "Yeah. Let's do it."));
        dialogDic[DialogType.tutorial_entrance].Add(new WhoSayWhat(odetta, "OK."));

        // Tutorial for weight pad
        dialogDic[DialogType.tutorial_weightpad] = new List<WhoSayWhat>();

        // When door is visible
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(odetta, "There is a door over there."));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(ice, "Uh, we should open it."));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(ice, "You see, there is something up there."));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(ice, "We should check it out."));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(odetta, "Sure."));

        // when door is open
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(odetta, "It's opening!"));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(ice, "Let's go."));

        // When door is closed
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(ice, "That's not how it works."));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(ice, "We need something to hold it."));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(odetta, "You see the sign next to the trigger?"));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(ice, "It has been scrawled out..."));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(ice, "Oh, wait! Maybe I can put my ice on it."));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(odetta, "Oh! You're right!"));

        // After pass the door
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(odetta, "We got it!."));
        dialogDic[DialogType.tutorial_weightpad].Add(new WhoSayWhat(ice, "Let's go!"));

        // Combat tutorial
        dialogDic[DialogType.tutorial_combat] = new List<WhoSayWhat>();

        // When normal monster is visible ( 0-7 )
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "There is a monster."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "It's called spitbomb. I can take it down easily."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "I can do little damage... but, better than nothing."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "Sure..."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "... but I will lose more enegy if I fight. I need your help."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "No problem."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "I'm so glad that I can do a favor."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "Let's do it!"));

        // When fire monster is visible ( 8-13 )
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "This one is spitfire."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "I can do more damage to it, but I will also get hurt by its fire."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "Is it scared of ice?"));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "So badly."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "Then let me protect you."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "Thank you..."));

        // when no monster die ( 14-15 ) 
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "It's not bad to flee away."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "Yeah, it's important to survive from this place."));

        // when one monster die ( 16-18 )
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "Hard to beat them all."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "It's better to be alive."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "You're right."));

        // when two monster die ( 19-23 )
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "It's so nice to have you around me."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "The same to me."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "We work perfectly with each other."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "I think we will escape from this place."));
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "Right!"));

        // when first monster die ( 24 )
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "Nice!"));
        // when ice get demage ( 25 )
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "Watch out!"));
        // when second monster die ( 26 )
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "Ah!"));
        // when ice die ( 27 )
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(odetta, "No!!!"));
        // when odetta die ( 28 )
        dialogDic[DialogType.tutorial_combat].Add(new WhoSayWhat(ice, "Odetta! No!!!"));

        // 
        dialogDic[DialogType.chichat] = new List<WhoSayWhat>();
        // Randomly, got damaged ( 0-4 )
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(ice, "Odetta!"));
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(odetta, "Watch out!"));
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(ice, "We need a clever way."));
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(odetta, "It won't be hard to beat."));
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(ice, "Oops!"));
        // when ice die ( 5 )
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(odetta, "No!!!"));
        // when odetta die ( 6 )
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(ice, "Odetta! No!!!"));
        // when odetta's health is lower 1/2 ( 7-8 )
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(odetta, "I'm getting slower."));
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(ice, "Do you need some ice?"));
        // when odetta lower than 1/3 ( 9-10 )
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(odetta, "I think I need ice."));
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(ice, "Odetta, let me heal you."));
        // when odetta lower than 1/4 ( 11-12 )
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(odetta, "I can't fight anymore."));
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(ice, "You will die! Get some cure!"));

        // when get closer to entrance ( 13-16 )
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(odetta, "Where are we heading to?"));
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(ice, "I don't know."));
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(ice, "But we can't come back anymore."));
        dialogDic[DialogType.chichat].Add(new WhoSayWhat(odetta, "......"));

        // Level 2 hint
        dialogDic[DialogType.levelTwo_hint] = new List<WhoSayWhat>();
        // ( 0-3 )
        dialogDic[DialogType.levelTwo_hint].Add(new WhoSayWhat(odetta, "Where is the end of this place?"));
        dialogDic[DialogType.levelTwo_hint].Add(new WhoSayWhat(ice, "I dont's know..."));
        dialogDic[DialogType.levelTwo_hint].Add(new WhoSayWhat(ice, "... but I believe there is a way."));
        dialogDic[DialogType.levelTwo_hint].Add(new WhoSayWhat(odetta, "I trust you!"));

        // After 10s not passing the door ( 4-5 )
        dialogDic[DialogType.levelTwo_hint].Add(new WhoSayWhat(ice, "I think we should make use of those boxes."));
        dialogDic[DialogType.levelTwo_hint].Add(new WhoSayWhat(odetta, "Let's figure it our."));

        // After passing the door ( 6-7 )
        dialogDic[DialogType.levelTwo_hint].Add(new WhoSayWhat(odetta, "We got it!"));
        dialogDic[DialogType.levelTwo_hint].Add(new WhoSayWhat(ice, "Good job!"));
    }

    // Use this for initialization
    void Start () {
        if (SceneManager.GetActiveScene().name == "tutorial")
        {
            pendingDialogType = DialogType.tutorial_heal;
            StartCoroutine(TutorialHealing());
        }
        else if (SceneManager.GetActiveScene().name == "tutorial2")
        {
            pendingDialogType = DialogType.tutorial_weightpad;
            StartCoroutine(TutorialWeightPad());
        }
        else if (SceneManager.GetActiveScene().name == "tutorial3")
        {
            pendingDialogType = DialogType.tutorial_combat;
            StartCoroutine(TutorialCombat());
        }
        else if (SceneManager.GetActiveScene().name == "level1")
        {
            pendingDialogType = DialogType.chichat;
            StartCoroutine(GetDamageChecking());
            StartCoroutine(IceDieChecking());
            StartCoroutine(OdettaDieChecking());
            StartCoroutine(OdettaHealthChecking());
            StartCoroutine(GetToEntranceChecking());
        }
        else if (SceneManager.GetActiveScene().name == "level2")
        {
            pendingDialogType = DialogType.levelTwo_hint;
            StartCoroutine(Level2Hint());
            StartCoroutine(GetDamageChecking());
            StartCoroutine(OdettaDieChecking());
            StartCoroutine(OdettaHealthChecking());
        }
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    // Show top hint text
    IEnumerator PopUpHintText(string s, float t)
    {
        topHint.enabled = true;
        popIndex++;
        int tempIndex = popIndex;
        topHint.text = s;
        float timestamp = Time.time;
        while (Time.time - timestamp <= t && tempIndex == popIndex)
        {
            yield return null;
        }

        if (popIndex == tempIndex)
        {
            topHint.enabled = false;
        }
    }

    // Healing Tutorial asking for help
    IEnumerator TutorialHealingHelp()
    {
        while (!eatIce)
        {
            int helpingIndex = Random.Range(0, 3);
            dialogDic[pendingDialogType][helpingIndex].who.charDialog.PopupDialog(dialogDic[pendingDialogType][helpingIndex].what, 2f);
            yield return new WaitForSeconds(2f);
        }
        while (!isHalfHealed)
        {
            int helpingIndex = Random.Range(3, 5);
            dialogDic[pendingDialogType][helpingIndex].who.charDialog.PopupDialog(dialogDic[pendingDialogType][helpingIndex].what, 2f);
            yield return new WaitForSeconds(2f);
        }
    }

    // Healing tutorial checking
    IEnumerator TutorialHealingFlagChecking()
    {
        float previousHealth = odetta.healthSlider.maxValue - 75f;
        while (!eatIce)
        {
            float currentHealth = odetta.healthSlider.value;
            if (currentHealth > previousHealth)
            {
                eatIce = true;
            }
            previousHealth = currentHealth;
            yield return null;
        }

        while (!isHalfHealed)
        {
            if (odetta.healthSlider.value >= 0.5f * odetta.healthSlider.maxValue)
            {
                isHalfHealed = true;
            }
            yield return null;
        }
    }

    IEnumerator TutorialHealingOdettaDie()
    {
        while (odetta)
        {
            yield return null;
        }
        ice.charDialog.StopAllCoroutines();
        for (int i= dialogDic[pendingDialogType].Count-1; ice && i < dialogDic[pendingDialogType].Count; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }
    }

    // Main healing tutorial coroutine
    IEnumerator TutorialHealing()
    {
        // Start ask for help
        StartCoroutine(TutorialHealingHelp());
        StartCoroutine(TutorialHealingOdettaDie());

        int dialogIndex = 5;
        odetta.healthChange(-75f);
        // Given some time to let health value decrease
        yield return new WaitForSeconds(1f);

        // Pop up hint
        StartCoroutine(PopUpHintText("Use WASD to move Player 1 and Arrow key to move Player 2", 10f));

        // Start to check flags
        StartCoroutine(TutorialHealingFlagChecking());

        while (odetta && Vector3.Distance(ice.transform.position, odetta.transform.position) > 10f)
        {
            yield return null;
        }

        StartCoroutine(PopUpHintText("Hold right mouse button to create an ice", 10f));

        if (ice && odetta)
        {
            dialogDic[pendingDialogType][dialogIndex].who.charDialog.PopupDialog(dialogDic[pendingDialogType][dialogIndex].what, 2f);
        }
        dialogIndex++;

        while (!isHalfHealed)
        {
            yield return null;
        }

        // Stop Coroutines
        dialogDic[pendingDialogType][dialogIndex].who.charDialog.StopAllCoroutines();
        while (ice && odetta && dialogIndex < dialogDic[pendingDialogType].Count - 1)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][dialogIndex].what.Length);
            dialogDic[pendingDialogType][dialogIndex].who.charDialog.PopupDialog(dialogDic[pendingDialogType][dialogIndex].what, time);
            dialogIndex++;
            yield return new WaitForSeconds(time);
        }

        pendingDialogType = DialogType.introduce;

        if (ice && odetta)
        {
            StartCoroutine(Introduce());
        }
    }

    IEnumerator Introduce()
    {
        yield return new WaitForSeconds(2f);

        // Start to check box tutorial's flag
        StartCoroutine(TutorialBoxFlagChecking());

        for (int i = 0; i < dialogDic[pendingDialogType].Count; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }

        pendingDialogType = DialogType.tutorial_box;
        StartCoroutine(TutorialBox());
    }

    // Box tutorial flag checking
    IEnumerator TutorialBoxFlagChecking()
    {
        float height = ice.transform.position.y;
        while (!isJumped)
        {
            if (ice.transform.position.y > height)
            {
                isJumped = true;
            }
            yield return null;
        }
    }

    // Main box tutorial coroutine
    IEnumerator TutorialBox()
    {
        
        yield return new WaitForSeconds(1.5f);

        while (!isJumped)
        {
            yield return null;
        }

        for (int i=0; i<dialogDic[pendingDialogType].Count; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }

        StartCoroutine(PopUpHintText("Hold K to grab box when Odetta is near a box", 10f));

        pendingDialogType = DialogType.tutorial_entrance;
        StartCoroutine(TutorialEntrance());
    }

    // Main entrance tutorial
    IEnumerator TutorialEntrance()
    {
        GameObject entrance = GameObject.FindGameObjectWithTag("Entrance");
        SpriteRenderer renderer = entrance.GetComponent<SpriteRenderer>();
        while (!renderer.isVisible)
        {
            yield return null;
        }

        for (int i = 0; i < dialogDic[pendingDialogType].Count; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }
    }

    /////////////////////////////////////////////////////////////
    ////////////////////////// Tutorial 2 ///////////////////////
    /////////////////////////////////////////////////////////////

    // Weight tutorial secondary checking
    IEnumerator TutorialWeightPadPassDoorChecking(GameObject door)
    {
        while (!isPassTheDoor)
        {
            if (ice.transform.position.x > door.transform.position.x && odetta.transform.position.x > door.transform.position.x)
            {
                isPassTheDoor = true;
            }
            yield return null;
        }
    }

    // Weight tutorial checking
    IEnumerator TutorialWeightPadFlagChecing()
    {
        GameObject door = GameObject.Find("TriggerDoor");
        SpriteRenderer doorRenderer = door.GetComponent<SpriteRenderer>();
        Vector3 originPos = door.transform.position;
        Vector3 targetPos = door.transform.position + Vector3.up * 1.5f;

        StartCoroutine(TutorialWeightPadPassDoorChecking(door));

        while (!isDoorVisible)
        {
            if (doorRenderer.isVisible)
            {
                isDoorVisible = true;
            }
            yield return null;
        }

        while (!isDoorOpen)
        {
            if (door.transform.position.y >= targetPos.y)
            {
                isDoorOpen = true;
            }
            yield return null;
        }

        while (!isDoorClosed)
        {
            if (Mathf.Abs(door.transform.position.y - originPos.y) < 0.1f)
            {
                isDoorClosed = true;
            }
            yield return null;
        }
    }

    // Main weight pad tutorial tutorial
    IEnumerator TutorialWeightPad()
    {
        StartCoroutine(TutorialWeightPadFlagChecing());

        while (!isDoorVisible)
        {
            yield return null;
        }

        // When door is visible
        for (int i=0; i<5; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }

        while (!isDoorOpen)
        {
            yield return null;
        }

        // When door is open
        for (int i=5; i<7; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }

        
        while (!isDoorClosed && !isPassTheDoor)
        {
            yield return null;
        }

        if (isDoorClosed)
        {
            for (int i = 7; i < 13; i++)
            {
                float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
                dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
                yield return new WaitForSeconds(time);
            }
        }

        while (!isPassTheDoor)
        {
            yield return null;
        }

        for (int i = 13; i < 15; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }

    }

    ////////////////////////////////////////////////////////////
    ////////////////////// Tutorial 3 //////////////////////////
    ////////////////////////////////////////////////////////////

    IEnumerator TutorialCombatIceDie()
    {
        while (ice)
        {
            yield return null;
        }
        odetta.charDialog.StopAllCoroutines();
        for (int i = 27; i < 28; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator TutorialCombatOdettaDie()
    {
        while (odetta)
        {
            yield return null;
        }
        ice.charDialog.StopAllCoroutines();
        for (int i = 28; i < 29; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator TutorialCombatDie1(SpriteRenderer normal)
    {
        while (normal)
        {
            yield return null;
        }
        for (int i = 24; i < 25; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator TutorialCombatDie2(SpriteRenderer fire)
    {
        while (fire)
        {
            yield return null;
        }
        for (int i = 26; i < 27; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator TutorialCombatIceGetDamaged()
    {
        float iceHealth = ice.healthSlider.value;
        while (ice.healthSlider.value >= iceHealth)
        {
            yield return null;
        }

        for (int i = 25; i < 26; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator TutorialCombat ()
    {
        SpriteRenderer normal = GameObject.Find("NormalSpirit").GetComponent<SpriteRenderer>();
        SpriteRenderer fire = GameObject.Find("FireSpirit").GetComponent<SpriteRenderer>();

        while (!normal.isVisible)
        {
            yield return null;
        }

        StartCoroutine(PopUpHintText("Press J to let Odetta attack, and hold left mouse button to let Ice throw a ice", 10f));

        StartCoroutine(TutorialCombatIceGetDamaged());
        StartCoroutine(TutorialCombatDie1(normal));
        StartCoroutine(TutorialCombatIceDie());
        StartCoroutine(TutorialCombatOdettaDie());

        for (int i = 0; i < 8 && ice && odetta; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }

        while (!fire.isVisible)
        {
            yield return null;
        }

        StartCoroutine(TutorialCombatDie2(fire));

        for (int i=8; i< 14 && ice && odetta; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }

        GameObject trigger = GameObject.Find("TriggerBlock");
        while (ice && odetta && trigger.transform.position.x > ice.transform.position.x && trigger.transform.position.x > odetta.transform.position.x)
        {
            yield return null;
        }

        // no monster die
        if (normal && fire)
        {
            for (int i = 14; i < 16 && ice && odetta; i++)
            {
                float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
                dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
                yield return new WaitForSeconds(time);
            }
        }
        // one monster die
        else if (normal || fire)
        {
            for (int i = 16; i < 19 && ice && odetta; i++)
            {
                float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
                dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
                yield return new WaitForSeconds(time);
            }
        }
        // all die
        else
        {
            for (int i = 19; i < 24 && ice && odetta; i++)
            {
                float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
                dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
                yield return new WaitForSeconds(time);
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    //////////////////////// Conditional Chitchat ///////////////////////////////
    /////////////////////////////////////////////////////////////////////////////


    IEnumerator GetDamageChecking()
    {
        float iceHealth = ice.healthSlider.value;
        float odettaHealth = odetta.healthSlider.value;

        while (ice && odetta && ice.healthSlider.value >= iceHealth && odetta.healthSlider.value >= (odettaHealth - 2f))
        {
            iceHealth = ice.healthSlider.value;
            odettaHealth = odetta.healthSlider.value;
            yield return null;
        }

        if (ice && odetta)
        {
            int randomI = Random.Range(0, 5);
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[DialogType.chichat][randomI].what.Length);
            dialogDic[DialogType.chichat][randomI].who.charDialog.PopupDialog(dialogDic[DialogType.chichat][randomI].what, time);
            yield return new WaitForSeconds(time);

            StartCoroutine(GetDamageChecking());
        }
    }

    IEnumerator IceDieChecking()
    {
        while (ice)
        {
            yield return null;
        }

        if (odetta)
        {
            odetta.charDialog.StopAllCoroutines();
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[DialogType.chichat][5].what.Length);
            dialogDic[DialogType.chichat][5].who.charDialog.PopupDialog(dialogDic[DialogType.chichat][5].what, time);
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator OdettaDieChecking()
    {
        while (odetta)
        {
            yield return null;
        }

        if (ice)
        {
            ice.charDialog.StopAllCoroutines();
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[DialogType.chichat][6].what.Length);
            dialogDic[DialogType.chichat][6].who.charDialog.PopupDialog(dialogDic[DialogType.chichat][6].what, time);
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator OdettaHealthChecking()
    {
        while (odetta && odetta.healthSlider.value > 1/2f * odetta.healthSlider.maxValue)
        {
            yield return null;
        }

        for (int i = 7; i < 9; i++)
        {
            if (dialogDic[DialogType.chichat][i].who && odetta)
            {
                float time = Mathf.Max(1.5f, timePerChar * dialogDic[DialogType.chichat][i].what.Length);
                dialogDic[DialogType.chichat][i].who.charDialog.PopupDialog(dialogDic[DialogType.chichat][i].what, time);
                float interval = Random.Range(0f, 0.75f);
                yield return new WaitForSeconds(time);
            }
        }

        while (odetta && odetta.healthSlider.value > 1/3f * odetta.healthSlider.maxValue)
        {
            yield return null;
        }

        for (int i = 9; i < 11; i++)
        {
            if (dialogDic[DialogType.chichat][i].who && odetta)
            {
                float time = Mathf.Max(1.5f, timePerChar * dialogDic[DialogType.chichat][i].what.Length);
                dialogDic[DialogType.chichat][i].who.charDialog.PopupDialog(dialogDic[DialogType.chichat][i].what, time);
                float interval = Random.Range(0f, 0.75f);
                yield return new WaitForSeconds(time);
            }
        }

        while (odetta && odetta.healthSlider.value > 1 / 4f * odetta.healthSlider.maxValue)
        {
            yield return null;
        }

        for (int i = 11; i < 13; i++)
        {
            if (dialogDic[DialogType.chichat][i].who && odetta)
            {
                float time = Mathf.Max(1.5f, timePerChar * dialogDic[DialogType.chichat][i].what.Length);
                dialogDic[DialogType.chichat][i].who.charDialog.PopupDialog(dialogDic[DialogType.chichat][i].what, time);
                float interval = Random.Range(0f, 0.75f);
                yield return new WaitForSeconds(time);
            }
        }

        if (odetta)
        {
            StartCoroutine(OdettaHealthChecking());
        }
    }

    IEnumerator GetToEntranceChecking()
    {
        GameObject entrance = GameObject.Find("Entrance");
        while (ice && odetta && Vector3.Distance(ice.transform.position, entrance.transform.position) > 4f 
            && Vector3.Distance(odetta.transform.position, entrance.transform.position) > 4f)
        {
            yield return null;
        }

        for (int i = 13; i < 17 && ice && odetta; i++)
        {
            if (dialogDic[pendingDialogType][i].who)
            {
                float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
                dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
                float interval = Random.Range(0f, 0.75f);
                yield return new WaitForSeconds(interval);
            }
        }
    }

    // Main chitchat
    IEnumerator Chitchat()
    {
        yield return null;
    }

    ///////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////// Level 2 hint ///////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////

    IEnumerator Level2NotPassChecking()
    {
        GameObject door = GameObject.Find("TriggerDoor");
        float timestamp = Time.time;
        while (ice && odetta && Time.time - timestamp < 20f 
            && door.transform.position.x > ice.transform.position.x 
            && door.transform.position.x > odetta.transform.position.x)
        {
            yield return null;
        }

        if (Time.time - timestamp >= 20f)
        {
            for (int i = 4; i < 6 && ice && odetta; i++)
            {
                float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
                dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
                yield return new WaitForSeconds(time);
            }
        }
    }

    IEnumerator Level2Hint()
    {
        StartCoroutine(Level2NotPassChecking());

        yield return new WaitForSeconds(1f);

        for(int i=0; i<4 && ice && odetta; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }

        GameObject door = GameObject.Find("TriggerDoor");
        while (ice && odetta && door.transform.position.x > ice.transform.position.x && door.transform.position.x > odetta.transform.position.x)
        {
            yield return null;
        }

        for (int i = 6; i < 8 && ice && odetta; i++)
        {
            float time = Mathf.Max(1.5f, timePerChar * dialogDic[pendingDialogType][i].what.Length);
            dialogDic[pendingDialogType][i].who.charDialog.PopupDialog(dialogDic[pendingDialogType][i].what, time);
            yield return new WaitForSeconds(time);
        }
    }
}