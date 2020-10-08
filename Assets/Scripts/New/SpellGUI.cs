using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class SpellDragFinishedEvent : UnityEvent<GameObject, Vector2> { }

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class SpellGUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private SpellDragFinishedEvent onSpellDragFinished;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float delay;
    
    private Vector2 defaultPosition;
    private float nextTime;
    private bool onDrag;
    
    private RectTransform rectTransform;
    private Image image;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    
    private void Start()
    {
        defaultPosition = rectTransform.position;
    }

    private void Update()
    {
        if (onDrag)
        {
            rectTransform.position = Input.mousePosition;
        }
        image.fillAmount = Mathf.Clamp01(1 - (nextTime - Time.time) / delay);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BeginDrag();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        FinishDrag(Input.mousePosition);
    }
    
    public void BeginDrag()
    {
        if (Time.time < nextTime || GUIManager.onDrag)
            return;
        GUIManager.onDrag = true;
        onDrag = true;
    }

    private void FinishDrag(Vector2 screenPosition)
    {
        if (Time.time < nextTime)
            return;
        GUIManager.onDrag = false;
        onDrag = false;
        
        rectTransform.position = defaultPosition;
        nextTime = Time.time + delay;
        onSpellDragFinished?.Invoke(explosionPrefab, screenPosition);
    }
}
