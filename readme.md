[![Animated Microsoft Teams presence Led](https://github.com/xs4free/MicrosoftTeamsPresenceLed/blob/master/img/animated-microsoft-teams-presence-led.gif)](https://github.com/xs4free/MicrosoftTeamsPresenceLed)

# Microsoft Teams presence Led
> DoItYourself led that follows your Microsoft Teams status

[![License: LGPL v3](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](https://www.gnu.org/licenses/lgpl-3.0)

## 1. Making the led
### 1.1 Required hardware
### 1.2 Flashing the ESP32
### 1.3 Connecting the hardware
### 1.4 3D printing the case
### 1.5 Assembling all parts

## 2 Using the Microsoft Teams Presence Publisher
### 2.1 Installing the software
### 2.2 Configuring the software
### 2.3 Using your own Application Registration ID
The Microsoft Teams Presence Publisher needs an Azure Application Registration ID to identify itself with, when requesting the presence information on behalve of a user. In the source-code there is a hardcoded registration number that I created on my personal domain. If you want to setup you own application registration, follow [this link](https://go.microsoft.com/fwlink/?linkid=2083908) and configure every step to match the following screenshots:

![Azure App Registration - API Permissions](https://github.com/xs4free/MicrosoftTeamsPresenceLed/blob/master/img/AppRegistration-API-Permissions.png)

![Azure App Registration - Authentication](https://github.com/xs4free/MicrosoftTeamsPresenceLed/blob/master/img/AppRegistration-Authentication.png)

The branding can be configured as your own, as long as you add a verified publisher domain:
![Azure App Registration - Branding](https://github.com/xs4free/MicrosoftTeamsPresenceLed/blob/master/img/AppRegistration-Branding.png)

If you have your own domain hosted in GitHub Pages and are having trouble adding the `.well-known` directory, I've found a work-around using CloudFlare. I'll write a blogpost about this soon and link it here.

## Credits
- This source is my rewrite of [PresenceLight](https://github.com/isaacrlevin/PresenceLight) by [Isaac Levin](https://github.com/isaacrlevin)
- [Greasemann](https://commons.wikimedia.org/wiki/File:Portrait_Placeholder.png) for the unknown user icon (under the [Creative Commons Attribution-Share Alike 4.0 International license](https://creativecommons.org/licenses/by-sa/4.0/deed.en))
