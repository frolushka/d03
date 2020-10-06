using UnityEngine;
using System.Collections;

public class ennemyScript : MonoBehaviour {

	[HideInInspector]public GameObject nextCheckpoint;
	[HideInInspector]public GameObject playerCore;
	public int waveNumber;
	public GameObject explosion;
	private Vector3 lastPos;
	public float speed;
	public bool isFlying = false;
	public int score;
	public int hp;
	public int value;									//Energie gagnee en tuant l'ennemi
	public int ennemyDamage;				//Dommages lorsque l'ennemi touche le core
	public float spawnRate;						//Vitesse de spawn en secondes
	public int waveLenghtModifier;		//Modificateur de taille de vague en %

	//Initialisation de la classe
	void Start() {
		lastPos = gameObject.transform.position;
		score = hp;
	}

	//Boucle d'update de la classe
	void Update() {
		lookForward();
		float step = speed * Time.deltaTime;
		transform.position = Vector2.MoveTowards (transform.position, nextCheckpoint.transform.position, step);
		if (hp <= 0) {
			GameManager.instance.PlayerEnergy += value;
			GameManager.instance.Score += score;
			destruction ();
		}
		else if (transform.position == playerCore.transform.position) {
			GameManager.instance.DamagePlayer(ennemyDamage);
			destruction();
		}
		else if (transform.position == nextCheckpoint.transform.position) {
			nextCheckpoint = nextCheckpoint.GetComponent<checkpoint>().nextCheckpoint;
		}
	}

	//Permet a l'ennemi de regarder dans la direction ou il se deplace
	void lookForward() {
		Vector3 moveDirection = gameObject.transform.position - lastPos; 
		if (moveDirection != Vector3.zero) {
			float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		lastPos = gameObject.transform.position;
	}

	//Fonction appellee a la destruction d'un ennemi
	void destruction(){
		Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
		Destroy(gameObject);
		checkLastEnemy();
	}

	//Fonction testant la destruction du dernier ennemi pour la victoire du niveau
	void checkLastEnemy() {
		if (GameManager.instance.lastWave == true) {
			GameObject[] spawners = GameObject.FindGameObjectsWithTag("spawner");
			foreach (GameObject spawner in spawners) {
				if (spawner.GetComponent<ennemySpawner>().isEmpty == false || spawner.transform.childCount > 1) {
					return ;
				}
			}
			GameManager.instance.Victory();
		}
	}
}
