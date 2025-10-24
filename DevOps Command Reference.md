# 📘 Ubuntu DevOps Command Reference — With Practical Examples

A clean, single-copy reference of **must-know Linux commands** for DevOps engineers using Ubuntu.  
Each entry includes **purpose**, **important switches**, and **a real example**.

---

## 1) Navigation & Files

| Command | Purpose | Common Switches | Example |
|---------|---------|------------------|---------|
| `pwd` | Print working directory | — | `pwd` → `/home/ubuntu` |
| `ls` | List files and folders | `-l` (long) `-a` (hidden) `-h` (human readable) `-S` (sort by size) | `ls -lah` |
| `cd` | Change directory | `-` (previous) | `cd /var/log` → `cd -` |
| `tree` | Show directory tree | `-L` (depth) | `tree -L 2` |
| `cat` | Print file contents | `-n` (line numbers) | `cat -n /etc/hosts` |
| `less` | View file page by page | `/` (search) `-S` (no wrap) | `less -S access.log` |
| `head` / `tail` | Show first or last lines | `-n` (line count) `-f` (follow tail) | `tail -n 50 -f app.log` |
| `touch` | Create empty file / update timestamp | — | `touch .env` |
| `mkdir` | Create directory | `-p` (create parents) | `mkdir -p /opt/project/releases` |
| `cp` | Copy files or directories | `-r` (recursive) `-a` (archive) | `cp -a config/ backup/` |
| `mv` | Move or rename | — | `mv server.old server` |
| `rm` | Remove files or directories | `-r` (recursive) `-f` (force) | `rm -rf node_modules/` |

---

## 2) Permissions & Ownership

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `chmod` | Change permissions | `u,g,o` `+/-/=` or octal like `600` | `chmod 600 key.pem` |
| `chown` | Change owner and group | `-R` (recursive) | `sudo chown -R ubuntu:www-data /var/www` |
| `chgrp` | Change group | `-R` (recursive) | `sudo chgrp -R www-data /var/www` |
| `umask` | Set default file permission mask | — | `umask 022` |

**Quick example:**  
`ls -l key.pem` → `-rw------- 1 ubuntu ubuntu 1675 Oct 24 key.pem` after `chmod 600`.

---

## 3) Find, Search & Replace

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `find` | Search for files | `-name` `-mtime` `-size` `-type` `-exec` | `find /var/log -type f -name "*.log"` |
| `grep` | Search in text | `-R` (recursive) `-i` (ignore case) `-n` (line) | `grep -Ri "ERROR" /var/log` |
| `rg` | Fast grep (ripgrep) | similar to grep | `rg -n "TODO" src/` |
| `sed` | Find and replace | `-i` (in-place) | `sed -i 's/8080/80/g' .env` |
| `awk` | Process columns | `-F` (delimiter) | `awk -F: '{print $1}' /etc/passwd` |
| `xargs` | Build commands from output | `-0` (null) | `rg -l "DEBUG" | xargs sed -i 's/DEBUG/INFO/g'` |

---

## 4) Compression & Archives

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `tar` | Create/extract tarballs | `czf` (create gz) `xzf` (extract gz) | `tar czf backup.tgz dist/` → `tar xzf backup.tgz -C /opt` |
| `zip` / `unzip` | Zip or unzip files | `-r` (recursive) | `zip -r site.zip public/` → `unzip site.zip -d /var/www` |

---

## 5) System Info & Monitoring

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `uname` | Kernel info | `-a` (all) | `uname -a` |
| `lsb_release` | Distro info | `-a` | `lsb_release -a` |
| `uptime` | Show load and uptime | — | `uptime` |
| `top` / `htop` | Live CPU/mem/process | `htop` is interactive | `top` |
| `vmstat` | Memory/CPU stats | `1` (interval) | `vmstat 1` |
| `free` | Memory usage | `-h` (human readable) | `free -h` |
| `df` | Disk usage | `-h` | `df -h` |
| `du` | Folder/file size | `-sh` (summary) | `du -sh *` |
| `ps` | Process listing | `aux` or `-ef` | `ps aux | grep nginx` |
| `kill` / `pkill` | Kill processes | `-9` force | `pkill -f gunicorn` |

