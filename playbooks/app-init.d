#!/bin/sh

### BEGIN INIT INFO
# Provides:          appd
# Required-Start:    $local_fs $remote_fs $network $syslog $named
# Required-Stop:     $local_fs $remote_fs $network $syslog $named
# Default-Start:     2 3 4 5
# Default-Stop:      0 1 6
# Short-Description: starts the appd
# Description:       starts appd using start-stop-daemon
### END INIT INFO

NAME=dotnet
DAEMON=/usr/bin/dotnet
APPDIR=/home/app
DAEMON_OPTS=Api.Hosting.dll
PID=/run/dotnet/appd.pid

test -x $DAEMON || exit 0

. /lib/init/vars.sh
. /lib/lsb/init-functions

start_hosting() {
    # Start the daemon/service
    #
    # Returns:
    #   0 if daemon has been started
    #   1 if daemon was already running
    #   2 if daemon could not be started
	start-stop-daemon --start --background --make-pidfile --chdir $APPDIR --pidfile $PID --exec $DAEMON --test > /dev/null \
		|| return 1
	start-stop-daemon --start --background --make-pidfile --chdir $APPDIR --pidfile $PID --exec $DAEMON -- $DAEMON_OPTS 2>/dev/null \
		|| return 2
}

stop_hosting() {
    # Stops the daemon/service
	#
	# Return
	#   0 if daemon has been stopped
	#   1 if daemon was already stopped
	#   2 if daemon could not be stopped
	#   other if a failure occurred
	start-stop-daemon --stop --quiet --pidfile $PID \
                        --retry 300 \
                        --exec $DAEMON
	RETVAL="$?"
	sleep 1
	return "$RETVAL"
}

case "$1" in
	start)
		log_daemon_msg "Starting $NAME"
		start_hosting
		case "$?" in
			0|1) log_end_msg 0 ;;
			2)   log_end_msg 1 ;;
		esac
		;;
	stop)
		log_daemon_msg "Stopping $NAME"
		stop_hosting
		case "$?" in
			0|1) log_end_msg 0 ;;
			2)   log_end_msg 1 ;;
		esac
		;;
	restart)
		log_daemon_msg "Restarting $NAME"
		stop_hosting
		case "$?" in
			0|1)
				start_hosting
				case "$?" in
					0) log_end_msg 0 ;;
					1) log_end_msg 1 ;; # Old process is still running
					*) log_end_msg 1 ;; # Failed to start
				esac
				;;
			*)
				# Failed to stop
				log_end_msg 1
				;;
		esac
		;;
	status)
		status_of_proc -p $PID "$DAEMON" "$NAME" && exit 0 || exit $?
		;;
	*)
		echo "Usage: $NAME {start|stop|restart|status}" >&2
		exit 3
		;;
esac