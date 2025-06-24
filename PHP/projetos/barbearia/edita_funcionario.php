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

$id = $_GET['id'];

//preenche os campos do formulário com os dados do funcionario e usuario
$sql = "SELECT * FROM funcionarios INNER JOIN usuarios ON USU_FK_FUNC_ID = FUNC_ID 
        WHERE FUNC_ID = $id";
$dados = mysqli_query($link, $sql);

//preenche os campos com while
while ($linha = mysqli_fetch_assoc($dados)) {
    $nomefuncionario = $linha['FUNC_NOME'];
    $cpf = $linha['FUNC_CPF'];
    $funcao = $linha['FUNC_FUNCAO'];
    $telefone = $linha['FUNC_TEL'];

    $usuario = $linha['USU_LOGIN'];
    $senha = $linha['USU_SENHA'];
    $ativo = $linha['USU_ATIVO'];
}

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    
    $idfun = $_POST['txtID']; // Certifique-se de que o ID do funcionário está sendo enviado no formulário

    $nomefuncionario = $_POST ['txtNome'];
    $funcao = $_POST ['txtFuncao'];
    $telefone = $_POST ['txtTelefone'];

    $usuario = $_POST['txtUsuario'];
    $senha = $_POST['txtSenha'];
    $ativo = $_POST['rbAtivo'];

    // alteraçao dos dados do funcionario e usuario do sistema
    $sqlfun = "UPDATE funcionarios SET 
        FUNC_NOME = '$nomefuncionario', 
        FUNC_FUNCAO = '$funcao', 
        FUNC_TEL = '$telefone'
    WHERE FUNC_ID = $idfun";
    mysqli_query($link, $sqlfun);

    $sqlusu = "UPDATE usuarios SET 
        USU_SENHA = '$senha', 
        USU_ATIVO = $ativo 
    WHERE USU_FK_FUNC_ID = $idfun";
    mysqli_query($link, $sqlusu);
    
    echo "<script>alert('Funcionário atualizado com sucesso!');</script>";
    echo "<script>window.location.href = 'lista_funcionario.php';</script>";
    exit();
}

?>

<!DOCTYPE html>
<html lang="pt-br">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="css/cadastro_funcionario.css">
    <link rel="stylesheet" href="css/global.css">
    <title>Edição de funcionario</title>
</head>

    <body>
        <form id="login" class="form1" action="edita_funcionario.php" method="post">
            <h2> Edição de funcionário </h2>
            <br>
            <input type="text" name="txtID" value="<?=$id?>" disabled>
            <input type="text" name="txtNome" value="<?=$nomefuncionario?>" required>
            <input type="text" name="txtCPF" value="<?=$cpf?>" disabled required>
            <input type="text" name="txtFuncao" value="<?=$funcao?>" required>
            <input type="number" name="txtTelefone" value="<?=$telefone?>" required>
            <br>
            <br>
            <h2> Edição de usuário sistema</h2>
            <input type='text' name='txtUsuario' value="<?=$usuario?>" disabled>
            <input type='password' name='txtSenha' value="<?=$senha?>">
            <br>
            <label>
                <input type="radio" name="rbAtivo" value="1" <?= $ativo == 1 ? 'checked' : '' ?> required> Ativo
                <input type="radio" name="rbAtivo" value="0" <?= $ativo == 0 ? 'checked' : '' ?> required> Inativo
            </label>
            <br>
            <button type="submit">Salvar</button>
        </form>    
    </body>

</html>