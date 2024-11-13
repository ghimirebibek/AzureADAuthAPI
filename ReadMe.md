This Project is a basic demo setup to get you started with Authentication using Azure AD.

The environment variables are stored in a .env file in the root of the project. You will need to create a .env file and add the following variables:

```
	AZURE_AD_INSTANCE=https://login.microsoftonline.com/
	AZURE_AD_DOMAIN=domain_name
	AZURE_AD_TENANT_ID=tentant_id
	AZURE_AD_CLIENT_ID=client_id
	AZURE_AD_CLIENT_SECRET=client_secret
	AZURE_AD_CALLBACK_URL=http://localhost:3000/auth/openid/return
```

The API for Login:
```
	URL: {url}/api/auth/login
	Method: POST
```

Swagger is also setup for the project. You can access the swagger documentation at:
```
	https://localhost:7200/swagger/index.html
```