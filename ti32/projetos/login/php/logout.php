<?php 

session_start();

session_destroy(); // Destroi a sessão

echo "<script>alert('Você foi desconectado!');</script>";
echo "<script>window.location.href = '../login.html';</script>"; // Redireciona para a página de login

?>