# Aproplan API

You can use this C# project to interact with the APROPLAN API in order to create, modify or delete entities into the platform in a programmatic way.

- [Installation](#installation)
- [Usage](#usage)
- [Api documentation](#documentation)

# Installation

## Add a dependency

You need to add a dependency in your solution, you can make in several ways:

* Run the nuget command for installing the client as `Install-Package Aproplan.Api`
* Use the NuGet manager and search for Aproplan to install `Aproplan.Api`

# Usage

### Initialization

* First, you need to a requestid to identify your app into APROPLAN. You can make the [request to get it](https://www.aproplan.com/fr-be/integrations)
* When you have a requesterid and an account to connect to APROPLAN, you need to initialize an ApiRequest object in the following way:

```cs
using (ApiRequest requester = new ApiRequest("[userlogin]", "[userpassword]", new Guid("[requesterid")))
{
}
```

### Login

* You need to log you first before to make any other call through the APROPLAN API and to retrieve the user entity

```cs
requester.Login()
```

### CRUD

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

### Sync methods

When you need to update your data often in an offline way, you can use sync method that will returns you the data from a specific timestamp. 
That returned contains nested data then, you need only to call some kind of method. There are methods to get data by batch or to get all data, the logic loop is in the method itself.:
To use those methods, you need to create an instance of Aproplan.Api.Http.Services.SyncService.

* **SyncNotes** To get all notes (points) and nested data like NoteComment, NoteDocument...
* **SyncForms** To get all forms and nested data like FormItem, FormSection...
* **SyncFormTemplates** To get all form templates and nested data like FormQuestion, FormSectionRule,...
* **SyncIssueTypes** To get all the IssueTypes (sub category) with theirs subjects
* **SyncChapters** To get all the chapters (Category)
* **SyncSubCells** To get all subcells (room level 2)
* **SyncParentCells** To get all parent cell (room level 1)
* **SyncFolders** To get all folders and its hierarchy
* **SyncProjects** To get all projects
* **SyncContactDetails** To get all contact details
* **SyncProjectStatus** To get all status configured by project
* **SyncUsers** To get all users that the user can see
* **SyncMeetings** To get all meetings (lists)
* **SyncAttachmentDocuments** To get all document that user can see through forms or points because they are attached to them
* **SyncFolderDocuments** To get all documents that user can see through the visibility of folders
* **SyncCompanyUsers** To get all members of your company
* **SyncUsers** To get all users with who you worked on all projects you are invited
* **SyncAccessRights** To get level of access right that exists in APROPLAN. Then, you can already make some access right check in the client App.
* **SyncNoteBaseVisibilityLostSync** To get all users with who you worked on all projects you are invited

All of those methods exists with the postfix "All", in that case, the api is call after each batch received to be synchronized until now.

```cs

SyncService syncService = new SyncService(requester);
SyncResult<Project> result = syncService.SyncProjects(null).GetAwaiter().GetResult();
```

SyncResult has 2 properties: 
* **Data**: contains the list of entities retrieved in the batch
* **ContinuationToken**: the token, timestamp to send back to the api to retrieve the next batch. If null, you are synchronized

### Upload documents

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