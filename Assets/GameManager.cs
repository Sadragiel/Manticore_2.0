using UnityEngine;
using Assets.GameStrategy;
using Assets.MenuScripts.Fight;
using Assets.MenuScripts.HeroOverviewPanel;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	private static GameManager _instance;
	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<GameManager>();
			return _instance;
		}
	}

	public HexGrid hexGrid;
	public FightDialog fightDialog;
	public HeroOverviewPanel overviewPanel;
	public GameObject sidePanelButtonHolder;

	public GameStrategy[] strategies;
	int currentStrategy;

	bool isDialogOpened;
	public bool isManticoreDead;
	public bool IsDialogOpened
    {
		get => isDialogOpened;
    }

	public EndGameState endGameState;

	private GameStrategy Strategy
    {
		get
        {
			return strategies?[currentStrategy];
        }
    }

	void Update()
	{
		Strategy?.Update();
	}

	void Start()
    {
		hexGrid.GenerateMap();
		InitStrategies();
	}

	void Awake()
    {
		
	}

	public void RunNextStrategy()
	{
		currentStrategy = (currentStrategy + 1) % strategies.Length;
		Strategy.Start();
	}

	void InitStrategies()
	{
		strategies = DataController.Instance.GetStrategies();
		currentStrategy = -1;
		RunNextStrategy();
	}

	bool isCharacterMovementstrategyEnabled()
    {
		// TODO: find a way to avoid hardcodding
		return currentStrategy == 2;
	}

	public void EndTurn()
    {
		if(isCharacterMovementstrategyEnabled()) 
		{
			((CharactersActions)Strategy).NextTurn();
		}
    }

	public void HandleDeath(HexUnit deadUnit)
    {
		if(isCharacterMovementstrategyEnabled()) 
		{
			((CharactersActions)Strategy).HandleDeath(deadUnit);
		}
    }

	public void OpenArtifactManagementDialog()
	{
		if (isCharacterMovementstrategyEnabled())
		{
			isDialogOpened = true;
			((CharactersActions)Strategy).OpenArtifactManagementDialog(null);
		}
	}

	public void OpenArtifactManagementDialog(HexUnit unit)
	{
		if (isCharacterMovementstrategyEnabled())
		{
			isDialogOpened = true;
			((CharactersActions)Strategy).OpenArtifactManagementDialog(unit.bag);
		}
	}

	public void CloseArtifactManagementDialog()
	{
		if (isCharacterMovementstrategyEnabled())
		{
			isDialogOpened = false;
			((CharactersActions)Strategy).CloseArtifactManagementDialog();
		}
	}

	public void InitFight(HexUnit attacker, HexUnit defender, bool randomWeapon)
    {
		isDialogOpened = true;
		fightDialog.OpenDialog(attacker, defender, randomWeapon);
	}

	public void SetDialogState(bool isDialogOpened)
    {
		this.isDialogOpened = isDialogOpened;
    }

	public void SetStateOverviewPanel(bool state)
    {
		overviewPanel.self.SetActive(state);
		sidePanelButtonHolder.SetActive(state);
	}

	public void SetCharacterInfo(HexUnit unit)
	{
		overviewPanel.SetCharacterInfo(unit);
	}

	public HexUnit GetActiveUnit()
    {
		return isCharacterMovementstrategyEnabled() ? ((CharactersActions)Strategy).currentUnit : null;
	}

	public void AttackCastle()
    {
		if (isCharacterMovementstrategyEnabled())
		{
			((CharactersActions)Strategy).AttackCastle();
		}
	}

	void EndTheGame()
    {
		SetStateOverviewPanel(false);
		RunNextStrategy();
	}

	public void Defeat(bool manticoreReachedCastle)
    {
		endGameState = manticoreReachedCastle ? EndGameState.Lose : EndGameState.GreatLose;
		EndTheGame();
	}

	public void Win()
	{
		if (!isCharacterMovementstrategyEnabled())
			return;
		List<HexUnit> units = ((CharactersActions)Strategy).GetUnits();
		endGameState = units.FindAll(unit => unit.isEnemy).Count == 1 ? EndGameState.GreatVictory : EndGameState.Victory;
		EndTheGame();
	}

}
