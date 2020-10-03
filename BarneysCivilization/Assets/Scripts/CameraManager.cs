using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed;

    private Transform CamTransform;
    public float DragSpeed;
    private void Start()
    {
        CamTransform = Camera.main.transform;

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);

    }
    public void OnDragDelegate(PointerEventData data)
    {
        CamTransform.position += new Vector3(-data.delta.y, 0, data.delta.x) * DragSpeed;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position+=direction.normalized * MoveSpeed * Time.deltaTime;
    }

}
