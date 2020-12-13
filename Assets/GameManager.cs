using UnityEngine;
using Assets.GameStrategy;
using Assets.MenuScripts.Fight;
using Assets.MenuScripts.HeroOverviewPanel;

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
	public bool IsDialogOpened
    {
		get => isDialogOpened;
    }

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
		currentStrategy++;
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
		return currentStrategy == 1;
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

	public void ShowOverviewPanel()
    {
		overviewPanel.self.SetActive(true);
		sidePanelButtonHolder.SetActive(true);

	}

	public void SetCharacterInfo(HexUnit unit)
	{
		overviewPanel.SetCharacterInfo(unit);
	}

	public HexUnit GetActiveUnit()
    {
		return isCharacterMovementstrategyEnabled() ? ((CharactersActions)Strategy).currentUnit : null;
	}
}
