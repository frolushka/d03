using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class TowerDragFinishedEvent : UnityEvent<GameObject, Vector2> { }

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class TowerGUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TowerDragFinishedEvent onTowerDragFinished;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Color enabledColor = Color.white;
    [SerializeField] private Color disabledColor = Color.red;

    [Header("UI Setup")]
    [SerializeField] private Text waitLabel;
    [SerializeField] private Text powerLabel;
    [SerializeField] private Text rangeLabel;
    [SerializeField] private Text energyLabel;
    [Space]
    [SerializeField] private Sprite flySprite;
    [SerializeField] private Sprite notFlySprite;
    [SerializeField] private Image flyImage;

    private bool buyAvailable;
    private bool onDrag;
    private Vector2 defaultPosition;
    private TowerScript tower;

    private RectTransform rectTransform;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        
        tower = towerPrefab.GetComponent<TowerScript>();
        waitLabel.text = tower.fireRate.ToString();
        powerLabel.text = tower.damage.ToString();
        rangeLabel.text = tower.range.ToString();
        energyLabel.text = tower.energy.ToString();
        flyImage.sprite = tower.CanKillFlyingEnemies 
            ? flySprite 
            : notFlySprite;
    }

    private void OnEnable()
    {
        GameManager.instance.onPlayerEnergyChange += ChangeAvailability;
    }
    
    private void OnDisable()
    {
        GameManager.instance.onPlayerEnergyChange -= ChangeAvailability;
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
    }

    public void BeginDrag()
    {
        if (!buyAvailable || GUIManager.onDrag)
            return;
        GUIManager.onDrag = true;
        onDrag = true;
    }

    private void FinishDrag(Vector2 screenPosition)
    {
        if (!buyAvailable)
            return;
        GUIManager.onDrag = false;
        onDrag = false;
        rectTransform.position = defaultPosition;
        onTowerDragFinished?.Invoke(towerPrefab, screenPosition);
    }

    private void ChangeAvailability(int energy)
    {
        if (energy >= tower.energy)
        {
            image.color = enabledColor;
            buyAvailable = true;
        }
        else
        {
            image.color = disabledColor;
            buyAvailable = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData) => BeginDrag();
    public void OnPointerUp(PointerEventData eventData) => FinishDrag(eventData.position);
}
