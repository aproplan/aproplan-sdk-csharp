# APROPLAN API

You can use this C# project to interact with the APROPLAN API in order to create, modify or delete entities into the platform in a programmatic way.

# Usage

1. First, you need to a requestid to identify your app into APROPLAN. You can make the [request to get it](https://www.aproplan.com/fr-be/integrations)
2. When you have a requesterid and an account to connect to APROPLAN, you need to initialize an ApiRequest object in the following way:

```csharp
using (ApiRequest requester = new ApiRequest("[userlogin]", "[userpassword]", new Guid("[requesterid")))
{
}
```

3. You need to log you first before to make any other call through the APROPLAN API and to retrieve the user entity

```csharp
requester.Login()
```

4. Create/Update/Get/Delete

To make CRUD operaions on the entities, you need to use the following methods of ApiRequest:
* GetEntities
* GetEntitiesCount
* GetEntityById
* GetEntitiesByIds
* CreateEntities
* CreateEntity
* UpdateEntities
* UpdateEntity
* DeleteEntities
* DeleteEntity

5. Upload documents

To upload document, there is a specific service that you can instantiate to make this kind of operaion

```csharp
DocumentService documentService = new DocumentService(requester);
Document d = documentService.UploadNewDocument([filepath], [predefinedDocumentId]).GetAwaiter().GetResult();
```

# Documentation

The API documentation can be found [here](https://github.com/aproplan/aproplan-api-doc)