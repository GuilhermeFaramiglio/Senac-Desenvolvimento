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

//bloco de alteração para correção de ID inválido
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $id = isset($_POST['txtID']) ? intval($_POST['txtID']) : 0;
} else {
    $id = isset($_GET['id']) ? intval($_GET['id']) : 0;
}

if ($id <= 0) {
    echo "<script>alert('ID inválido!');</script>";
    echo "<script>window.location.href = 'lista_funcionario.php';</script>";
    exit();
}

//preenche os campos do formulário com os dados do funcionario e usuario
$sql = "SELECT * FROM clientes WHERE CLI_ID = $id";
$dados = mysqli_query($link, $sql);

//preenche os campos com while
while ($linha = mysqli_fetch_assoc($dados)) {
    $nomecliente = $linha['CLI_NOME'];
    $cpfcliente = $linha['CLI_CPF'];
    $telefonecliente = $linha['CLI_TEL'];
    $datanascimento = $linha['CLI_DATANASCIMENTO'];
    $ativocliente = $linha['CLI_ATIVO'];
}

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    
    $idcli = $_POST['txtID']; // Certifique-se de que o ID do funcionário está sendo enviado no formulário

    $nomecliente = $_POST ['txtNome'];
    $telefonecliente = $_POST ['txtTelefone'];
    $datanascimento = $_POST ['txtDatanascimento'];
    $ativocliente = $_POST['rbAtivo'];

    // alteraçao dos dados do funcionario e usuario do sistema
    $sqlfun = "UPDATE clientes SET 
        CLI_NOME = '$nomecliente',
        CLI_TEL = '$telefonecliente',
        CLI_DATANASCIMENTO = '$datanascimento',
        CLI_ATIVO = $ativocliente
    WHERE CLI_ID = $idcli";    
    mysqli_query($link, $sqlfun);

    echo "<script>alert('Cliente atualizado com sucesso!');</script>";
    echo "<script>window.location.href = 'lista_cliente.php';</script>";
    exit();
}

?>

<!DOCTYPE html>
<html lang="pt-br">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="css/cadastro_cliente.css">
    <link rel="stylesheet" href="css/global.css">
    <title>Edição de cliente</title>
</head>

    <body>
        <form id="login" class="form1" action="edita_cliente.php" method="post">
            <input type="hidden" name="txtID" value="<?=$id?>">
            <h2> Edição de cliente </h2>
            <br>
            <input type="text" name="txtNome" value="<?=$nomecliente?>" required>
            <input type="text" name="txtCPF" value="<?=$cpfcliente?>" required disabled>
            <input type="number" name="txtTelefone" value="<?=$telefonecliente?>" required>
            <input type="date" name="txtDatanascimento" value="<?=$datanascimento?>" required>
            <br>
            <label>
                <input type="radio" name="rbAtivo" value="1" <?= $ativocliente == 1 ? 'checked' : '' ?> required> Ativo
                <input type="radio" name="rbAtivo" value="0" <?= $ativocliente == 0 ? 'checked' : '' ?> required> Inativo
            </label>
            <br>
            <button type="submit">Salvar</button>
        </form>    
    </body>

</html>