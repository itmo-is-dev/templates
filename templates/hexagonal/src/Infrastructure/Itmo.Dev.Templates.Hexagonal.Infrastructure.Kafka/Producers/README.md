Folder for wrappers over `IKafkaProducer<,>`. Place wrapper interfaces into `*.Application.Abstraction` project and
implement them here.

Register producers in `ServiceCollectionExtensions` by calling `AddProducer` method, register wrapper implementation by
calling `ConfigureServices` method.

---