---

## 6) Services, Boot & Logs (systemd)

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `systemctl` | Manage services | `start` `stop` `status` `enable` `disable` | `sudo systemctl restart nginx` |
| `journalctl` | Logs for systemd units | `-u` (unit) `-f` (follow) `-n` (lines) | `sudo journalctl -u nginx -f` |
| `loginctl` | Session info | — | `loginctl list-sessions` |

**Example:**  
`sudo systemctl enable myapp.service` → start on boot.

---

## 7) Networking & HTTP

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `ip` | Network info | `a` (addr) `r` (route) | `ip a` |
| `ss` | Ports/sockets | `-lntp` | `ss -lntp` |
| `curl` | HTTP/API requests | `-I` (headers) `-L` (redirect) `-X` (method) `-d` (data) `-H` (header) | `curl -I https://example.com` |
| `wget` | Download files | `-O` (output) | `wget -O app.deb URL` |
| `nc` | Test TCP/UDP | `-zv` (scan) | `nc -zv 127.0.0.1 22` |

**Example:**  
`curl -X POST -H "Content-Type: application/json" -d '{"hello":"world"}' https://api.server.com`

---

## 8) Users, Groups & Sudo

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `id` / `whoami` | Show current user info | — | `id` |
| `adduser` | Add user with home dir | — | `sudo adduser deploy` |
| `usermod` | Modify user | `-aG` (append group) | `sudo usermod -aG docker ubuntu` |
| `passwd` | Change password | — | `sudo passwd deploy` |
| `chsh` | Change shell | `-s` | `chsh -s /usr/bin/zsh` |
| `visudo` | Edit sudoers safely | — | `sudo visudo` |

**Example:**  
`sudo usermod -aG sudo deploy` → adds `deploy` to sudo group.

---

## 9) Package Management

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `apt update` | Update package list | — | `sudo apt update` |
| `apt upgrade` | Upgrade packages | `-y` | `sudo apt upgrade -y` |
| `apt install` | Install package | `-y` | `sudo apt install nginx -y` |
| `apt remove` | Remove package | `-y` | `sudo apt remove nginx -y` |
| `apt-cache policy` | Check package version | — | `apt-cache policy nginx` |
| `dpkg -i` | Install `.deb` file | — | `sudo dpkg -i package.deb` |
| `snap` | Manage snap packages | — | `sudo snap install core` |

---

## 10) SSH & File Transfer

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `ssh` | Remote login | `-i` (identity) `-p` (port) | `ssh -i key.pem ubuntu@192.168.1.10` |
| `scp` | Secure copy | `-i` (key) `-r` (recursive) | `scp -i key.pem -r dist/ ubuntu@host:/var/www/` |
| `rsync` | Sync directories | `-a` (archive) `-v` (verbose) `-z` (compress) `--delete` | `rsync -avz dist/ ubuntu@host:/var/www/` |
| `ssh-copy-id` | Add pub key to server | — | `ssh-copy-id -i ~/.ssh/id_rsa.pub ubuntu@host` |
| `ssh-agent` / `ssh-add` | Manage keys | — | `eval "$(ssh-agent -s)"; ssh-add ~/.ssh/id_ed25519` |

**Example:**  
`scp -i key.pem backup.tgz ubuntu@ec2-xx-xx-xx-xx.compute.amazonaws.com:/home/ubuntu`

---

## 11) Firewall

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `ufw` | Easy firewall | `enable` `disable` `allow` `deny` | `sudo ufw allow 22/tcp` |
| `iptables` | Low-level firewall | `-L` (list) `-n` (numeric) `-v` (verbose) | `sudo iptables -L -n -v` |

**Example:**  
`sudo ufw allow 80,443/tcp` → open web ports.

---

## 12) Disks, Mounts & Filesystems

| Command | Purpose | Switches | Example |
|---------|---------|----------|---------|
| `lsblk` | Show block devices | `-f` (with FS) | `lsblk -f` |
| `blkid` | Show UUID/labels | — | `sudo blkid` |
| `mount` / `umount` | Mount/unmount | `-t` (fs type) | `sudo mount -t ext4 /dev/xvdf1 /mnt/data` |
| `df` | Disk usage | `-h` | `df -h` |
| `du` | Directory size | `-sh` | `du -sh /var/www` |

