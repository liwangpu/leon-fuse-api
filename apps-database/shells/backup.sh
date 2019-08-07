#!/bin/bash

myDBBackupDir="/var/lib/postgresql/dbBackup"

initBackupFiles() {

    if [ ! -e "${myDBBackupDir}/0.bak" ]; then
        touch "${myDBBackupDir}/0.bak"
    fi
    if [ ! -e "${myDBBackupDir}/1.bak" ]; then
        touch "${myDBBackupDir}/1.bak"
    fi
    if [ ! -e "${myDBBackupDir}/2.bak" ]; then
        touch "${myDBBackupDir}/2.bak"
    fi
    if [ ! -e "${myDBBackupDir}/3.bak" ]; then
        touch "${myDBBackupDir}/3.bak"
    fi
    if [ ! -e "${myDBBackupDir}/4.bak" ]; then
        touch "${myDBBackupDir}/4.bak"
    fi
    if [ ! -e "${myDBBackupDir}/5.bak" ]; then
        touch "${myDBBackupDir}/5.bak"
    fi
    if [ ! -e "${myDBBackupDir}/6.bak" ]; then
        touch "${myDBBackupDir}/6.bak"
    fi
}

transferBackupFiles() {
    cp -p "${myDBBackupDir}/5.bak" "${myDBBackupDir}/6.bak"
    cp -p "${myDBBackupDir}/4.bak" "${myDBBackupDir}/5.bak"
    cp -p "${myDBBackupDir}/3.bak" "${myDBBackupDir}/4.bak"
    cp -p "${myDBBackupDir}/2.bak" "${myDBBackupDir}/3.bak"
    cp -p "${myDBBackupDir}/1.bak" "${myDBBackupDir}/2.bak"
    cp -p "${myDBBackupDir}/0.bak" "${myDBBackupDir}/1.bak"
}

backupDatabase() {
    pg_dumpall -U postgres >"${myDBBackupDir}/0.bak"
}

initBackupFiles
transferBackupFiles
backupDatabase
