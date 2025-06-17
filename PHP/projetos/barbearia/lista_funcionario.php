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

//traz os funcionarios do banco
$sqlfun = "SELECT * FROM funcionarios";
$queryfun = mysqli_query($link, $sqlfun);

$sqlusu = "SELECT USU_ID, USU_LOGIN, USU_ATIVO FROM usuarios WHERE USU_ID IN (SELECT USU_FK_FUNC_ID FROM funcionarios)";
$queryusu = mysqli_query($link, $sqlusu);
?>

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Lista de Funcionários</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <link rel="stylesheet" href="css/lista_funcionario.css">
</head>
<body>
    <div class="container">
        <h1 class="text-center mb-4">Lista de Funcionários</h1>
        <table class="table table-bordered table-striped">
            <thead class="table-dark">
                <tr>
                    <th><i class="fas fa-id-card"></i> ID</th>
                    <th><i class="fas fa-user"></i> Nome</th>
                    <th><i class="fas fa-id-badge"></i> CPF</th>
                    <th><i class="fas fa-briefcase"></i> Função</th>
                    <th><i class="fas fa-phone"></i> Telefone</th>
                    <th><i class="fas fa-toggle-on"></i> Status</th>
                    <th><i class="fas fa-user-circle"></i> Usuário do Sistema</th>
                    <th><i class="fas fa-check-circle"></i> Usuário Status</th>
                    <th><i class="fas fa-cogs"></i> Ações</th>
                </tr>

                <!-- PHP -->
                 <?php
                 while ($tblfun = mysqli_fetch_array($queryfun)) { 
                 ?> 
                    <tr>
                        <td><?=$tblfun[0]?></td> <!-- ID do Funcionário -->
                        <td><?=$tblfun[1]?></td> <!-- Nome do Funcionário -->
                        <td><?=$tblfun[2]?></td> <!-- CPF do Funcionário -->
                        <td><?=$tblfun[3]?></td> <!-- Função do Funcionário -->
                        <td><?=$tblfun[4]?></td> <!-- Telefone do Funcionário -->
                        <td><?=$tblfun[5]?></td> <!-- Ativo/Inativo do Funcionário -->
                        <?php while ($tblusu = mysqli_fetch_array($queryusu)) { 
                        ?>
                        <td><?=$tblusu[1]?></td> <!-- Nome do Usuário -->    
                        <td><?=$tblusu[2]?></td> <!-- Senha do Usuário  -->   
                    </tr>
                    
                <?php
                }
                ?> 
                <?php 
                }
                ?>                
            </thead>
        </table>
    </div>
    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>