using UnityEngine;
using UnityEngine.EventSystems;

//Code based on https://www.youtube.com/watch?v=h_BMXDWv10I&list=PLmc6GPFDyfw85CcfwbB7ptNVJn5BSBaXz&index=2
public class VRModuleInput : BaseInputModule
{
    [SerializeField] private Pointer pointer = null;

    public PointerEventData Data { get; private set; } = null;

    protected override void Start()
    {
        Data = new PointerEventData(eventSystem);
        Data.position = new Vector2(pointer.Camera.pixelWidth / 2, pointer.Camera.pixelHeight / 2);
    }

    public override void Process()
    {
        eventSystem.RaycastAll(Data, m_RaycastResultCache);
        Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);

        HandlePointerExitAndEnter(Data, Data.pointerCurrentRaycast.gameObject);

        ExecuteEvents.Execute(Data.pointerDrag, Data, ExecuteEvents.dragHandler);
    }

    public void Press()
    {
        Data.pointerPressRaycast = Data.pointerCurrentRaycast;

        Data.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(Data.pointerPressRaycast.gameObject);
        Data.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(Data.pointerPressRaycast.gameObject);

        ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerDownHandler);
        ExecuteEvents.Execute(Data.pointerDrag, Data, ExecuteEvents.beginDragHandler);

    }

    public void Release()
    {
        GameObject pointerRelease = ExecuteEvents.GetEventHandler<IPointerClickHandler>(Data.pointerCurrentRaycast.gameObject);

        if (Data.pointerPress == pointerRelease)
            ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerClickHandler);

        ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(Data.pointerDrag, Data, ExecuteEvents.endDragHandler);

        Data.pointerDrag = null;
        Data.pointerPress = null;

        Data.pointerCurrentRaycast.Clear();
    }
}


//public Camera cam;
//public SteamVR_Input_Sources targetSource;
//public SteamVR_Action_Boolean clickAction;

//private GameObject currentObject;
//private PointerEventData data;

//protected override void Awake()
//{
//    base.Awake();

//    data = new PointerEventData(eventSystem);
//}

//public override void Process()
//{
//    data.Reset();
//    data.position = new Vector2(cam.pixelWidth / 2, cam.pixelHeight / 2);

//    eventSystem.RaycastAll(data, m_RaycastResultCache);
//    data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
//    currentObject = data.pointerCurrentRaycast.gameObject;

//    m_RaycastResultCache.Clear();

//    HandlePointerExitAndEnter(data, currentObject);

//    if (clickAction.GetStateDown(targetSource))
//        ProcessPress(data);

//    if (clickAction.GetStateUp(targetSource))
//        ProcessRelease(data);
//}

//public PointerEventData GetData()
//{
//    return data;
//}

//private void ProcessPress(PointerEventData data)
//{
//    data.pointerPressRaycast = data.pointerCurrentRaycast;
//    GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(currentObject, data, ExecuteEvents.pointerDownHandler);

//    if (newPointerPress == null)
//        newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

//    data.pressPosition = data.position;
//    data.pointerPress = newPointerPress;
//    data.rawPointerPress = currentObject;
//}

//private void ProcessRelease(PointerEventData data)
//{
//    ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);
//    GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

//    if (data.pointerPress == pointerUpHandler)
//        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);

//    eventSystem.SetSelectedGameObject(null);
//    data.pressPosition = Vector2.zero;
//    data.pointerPress = null;
//    data.rawPointerPress = null;


//}