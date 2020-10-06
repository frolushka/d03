using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Text playerHpLabel;
    [SerializeField] private Text playerEnergyLabel;

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
        GameManager.instance.SpawnTower(prefab, worldPosition);
    }
}
