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
