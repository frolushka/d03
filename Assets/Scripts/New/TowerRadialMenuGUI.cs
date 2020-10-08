using UnityEngine;
using UnityEngine.UI;

public class TowerRadialMenuGUI : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Text upgradeLabel;
    [SerializeField] private Button sellButton;
    [SerializeField] private Text sellLabel;
    [SerializeField] private GameObject defaultTile;

    private TowerScript _currentTower;

    private void OnEnable()
    {
        GameManager.instance.onPlayerEnergyChange += UpdateInfoAboutCurrentTower;
    }

    private void OnDisable()
    {
        GameManager.instance.onPlayerEnergyChange -= UpdateInfoAboutCurrentTower;
    }

    public void SetTower(TowerScript towerScript)
    {
        _currentTower = towerScript;
        UpdateInfoAboutCurrentTower(GameManager.instance.PlayerEnergy);
    }

    public void UpdateInfoAboutCurrentTower(int currentEnergy)
    {
        Setup(_currentTower.upgrade, currentEnergy, upgradeLabel, upgradeButton, true);
        Setup(_currentTower.gameObject, currentEnergy, sellLabel, sellButton, false);
    }

    public void Upgrade()
    {
        GameManager.instance.PlayerEnergy -= _currentTower.upgrade.GetComponent<TowerScript>().energy;
        var tower = Instantiate(_currentTower.upgrade, _currentTower.transform.position, Quaternion.identity);
        Destroy(_currentTower.gameObject);
        SetTower(tower.GetComponent<TowerScript>());
    }

    public void Sell()
    {
        GameManager.instance.PlayerEnergy += _currentTower.energy / 2;
        if (_currentTower.downgrade)
        {
            var tower = Instantiate(_currentTower.downgrade, _currentTower.transform.position, Quaternion.identity);
            Destroy(_currentTower.gameObject);
            SetTower(tower.GetComponent<TowerScript>());
        }
        else
        {
            Instantiate(defaultTile, _currentTower.transform.position, Quaternion.identity);
            Destroy(_currentTower.gameObject);
            gameObject.SetActive(false);
        }
    }

    private void Setup(GameObject target, int currentEnergy, Text label, Button button, bool buy)
    {
        if (target)
        {
            var towerScript = target.GetComponent<TowerScript>();
            var energy = towerScript.energy;
            if (!buy)
                energy /= 2;
            label.text = energy.ToString();
            if (buy)
            {
                var isEnoughEnergy = currentEnergy >= energy;
                label.color = isEnoughEnergy
                    ? Color.white
                    : Color.red;
                button.interactable = isEnoughEnergy;
            }
            
            button.gameObject.SetActive(true);
        }
        else
        {
            button.gameObject.SetActive(false);
        }
    }
}
