<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/5.11.3/main.min.css">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/5.11.3/main.min.js"></script>
    <!-- <link rel="stylesheet" href="css/backoffice.css"> -->
    <title>Agenda</title>
    
</head>
<body>
    <div class="header">
        <h1>Bem-vindo, Usuario</h1>
        <button onclick="window.location.href='logoff.php'">Logoff</button>
    </div>

    <div class="card-container">
        <div class="card" onclick="window.location.href='cadastrar_cliente.php'">Cadastrar Cliente</div>
        <div class="card" onclick="window.location.href='listar_clientes.php'">Listar Clientes</div>
        <div class="card" onclick="window.location.href='cadastrar_funcionario.php'">Cadastrar Funcionário</div>
        <div class="card" onclick="window.location.href='listar_funcionarios.php'">Listar Funcionários</div>
        <div class="card" onclick="window.location.href='cadastrar_agenda.php'">Cadastrar Agenda</div>
        <div class="card" onclick="window.location.href='listar_agendas.php'">Listar Agendas</div>
        <div class="card">Em breve...</div>
        <div class="card">Em breve...</div>
    </div>

    <div class="calendar-container">
        <div id="calendar"></div>
    </div>
</body>

<style>
    body {
        background-color: #121212;
        color: #ffffff;
    }
    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 10px 20px;
        background-color: #1f1f1f;
    }
    .header button {
        background-color: #007bff;
        color: #ffffff;
        border: none;
        padding: 5px 10px;
        border-radius: 5px;
        transition: background-color 0.3s ease;
    }
    .header button:hover {
        background-color: #0056b3;
    }
    .card-container {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 20px;
        padding: 20px;
        margin: auto;
        width: 90%;
    }
    .card {
        background-color: #1f1f1f;
        color: #ffffff;
        border: 1px solid #007bff;
        border-radius: 10px;
        padding: 20px;
        text-align: center;
        cursor: pointer;
        transition: transform 0.3s ease, background-color 0.3s ease;
    }
    .card:hover {
        transform: scale(1.05);
        background-color: #007bff;
        color: #ffffff;
    }
    .calendar-container {
        margin: 20px;
        padding: 20px;
        background-color: #1f1f1f;
        border-radius: 10px;
    }
    #calendar {
        margin: auto;
    }
    </style>
</html>