FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim

# Install Mono
RUN apt update
RUN apt install apt-transport-https dirmngr gnupg ca-certificates -y
RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
RUN echo "deb https://download.mono-project.com/repo/debian stable-buster main" | tee /etc/apt/sources.list.d/mono-official-stable.list
RUN apt update
RUN apt install mono-complete -y
RUN mono --version
