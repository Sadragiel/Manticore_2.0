using UnityEngine;
using Assets.GameStrategy;

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

	public GameStrategy[] strategies;
	int currentStrategy;

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
		return currentStrategy == 0;
	}

	public void EndTurn()
    {
		if(isCharacterMovementstrategyEnabled()) 
		{
			((CharactersActions)Strategy).NextTurn();
		}
    }

	public void OpenArtifactManagementDialog()
	{
		if (isCharacterMovementstrategyEnabled())
		{
			((CharactersActions)Strategy).OpenArtifactManagementDialog(null);
		}
	}

	public void CloseArtifactManagementDialog()
	{
		if (isCharacterMovementstrategyEnabled())
		{
			((CharactersActions)Strategy).CloseArtifactManagementDialog();
		}
	}
}
