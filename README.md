# Aproplan API

You can use this C# project to interact with the APROPLAN API in order to create, modify or delete entities into the platform in a programmatic way.

# Usage

## Initialization

* First, you need to a requestid to identify your app into APROPLAN. You can make the [request to get it](https://www.aproplan.com/fr-be/integrations)
* When you have a requesterid and an account to connect to APROPLAN, you need to initialize an ApiRequest object in the following way:

```cs
using (ApiRequest requester = new ApiRequest("[userlogin]", "[userpassword]", new Guid("[requesterid")))
{
}
```

## Login

* You need to log you first before to make any other call through the APROPLAN API and to retrieve the user entity

```cs
requester.Login()
```

## CRUD

To make CRUD operations on the entities, you need to use the following methods of ApiRequest:
* Read
	* GetEntities
	* GetEntitiesCount
	* GetEntityById
	* GetEntitiesByIds
* Creation
	* CreateEntities
	* CreateEntity
* Update
	* UpdateEntities
	* UpdateEntity
* Delete
	* DeleteEntities
	* DeleteEntity

5. Upload documents

To upload document, there is a specific service that you can instantiate to make this kind of operaion

```cs
DocumentService documentService = new DocumentService(requester);
Document d = documentService.UploadNewDocument([filepath], [predefinedDocumentId]).GetAwaiter().GetResult();
```

It is possible to:
* Add version
* To join source file to an existing document

# Documentation

The API documentation can be found [here](https://github.com/aproplan/aproplan-api-doc)