**Example:**  
`sudo mount /dev/xvdf1 /mnt/data` → temporary mount.  
To persist, add to `/etc/fstab`.

---

## 13) Misc Useful Commands

| Command | Purpose | Example |
|---------|---------|---------|
| `history` | Show command history | `history | grep ssh` |
| `watch` | Re-run a command at interval | `watch -n 2 df -h` |
| `alias` | Create short commands | `alias ll='ls -lah'` |
| `which` | Locate binary path | `which nginx` |
| `whereis` | Locate binary and man page | `whereis python3` |
| `man` | Manual page | `man tar` |
| `echo` | Print text or vars | `echo $HOME` |
| `export` | Set env vars | `export APP_ENV=prod` |
| `cron` | Schedule jobs | `crontab -e` |
| `at` | Run command later | `echo "reboot" | at now + 1 minute` |

---

✅ **Tip for daily DevOps**:
- Always check `journalctl` or `tail -f` on logs first when debugging.  
- Use `df -h`, `free -h`, and `top` to detect system bottlenecks.  
- Keep `rsync` handy for quick, reliable deployments.  
- Combine `grep`, `awk`, `sed`, `find`, and `xargs` to automate text and file ops.

```bash
# Quick Handy DevOps combo example:
find /var/log -name "*.log" -mtime +7 -print0 | xargs -0 rm -f


# Ubuntu DevOps Command Guide (with Examples)

**Pro tip:** To quickly see a command’s usage, add `--help`.  
Example: `ls --help`

---

## 1) `ls`
**List directory contents.**  
**Syntax:** `ls [options] [path]`  
**Common:** `-l` long, `-a` hidden, `-h` human, `-R` recursive  
**Example:** `ls -lahR /var/www`

---

## 2) `pwd`
**Show full path of current directory.**  
**Syntax:** `pwd [options]`  
**Common:** `-L` logical (respects symlinks), `-P` physical  
**Example:** `pwd -P`

---

## 3) `cd`
**Change directories.**  
**Syntax:** `cd [path]`  
**Shortcuts:** `cd` (home), `cd ..` (up), `cd -` (previous)  
**Example:** `cd /etc/nginx/sites-available && cd -`

---

## 4) `mkdir`
**Create directories.**  
**Syntax:** `mkdir [options] dir...`  
**Common:** `-p` parents, `-m` mode  
**Example:** `mkdir -p -m 755 /opt/app/releases/2025-10-24`

---

## 5) `rmdir`
**Remove **empty** directories.**  
**Syntax:** `rmdir [options] dir...`  
**Common:** `-p` remove parents if empty  
**Example:** `rmdir -p /tmp/app/cache/old`

---

## 6) `rm`
**Remove files/dirs.**  
**Syntax:** `rm [options] file...`  
**Common:** `-r` recursive, `-f` force, `-i` prompt  
**Example:** `rm -rf /var/tmp/build-*`

---

## 7) `cp`
**Copy files/dirs.**  
**Syntax:** `cp [options] src... dest`  
**Common:** `-a` archive, `-r` recursive, `-v` verbose  
**Example:** `cp -av config/ /etc/myapp/`

---

## 8) `mv`
**Move/rename.**  
**Syntax:** `mv [options] src... dest`  
**Common:** `-v` verbose, `-n` no-clobber, `-i` prompt  
**Example:** `mv -v app.env app.env.bak`

---

## 9) `touch`
**Create empty file / update mtime.**  
**Syntax:** `touch [options] file...`  
**Common:** `-t` timestamp, `-a` atime, `-m` mtime  
**Example:** `touch -t 202510240900 .deployment_stamp`

---

## 10) `file`
**Detect file type.**  
**Syntax:** `file [options] file...`  
**Common:** `-k` keep going (more detail), `-i` mime  
**Example:** `file -i backup.tgz`

---

## 11) `zip` / `unzip`
**Compress/extract ZIP.**  
**Syntax:** `zip [options] zipfile files...` / `unzip [options] zipfile`  
**Common:** `zip -r` recursive, `unzip -d DIR` dest  
**Example:** `zip -r site.zip public/ && unzip site.zip -d /var/www`

---

## 12) `tar`
**Create/extract tar archives (with/without compression).**  
**Syntax (create gz):** `tar -czf archive.tar.gz paths...`  
**Syntax (extract):** `tar -xzf archive.tar.gz -C /dest`  
**Common:** `-c` create, `-x` extract, `-z` gzip, `-j` bzip2, `-J` xz, `-f` file  
**Example:** `tar -czf release.tgz dist/`

---

## 13) Editors: `nano` / `vi` / `jed`
**Edit files.**  
**Syntax:** `nano file` / `vi file` / `jed file`  
**Example:** `sudo nano /etc/systemd/system/myapp.service`

---

## 14) `cat` / `tac`
**Print/join files (`tac` reverses).**  
**Syntax:** `cat file...`  
**Examples:**  
- `cat /etc/hosts`  
- `cat part1.txt part2.txt > merged.txt`  
- `tac app.log | head -n 20`

---

## 15) `grep`
**Search text.**  
**Syntax:** `grep [options] pattern [files]`  
**Common:** `-R` recursive, `-n` line, `-i` ignore-case, `-E` ERE  
**Example:** `grep -Rni "ERROR" /var/log`

---

## 16) `sed`
**Stream edit / find & replace.**  
**Syntax:** `sed [options] 's/old/new/g' file`  
**Common:** `-i` in-place, `-E` extended regex  
**Example:** `sed -i 's/8080/80/g' .env`

---

## 17) `head`
**First N lines/bytes.**  
**Syntax:** `head [options] file`  
**Common:** `-n N` lines, `-c N` bytes  
**Example:** `head -n 20 app.log`

---

## 18) `tail`
**Last N lines; follow logs.**  
**Syntax:** `tail [options] file`  
**Common:** `-n N`, `-f` follow  
**Example:** `tail -n 200 -f /var/log/nginx/access.log`

---

## 19) `awk`
**Column processing / mini language.**  
**Syntax:** `awk -F'<delim>' 'prog' file`  
**Examples:**  
- `awk -F: '{print $1}' /etc/passwd`  
- `awk '{sum+=$1} END{print sum}' numbers.txt`

---

## 20) `sort`
**Sort lines.**  
**Syntax:** `sort [options] [file]`  
**Common:** `-n` numeric, `-r` reverse, `-k` key  
**Example:** `sort -nrk5,5 usage.txt`

---

## 21) `cut`
**Extract columns/bytes/chars.**  
**Syntax:** `cut [options] file`  
**Common:** `-d` delim, `-f` fields, `-c` chars, `-b` bytes  
**Example:** `cut -d',' -f3-5 data.csv`

---

## 22) `diff`
**Show differences between files.**  
**Syntax:** `diff [options] a b`  
**Common:** `-u` unified, `-c` context, `-i` ignore case  
**Example:** `diff -u config.old.yml config.yml`

---

## 23) `tee`
**Split output to file and stdout.**  
**Syntax:** `cmd | tee [options] file`  
**Common:** `-a` append  
**Example:** `ping -c 5 8.8.8.8 | tee -a netcheck.txt`

---

## 24) `locate` / `updatedb`
**Fast file lookup via DB.**  
**Syntax:** `locate [options] pattern`  
**Common:** `-i` ignore case, `-r` regex  
**Example:** `sudo updatedb && locate nginx.conf`

---

## 25) `find`
**Find files in real time.**  
**Syntax:** `find [path] [tests] [actions]`  
**Common:** `-type f|d` `-name` `-mtime` `-size` `-exec`  
**Example:** `find /var/log -type f -name "*.log" -mtime +7 -delete`

---

## 26) `sudo`
**Run as another user (default root).**  
**Syntax:** `sudo [options] command`  
**Example:** `sudo systemctl restart nginx`

---

## 27) `su` / `whoami`
**Switch user / show current user.**  
**Syntax:** `su [user]` / `whoami`  
**Example:** `su - postgres && whoami`

---

## 28) `chmod`
**Change permissions.**  
**Syntax:** `chmod [options] MODE file`  
**Common:** symbolic `u+rw`, octal `600/644/755`, `-R` recursive  
**Example:** `chmod 600 ~/.ssh/id_ed25519`

---

## 29) `chown`
**Change owner/group.**  
**Syntax:** `chown [options] user:group path`  
**Common:** `-R` recursive  
**Example:** `sudo chown -R ubuntu:www-data /var/www`

---

## 30) `useradd` / `passwd` / `userdel`
**Manage users.**  
**Syntax:** `useradd [opts] user` / `passwd user` / `userdel [opts] user`  
**Examples:**  
- `sudo useradd -m -s /bin/bash deploy`  
- `sudo passwd deploy`  
- `sudo userdel -r deploy`

**Pro tip:** `adduser newname` (interactive)

---

## 31) `df`
**Filesystem disk usage.**  
**Syntax:** `df [options] [fs]`  
**Common:** `-h` human, `-T` type  
**Example:** `df -hT`

---

## 32) `du`
**Directory/file sizes.**  
**Syntax:** `du [options] [path]`  
**Common:** `-sh` summary, `-d N` depth  
**Example:** `du -sh /var/www/*`

---

## 33) `top`
**Live processes & load.**  
**Syntax:** `top [options]`  
**Common:** `-p PID` filter, interactive keys (e.g., `M` sort mem)  
**Example:** `top`

---

## 34) `htop`
**Improved interactive process viewer.**  
**Syntax:** `htop [options]`  
**Common:** `-C` monochrome, `--tree`  
**Example:** `htop`

---

## 35) `ps`
**Process snapshot.**  
**Syntax:** `ps [options]`  
**Common:** `aux` or `-ef`  
**Example:** `ps aux | grep nginx`

---

## 36) `uname`
**Kernel/system info.**  
**Syntax:** `uname [options]`  
**Common:** `-a` all  
**Example:** `uname -a`

---

## 37) `hostname`
**Show/set hostname & info.**  
**Syntax:** `hostname [options]`  
**Common:** `-i` IP, `-A` FQDN (if configured)  
**Example:** `hostname -A`

---

## 38) `time`
**Measure command runtime.**  
**Syntax:** `time command`  
**Example:** `time tar -czf backup.tgz /var/www`

---

## 39) `systemctl`
**Manage systemd services/units.**  
**Syntax:** `systemctl subcommand [unit] [options]`  
**Common:** `start|stop|restart|status|enable|disable`  
**Example:** `sudo systemctl enable --now nginx`

---

## 40) `watch`
**Run a command repeatedly.**  
**Syntax:** `watch [options] command`  
**Common:** `-n SEC` interval, `-d` highlight diffs  
**Example:** `watch -n 2 'df -h'`

---

## 41) `jobs`
**List background/foreground jobs in current shell.**  
**Syntax:** `jobs [options] [JOBID]`  
**Common:** `-l` PIDs, `-n` changed since last notice  
**Example:** `jobs -l`

---

## 42) `kill`
**Send signal to PID.**  
**Syntax:** `kill [signal] PID...`  
**Common:** `-TERM` graceful (default), `-9` force  
**Example:** `kill -TERM 12345`

---

## 43) `shutdown`
**Shutdown or reboot at a time.**  
**Syntax:** `shutdown [options] [time] [message]`  
**Common:** `-r` reboot, `now`, `+5` minutes  
**Example:** `sudo shutdown -r +2 "Rebooting for updates"`

---

## 44) `ping`
**ICMP echo for reachability/latency.**  
**Syntax:** `ping [options] host`  
**Common:** `-c COUNT`, `-i SEC`, `-W TIMEOUT`  
**Example:** `ping -c 10 -i 0.5 8.8.8.8`

---

## 45) `wget`
**Download over HTTP/HTTPS/FTP.**  
**Syntax:** `wget [options] URL`  
**Common:** `-O FILE`, `-q` quiet  
**Example:** `wget -O latest.zip https://wordpress.org/latest.zip`

---

## 46) `curl`
**HTTP client & APIs.**  
**Syntax:** `curl [options] URL`  
**Common:** `-I` headers, `-L` follow, `-X` method, `-H` header, `-d` data, `-o` file  
**Examples:**  
- `curl -I https://example.com`  
- `curl -X POST -H "Content-Type: application/json" -d '{"ok":1}' https://api.example.com/endpoint`

---

## 47) `scp`
**Copy files over SSH.**  
**Syntax:** `scp [options] src dest`  
**Common:** `-i KEY`, `-P PORT`, `-r` recursive, `-C` compress  
**Example:** `scp -i key.pem -r dist/ ubuntu@host:/var/www/`

---

## 48) `rsync`
**Sync files/folders (local/remote).**  
**Syntax:** `rsync [options] SRC DST`  
**Common:** `-a` archive, `-v` verbose, `-z` compress, `--delete`  
**Example:** `rsync -avz --delete ./dist/ ubuntu@host:/var/www/`

---

## 49) `ip`
**Network addresses/routes/links.**  
**Syntax:** `ip [object] [command]`  
**Common:** `ip a` (addr), `ip r` (route), `ip l` (link)  
**Example:** `ip address show dev eth0`

---

## 50) `netstat` *(deprecated; use `ss`)*
**Network stats.**  
**Syntax:** `netstat [options]`  
**Common:** `-t` TCP, `-u` UDP, `-l` listening, `-p` PID, `-n` numeric  
**Example:** `netstat -tulnp`

---

## 51) `traceroute`
**Trace packet path/hops.**  
**Syntax:** `traceroute [options] dest`  
**Common:** `-m` max hops, `-n` no DNS, `-w` timeout  
**Example:** `traceroute -n -w 2 8.8.8.8`

---

## 52) `nslookup`
**Query DNS (simple).**  
**Syntax:** `nslookup [options] name [server]`  
**Common:** `-type=MX|TXT|A|AAAA|NS`  
**Example:** `nslookup -type=TXT example.com 1.1.1.1`

---

## 53) `dig`
**Query DNS (detailed).**  
**Syntax:** `dig [@server] name [type] [options]`  
**Examples:**  
- `dig A example.com +short`  
- `dig MX example.com`  
- `dig -x 8.8.8.8 +short`

---

## 54) `history`
**Show command history.**  
**Syntax:** `history [options]`  
**Common:** `!N` rerun Nth cmd, `-c` clear  
**Examples:**  
- `history | grep ssh`  
- `!145`

---

## 55) `man`
**Manual pages.**  
**Syntax:** `man [section] command`  
**Example:** `man 5 crontab`

---

## 56) `echo`
**Print text/vars; redirect to files.**  
**Syntax:** `echo [options] text`  
**Examples:**  
- `echo $HOME`  
- `echo "APP_ENV=prod" | sudo tee -a /etc/environment`

---

## 57) `ln`
**Create links.**  
**Syntax:** `ln [options] target linkname`  
**Common:** `-s` symlink  
**Examples:**  
- `ln -s /etc/nginx/sites-available/app /etc/nginx/sites-enabled/app`  
- `ln file.txt alias.txt`

---

## 58) `alias` / `unalias`
**Create/remove command shortcuts.**  
**Syntax:** `alias name='cmd'` / `unalias name`  
**Examples:**  
- `alias ll='ls -lah'`  
- `unalias ll`

---

## 59) `cal`
**Show calendar.**  
**Syntax:** `cal [options] [month] [year]`  
**Common:** `-3` prev/cur/next  
**Example:** `cal -3 10 2025`

---

## 60) `apt` / `dnf`
**Package management (`apt` on Ubuntu).**  
**Syntax:** `apt [options] subcommand`  
**Common:** `update` `upgrade` `install` `remove`  
**Examples:**  
- `sudo apt update && sudo apt upgrade -y`  
- `sudo apt install nginx -y`

---

## Extra DevOps Favourites

### `journalctl` (systemd logs)
**Syntax:** `journalctl [options]`  
**Examples:**  
- `sudo journalctl -u nginx -f` (follow a unit’s logs)  
- `sudo journalctl --since "2025-10-24 08:00"`

### `ss` (modern sockets)
**Syntax:** `ss [options]`  
**Common:** `-lntp` listening TCP with PIDs  
**Example:** `ss -lntp | grep 80`

### `uptime` / `free` / `df` quick triage
```bash
uptime && free -h && df -h

