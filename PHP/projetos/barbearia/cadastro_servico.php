<?php
include('utils/conectadb.php');

session_start();

if (isset($_SESSION['idfuncionario'])) {
    $idfuncionario = $_SESSION['idfuncionario'];
    $sql = "SELECT FUNC_NOME FROM funcionarios WHERE FUNC_ID = '$idfuncionario'";
    $enviaquery = mysqli_query($link, $sql);
    $nomeusuario = mysqli_fetch_array($enviaquery)[0];
} else {
    echo "<script>alert('Usuário não logado!');</script>";
    echo "<script>window.location.href = 'login.php';</script>";
} 

if($_SERVER['REQUEST_METHOD'] == 'POST') {
    $nomeservico = $_POST['txtNome'];
    $precoservico = $_POST['txtPreco'];
    $duracaoservico = $_POST['txtTempo'];
    $ativoservico = $_POST['rbAtivo'];
    
    //tratamento da imagem
    if(isset($_FILES['txtImagem']) && $_FILES['txtImagem']['error'] === UPLOAD_ERR_OK) {
        $imagem_temp = $_FILES['txtImagem']['tmp_name'];
        $imagem = file_get_contents($imagem_temp);
        $imagem_base64 = base64_encode($imagem); 
    }

    $sql = "SELECT COUNT(SERV_ID) FROM servicos WHERE SERV_NOME = '$nomeservico'";
    $enviaquery1 = mysqli_query($link, $sql);
    $retorno = mysqli_fetch_array($enviaquery1) [0];

    if ($retorno == 1) {
        echo "<script>alert('Serviço já cadastrado!');</script>";
        echo "<script>window.location.href = 'cadastro_servico.php';</script>";
    } else {      
        $sqlcadastro = "INSERT INTO servicos (SERV_NOME, SERV_PRECO, SERV_TEMPO, SERV_ATIVO, SERV_IMAGEM) 
        VALUES ('$nomeservico', '$precoservico', '$duracaoservico', '$ativoservico', '$imagem_base64')";
        $enviaquery2 = mysqli_query($link, $sqlcadastro); //usa a variável apenas se quiser algum retorno
        if ($enviaquery2) {
            echo "<script>alert('Serviço cadastrado com sucesso!');</script>";
            echo "<script>window.location.href = 'lista_servico.php';</script>";
        } else {
            echo "<script>alert('Erro ao cadastrar serviço!');</script>";
            echo "<script>window.location.href = 'cadastro_servico.php';</script>";
        }
    }
}
?>

<!DOCTYPE html>
<html lang="pt-br">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="css/cadastro_servico.css">
    <link rel="stylesheet" href="css/global.css">
    <title>Cadastro de serviços</title>
</head>

    <body>
        <form id="login" class="form1" action="cadastro_servico.php" method="post" enctype="multipart/form-data">
           
            <h2> Cadastro de serviços </h2>
            <br>
            <input type="text" name="txtNome" placeholder="Nome serviço" required>
            <input type="decimal" name="txtPreco" placeholder="Preço">
            <input type="number" name="txtTempo" placeholder="Duração (em minutos)" required>
            <input type="file" name="txtImagem">
            <script>
                document.addEventListener('DOMContentLoaded', function () {
                    const fileInput = document.querySelector('input[name="txtImagem"]');
                    const previewContainer = document.createElement('div');
                    const previewImage = document.createElement('img');
                    previewImage.style.maxWidth = '100px';
                    previewImage.style.maxHeight = '100px';
                    previewImage.style.display = 'none';
                    previewContainer.appendChild(previewImage);
                    fileInput.parentNode.insertBefore(previewContainer, fileInput.nextSibling);

                    fileInput.addEventListener('change', function () {
                        const file = fileInput.files[0];
                        if (file) {
                            const reader = new FileReader();
                            reader.onload = function (e) {
                                previewImage.src = e.target.result;
                                previewImage.style.display = 'block';
                            };
                            reader.readAsDataURL(file);
                        } else {
                            previewImage.style.display = 'none';
                        }
                    });
                });
            </script>
            <br>
            <br>
            <h2> Cadastrar serviço como: </h2>
            <label>
                <input type="radio" name="rbAtivo" value="1" checked required> Ativo
                <input type="radio" name="rbAtivo" value="0" required> Inativo
            </label>
            <br>
            <button type="submit">Cadastrar</button>
        </form>    
    </body>

</html>