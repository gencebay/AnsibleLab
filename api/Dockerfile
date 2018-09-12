FROM ubuntu:xenial

RUN echo "root:docker" | chpasswd

RUN apt-get update && apt-get install -y \ 
    vim \
    iputils-ping \
    software-properties-common \
    openssh-server

RUN sed -ri 's/^#?PermitRootLogin\s+.*/PermitRootLogin yes/' /etc/ssh/sshd_config
RUN sed -ri 's/^#?UsePAM\s+.*/UsePAM yes/' /etc/ssh/sshd_config
RUN sed -ri 's/^#?PasswordAuthentication\s+.*/PasswordAuthentication no/' /etc/ssh/sshd_config

RUN mkdir /var/run/sshd

RUN mkdir /root/.ssh

COPY AnsibleLabKey.pub /root/.ssh

RUN cat /root/.ssh/AnsibleLabKey.pub >> /root/.ssh/authorized_keys

EXPOSE 22

CMD    ["/usr/sbin/sshd", "-D"]