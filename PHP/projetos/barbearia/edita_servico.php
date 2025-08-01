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
    echo "<script>window.location.href = 'lista_servico.php';</script>";
    exit();
}

//preenche os campos do formulário com os dados do funcionario e usuario
$sql = "SELECT * FROM servicos WHERE SERV_ID = $id";
$dados = mysqli_query($link, $sql);

//preenche os campos com while
while ($linha = mysqli_fetch_assoc($dados)) {
    $nomeservico = $linha['SERV_NOME'];
    $precoservico = $linha['SERV_PRECO'];
    $temposervico = $linha['SERV_TEMPO'];
    $ativocliente = $linha['SERV_ATIVO'];
    $imagemservicoatual = $linha['SERV_IMAGEM'];
}

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    
    $idserv = $_POST['txtID']; 

    $nomeservico = $_POST ['txtNome'];
    $precoservico = $_POST ['txtPreco'];
    $temposervico = $_POST ['txtTempo'];
    $ativocliente = $_POST['rbAtivo'];
    if (isset($_FILES['txtImagem']) && $_FILES['txtImagem']['error'] == UPLOAD_ERR_OK) {
        $imagemservicoatual = file_get_contents($_FILES['txtImagem']['tmp_name']);
        $imagemalterada = base64_encode($imagemservicoatual);
    } else {
        $imagemalterada = $imagemservicoatual;
    }
    $sqlserv = "UPDATE servicos SET 
        SERV_NOME = '$nomeservico',
        SERV_PRECO = '$precoservico',
        SERV_TEMPO = '$temposervico',
        SERV_IMAGEM = '$imagemalterada',
        SERV_ATIVO = $ativocliente
    WHERE SERV_ID = $idserv";    
    mysqli_query($link, $sqlserv);

    echo "<script>alert('Serviço atualizado com sucesso!');</script>";
    echo "<script>window.location.href = 'lista_servico.php';</script>";
    exit();
}
?>

<!DOCTYPE html>
<html lang="pt-br">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="css/cadastro_servico.css">
    <link rel="stylesheet" href="css/global.css">
    <title>Edição de serviços</title>
</head>

    <body>
        <form id="login" class="form1" action="edita_servico.php" method="post" enctype="multipart/form-data">
            <h2> Edição de serviços </h2>
            <br>
            <input type="hidden" name="txtID" value="<?=$id?>">
            <input type="text" name="txtNome" value="<?=$nomeservico?>" required>
            <input type="decimal" name="txtPreco" value="<?=$precoservico?>">
            <input type="number" name="txtTempo" value="<?=$temposervico?>" required>
            <input type="file" name="txtImagem" >
            
            <div id='cat_imagem'>
                <img src='data:img/jpeg;base64, <?= $imagemservicoatual?>' width=100 height=100>
            </div>
            <br>
            <br>
            <h2> Status do serviço: </h2>
            <label>
                <input type="radio" name="rbAtivo" value="1" checked required> Ativo
                <input type="radio" name="rbAtivo" value="0" required> Inativo
            </label>
            <br>
            <button type="submit">Salvar</button>
        </form>    
    </body>

</html>