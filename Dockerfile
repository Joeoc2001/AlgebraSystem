FROM mcr.microsoft.com/dotnet/sdk:5.0

# Install Mono
RUN apt install apt-transport-https dirmngr gnupg ca-certificates \
    && sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF \
    && echo "deb https://download.mono-project.com/repo/debian stable-buster main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list \
    && sudo apt update \
    && sudo apt install mono-complete \
	&& mono --version
