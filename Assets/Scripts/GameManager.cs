using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	[SerializeField] private UnityEvent onVictory;
	[SerializeField] private UnityEvent onGameOver;

	
	//Vous pouvez directement changer ces valeurs de base dans l'inspecteur si vous voulez personnaliser votre jeu
	public int playerMaxHp = 20;
	public int playerStartEnergy = 300;

	public int delayBetweenWaves = 10;					//Temps entre les vagues
	public int nextWaveEnnemyHpUp = 20; 				//Augmentation de la vie des bots a chaque vague (en %)
	public int nextWaveEnnemyValueUp = 30; 		//Augmentation de l'energie donnee par les bots a chaque vague (en %)
	public int averageWavesLenght = 15;					//Taille moyenne d'une vague d'ennemis
	public int totalWavesNumber = 20;						// Nombre des vagues au total
	[HideInInspector]public bool lastWave = false;
	[HideInInspector]public int currentWave = 1;
	private float tmpTimeScale = 1;

	public static GameManager instance;
	
	[SerializeField] private int playerHp = 20;
	public int PlayerHp
	{
		get => playerHp;
		set
		{
			playerHp = value;
			onPlayerHpChange?.Invoke(playerHp);
		}
	}
	public event System.Action<int> onPlayerHpChange;
	
	[SerializeField] private int score = 0;
	public int Score
	{
		get => score;
		set
		{
			score = value;
			onScoreChange?.Invoke(score);
		}
	}
	public event System.Action<int> onScoreChange;

	[SerializeField] private int playerEnergy = 300;
	public int PlayerEnergy
	{
		get => playerEnergy;
		set
		{
			playerEnergy = value;
			onPlayerEnergyChange?.Invoke(playerEnergy);
		}
	}
	public event System.Action<int> onPlayerEnergyChange;
	
	private void Awake () 
	{
		if (!instance)
			instance = this;
	}

	private void Start() 
	{
		Pause(true);
		PlayerHp = playerMaxHp;
		PlayerEnergy = playerStartEnergy;
	}

	private void OnDestroy()
	{
		Time.timeScale = 1;
	}

	//Pour mettre le jeu en pause
	public void Pause(bool paused) {
		if (paused == true) {
			tmpTimeScale = Time.timeScale;
			Time.timeScale = 0;
		}
		else
			Time.timeScale = tmpTimeScale;
	}

	//Pour changer la vitesse de base du jeu
	public void ChangeSpeed(float speed) {
		Time.timeScale = speed;
	}

	//Le joueur perd de la vie
	public void DamagePlayer(int damage) {
		PlayerHp -= damage;
		if (PlayerHp <= 0)
			GameOver();
		else
			Debug.Log ("Il reste au joueur " + playerHp + "hp");
	}

	public void Victory()
	{
		Debug.Log ("Victoire !");
		onVictory?.Invoke();
	}

	public void GameOver() 
	{
		Time.timeScale = 0;
		Debug.Log ("Game Over");
		onGameOver?.Invoke();
	}

	public void SpawnTower(GameObject prefab, Vector2 worldPosition)
	{
		var hit = Physics2D.OverlapPoint(worldPosition, LayerMask.GetMask("Default"));
		if (!hit)
			return;
		var hitPosition = hit.transform.position;
		Destroy(hit.gameObject);
		var go = Instantiate(prefab, hitPosition, Quaternion.identity);
		var tower = go.GetComponent<TowerScript>();
		PlayerEnergy -= tower.energy;
	}
}
