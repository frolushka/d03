using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static bool onDrag = false;
    
    [SerializeField] private Camera camera;
    [SerializeField] private Text playerHpLabel;
    [SerializeField] private Text playerEnergyLabel;
    [SerializeField] private GameObject towerRadialMenu;

    private RectTransform _towerRadialMenuRectTransform;
    private TowerRadialMenuGUI _towerRadialMenuGUI;
    
    private void Awake()
    {
        _towerRadialMenuRectTransform = towerRadialMenu.GetComponent<RectTransform>();
        _towerRadialMenuGUI = towerRadialMenu.GetComponent<TowerRadialMenuGUI>();
    }

    private void OnEnable()
    {
        GameManager.instance.onPlayerHpChange += UpdatePlayerHpLabel;
        GameManager.instance.onPlayerEnergyChange += UpdatePlayerEnergyLabel;
    }
    
    private void OnDisable()
    {
        GameManager.instance.onPlayerHpChange -= UpdatePlayerHpLabel;
        GameManager.instance.onPlayerEnergyChange -= UpdatePlayerEnergyLabel;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            var hit = Physics2D.OverlapPoint(camera.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("Tower"));
            if (hit && hit.transform.parent && hit.transform.parent.TryGetComponent<TowerScript>(out var towerScript))
            {
                _towerRadialMenuRectTransform.position = camera.WorldToScreenPoint(towerScript.transform.position);
                towerRadialMenu.SetActive(true);
                
                _towerRadialMenuGUI.SetTower(towerScript);
            }
        }
    }

    private void UpdatePlayerHpLabel(int value)
    {
        playerHpLabel.text = value.ToString();
    }

    private void UpdatePlayerEnergyLabel(int value)
    {
        playerEnergyLabel.text = value.ToString();
    }

    public void SpawnTower(GameObject prefab, Vector2 screenPosition)
    {
        var worldPosition = camera.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;
        GameManager.instance.SpawnTower(prefab, worldPosition);
    }

    public void Spawn(GameObject prefab, Vector2 screenPosition)
    {
        var worldPosition = camera.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;
        Instantiate(prefab, worldPosition, Quaternion.identity);
    }
}
