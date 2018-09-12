FROM ubuntu:xenial

RUN echo "deb http://ppa.launchpad.net/ansible/ansible/ubuntu trusty main" >> /etc/apt/sources.list
RUN apt-key adv --keyserver keyserver.ubuntu.com --recv-keys 93C4A3FD7BB9C367
RUN apt-get update && apt-key update

RUN apt-get install -y \
    vim \
    iputils-ping \
    software-properties-common \
    openssh-server \
    openssh-client \
    ansible

RUN echo "root:docker" | chpasswd

RUN sed -ri 's/^#?PermitRootLogin\s+.*/PermitRootLogin yes/' /etc/ssh/sshd_config
RUN sed -ri 's/^#?UsePAM\s+.*/UsePAM yes/' /etc/ssh/sshd_config
RUN sed -ri 's/^#?PasswordAuthentication\s+.*/PasswordAuthentication no/' /etc/ssh/sshd_config
RUN sed -i "/\[defaults\]/a host_key_checking=False" /etc/ansible/ansible.cfg

RUN mkdir /var/run/sshd

RUN mkdir /root/.ssh

COPY AnsibleLabKey AnsibleLabKey.pub /root/.ssh/

ENV container=docker

ENTRYPOINT ["/bin/bash"]