namespace ServeRestTreinamentoAPI.Features
{
    [TestClass]
    public class UsuariosFeatures
    {
        Usuarios usuarios = new Usuarios();

        #region Cadastrar

        [TestMethod]
        public void CT01ValidarCadastroUsuario() => usuarios.realizarCadastroComSucesso();

        [TestMethod]
        public void CTO2ValidarCadastroUsuarioExistente() => usuarios.realizarCadastroComDadosExistente();

        [TestMethod]
        public void CT03ValidarCadastroAdministrador() => usuarios.realizarCadastroAdm();

        #endregion Cadastrar

        #region Consultar

        [TestMethod]
        public void CT04ValidarConsultaUsuarioCadastrado() => usuarios.realizarConsultaListarUsuariosCadastrados();

        [TestMethod]
        public void CT05ValidarConsultaUsuarioPorID() => usuarios.realizarConsultaDeUsuarioPorId();

        [TestMethod]
        public void CT06ValidarConsultaUsuarioComIdInexistente() => usuarios.realizarConsultaDeUsuarioComIdInexistente();

        #endregion Consultar

        #region Excluir

        [TestMethod]
        public void CT07ValidarExcluirUsuarioPorId() => usuarios.excluirUsuarioPorId();

        [TestMethod]
        public void CT08ValidarExcluirUsuarioComIdInexistente() => usuarios.excluirUsuarioComIdInexistente();
        #endregion Excluir

        #region Editar

        [TestMethod]
        public void CT09ValidarEditarUsuarioComSucesso() => usuarios.realizarEdicaoUsuarioComSucesso();

        [TestMethod]
        public void CT10ValidarEdicaoDeUsuarioMesmoEmail() => usuarios.realizarEdicaoUsuarioComMesmoEmail();

        #endregion Editar

    }
}
