# Dockerfile para CS2 Server - Qota Community
FROM joedwards32/cs2:latest

# Definir usuário steam
USER steam

# Definir diretório de trabalho
WORKDIR /home/steam

# Variáveis de ambiente para CS2
ENV STEAMAPPID=730
ENV STEAMAPP=cs2
ENV STEAMAPPDIR=/home/steam/cs2

# Configurações específicas do servidor baseadas no comando cs2
ENV CS2_SERVERID=
ENV CS2_RCONPW=admin123
ENV CS2_PW=six321123
ENV CS2_PORT=27015
ENV CS2_STARTMAP=de_mirage
ENV CS2_HOSTNAME="Qota Community Cs2 Server 1"
ENV CS2_GAME_ALIAS=competitive
ENV CS2_MAXPLAYERS=10
ENV CS2_LAN=0

# Portas que serão expostas
EXPOSE 27015/tcp
EXPOSE 27015/udp
EXPOSE 27020/udp

# Instalar CS2 usando SteamCMD (já instalado na imagem base joedwards32/cs2)
# A imagem base já tem CS2 instalado, apenas garantimos que está atualizado
COPY --chown=steam:steam update.sh /home/steam/update.sh
RUN ./update.sh

# Criar script de inicialização personalizado
COPY --chown=steam:steam startup.sh /home/steam/startup.sh
RUN chmod +x /home/steam/startup.sh

# Criar diretórios necessários para CS2
RUN mkdir -p /home/steam/cs2/game/csgo/cfg

# Arquivo de configuração do servidor CS2
RUN echo "// Configuração do servidor CS2 Qota Community" > /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "hostname \"${CS2_HOSTNAME}\"" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "sv_password \"${CS2_PW}\"" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "rcon_password \"${CS2_RCONPW}\"" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "sv_lan ${CS2_LAN}" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "sv_cheats 0" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "sv_pure 1" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "mp_autoteambalance 1" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "mp_limitteams 1" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "mp_maxmoney 16000" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "mp_startmoney 800" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "mp_buytime 20" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "mp_freezetime 15" >> /home/steam/cs2/game/csgo/cfg/server.cfg && \
    echo "mp_roundtime 1.92" >> /home/steam/cs2/game/csgo/cfg/server.cfg

# Definir comando padrão
CMD ["/home/steam/startup.sh"]
