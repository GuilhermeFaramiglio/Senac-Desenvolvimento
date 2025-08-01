<?php
session_start();

if (isset($_SESSION['usuario'])) {
    $usuario = $_SESSION['usuario'];
} else {
    echo "<script>alert('Usuário não logado!');</script>";
    echo "<script>window.location.href = '../login.html';</script>";
}
?>

<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Página inicial</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        body {
            background-color: #202020;
            color: #ffffff;
        }
        .card {
            background-color: #303030;
            border: none;
        }
        .btn-danger {
            background-color: #007bff;
            border-color: #007bff;
        }
        .btn-danger:hover {
            background-color: #0056b3;
            border-color: #0056b3;
        }
        .card-title {
            color: #ffffff;
        }
        .card-text {
            color: #cccccc;
        }
    </style>
</head>
<body>
    <div class="container mt-5">
        <div class="card shadow-sm">
            <div class="card-body text-center">
                
            <h1 class="card-title">Bem-vindo!</h1>

                <p class="card-text">Olá, <strong><?php echo htmlspecialchars($_SESSION['usuario']); ?></strong>. Você está logado.</p>
                
                
                <form action="lista.php" method="post">
                    <button type="submit" class="btn btn-primary mt-3">Ver Lista de Usuários</button>
                </form>
                
                <br>
                
                <form action="logout.php" method="post">
                    <button type="submit" class="btn btn-danger">Sair</button>
                </form>

            </div>
        </div>
    </div>
    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>