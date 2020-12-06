namespace Assets.GameStrategy
{

    /*
     * Class should create all needed units on Start event: 
     *      - Heroes in the Castle
     *      - Manticore in her Cave
     *      - Monsters on their road cases
     *     
     * On Update event it should run the turn method of the characters one by one.
     *      - 
     */
    class CharactersActions : GameStrategy
    {
        public override void Update()
        {
            //TODO
        }

        public override void Start()
        {
            base.Start();

        }

        //HexUnit CreateUnit(HexCell cell)
        //{
        //    HexUnit unit = Instantiate(DataController.Instance.unitPrefab);
        //    unit.transform.SetParent(hexGrid.transform, false);
        //    unit.Location = cell;
        //    return unit;
        //}
    }
}
