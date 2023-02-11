namespace ServeRestTreinamentoAPI.StepsDefinitions
{
    public class Autenticacao
    {
        Usuarios usuarios = new Usuarios();

        private static RestClient? restClient;
        public Autenticacao() => restClient = new RestClient("https://serverest.dev/#/");
        public static RestResponse? response { get; set; }

        public string getToken()
        {
            JsonNode jsn = JsonNode.Parse(usuarios.realizarCadastroAdm())!;
            string email = (string)jsn!["email"]!;
            string senha = (string)jsn!["senha"]!;

            RestRequest request = new RestRequest("/login", Method.Post);
            request.AddJsonBody(new
            {
                email,
                password = senha
            });

            response = restClient!.Execute(request);
            JsonNode jsn1 = JsonNode.Parse(response.Content!)!;

            return (string)jsn1!["authorization"]!;

        }

        public void autenticar(RestClient client)
        {
            client.AddDefaultHeader("Authorization", getToken());
        }
    }
}
