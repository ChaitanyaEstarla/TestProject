using UnityEngine;
using UnityEngine.EventSystems;

public class CreateShape : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Material lineMaterial;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform cubePrefab;
    
    private GameObject m_DrawnObject;
    private bool m_StartDrawing = false;
    private Vector3 m_MousePosition;
    private LineRenderer m_LineRenderer;
    private int m_CurrentIndex = 0;

    private void Start()
    {
        m_DrawnObject = new GameObject();
    }

    private void Update()
    {
        if (!m_StartDrawing) return;
        var distance = m_MousePosition - Input.mousePosition;
        var distanceSquareMagnitude = distance.sqrMagnitude;
        if (!(distanceSquareMagnitude > 500f)) return;

        //Draw Shape
        m_LineRenderer.SetPosition(m_CurrentIndex, mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10)));
        m_MousePosition = Input.mousePosition;
        m_CurrentIndex++;
        m_LineRenderer.positionCount = m_CurrentIndex + 1;
        m_LineRenderer.SetPosition(m_CurrentIndex, mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10)));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_StartDrawing = true;
        m_MousePosition = Input.mousePosition;
        
        m_LineRenderer = m_DrawnObject.AddComponent<LineRenderer>();
        m_LineRenderer.startWidth = 0.2f;
        m_LineRenderer.material = lineMaterial;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_StartDrawing = false;
        m_DrawnObject.AddComponent<Rigidbody>();
        m_LineRenderer.useWorldSpace = false;
        Start();
        m_CurrentIndex = 0;
    }
}
