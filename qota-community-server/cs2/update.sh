#!/bin/bash
/home/steam/steamcmd/steamcmd.sh +@ShutdownOnFailedCommand 1 +@NoPromptForPassword 1 \
        +@sSteamCmdForcePlatformType linux \
        +login anonymous +force_install_dir "/home/steam/cs2/" \
        +app_update 730 +quit +exit
