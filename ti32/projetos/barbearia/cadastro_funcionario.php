<?php

include('utils/conectadb.php');

session_start();

if (isset($_SESSION['idfuncionario'])) {

    $idfuncionario = $_SESSION['idfuncionario'];

    $sql = "SELECT FUNC_NOME FROM funcionarios 
        WHERE FUNC_ID = '$idfuncionario'";

    $enviaquery = mysqli_query($link, $sql);
    $nomeusuario = mysqli_fetch_array($enviaquery) [0];
} 
else {
    echo "<script>alert('Usuário não logado!');</script>";
    echo "<script>window.location.href = 'login.php';</script>";
}

if ($_SERVER['REQUEST_METHOD'] == 'POST') {

    $nomefuncionario = $_POST ['txtNome'];
    $cpf = $_POST ['txtCPF'];
    $funcao = $_POST ['txtFuncao'];
    $telefone = $_POST ['txtTelefone'];
    $ativo = $_POST['rbAtivo'];

    $usuario = $_POST['txtUsuario'];
    $senha = $_POST['txtSenha'];

    //verifica usuario e senha se existe
    $sql = "SELECT COUNT(FUNC_CPF) FROM funcionarios 
    WHERE FUNC_CPF = '$cpf'";
    
    $enviaquery = mysqli_query($link, $sql);
    $retorno = mysqli_fetch_array($enviaquery) [0];

    //validar o retorno se existe login e se ativo
    if ($retorno == 1)
    {
        echo "<script>alert('O CPF informado já possui cadastro!');</script>";
        echo "<script>window.location.href = 'cadastro_funcionario.php';</script>";
    }
    else 
    {
        // CASO FUNCIONÁRIO NÃO ESTEJA CADASTRADO
        $sql = "INSERT INTO funcionarios (FUNC_NOME, FUNC_CPF, FUNC_FUNCAO, FUNC_TEL, FUNC_ATIVO)
        VALUES ('$nomefuncionario', '$cpf', '$funcao', '$telefone', $ativo)";

        // CONECTA COM O BANCO E MANDA A QUERY
        $enviaquery = mysqli_query($link, $sql);

        // ROLE COM A TABELA DE USUARIOS
        // PERGUNTA PARA A TABELA DE FUNCIONÁRIO QUAL FOI O ULTIMO ID CADASTRADO
        // ANTES PRECISO SABER SE A VARIÁVEL USUFUN ESTÁ PREENCHIDA
        if($usuario != null){
            // TRAZ O ID DO FUNCIONARIO CADASTRADO PARA PASSAR NO LOGIN
            $sqlfun = "SELECT FUNC_ID FROM funcionarios where FUNC_CPF = '$cpf'";
            $enviaquery = mysqli_query($link, $sqlfun);
            $retorno = mysqli_fetch_array($enviaquery) [0];

            // AGORA SALVAMOS TUDO NA TABELA DO USUARIO
            $sqlusu = "INSERT INTO usuarios (USU_LOGIN, USU_SENHA, USU_FK_FUNC_ID, USU_ATIVO)
            VALUES ('$usuario', '$senha', $retorno, $ativo)";
            $enviaqueryusu = mysqli_query($link, $sqlusu);
        }
        echo ("<script>window.alert('Cadastro realizado com sucesso!');</script>");
        echo "<script>window.location.href = 'backoffice.php';</script>";
        exit();
    }
}

?>

<!DOCTYPE html>
<html lang="pt-br">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="css/cadastro_funcionario.css">
    <link rel="stylesheet" href="css/global.css">
    <title>Cadastro de funcionario</title>
</head>

    <body>
        <form id="login" class="form1" action="cadastro_funcionario.php" method="post">
           
            <h2> Cadastro de funcionário </h2>
            <br>
            <input type="text" name="txtNome" placeholder="Nome do funcionário" required>
            <input type="text" name="txtCPF" placeholder="CPF" required>
            <input type="text" name="txtFuncao" placeholder="Função" required>
            <input type="number" name="txtTelefone" placeholder="Telefone" required>
            <br>
            <br>
            
            <h2> Cadastro de usuário sistema</h2>
            <input type='text' name='txtUsuario' placeholder='Usuário'>
            <input type='password' name='txtSenha' placeholder='Senha'>
            <br>
            <label>
                <input type="radio" name="rbAtivo" value="1" checked required> Ativo
                <input type="radio" name="rbAtivo" value="0" required> Inativo
            </label>
            <br>
            <button type="submit">Cadastrar</button>
        </form>    
    </body>

</html>