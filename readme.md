# Object instantiation comparison

## Introduction

Popular serialization and test utility libraries create objects using different strategies. It is quite frequent mistake
when developer assumes that i.e. object will be created in test unit in the same way as in production code by 
serialization library used for sending data from application or store it in a database.

Below is a comparison of behaviors shown by following libraries:
* [AutoFixture](https://github.com/AutoFixture/AutoFixture) 4.17.0
* [MongoDB.Bson](https://mongodb.github.io/mongo-csharp-driver/2.12/) 2.12.2
* [Newtonsoft.Json](https://www.newtonsoft.com/json/help/html/Introduction.htm) 13.0.1
* [SpecFlow](https://docs.specflow.org/projects/specflow/en/latest/) 3.7.38 

Test application is build using .NET 5.

## Results

|Behavior                       |Netwosoft.Json                   |MongoDB.Bson |SpecFlow Table.CreateInstance<>()|AutoFixture Fixture.Create<>()   |
|-------------------------------|---------------------------------|-------------|---------------------------------|---------------------------------|
|Default constructor called     |Yes                              |Yes          |Yes                              |Yes                              |
|Parametrized constructor called|Yes                              |No           |Yes                              |Yes                              |
|Constructor preference         |Default                          |Default      |Default                          |Default                          |
|Get-only property set          |Only via parametrized constructor|Exception[^3]|Exception                        |Only via parametrized constructor|
|Private setter property set    |Only via parametrized constructor|Yes          |Yes                              |Only via parametrized constructor|
|Init property set              |Yes                              |Yes          |Yes                              |Yes                              |
|Setter called twice[^1]        |No                               |No           |No                               |Yes                              |
|Instantiation customization    |Yes[^2]                          |Yes[^4]      |Yes[^5]                          |Yes[^6]                          |

[^1]: First time from parametrized constructor, second time directly
[^2]: Via CustomCreationConverter, controlled by ConstructorHandling setting or JsonConstructorAttribute
[^3]: Property can be ignored using SetIgnoreExtraElements() in class map
[^4]: Via MapCreator() in class map or BsonConstructorAttribute
[^5]: Via factory method passed to CreateInstance()
[^6]: Via custom ISpecimeBuilder, constructor can be choosen by using GreedyConstructorQuery [see here](https://blog.ploeh.dk/2011/04/19/ConstructorstrategiesforAutoFixture/)