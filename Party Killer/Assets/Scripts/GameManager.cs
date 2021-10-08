using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class GameManager : MonoBehaviour
{

	public static GameManager Instance;

	public static int playerAScore = 0;

	public PlayerInputManager inputManager;

	public GameObject playerA;

	public GameObject playerPrefabA;

	public Transform spawnPointA;

	public GameObject players;
	public GameObject playerDeathPrefab;

	public bool waitingForPlayers = true;
	public Animator waitingForPlayersUI;
	public Animator fader;

	public bool gameIsEnded = false;

	bool policeWon = false;

	int currentLevel = 0;

	public int LOADLEVEL = -1;

	private void Awake()
	{
		Instance = this;
		StartCoroutine(LoadFirstLevel());
	}

	void OnPlayerJoined(PlayerInput input)
	{
		playerA = input.gameObject;
		playerA.transform.position = spawnPointA.position;
		inputManager.DisableJoining();
		waitingForPlayers = false;
		waitingForPlayersUI.SetTrigger("Stop");
		AudioManager.instance.Play("Ready");
		if (!inputManager.joiningEnabled)
			return;

		if (playerA == null)
		{
			playerA = input.gameObject;

			playerA.transform.position = spawnPointA.position;
		}
		
	}

	public void KillPlayer(GameObject p)
	{

		if (gameIsEnded)
			return;

		gameIsEnded = false;

		if (p == playerA)
		{
			policeWon = true;
		} else
		{
			policeWon = false;
			playerAScore++;
		}

		AudioManager.instance.Play("BigExplode");
		CameraShaker.Instance.ShakeOnce(6f, 6f, .1f, 1.5f);

		p.SetActive(false);
		//Instantiate(playerDeathPrefab, p.transform.position, Quaternion.identity);

		//StartCoroutine(LoadNextLevel());
	}

	IEnumerator LoadFirstLevel()
	{
		if (LOADLEVEL == -1)
		{
			currentLevel = GetRandomLevelIndex();
		} else
		{
			currentLevel = LOADLEVEL;
		}
		
		SceneManager.LoadScene(currentLevel, LoadSceneMode.Additive);

		yield return 0;

		spawnPointA = GameObject.FindGameObjectWithTag("SpawnPointA").transform;
	}

	IEnumerator LoadNextLevel()
	{
		yield return new WaitForSeconds(.75f);

		fader.SetTrigger("FadeOut");

		if (policeWon)
			AudioManager.instance.Play("PowerDown");

		yield return new WaitForSeconds(.75f);

		foreach (GameObject fx in GameObject.FindGameObjectsWithTag("FX"))
		{
			Destroy(fx);
		}
		foreach (GameObject fx in GameObject.FindGameObjectsWithTag("Bullet"))
		{
			Destroy(fx);
		}

		AsyncOperation unload = SceneManager.UnloadSceneAsync(currentLevel);

		while (!unload.isDone)
		{
			yield return 0;
		}

		if (LOADLEVEL == -1)
		{
			currentLevel = GetRandomLevelIndex();
		}
		else
		{
			currentLevel = LOADLEVEL;
		}

		SceneManager.LoadScene(currentLevel, LoadSceneMode.Additive);

		yield return 0;

		waitingForPlayers = true;

		playerA.GetComponent<Rigidbody>().velocity = Vector3.zero;

		spawnPointA = GameObject.FindGameObjectWithTag("SpawnPointA").transform;

		playerA.transform.position = spawnPointA.position;

		playerA.SetActive(true);

		playerA.GetComponent<PlayerWeapon>().Reset();

		fader.SetTrigger("FadeIn");

		yield return new WaitForSeconds(.2f);

		AudioManager.instance.Play("Ready");

		gameIsEnded = false;
		waitingForPlayers = false;
	}

	int GetRandomLevelIndex ()
	{
		return 1;
		int random = currentLevel;
		while (random == currentLevel)
		{
			random = Random.Range(1, SceneManager.sceneCountInBuildSettings);
		}
		return random;
	}

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.R))
		{
			//AsyncOperation unload = SceneManager.UnloadSceneAsync(currentLevel);
			//SceneManager.LoadScene(currentLevel, LoadSceneMode.Additive);
			playerA.GetComponent<Rigidbody>().velocity = Vector3.zero;
			playerA.GetComponent<Rigidbody>().velocity = Vector3.zero;

			spawnPointA = GameObject.FindGameObjectWithTag("SpawnPointA").transform;

			playerA.transform.position = spawnPointA.position;

			playerA.SetActive(true);

			playerA.GetComponent<PlayerWeapon>().Reset();

			fader.SetTrigger("FadeIn");
		}
		IEnumerator enumerator = spwanPlayers();
		StartCoroutine(enumerator);
	}

	IEnumerator spwanPlayers()
    {
		yield return new WaitForSeconds(1);
		players.SetActive(true);

	}
}
