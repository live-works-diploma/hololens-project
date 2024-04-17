# **HoloLens Project Overview**

The HoloLens Project focuses on retrieving sensor data from a Raspberry Pi and efficiently transmitting it to Azure services for processing and storage. This continuous data flow is managed through Azure Functions or Azure IoT Hub, which then interfaces with an Azure database. The project is designed to operate with a predefined time delay, currently under consideration but likely set to transmit data every 24 hours.

On unity side of things the project routinely pulls down data from the database in unity, using azure functions, and then display that data to the user. How it displays the data is under consideration.

## _Folder Structure_

- [**Hololens**](#unity): Contains the Unity project tailored for deployment on the HoloLens device.
- [**Functions**](#functions): Hosts the Azure Functions deployed within an Azure Function App.
- [**RaspberryPiSimulator**](#raspberry-pi-simulator): Provides a simulated environment for generating and transmitting test data to Azure, mimicking the behavior of the Raspberry Pi.
- [**TestSendingData**](#test-sending-data): Implements testing scenarios for HTTP functions on Azure, covering CRUD operations for data tables and their attributes (e.g., creating new columns, tables). Currently, only creation (C) functionality is operational.
- [**uml diagram**](#uml-diagram): Illustrates the interaction between classes and components within the project, offering insights into the system's overall architecture and data flow.

# **Unity**

Unity plays a pivotal role in the project by managing the incoming data, converting it into class which implements the interface [IDataHandler](#idatahandler). 

After converting the data into these data classes, Unity passes them to the listeners. These listeners then process the data, converting it into user-friendly information that can be easily understood and interacted with by the user.

The way the retrieval of data works is you create a class which does what you want and you find a reference to an [IDRInteractor](#idrinteractor) class and you use the AddListener method. You add in a the [IDataHandler](#idatahandler) class you wish to listen for and the method you wish to be called when there is data found.

## _DataRetrieval_

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

## _DR_Usage_


## _Classes_

### Sensor

### Plant

### TelementryData

### Interactor_AzureDB

### Interactor_Dummy

### Interactor_Network

### DR_AzureDB

### DR_Dummy

### DR_Network

### DRInteractor

# **Functions**

# **Raspberry Pi Simulator**

# **Test Sending Data**

# **uml diagram**

[Uml Diagram](/uml%20diagram/UmlV1.jpg)
