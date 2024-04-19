# **HoloLens Project Overview**

The HoloLens Project focuses on retrieving sensor data from a Raspberry Pi and efficiently transmitting it to Azure services for processing and storage. This continuous data flow is managed through Azure Functions or Azure IoT Hub, which then interfaces with an Azure database. The project is designed to operate with a predefined time delay, currently under consideration but likely set to transmit data every 24 hours.

On unity side of things the project routinely pulls down data from the database in unity, using azure functions, and then display that data to the user. How it displays the data is under consideration.

## _Aspects of the Project_

- [**Hololens**](#unity): Contains the Unity project tailored for deployment on the HoloLens device.
- [**Functions**](#azure-functions): Hosts the Azure Functions deployed within an Azure Function App.
- [**RaspberryPiSimulator**](#raspberry-pi-simulator): Provides a simulated environment for generating and transmitting test data to Azure, mimicking the behavior of the Raspberry Pi.
- [**TestSendingData**](#test-sending-data): Implements testing scenarios for HTTP functions on Azure, covering CRUD operations for data tables and their attributes (e.g., creating new columns, tables). Currently, only creation (C) functionality is operational.
- [**uml diagram**](#uml-diagram): Illustrates the interaction between classes and components within the project, offering insights into the system's overall architecture and data flow.
- [**Raspberry Pi**](#raspberry-pi): This device gathers data from the sensors and sends it to Azure, although its code is not contained in this repository.

# **Unity**

This project is designed to interact with azure. It retrieves data, which is converted to information for the user, and it sends commands to an iot device so you can control what is happening remotely. The information this project provides to the user is a constructed scene which resembled the hydroponic area and then sensor canvases which appear above each sensor location, showing their data.

How it works is it retrieves and builds the data in a class which implements the [IDataRetrieval](#idataretrieval) interface. This data is then passed to a class which implements the [IDRHandler](#idrhandler) interface, which then passes that data back to a class which implements the [IDRInteractor](#idrinteractor) interface. Once it has reached this stage it passes that data into each of the classes that are listening for data. These classes are prefixed with "DRU", which then convert the data into something the user can see and understand. There is no consistency with what these classes do with the information and it is up to them how much of the information to view.

Different part of project:
- [**Flow of Data**](#flow-of-data)
- [**Interfaces**](#interfaces)
- [**Classes**](#classes)


## _Flow of Data_

- [Retrieval of Data Classes (IDataRetrieval)](#idataretrieval) 
  -> [Classes that Control the Loop of Calls (IDRHandler)](#idrhandler) 
  -> [Classes that Act as Choke Points (IDRInteractor)](#idrinteractor) 
  -> Frontend

Versions of front end include:

- Frontend 
  -> [DRU_Sensors](#dru_sensors) 
  -> Prefab 
  -> [SensorControl](#sensorcontrol) 
  -> Prefab
- Frontend 
  -> [DRU_T](#dru_t)

## _Interfaces_

### IDataHandler

These classes serve as the foundation for managing data retrieved from Azure databases. They offer essential methods for constructing classes from data, converting instances into dictionaries, and generating dummy data. Naming conventions align these classes directly with the tables they represent in Azure databases, ensuring clarity and consistency.

Examples:
- [**Sensor**](#sensor)
- [**Plant**](#plant)
- [**TelementryData**](#telementrydata)

### IDRInteractor

Prefixed with "Interactor," these classes play a pivotal role as centralized points for other Unity classes to listen for data. By consolidating the listening logic, they eliminate the need for individual classes to set up their listeners, minimizing redundant calls. IDRInteractor classes create their instances of IDataRetrieval and IDRHandler, promoting a modular design that allows for seamless interchangeability without necessitating alterations in other classes.

Examples:
- [**Interactor_AzureDB**](#interactor_azuredb)
- [**Interactor_Dummy**](#interactor_dummy)
- [**Interactor_Network**](#interactor_network)

### IDataRetrieval

Classes prefixed with "DR" focus on retrieving and structuring data into the desired format, typically a list of instances. They invoke delegates to which listeners have attached their methods, passing the retrieved data as an argument. IDataRetrieval classes do not loop; instead, they provide methods for data retrieval. They also handle the same listener functionality as IDRInteractor, ensuring efficient and targeted data retrieval, such as in Azure where specific data types are retrieved based on listener type names.

Examples:
- [**DR_AzureDB**](#dr_azuredb)
- [**DR_Dummy**](#dr_dummy)
- [**DR_Network**](#dr_network)

### IDRHandler

Designed to work with an IDataRetrieval instance, IDRHandler classes loop at defined intervals. They offer anchors that other classes can use to pause the loop. When these anchors reach or fall below 0, the routine restarts, introducing a delay as needed.

Examples:
- [**DRInteractor**](#drinteractor)

### IJsonHandler

Forces the class to implement a way of retrieving a json string. It allows the json string to be built in the method itself instead of retrieving it.

Examples:
- [**DR_AzureDB**](#dr_azuredb)
- [**DR_Dummy**](#dr_dummy)
- [**DR_Network**](#dr_network)

## _Classes_

### Sensor

This class encapsulates data related to sensor information.

### Plant

This class encapsulates data related to plant information.

### TelemetryData

This class holds telemetry-related data.

### Interactor_AzureDB

This class creates an instance of [DR_AzureDB](#dr_azuredb) and an instance of [DRInteractor](#drinteractor). It then feeds the instance of DR_AzureDB into the instance of DRInteractor. This class utilizes [AzureFunctionAccess](#azurefunctionaccess) to retrieve the necessary keys for accessing Azure functions. This design allows for easy switching between functions without the need to rewrite keys, which is particularly useful when dealing with multiple functions or accounts that perform the same task but require different keys.

Interfaces:
- [**IDRInteractor**](#idrinteractor)
- [**IDataHandler**](#idatahandler)

### Interactor_Dummy

This class creates an instance of [DR_Dummy](#dr_dummy) and an instance of [DRInteractor](#drinteractor). It then feeds the instance of DR_Dummy into the instance of DRInteractor.

Interfaces:
- [**IDRInteractor**](#idrinteractor)
- [**IDataHandler**](#idatahandler)

### Interactor_Network

This class creates an instance of [DR_Network](#dr_network) and an instance of [DRInteractor](#drinteractor). It then feeds the instance of DR_Network into the instance of DRInteractor.

Interfaces:
- [**IDRInteractor**](#idrinteractor)
- [**IDataHandler**](#idatahandler)

### DR_AzureDB

This sends a post request, using [AzureFunctionRequestHandler](#azurefunctionrequesthandler), that retrieves a json string of the needeed data and then converted into the needed structure using [JsonBuildTask](#jsonbuildtask). This data is then passed into a class which implements the [IDRHandler](#idrhandler) interface.

Interfaces:
- [**IDataRetrieval**](#idataretrieval)
- [**IJsonHandler**](#ijsonhandler)

### DR_Dummy

This class creates dummy data, converts it into a JSON string, rebuilds it using [JsonBuildTask](#jsonbuildtask), and then passes it into a class that implements the [IDRHandler](#idrhandler) interface.

Interfaces:
- [**IDataRetrieval**](#idataretrieval)
- [**IJsonHandler**](#ijsonhandler)

### DR_Network

This does nothing currently

Interaces:
- [**IDataRetrieval**](#idataretrieval)
- [**IJsonHandler**](#ijsonhandler)

### DRInteractor

This component serves as a mechanism to iterate over the calls provided by classes implementing [IDataRetrieval](#idataretrieval). It offers methods to initiate the data retrieval process, along with 'anchors'. These anchors allow the pausing of the iteration when a listener's method is invoked, ensuring that no further calls are made until the current operation completes. Additionally, this component introduces a configurable delay between calls to limit the frequency of requests. The current delay is set to 500ms, though in practice it tends to be closer to 550ms to allow listeners sufficient time to process each call.

Interfaces:
- [**IDRHandler**](#idrhandler)

### JsonBuildTask

This class supplies methods for converting JSON strings into the desired data structure.

### Interactor_AzureCommand

This class supplies methods for sending commands to an Azure function.

### AzureBlobAccess

This class currently has no functionality.

### AzureFunctionAccess

This class is a scriptable object that contains the necessary data for interacting with an Azure function.

### NetworkData

This class is a scriptable object that contains the necessary data for sending signals using websockets.

### ExampleUsage

This class is an example of a "DRU" listener. It prints the count of instances.

### DS_AzureCommands

This class acts as a central point of control for other classes in Unity. It provides methods for sending commands, abstracting away the implementation details from other classes. This design allows the functionality to be easily accessed by a canvas button or other components without requiring them to handle the specifics of the command sending process.

### AzureFunctionRequestHandler

This class supplies methods for sending POST and GET requests to Azure functions.

### SensorControl

Supplies methods for adding / altering the data on a canvas. When creating fields on the sensor it creates a new prefab which is parented to the sensor canvas. It then adds each field that is created to a dictionary. Before it adds new fields it checks if that field already exists and if it does then it just alters value on that field instead.

### DRU_Sensors

Acts as a listener. When data is found it is passed back to this class which then passes that data into [SensorControl](#sensorcontrol) which then populates the sensor canvases which display the data to the user.

### DRU_T

Not finished yet

# **Azure Functions**

These are functions created and hosted on azure. They allow the offloading of resources and make it so each project only needs to send HTTP requests to the functions to be able to do what they want saving the repeating of code. For unity it also allows a way to bipass difficulties such as accessing a database since it doesn't directly need to access it since the function does for it.There is also a function which can be used as an end point for an iot device, which isn't currently being used. This function always the iot device to send data to an azure database.

Different parts of azure functions:

- [**Types of functions Used**](#types-of-functions-used)
- [**Functions**](#functions)
- [**Models**](#models)

## _Types of Functions Used_

- HTTP
- Event Grid Event

## _Functions_

### EGEDBItemCreate

- **Event Type:** Event Grid Event
- **Function Name:** EGEDBItemCreate
- **Return Type:** IActionResult
- **Description:** This function serves as an endpoint for an IoT device. It extracts the required data from the incoming request and forwards it to [ModelDBItemCreate](#modeldbitemcreate). Uses [ModelDBAccountInfo](#modeldbaccountinfo) to build the connection string to pass into the models.

### HttpDBItemCreate

- **Type:** POST request
- **Event Type:** HttpTrigger
- **Authorisation Level:** function
- **Function Name:** HttpDBItemCreate
- **Return Type:** HttpResponseData
- **Description:** Extracts the required data from the request and forwards it to [ModelDBItemCreate](#modeldbitemcreate). Uses [ModelDBAccountInfo](#modeldbaccountinfo) to build the connection string to pass into the models.

### HttpDBItemRead

- **Type:** GET request
- **Event Type:** HttpTrigger
- **Authorisation Level:** function
- **Function Name:** HttpDBItemRead
- **Return Type:** HttpResponseData
- **Description:** Finds the needed data in the request and retrieves that data from [ModelDBItemRead](#modeldbitemread). Used as the entry point for reading the content of a table in a database. Uses [ModelDBAccountInfo](#modeldbaccountinfo) to build the connection string to pass into the models.

### HttpDBItemUpdate

- **Type:** POST request
- **Event Type:** HttpTrigger
- **Authorisation Level:** function
- **Function Name:** HttpDBItemUpdate
- **Return Type:** HttpResponseData
- **Description:** Finds the needed data in the request and sends that data to [ModelDBItemUpdate](#modeldbitemupdate). Used as the entry point for updating rows on a table in a database. Uses [ModelDBAccountInfo](#modeldbaccountinfo) to build the connection string to pass into the models.

### HttpDBItemDelete

- **Type:** POST request
- **Event Type:** HttpTrigger
- **Authorisation Level:** function
- **Function Name:** HttpDBItemDelete
- **Return Type:** HttpResponseData
- **Description:** Finds the needed data in the request and sends that data to [ModelDBItemDelete](#modeldbitemdelete). Used as the entry point for deleting rows on a table in a database. Uses [ModelDBAccountInfo](#modeldbaccountinfo) to build the connection string to pass into the models.

### HttpDBTableCreate

- **Type:** POST request
- **Event Type:** HttpTrigger
- **Authorisation Level:** Admin
- **Function Name:** HttpDBTableCreate
- **Return Type:** HttpResponseData
- **Description:** Finds the needed data in the request and sends that data to [ModelDBTableCreate](#modeldbtablecreate). Used as the entry point for creating new tables in a database. Uses [ModelDBAccountInfo](#modeldbaccountinfo) to build the connection string to pass into the models.

### HttpDBTableRead

- **Type:** GET request
- **Event Type:** HttpTrigger
- **Authorisation Level:** Admin
- **Function Name:** HttpDBTableRead
- **Return Type:** HttpResponseData
- **Description:** Not completed

### HttpDBTableUpdate

- **Type:** POST request
- **Event Type:** HttpTrigger
- **Authorisation Level:** Admin
- **Function Name:** HttpDBTableUpdate
- **Return Type:** HttpResponseData
- **Description:** Not completed

### HttpDBTableDelete

- **Type:** POST request
- **Event Type:** HttpTrigger
- **Authorisation Level:** Admin
- **Function Name:** HttpDBTableDelete
- **Return Type:** HttpResponseData
- **Description:** Not completed

### HttpIOTHubCommand

- **Type:** POST request
- **Event Type:** HttpTrigger
- **Authorisation Level:** function
- **Function Name:** HttpIOTHubCommand
- **Return Type:** HttpResponseData
- **Description:** Used to send commands to an iot hub device. It uses [ModelIOTInfo](#modeliotinfo) to get the details which connects to the iot device.

## Models

### ModelDBItemCreate

Provides the methods for adding new rows to a table. It uses [ModelDBConnect](#modeldbconnect) to connect to the database but it tells it what to do after it connects.

### ModelDBItemRead

provides the methods for reading rows on a table. It uses [ModelDBConnect](#modeldbconnect) to connect to the database but it tells it what to do after it connects.

### ModelDBItemUpdate

provides the methods for updating rows on a table. It uses [ModelDBConnect](#modeldbconnect) to connect to the database but it tells it what to do after it connects.

### ModelDBItemDelete

provides the methods for deleting rows from a table. It uses [ModelDBConnect](#modeldbconnect) to connect to the database but it tells it what to do after it connects.

### ModelDBTableCreate

provides methods for creating new tables in a database. It uses [ModelDBConnect](#modeldbconnect) to connect to the database but it tells it what to do after it connects.

### ModelDBAccountInfo

Provides the needed details for building an SqlConnectionStringBuilder and has methods for retrieving that builder with the details already added.

### ModelDBConnect

Provides the needed methods for connecting and disconnecting from a database.

### ModelIOTInfo

Provides the needed details for connecting to an iot hub device.

# **Raspberry Pi Simulator**

This is used to simulate the behaviour of a raspberry pi. It isn't mainly being used but was an alternative to the azure raspberry pi simulator online.

# **Test Sending Data**

This is a way for testing the functions to make sure they are working as intended. This project connects to virtually all the functions in [AzureFunctions](#azure-functions). This currently contains the only code that has access to the admin functions, which is needed for creating new tables on a database outside of azure itself. 

## _Functions Connects to_

### Function Level
- [**HttpDBItemCreate**](#httpdbitemcreate)
- [**HttpDBItemRead**](#httpdbitemread)
- [**HttpDBItemUpdate**](#httpdbitemupdate)
- [**HttpDBItemDelete**](#httpdbitemdelete)

### Admin Level
- [**HttpDBTableCreate**](#httpdbtablecreate)

## _Files_

- **main:** connects all the code together.
- **azure_database:** sends and recieves all requests from azure.
- **default_data:** generates random data
- **json:** converts data to json

# **Raspberry Pi**

This is the device that gathers the data from sensors which is sent to the iothub on azure. This is not finished.

## _Hardware_

- **Raspberry Pi**
- [**Gravity: Digital Peristaltic Pump**](https://www.dfrobot.com/product-1698.html)

# **uml diagram**

![Uml Diagram](/uml%20diagram/UmlV1.jpg)
