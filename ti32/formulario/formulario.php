<?php

if($_SERVER['REQUEST_METHOD'] == 'POST') {
    $nome = $_POST['txtNome'];
    $sobrenome = $_POST['txtSobrenome'];
    $idade = $_POST['txtIdade'];
}

$mensagem = "Nome: $nome $sobrenome <br>Idade: $idade";

?>

<!DOCTYPE html>
<html lang="pt-br">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="css/style.css">
    <title>Formulário</title>
</head>

    <body>
        <form class = "form1" action="formulario.php" method="post">

            <label> Nome </label>
            <br>
            <input type="text" name="txtNome" placeholder="Preencha seu nome" required>
            <br>
            <br>
            <label> Sobrenome </label>
            <br>
            <input type="text" name="txtSobrenome" placeholder="Preencha seu sobrenome">
            <br>
            <br>
            <label> Idade </label>
            <br>
            <input type="number" name="txtIdade" placeholder="Preencha sua idade">
            <br>
            <br>
            
            <button type="submit">Enviar</button>
        </form>    

        <h3> <?php echo($mensagem); ?> </h3>
    </body>

</html>