# Keycloak .NET Core Project

## Description

[Watch Keycloak tutorial here](https://www.youtube.com/watch?v=UxAiWpkyFOI)

---

<details>
<summary><strong>Features</strong></summary>  
- Running Keycloak locally
- Configuring the .NET API in Keycloak 
- Configuring Postman in Keycloak  
- The Authorization Code Flow 
- Creating a user in Keycloak
- Getting access tokens from Keycloak
- Protecting the .NET API
</details>

---

## Installation

**Clone the repository**

```powershell
git clone https://github.com/victornovik/MinimalAPI-GameStore.git
```

**Install Keycloak**

- [Get started with Keycloak on Docker](https://www.keycloak.org/getting-started/getting-started-docker)

```powershell
docker compose up -d
OR
docker run -p 127.0.0.1:8080:8080 -e KC_BOOTSTRAP_ADMIN_USERNAME=admin -e KC_BOOTSTRAP_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:26.3.5 start-dev
```

**Configure Keycloak**

- [Open Admin Console](http://localhost:8080)
- [Create GameStore realm](http://localhost:8080/admin/master/console/#/master/realms)
- [Register our GameStore API as a Client](http://localhost:8080/admin/master/console/#/gamestore/clients/add-client)
- [Create a Client scope](http://localhost:8080/admin/master/console/#/gamestore/client-scopes)
	- [Configure `Audience` mapper to send necessary claims in JWT](http://localhost:8080/admin/master/console/#/gamestore/client-scopes/9a0f994d-a4fe-41d5-9852-c355ac0174ca/mappers)
-

## Usage

To run

```powershell
dotnet run
```