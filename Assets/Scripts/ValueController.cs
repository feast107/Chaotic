using System;
using UnityEngine;

[Serializable]
public class ValueController : MonoBehaviour
{
    public string Name;

    public Transform Value;
    public Line Direction = Line.Horizontal;
    public bool CanRunOut = true;

    public float Maximum;
    public float Minimum;

    private float current;
    private float future;
    private float last;

    public int Delay = 500;
    private TimeSpan delaySpan;
    private DateTime lastChange;

    public delegate void OnEmptyHandler();

    public delegate void OnFullHandler();
    /// <summary>
    /// 耗尽的事件
    /// </summary>
    public event OnEmptyHandler OnEmptyEvent;
    /// <summary>
    /// 充满的事件
    /// </summary>
    public event OnFullHandler OnFullEvent;

    private bool completed = false;

    private void ChangeValue(Change change, float amount)
    {
        var next = future + change.ToFeature() * amount;
        if (next <= Minimum)
        {
            next = Minimum;
            if (future > Minimum)
            {
                OnEmptyEvent?.Invoke();
            }
        }

        if (next >= Maximum)
        {
            next = Maximum;
            if (future < Maximum)
            {
                OnFullEvent?.Invoke();
            }
        }
        lastChange = DateTime.Now;
        future = next;
        last = current;
        completed = false;
    }

    public bool Cast(float amount)
    {
        if ((CanRunOut && future <= Minimum) || (!CanRunOut && future - amount < Minimum)) return false;
        ChangeValue(Change.Decrement, amount);
        return true;
    }

    public void Recover(float amount)
    {
        ChangeValue(Change.Increment, amount);
    }

    // Start is called before the first frame update
    private void Start()
    {
        last = future = current = Maximum;
        delaySpan = Delay <= 0 ? TimeSpan.Zero : TimeSpan.FromMilliseconds(Delay);
    }

    private void SetAnimation(float value)
    {
        var percent = (value - Minimum) / (Maximum - Minimum);
        switch (Direction)
        {
            case Line.Horizontal:
                Value.localScale = new(percent, 1);
                return;
            case Line.Vertical:
                Value.localScale = new(1, percent);
                return;
        }
    }

    private void FixedUpdate()
    {
        if (completed) return;
        if (delaySpan != TimeSpan.Zero)
        {
            var during = DateTime.Now - lastChange;
            if (during <= delaySpan)
            {
                var de = future - last;
                current = last + (float)(de * (during / delaySpan));
                SetAnimation(current);
                return;
            }
        }
        last = current = future;
        completed = true;
        SetAnimation(current);
    }
    // Update is called once per frame
    private void Update()
    {
       
    }
}
