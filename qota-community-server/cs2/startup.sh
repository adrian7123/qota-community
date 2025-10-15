#!/bin/bash

# Script de inicialização do servidor CS2 - Qota Community
# Baseado no comando: cd /home/ubuntu/.local/share/Steam/steamcmd/cs2/game/bin/linuxsteamrt64
# ./cs2 -dedicated +sv_setsteamaccount C103BBAABC437FA5F3F49804D5707EB2 +map de_mirage +hostname "Qota Community Cs2 Server 1" +game_alias competitive +sv_password "six321123"

set -e

echo "=== Iniciando Servidor CS2 - Qota Community ==="
echo "Steam Account ID: ${CS2_SERVERID}"
echo "Hostname: ${CS2_HOSTNAME}"
echo "Porta: ${CS2_PORT}"
echo "Mapa inicial: ${CS2_STARTMAP}"
echo "Game Alias: ${CS2_GAME_ALIAS}"
echo "Senha: ${CS2_PW}"
echo "========================================="

# Diretório do servidor CS2
cd /home/steam/cs2/game/bin/linuxsteamrt64

# Verificar se o executável CS2 existe
if [ ! -f "./cs2" ]; then
    echo "ERRO: cs2 executável não encontrado! Verificando instalação..."
    ls -la /home/steam/.local/share/Steam/steamcmd/cs2/game/bin/
    echo "Tentando instalar/atualizar CS2..."
    ./update.sh
fi

# Executar o servidor CS2 com as configurações específicas
exec ./cs2 \
    -dedicated \
    +sv_setsteamaccount "${CS2_SERVERID}" \
    +map "${CS2_STARTMAP}" \
    +hostname "${CS2_HOSTNAME}" \
    +game_alias "${CS2_GAME_ALIAS}" \
    +sv_password "${CS2_PW}" \
    +rcon_password "${CS2_RCONPW}" \
    +sv_lan "${CS2_LAN}" \
    +exec server.cfg
