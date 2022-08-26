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
    private Transform m_LastInstantiatedCollider;

    private void Start()
    {
        m_DrawnObject = new GameObject();
    }

    private void FixedUpdate()
    {
        if (!m_StartDrawing) return;
        var distance = m_MousePosition - Input.mousePosition;
        var distanceSquareMagnitude = distance.sqrMagnitude;
        if (!(distanceSquareMagnitude > 2000)) return;

        //Draw Shape
        m_LineRenderer.SetPosition(m_CurrentIndex, mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10)));
        
        if (m_LastInstantiatedCollider != null)
        {
            var currentPosition = m_LineRenderer.GetPosition(m_CurrentIndex);
            m_LastInstantiatedCollider.LookAt(currentPosition);
            m_LastInstantiatedCollider.gameObject.SetActive(true);

            if (m_LastInstantiatedCollider.rotation.y == 0)
            {
                var eulerAngles = m_LastInstantiatedCollider.eulerAngles;
                eulerAngles = new Vector3(eulerAngles.x, 90, eulerAngles.z);
                m_LastInstantiatedCollider.eulerAngles = eulerAngles;
            }
            
            var localScale = m_LastInstantiatedCollider.localScale;
            localScale = new Vector3(localScale.x, Vector3.Distance(m_LastInstantiatedCollider.position, currentPosition) * 0.1f, localScale.z);
            m_LastInstantiatedCollider.localScale = localScale;
        }

        m_LastInstantiatedCollider = Instantiate(cubePrefab, m_LineRenderer.GetPosition(m_CurrentIndex), Quaternion.identity, m_DrawnObject.transform);

        m_MousePosition = Input.mousePosition;
        m_CurrentIndex++;
        m_LineRenderer.positionCount = m_CurrentIndex + 1;
        //position for line renderer
        m_LineRenderer.SetPosition(m_CurrentIndex, mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10)));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_StartDrawing = true;
        m_MousePosition = Input.mousePosition;
        
        //Setup LineRenderer
        m_LineRenderer = m_DrawnObject.AddComponent<LineRenderer>();
        m_LineRenderer.startWidth = 0.2f;
        m_LineRenderer.material = lineMaterial;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_StartDrawing = false;
        m_DrawnObject.AddComponent<Rigidbody>();
        m_LineRenderer.useWorldSpace = false;
        if(m_LastInstantiatedCollider != null)
            Destroy(m_LastInstantiatedCollider.gameObject);
        Start();
        m_CurrentIndex = 0;
    }
}
