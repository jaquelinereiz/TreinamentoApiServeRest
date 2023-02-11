using System.ComponentModel.DataAnnotations;

namespace ServeRestTreinamentoAPI.StepsDefinitions
{
    public class Usuarios : Hooks
    {
        private static RestClient? restClient;
        public Usuarios() => restClient = new RestClient("https://serverest.dev/#/");

        public static RestResponse? response { get; set; }
        public static string? idUser { get; set; }

        #region Dados
        //Dados para ServRestAPI
        public static string nome1 = "Lord";
        public static string? email1;
        public static string password1 = "teste123";
        public static string admnistrador1 = "false";

        public static void aleatorizarEmail()
        {
            Random random = new Random();
            string rdn = random.Next(1, 100).ToString();
            email1 = "Lord" + rdn + "@qa.com";
        }
        

        #endregion Dados

        #region Cadastrar

        public void realizarCadastroComSucesso()
        {
            Random random = new Random();
            string rdn = random.Next(1, 100).ToString();
            email1 = "Lord" + rdn + "@qa.com";

            //acessando o EndPoint
            RestRequest request = new RestRequest("/usuarios", Method.Post);
            //passando parametros json
            request.AddJsonBody(new
            {
                nome = nome1,
                email = email1,
                password = password1,
                administrador = admnistrador1
            });

            response = restClient!.Execute(request);
            var code = (int)response.StatusCode;

            //Converte a 'String' do response para que o C# consiga identificar como JSON
            JsonNode JsonNode = JsonNode.Parse(response.Content!)!;
            //Salva o ID na variavel idUser
            idUser = (string)JsonNode!["_id"]!;

            Console.WriteLine("id: " + idUser);
            Console.WriteLine("Code: " + code + " Response " + response.Content);

            Assert.AreEqual(201, code);
            Assert.IsTrue(response.Content!.Contains("Cadastro realizado com sucesso"), "Verificado que contém mensagem de sucesso");
        }

        public void realizarCadastroComDadosExistente()
        {
            //pré-condição para realizar o teste: ter um usuário cadastrado
            realizarCadastroComSucesso();

            RestRequest request = new RestRequest("/usuarios", Method.Post);
            request.AddJsonBody(new
            {
                nome = nome1,
                email = email1,
                password = password1,
                administrador = admnistrador1
            }
            );

            response = restClient!.Execute(request);
            var code = (int)response.StatusCode;

            Console.WriteLine("id: " + idUser);
            Console.WriteLine("Code: " + code + " Response " + response.Content);

            Assert.AreEqual(400, code);
            Assert.IsTrue(response.Content!.Contains("Este email já está sendo usado"), "Verifica se contém mensagem: 'Este email já está sendo usado' ");
        }

        #region cadastroADM
        public string realizarCadastroAdm()
        {
            Random random = new Random();
            string rdn = random.Next(1, 100).ToString();

            string email = "lady" + rdn + "@qa.com";
            string idS, retorno;

            RestRequest request = new RestRequest("/usuarios", Method.Post);
            request.AddJsonBody(new
            {
                nome = "Lady",
                email,
                password = "adm123",
                administrador = "true"
            });

            response = restClient!.Execute(request);
            JsonNode JsonNode = JsonNode.Parse(response.Content!)!;
            idS = (string)JsonNode!["_id"]!;

            retorno = "{\r\n  \"email\": \"" + email + "\"," +
                "\r\n  \"senha\": \"teste\"," +
                "\r\n  \"idUsuario\": \"" + idS + "\"" + "" +
                "\n}";

            return retorno;
        }
        #endregion cadastroADM

        #endregion Cadastrar

        #region Consultar
        public void realizarConsultaListarUsuariosCadastrados()
        {
            //pré-condição para realizar o teste: ter um usuário cadastrado
            realizarCadastroComSucesso();

            RestRequest request = new RestRequest("/usuarios", Method.Get);
            request.AddParameter("_id", idUser!);
            request.AddParameter("nome", nome1);
            request.AddParameter("email", email1!);
            request.AddParameter("password", password1);

            response = restClient!.Execute(request);
            var code = (int)response.StatusCode;

            Console.WriteLine("id: " + idUser);
            Console.WriteLine("Code: " + code + " Response " + response.Content);

            Assert.AreEqual(200, code);
            Assert.IsTrue(response.Content!.Contains(idUser!), "Verifica se o contém o Id no response");
            Assert.IsTrue(response.Content!.Contains(email1!), "Verifica se o contém o Email no response");
        }

