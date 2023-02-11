namespace ServeRestTreinamentoAPI.StepsDefinitions
{
    public class Hooks
    {
        public void Initialize()
        {
            //acessando a API
            RestClient client = new RestClient("https://serverest.dev/");
        }

        [TestInitialize]
        public void getUp()
        {
            Initialize();
        }

        [TestCleanup]
        public void tearDown()
        {
            Console.WriteLine("Fim do teste!");
        }
    }
}
