using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class StatusDetail : MonoBehaviour
{
    // Start is called before the first frame update
    public string Hp;
    public string Mp;
    public string Charge;

    private ValueController hpControl;
    private ValueController mpControl;
    private ValueController chargeControl;


    public delegate void DiedEventHandler();
    public event DiedEventHandler OnDiedEvent;

    public Transform BoostSkill;
    private SkillControl Skill;
    void Start()
    {
        this.Claim();

        hpControl = this.GetComponents<ValueController>().ToList().Find(x => x.Name == Hp);
        mpControl = this.GetComponents<ValueController>().ToList().Find(x => x.Name == Mp);
        chargeControl = this.GetComponents<ValueController>().ToList().Find(x => x.Name == Charge);

        hpControl.OnEmptyEvent += () => { OnDiedEvent?.Invoke(); };
        OnDiedEvent += () => { Debug.Log("Died"); };
        Skill = BoostSkill.GetComponent<SkillControl>();
    }
    

    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonUp((int)MouseButton.Right))
        {
            Skill.Release(() => { Debug.Log("³ÖÐø½áÊø"); }, 4);
        }

        if (Input.GetMouseButtonUp((int)MouseButton.Left))
        {
            if (!set)
            {
                hpControl.Cast(75f);
                set = true;
            }
            else
            {
                hpControl.Recover(75f);
                set = false;
            }
        }
    }

    bool set =false;
}