        public void realizarConsultaDeUsuarioPorId()
        {
            string id = idUser!.ToString();

            RestRequest request = new RestRequest("/usuarios", Method.Get);
            response = restClient!.Execute(request);
            var code = (int)response.StatusCode;

            Console.WriteLine("id: " + idUser);
            Console.WriteLine("Code: " + code + " Response " + response.Content);

            Assert.AreEqual(200, code);
            Assert.IsTrue(response.Content!.Contains(id), "Verifica se contém o Id no response");
        }

        public void realizarConsultaDeUsuarioComIdInexistente()
        {
            string id = "LeIt3DeS0j4";

            RestRequest request = new RestRequest("/usuarios/" + id, Method.Get);

            response = restClient!.Execute(request);
            var code = (int)response.StatusCode;

            Console.WriteLine("id: " + idUser);
            Console.WriteLine("Code: " + code + " Response " + response.Content);

            Assert.AreEqual(400, code);
            Assert.IsTrue(response.Content!.Contains("Usuário não encontrado"), "Verifica mensagem de usuário não encontrado");
        }
        #endregion Consultar

        #region Excluir
        public void excluirUsuarioPorId()
        {
            string id = idUser!.ToString();

            RestRequest request = new RestRequest("/usuarios/" + id, Method.Delete);

            response = restClient!.Execute(request);
            var code = (int)response.StatusCode;

            Console.WriteLine("id: " + id);
            Console.WriteLine("Code: " + code + " Response " + response.Content);

            Assert.AreEqual(200, code);
            Assert.IsTrue(response.Content!.Contains("Registro excluído com sucesso"));
        }

        public void excluirUsuarioComIdInexistente()
        {
            string id = "LeIt3DeS0j4";
            RestRequest request = new RestRequest("/usuarios/" + id, Method.Delete);

            response = restClient!.Execute(request);
            var code = (int)response.StatusCode;

            Console.WriteLine("id: " + id);
            Console.WriteLine("Code: " + code + response.Content);

            Assert.AreEqual(200, code);
            Assert.IsTrue(response.Content!.Contains("Nenhum registro excluído"));
        }
        #endregion Excluir

        #region Editar
        public void realizarEdicaoUsuarioComSucesso()
        {
            string Cadastro1 = realizarCadastroAdm();
            JsonNode jsn1 = JsonNode.Parse(Cadastro1)!;
            string id = (string)jsn1["idUsuario"]!;

            Random random = new Random();
            string rdn = random.Next(1, 100).ToString();

            RestRequest request = new RestRequest($"/usuarios/{id}", Method.Put);
            request.AddJsonBody(new
            {
                nome = "Usuário Editado",
                email = "usuarioEditado" + rdn + "@qa.com",
                password = "editar123",
                administrador = "true"
            });

            response = restClient!.Execute(request);
            var code = (int)response.StatusCode;

            Console.WriteLine("id: " + idUser);
            Console.WriteLine("Code: " + code + " Response " + response.Content);

            Assert.AreEqual(200, code);
            Assert.IsTrue(response.Content!.Contains("Registro alterado com sucesso"));
        }

        public void realizarEdicaoUsuarioComMesmoEmail()
        {
            string Cadastro1 = realizarCadastroAdm();
            JsonNode jsn1 = JsonNode.Parse(Cadastro1)!;
            string email = (string)jsn1["email"]!;

            string Cadastro2 = realizarCadastroAdm();
            JsonNode jsn2 = JsonNode.Parse(Cadastro2)!;
            string id = (string)jsn2["idUsuario"]!;

            RestRequest request = new RestRequest($"/usuarios/{id}", Method.Put);
            request.AddJsonBody(new
            {
                nome = "Usuário Editado",
                email = email,
                password = "editando123",
                administrador = "true"
            });

            response = restClient!.Execute(request);
            var code = (int)response.StatusCode;

            Console.WriteLine("Code: " + code + " Response " + response.Content);

            Assert.AreEqual(400, code);
            Assert.IsTrue(response.Content!.Contains("Este email já está sendo usado"));
        }

        #endregion Editar
    }
}
