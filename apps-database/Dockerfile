FROM postgres:10.5
WORKDIR /app
RUN apt-get update
RUN apt-get -y install cron rsyslog
COPY ./shells/* /var/lib/postgresql/
COPY ./startup.sh /
RUN chmod +x /var/lib/postgresql/backup.sh
RUN chmod +x /startup.sh
COPY ./backupcron /etc/cron.d/backupcron
RUN chmod 0644 /etc/cron.d/backupcron
RUN crontab /etc/cron.d/backupcron
# RUN crontab -l | { cat; echo "*/1 * * * * /var/tmp/a.md"; } | crontab -
ENTRYPOINT [ "/startup.sh" ]
