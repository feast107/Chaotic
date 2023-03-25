using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;

public class SkillControl : MonoBehaviour
{
    // Start is called before the first frame update
    public float ColdDown = 10;//seconds
    public float Lasting = 5;

    private bool ready = true;
    private bool isLasting = false;

    private DateTime triggerTime;
    private TimeSpan lastTime;
    private TimeSpan coldTime;
    public delegate void OnReadyHandler();
    public event OnReadyHandler OnReadyEvent;

    public delegate void OnLastingOverHandler();
    public event OnLastingOverHandler OnLastingOverEvent;

    public Transform ColdText;
    public Transform ColdMask;

    private TextMeshProUGUI ColdTextMesh { get; set; }
    private SpriteRenderer ColdMaskRenderer { get; set; }
    public int ColdLayer = 99;
    public int ReadyLayer = -99;

    private void Start()
    {
        this.Claim();
        coldTime = TimeSpan.FromSeconds(ColdDown);
        lastTime = TimeSpan.FromSeconds(Lasting);
        ColdTextMesh = ColdText.GetComponent<TextMeshProUGUI>();
        ColdMaskRenderer = ColdMask.GetComponent<SpriteRenderer>();
        ColdTextMesh.text = string.Empty;
        ColdMaskRenderer.sortingOrder = ReadyLayer;
    }

#nullable enable
    public bool Release(OnLastingOverHandler? callback = null, float? lasting = 5f)
    {
        if (!ready) return false;
        ready = false;
        triggerTime = DateTime.Now;
        isLasting = true;
        lastTime = TimeSpan.FromSeconds(lasting ?? Lasting);
        if (callback != null)
        {
            void Del()
            {
                OnLastingOverEvent -= Del;
                callback!.Invoke();
            }
            OnLastingOverEvent += Del;
        }
        Debug.Log("ÊÍ·Å¼¼ÄÜ");
        ColdMaskRenderer.sortingOrder = ColdLayer;
        return true;
    }

    private void ProcessColdDown(TimeSpan span)
    {
        if (span >= coldTime)
        {
            ready = true;
            ColdMaskRenderer.sortingOrder = ReadyLayer;
            OnReadyEvent?.Invoke();
            ColdTextMesh.text = string.Empty;
        }
        else
        {
            ColdTextMesh.text = ((int)(coldTime - span).TotalSeconds + 1).ToString();
        }
    }

    private void ProcessLasting(TimeSpan span)
    {
        if (span < lastTime) return;
        isLasting = false;
        OnLastingOverEvent?.Invoke();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (ready) return;
        var span = DateTime.Now - triggerTime;
        ProcessColdDown(span);
        if (isLasting)
        {
            ProcessLasting(span);
        }
    }
}
