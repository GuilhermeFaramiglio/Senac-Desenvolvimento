<?php

include('utils/conectadb.php');

if ($_SERVER['REQUEST_METHOD'] == 'POST') {

    $_nomefuncionario = $POST ['txtNome'];
    $_cpf = $POST ['txtCPF'];
    $_funcao = $POST ['txtFuncao'];
    $_telefone = $POST ['txtTelefone'];
    $_ativo = $_POST['rbAtivo'];

    $_usuario = $_POST['txtUsuario'];
    $_senha = $_POST['txtSenha'];

    //verifica usuario e senha se existe
    $sql = "SELECT COUNT(FUNC_CPF) FROM funcionarios 
    WHERE FUNC_CPF = '$_cpf'";
    
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
           
            <h2> Cadastro de funcionários </h2>
            <br>
            <input type="text" name="txtNome" placeholder="Nome do funcionário" required>
            <input type="text" name="txtCPF" placeholder="CPF" required>
            <input type="text" name="txtFuncao" placeholder="Função" required>
            <input type="number" name="txtTelefone" placeholder="Telefone" required>
            <label>
                <input type="radio" name="rbAtivo" value="1" checked required> Ativo
                <input type="radio" name="rbAtivo" value="0" required> Inativo
            </label>
            <input type="text" name="txtUsuario" placeholder="Usuário ID">
            <input type="password" name="txtSenha" placeholder="Senha">
            <br>
            <br>
            <button type="submit">Cadastrar</button>
        </form>    
    </body>

</html>